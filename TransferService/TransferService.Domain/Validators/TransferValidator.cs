using FluentValidation;
using System.Text.RegularExpressions;

namespace TransferService.Domain.Validators
{
    public class TransferValidator : AbstractValidator<Transfer>
    {
        public TransferValidator()
        {
            RuleFor(t => t.Value)
                .NotNull().WithMessage("Campo valor é obrigatório.")
                .LessThanOrEqualTo(0).WithMessage("Valor não pode ser menor que 0.")
                .GreaterThan(10000).WithMessage("Valor não pode ser maior que 10 mil.");
        }
    }
}
