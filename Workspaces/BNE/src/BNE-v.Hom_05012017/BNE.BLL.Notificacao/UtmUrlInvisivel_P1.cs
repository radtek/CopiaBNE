//-- Data: 05/08/2016 10:59
//-- Autor: Gieyson Stelmak

namespace BNE.BLL.Notificacao
{
	public partial class UtmUrlInvisivel // Tabela: alerta.TAB_UtmUrl_Invisivel
    {
		#region Atributos
        #endregion

		#region Propriedades

		#region IdUtmUrl
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdUtmUrl { get; set; }
        #endregion 

		#region NomeUtmUrl
		/// <summary>
		/// Tamanho do campo: 70.
		/// Campo obrigatório.
		/// </summary>
		public string NomeUtmUrl { get; set; }
        #endregion 

		#region ValorUtmUrl
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo obrigatório.
		/// </summary>
		public string ValorUtmUrl { get; set; }
        #endregion 

		#endregion
	}
}