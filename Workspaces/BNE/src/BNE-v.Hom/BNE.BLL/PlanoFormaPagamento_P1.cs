//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PlanoFormaPagamento // Tabela: BNE_Plano_Forma_Pagamento
	{
		#region Atributos
		private int _idPlanoFormaPagamento;
		private string _descricaoPlanoFormaPagamento;
		private string _TxtFormula;
		private DateTime _dataCadasrto;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPlanoFormaPagamento
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdPlanoFormaPagamento
		{
			get
			{
				return this._idPlanoFormaPagamento;
			}
			set
			{
				this._idPlanoFormaPagamento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPlanoFormaPagamento
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoPlanoFormaPagamento
		{
			get
			{
				return this._descricaoPlanoFormaPagamento;
			}
			set
			{
				this._descricaoPlanoFormaPagamento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TxtFormula
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string TxtFormula
		{
			get
			{
				return this._TxtFormula;
			}
			set
			{
				this._TxtFormula = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCadasrto
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataCadasrto
		{
			get
			{
				return this._dataCadasrto;
			}
			set
			{
				this._dataCadasrto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagInativo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagInativo
		{
			get
			{
				return this._flagInativo;
			}
			set
			{
				this._flagInativo = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PlanoFormaPagamento()
		{
		}
		public PlanoFormaPagamento(int idPlanoFormaPagamento)
		{
			this._idPlanoFormaPagamento = idPlanoFormaPagamento;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Plano_Forma_Pagamento (Idf_Plano_Forma_Pagamento, Des_Plano_Forma_Pagamento, Txt_Formula, Dta_Cadasrto, Flg_Inativo) VALUES (@Idf_Plano_Forma_Pagamento, @Des_Plano_Forma_Pagamento, @Txt_Formula, @Dta_Cadasrto, @Flg_Inativo);";
		private const string SPUPDATE = "UPDATE BNE_Plano_Forma_Pagamento SET Des_Plano_Forma_Pagamento = @Des_Plano_Forma_Pagamento, Txt_Formula = @Txt_Formula, Dta_Cadasrto = @Dta_Cadasrto, Flg_Inativo = @Flg_Inativo WHERE Idf_Plano_Forma_Pagamento = @Idf_Plano_Forma_Pagamento";
		private const string SPDELETE = "DELETE FROM BNE_Plano_Forma_Pagamento WHERE Idf_Plano_Forma_Pagamento = @Idf_Plano_Forma_Pagamento";
		private const string SPSELECTID = "SELECT * FROM BNE_Plano_Forma_Pagamento WHERE Idf_Plano_Forma_Pagamento = @Idf_Plano_Forma_Pagamento";
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
			parms.Add(new SqlParameter("@Idf_Plano_Forma_Pagamento", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Plano_Forma_Pagamento", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Txt_Formula", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Dta_Cadasrto", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idPlanoFormaPagamento;
			parms[1].Value = this._descricaoPlanoFormaPagamento;
			parms[2].Value = this._TxtFormula;
			parms[3].Value = this._dataCadasrto;
			parms[4].Value = this._flagInativo;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PlanoFormaPagamento no banco de dados.
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
		/// Método utilizado para inserir uma instância de PlanoFormaPagamento no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de PlanoFormaPagamento no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PlanoFormaPagamento no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PlanoFormaPagamento no banco de dados.
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
		/// Método utilizado para salvar uma instância de PlanoFormaPagamento no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PlanoFormaPagamento no banco de dados.
		/// </summary>
		/// <param name="idPlanoFormaPagamento">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPlanoFormaPagamento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Forma_Pagamento", SqlDbType.Int, 4));

			parms[0].Value = idPlanoFormaPagamento;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PlanoFormaPagamento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPlanoFormaPagamento">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPlanoFormaPagamento, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Forma_Pagamento", SqlDbType.Int, 4));

			parms[0].Value = idPlanoFormaPagamento;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PlanoFormaPagamento no banco de dados.
		/// </summary>
		/// <param name="idPlanoFormaPagamento">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPlanoFormaPagamento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Plano_Forma_Pagamento where Idf_Plano_Forma_Pagamento in (";

			for (int i = 0; i < idPlanoFormaPagamento.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPlanoFormaPagamento[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPlanoFormaPagamento">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPlanoFormaPagamento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Forma_Pagamento", SqlDbType.Int, 4));

			parms[0].Value = idPlanoFormaPagamento;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPlanoFormaPagamento">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPlanoFormaPagamento, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Forma_Pagamento", SqlDbType.Int, 4));

			parms[0].Value = idPlanoFormaPagamento;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano_Forma_Pagamento, Pla.Des_Plano_Forma_Pagamento, Pla.Txt_Formula, Pla.Dta_Cadasrto, Pla.Flg_Inativo FROM BNE_Plano_Forma_Pagamento Pla";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PlanoFormaPagamento a partir do banco de dados.
		/// </summary>
		/// <param name="idPlanoFormaPagamento">Chave do registro.</param>
		/// <returns>Instância de PlanoFormaPagamento.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PlanoFormaPagamento LoadObject(int idPlanoFormaPagamento)
		{
			using (IDataReader dr = LoadDataReader(idPlanoFormaPagamento))
			{
				PlanoFormaPagamento objPlanoFormaPagamento = new PlanoFormaPagamento();
				if (SetInstance(dr, objPlanoFormaPagamento))
					return objPlanoFormaPagamento;
			}
			throw (new RecordNotFoundException(typeof(PlanoFormaPagamento)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PlanoFormaPagamento a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPlanoFormaPagamento">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PlanoFormaPagamento.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PlanoFormaPagamento LoadObject(int idPlanoFormaPagamento, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPlanoFormaPagamento, trans))
			{
				PlanoFormaPagamento objPlanoFormaPagamento = new PlanoFormaPagamento();
				if (SetInstance(dr, objPlanoFormaPagamento))
					return objPlanoFormaPagamento;
			}
			throw (new RecordNotFoundException(typeof(PlanoFormaPagamento)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PlanoFormaPagamento a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPlanoFormaPagamento))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PlanoFormaPagamento a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPlanoFormaPagamento, trans))
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
		/// <param name="objPlanoFormaPagamento">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PlanoFormaPagamento objPlanoFormaPagamento)
		{
			try
			{
				if (dr.Read())
				{
					objPlanoFormaPagamento._idPlanoFormaPagamento = Convert.ToInt32(dr["Idf_Plano_Forma_Pagamento"]);
					objPlanoFormaPagamento._descricaoPlanoFormaPagamento = Convert.ToString(dr["Des_Plano_Forma_Pagamento"]);
					objPlanoFormaPagamento._TxtFormula = Convert.ToString(dr["Txt_Formula"]);
					objPlanoFormaPagamento._dataCadasrto = Convert.ToDateTime(dr["Dta_Cadasrto"]);
					objPlanoFormaPagamento._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objPlanoFormaPagamento._persisted = true;
					objPlanoFormaPagamento._modified = false;

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