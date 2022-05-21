using hanap_buhay_server.Contexts;
using hanap_buhay_server.Entities;
using hanap_buhay_server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using EmailValidation;
using hanap_buhay_server.Models;
using static BCrypt.Net.BCrypt;

namespace hanap_buhay_server.Services
{
    public class UserService : IUserService
    {
        private readonly hanapbuhayContext _context;

        public UserService(hanapbuhayContext context)
        {
            _context = context;
        }

        private async Task<bool> IsUserNameExists(string username)
        {
            var isUserNameExist = await _context
                .Users
                .AnyAsync(x => x.UserName.ToLower().Trim() == username.ToLower().Trim());

            return isUserNameExist;
        }

        private async Task<bool> IsEmailExists(string email)
        {
            var isEmailExists = await _context
                .Users
                .AnyAsync(x => x.Email.ToLower().Trim() == email.ToLower().Trim());

            return isEmailExists;
        }

        public async Task<User> Create(User user)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user));

                if (await IsUserNameExists(user.UserName))
                    throw new OperationCanceledException($"Username '{user.UserName}' already in use");

                if (await IsEmailExists(user.Email))
                    throw new OperationCanceledException($"Email '{user.Email}' already in use");

                if (!EmailValidator.Validate(user.Email))
                    throw new OperationCanceledException("Invalid email address");

                if (string.IsNullOrEmpty(user.Password))
                    throw new OperationCanceledException("Invalid password!");

                user.Uuid = Guid.NewGuid();
                user.Password = HashPassword(user.Password);

                var record = await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return record.Entity;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<User> Update(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var record = _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return record.Entity;
        }
        public async Task Delete(Guid userId)
        {
            _context.Users.Remove(new User { Uuid = userId });
            await _context.SaveChangesAsync();
        }

        public async Task<User?> Read(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Uuid == userId);
        }

        public async Task<List<User>> ReadAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<dynamic> Login(Login parameters)
        {
            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.UserName.ToLower().Trim() == parameters.UserName);

            const string errorMsg = "Invalid username and password";

            if (user is null)
                throw new OperationCanceledException(errorMsg);

            if (!Verify(parameters.Password, user.Password))
                throw new OperationCanceledException(errorMsg);

            user.Password = null;
            return user;
        }
    }
}
