//-- Data: 23/07/2013 15:25
//-- Autor: Luan Fernandes

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class AlertaFuncoes // Tabela: alerta.Tab_Alerta_Funcoes
	{
		#region Atributos
		private int _idFuncao;
		private AlertaCurriculos _AlertaCurriculos;
		private string _descricaoFuncao;
		private bool _flagSimilar;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdFuncao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdFuncao
		{
			get
			{
				return this._idFuncao;
			}
			set
			{
				this._idFuncao = value;
				this._modified = true;
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

		#region DescricaoFuncao
		/// <summary>
		/// Tamanho do campo: 250.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoFuncao
		{
			get
			{
				return this._descricaoFuncao;
			}
			set
			{
				this._descricaoFuncao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagSimilar
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagSimilar
		{
			get
			{
				return this._flagSimilar;
			}
			set
			{
				this._flagSimilar = value;
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
		public AlertaFuncoes()
		{
		}
		public AlertaFuncoes(int idFuncao, AlertaCurriculos AlertaCurriculos)
		{
			this._idFuncao = idFuncao;
			this._AlertaCurriculos = AlertaCurriculos;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO alerta.Tab_Alerta_Funcoes (Idf_Funcao, Idf_Curriculo, Des_Funcao, Flg_Similar, Flg_Inativo) VALUES (@Idf_Funcao, @Idf_Curriculo, @Des_Funcao, @Flg_Similar, @Flg_Inativo);";
		private const string SPUPDATE = "UPDATE alerta.Tab_Alerta_Funcoes SET Des_Funcao = @Des_Funcao, Flg_Similar = @Flg_Similar, Flg_Inativo = @Flg_Inativo WHERE Idf_Funcao = @Idf_Funcao AND Idf_Curriculo = @Idf_Curriculo";
		private const string SPDELETE = "DELETE FROM alerta.Tab_Alerta_Funcoes WHERE Idf_Funcao = @Idf_Funcao AND Idf_Curriculo = @Idf_Curriculo";
		private const string SPSELECTID = "SELECT * FROM alerta.Tab_Alerta_Funcoes WITH(NOLOCK) WHERE Idf_Funcao = @Idf_Funcao AND Idf_Curriculo = @Idf_Curriculo";
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
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Funcao", SqlDbType.VarChar, 250));
			parms.Add(new SqlParameter("@Flg_Similar", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idFuncao;
			parms[1].Value = this._AlertaCurriculos.IdCurriculo;
			parms[2].Value = this._descricaoFuncao;
			parms[3].Value = this._flagSimilar;
			parms[4].Value = this._flagInativo;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de AlertaFuncoes no banco de dados.
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
		/// Método utilizado para inserir uma instância de AlertaFuncoes no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de AlertaFuncoes no banco de dados.
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
		/// Método utilizado para atualizar uma instância de AlertaFuncoes no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de AlertaFuncoes no banco de dados.
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
		/// Método utilizado para salvar uma instância de AlertaFuncoes no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de AlertaFuncoes no banco de dados.
		/// </summary>
		/// <param name="idFuncao">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(int idFuncao, int idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idFuncao;
			parms[1].Value = idCurriculo;

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
            {
                conn.Open();
                SqlTransaction trans;
                using (trans = conn.BeginTransaction())
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
		/// Método utilizado para excluir uma instância de AlertaFuncoes no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncao">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(int idFuncao, int idCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idFuncao;
			parms[1].Value = idCurriculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idFuncao">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static IDataReader LoadDataReader(int idFuncao, int idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idFuncao;
			parms[1].Value = idCurriculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms, DataAccessLayer.CONN_NOTIFICACAO);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncao">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static IDataReader LoadDataReader(int idFuncao, int idCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idFuncao;
			parms[1].Value = idCurriculo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, bAl.Idf_Funcao, bAl.Idf_Curriculo, bAl.Des_Funcao, bAl.Flg_Similar, bAl.Flg_Inativo FROM alerta.Tab_Alerta_Funcoes bAl";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null, DataAccessLayer.CONN_NOTIFICACAO);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaFuncoes a partir do banco de dados.
		/// </summary>
		/// <param name="idFuncao">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <returns>Instância de AlertaFuncoes.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public static AlertaFuncoes LoadObject(int idFuncao, int idCurriculo)
		{
			using (IDataReader dr = LoadDataReader(idFuncao, idCurriculo))
			{
				AlertaFuncoes objAlertaFuncoes = new AlertaFuncoes();
				if (SetInstance(dr, objAlertaFuncoes))
					return objAlertaFuncoes;
			}
			throw (new RecordNotFoundException(typeof(AlertaFuncoes)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaFuncoes a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncao">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de AlertaFuncoes.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public static AlertaFuncoes LoadObject(int idFuncao, int idCurriculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idFuncao, idCurriculo, trans))
			{
				AlertaFuncoes objAlertaFuncoes = new AlertaFuncoes();
				if (SetInstance(dr, objAlertaFuncoes))
					return objAlertaFuncoes;
			}
			throw (new RecordNotFoundException(typeof(AlertaFuncoes)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaFuncoes a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idFuncao, this._AlertaCurriculos.IdCurriculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaFuncoes a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idFuncao, this._AlertaCurriculos.IdCurriculo, trans))
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
		/// <param name="objAlertaFuncoes">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static bool SetInstance(IDataReader dr, AlertaFuncoes objAlertaFuncoes)
		{
			try
			{
				if (dr.Read())
				{
					objAlertaFuncoes._idFuncao = Convert.ToInt32(dr["Idf_Funcao"]);
					objAlertaFuncoes._AlertaCurriculos = new AlertaCurriculos(Convert.ToInt32(dr["Idf_Curriculo"]));
					objAlertaFuncoes._descricaoFuncao = Convert.ToString(dr["Des_Funcao"]);
					objAlertaFuncoes._flagSimilar = Convert.ToBoolean(dr["Flg_Similar"]);
					objAlertaFuncoes._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objAlertaFuncoes._persisted = true;
					objAlertaFuncoes._modified = false;

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