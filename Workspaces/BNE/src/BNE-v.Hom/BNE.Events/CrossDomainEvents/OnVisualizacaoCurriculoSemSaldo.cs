using SharedKernel.DomainEvents.CrossDomainEvents;

namespace BNE.Domain.Events.CrossDomainEvents
{
    public class OnVisualizacaoCurriculoSemSaldo : ICrossDomainEvent
    {
        public OnVisualizacaoCurriculoSemSaldo(int idCurriculo, decimal cnpj)
        {
            IdCurriculo = idCurriculo;
            CNPJ = cnpj;
        }

        public int IdCurriculo { get; private set; }
        public decimal CNPJ { get; private set; }
    }
}