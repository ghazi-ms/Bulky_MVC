using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bulky.DataAccess.Data;

namespace BulkyWeb.Areas.Identity
{
    public class CustomUserValidator : IUserValidator<IdentityUser>
    {
        private readonly ApplicationDbContext _context;

        public CustomUserValidator(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            var errors = new List<IdentityError>();

            // Check if PhoneNumber already exists
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber);

                if (existingUser != null && existingUser.Id != user.Id)
                {
                    errors.Add(new IdentityError
                    {
                        Description = $"Phone number {user.PhoneNumber} is already taken."
                    });
                }
            }

            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
