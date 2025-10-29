using ECommerceApp.Application.Dto.Product;
using FluentValidation;

namespace ECommerceApp.Application.Validation
{
    public interface IValidationService
    {
        Task<ServiceResponse> ValidateAsync<T> (T model,IValidator<T> validator);
    }
    public class ValidationService : IValidationService
    {
        public async Task<ServiceResponse> ValidateAsync<T>(T model, IValidator<T> validator)
        {
            var result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                return new ServiceResponse(false, errors);
            }

            return new ServiceResponse(true, "Validation successful");
        }
    }

}

