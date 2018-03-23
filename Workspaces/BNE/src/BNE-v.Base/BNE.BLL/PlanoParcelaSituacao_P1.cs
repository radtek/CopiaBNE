//-- Data: 07/07/2010 14:18
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PlanoParcelaSituacao // Tabela: BNE_Plano_Parcela_Situacao
	{
		#region Atributos
		private int _idPlanoParcelaSituacao;
		private string _descricaoStatusPagamento;
		private DateTime _datacadastro;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPlanoParcelaSituacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdPlanoParcelaSituacao
		{
			get
			{
				return this._idPlanoParcelaSituacao;
			}
			set
			{
				this._idPlanoParcelaSituacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoStatusPagamento
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoStatusPagamento
		{
			get
			{
				return this._descricaoStatusPagamento;
			}
			set
			{
				this._descricaoStatusPagamento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Datacadastro
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime Datacadastro
		{
			get
			{
				return this._datacadastro;
			}
			set
			{
				this._datacadastro = value;
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
		public PlanoParcelaSituacao()
		{
		}
		public PlanoParcelaSituacao(int idPlanoParcelaSituacao)
		{
			this._idPlanoParcelaSituacao = idPlanoParcelaSituacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Plano_Parcela_Situacao (Idf_Plano_Parcela_Situacao, Des_Status_Pagamento, Dta_cadastro, Flg_Inativo) VALUES (@Idf_Plano_Parcela_Situacao, @Des_Status_Pagamento, @Dta_cadastro, @Flg_Inativo);";
		private const string SPUPDATE = "UPDATE BNE_Plano_Parcela_Situacao SET Des_Status_Pagamento = @Des_Status_Pagamento, Dta_cadastro = @Dta_cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_Plano_Parcela_Situacao = @Idf_Plano_Parcela_Situacao";
		private const string SPDELETE = "DELETE FROM BNE_Plano_Parcela_Situacao WHERE Idf_Plano_Parcela_Situacao = @Idf_Plano_Parcela_Situacao";
		private const string SPSELECTID = "SELECT * FROM BNE_Plano_Parcela_Situacao WHERE Idf_Plano_Parcela_Situacao = @Idf_Plano_Parcela_Situacao";
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
			parms.Add(new SqlParameter("@Idf_Plano_Parcela_Situacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Status_Pagamento", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Dta_cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idPlanoParcelaSituacao;
			parms[1].Value = this._descricaoStatusPagamento;
			parms[2].Value = this._datacadastro;
			parms[3].Value = this._flagInativo;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PlanoParcelaSituacao no banco de dados.
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
		/// Método utilizado para inserir uma instância de PlanoParcelaSituacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de PlanoParcelaSituacao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PlanoParcelaSituacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PlanoParcelaSituacao no banco de dados.
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
		/// Método utilizado para salvar uma instância de PlanoParcelaSituacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PlanoParcelaSituacao no banco de dados.
		/// </summary>
		/// <param name="idPlanoParcelaSituacao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPlanoParcelaSituacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Parcela_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idPlanoParcelaSituacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PlanoParcelaSituacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPlanoParcelaSituacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPlanoParcelaSituacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Parcela_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idPlanoParcelaSituacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PlanoParcelaSituacao no banco de dados.
		/// </summary>
		/// <param name="idPlanoParcelaSituacao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPlanoParcelaSituacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Plano_Parcela_Situacao where Idf_Plano_Parcela_Situacao in (";

			for (int i = 0; i < idPlanoParcelaSituacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPlanoParcelaSituacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPlanoParcelaSituacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPlanoParcelaSituacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Parcela_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idPlanoParcelaSituacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPlanoParcelaSituacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPlanoParcelaSituacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plano_Parcela_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idPlanoParcelaSituacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano_Parcela_Situacao, Pla.Des_Status_Pagamento, Pla.Dta_cadastro, Pla.Flg_Inativo FROM BNE_Plano_Parcela_Situacao Pla";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PlanoParcelaSituacao a partir do banco de dados.
		/// </summary>
		/// <param name="idPlanoParcelaSituacao">Chave do registro.</param>
		/// <returns>Instância de PlanoParcelaSituacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PlanoParcelaSituacao LoadObject(int idPlanoParcelaSituacao)
		{
			using (IDataReader dr = LoadDataReader(idPlanoParcelaSituacao))
			{
				PlanoParcelaSituacao objPlanoParcelaSituacao = new PlanoParcelaSituacao();
				if (SetInstance(dr, objPlanoParcelaSituacao))
					return objPlanoParcelaSituacao;
			}
			throw (new RecordNotFoundException(typeof(PlanoParcelaSituacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PlanoParcelaSituacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPlanoParcelaSituacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PlanoParcelaSituacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PlanoParcelaSituacao LoadObject(int idPlanoParcelaSituacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPlanoParcelaSituacao, trans))
			{
				PlanoParcelaSituacao objPlanoParcelaSituacao = new PlanoParcelaSituacao();
				if (SetInstance(dr, objPlanoParcelaSituacao))
					return objPlanoParcelaSituacao;
			}
			throw (new RecordNotFoundException(typeof(PlanoParcelaSituacao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PlanoParcelaSituacao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPlanoParcelaSituacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PlanoParcelaSituacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPlanoParcelaSituacao, trans))
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
		/// <param name="objPlanoParcelaSituacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PlanoParcelaSituacao objPlanoParcelaSituacao)
		{
			try
			{
				if (dr.Read())
				{
					objPlanoParcelaSituacao._idPlanoParcelaSituacao = Convert.ToInt32(dr["Idf_Plano_Parcela_Situacao"]);
					objPlanoParcelaSituacao._descricaoStatusPagamento = Convert.ToString(dr["Des_Status_Pagamento"]);
					objPlanoParcelaSituacao._datacadastro = Convert.ToDateTime(dr["Dta_cadastro"]);
					objPlanoParcelaSituacao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objPlanoParcelaSituacao._persisted = true;
					objPlanoParcelaSituacao._modified = false;

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