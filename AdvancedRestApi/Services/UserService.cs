using AdvancedRestApi.Data;
using AdvancedRestApi.DTO_s;
using AdvancedRestApi.Interfaces;
using AdvancedRestApi.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace AdvancedRestApi.Services
{
    public class UserService : IUser
    {

        //private UserDbContext _dbContext;
        private IMapper _mapper;
        private IMongoCollection<User> _usersCollection;

        public UserService(IMapper mapper, IConfiguration config)
        {
            _mapper= mapper;
            //MongoDb DI
            var mongoClient = new MongoClient(config.GetConnectionString("UserConnection"));
            var mongoDatabase = mongoClient.GetDatabase("UsersDb");
            _usersCollection = mongoDatabase.GetCollection<User>("Users");
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> UpdateUser(Guid id, UserDTO userdto)
        {
            //var userObj =await _dbContext.Users.FindAsync(id);
            var userObj = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (userObj != null)
            {
                var user = _mapper.Map<User>(userdto);
                userObj.Name = user.Name;
                userObj.Address = user.Address;
                userObj.Phone = user.Phone;
                userObj.BloogGroup = user.BloogGroup;

                //await _dbContext.SaveChangesAsync();
                await _usersCollection.ReplaceOneAsync(u=>u.Id==id, userObj);
                return (true,null);
            }
            return (false,"User not Found");

        }

        public async Task<(bool IsSuccess, string ErrorMessage)> DeleteUser(Guid id)
        {
            //var user =await _dbContext.Users.FindAsync(id);
            var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user != null)
            {
                //_dbContext.Remove(user);
                //await _dbContext.SaveChangesAsync();
                await _usersCollection.DeleteOneAsync(u => u.Id == id);
                return (true,null);
            }
            return (false, "User not Found");

        }

        public async Task<(bool IsSuccess, string ErrorMessage)> AddUser(UserDTO userdto)
        {
            if (userdto != null)
            {
                var user = _mapper.Map<User>(userdto);
                //await _dbContext.Users.AddAsync(user);
                //await _dbContext.SaveChangesAsync();
                await _usersCollection.InsertOneAsync(user);
                return (true, null);
            }
            return (false, "Please provide the user data");
        }
        
        public async Task<(bool IsSuccess, List<UserDTO>, string ErrorMessage)> GetAllUser()
        {
            //var users = await _dbContext.Users.ToListAsync();
            var users = await _usersCollection.Find(u => true).ToListAsync();

            if (users.Any())
            {
                // Return the list of users as list of UserDto to the client.
                var result = _mapper.Map<List<UserDTO>>(users);
                return (true, result, null);
            }
            return (false, null, "No Users Found");
        }

        public async Task<(bool IsSuccess, UserDTO, string ErrorMessage)> GetUserById(Guid id)
        {
            //var user = await _dbContext.Users.FindAsync(id);
            var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user != null)
            {
                //Return the user as userdto.
                var result = _mapper.Map<UserDTO>(user);
                return (true, result, null);
            }
            return (false,null,"No User Found");
        }

    }
}
