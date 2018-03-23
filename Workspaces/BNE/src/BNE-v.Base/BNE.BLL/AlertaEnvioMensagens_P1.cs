//-- Data: 06/09/2013 16:34
//-- Autor: Luan Fernandes

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class AlertaEnvioMensagens // Tabela: alerta.Tab_Alerta_EnvioMensagens
	{
		#region Atributos
		private int _idCurriculo;
		private string _nomeDestinatario;
		private string _emailDestinatario;
		private string _descricaoMensagem;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCurriculo
		{
			get
			{
				return this._idCurriculo;
			}
			set
			{
				this._idCurriculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeDestinatario
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo obrigatório.
		/// </summary>
		public string NomeDestinatario
		{
			get
			{
				return this._nomeDestinatario;
			}
			set
			{
				this._nomeDestinatario = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EmailDestinatario
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo obrigatório.
		/// </summary>
		public string EmailDestinatario
		{
			get
			{
				return this._emailDestinatario;
			}
			set
			{
				this._emailDestinatario = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMensagem
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoMensagem
		{
			get
			{
				return this._descricaoMensagem;
			}
			set
			{
				this._descricaoMensagem = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public AlertaEnvioMensagens()
		{
		}
		public AlertaEnvioMensagens(int idCurriculo)
		{
			this._idCurriculo = idCurriculo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO alerta.Tab_Alerta_EnvioMensagens (Idf_Curriculo, Nme_Destinatario, Eml_Destinatario, Des_Mensagem) VALUES (@Idf_Curriculo, @Nme_Destinatario, @Eml_Destinatario, @Des_Mensagem);";
		private const string SPUPDATE = "UPDATE alerta.Tab_Alerta_EnvioMensagens SET Nme_Destinatario = @Nme_Destinatario, Eml_Destinatario = @Eml_Destinatario, Des_Mensagem = @Des_Mensagem WHERE Idf_Curriculo = @Idf_Curriculo";
		private const string SPDELETE = "DELETE FROM alerta.Tab_Alerta_EnvioMensagens WHERE Idf_Curriculo = @Idf_Curriculo";
		private const string SPSELECTID = "SELECT * FROM alerta.Tab_Alerta_EnvioMensagens WITH(NOLOCK) WHERE Idf_Curriculo = @Idf_Curriculo";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Destinatario", SqlDbType.VarChar, 500));
			parms.Add(new SqlParameter("@Eml_Destinatario", SqlDbType.VarChar, 500));
			parms.Add(new SqlParameter("@Des_Mensagem", SqlDbType.VarChar));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Luan Fernandes</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idCurriculo;
			parms[1].Value = this._nomeDestinatario;
			parms[2].Value = this._emailDestinatario;
			parms[3].Value = this._descricaoMensagem;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de AlertaEnvioMensagens no banco de dados.
		/// </summary>
		/// <remarks>Luan Fernandes</remarks>
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
		/// Método utilizado para inserir uma instância de AlertaEnvioMensagens no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
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
		/// Método utilizado para atualizar uma instância de AlertaEnvioMensagens no banco de dados.
		/// </summary>
		/// <remarks>Luan Fernandes</remarks>
		private void Update()
		{
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                SetParameters(parms);
                using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {

                            DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPUPDATE, parms);
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
		}
		/// <summary>
		/// Método utilizado para atualizar uma instância de AlertaEnvioMensagens no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
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
		/// Método utilizado para salvar uma instância de AlertaEnvioMensagens no banco de dados.
		/// </summary>
		/// <remarks>Luan Fernandes</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de AlertaEnvioMensagens no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
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
		/// Método utilizado para excluir uma instância de AlertaEnvioMensagens no banco de dados.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(int idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCurriculo;

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                }
                conn.Close();
                conn.Dispose();
            }
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de AlertaEnvioMensagens no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(int idCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCurriculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de AlertaEnvioMensagens no banco de dados.
		/// </summary>
		/// <param name="idCurriculo">Lista de chaves.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(List<int> idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from alerta.Tab_Alerta_EnvioMensagens where Idf_Curriculo in (";

			for (int i = 0; i < idCurriculo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCurriculo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static IDataReader LoadDataReader(int idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCurriculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms, DataAccessLayer.CONN_NOTIFICACAO);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static IDataReader LoadDataReader(int idCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCurriculo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, AEM.Idf_Curriculo, AEM.Nme_Destinatario, AEM.Eml_Destinatario, AEM.Des_Mensagem FROM alerta.Tab_Alerta_EnvioMensagens AEM";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null, DataAccessLayer.CONN_NOTIFICACAO);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaEnvioMensagens a partir do banco de dados.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <returns>Instância de AlertaEnvioMensagens.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public static AlertaEnvioMensagens LoadObject(int idCurriculo)
		{
			using (IDataReader dr = LoadDataReader(idCurriculo))
			{
				AlertaEnvioMensagens objAlertaEnvioMensagens = new AlertaEnvioMensagens();
				if (SetInstance(dr, objAlertaEnvioMensagens))
					return objAlertaEnvioMensagens;
			}
			throw (new RecordNotFoundException(typeof(AlertaEnvioMensagens)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaEnvioMensagens a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de AlertaEnvioMensagens.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public static AlertaEnvioMensagens LoadObject(int idCurriculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurriculo, trans))
			{
				AlertaEnvioMensagens objAlertaEnvioMensagens = new AlertaEnvioMensagens();
				if (SetInstance(dr, objAlertaEnvioMensagens))
					return objAlertaEnvioMensagens;
			}
			throw (new RecordNotFoundException(typeof(AlertaEnvioMensagens)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaEnvioMensagens a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaEnvioMensagens a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculo, trans))
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
		/// <param name="objAlertaEnvioMensagens">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static bool SetInstance(IDataReader dr, AlertaEnvioMensagens objAlertaEnvioMensagens)
		{
			try
			{
				if (dr.Read())
				{
					objAlertaEnvioMensagens._idCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
					objAlertaEnvioMensagens._nomeDestinatario = Convert.ToString(dr["Nme_Destinatario"]);
					objAlertaEnvioMensagens._emailDestinatario = Convert.ToString(dr["Eml_Destinatario"]);
					objAlertaEnvioMensagens._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);

					objAlertaEnvioMensagens._persisted = true;
					objAlertaEnvioMensagens._modified = false;

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