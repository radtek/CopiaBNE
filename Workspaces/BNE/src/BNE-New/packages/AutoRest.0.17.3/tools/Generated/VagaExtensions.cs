// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace 
{
    using System.Threading.Tasks;
   using Models;

    /// <summary>
    /// Extension methods for Vaga.
    /// </summary>
    public static partial class VagaExtensions
    {
            /// <summary>
            /// Recupera uma vaga pelo ID
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='idVaga'>
            /// </param>
            public static VagaResponse Get(this IVaga operations, int? idVaga = default(int?))
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((IVaga)s).GetAsync(idVaga), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Recupera uma vaga pelo ID
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='idVaga'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<VagaResponse> GetAsync(this IVaga operations, int? idVaga = default(int?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.GetWithHttpMessagesAsync(idVaga, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
