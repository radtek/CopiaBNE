//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Estado // Tabela: plataforma.TAB_Estado
	{
		#region Atributos
		private int _idEstado;
		private string _siglaEstado;
		private string _nomeEstado;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private Regiao _regiao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdEstado
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdEstado
		{
			get
			{
				return this._idEstado;
			}
			set
			{
				this._idEstado = value;
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

		#region NomeEstado
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string NomeEstado
		{
			get
			{
				return this._nomeEstado;
			}
			set
			{
				this._nomeEstado = value;
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

		#region Regiao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Regiao Regiao
		{
			get
			{
				return this._regiao;
			}
			set
			{
				this._regiao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Estado()
		{
		}
		public Estado(int idEstado)
		{
			this._idEstado = idEstado;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Estado (Idf_Estado, Sig_Estado, Nme_Estado, Flg_Inativo, Dta_Cadastro, Idf_Regiao) VALUES (@Idf_Estado, @Sig_Estado, @Nme_Estado, @Flg_Inativo, @Dta_Cadastro, @Idf_Regiao);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Estado SET Sig_Estado = @Sig_Estado, Nme_Estado = @Nme_Estado, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Idf_Regiao = @Idf_Regiao WHERE Idf_Estado = @Idf_Estado";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Estado WHERE Idf_Estado = @Idf_Estado";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Estado WHERE Idf_Estado = @Idf_Estado";
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
			parms.Add(new SqlParameter("@Idf_Estado", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Sig_Estado", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Nme_Estado", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Regiao", SqlDbType.Int, 4));
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
			parms[0].Value = this._idEstado;
			parms[1].Value = this._siglaEstado;
			parms[2].Value = this._nomeEstado;
			parms[3].Value = this._flagInativo;

			if (this._regiao != null)
				parms[5].Value = this._regiao.IdRegiao;
			else
				parms[5].Value = DBNull.Value;


			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[4].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Estado no banco de dados.
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
		/// Método utilizado para inserir uma instância de Estado no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
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
		/// Método utilizado para atualizar uma instância de Estado no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Estado no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Estado no banco de dados.
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
		/// Método utilizado para salvar uma instância de Estado no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Estado no banco de dados.
		/// </summary>
		/// <param name="idEstado">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idEstado)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estado", SqlDbType.Int, 4));

			parms[0].Value = idEstado;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Estado no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEstado">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idEstado, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estado", SqlDbType.Int, 4));

			parms[0].Value = idEstado;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Estado no banco de dados.
		/// </summary>
		/// <param name="idEstado">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idEstado)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Estado where Idf_Estado in (";

			for (int i = 0; i < idEstado.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idEstado[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idEstado">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idEstado)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estado", SqlDbType.Int, 4));

			parms[0].Value = idEstado;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEstado">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idEstado, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Estado", SqlDbType.Int, 4));

			parms[0].Value = idEstado;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Est.Idf_Estado, Est.Sig_Estado, Est.Nme_Estado, Est.Flg_Inativo, Est.Dta_Cadastro, Est.Idf_Regiao FROM plataforma.TAB_Estado Est";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Estado a partir do banco de dados.
		/// </summary>
		/// <param name="idEstado">Chave do registro.</param>
		/// <returns>Instância de Estado.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Estado LoadObject(int idEstado)
		{
			using (IDataReader dr = LoadDataReader(idEstado))
			{
				Estado objEstado = new Estado();
				if (SetInstance(dr, objEstado))
					return objEstado;
			}
			throw (new RecordNotFoundException(typeof(Estado)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Estado a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEstado">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Estado.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Estado LoadObject(int idEstado, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idEstado, trans))
			{
				Estado objEstado = new Estado();
				if (SetInstance(dr, objEstado))
					return objEstado;
			}
			throw (new RecordNotFoundException(typeof(Estado)));
		}
		#endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objEstado">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Estado objEstado)
		{
			try
			{
				if (dr.Read())
				{
					objEstado._idEstado = Convert.ToInt32(dr["Idf_Estado"]);
					objEstado._siglaEstado = Convert.ToString(dr["Sig_Estado"]);
					objEstado._nomeEstado = Convert.ToString(dr["Nme_Estado"]);
					objEstado._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objEstado._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Idf_Regiao"] != DBNull.Value)
						objEstado._regiao = new Regiao(Convert.ToInt32(dr["Idf_Regiao"]));

					objEstado._persisted = true;
					objEstado._modified = false;

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