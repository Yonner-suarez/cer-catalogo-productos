using FluentValidation;

namespace microCatalagoProductos.API.Model.Request.Validators
{
    public class ProductosValidator : AbstractValidator<ProductosRequest>
    {
        public ProductosValidator()
        {
            //RuleFor(x => x.MyProperty)
            //   .Must(m => m != 0)
            //   .WithMessage("Debe ingresar una marca válida.");
        }
    }
}
