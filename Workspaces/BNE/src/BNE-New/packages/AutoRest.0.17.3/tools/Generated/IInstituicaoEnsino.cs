// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace 
{
    using Models;

    /// <summary>
    /// InstituicaoEnsino operations.
    /// </summary>
    public partial interface IInstituicaoEnsino
    {
        /// <summary>
        /// Recupera as sugestões de email
        /// </summary>
        /// <param name='query'>
        /// </param>
        /// <param name='limit'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        System.Threading.Tasks.Task<Microsoft.Rest.HttpOperationResponse<System.Collections.Generic.IList<InstituicaoEnsinoResponse>>> GetInstituicaoEnsinosWithHttpMessagesAsync(string query = default(string), int? limit = default(int?), System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
}
