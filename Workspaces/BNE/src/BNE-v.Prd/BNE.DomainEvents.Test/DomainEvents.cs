using BNE.Domain.Events.CrossDomainEvents;
using BNE.Domain.Events.Handler;
using System.Collections.Generic;
using Xunit;

namespace BNE.DomainEvents.Test
{
    public class DomainEvents
    {
        [Fact]
        [Trait("Category", "Candidatura")]
        public void Candidatura_NotificarNovaCandidatura_DeveEnviarComSucesso()
        {
            var cvvaga = new Dictionary<int, int>
            {
                {564517, 1804943 },
                {4866763, 1816074 }
            };

            foreach (var kvp in cvvaga)
            {
                DomainEventsHandler.Handle(new OnNovaCandidatura(kvp.Key, kvp.Value));
            }
        }
    }
}
