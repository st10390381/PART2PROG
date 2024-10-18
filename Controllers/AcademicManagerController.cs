using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PART2PROG.Data;

namespace PART2PROG.Controllers
{
    [Authorize(Roles = "Academic Manager")]
    public class AcademicManagerController : Controller
    {
        // Injecting the database context to interact with the database
        private readonly ApplicationDBContext _context;

        // Constructor to initialize the context
        public AcademicManagerController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Fetches claims approved by Coordinator but not yet by Manager
        public IActionResult Index()
        {
            var pendingClaims = _context.Claims
                .Include(c => c.ApplicationUser)  // Includes related user data
                .Include(c => c.Doc)              // Includes related document data
                .Where(c => c.IsApprovedByCoordinator && !c.IsApprovedByManager && c.Status == "Approved by Coordinator")
                .ToList();
            return View(pendingClaims);
        }

        // Action to approve a claim by the Manager
        [HttpPost]
        public async Task<IActionResult> Approve(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);

            if (claim != null)
            {
                claim.IsApprovedByManager = true;  // Mark claim as approved by Manager
                claim.Status = "Approved by Manager";  // Update claim status
                await _context.SaveChangesAsync();  // Save changes to the database
            }
            return RedirectToAction("Index");
        }

        // Action to reject a claim by the Manager
        [HttpPost]
        public async Task<IActionResult> Reject(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);

            if (claim != null)
            {
                claim.Status = "Rejected by Manager";  // Update claim status to rejected
                await _context.SaveChangesAsync();  // Save changes to the database
            }

            return RedirectToAction("Index");
        }
}
}

//https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-8.0
//-Microsoft
//-Accessed 17 October 2024

//Reference list:
//https://www.w3schools.com/html/
//-w3schools
//- Accessed 15 October 2024


