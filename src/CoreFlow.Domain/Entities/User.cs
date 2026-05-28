using CoreFlow.Domain.Common;
using CoreFlow.Domain.Enums;

namespace CoreFlow.Domain.Entities;

public class User : BaseEntity
{
    public Guid CompanyId { get; private set; }
    public Company Company { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public ICollection<Sale> Sales { get; private set; } = new List<Sale>();

    private User() { }

    public User(Guid companyId, string name, string email, string passwordHash, UserRole role)
    {
        CompanyId = companyId;
        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        Role = role;
        IsActive = true;
    }

    public void Update(string name, string email, UserRole role)
    {
        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        Role = role;
        SetUpdated();
    }

    public void ChangeCompany(Guid companyId)
    {
        CompanyId = companyId;
        SetUpdated();
    }

    public void ResetPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        SetUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdated();
    }
}
