using Domain.Entities.Identities;
using Domain.Enums;
using Domain.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.DbContext
{
     public static class DbInitializer
    {
        public static async Task SeedRoleData(this IHost host)
        {
            var serviceProvider = host.Services.CreateScope().ServiceProvider;
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            var rolesEnumList = EnumExtension.GetEnumResults<ERole>();
            if (rolesEnumList.Any())
            {
                foreach (var item in rolesEnumList)
                {
                    var roleRecord = context.Roles.Where(x => x.Name.Equals(item.Name));
                    if (roleRecord.FirstOrDefault()?.Name == null)
                    {
                        Role role = new()
                        {
                            ConcurrencyStamp = Guid.NewGuid().ToString(),
                            Name = item.Name,
                        };
                        await roleManager.CreateAsync(role);
                    }
                }
            }
        }

        public static async Task Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            await context.Database.EnsureCreatedAsync();

            var query = context.Set<User>().AsQueryable();
            var email = "admin@admin.com";

            var getUser = await query.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.UserName.Equals(email));

            if (getUser == null)
            {
                var newUser = new User
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                    PhoneNumberConfirmed = false,
                    LockoutEnabled = false,
                    Email = email,
                    UserName = email,
                    PhoneNumber = "07036HRMS000",
                };

                var result = await userManager.CreateAsync(newUser, "Admin123@");
                var systemAdminrole = ERole.Admin.ToString();
                if (!(await userManager.IsInRoleAsync(newUser, systemAdminrole)))
                {
                    await userManager.AddToRoleAsync(newUser, systemAdminrole);
                }
            }
        }

    }
}
