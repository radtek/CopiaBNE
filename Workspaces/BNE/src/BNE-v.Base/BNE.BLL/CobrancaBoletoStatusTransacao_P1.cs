//-- Data: 02/05/2014 15:19
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CobrancaBoletoStatusTransacao // Tabela: GLO_Status_Transacao
	{
		#region Atributos
		private int _idCobrancaBoletoStatusTransacao;
		private string _descricaoCobrancaBoletoStatusTransacao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCobrancaBoletoStatusTransacao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCobrancaBoletoStatusTransacao
		{
			get
			{
				return this._idCobrancaBoletoStatusTransacao;
			}
		}
		#endregion 

		#region DescricaoCobrancaBoletoStatusTransacao
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCobrancaBoletoStatusTransacao
		{
			get
			{
				return this._descricaoCobrancaBoletoStatusTransacao;
			}
			set
			{
				this._descricaoCobrancaBoletoStatusTransacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CobrancaBoletoStatusTransacao()
		{
		}
		public CobrancaBoletoStatusTransacao(int idCobrancaBoletoStatusTransacao)
		{
			this._idCobrancaBoletoStatusTransacao = idCobrancaBoletoStatusTransacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO GLO_Status_Transacao (Des_Status_Transacao) VALUES (@Des_Status_Transacao);SET @Idf_Status_Transacao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE GLO_Status_Transacao SET Des_Status_Transacao = @Des_Status_Transacao WHERE Idf_Status_Transacao = @Idf_Status_Transacao";
		private const string SPDELETE = "DELETE FROM GLO_Status_Transacao WHERE Idf_Status_Transacao = @Idf_Status_Transacao";
		private const string SPSELECTID = "SELECT * FROM GLO_Status_Transacao WITH(NOLOCK) WHERE Idf_Status_Transacao = @Idf_Status_Transacao";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Transacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Status_Transacao", SqlDbType.VarChar, 50));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idCobrancaBoletoStatusTransacao;

			if (!String.IsNullOrEmpty(this._descricaoCobrancaBoletoStatusTransacao))
				parms[1].Value = this._descricaoCobrancaBoletoStatusTransacao;
			else
				parms[1].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CobrancaBoletoStatusTransacao no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
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
						this._idCobrancaBoletoStatusTransacao = Convert.ToInt32(cmd.Parameters["@Idf_Status_Transacao"].Value);
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
		/// Método utilizado para inserir uma instância de CobrancaBoletoStatusTransacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCobrancaBoletoStatusTransacao = Convert.ToInt32(cmd.Parameters["@Idf_Status_Transacao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CobrancaBoletoStatusTransacao no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para atualizar uma instância de CobrancaBoletoStatusTransacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para salvar uma instância de CobrancaBoletoStatusTransacao no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de CobrancaBoletoStatusTransacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para excluir uma instância de CobrancaBoletoStatusTransacao no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoStatusTransacao">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoStatusTransacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Transacao", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoStatusTransacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CobrancaBoletoStatusTransacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoStatusTransacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoStatusTransacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Transacao", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoStatusTransacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CobrancaBoletoStatusTransacao no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoStatusTransacao">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idCobrancaBoletoStatusTransacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from GLO_Status_Transacao where Idf_Status_Transacao in (";

			for (int i = 0; i < idCobrancaBoletoStatusTransacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCobrancaBoletoStatusTransacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoStatusTransacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoStatusTransacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Transacao", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoStatusTransacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoStatusTransacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoStatusTransacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Transacao", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoStatusTransacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sta.Idf_Status_Transacao, Sta.Des_Status_Transacao FROM GLO_Status_Transacao Sta";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoStatusTransacao a partir do banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoStatusTransacao">Chave do registro.</param>
		/// <returns>Instância de CobrancaBoletoStatusTransacao.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoStatusTransacao LoadObject(int idCobrancaBoletoStatusTransacao)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoStatusTransacao))
			{
				CobrancaBoletoStatusTransacao objCobrancaBoletoStatusTransacao = new CobrancaBoletoStatusTransacao();
				if (SetInstance(dr, objCobrancaBoletoStatusTransacao))
					return objCobrancaBoletoStatusTransacao;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoStatusTransacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoStatusTransacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoStatusTransacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CobrancaBoletoStatusTransacao.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoStatusTransacao LoadObject(int idCobrancaBoletoStatusTransacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoStatusTransacao, trans))
			{
				CobrancaBoletoStatusTransacao objCobrancaBoletoStatusTransacao = new CobrancaBoletoStatusTransacao();
				if (SetInstance(dr, objCobrancaBoletoStatusTransacao))
					return objCobrancaBoletoStatusTransacao;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoStatusTransacao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoStatusTransacao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoStatusTransacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoStatusTransacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoStatusTransacao, trans))
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
		/// <param name="objCobrancaBoletoStatusTransacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, CobrancaBoletoStatusTransacao objCobrancaBoletoStatusTransacao)
		{
			try
			{
				if (dr.Read())
				{
					objCobrancaBoletoStatusTransacao._idCobrancaBoletoStatusTransacao = Convert.ToInt32(dr["Idf_Status_Transacao"]);
					if (dr["Des_Status_Transacao"] != DBNull.Value)
						objCobrancaBoletoStatusTransacao._descricaoCobrancaBoletoStatusTransacao = Convert.ToString(dr["Des_Status_Transacao"]);

					objCobrancaBoletoStatusTransacao._persisted = true;
					objCobrancaBoletoStatusTransacao._modified = false;

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