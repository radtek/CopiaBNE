using System;

namespace BNE.Services.AsyncServices.Base.ProcessosAssincronos
{
    /// <summary>
    /// A classe de atividade que trafega na message Queue
    /// </summary>
    [Serializable]
    public class MensagemAtividade
    {
        /// <summary>
        /// O idf da atividade no banco
        /// </summary>
        public int IdfAtividade { get; set; }       
        
    }
}