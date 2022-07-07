using Microsoft.AspNetCore.Mvc;
using TestApi.Models;
using TestApi.Repositories;
using TestApi.SecretSauce;

namespace TestApi.EndpointDefinitions;

public class CustomerEndpointDefinition : IEndpointDefinition
{
	private readonly string _path = "/customers";
	public void DefineEndpoints(WebApplication app)
	{
		app.MapGet(_path, GetAllCustomers);
		app.MapGet(_path + "/{id}", GetCustomerById);
		app.MapPost(_path, CreateCustomer);
		app.MapPut(_path + "/{id}", UpdateCustomer);
		app.MapDelete(_path + "/{id}", RemoveCustomer);
	}

	internal List<Customer> GetAllCustomers(ICustomerRepository repo)
	{
		return repo.GetAll();
	}

	internal IResult GetCustomerById(ICustomerRepository repo, Guid id)
	{
		var customer = repo.GetById(id);
		return customer is not null ? Results.Ok(customer) : Results.NotFound();
	}

	internal IResult CreateCustomer(ICustomerRepository repo, string fullname)
	{
		var id = repo.Add(fullname);
		return Results.Created($"{_path}/{id}", repo.GetById(id));
	}

	internal IResult UpdateCustomer(ICustomerRepository repo, Guid id, string fullname)
	{
		return repo.TryUpdate(id, fullname) ? Results.Ok(repo.GetById(id)) : Results.NotFound();
	}

	internal IResult RemoveCustomer(ICustomerRepository repo, Guid id)
	{
		return repo.TryDelete(id) ? Results.Ok() : Results.NotFound();
	}

	public void DefineServices(IServiceCollection services)
	{
		services.AddSingleton<ICustomerRepository, CustomerRepository>();
	}
}