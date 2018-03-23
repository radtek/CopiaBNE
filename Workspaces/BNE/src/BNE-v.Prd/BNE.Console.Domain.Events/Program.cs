using System.Threading.Tasks;
using BNE.Domain.Events.CrossDomainEvents;
using BNE.Domain.Events.Handler;
using BNE.BLL;

namespace BNE.Console.Domain.Events
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parallel.ForEach(Curriculo.RecuperarIdsParaDW(), curriculo =>
            {
                DomainEventsHandler.Handle(new OnAtualizarCurriculo(curriculo, string.Empty));
            });
           
        }
    }
}