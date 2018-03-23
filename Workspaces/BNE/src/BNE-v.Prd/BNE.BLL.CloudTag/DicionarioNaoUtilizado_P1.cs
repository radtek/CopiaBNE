//-- Data: 21/01/2014 10:33
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL.CloudTag
{
	public partial class DicionarioNaoUtilizado // Tabela: TAB_Dicionario_Nao_Utilizado
	{
		#region Atributos
		private int _idDicionarioNaoUtilizado;
		private string _descricaoDicionarioNaoUtilizado;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdDicionarioNaoUtilizado
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdDicionarioNaoUtilizado
		{
			get
			{
				return this._idDicionarioNaoUtilizado;
			}
		}
		#endregion 

		#region DescricaoDicionarioNaoUtilizado
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoDicionarioNaoUtilizado
		{
			get
			{
				return this._descricaoDicionarioNaoUtilizado;
			}
			set
			{
				this._descricaoDicionarioNaoUtilizado = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public DicionarioNaoUtilizado()
		{
		}
		public DicionarioNaoUtilizado(int idDicionarioNaoUtilizado)
		{
			this._idDicionarioNaoUtilizado = idDicionarioNaoUtilizado;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Dicionario_Nao_Utilizado (Des_Dicionario_Nao_Utilizado) VALUES (@Des_Dicionario_Nao_Utilizado);SET @Idf_Dicionario_Nao_Utilizado = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Dicionario_Nao_Utilizado SET Des_Dicionario_Nao_Utilizado = @Des_Dicionario_Nao_Utilizado WHERE Idf_Dicionario_Nao_Utilizado = @Idf_Dicionario_Nao_Utilizado";
		private const string SPDELETE = "DELETE FROM TAB_Dicionario_Nao_Utilizado WHERE Idf_Dicionario_Nao_Utilizado = @Idf_Dicionario_Nao_Utilizado";
		private const string SPSELECTID = "SELECT * FROM TAB_Dicionario_Nao_Utilizado WITH(NOLOCK) WHERE Idf_Dicionario_Nao_Utilizado = @Idf_Dicionario_Nao_Utilizado";
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
			parms.Add(new SqlParameter("@Idf_Dicionario_Nao_Utilizado", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Dicionario_Nao_Utilizado", SqlDbType.VarChar, 100));
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
			parms[0].Value = this._idDicionarioNaoUtilizado;

			if (!String.IsNullOrEmpty(this._descricaoDicionarioNaoUtilizado))
				parms[1].Value = this._descricaoDicionarioNaoUtilizado;
			else
				parms[1].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de DicionarioNaoUtilizado no banco de dados.
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
						this._idDicionarioNaoUtilizado = Convert.ToInt32(cmd.Parameters["@Idf_Dicionario_Nao_Utilizado"].Value);
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
		/// Método utilizado para inserir uma instância de DicionarioNaoUtilizado no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idDicionarioNaoUtilizado = Convert.ToInt32(cmd.Parameters["@Idf_Dicionario_Nao_Utilizado"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de DicionarioNaoUtilizado no banco de dados.
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
		/// Método utilizado para atualizar uma instância de DicionarioNaoUtilizado no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de DicionarioNaoUtilizado no banco de dados.
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
		/// Método utilizado para salvar uma instância de DicionarioNaoUtilizado no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de DicionarioNaoUtilizado no banco de dados.
		/// </summary>
		/// <param name="idDicionarioNaoUtilizado">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idDicionarioNaoUtilizado)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Dicionario_Nao_Utilizado", SqlDbType.Int, 4));

			parms[0].Value = idDicionarioNaoUtilizado;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de DicionarioNaoUtilizado no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idDicionarioNaoUtilizado">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idDicionarioNaoUtilizado, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Dicionario_Nao_Utilizado", SqlDbType.Int, 4));

			parms[0].Value = idDicionarioNaoUtilizado;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de DicionarioNaoUtilizado no banco de dados.
		/// </summary>
		/// <param name="idDicionarioNaoUtilizado">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idDicionarioNaoUtilizado)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Dicionario_Nao_Utilizado where Idf_Dicionario_Nao_Utilizado in (";

			for (int i = 0; i < idDicionarioNaoUtilizado.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idDicionarioNaoUtilizado[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idDicionarioNaoUtilizado">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idDicionarioNaoUtilizado)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Dicionario_Nao_Utilizado", SqlDbType.Int, 4));

			parms[0].Value = idDicionarioNaoUtilizado;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idDicionarioNaoUtilizado">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idDicionarioNaoUtilizado, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Dicionario_Nao_Utilizado", SqlDbType.Int, 4));

			parms[0].Value = idDicionarioNaoUtilizado;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Dic.Idf_Dicionario_Nao_Utilizado, Dic.Des_Dicionario_Nao_Utilizado FROM TAB_Dicionario_Nao_Utilizado Dic";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de DicionarioNaoUtilizado a partir do banco de dados.
		/// </summary>
		/// <param name="idDicionarioNaoUtilizado">Chave do registro.</param>
		/// <returns>Instância de DicionarioNaoUtilizado.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static DicionarioNaoUtilizado LoadObject(int idDicionarioNaoUtilizado)
		{
			using (IDataReader dr = LoadDataReader(idDicionarioNaoUtilizado))
			{
				DicionarioNaoUtilizado objDicionarioNaoUtilizado = new DicionarioNaoUtilizado();
				if (SetInstance(dr, objDicionarioNaoUtilizado))
					return objDicionarioNaoUtilizado;
			}
			throw (new RecordNotFoundException(typeof(DicionarioNaoUtilizado)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de DicionarioNaoUtilizado a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idDicionarioNaoUtilizado">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de DicionarioNaoUtilizado.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static DicionarioNaoUtilizado LoadObject(int idDicionarioNaoUtilizado, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idDicionarioNaoUtilizado, trans))
			{
				DicionarioNaoUtilizado objDicionarioNaoUtilizado = new DicionarioNaoUtilizado();
				if (SetInstance(dr, objDicionarioNaoUtilizado))
					return objDicionarioNaoUtilizado;
			}
			throw (new RecordNotFoundException(typeof(DicionarioNaoUtilizado)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de DicionarioNaoUtilizado a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idDicionarioNaoUtilizado))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de DicionarioNaoUtilizado a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idDicionarioNaoUtilizado, trans))
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
		/// <param name="objDicionarioNaoUtilizado">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, DicionarioNaoUtilizado objDicionarioNaoUtilizado)
		{
			try
			{
				if (dr.Read())
				{
					objDicionarioNaoUtilizado._idDicionarioNaoUtilizado = Convert.ToInt32(dr["Idf_Dicionario_Nao_Utilizado"]);
					if (dr["Des_Dicionario_Nao_Utilizado"] != DBNull.Value)
						objDicionarioNaoUtilizado._descricaoDicionarioNaoUtilizado = Convert.ToString(dr["Des_Dicionario_Nao_Utilizado"]);

					objDicionarioNaoUtilizado._persisted = true;
					objDicionarioNaoUtilizado._modified = false;

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