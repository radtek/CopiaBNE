//-- Data: 16/11/2010 15:04
//-- Autor: BNE

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class EmailSituacao // Tabela: plataforma.TAB_Email_Situacao
	{
		#region Atributos
		private int _idEmailSituacao;
		private string _descricaoEmailSituacao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdEmailSituacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdEmailSituacao
		{
			get
			{
				return this._idEmailSituacao;
			}
			set
			{
				this._idEmailSituacao = value;
				this._modified = true;
			}
		}
		#endregion 


        #region DescricaoFuncao
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoEmailSituacao
		{
			get
			{
				return this._descricaoEmailSituacao;
			}
			set
			{
				this._descricaoEmailSituacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public EmailSituacao()
		{
		}
		public EmailSituacao(int idEmailSituacao)
		{
			this._idEmailSituacao = idEmailSituacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Email_Situacao (Idf_Email_Situacao, Des_Email_Situacao);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Email_Situacao SET Idf_Email_Situacao = @Idf_Email_Situacao, Des_Email_Situacao = @Des_Email_Situacao WHERE Idf_Email_Situacao = @Idf_Email_Situacao";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Email_Situacao WHERE Idf_Email_Situacao = @Idf_Email_Situacao";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Email_Situacao WHERE Idf_Email_Situacao = @Idf_Email_Situacao";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Situacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Email_Situacao", SqlDbType.VarChar, 50));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idEmailSituacao;
			parms[1].Value = this._descricaoEmailSituacao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Funcao no banco de dados.
		/// </summary>
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
		/// Método utilizado para inserir uma instância de Funcao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
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
		/// Método utilizado para atualizar uma instância de Funcao no banco de dados.
		/// </summary>
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
		/// Método utilizado para atualizar uma instância de Funcao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
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
		/// Método utilizado para salvar uma instância de Funcao no banco de dados.
		/// </summary>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de Funcao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
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
		/// Método utilizado para excluir uma instância de Funcao no banco de dados.
		/// </summary>
		/// <param name="idEmailSituacao">Chave do registro.</param>
		public static void Delete(int idEmailSituacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idEmailSituacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Funcao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEmailSituacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		public static void Delete(int idEmailSituacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idEmailSituacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Funcao no banco de dados.
		/// </summary>
		/// <param name="idEmailSituacao">Lista de chaves.</param>
		public static void Delete(List<int> idEmailSituacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Email_Situacao where Idf_Email_Situacao in (";

			for (int i = 0; i < idEmailSituacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idEmailSituacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idEmailSituacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		private static IDataReader LoadDataReader(int idEmailSituacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idEmailSituacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEmailSituacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		private static IDataReader LoadDataReader(int idEmailSituacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Situacao", SqlDbType.Int, 4));

			parms[0].Value = idEmailSituacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ema.Idf_Email_Situacao, Ema.Des_Email_Situacao FROM plataforma.TAB_Email_Situacao Ema";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Email_Situacao a partir do banco de dados.
        /// </summary>
        /// <param name="idEmailSituacao">Chave do registro.</param>
        /// <returns>Instância de Email_Situacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static EmailSituacao LoadObject(int idEmailSituacao)
        {
            using (IDataReader dr = LoadDataReader(idEmailSituacao))
            {
                EmailSituacao objEmailSituacao = new EmailSituacao();
                if (SetInstance(dr, objEmailSituacao))
                    return objEmailSituacao;
            }
            throw (new RecordNotFoundException(typeof(EmailSituacao)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Estado a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idEstado">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Email_Situacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static EmailSituacao LoadObject(int idEmailSituacao, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idEmailSituacao, trans))
            {
                EmailSituacao objEmailSituacao = new EmailSituacao();
                if (SetInstance(dr, objEmailSituacao))
                    return objEmailSituacao;
            }
            throw (new RecordNotFoundException(typeof(EmailSituacao)));
        }
        #endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objEmailSituacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		private static bool SetInstance(IDataReader dr, EmailSituacao objEmailSituacao)
		{
			try
			{
				if (dr.Read())
				{
					objEmailSituacao._idEmailSituacao = Convert.ToInt32(dr["Idf_Email_Situacao"]);
					objEmailSituacao._descricaoEmailSituacao = Convert.ToString(dr["Des_Email_Situacao"]);
					objEmailSituacao._persisted = true;
					objEmailSituacao._modified = false;

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