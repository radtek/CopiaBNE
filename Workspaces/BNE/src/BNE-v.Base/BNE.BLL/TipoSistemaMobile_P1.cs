//-- Data: 16/07/2013 16:45
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TipoSistemaMobile // Tabela: BNE_Tipo_Sistema_Mobile
	{
		#region Atributos
		private int _idTipoSistemaMobile;
		private string _descricaoTipoSistemaMobile;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTipoSistemaMobile
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdTipoSistemaMobile
		{
			get
			{
				return this._idTipoSistemaMobile;
			}
		}
		#endregion 

		#region DescricaoTipoSistemaMobile
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoTipoSistemaMobile
		{
			get
			{
				return this._descricaoTipoSistemaMobile;
			}
			set
			{
				this._descricaoTipoSistemaMobile = value;
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

		#endregion

		#region Construtores
		public TipoSistemaMobile()
		{
		}
		public TipoSistemaMobile(int idTipoSistemaMobile)
		{
			this._idTipoSistemaMobile = idTipoSistemaMobile;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Tipo_Sistema_Mobile (Des_Tipo_Sistema_Mobile, Dta_Cadastro) VALUES (@Des_Tipo_Sistema_Mobile, @Dta_Cadastro);SET @Idf_Tipo_Sistema_Mobile = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Tipo_Sistema_Mobile SET Des_Tipo_Sistema_Mobile = @Des_Tipo_Sistema_Mobile, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Tipo_Sistema_Mobile = @Idf_Tipo_Sistema_Mobile";
		private const string SPDELETE = "DELETE FROM BNE_Tipo_Sistema_Mobile WHERE Idf_Tipo_Sistema_Mobile = @Idf_Tipo_Sistema_Mobile";
		private const string SPSELECTID = "SELECT * FROM BNE_Tipo_Sistema_Mobile WITH(NOLOCK) WHERE Idf_Tipo_Sistema_Mobile = @Idf_Tipo_Sistema_Mobile";
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
			parms.Add(new SqlParameter("@Idf_Tipo_Sistema_Mobile", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Tipo_Sistema_Mobile", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idTipoSistemaMobile;
			parms[1].Value = this._descricaoTipoSistemaMobile;

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
		/// Método utilizado para inserir uma instância de TipoSistemaMobile no banco de dados.
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
						this._idTipoSistemaMobile = Convert.ToInt32(cmd.Parameters["@Idf_Tipo_Sistema_Mobile"].Value);
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
		/// Método utilizado para inserir uma instância de TipoSistemaMobile no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idTipoSistemaMobile = Convert.ToInt32(cmd.Parameters["@Idf_Tipo_Sistema_Mobile"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de TipoSistemaMobile no banco de dados.
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
		/// Método utilizado para atualizar uma instância de TipoSistemaMobile no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de TipoSistemaMobile no banco de dados.
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
		/// Método utilizado para salvar uma instância de TipoSistemaMobile no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de TipoSistemaMobile no banco de dados.
		/// </summary>
		/// <param name="idTipoSistemaMobile">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoSistemaMobile)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Sistema_Mobile", SqlDbType.Int, 4));

			parms[0].Value = idTipoSistemaMobile;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TipoSistemaMobile no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoSistemaMobile">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoSistemaMobile, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Sistema_Mobile", SqlDbType.Int, 4));

			parms[0].Value = idTipoSistemaMobile;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TipoSistemaMobile no banco de dados.
		/// </summary>
		/// <param name="idTipoSistemaMobile">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idTipoSistemaMobile)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Tipo_Sistema_Mobile where Idf_Tipo_Sistema_Mobile in (";

			for (int i = 0; i < idTipoSistemaMobile.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTipoSistemaMobile[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTipoSistemaMobile">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoSistemaMobile)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Sistema_Mobile", SqlDbType.Int, 4));

			parms[0].Value = idTipoSistemaMobile;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoSistemaMobile">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoSistemaMobile, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Sistema_Mobile", SqlDbType.Int, 4));

			parms[0].Value = idTipoSistemaMobile;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Sistema_Mobile, Tip.Des_Tipo_Sistema_Mobile, Tip.Dta_Cadastro FROM BNE_Tipo_Sistema_Mobile Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoSistemaMobile a partir do banco de dados.
		/// </summary>
		/// <param name="idTipoSistemaMobile">Chave do registro.</param>
		/// <returns>Instância de TipoSistemaMobile.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TipoSistemaMobile LoadObject(int idTipoSistemaMobile)
		{
			using (IDataReader dr = LoadDataReader(idTipoSistemaMobile))
			{
				TipoSistemaMobile objTipoSistemaMobile = new TipoSistemaMobile();
				if (SetInstance(dr, objTipoSistemaMobile))
					return objTipoSistemaMobile;
			}
			throw (new RecordNotFoundException(typeof(TipoSistemaMobile)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoSistemaMobile a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoSistemaMobile">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TipoSistemaMobile.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TipoSistemaMobile LoadObject(int idTipoSistemaMobile, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTipoSistemaMobile, trans))
			{
				TipoSistemaMobile objTipoSistemaMobile = new TipoSistemaMobile();
				if (SetInstance(dr, objTipoSistemaMobile))
					return objTipoSistemaMobile;
			}
			throw (new RecordNotFoundException(typeof(TipoSistemaMobile)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TipoSistemaMobile a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTipoSistemaMobile))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TipoSistemaMobile a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTipoSistemaMobile, trans))
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
		/// <param name="objTipoSistemaMobile">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, TipoSistemaMobile objTipoSistemaMobile)
		{
			try
			{
				if (dr.Read())
				{
					objTipoSistemaMobile._idTipoSistemaMobile = Convert.ToInt32(dr["Idf_Tipo_Sistema_Mobile"]);
					objTipoSistemaMobile._descricaoTipoSistemaMobile = Convert.ToString(dr["Des_Tipo_Sistema_Mobile"]);
					objTipoSistemaMobile._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objTipoSistemaMobile._persisted = true;
					objTipoSistemaMobile._modified = false;

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