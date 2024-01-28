using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;

namespace YourPlace.Core.Services
{
    public class SuggestionsServices 
    {
        private readonly YourPlaceDbContext _dbContext;
        public SuggestionsServices(YourPlaceDbContext dbContext)
        {
            dbContext = _dbContext;
        }
        #region CRUD
        public async Task CreateAsync(Suggestion suggestion) 
        {
            try
            {
                _dbContext.Suggestions.Add(suggestion);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Suggestion> ReadAsync(string userID, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Suggestion> suggestions = _dbContext.Suggestions;
                if(useNavigationalProperties)
                {
                    suggestions = suggestions.Include(x => x.User);
                }
                if (isReadOnly)
                {
                    suggestions.AsNoTrackingWithIdentityResolution();
                }
                return await suggestions.SingleOrDefaultAsync(x => x.UserID == userID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Suggestion>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Suggestion> suggestions = _dbContext.Suggestions;
                if (useNavigationalProperties)
                {
                    suggestions = suggestions.Include(x => x.User);
                }
                if (isReadOnly)
                {
                    suggestions.AsNoTrackingWithIdentityResolution();
                }
                return await suggestions.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateAsync(Suggestion suggestion)
        {
            try
            {
                _dbContext.Suggestions.Update(suggestion);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task DeleteAsync(string userID)
        {
            try
            {
                Suggestion suggestion = await ReadAsync(userID, false, false);
                if (suggestion is null)
                {
                    throw new ArgumentException(string.Format($"Category with hotelID {userID} does " +
                        $"not exist in the database!"));
                }
                _dbContext.Suggestions.Remove(suggestion);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
