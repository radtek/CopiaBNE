using FluentValidation;

namespace BNE.Web.LanHouse.FluentValidators
{
    public class ModelAjaxSextaTelaValidator : AbstractValidator<BNE.Web.LanHouse.Models.ModelAjaxSextaTela>
    {
        public ModelAjaxSextaTelaValidator()
        {
            RuleFor(x => x.IdEstadoCivil)
                .NotEmpty()
                .InclusiveBetween(1 /*solteiro*/, 7 /*amasiado*/)
                .WithName("Número relacionado ao estado civil")
                .WithMessage("Selecione um estado civil");
        }
    }
}