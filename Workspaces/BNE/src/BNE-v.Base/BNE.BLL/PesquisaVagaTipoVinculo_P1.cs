//-- Data: 16/01/2012 11:03
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PesquisaVagaTipoVinculo // Tabela: TAB_Pesquisa_Vaga_Tipo_Vinculo
	{
		#region Atributos
		private int _idPesquisaVagaTipoVinculo;
		private TipoVinculo _tipoVinculo;
		private PesquisaVaga _pesquisaVaga;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPesquisaVagaTipoVinculo
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPesquisaVagaTipoVinculo
		{
			get
			{
				return this._idPesquisaVagaTipoVinculo;
			}
		}
		#endregion 

		#region TipoVinculo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public TipoVinculo TipoVinculo
		{
			get
			{
				return this._tipoVinculo;
			}
			set
			{
				this._tipoVinculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region PesquisaVaga
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public PesquisaVaga PesquisaVaga
		{
			get
			{
				return this._pesquisaVaga;
			}
			set
			{
				this._pesquisaVaga = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PesquisaVagaTipoVinculo()
		{
		}
		public PesquisaVagaTipoVinculo(int idPesquisaVagaTipoVinculo)
		{
			this._idPesquisaVagaTipoVinculo = idPesquisaVagaTipoVinculo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pesquisa_Vaga_Tipo_Vinculo (Idf_Tipo_Vinculo, Idf_Pesquisa_Vaga) VALUES (@Idf_Tipo_Vinculo, @Idf_Pesquisa_Vaga);SET @Idf_Pesquisa_Vaga_Tipo_Vinculo = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Pesquisa_Vaga_Tipo_Vinculo SET Idf_Tipo_Vinculo = @Idf_Tipo_Vinculo, Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga WHERE Idf_Pesquisa_Vaga_Tipo_Vinculo = @Idf_Pesquisa_Vaga_Tipo_Vinculo";
		private const string SPDELETE = "DELETE FROM TAB_Pesquisa_Vaga_Tipo_Vinculo WHERE Idf_Pesquisa_Vaga_Tipo_Vinculo = @Idf_Pesquisa_Vaga_Tipo_Vinculo";
		private const string SPSELECTID = "SELECT * FROM TAB_Pesquisa_Vaga_Tipo_Vinculo WHERE Idf_Pesquisa_Vaga_Tipo_Vinculo = @Idf_Pesquisa_Vaga_Tipo_Vinculo";
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
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Tipo_Vinculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Vinculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));
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
			parms[0].Value = this._idPesquisaVagaTipoVinculo;

			if (this._tipoVinculo != null)
				parms[1].Value = this._tipoVinculo.IdTipoVinculo;
			else
				parms[1].Value = DBNull.Value;


			if (this._pesquisaVaga != null)
				parms[2].Value = this._pesquisaVaga.IdPesquisaVaga;
			else
				parms[2].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de PesquisaVagaTipoVinculo no banco de dados.
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
						this._idPesquisaVagaTipoVinculo = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Vaga_Tipo_Vinculo"].Value);
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
		/// Método utilizado para inserir uma instância de PesquisaVagaTipoVinculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPesquisaVagaTipoVinculo = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Vaga_Tipo_Vinculo"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PesquisaVagaTipoVinculo no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PesquisaVagaTipoVinculo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PesquisaVagaTipoVinculo no banco de dados.
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
		/// Método utilizado para salvar uma instância de PesquisaVagaTipoVinculo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PesquisaVagaTipoVinculo no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaTipoVinculo">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPesquisaVagaTipoVinculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Tipo_Vinculo", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaTipoVinculo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PesquisaVagaTipoVinculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVagaTipoVinculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPesquisaVagaTipoVinculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Tipo_Vinculo", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaTipoVinculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PesquisaVagaTipoVinculo no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaTipoVinculo">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPesquisaVagaTipoVinculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pesquisa_Vaga_Tipo_Vinculo where Idf_Pesquisa_Vaga_Tipo_Vinculo in (";

			for (int i = 0; i < idPesquisaVagaTipoVinculo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPesquisaVagaTipoVinculo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaTipoVinculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPesquisaVagaTipoVinculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Tipo_Vinculo", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaTipoVinculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVagaTipoVinculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPesquisaVagaTipoVinculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Tipo_Vinculo", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaTipoVinculo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pesquisa_Vaga_Tipo_Vinculo, Pes.Idf_Tipo_Vinculo, Pes.Idf_Pesquisa_Vaga FROM TAB_Pesquisa_Vaga_Tipo_Vinculo Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaVagaTipoVinculo a partir do banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaTipoVinculo">Chave do registro.</param>
		/// <returns>Instância de PesquisaVagaTipoVinculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PesquisaVagaTipoVinculo LoadObject(int idPesquisaVagaTipoVinculo)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaVagaTipoVinculo))
			{
				PesquisaVagaTipoVinculo objPesquisaVagaTipoVinculo = new PesquisaVagaTipoVinculo();
				if (SetInstance(dr, objPesquisaVagaTipoVinculo))
					return objPesquisaVagaTipoVinculo;
			}
			throw (new RecordNotFoundException(typeof(PesquisaVagaTipoVinculo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaVagaTipoVinculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVagaTipoVinculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PesquisaVagaTipoVinculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PesquisaVagaTipoVinculo LoadObject(int idPesquisaVagaTipoVinculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaVagaTipoVinculo, trans))
			{
				PesquisaVagaTipoVinculo objPesquisaVagaTipoVinculo = new PesquisaVagaTipoVinculo();
				if (SetInstance(dr, objPesquisaVagaTipoVinculo))
					return objPesquisaVagaTipoVinculo;
			}
			throw (new RecordNotFoundException(typeof(PesquisaVagaTipoVinculo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaVagaTipoVinculo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaVagaTipoVinculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaVagaTipoVinculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaVagaTipoVinculo, trans))
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
		/// <param name="objPesquisaVagaTipoVinculo">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PesquisaVagaTipoVinculo objPesquisaVagaTipoVinculo)
		{
			try
			{
				if (dr.Read())
				{
					objPesquisaVagaTipoVinculo._idPesquisaVagaTipoVinculo = Convert.ToInt32(dr["Idf_Pesquisa_Vaga_Tipo_Vinculo"]);
					if (dr["Idf_Tipo_Vinculo"] != DBNull.Value)
						objPesquisaVagaTipoVinculo._tipoVinculo = new TipoVinculo(Convert.ToInt32(dr["Idf_Tipo_Vinculo"]));
					if (dr["Idf_Pesquisa_Vaga"] != DBNull.Value)
						objPesquisaVagaTipoVinculo._pesquisaVaga = new PesquisaVaga(Convert.ToInt32(dr["Idf_Pesquisa_Vaga"]));

					objPesquisaVagaTipoVinculo._persisted = true;
					objPesquisaVagaTipoVinculo._modified = false;

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