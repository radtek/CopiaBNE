using SharedKernel.DomainEvents.CrossDomainEvents;

namespace BNE.Domain.Events.CrossDomainEvents
{
    public class OnNovaCandidatura : ICrossDomainEvent
    {
        public OnNovaCandidatura(int idCurriculo, int idVaga)
        {
            IdCurriculo = idCurriculo;
            IdVaga = idVaga;
        }

        public int IdCurriculo { get; private set; }
        public int IdVaga { get; private set; }
    }
}