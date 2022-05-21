using hanap_buhay_server.Contexts;
using hanap_buhay_server.Entities;
using hanap_buhay_server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using EmailValidation;

namespace hanap_buhay_server.Services
{
    public class UserService : IUserService
    {
        private readonly hanapbuhayContext _context;

        public UserService(hanapbuhayContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.Uuid = Guid.NewGuid();
            user.DateCreated = DateTime.Now;
            user.DateModified = null;

            var isUserNameExist = await _context
                .Users
                .AnyAsync(x => x.UserName.ToLower().Trim() == user.UserName.ToLower().Trim());

            if (isUserNameExist)
                throw new OperationCanceledException($"Username '{user.UserName}' already in use");

            var isEmailExists = await _context
                .Users
                .AnyAsync(x => x.Email.ToLower().Trim() == user.Email.ToLower().Trim());

            if (isEmailExists)
                throw new OperationCanceledException($"Email '{user.Email}' already in use");

            var isValidEmail = EmailValidator.Validate(user.Email);

            if (!isValidEmail)
                throw new OperationCanceledException("Invalid email address");

            var record = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return record.Entity;
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


    }
}
