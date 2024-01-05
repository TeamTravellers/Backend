using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Core.Contracts;

namespace YourPlace.Core.Services
{
    public class UserServices : IUsers
    {
        private readonly YourPlaceDbContext _dbContext;
        public UserServices(YourPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //SIGN UP
        public async Task CreateAccount(User user) 
        {
            var newUser = new User
            {
                FirstName = user.FirstName,
                Surname = user.Surname,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            };
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }

        //LOG IN
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
                CreateAccount(user);    
            }
            Console.WriteLine(result);
        }

        //Account Features
        public async Task DeleteAccount(User user)
        {
            _dbContext.Users.Remove(user);
            _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAccount(User editedUser)
        {
            var userToBeEdited = await _dbContext.Users.FindAsync(editedUser.UserID);
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
    }
}
