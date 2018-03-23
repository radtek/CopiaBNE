//-- Data: 21/06/2011 16:23
//-- Autor: Vinicius Maciel

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class AuditorPublicador // Tabela: BNE_Auditor_Publicador
	{
		#region Atributos
		private int _idAuditorPublicador;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private UsuarioFilialPerfil _publicador;
		private DateTime _dataCadastro;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdAuditorPublicador
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdAuditorPublicador
		{
			get
			{
				return this._idAuditorPublicador;
			}
		}
		#endregion 

		#region UsuarioFilialPerfil
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public UsuarioFilialPerfil UsuarioFilialPerfil
		{
			get
			{
				return this._usuarioFilialPerfil;
			}
			set
			{
				this._usuarioFilialPerfil = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Publicador
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public UsuarioFilialPerfil Publicador
		{
			get
			{
				return this._publicador;
			}
			set
			{
				this._publicador = value;
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
		public AuditorPublicador()
		{
		}
		public AuditorPublicador(int idAuditorPublicador)
		{
			this._idAuditorPublicador = idAuditorPublicador;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Auditor_Publicador (Idf_Usuario_Filial_Perfil, Idf_Publicador, Dta_Cadastro, Flg_Inativo) VALUES (@Idf_Usuario_Filial_Perfil, @Idf_Publicador, @Dta_Cadastro, @Flg_Inativo);SET @Idf_Auditor_Publicador = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Auditor_Publicador SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Idf_Publicador = @Idf_Publicador, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_Auditor_Publicador = @Idf_Auditor_Publicador";
		private const string SPDELETE = "DELETE FROM BNE_Auditor_Publicador WHERE Idf_Auditor_Publicador = @Idf_Auditor_Publicador";
		private const string SPSELECTID = "SELECT * FROM BNE_Auditor_Publicador WHERE Idf_Auditor_Publicador = @Idf_Auditor_Publicador";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Auditor_Publicador", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Publicador", SqlDbType.Int, 4));
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
		/// <remarks>Vinicius Maciel</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idAuditorPublicador;
			parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
			parms[2].Value = this._publicador.IdUsuarioFilialPerfil;
			parms[4].Value = this._flagInativo;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de AuditorPublicador no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
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
						this._idAuditorPublicador = Convert.ToInt32(cmd.Parameters["@Idf_Auditor_Publicador"].Value);
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
		/// Método utilizado para inserir uma instância de AuditorPublicador no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idAuditorPublicador = Convert.ToInt32(cmd.Parameters["@Idf_Auditor_Publicador"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de AuditorPublicador no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para atualizar uma instância de AuditorPublicador no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para salvar uma instância de AuditorPublicador no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de AuditorPublicador no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para excluir uma instância de AuditorPublicador no banco de dados.
		/// </summary>
		/// <param name="idAuditorPublicador">Chave do registro.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idAuditorPublicador)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Auditor_Publicador", SqlDbType.Int, 4));

			parms[0].Value = idAuditorPublicador;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de AuditorPublicador no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAuditorPublicador">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idAuditorPublicador, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Auditor_Publicador", SqlDbType.Int, 4));

			parms[0].Value = idAuditorPublicador;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de AuditorPublicador no banco de dados.
		/// </summary>
		/// <param name="idAuditorPublicador">Lista de chaves.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(List<int> idAuditorPublicador)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Auditor_Publicador where Idf_Auditor_Publicador in (";

			for (int i = 0; i < idAuditorPublicador.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idAuditorPublicador[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idAuditorPublicador">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idAuditorPublicador)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Auditor_Publicador", SqlDbType.Int, 4));

			parms[0].Value = idAuditorPublicador;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAuditorPublicador">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idAuditorPublicador, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Auditor_Publicador", SqlDbType.Int, 4));

			parms[0].Value = idAuditorPublicador;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Aud.Idf_Auditor_Publicador, Aud.Idf_Usuario_Filial_Perfil, Aud.Idf_Publicador, Aud.Dta_Cadastro, Aud.Flg_Inativo FROM BNE_Auditor_Publicador Aud";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de AuditorPublicador a partir do banco de dados.
		/// </summary>
		/// <param name="idAuditorPublicador">Chave do registro.</param>
		/// <returns>Instância de AuditorPublicador.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static AuditorPublicador LoadObject(int idAuditorPublicador)
		{
			using (IDataReader dr = LoadDataReader(idAuditorPublicador))
			{
				AuditorPublicador objAuditorPublicador = new AuditorPublicador();
				if (SetInstance(dr, objAuditorPublicador))
					return objAuditorPublicador;
			}
			throw (new RecordNotFoundException(typeof(AuditorPublicador)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de AuditorPublicador a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAuditorPublicador">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de AuditorPublicador.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static AuditorPublicador LoadObject(int idAuditorPublicador, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idAuditorPublicador, trans))
			{
				AuditorPublicador objAuditorPublicador = new AuditorPublicador();
				if (SetInstance(dr, objAuditorPublicador))
					return objAuditorPublicador;
			}
			throw (new RecordNotFoundException(typeof(AuditorPublicador)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de AuditorPublicador a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idAuditorPublicador))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de AuditorPublicador a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idAuditorPublicador, trans))
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
		/// <param name="objAuditorPublicador">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static bool SetInstance(IDataReader dr, AuditorPublicador objAuditorPublicador)
		{
			try
			{
				if (dr.Read())
				{
					objAuditorPublicador._idAuditorPublicador = Convert.ToInt32(dr["Idf_Auditor_Publicador"]);
					objAuditorPublicador._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					objAuditorPublicador._publicador = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Publicador"]));
					objAuditorPublicador._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objAuditorPublicador._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objAuditorPublicador._persisted = true;
					objAuditorPublicador._modified = false;

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