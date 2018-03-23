using SharedKernel.DomainEvents.CrossDomainEvents;

namespace BNE.Domain.Events.Handler
{
    public static class DomainEventsHandler
    {
        private static CrossDomainEventHandler _crossDomainEventHandler;
        public static CrossDomainEventHandler CrossDomainEventHandler => _crossDomainEventHandler ?? (_crossDomainEventHandler = new CrossDomainEventHandler());

        public static void Handle(ICrossDomainEvent args)
        {
            CrossDomainEventHandler.Handle(args);
        }
    }
}
