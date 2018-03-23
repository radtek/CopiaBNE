//-- Data: 13/04/2010 13:06
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PerfilUsuario // Tabela: TAB_Perfil_Usuario
	{
		#region Atributos
		private int _idfperfilusuario;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private bool _flagInativo;
		private Perfil _perfil;
		private DateTime _dataCadastro;
		private DateTime _dataInicio;
		private DateTime? _dataFim;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region Idfperfilusuario
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int Idfperfilusuario
		{
			get
			{
				return this._idfperfilusuario;
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

		#region Perfil
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Perfil Perfil
		{
			get
			{
				return this._perfil;
			}
			set
			{
				this._perfil = value;
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

		#region DataInicio
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataInicio
		{
			get
			{
				return this._dataInicio;
			}
			set
			{
				this._dataInicio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataFim
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataFim
		{
			get
			{
				return this._dataFim;
			}
			set
			{
				this._dataFim = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PerfilUsuario()
		{
		}
		public PerfilUsuario(int idfperfilusuario)
		{
			this._idfperfilusuario = idfperfilusuario;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Perfil_Usuario (Idf_Usuario_Filial_Perfil, Flg_Inativo, Idf_Perfil, Dta_Cadastro, Dta_Inicio, Dta_Fim) VALUES (@Idf_Usuario_Filial_Perfil, @Flg_Inativo, @Idf_Perfil, @Dta_Cadastro, @Dta_Inicio, @Dta_Fim);SET @idf_perfil_usuario = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Perfil_Usuario SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Flg_Inativo = @Flg_Inativo, Idf_Perfil = @Idf_Perfil, Dta_Cadastro = @Dta_Cadastro, Dta_Inicio = @Dta_Inicio, Dta_Fim = @Dta_Fim WHERE idf_perfil_usuario = @idf_perfil_usuario";
		private const string SPDELETE = "DELETE FROM TAB_Perfil_Usuario WHERE idf_perfil_usuario = @idf_perfil_usuario";
		private const string SPSELECTID = "SELECT * FROM TAB_Perfil_Usuario WHERE idf_perfil_usuario = @idf_perfil_usuario";
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
			parms.Add(new SqlParameter("@idf_perfil_usuario", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Inicio", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Fim", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idfperfilusuario;
			parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
			parms[2].Value = this._flagInativo;
			parms[3].Value = this._perfil.IdPerfil;
			parms[5].Value = this._dataInicio;

			if (this._dataFim.HasValue)
				parms[6].Value = this._dataFim;
			else
				parms[6].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[4].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PerfilUsuario no banco de dados.
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
						this._idfperfilusuario = Convert.ToInt32(cmd.Parameters["@idf_perfil_usuario"].Value);
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
		/// Método utilizado para inserir uma instância de PerfilUsuario no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idfperfilusuario = Convert.ToInt32(cmd.Parameters["@idf_perfil_usuario"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PerfilUsuario no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PerfilUsuario no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PerfilUsuario no banco de dados.
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
		/// Método utilizado para salvar uma instância de PerfilUsuario no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PerfilUsuario no banco de dados.
		/// </summary>
		/// <param name="idfperfilusuario">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idfperfilusuario)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@idf_perfil_usuario", SqlDbType.Int, 4));

			parms[0].Value = idfperfilusuario;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PerfilUsuario no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idfperfilusuario">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idfperfilusuario, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@idf_perfil_usuario", SqlDbType.Int, 4));

			parms[0].Value = idfperfilusuario;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PerfilUsuario no banco de dados.
		/// </summary>
		/// <param name="idfperfilusuario">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idfperfilusuario)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Perfil_Usuario where idf_perfil_usuario in (";

			for (int i = 0; i < idfperfilusuario.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idfperfilusuario[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idfperfilusuario">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idfperfilusuario)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@idf_perfil_usuario", SqlDbType.Int, 4));

			parms[0].Value = idfperfilusuario;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idfperfilusuario">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idfperfilusuario, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@idf_perfil_usuario", SqlDbType.Int, 4));

			parms[0].Value = idfperfilusuario;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Per.idf_perfil_usuario, Per.Idf_Usuario_Filial_Perfil, Per.Flg_Inativo, Per.Idf_Perfil, Per.Dta_Cadastro, Per.Dta_Inicio, Per.Dta_Fim FROM TAB_Perfil_Usuario Per";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PerfilUsuario a partir do banco de dados.
		/// </summary>
		/// <param name="idfperfilusuario">Chave do registro.</param>
		/// <returns>Instância de PerfilUsuario.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PerfilUsuario LoadObject(int idfperfilusuario)
		{
			using (IDataReader dr = LoadDataReader(idfperfilusuario))
			{
				PerfilUsuario objPerfilUsuario = new PerfilUsuario();
				if (SetInstance(dr, objPerfilUsuario))
					return objPerfilUsuario;
			}
			throw (new RecordNotFoundException(typeof(PerfilUsuario)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PerfilUsuario a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idfperfilusuario">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PerfilUsuario.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PerfilUsuario LoadObject(int idfperfilusuario, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idfperfilusuario, trans))
			{
				PerfilUsuario objPerfilUsuario = new PerfilUsuario();
				if (SetInstance(dr, objPerfilUsuario))
					return objPerfilUsuario;
			}
			throw (new RecordNotFoundException(typeof(PerfilUsuario)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PerfilUsuario a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idfperfilusuario))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PerfilUsuario a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idfperfilusuario, trans))
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
		/// <param name="objPerfilUsuario">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PerfilUsuario objPerfilUsuario)
		{
			try
			{
				if (dr.Read())
				{
					objPerfilUsuario._idfperfilusuario = Convert.ToInt32(dr["idf_perfil_usuario"]);
					objPerfilUsuario._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					objPerfilUsuario._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objPerfilUsuario._perfil = new Perfil(Convert.ToInt32(dr["Idf_Perfil"]));
					objPerfilUsuario._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objPerfilUsuario._dataInicio = Convert.ToDateTime(dr["Dta_Inicio"]);
					if (dr["Dta_Fim"] != DBNull.Value)
						objPerfilUsuario._dataFim = Convert.ToDateTime(dr["Dta_Fim"]);

					objPerfilUsuario._persisted = true;
					objPerfilUsuario._modified = false;

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