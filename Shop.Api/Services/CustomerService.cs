using FluentValidation;
using Shop.Api.Contracts.Requests;
using Shop.Api.Mapping;
using Shop.Api.Models;
using Shop.Api.Repositories;
using Shop.Api.Validators;

namespace Shop.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IValidator<UpdateCustomerRequest> _updateCustomerValidator;
    private readonly IValidator<CreateCustomerRequest> _createCustomerValidator;

    public CustomerService(ICustomerRepository customerRepository, IValidator<UpdateCustomerRequest> customerValidator, IValidator<CreateCustomerRequest> createCustomerValidator)
    {
        _customerRepository = customerRepository;
        _updateCustomerValidator = customerValidator;
        _createCustomerValidator = createCustomerValidator;
    }

    public async Task<Customer?> CreateAsync(CreateCustomerRequest createCustomerRequest, CancellationToken token = default)
    {
        await _createCustomerValidator.ValidateAndThrowAsync(createCustomerRequest, token);
        var customer = createCustomerRequest.MapToCustomer();
        await _customerRepository.CreateAsync(customer, token);
        return customer;
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
    {
        return _customerRepository.DeleteAsync(id, token);
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _customerRepository.GetByIdAsync(id, token);
    }

    public Task<IEnumerable<Customer?>> GetAllAsync(CancellationToken token = default)
    {
        return _customerRepository.GetAllAsync(token);
    }

    public async Task<Customer?> UpdateAsync(Guid id, UpdateCustomerRequest customerUpdate, CancellationToken token = default)
    {
        await _updateCustomerValidator.ValidateAndThrowAsync(customerUpdate, cancellationToken: token);
        var customer = await _customerRepository.GetByIdAsync(id, token);
        if (customer == null)
        {
            return null;
        }

        customerUpdate.MapToCustomer(customer);
        await _customerRepository.UpdateAsync(token);
        return customer;
    }
}