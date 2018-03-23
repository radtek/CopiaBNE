//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PagamentoSituacao // Tabela: BNE_Pagamento_Situacao
	{
		#region Atributos
		private int _idPagamentoSituacao;
		private string _descricaoPagamentoSituacao;
		private DateTime _dataCadastro;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPagamentoSituacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdPagamentoSituacao
		{
			get
			{
				return this._idPagamentoSituacao;
			}
			set
			{
				this._idPagamentoSituacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPagamentoSituacao
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoPagamentoSituacao
		{
			get
			{
				return this._descricaoPagamentoSituacao;
			}
			set
			{
				this._descricaoPagamentoSituacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCadastro
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataCadastro
		{
			get
			{
				return this._dataCadastro;
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
		public PagamentoSituacao()
		{
		}
		public PagamentoSituacao(int idPagamentoSituacao)
		{
			this._idPagamentoSituacao = idPagamentoSituacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Pagamento_Situacao (Idf_Pagamento_Situacao, Des_Pagamento_Situacao, Dta_Cadastro, Flg_Inativo) VALUES (@Idf_Pagamento_Situacao, @Des_Pagamento_Situacao, @Dta_Cadastro, @Flg_Inativo);";
		private const string SPUPDATE = "UPDATE BNE_Pagamento_Situacao SET Des_Pagamento_Situacao = @Des_Pagamento_Situacao, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_Pagamento_Situacao = @Idf_Pagamento_Situacao";
		private const string SPDELETE = "DELETE FROM BNE_Pagamento_Situacao WHERE Idf_Pagamento_Situacao = @Idf_Pagamento_Situacao";
		private const string SPSELECTID = "SELECT * FROM BNE_Pagamento_Situacao WHERE Idf_Pagamento_Situacao = @Idf_Pagamento_Situacao";
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
			parms.Add(new SqlParameter("@Idf_Pagamento_Situacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Pagamento_Situacao", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idPagamentoSituacao;
			parms[1].Value = this._descricaoPagamentoSituacao;
			parms[3].Value = this._flagInativo;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PagamentoSituacao no banco de dados.
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
		/// Método utilizado para inserir uma instância de PagamentoSituacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de PagamentoSituacao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PagamentoSituacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PagamentoSituacao no banco de dados.
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
		/// Método utilizado para salvar uma instância de PagamentoSituacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PagamentoSituacao no banco de dados.
		/// </summary>
		/// <param name="idPagamentoSituacao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPagamentoSituacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pagamento_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idPagamentoSituacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PagamentoSituacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPagamentoSituacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPagamentoSituacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pagamento_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idPagamentoSituacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PagamentoSituacao no banco de dados.
		/// </summary>
		/// <param name="idPagamentoSituacao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPagamentoSituacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Pagamento_Situacao where Idf_Pagamento_Situacao in (";

			for (int i = 0; i < idPagamentoSituacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPagamentoSituacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPagamentoSituacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPagamentoSituacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pagamento_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idPagamentoSituacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPagamentoSituacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPagamentoSituacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pagamento_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idPagamentoSituacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pag.Idf_Pagamento_Situacao, Pag.Des_Pagamento_Situacao, Pag.Dta_Cadastro, Pag.Flg_Inativo FROM BNE_Pagamento_Situacao Pag";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PagamentoSituacao a partir do banco de dados.
		/// </summary>
		/// <param name="idPagamentoSituacao">Chave do registro.</param>
		/// <returns>Instância de PagamentoSituacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PagamentoSituacao LoadObject(int idPagamentoSituacao)
		{
			using (IDataReader dr = LoadDataReader(idPagamentoSituacao))
			{
				PagamentoSituacao objPagamentoSituacao = new PagamentoSituacao();
				if (SetInstance(dr, objPagamentoSituacao))
					return objPagamentoSituacao;
			}
			throw (new RecordNotFoundException(typeof(PagamentoSituacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PagamentoSituacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPagamentoSituacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PagamentoSituacao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PagamentoSituacao LoadObject(int idPagamentoSituacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPagamentoSituacao, trans))
			{
				PagamentoSituacao objPagamentoSituacao = new PagamentoSituacao();
				if (SetInstance(dr, objPagamentoSituacao))
					return objPagamentoSituacao;
			}
			throw (new RecordNotFoundException(typeof(PagamentoSituacao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PagamentoSituacao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPagamentoSituacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PagamentoSituacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPagamentoSituacao, trans))
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
		/// <param name="objPagamentoSituacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PagamentoSituacao objPagamentoSituacao)
		{
			try
			{
				if (dr.Read())
				{
					objPagamentoSituacao._idPagamentoSituacao = Convert.ToInt32(dr["Idf_Pagamento_Situacao"]);
					objPagamentoSituacao._descricaoPagamentoSituacao = Convert.ToString(dr["Des_Pagamento_Situacao"]);
					objPagamentoSituacao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objPagamentoSituacao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objPagamentoSituacao._persisted = true;
					objPagamentoSituacao._modified = false;

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