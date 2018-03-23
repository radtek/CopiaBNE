//-- Data: 05/09/2013 11:45
//-- Autor: Luan Fernandes

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class AlertaEmpresasExecao // Tabela: alerta.Tab_Alerta_EmpresasExecao
	{
		#region Atributos
		private int _idEmpresasExcecao;
		private AlertaCurriculos _AlertaCurriculos;
		private int? _idEmpresa;
		private string _descricaoRazaoSocialEmpresa;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdEmpresasExcecao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdEmpresasExcecao
		{
			get
			{
				return this._idEmpresasExcecao;
			}
		}
		#endregion 

		#region AlertaCurriculos
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public AlertaCurriculos AlertaCurriculos
		{
			get
			{
				return this._AlertaCurriculos;
			}
			set
			{
				this._AlertaCurriculos = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdEmpresa
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? IdEmpresa
		{
			get
			{
				return this._idEmpresa;
			}
			set
			{
				this._idEmpresa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoRazaoSocialEmpresa
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoRazaoSocialEmpresa
		{
			get
			{
				return this._descricaoRazaoSocialEmpresa;
			}
			set
			{
				this._descricaoRazaoSocialEmpresa = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public AlertaEmpresasExecao()
		{
		}
		public AlertaEmpresasExecao(int idEmpresasExcecao)
		{
			this._idEmpresasExcecao = idEmpresasExcecao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO alerta.Tab_Alerta_EmpresasExecao (Idf_Curriculo, Idf_Empresa, Des_RazaoSocial_Empresa) VALUES (@Idf_Curriculo, @Idf_Empresa, @Des_RazaoSocial_Empresa);SET @Idf_EmpresasExcecao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE alerta.Tab_Alerta_EmpresasExecao SET Idf_Curriculo = @Idf_Curriculo, Idf_Empresa = @Idf_Empresa, Des_RazaoSocial_Empresa = @Des_RazaoSocial_Empresa WHERE Idf_EmpresasExcecao = @Idf_EmpresasExcecao";
		private const string SPDELETE = "DELETE FROM alerta.Tab_Alerta_EmpresasExecao WHERE Idf_EmpresasExcecao = @Idf_EmpresasExcecao";
		private const string SPSELECTID = "SELECT * FROM alerta.Tab_Alerta_EmpresasExecao WITH(NOLOCK) WHERE Idf_EmpresasExcecao = @Idf_EmpresasExcecao";
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
			parms.Add(new SqlParameter("@Idf_EmpresasExcecao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Empresa", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_RazaoSocial_Empresa", SqlDbType.VarChar, 500));
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
			parms[0].Value = this._idEmpresasExcecao;
			parms[1].Value = this._AlertaCurriculos.IdCurriculo;

			if (this._idEmpresa.HasValue)
				parms[2].Value = this._idEmpresa;
			else
				parms[2].Value = DBNull.Value;

			parms[3].Value = this._descricaoRazaoSocialEmpresa;

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
		/// Método utilizado para inserir uma instância de AlertaEmpresasExecao no banco de dados.
		/// </summary>
		/// <remarks>Luan Fernandes</remarks>
		private void Insert()
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
						SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
						this._idEmpresasExcecao = Convert.ToInt32(cmd.Parameters["@Idf_EmpresasExcecao"].Value);
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
		/// Método utilizado para inserir uma instância de AlertaEmpresasExecao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idEmpresasExcecao = Convert.ToInt32(cmd.Parameters["@Idf_EmpresasExcecao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de AlertaEmpresasExecao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de AlertaEmpresasExecao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de AlertaEmpresasExecao no banco de dados.
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
		/// Método utilizado para salvar uma instância de AlertaEmpresasExecao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de AlertaEmpresasExecao no banco de dados.
		/// </summary>
		/// <param name="idEmpresasExcecao">Chave do registro.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(int idEmpresasExcecao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_EmpresasExcecao", SqlDbType.Int, 4));

			parms[0].Value = idEmpresasExcecao;

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
                        throw;
                        
                    }
                }
                conn.Close();
                conn.Dispose();
            }
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de AlertaEmpresasExecao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEmpresasExcecao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(int idEmpresasExcecao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_EmpresasExcecao", SqlDbType.Int, 4));

			parms[0].Value = idEmpresasExcecao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de AlertaEmpresasExecao no banco de dados.
		/// </summary>
		/// <param name="idEmpresasExcecao">Lista de chaves.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(List<int> idEmpresasExcecao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from alerta.Tab_Alerta_EmpresasExecao where Idf_EmpresasExcecao in (";

			for (int i = 0; i < idEmpresasExcecao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idEmpresasExcecao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idEmpresasExcecao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static IDataReader LoadDataReader(int idEmpresasExcecao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_EmpresasExcecao", SqlDbType.Int, 4));

			parms[0].Value = idEmpresasExcecao;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms, DataAccessLayer.CONN_NOTIFICACAO);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEmpresasExcecao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static IDataReader LoadDataReader(int idEmpresasExcecao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_EmpresasExcecao", SqlDbType.Int, 4));

			parms[0].Value = idEmpresasExcecao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, EE.Idf_EmpresasExcecao, EE.Idf_Curriculo, EE.Idf_Empresa, EE.Des_RazaoSocial_Empresa FROM alerta.Tab_Alerta_EmpresasExecao EE";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null, DataAccessLayer.CONN_NOTIFICACAO);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaEmpresasExecao a partir do banco de dados.
		/// </summary>
		/// <param name="idEmpresasExcecao">Chave do registro.</param>
		/// <returns>Instância de AlertaEmpresasExecao.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public static AlertaEmpresasExecao LoadObject(int idEmpresasExcecao)
		{
			using (IDataReader dr = LoadDataReader(idEmpresasExcecao))
			{
				AlertaEmpresasExecao objAlertaEmpresasExecao = new AlertaEmpresasExecao();
				if (SetInstance(dr, objAlertaEmpresasExecao))
					return objAlertaEmpresasExecao;
			}
			throw (new RecordNotFoundException(typeof(AlertaEmpresasExecao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaEmpresasExecao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEmpresasExcecao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de AlertaEmpresasExecao.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public static AlertaEmpresasExecao LoadObject(int idEmpresasExcecao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idEmpresasExcecao, trans))
			{
				AlertaEmpresasExecao objAlertaEmpresasExecao = new AlertaEmpresasExecao();
				if (SetInstance(dr, objAlertaEmpresasExecao))
					return objAlertaEmpresasExecao;
			}
			throw (new RecordNotFoundException(typeof(AlertaEmpresasExecao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaEmpresasExecao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idEmpresasExcecao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaEmpresasExecao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idEmpresasExcecao, trans))
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
		/// <param name="objAlertaEmpresasExecao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static bool SetInstance(IDataReader dr, AlertaEmpresasExecao objAlertaEmpresasExecao)
		{
			try
			{
				if (dr.Read())
				{
					objAlertaEmpresasExecao._idEmpresasExcecao = Convert.ToInt32(dr["Idf_EmpresasExcecao"]);
					objAlertaEmpresasExecao._AlertaCurriculos = new AlertaCurriculos(Convert.ToInt32(dr["Idf_Curriculo"]));
					if (dr["Idf_Empresa"] != DBNull.Value)
						objAlertaEmpresasExecao._idEmpresa = Convert.ToInt32(dr["Idf_Empresa"]);
					objAlertaEmpresasExecao._descricaoRazaoSocialEmpresa = Convert.ToString(dr["Des_RazaoSocial_Empresa"]);

					objAlertaEmpresasExecao._persisted = true;
					objAlertaEmpresasExecao._modified = false;

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