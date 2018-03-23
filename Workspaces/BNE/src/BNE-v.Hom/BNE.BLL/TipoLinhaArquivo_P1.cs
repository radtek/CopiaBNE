//-- Data: 06/06/2014 15:27
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TipoLinhaArquivo // Tabela: TAB_Tipo_Linha_Arquivo
	{
		#region Atributos
		private int _idTipoLinhaArquivo;
		private string _DscTipoLinhaArquivo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTipoLinhaArquivo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdTipoLinhaArquivo
		{
			get
			{
				return this._idTipoLinhaArquivo;
			}
			set
			{
				this._idTipoLinhaArquivo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DscTipoLinhaArquivo
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DscTipoLinhaArquivo
		{
			get
			{
				return this._DscTipoLinhaArquivo;
			}
			set
			{
				this._DscTipoLinhaArquivo = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public TipoLinhaArquivo()
		{
		}
		public TipoLinhaArquivo(int idTipoLinhaArquivo)
		{
			this._idTipoLinhaArquivo = idTipoLinhaArquivo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Tipo_Linha_Arquivo (Idf_Tipo_Linha_Arquivo, Dsc_Tipo_Linha_Arquivo) VALUES (@Idf_Tipo_Linha_Arquivo, @Dsc_Tipo_Linha_Arquivo);";
		private const string SPUPDATE = "UPDATE TAB_Tipo_Linha_Arquivo SET Dsc_Tipo_Linha_Arquivo = @Dsc_Tipo_Linha_Arquivo WHERE Idf_Tipo_Linha_Arquivo = @Idf_Tipo_Linha_Arquivo";
		private const string SPDELETE = "DELETE FROM TAB_Tipo_Linha_Arquivo WHERE Idf_Tipo_Linha_Arquivo = @Idf_Tipo_Linha_Arquivo";
		private const string SPSELECTID = "SELECT * FROM TAB_Tipo_Linha_Arquivo WITH(NOLOCK) WHERE Idf_Tipo_Linha_Arquivo = @Idf_Tipo_Linha_Arquivo";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Linha_Arquivo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dsc_Tipo_Linha_Arquivo", SqlDbType.VarChar, 50));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idTipoLinhaArquivo;
			parms[1].Value = this._DscTipoLinhaArquivo;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de TipoLinhaArquivo no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para inserir uma instância de TipoLinhaArquivo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para atualizar uma instância de TipoLinhaArquivo no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para atualizar uma instância de TipoLinhaArquivo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para salvar uma instância de TipoLinhaArquivo no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de TipoLinhaArquivo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para excluir uma instância de TipoLinhaArquivo no banco de dados.
		/// </summary>
		/// <param name="idTipoLinhaArquivo">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idTipoLinhaArquivo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Linha_Arquivo", SqlDbType.Int, 4));

			parms[0].Value = idTipoLinhaArquivo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TipoLinhaArquivo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoLinhaArquivo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idTipoLinhaArquivo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Linha_Arquivo", SqlDbType.Int, 4));

			parms[0].Value = idTipoLinhaArquivo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TipoLinhaArquivo no banco de dados.
		/// </summary>
		/// <param name="idTipoLinhaArquivo">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idTipoLinhaArquivo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Tipo_Linha_Arquivo where Idf_Tipo_Linha_Arquivo in (";

			for (int i = 0; i < idTipoLinhaArquivo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTipoLinhaArquivo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTipoLinhaArquivo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idTipoLinhaArquivo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Linha_Arquivo", SqlDbType.Int, 4));

			parms[0].Value = idTipoLinhaArquivo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoLinhaArquivo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idTipoLinhaArquivo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Linha_Arquivo", SqlDbType.Int, 4));

			parms[0].Value = idTipoLinhaArquivo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Linha_Arquivo, Tip.Dsc_Tipo_Linha_Arquivo FROM TAB_Tipo_Linha_Arquivo Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoLinhaArquivo a partir do banco de dados.
		/// </summary>
		/// <param name="idTipoLinhaArquivo">Chave do registro.</param>
		/// <returns>Instância de TipoLinhaArquivo.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static TipoLinhaArquivo LoadObject(int idTipoLinhaArquivo)
		{
			using (IDataReader dr = LoadDataReader(idTipoLinhaArquivo))
			{
				TipoLinhaArquivo objTipoLinhaArquivo = new TipoLinhaArquivo();
				if (SetInstance(dr, objTipoLinhaArquivo))
					return objTipoLinhaArquivo;
			}
			throw (new RecordNotFoundException(typeof(TipoLinhaArquivo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoLinhaArquivo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoLinhaArquivo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TipoLinhaArquivo.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static TipoLinhaArquivo LoadObject(int idTipoLinhaArquivo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTipoLinhaArquivo, trans))
			{
				TipoLinhaArquivo objTipoLinhaArquivo = new TipoLinhaArquivo();
				if (SetInstance(dr, objTipoLinhaArquivo))
					return objTipoLinhaArquivo;
			}
			throw (new RecordNotFoundException(typeof(TipoLinhaArquivo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TipoLinhaArquivo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTipoLinhaArquivo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TipoLinhaArquivo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTipoLinhaArquivo, trans))
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
		/// <param name="objTipoLinhaArquivo">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, TipoLinhaArquivo objTipoLinhaArquivo)
		{
			try
			{
				if (dr.Read())
				{
					objTipoLinhaArquivo._idTipoLinhaArquivo = Convert.ToInt32(dr["Idf_Tipo_Linha_Arquivo"]);
					objTipoLinhaArquivo._DscTipoLinhaArquivo = Convert.ToString(dr["Dsc_Tipo_Linha_Arquivo"]);

					objTipoLinhaArquivo._persisted = true;
					objTipoLinhaArquivo._modified = false;

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