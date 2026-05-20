using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Auth.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
}