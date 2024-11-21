using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PART2PROG.Data;
using PART2PROG.Models;
using System.Globalization;

namespace PART2PROG.Controllers
{
    [Authorize(Roles = "HR")]
    public class HRController : Controller
    {
        private readonly ApplicationDBContext _context;

        public HRController(ApplicationDBContext context)
        {
            _context = context;
        }

        // View all claims
        public async Task<IActionResult> Index()
        {
            var claims = await _context.Claims
                .Include(c => c.ApplicationUser)
                .Include(c => c.Doc)
                .ToListAsync();

            var viewModel = claims.Select(c => new HRViewModel
            {
                ClaimId = c.ClaimId,
                LecturerName = c.ApplicationUser?.UserName ?? "Unknown",
                HoursWorked = c.HoursWorked,
                HourlyRate = c.HourlyRate,
                TotalAmount = c.TotalAmount,
                Notes = c.Notes,
                DateSubmitted = c.DateSubmitted,
                Status = c.Status
            });

            return View(viewModel);
        }

        // Approve or reject a claim
        [HttpPost]
        public async Task<IActionResult> ApproveRejectClaim(int claimId, bool isApproved)
        {
            var claim = await _context.Claims.FindAsync(claimId);

            if (claim == null)
            {
                return NotFound();
            }

            // If HR has already approved or rejected, don't allow further changes
            if (claim.Status == "Approved by HR" || claim.Status == "Rejected by HR")
            {
                TempData["ErrorMessage"] = "This claim has already been finalized by HR.";
                return RedirectToAction(nameof(Index));
            }

            // HR approval/rejection logic
            claim.Status = isApproved ? "Approved by HR" : "Rejected by HR";
            _context.Claims.Update(claim);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Claim #{claimId} has been {(isApproved ? "approved" : "rejected")} by HR.";
            return RedirectToAction(nameof(Index));
        }

        // Generate and download an individual claim report as PDF
        public async Task<IActionResult> GenerateIndividualReport(int claimId)
        {
            var claim = await _context.Claims
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(c => c.ClaimId == claimId);

            if (claim == null)
            {
                return NotFound();
            }

            var reportBytes = GeneratePDFForClaim(claim);

            return File(reportBytes, "application/pdf", $"ClaimReport_{claimId}.pdf");
        }

        private byte[] GeneratePDFForClaim(Claim claim)
        {
            using (var ms = new MemoryStream())
            {
                var document = new Document();
                PdfWriter.GetInstance(document, ms);
                document.Open();

                var culture = new CultureInfo("en-ZA");

                document.Add(new Paragraph("----------------------------------------------------------------------------------------------------------"));
                document.Add(new Paragraph("------------------------------Lecturer Contract Monthly Claim Report-----------------------"));
                document.Add(new Paragraph("----------------------------------------------------------------------------------------------------------"));
                document.Add(new Paragraph($"Claim ID: {claim.ClaimId}"));
                document.Add(new Paragraph($"Lecturer: {claim.ApplicationUser?.UserName ?? "Unknown"}"));
                document.Add(new Paragraph($"Total Amount: {claim.TotalAmount.ToString("C", culture)}"));
                document.Add(new Paragraph($"Hours Worked: {claim.HoursWorked}"));
                document.Add(new Paragraph($"Hourly Rate: {claim.HourlyRate.ToString("C", culture)}"));
                document.Add(new Paragraph($"Coordinator Approval: {(claim.IsApprovedByCoordinator ? "Yes" : "No")}"));
                document.Add(new Paragraph($"Manager Approval: {(claim.IsApprovedByManager ? "Yes" : "No")}"));
                document.Add(new Paragraph($"Submitted On: {claim.DateSubmitted}"));
                document.Add(new Paragraph($"Status: {claim.Status}"));
                document.Add(new Paragraph("------------------------------------------------------------------------------------------------------------\n"));

                document.Close();
                return ms.ToArray();
            }
        }
    }
}
