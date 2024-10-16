using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PART2PROG.Data;

namespace PART2PROG.Controllers
{
    [Authorize(Roles = "Programme Coordinator")]
    public class ProgrammeCoordinatorController : Controller
    {
           
            private readonly ApplicationDBContext _context;

            public ProgrammeCoordinatorController(ApplicationDBContext context)
            {
                _context = context;
            }
            public IActionResult Index()
            {
                var pendingClaims = _context.Claims
                    .Include(c => c.ApplicationUser) 
                    .Include(c => c.Doc) 
                    .Where(c => !c.IsApprovedByCoordinator && c.Status == "Pending") 
                    .ToList();

                return View(pendingClaims);
            }

            [HttpPost]
            public async Task<IActionResult> Approve(int claimId)
            {
                var claim = await _context.Claims.FindAsync(claimId);

                if (claim != null)
                {
                    claim.IsApprovedByCoordinator = true;
                    claim.Status = "Approved by Coordinator";
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
                    claim.Status = "Rejected by Coordinator";
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
        }
    }
        
