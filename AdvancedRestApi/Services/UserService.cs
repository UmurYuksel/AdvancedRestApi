using AdvancedRestApi.Data;
using AdvancedRestApi.DTO_s;
using AdvancedRestApi.Interfaces;
using AdvancedRestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedRestApi.Services
{
    public class UserService : IUser
    {

        private UserDbContext _dbContext;

        public UserService(UserDbContext dbContext)
        {
            _dbContext= dbContext;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> UpdateUser(Guid id, UserDTO user)
        {
            var userObj =await _dbContext.Users.FindAsync(id);
            if (userObj != null)
            {
                userObj.Name = user.Name;
                userObj.Address = user.Address;
                userObj.Phone = user.Phone;
                userObj.BloogGroup = user.BloogGroup;
                await _dbContext.SaveChangesAsync();
                return (true,null);
            }
            return (false,"User not Found");

        }

        public async Task<(bool IsSuccess, string ErrorMessage)> DeleteUser(Guid id)
        {
            var user =await _dbContext.Users.FindAsync(id);
            if(user != null)
            {
                _dbContext.Remove(user);
                await _dbContext.SaveChangesAsync();
                return (true,null);
            }
            return (false, "User not Found");

        }

        public async Task<(bool IsSuccess, string ErrorMessage)> AddUser(UserDTO userdto)
        {
            if (userdto != null)
            {
                var user = new User();
                user.Name = userdto.Name;
                user.Address = userdto.Address;
                user.Phone = userdto.Phone;
                user.BloogGroup= userdto.BloogGroup;
                //
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            return (false, "Please provide the user data");
        }
        
        public async Task<(bool IsSuccess, List<UserDTO>, string ErrorMessage)> GetAllUser()
        {
            var users = await _dbContext.Users.ToListAsync();
            if (users.Any())
            {
                return (true, users, null);
            }
            return (false, null, "No Users Found");
        }

        public async Task<(bool IsSuccess, UserDTO, string ErrorMessage)> GetUserById(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                return (true, user, null);
            }
            return (false,null,"No User Found");
        }

    }
}
