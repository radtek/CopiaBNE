using SharedKernel.DomainEvents.CrossDomainEvents;

namespace BNE.Domain.Events.CrossDomainEvents
{
    public class OnRemoverCurriculo : ICrossDomainEvent
    {
        public string Email { get; set; }
        public int Id { get; set; }


        public OnRemoverCurriculo(int id, string email)
        {
            this.Id = id;
            this.Email = email;
        }
    }
}
