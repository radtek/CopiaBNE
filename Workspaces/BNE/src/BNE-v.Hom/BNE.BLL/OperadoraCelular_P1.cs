//-- Data: 17/09/2013 12:54
//-- Autor: BNE

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	[Serializable]
	public partial class OperadoraCelular // Tabela: plataforma.TAB_Operadora_Celular
	{
		#region Atributos
		private int _idOperadoraCelular;
		private string _nomeOperadoraCelular;
		private bool _flagInativo;
		private string _descricaoURLLogo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdOperadoraCelular
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdOperadoraCelular
		{
			get
			{
				return this._idOperadoraCelular;
			}
			set
			{
				this._idOperadoraCelular = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeOperadoraCelular
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string NomeOperadoraCelular
		{
			get
			{
				return this._nomeOperadoraCelular;
			}
			set
			{
				this._nomeOperadoraCelular = value;
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

		#region DescricaoURLLogo
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo opcional.
		/// </summary>
		public string DescricaoURLLogo
		{
			get
			{
				return this._descricaoURLLogo;
			}
			set
			{
				this._descricaoURLLogo = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public OperadoraCelular()
		{
		}
		public OperadoraCelular(int idOperadoraCelular)
		{
			this._idOperadoraCelular = idOperadoraCelular;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Operadora_Celular (Idf_Operadora_Celular, Nme_Operadora_Celular, Flg_Inativo, Des_URL_Logo) VALUES (@Idf_Operadora_Celular, @Nme_Operadora_Celular, @Flg_Inativo, @Des_URL_Logo);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Operadora_Celular SET Nme_Operadora_Celular = @Nme_Operadora_Celular, Flg_Inativo = @Flg_Inativo, Des_URL_Logo = @Des_URL_Logo WHERE Idf_Operadora_Celular = @Idf_Operadora_Celular";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Operadora_Celular WHERE Idf_Operadora_Celular = @Idf_Operadora_Celular";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Operadora_Celular WHERE Idf_Operadora_Celular = @Idf_Operadora_Celular";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>BNE</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Operadora_Celular", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Operadora_Celular", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_URL_Logo", SqlDbType.VarChar, 500));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>BNE</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idOperadoraCelular;
			parms[1].Value = this._nomeOperadoraCelular;
			parms[2].Value = this._flagInativo;

			if (!String.IsNullOrEmpty(this._descricaoURLLogo))
				parms[3].Value = this._descricaoURLLogo;
			else
				parms[3].Value = DBNull.Value;


			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de OperadoraCelular no banco de dados.
		/// </summary>
		/// <remarks>BNE</remarks>
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
		/// Método utilizado para inserir uma instância de OperadoraCelular no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>BNE</remarks>
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
		/// Método utilizado para atualizar uma instância de OperadoraCelular no banco de dados.
		/// </summary>
		/// <remarks>BNE</remarks>
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
		/// Método utilizado para atualizar uma instância de OperadoraCelular no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>BNE</remarks>
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
		/// Método utilizado para salvar uma instância de OperadoraCelular no banco de dados.
		/// </summary>
		/// <remarks>BNE</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de OperadoraCelular no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>BNE</remarks>
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
		/// Método utilizado para excluir uma instância de OperadoraCelular no banco de dados.
		/// </summary>
		/// <param name="idOperadoraCelular">Chave do registro.</param>
		/// <remarks>BNE</remarks>
		public static void Delete(int idOperadoraCelular)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Operadora_Celular", SqlDbType.Int, 4));

			parms[0].Value = idOperadoraCelular;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de OperadoraCelular no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOperadoraCelular">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>BNE</remarks>
		public static void Delete(int idOperadoraCelular, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Operadora_Celular", SqlDbType.Int, 4));

			parms[0].Value = idOperadoraCelular;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de OperadoraCelular no banco de dados.
		/// </summary>
		/// <param name="idOperadoraCelular">Lista de chaves.</param>
		/// <remarks>BNE</remarks>
		public static void Delete(List<int> idOperadoraCelular)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Operadora_Celular where Idf_Operadora_Celular in (";

			for (int i = 0; i < idOperadoraCelular.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idOperadoraCelular[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idOperadoraCelular">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>BNE</remarks>
		private static IDataReader LoadDataReader(int idOperadoraCelular)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Operadora_Celular", SqlDbType.Int, 4));

			parms[0].Value = idOperadoraCelular;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOperadoraCelular">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>BNE</remarks>
		private static IDataReader LoadDataReader(int idOperadoraCelular, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Operadora_Celular", SqlDbType.Int, 4));

			parms[0].Value = idOperadoraCelular;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ope.Idf_Operadora_Celular, Ope.Nme_Operadora_Celular, Ope.Flg_Inativo, Ope.Des_URL_Logo FROM plataforma.TAB_Operadora_Celular Ope";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de OperadoraCelular a partir do banco de dados.
		/// </summary>
		/// <param name="idOperadoraCelular">Chave do registro.</param>
		/// <returns>Instância de OperadoraCelular.</returns>
		/// <remarks>BNE</remarks>
		public static OperadoraCelular LoadObject(int idOperadoraCelular)
		{
			using (IDataReader dr = LoadDataReader(idOperadoraCelular))
			{
				OperadoraCelular objOperadoraCelular = new OperadoraCelular();
				if (SetInstance(dr, objOperadoraCelular))
					return objOperadoraCelular;
			}
			throw (new RecordNotFoundException(typeof(OperadoraCelular)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de OperadoraCelular a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOperadoraCelular">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de OperadoraCelular.</returns>
		/// <remarks>BNE</remarks>
		public static OperadoraCelular LoadObject(int idOperadoraCelular, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idOperadoraCelular, trans))
			{
				OperadoraCelular objOperadoraCelular = new OperadoraCelular();
				if (SetInstance(dr, objOperadoraCelular))
					return objOperadoraCelular;
			}
			throw (new RecordNotFoundException(typeof(OperadoraCelular)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de OperadoraCelular a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>BNE</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idOperadoraCelular))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de OperadoraCelular a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>BNE</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idOperadoraCelular, trans))
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
		/// <param name="objOperadoraCelular">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>BNE</remarks>
		private static bool SetInstance(IDataReader dr, OperadoraCelular objOperadoraCelular)
		{
			try
			{
				if (dr.Read())
				{
					objOperadoraCelular._idOperadoraCelular = Convert.ToInt32(dr["Idf_Operadora_Celular"]);
					objOperadoraCelular._nomeOperadoraCelular = Convert.ToString(dr["Nme_Operadora_Celular"]);
					objOperadoraCelular._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					if (dr["Des_URL_Logo"] != DBNull.Value)
						objOperadoraCelular._descricaoURLLogo = Convert.ToString(dr["Des_URL_Logo"]);

					objOperadoraCelular._persisted = true;
					objOperadoraCelular._modified = false;

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