using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Auth.Token;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}