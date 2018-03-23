//-- Data: 14/07/2014 16:39
//-- Autor: Lennon Vidal

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TipoGatilho // Tabela: TAB_Tipo_Gatilho
	{
		#region Atributos
		private int _idTipoGatilho;
		private string _descricaoTipoGatilho;
		private DateTime? _dataCadastro;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdtipoGatilho
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdTipoGatilho
		{
			get
			{
				return this._idTipoGatilho;
			}
			set
			{
				this._idTipoGatilho = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoTipoGatilho
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoTipoGatilho
		{
			get
			{
				return this._descricaoTipoGatilho;
			}
			set
			{
				this._descricaoTipoGatilho = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCadastro
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataCadastro
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
		public TipoGatilho()
		{
		}
		public TipoGatilho(int idtipoGatilho)
		{
			this._idTipoGatilho = idtipoGatilho;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Tipo_Gatilho (Idf_tipo_Gatilho, Des_Tipo_Gatilho, Dta_Cadastro, Flg_Inativo) VALUES (@Idf_tipo_Gatilho, @Des_Tipo_Gatilho, @Dta_Cadastro, @Flg_Inativo);";
		private const string SPUPDATE = "UPDATE TAB_Tipo_Gatilho SET Des_Tipo_Gatilho = @Des_Tipo_Gatilho, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_tipo_Gatilho = @Idf_tipo_Gatilho";
		private const string SPDELETE = "DELETE FROM TAB_Tipo_Gatilho WHERE Idf_tipo_Gatilho = @Idf_tipo_Gatilho";
		private const string SPSELECTID = "SELECT * FROM TAB_Tipo_Gatilho WITH(NOLOCK) WHERE Idf_tipo_Gatilho = @Idf_tipo_Gatilho";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Lennon Vidal</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_tipo_Gatilho", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Tipo_Gatilho", SqlDbType.VarChar, 50));
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
		/// <remarks>Lennon Vidal</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idTipoGatilho;
			parms[1].Value = this._descricaoTipoGatilho;
			parms[3].Value = this._flagInativo;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de TipoGatilho no banco de dados.
		/// </summary>
		/// <remarks>Lennon Vidal</remarks>
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
		/// Método utilizado para inserir uma instância de TipoGatilho no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Lennon Vidal</remarks>
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
		/// Método utilizado para atualizar uma instância de TipoGatilho no banco de dados.
		/// </summary>
		/// <remarks>Lennon Vidal</remarks>
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
		/// Método utilizado para atualizar uma instância de TipoGatilho no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Lennon Vidal</remarks>
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
		/// Método utilizado para salvar uma instância de TipoGatilho no banco de dados.
		/// </summary>
		/// <remarks>Lennon Vidal</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de TipoGatilho no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Lennon Vidal</remarks>
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
		/// Método utilizado para excluir uma instância de TipoGatilho no banco de dados.
		/// </summary>
		/// <param name="idtipoGatilho">Chave do registro.</param>
		/// <remarks>Lennon Vidal</remarks>
		public static void Delete(int idtipoGatilho)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_tipo_Gatilho", SqlDbType.Int, 4));

			parms[0].Value = idtipoGatilho;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TipoGatilho no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idtipoGatilho">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Lennon Vidal</remarks>
		public static void Delete(int idtipoGatilho, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_tipo_Gatilho", SqlDbType.Int, 4));

			parms[0].Value = idtipoGatilho;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TipoGatilho no banco de dados.
		/// </summary>
		/// <param name="idtipoGatilho">Lista de chaves.</param>
		/// <remarks>Lennon Vidal</remarks>
		public static void Delete(List<int> idtipoGatilho)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Tipo_Gatilho where Idf_tipo_Gatilho in (";

			for (int i = 0; i < idtipoGatilho.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idtipoGatilho[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idtipoGatilho">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Lennon Vidal</remarks>
		private static IDataReader LoadDataReader(int idtipoGatilho)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_tipo_Gatilho", SqlDbType.Int, 4));

			parms[0].Value = idtipoGatilho;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idtipoGatilho">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Lennon Vidal</remarks>
		private static IDataReader LoadDataReader(int idtipoGatilho, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_tipo_Gatilho", SqlDbType.Int, 4));

			parms[0].Value = idtipoGatilho;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_tipo_Gatilho, Tip.Des_Tipo_Gatilho, Tip.Dta_Cadastro, Tip.Flg_Inativo FROM TAB_Tipo_Gatilho Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoGatilho a partir do banco de dados.
		/// </summary>
		/// <param name="idtipoGatilho">Chave do registro.</param>
		/// <returns>Instância de TipoGatilho.</returns>
		/// <remarks>Lennon Vidal</remarks>
		public static TipoGatilho LoadObject(int idtipoGatilho)
		{
			using (IDataReader dr = LoadDataReader(idtipoGatilho))
			{
				TipoGatilho objTipoGatilho = new TipoGatilho();
				if (SetInstance(dr, objTipoGatilho))
					return objTipoGatilho;
			}
			throw (new RecordNotFoundException(typeof(TipoGatilho)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoGatilho a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idtipoGatilho">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TipoGatilho.</returns>
		/// <remarks>Lennon Vidal</remarks>
		public static TipoGatilho LoadObject(int idtipoGatilho, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idtipoGatilho, trans))
			{
				TipoGatilho objTipoGatilho = new TipoGatilho();
				if (SetInstance(dr, objTipoGatilho))
					return objTipoGatilho;
			}
			throw (new RecordNotFoundException(typeof(TipoGatilho)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TipoGatilho a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Lennon Vidal</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTipoGatilho))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TipoGatilho a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Lennon Vidal</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTipoGatilho, trans))
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
		/// <param name="objTipoGatilho">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Lennon Vidal</remarks>
		private static bool SetInstance(IDataReader dr, TipoGatilho objTipoGatilho)
		{
			try
			{
				if (dr.Read())
				{
					objTipoGatilho._idTipoGatilho = Convert.ToInt32(dr["Idf_tipo_Gatilho"]);
					objTipoGatilho._descricaoTipoGatilho = Convert.ToString(dr["Des_Tipo_Gatilho"]);
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objTipoGatilho._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objTipoGatilho._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objTipoGatilho._persisted = true;
					objTipoGatilho._modified = false;

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