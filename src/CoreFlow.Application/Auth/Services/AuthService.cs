using CoreFlow.Application.Auth.DTOs;
using CoreFlow.Application.Auth.Repositories;
using CoreFlow.Application.Auth.Token;
using CoreFlow.Application.Security;
using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterUserRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);

        if (existingUser is not null)
            throw new Exception("User already exists.");

        var passwordHash = PasswordHasher.Hash(request.Password);

        var user = new User(
            request.CompanyId,
            request.Name,
            request.Email,
            passwordHash,
            request.Role
        );

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var token = _jwtTokenGenerator.Generate(user);

        return new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            CompanyId = user.CompanyId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
            throw new Exception("Invalid credentials.");

        var passwordIsValid = PasswordHasher.Verify(request.Password, user.PasswordHash);

        if (!passwordIsValid)
            throw new Exception("Invalid credentials.");

        var token = _jwtTokenGenerator.Generate(user);

        return new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            CompanyId = user.CompanyId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}