using Catalog.Application.Dto.CatalogItem;
using Catalog.Application.Services;
using Catalog.Application.Validation.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Validation.Validators.Catalog;

public class CatalogItemEditDtoValidator : AbstractValidator<CatalogItemEditDto>
{
    public CatalogItemEditDtoValidator(ICatalogDbContext dbContext)
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