﻿using Microsoft.AspNetCore.Identity;
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
