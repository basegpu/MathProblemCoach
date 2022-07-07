using Microsoft.AspNetCore.Mvc;
using TestApi.Repositories;
using TestApi.SecretSauce;

namespace TestApi.EndpointDefinitions;

public class CustomerEndpointDefinition : IEndpointDefinition
{
	public void DefineEndpoints(WebApplication app)
	{
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
	}

	public void DefineServices(IServiceCollection services)
	{
		services.AddSingleton<CustomerRepository>();
	}
}