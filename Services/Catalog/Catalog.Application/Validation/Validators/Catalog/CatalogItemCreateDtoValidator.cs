using Catalog.Application.Dto.CatalogItem;
using Catalog.Application.Interfaces.DataAccess;
using Catalog.Application.Validation.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Validation.Validators.Catalog;

public class CatalogItemCreateDtoValidator : AbstractValidator<CatalogItemCreateDto>
{
    public CatalogItemCreateDtoValidator(ICatalogDbContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.Price).GreaterThanOrEqualTo(0)
            .When(x => x.Price.HasValue);

        RuleFor(x => x.BrandId).MustAsync((id, _) => dbContext.CatalogBrands.AnyAsync(x => x.Id == id))
            .WithMessage(x => ErrorMessageBuilder.EntityNotFound());

        RuleFor(x => x.TypeId).MustAsync((id, _) => dbContext.CatalogTypes.AnyAsync(x => x.Id == id))
            .WithMessage(x => ErrorMessageBuilder.EntityNotFound());
    }
}