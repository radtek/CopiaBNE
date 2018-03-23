//-- Data: 23/07/2013 15:25
//-- Autor: Luan Fernandes

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL.Notificacao
{
	public partial class AlertaCidades // Tabela: alerta.Tab_Alerta_Cidades
	{
		#region Atributos
		private AlertaCurriculos _AlertaCurriculos;
		private int _idCidade;
		private string _nomeCidade;
		private string _siglaEstado;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

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

		#region IdCidade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCidade
		{
			get
			{
				return this._idCidade;
			}
			set
			{
				this._idCidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeCidade
		/// <summary>
		/// Tamanho do campo: 250.
		/// Campo obrigatório.
		/// </summary>
		public string NomeCidade
		{
			get
			{
				return this._nomeCidade;
			}
			set
			{
				this._nomeCidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region SiglaEstado
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo obrigatório.
		/// </summary>
		public string SiglaEstado
		{
			get
			{
				return this._siglaEstado;
			}
			set
			{
				this._siglaEstado = value;
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
		public AlertaCidades()
		{
		}
		public AlertaCidades(AlertaCurriculos AlertaCurriculos, int idCidade)
		{
			this._AlertaCurriculos = AlertaCurriculos;
			this._idCidade = idCidade;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO alerta.Tab_Alerta_Cidades (Idf_Curriculo, Idf_Cidade, Nme_Cidade, Sig_Estado, Flg_Inativo) VALUES (@Idf_Curriculo, @Idf_Cidade, @Nme_Cidade, @Sig_Estado, @Flg_Inativo);";
		private const string SPUPDATE = "UPDATE alerta.Tab_Alerta_Cidades SET Nme_Cidade = @Nme_Cidade, Sig_Estado = @Sig_Estado, Flg_Inativo = @Flg_Inativo WHERE Idf_Curriculo = @Idf_Curriculo AND Idf_Cidade = @Idf_Cidade";
        //private const string SPDELETE = "DELETE FROM alerta.Tab_Alerta_Cidades WHERE Idf_Curriculo = @Idf_Curriculo AND Idf_Cidade = @Idf_Cidade";
        private const string SPDELETE = "UPDATE alerta.Tab_Alerta_Cidades SET Flg_inativo = 1 WHERE Idf_Curriculo = @Idf_Curriculo AND Idf_Cidade = @Idf_Cidade";
		private const string SPSELECTID = "SELECT * FROM alerta.Tab_Alerta_Cidades WITH(NOLOCK) WHERE Idf_Curriculo = @Idf_Curriculo AND Idf_Cidade = @Idf_Cidade";
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
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Cidade", SqlDbType.VarChar, 250));
			parms.Add(new SqlParameter("@Sig_Estado", SqlDbType.Char, 2));
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
			parms[0].Value = this._AlertaCurriculos.IdCurriculo;
			parms[1].Value = this._idCidade;
			parms[2].Value = this._nomeCidade;
			parms[3].Value = this._siglaEstado;
			parms[4].Value = this._flagInativo;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de AlertaCidades no banco de dados.
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
		/// Método utilizado para inserir uma instância de AlertaCidades no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de AlertaCidades no banco de dados.
		/// </summary>
		/// <remarks>Luan Fernandes</remarks>
		private void Update()
		{
			if (this._modified)
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
		/// Método utilizado para atualizar uma instância de AlertaCidades no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de AlertaCidades no banco de dados.
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
		/// Método utilizado para salvar uma instância de AlertaCidades no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de AlertaCidades no banco de dados.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(int idCurriculo, int idCidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idCurriculo;
			parms[1].Value = idCidade;

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DataAccessLayer.ExecuteNonQuery(trans,CommandType.Text, SPDELETE, parms);
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
		/// Método utilizado para excluir uma instância de AlertaCidades no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(int idCurriculo, int idCidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idCurriculo;
			parms[1].Value = idCidade;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static IDataReader LoadDataReader(int idCurriculo, int idCidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idCurriculo;
			parms[1].Value = idCidade;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms, DataAccessLayer.CONN_STRING);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static IDataReader LoadDataReader(int idCurriculo, int idCidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idCurriculo;
			parms[1].Value = idCidade;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, bAl.Idf_Curriculo, bAl.Idf_Cidade, bAl.Nme_Cidade, bAl.Sig_Estado, bAl.Flg_Inativo FROM alerta.Tab_Alerta_Cidades bAl WITH(NOLOCK) ";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null, DataAccessLayer.CONN_STRING);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaCidades a partir do banco de dados.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <returns>Instância de AlertaCidades.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public static AlertaCidades LoadObject(int idCurriculo, int idCidade)
		{
			using (IDataReader dr = LoadDataReader(idCurriculo, idCidade))
			{
				AlertaCidades objAlertaCidades = new AlertaCidades();
				if (SetInstance(dr, objAlertaCidades))
					return objAlertaCidades;
			}
			throw (new RecordNotFoundException(typeof(AlertaCidades)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaCidades a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de AlertaCidades.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public static AlertaCidades LoadObject(int idCurriculo, int idCidade, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurriculo, idCidade, trans))
			{
				AlertaCidades objAlertaCidades = new AlertaCidades();
				if (SetInstance(dr, objAlertaCidades))
					return objAlertaCidades;
			}
			throw (new RecordNotFoundException(typeof(AlertaCidades)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaCidades a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._AlertaCurriculos.IdCurriculo, this._idCidade))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaCidades a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._AlertaCurriculos.IdCurriculo, this._idCidade, trans))
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
		/// <param name="objAlertaCidades">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static bool SetInstance(IDataReader dr, AlertaCidades objAlertaCidades)
		{
			try
			{
				if (dr.Read())
				{
					objAlertaCidades._AlertaCurriculos = new AlertaCurriculos(Convert.ToInt32(dr["Idf_Curriculo"]));
					objAlertaCidades._idCidade = Convert.ToInt32(dr["Idf_Cidade"]);
					objAlertaCidades._nomeCidade = Convert.ToString(dr["Nme_Cidade"]);
					objAlertaCidades._siglaEstado = Convert.ToString(dr["Sig_Estado"]);
					objAlertaCidades._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objAlertaCidades._persisted = true;
					objAlertaCidades._modified = false;

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