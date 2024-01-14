using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Infrastructure.Data.Enums;
using YourPlace.Core.Contracts;
using Microsoft.AspNetCore.Identity;
using YourPlace.Infrastructure.Data.Enums;

namespace YourPlace.Core.Services
{
    public class UserServices 
    {
        private readonly UserManager<User> userManager;
        private readonly YourPlaceDbContext _dbContext;
        public UserServices(YourPlaceDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            this.userManager = userManager;
        }

        #region SIGN UP
        public async Task CreateAccountAsync(string firstName, string surname, string email, string password, Roles role)
        {
            try
            {
                User user = new User(firstName, surname, email);
                IdentityResult result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    throw new ArgumentException(result.Errors.First().Description);
                }
                if (role == Roles.HotelManager)
                {
                    await userManager.AddToRoleAsync(user, Roles.HotelManager.ToString());
                }
                else
                {
                    await userManager.AddToRoleAsync(user, Roles.Traveller.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        #endregion

        #region LOG IN
        public async Task LogIn(User user) //Checks if the account already exists in the database
        {
            bool result;
            if (_dbContext.Users.Contains(user))
            {
                result = true;
            }
            else
            {
                result = false;
                CreateAccountAsync(user.FirstName, user.Surname, user.Email, user.Password, user.Role);    
            }
            Console.WriteLine(result);
        }
        #endregion

        #region Account Features
        public async Task DeleteAccount(User user)
        {
            _dbContext.Users.Remove(user);
            _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAccount(User editedUser)
        {
            var userToBeEdited = await _dbContext.Users.FindAsync(editedUser.Id);
            userToBeEdited.FirstName = editedUser.FirstName;
            userToBeEdited.Surname = editedUser.Surname;
            userToBeEdited.Email = editedUser.Email;
            userToBeEdited.Password = editedUser.Password;
            userToBeEdited.Role = editedUser.Role;
            await _dbContext.SaveChangesAsync();
        }

        public async Task ResetPassword(User user, string newPassword)
        {
            user.Password = newPassword; 
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
