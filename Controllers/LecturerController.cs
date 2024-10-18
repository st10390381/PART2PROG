using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PART2PROG.Data;

namespace PART2PROG.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDBContext _context; 
        private readonly UserManager<IdentityUser> _userManager;

        public LecturerController(ApplicationDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard(DateTime? startDate, DateTime? endDate)
        {
          
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

          
            var claimsQuery = _context.Claims
                .Include(c => c.Doc) 
                .Where(c => c.ApplicationUserId == userId); 

            if (startDate.HasValue && endDate.HasValue)
            {
                claimsQuery = claimsQuery.Where(c => c.DateSubmitted >= startDate.Value && c.DateSubmitted <= endDate.Value);
            }

            var claims = await claimsQuery.ToListAsync();
            return View(claims);
        }

    }
}

//Reference List:

//https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-8.0
//-Microsoft
//-Accessed 17 October 2024

//Reference list:
//https://www.w3schools.com/html/
//-w3schools
//- Accessed 15 October 2024

