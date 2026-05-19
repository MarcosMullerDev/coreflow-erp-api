namespace CoreFlow.Domain.Entities;

public class Company
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Document { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    private Company() { }

    public Company(string name, string document, string email, string phone)
    {
        Id = Guid.NewGuid();
        Name = name;
        Document = document;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void Update(string name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}