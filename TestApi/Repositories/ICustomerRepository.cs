using TestApi.Models;

namespace TestApi.Repositories;

interface ICustomerRepository
{
	Guid Add(string fullname);
	Customer GetById(Guid id);
	List<Customer> GetAll();
	bool TryUpdate(Guid id, string fullname);
	bool TryDelete(Guid id);
}