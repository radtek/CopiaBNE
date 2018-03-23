//-- Data: 30/05/2011 09:45
//-- Autor: Vinicius Maciel

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CurriculoAuditoria // Tabela: BNE_Curriculo_Auditoria
	{
		#region Atributos
		private int _idCurriculoAuditoria;
		private DateTime _dataCadastro;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private Curriculo _curriculo;
		private UsuarioFilialPerfil _publicadorPerfil;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCurriculoAuditoria
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCurriculoAuditoria
		{
			get
			{
				return this._idCurriculoAuditoria;
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

		#region Curriculo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Curriculo Curriculo
		{
			get
			{
				return this._curriculo;
			}
			set
			{
				this._curriculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region PublicadorPerfil
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public UsuarioFilialPerfil PublicadorPerfil
		{
			get
			{
				return this._publicadorPerfil;
			}
			set
			{
				this._publicadorPerfil = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CurriculoAuditoria()
		{
		}
		public CurriculoAuditoria(int idCurriculoAuditoria)
		{
			this._idCurriculoAuditoria = idCurriculoAuditoria;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Curriculo_Auditoria (Dta_Cadastro, Idf_Usuario_Filial_Perfil, Idf_Curriculo, Idf_Publicador_Perfil) VALUES (@Dta_Cadastro, @Idf_Usuario_Filial_Perfil, @Idf_Curriculo, @Idf_Publicador_Perfil);SET @Idf_Curriculo_Auditoria = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Curriculo_Auditoria SET Dta_Cadastro = @Dta_Cadastro, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Idf_Curriculo = @Idf_Curriculo, Idf_Publicador_Perfil = @Idf_Publicador_Perfil WHERE Idf_Curriculo_Auditoria = @Idf_Curriculo_Auditoria";
		private const string SPDELETE = "DELETE FROM BNE_Curriculo_Auditoria WHERE Idf_Curriculo_Auditoria = @Idf_Curriculo_Auditoria";
		private const string SPSELECTID = "SELECT * FROM BNE_Curriculo_Auditoria WHERE Idf_Curriculo_Auditoria = @Idf_Curriculo_Auditoria";
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
			parms.Add(new SqlParameter("@Idf_Curriculo_Auditoria", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Publicador_Perfil", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCurriculoAuditoria;
			parms[2].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;

			if (this._curriculo != null)
				parms[3].Value = this._curriculo.IdCurriculo;
			else
				parms[3].Value = DBNull.Value;


			if (this._publicadorPerfil != null)
				parms[4].Value = this._publicadorPerfil.IdUsuarioFilialPerfil;
			else
				parms[4].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[1].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CurriculoAuditoria no banco de dados.
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
						this._idCurriculoAuditoria = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Auditoria"].Value);
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
		/// Método utilizado para inserir uma instância de CurriculoAuditoria no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCurriculoAuditoria = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Auditoria"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CurriculoAuditoria no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CurriculoAuditoria no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CurriculoAuditoria no banco de dados.
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
		/// Método utilizado para salvar uma instância de CurriculoAuditoria no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CurriculoAuditoria no banco de dados.
		/// </summary>
		/// <param name="idCurriculoAuditoria">Chave do registro.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idCurriculoAuditoria)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Auditoria", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoAuditoria;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CurriculoAuditoria no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoAuditoria">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idCurriculoAuditoria, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Auditoria", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoAuditoria;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CurriculoAuditoria no banco de dados.
		/// </summary>
		/// <param name="idCurriculoAuditoria">Lista de chaves.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(List<int> idCurriculoAuditoria)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Curriculo_Auditoria where Idf_Curriculo_Auditoria in (";

			for (int i = 0; i < idCurriculoAuditoria.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCurriculoAuditoria[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurriculoAuditoria">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idCurriculoAuditoria)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Auditoria", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoAuditoria;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoAuditoria">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idCurriculoAuditoria, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Auditoria", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoAuditoria;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curriculo_Auditoria, Cur.Dta_Cadastro, Cur.Idf_Usuario_Filial_Perfil, Cur.Idf_Curriculo, Cur.Idf_Publicador_Perfil FROM BNE_Curriculo_Auditoria Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoAuditoria a partir do banco de dados.
		/// </summary>
		/// <param name="idCurriculoAuditoria">Chave do registro.</param>
		/// <returns>Instância de CurriculoAuditoria.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static CurriculoAuditoria LoadObject(int idCurriculoAuditoria)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoAuditoria))
			{
				CurriculoAuditoria objCurriculoAuditoria = new CurriculoAuditoria();
				if (SetInstance(dr, objCurriculoAuditoria))
					return objCurriculoAuditoria;
			}
			throw (new RecordNotFoundException(typeof(CurriculoAuditoria)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoAuditoria a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoAuditoria">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CurriculoAuditoria.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static CurriculoAuditoria LoadObject(int idCurriculoAuditoria, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoAuditoria, trans))
			{
				CurriculoAuditoria objCurriculoAuditoria = new CurriculoAuditoria();
				if (SetInstance(dr, objCurriculoAuditoria))
					return objCurriculoAuditoria;
			}
			throw (new RecordNotFoundException(typeof(CurriculoAuditoria)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoAuditoria a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoAuditoria))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoAuditoria a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoAuditoria, trans))
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
		/// <param name="objCurriculoAuditoria">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static bool SetInstance(IDataReader dr, CurriculoAuditoria objCurriculoAuditoria)
		{
			try
			{
				if (dr.Read())
				{
					objCurriculoAuditoria._idCurriculoAuditoria = Convert.ToInt32(dr["Idf_Curriculo_Auditoria"]);
					objCurriculoAuditoria._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objCurriculoAuditoria._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					if (dr["Idf_Curriculo"] != DBNull.Value)
						objCurriculoAuditoria._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					if (dr["Idf_Publicador_Perfil"] != DBNull.Value)
						objCurriculoAuditoria._publicadorPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Publicador_Perfil"]));

					objCurriculoAuditoria._persisted = true;
					objCurriculoAuditoria._modified = false;

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