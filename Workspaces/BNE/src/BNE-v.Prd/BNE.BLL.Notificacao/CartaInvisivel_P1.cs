//-- Data: 15/02/2013 10:12
//-- Autor: Gieyson Stelmak

namespace BNE.BLL.Notificacao
{
	public partial class CartaInvisivel // Tabela: TAB_Carta_Invisivel
    {
		#region Atributos
		private int _idCarta;
		private string _nomeCarta;
		private string _valorCarta;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCarta
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCarta
		{
			get
			{
				return this._idCarta;
			}
			set
			{
				this._idCarta = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeCarta
		/// <summary>
		/// Tamanho do campo: 70.
		/// Campo obrigatório.
		/// </summary>
		public string NomeCarta
		{
			get
			{
				return this._nomeCarta;
			}
			set
			{
				this._nomeCarta = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorCarta
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo obrigatório.
		/// </summary>
		public string ValorCarta
		{
			get
			{
				return this._valorCarta;
			}
			set
			{
				this._valorCarta = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion
	}
}