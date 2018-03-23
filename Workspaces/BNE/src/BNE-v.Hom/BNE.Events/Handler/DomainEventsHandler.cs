using SharedKernel.DomainEvents.CrossDomainEvents;

namespace BNE.Domain.Events.Handler
{
    public static class DomainEventsHandler
    {
        private static CrossDomainEventHandler _crossDomainEventHandler;
        private static CrossDomainEventHandler CrossDomainEventHandler => _crossDomainEventHandler ?? new CrossDomainEventHandler();

        public static void Handle(ICrossDomainEvent args)
        {
            CrossDomainEventHandler.Handle(args);
        }
    }
}
