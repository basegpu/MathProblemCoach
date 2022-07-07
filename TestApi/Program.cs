using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<CustomerRepository>();

var app = builder.Build();

app.MapGet("/customers", ([FromServices] CustomerRepository repo) =>
{
	return repo.GetAll();
});

app.MapGet("/customers/{id}", ([FromServices] CustomerRepository repo, Guid id) =>
{
	var customer = repo.GetById(id);
	return customer is not null ? Results.Ok(customer) : Results.NotFound();
});

app.MapPost("/customers", ([FromServices] CustomerRepository repo, string fullname) =>
{
	return repo.Add(fullname);
});

app.MapPut("/customers/{id}", ([FromServices] CustomerRepository repo, Guid id, string fullname) =>
{
	return repo.TryUpdate(id, fullname) ? Results.Ok(repo.GetById(id)) : Results.NotFound();
	
});

app.MapDelete("/customers/{id}", ([FromServices] CustomerRepository repo, Guid id) =>
{
	repo.Delete(id);
	return Results.Ok();
});

app.Run();

record Customer(Guid Id, string FullName);

class CustomerRepository
{
	private readonly Dictionary<Guid, Customer> _customers = new();

	public Guid Add(string fullname)
	{
		var customer = new Customer(Guid.NewGuid(), fullname);
		_customers[customer.Id] = customer;
		return customer.Id;
	}

	public Customer GetById(Guid id)
	{
		return _customers[id];
	}

	public List<Customer> GetAll()
	{
		return _customers.Values.ToList();
	}

	public bool TryUpdate(Guid id, string fullname)
	{
		var existingCustomer = GetById(id);
		if (existingCustomer is null) return false;
		_customers[id] = new Customer(id, fullname);
		return true;
	}

	public void Delete(Guid id)
	{
		_customers.Remove(id);
	}
}