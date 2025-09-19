using FluentValidation;

namespace microCatalagoProductos.API.Model.Request.Validators
{
    public class AgregarProductoValidator : AbstractValidator<AgregarProductoRequest>
    {
        public AgregarProductoValidator()
        {
            RuleFor(x => x.MyProperty)
               .Must(m => m != 0)
               .WithMessage("Debe ingresar una marca válida.");
        }
    }
}
