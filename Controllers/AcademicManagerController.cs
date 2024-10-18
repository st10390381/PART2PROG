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

        private readonly ApplicationDBContext _context;

        public AcademicManagerController(ApplicationDBContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var pendingClaims = _context.Claims
                .Include(c => c.ApplicationUser)
                .Include(c => c.Doc)
                .Where(c => c.IsApprovedByCoordinator && !c.IsApprovedByManager && c.Status == "Approved by Coordinator")
                .ToList();
            return View(pendingClaims);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);

            if (claim != null)
            {
                claim.IsApprovedByManager = true;
                claim.Status = "Approved by Manager";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim != null)
            {
                claim.Status = "Rejected by Manager";
                await _context.SaveChangesAsync();
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


