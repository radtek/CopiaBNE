//-- Data: 16/01/2017 14:05
//-- Autor: Ramalho

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL.Notificacao
{
	public partial class CartaVip // Tabela: TAB_Carta_VIP
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

		#region Construtores
		public CartaVip()
		{
		}
		public CartaVip(int idCarta)
		{
			this._idCarta = idCarta;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Carta_VIP (Idf_Carta_VIP, Nme_Carta_VIP, Vlr_Carta_VIP) VALUES (@Idf_Carta_VIP, @Nme_Carta_VIP, @Vlr_Carta_VIP);";
		private const string SPUPDATE = "UPDATE TAB_Carta_VIP SET Nme_Carta_VIP = @Nme_Carta_VIP, Vlr_Carta_VIP = @Vlr_Carta_VIP WHERE Idf_Carta_VIP = @Idf_Carta_VIP";
		private const string SPDELETE = "DELETE FROM TAB_Carta_VIP WHERE Idf_Carta_VIP = @Idf_Carta_VIP";
		private const string SPSELECTID = "SELECT * FROM TAB_Carta_VIP WITH(NOLOCK) WHERE Idf_Carta_VIP = @Idf_Carta_VIP";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_VIP", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Carta_VIP", SqlDbType.VarChar, 70));
			parms.Add(new SqlParameter("@Vlr_Carta_VIP", SqlDbType.VarChar));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idCarta;
			parms[1].Value = this._nomeCarta;
			parms[2].Value = this._valorCarta;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Carta no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert()
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);

			using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
			{
				conn.Open();
				using (SqlTransaction trans = conn.BeginTransaction())
				{
					try
					{
						SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
						cmd.Parameters.Clear();
						this._persisted = true;
						this._modified = false;
						trans.Commit();
					}
					catch
					{
						trans.Rollback();
						throw;
					}
				}
			}
		}
		/// <summary>
		/// Método utilizado para inserir uma instância de Carta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Carta no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Update()
		{
			if (this._modified)
			{
				List<SqlParameter> parms = GetParameters();
				SetParameters(parms);
				DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPUPDATE, parms);
				this._modified = false;
			}
		}
		/// <summary>
		/// Método utilizado para atualizar uma instância de Carta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Update(SqlTransaction trans)
		{
			if (this._modified)
			{
				List<SqlParameter> parms = GetParameters();
				SetParameters(parms);
				DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
				this._modified = false;
			}
		}
		#endregion

		#region Save
		/// <summary>
		/// Método utilizado para salvar uma instância de Carta no banco de dados.
		/// </summary>
		/// <remarks>Gieyson Stelmak</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de Carta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public void Save(SqlTransaction trans)
		{
			if (!this._persisted)
				this.Insert(trans);
			else
				this.Update(trans);
		}
		#endregion

		#region Delete
		/// <summary>
		/// Método utilizado para excluir uma instância de Carta no banco de dados.
		/// </summary>
		/// <param name="idCarta">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCarta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_VIP", SqlDbType.Int, 4));

			parms[0].Value = idCarta;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Carta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCarta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCarta, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_VIP", SqlDbType.Int, 4));

			parms[0].Value = idCarta;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Carta no banco de dados.
		/// </summary>
		/// <param name="idCarta">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCarta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Carta_VIP where Idf_Carta_VIP in (";

			for (int i = 0; i < idCarta.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCarta[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCarta">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCarta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_VIP", SqlDbType.Int, 4));

			parms[0].Value = idCarta;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCarta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCarta, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_VIP", SqlDbType.Int, 4));

			parms[0].Value = idCarta;

			return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar uma consulta paginada do banco de dados.
		/// </summary>
		/// <param name="colunaOrdenacao">Nome da coluna pela qual será ordenada.</param>
		/// <param name="direcaoOrdenacao">Direção da ordenação (ASC ou DESC).</param>
		/// <param name="paginaCorrente">Número da página que será exibida.</param>
		/// <param name="tamanhoPagina">Quantidade de itens em cada página.</param>
		/// <param name="totalRegistros">Quantidade total de registros na tabela.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
		{
			int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
			int fim = paginaCorrente * tamanhoPagina;

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Car.Idf_Carta_VIP, Car.Nme_Carta_VIP, Car.Vlr_Carta_VIP, Car.Dta_Cadastro, Car.Des_Assunto FROM TAB_Carta_VIP Car";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Carta a partir do banco de dados.
		/// </summary>
		/// <param name="idCarta">Chave do registro.</param>
		/// <returns>Instância de Carta.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CartaVip LoadObject(int idCarta)
		{
			using (IDataReader dr = LoadDataReader(idCarta))
			{
                CartaVip objCarta = new CartaVip();
				if (SetInstance(dr, objCarta))
					return objCarta;
			}
			throw (new RecordNotFoundException(typeof(Carta)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Carta a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCarta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Carta.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CartaVip LoadObject(int idCarta, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCarta, trans))
			{
				CartaVip objCarta = new CartaVip();
				if (SetInstance(dr, objCarta))
					return objCarta;
			}
			throw (new RecordNotFoundException(typeof(Carta)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Carta a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCarta))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Carta a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCarta, trans))
			{
				return SetInstance(dr, this);
			}
		}
		#endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objCarta">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CartaVip objCarta)
		{
			try
			{
				if (dr.Read())
				{
					objCarta._idCarta = Convert.ToInt32(dr["Idf_Carta_VIP"]);
					objCarta._nomeCarta = Convert.ToString(dr["Nme_Carta_VIP"]);
					objCarta._valorCarta = Convert.ToString(dr["Vlr_Carta_VIP"]);

					objCarta._persisted = true;
					objCarta._modified = false;

					return true;
				}
				else
				{
					return false;
				}
			}
			catch 
			{
				throw;
			}
			finally
			{
				dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}