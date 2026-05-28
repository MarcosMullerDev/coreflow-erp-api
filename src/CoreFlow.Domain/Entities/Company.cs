using CoreFlow.Domain.Common;

namespace CoreFlow.Domain.Entities;

public class Company : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Document { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public ICollection<User> Users { get; private set; } = new List<User>();
    public ICollection<Product> Products { get; private set; } = new List<Product>();
    public ICollection<Sale> Sales { get; private set; } = new List<Sale>();
    public ICollection<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();
    public ICollection<Lead> Leads { get; private set; } = new List<Lead>();
    public CompanySettings? Settings { get; private set; }

    private Company() { }

    public Company(string name, string document, string email, string phone)
    {
        Name = name;
        Document = document;
        Email = email;
        Phone = phone;
        IsActive = true;
    }

    public void Update(string name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
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