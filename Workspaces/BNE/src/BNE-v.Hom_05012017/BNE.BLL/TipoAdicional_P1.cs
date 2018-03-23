//-- Data: 09/09/2011 09:41
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TipoAdicional // Tabela: BNE_Tipo_Adicional
	{
		#region Atributos
		private int _idTipoAdicional;
		private string _descricaoTipoAdicional;
		private DateTime _dataCadastro;
		private DateTime _dataAlteracao;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTipoAdicional
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdTipoAdicional
		{
			get
			{
				return this._idTipoAdicional;
			}
		}
		#endregion 

		#region DescricaoTipoAdicional
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoTipoAdicional
		{
			get
			{
				return this._descricaoTipoAdicional;
			}
			set
			{
				this._descricaoTipoAdicional = value;
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

		#region DataAlteracao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataAlteracao
		{
			get
			{
				return this._dataAlteracao;
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
		public TipoAdicional()
		{
		}
		public TipoAdicional(int idTipoAdicional)
		{
			this._idTipoAdicional = idTipoAdicional;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Tipo_Adicional (Des_Tipo_Adicional, Dta_Cadastro, Dta_Alteracao, Flg_Inativo) VALUES (@Des_Tipo_Adicional, @Dta_Cadastro, @Dta_Alteracao, @Flg_Inativo);SET @Idf_Tipo_Adicional = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Tipo_Adicional SET Des_Tipo_Adicional = @Des_Tipo_Adicional, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Flg_Inativo = @Flg_Inativo WHERE Idf_Tipo_Adicional = @Idf_Tipo_Adicional";
		private const string SPDELETE = "DELETE FROM BNE_Tipo_Adicional WHERE Idf_Tipo_Adicional = @Idf_Tipo_Adicional";
		private const string SPSELECTID = "SELECT * FROM BNE_Tipo_Adicional WHERE Idf_Tipo_Adicional = @Idf_Tipo_Adicional";
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
			parms.Add(new SqlParameter("@Idf_Tipo_Adicional", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Tipo_Adicional", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idTipoAdicional;
			parms[1].Value = this._descricaoTipoAdicional;
			parms[4].Value = this._flagInativo;

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
			this._dataAlteracao = DateTime.Now;
			parms[3].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de TipoAdicional no banco de dados.
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
						this._idTipoAdicional = Convert.ToInt32(cmd.Parameters["@Idf_Tipo_Adicional"].Value);
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
		/// Método utilizado para inserir uma instância de TipoAdicional no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idTipoAdicional = Convert.ToInt32(cmd.Parameters["@Idf_Tipo_Adicional"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de TipoAdicional no banco de dados.
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
		/// Método utilizado para atualizar uma instância de TipoAdicional no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de TipoAdicional no banco de dados.
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
		/// Método utilizado para salvar uma instância de TipoAdicional no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de TipoAdicional no banco de dados.
		/// </summary>
		/// <param name="idTipoAdicional">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoAdicional)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Adicional", SqlDbType.Int, 4));

			parms[0].Value = idTipoAdicional;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TipoAdicional no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoAdicional">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTipoAdicional, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Adicional", SqlDbType.Int, 4));

			parms[0].Value = idTipoAdicional;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TipoAdicional no banco de dados.
		/// </summary>
		/// <param name="idTipoAdicional">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idTipoAdicional)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Tipo_Adicional where Idf_Tipo_Adicional in (";

			for (int i = 0; i < idTipoAdicional.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTipoAdicional[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTipoAdicional">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoAdicional)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Adicional", SqlDbType.Int, 4));

			parms[0].Value = idTipoAdicional;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoAdicional">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTipoAdicional, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Tipo_Adicional", SqlDbType.Int, 4));

			parms[0].Value = idTipoAdicional;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Adicional, Tip.Des_Tipo_Adicional, Tip.Dta_Cadastro, Tip.Dta_Alteracao, Tip.Flg_Inativo FROM BNE_Tipo_Adicional Tip";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoAdicional a partir do banco de dados.
		/// </summary>
		/// <param name="idTipoAdicional">Chave do registro.</param>
		/// <returns>Instância de TipoAdicional.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TipoAdicional LoadObject(int idTipoAdicional)
		{
			using (IDataReader dr = LoadDataReader(idTipoAdicional))
			{
				TipoAdicional objTipoAdicional = new TipoAdicional();
				if (SetInstance(dr, objTipoAdicional))
					return objTipoAdicional;
			}
			throw (new RecordNotFoundException(typeof(TipoAdicional)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TipoAdicional a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTipoAdicional">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TipoAdicional.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TipoAdicional LoadObject(int idTipoAdicional, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTipoAdicional, trans))
			{
				TipoAdicional objTipoAdicional = new TipoAdicional();
				if (SetInstance(dr, objTipoAdicional))
					return objTipoAdicional;
			}
			throw (new RecordNotFoundException(typeof(TipoAdicional)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TipoAdicional a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTipoAdicional))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TipoAdicional a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTipoAdicional, trans))
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
		/// <param name="objTipoAdicional">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, TipoAdicional objTipoAdicional)
		{
			try
			{
				if (dr.Read())
				{
					objTipoAdicional._idTipoAdicional = Convert.ToInt32(dr["Idf_Tipo_Adicional"]);
					objTipoAdicional._descricaoTipoAdicional = Convert.ToString(dr["Des_Tipo_Adicional"]);
					objTipoAdicional._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objTipoAdicional._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					objTipoAdicional._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objTipoAdicional._persisted = true;
					objTipoAdicional._modified = false;

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