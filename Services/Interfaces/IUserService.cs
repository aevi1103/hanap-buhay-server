using hanap_buhay_server.Entities;
using hanap_buhay_server.Models;

namespace hanap_buhay_server.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Create(User user);
        Task<User> Update(User user);
        Task Delete(Guid userId);
        Task<User?> Read(Guid userId);
        Task<List<User>> ReadAll();
        Task<dynamic> Login(Login parameters);
    }
}
