using Catalog.API.Application.Models.CatalogItem;
using Catalog.API.Application.Validation.Common;
using Catalog.API.DataAccess;
using FluentValidation;

namespace Catalog.API.Application.Validation.Validators;

public class CatalogItemEditDtoValidator : AbstractValidator<CatalogItemEditDto>
{
    public CatalogItemEditDtoValidator(ICatalogDbContext catalogDb)
    {
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.Price).GreaterThanOrEqualTo(0)
            .When(x => x.Price.HasValue);

        RuleFor(x => x.BrandId).MustAsync((id, _) => catalogDb.CatalogBrands.Exists(id))
            .WithMessage(x => ErrorMessageBuilder.EntityNotFound());

        RuleFor(x => x.TypeId).MustAsync((id, _) => catalogDb.CatalogTypes.Exists(id))
            .WithMessage(x => ErrorMessageBuilder.EntityNotFound());
    }
}