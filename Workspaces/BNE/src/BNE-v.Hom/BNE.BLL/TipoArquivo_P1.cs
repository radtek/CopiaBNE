//-- Data: 22/05/2014 11:00
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TipoArquivo // Tabela: TAB_Tipo_Arquivo
	{
		#region Atributos
		private int _idTipoArquivo;
		private byte[] _DscTipoArquivo;
		private bool _flagRemessa;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTipoArquivo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdTipoArquivo
		{
			get
			{
				return this._idTipoArquivo;
			}
			set
			{
				this._idTipoArquivo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DscTipoArquivo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public byte[] DscTipoArquivo
		{
			get
			{
				return this._DscTipoArquivo;
			}
			set
			{
				this._DscTipoArquivo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagRemessa
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagRemessa
		{
			get
			{
				return this._flagRemessa;
			}
			set
			{
				this._flagRemessa = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public TipoArquivo()
		{
		}
		public TipoArquivo(int idTipoArquivo)
		{
			this._idTipoArquivo = idTipoArquivo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Tipo_Arquivo (Idf_Tipo_Arquivo, Dsc_Tipo_Arquivo, Flg_Remessa) VALUES (@Idf_Tipo_Arquivo, @Dsc_Tipo_Arquivo, @Flg_Remessa);";
		private const string SPUPDATE = "UPDATE TAB_Tipo_Arquivo SET Dsc_Tipo_Arquivo = @Dsc_Tipo_Arquivo, Flg_Remessa = @Flg_Remessa WHERE Idf_Tipo_Arquivo = @Idf_Tipo_Arquivo";
		private const string SPDELETE = "DELETE FROM TAB_Tipo_Arquivo WHERE Idf_Tipo_Arquivo = @Idf_Tipo_Arquivo";
		private const string SPSELECTID = "SELECT * FROM TAB_Tipo_Arquivo WITH(NOLOCK) WHERE Idf_Tipo_Arquivo = @Idf_Tipo_Arquivo";
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
			parms.Add(new SqlParameter("@Idf_Tipo_Arquivo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dsc_Tipo_Arquivo", SqlDbType.VarBinary, 50));
			parms.Add(new SqlParameter("@Flg_Remessa", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idTipoArquivo;
			parms[1].Value = this._DscTipoArquivo;
			parms[2].Value = this._flagRemessa;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de TipoArquivo no banco de dados.
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
		/// Método utilizado para inserir uma instância de TipoArquivo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de TipoArquivo no banco de dados.
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
		/// Método utilizado para atualizar uma instância de TipoArquivo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de TipoArquivo no banco de dados.
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
		/// Método utilizado para salvar uma instância de TipoArquivo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de TipoArquivo no banco de dados.
		/// </summary>
		/// <param name="idTipoArquivo">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idTipoArquivo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Arquivo", SqlDbType.Int, 4));

			parms[0].Value = idTipoArquivo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TipoArquivo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoArquivo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idTipoArquivo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Arquivo", SqlDbType.Int, 4));

			parms[0].Value = idTipoArquivo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TipoArquivo no banco de dados.
		/// </summary>
		/// <param name="idTipoArquivo">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idTipoArquivo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Tipo_Arquivo where Idf_Tipo_Arquivo in (";

			for (int i = 0; i < idTipoArquivo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTipoArquivo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTipoArquivo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idTipoArquivo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Arquivo", SqlDbType.Int, 4));

			parms[0].Value = idTipoArquivo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoArquivo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idTipoArquivo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Arquivo", SqlDbType.Int, 4));

			parms[0].Value = idTipoArquivo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Arquivo, Tip.Dsc_Tipo_Arquivo, Tip.Flg_Remessa FROM TAB_Tipo_Arquivo Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoArquivo a partir do banco de dados.
		/// </summary>
		/// <param name="idTipoArquivo">Chave do registro.</param>
		/// <returns>Instância de TipoArquivo.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static TipoArquivo LoadObject(int idTipoArquivo)
		{
			using (IDataReader dr = LoadDataReader(idTipoArquivo))
			{
				TipoArquivo objTipoArquivo = new TipoArquivo();
				if (SetInstance(dr, objTipoArquivo))
					return objTipoArquivo;
			}
			throw (new RecordNotFoundException(typeof(TipoArquivo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoArquivo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoArquivo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TipoArquivo.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static TipoArquivo LoadObject(int idTipoArquivo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTipoArquivo, trans))
			{
				TipoArquivo objTipoArquivo = new TipoArquivo();
				if (SetInstance(dr, objTipoArquivo))
					return objTipoArquivo;
			}
			throw (new RecordNotFoundException(typeof(TipoArquivo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TipoArquivo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTipoArquivo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TipoArquivo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTipoArquivo, trans))
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
		/// <param name="objTipoArquivo">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, TipoArquivo objTipoArquivo)
		{
			try
			{
				if (dr.Read())
				{
					objTipoArquivo._idTipoArquivo = Convert.ToInt32(dr["Idf_Tipo_Arquivo"]);
					objTipoArquivo._DscTipoArquivo = (byte[])(dr["Dsc_Tipo_Arquivo"]);
					objTipoArquivo._flagRemessa = Convert.ToBoolean(dr["Flg_Remessa"]);

					objTipoArquivo._persisted = true;
					objTipoArquivo._modified = false;

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