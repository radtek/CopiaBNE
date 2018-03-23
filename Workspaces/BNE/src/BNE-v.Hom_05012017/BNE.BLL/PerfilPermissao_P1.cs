//-- Data: 04/11/2010 14:03
//-- Autor: Bruno Flammarion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PerfilPermissao // Tabela: TAB_Perfil_Permissao
	{
		#region Atributos
		private Perfil _perfil;
		private Permissao _permissao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

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

		#region Permissao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Permissao Permissao
		{
			get
			{
				return this._permissao;
			}
			set
			{
				this._permissao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PerfilPermissao()
		{
		}
		public PerfilPermissao(Perfil perfil, Permissao permissao)
		{
			this._perfil = perfil;
			this._permissao = permissao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Perfil_Permissao (Idf_Perfil, Idf_Permissao) VALUES (@Idf_Perfil, @Idf_Permissao);";
		private const string SPUPDATE = "UPDATE TAB_Perfil_Permissao SET  WHERE Idf_Perfil = @Idf_Perfil AND Idf_Permissao = @Idf_Permissao";
		private const string SPDELETE = "DELETE FROM TAB_Perfil_Permissao WHERE Idf_Perfil = @Idf_Perfil AND Idf_Permissao = @Idf_Permissao";
		private const string SPSELECTID = "SELECT * FROM TAB_Perfil_Permissao WHERE Idf_Perfil = @Idf_Perfil AND Idf_Permissao = @Idf_Permissao";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Permissao", SqlDbType.Int, 4));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Bruno Flammarion</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._perfil.IdPerfil;
			parms[1].Value = this._permissao.IdPermissao;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PerfilPermissao no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para inserir uma instância de PerfilPermissao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para atualizar uma instância de PerfilPermissao no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para atualizar uma instância de PerfilPermissao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para salvar uma instância de PerfilPermissao no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de PerfilPermissao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para excluir uma instância de PerfilPermissao no banco de dados.
		/// </summary>
		/// <param name="idPerfil">Chave do registro.</param>
		/// <param name="idPermissao">Chave do registro.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idPerfil, int idPermissao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Permissao", SqlDbType.Int, 4));

			parms[0].Value = idPerfil;
			parms[1].Value = idPermissao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PerfilPermissao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPerfil">Chave do registro.</param>
		/// <param name="idPermissao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idPerfil, int idPermissao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Permissao", SqlDbType.Int, 4));

			parms[0].Value = idPerfil;
			parms[1].Value = idPermissao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPerfil">Chave do registro.</param>
		/// <param name="idPermissao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idPerfil, int idPermissao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Permissao", SqlDbType.Int, 4));

			parms[0].Value = idPerfil;
			parms[1].Value = idPermissao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPerfil">Chave do registro.</param>
		/// <param name="idPermissao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idPerfil, int idPermissao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Permissao", SqlDbType.Int, 4));

			parms[0].Value = idPerfil;
			parms[1].Value = idPermissao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Per.Idf_Perfil, Per.Idf_Permissao FROM TAB_Perfil_Permissao Per";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PerfilPermissao a partir do banco de dados.
		/// </summary>
		/// <param name="idPerfil">Chave do registro.</param>
		/// <param name="idPermissao">Chave do registro.</param>
		/// <returns>Instância de PerfilPermissao.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static PerfilPermissao LoadObject(int idPerfil, int idPermissao)
		{
			using (IDataReader dr = LoadDataReader(idPerfil, idPermissao))
			{
				PerfilPermissao objPerfilPermissao = new PerfilPermissao();
				if (SetInstance(dr, objPerfilPermissao))
					return objPerfilPermissao;
			}
			throw (new RecordNotFoundException(typeof(PerfilPermissao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PerfilPermissao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPerfil">Chave do registro.</param>
		/// <param name="idPermissao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PerfilPermissao.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static PerfilPermissao LoadObject(int idPerfil, int idPermissao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPerfil, idPermissao, trans))
			{
				PerfilPermissao objPerfilPermissao = new PerfilPermissao();
				if (SetInstance(dr, objPerfilPermissao))
					return objPerfilPermissao;
			}
			throw (new RecordNotFoundException(typeof(PerfilPermissao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PerfilPermissao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._perfil.IdPerfil, this._permissao.IdPermissao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PerfilPermissao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._perfil.IdPerfil, this._permissao.IdPermissao, trans))
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
		/// <param name="objPerfilPermissao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static bool SetInstance(IDataReader dr, PerfilPermissao objPerfilPermissao)
		{
			try
			{
				if (dr.Read())
				{
					objPerfilPermissao._perfil = new Perfil(Convert.ToInt32(dr["Idf_Perfil"]));
					objPerfilPermissao._permissao = new Permissao(Convert.ToInt32(dr["Idf_Permissao"]));

					objPerfilPermissao._persisted = true;
					objPerfilPermissao._modified = false;

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