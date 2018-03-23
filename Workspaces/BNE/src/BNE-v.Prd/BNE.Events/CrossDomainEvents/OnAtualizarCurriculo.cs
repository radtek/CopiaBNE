using SharedKernel.DomainEvents.CrossDomainEvents;

namespace BNE.Domain.Events.CrossDomainEvents
{
    public class OnAtualizarCurriculo : ICrossDomainEvent
    {
        public int Id { get; private set; }
        public string Email { get; private set; }

        public OnAtualizarCurriculo(int id, string email)
        {
            Id = id;
            Email = email;
        }
    }

}