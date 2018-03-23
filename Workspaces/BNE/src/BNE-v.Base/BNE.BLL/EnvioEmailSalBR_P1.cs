//-- Data: 12/09/2011 15:25
//-- Autor: Elias

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class EnvioEmailSalBR // Tabela: TAB_Envio_Email_Sal_BR
	{
		#region Atributos
		private int _idEnvioEmailSalBR;
		private string _descricaoEmail;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdEnvioEmailSalBR
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdEnvioEmailSalBR
		{
			get
			{
				return this._idEnvioEmailSalBR;
			}
		}
		#endregion 

		#region DescricaoEmail
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoEmail
		{
			get
			{
				return this._descricaoEmail;
			}
			set
			{
				this._descricaoEmail = value;
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
            set
			{
				_dataCadastro = value;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public EnvioEmailSalBR()
		{
		}
		public EnvioEmailSalBR(int idEnvioEmailSalBR)
		{
			this._idEnvioEmailSalBR = idEnvioEmailSalBR;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Envio_Email_Sal_BR (Des_Email, Dta_Cadastro) VALUES (@Des_Email, @Dta_Cadastro);SET @Idf_Envio_Email_Sal_BR = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Envio_Email_Sal_BR SET Des_Email = @Des_Email, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Envio_Email_Sal_BR = @Idf_Envio_Email_Sal_BR";
		private const string SPDELETE = "DELETE FROM TAB_Envio_Email_Sal_BR WHERE Idf_Envio_Email_Sal_BR = @Idf_Envio_Email_Sal_BR";
		private const string SPSELECTID = "SELECT * FROM TAB_Envio_Email_Sal_BR WHERE Idf_Envio_Email_Sal_BR = @Idf_Envio_Email_Sal_BR";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Elias</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Envio_Email_Sal_BR", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Email", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Elias</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idEnvioEmailSalBR;
			parms[1].Value = this._descricaoEmail;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de EnvioEmailSalBR no banco de dados.
		/// </summary>
		/// <remarks>Elias</remarks>
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
						this._idEnvioEmailSalBR = Convert.ToInt32(cmd.Parameters["@Idf_Envio_Email_Sal_BR"].Value);
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
		/// Método utilizado para inserir uma instância de EnvioEmailSalBR no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idEnvioEmailSalBR = Convert.ToInt32(cmd.Parameters["@Idf_Envio_Email_Sal_BR"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de EnvioEmailSalBR no banco de dados.
		/// </summary>
		/// <remarks>Elias</remarks>
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
		/// Método utilizado para atualizar uma instância de EnvioEmailSalBR no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias</remarks>
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
		/// Método utilizado para salvar uma instância de EnvioEmailSalBR no banco de dados.
		/// </summary>
		/// <remarks>Elias</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de EnvioEmailSalBR no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias</remarks>
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
		/// Método utilizado para excluir uma instância de EnvioEmailSalBR no banco de dados.
		/// </summary>
		/// <param name="idEnvioEmailSalBR">Chave do registro.</param>
		/// <remarks>Elias</remarks>
		public static void Delete(int idEnvioEmailSalBR)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Envio_Email_Sal_BR", SqlDbType.Int, 4));

			parms[0].Value = idEnvioEmailSalBR;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de EnvioEmailSalBR no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEnvioEmailSalBR">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Elias</remarks>
		public static void Delete(int idEnvioEmailSalBR, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Envio_Email_Sal_BR", SqlDbType.Int, 4));

			parms[0].Value = idEnvioEmailSalBR;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de EnvioEmailSalBR no banco de dados.
		/// </summary>
		/// <param name="idEnvioEmailSalBR">Lista de chaves.</param>
		/// <remarks>Elias</remarks>
		public static void Delete(List<int> idEnvioEmailSalBR)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Envio_Email_Sal_BR where Idf_Envio_Email_Sal_BR in (";

			for (int i = 0; i < idEnvioEmailSalBR.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idEnvioEmailSalBR[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idEnvioEmailSalBR">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Elias</remarks>
		private static IDataReader LoadDataReader(int idEnvioEmailSalBR)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Envio_Email_Sal_BR", SqlDbType.Int, 4));

			parms[0].Value = idEnvioEmailSalBR;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEnvioEmailSalBR">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Elias</remarks>
		private static IDataReader LoadDataReader(int idEnvioEmailSalBR, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Envio_Email_Sal_BR", SqlDbType.Int, 4));

			parms[0].Value = idEnvioEmailSalBR;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Env.Idf_Envio_Email_Sal_BR, Env.Des_Email, Env.Dta_Cadastro FROM TAB_Envio_Email_Sal_BR Env";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de EnvioEmailSalBR a partir do banco de dados.
		/// </summary>
		/// <param name="idEnvioEmailSalBR">Chave do registro.</param>
		/// <returns>Instância de EnvioEmailSalBR.</returns>
		/// <remarks>Elias</remarks>
		public static EnvioEmailSalBR LoadObject(int idEnvioEmailSalBR)
		{
			using (IDataReader dr = LoadDataReader(idEnvioEmailSalBR))
			{
				EnvioEmailSalBR objEnvioEmailSalBR = new EnvioEmailSalBR();
				if (SetInstance(dr, objEnvioEmailSalBR))
					return objEnvioEmailSalBR;
			}
			throw (new RecordNotFoundException(typeof(EnvioEmailSalBR)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de EnvioEmailSalBR a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEnvioEmailSalBR">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de EnvioEmailSalBR.</returns>
		/// <remarks>Elias</remarks>
		public static EnvioEmailSalBR LoadObject(int idEnvioEmailSalBR, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idEnvioEmailSalBR, trans))
			{
				EnvioEmailSalBR objEnvioEmailSalBR = new EnvioEmailSalBR();
				if (SetInstance(dr, objEnvioEmailSalBR))
					return objEnvioEmailSalBR;
			}
			throw (new RecordNotFoundException(typeof(EnvioEmailSalBR)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de EnvioEmailSalBR a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Elias</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idEnvioEmailSalBR))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de EnvioEmailSalBR a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Elias</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idEnvioEmailSalBR, trans))
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
		/// <param name="objEnvioEmailSalBR">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Elias</remarks>
		private static bool SetInstance(IDataReader dr, EnvioEmailSalBR objEnvioEmailSalBR)
		{
			try
			{
				if (dr.Read())
				{
					objEnvioEmailSalBR._idEnvioEmailSalBR = Convert.ToInt32(dr["Idf_Envio_Email_Sal_BR"]);
					objEnvioEmailSalBR._descricaoEmail = Convert.ToString(dr["Des_Email"]);
					objEnvioEmailSalBR._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objEnvioEmailSalBR._persisted = true;
					objEnvioEmailSalBR._modified = false;

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