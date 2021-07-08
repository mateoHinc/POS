using Microsoft.EntityFrameworkCore;
using POS.Api.Data.Entities;
using POS.Api.Enum;
using POS.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Api.Data
{
    public class SeeDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeeDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckUserAsync("pasha", "pekuns", "pasha@yopmail.com", "310 322 56 66");
            await CheckUserAsync("toby", "seagar", "toby@yopmail.com", "310 322 56 66");
            await CheckCustomersAsync();
            await CheckProductsAsync();

        }

        private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, UserType.Admin.ToString());
                await _userHelper.AddUserToRoleAsync(user, UserType.User.ToString());
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCustomersAsync()
        {
            if (!_context.Customers.Any())
            {
                User user = await _context.Users.FirstOrDefaultAsync();
                for (int i = 0; i < 40; i++)
                {
                    _context.Customers.Add(new Customer
                    {
                        FirstName = $"Cliente {i}",
                        LastName = $"Apellido {i}",
                        PhoneNumber = "322 314 35 66",
                        Address = "Calle Falsa 123",
                        Email = $"cliente{i}@yopmail.com",
                        IsActive = true,
                        User = user
                    });
                }

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                Random random = new Random();
                User user = await _context.Users.FirstOrDefaultAsync();
                for (int i = 0; i < 200; i++)
                {
                    _context.Products.Add(new Product
                    {
                        Name = $"Producto {i}",
                        Description = $"Producto {i}",
                        Price = random.Next(5, 1000),
                        Stock = random.Next(0, 500),
                        IsActive = true,
                        User = user
                    });
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
