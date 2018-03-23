//-- Data: 26/01/2016 17:17
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PesquisaVagaDeficiencia // Tabela: TAB_Pesquisa_Vaga_Deficiencia
	{
		#region Atributos
		private int _idPesquisaVagaDeficiencia;
		private PesquisaVaga _pesquisaVaga;
		private Deficiencia _deficiencia;
		private DeficienciaDetalhe _deficienciaDetalhe;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPesquisaVagaDeficiencia
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPesquisaVagaDeficiencia
		{
			get
			{
				return this._idPesquisaVagaDeficiencia;
			}
		}
		#endregion 

		#region PesquisaVaga
		/// <summary>
		/// Campo obrigatório.
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

		#region Deficiencia
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Deficiencia Deficiencia
		{
			get
			{
				return this._deficiencia;
			}
			set
			{
				this._deficiencia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DeficienciaDetalhe
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DeficienciaDetalhe DeficienciaDetalhe
		{
			get
			{
				return this._deficienciaDetalhe;
			}
			set
			{
				this._deficienciaDetalhe = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PesquisaVagaDeficiencia()
		{
		}
		public PesquisaVagaDeficiencia(int idPesquisaVagaDeficiencia)
		{
			this._idPesquisaVagaDeficiencia = idPesquisaVagaDeficiencia;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pesquisa_Vaga_Deficiencia (Idf_Pesquisa_Vaga, Idf_Deficiencia, Idf_Deficiencia_Detalhe) VALUES (@Idf_Pesquisa_Vaga, @Idf_Deficiencia, @Idf_Deficiencia_Detalhe);SET @Idf_Pesquisa_Vaga_Deficiencia = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Pesquisa_Vaga_Deficiencia SET Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga, Idf_Deficiencia = @Idf_Deficiencia, Idf_Deficiencia_Detalhe = @Idf_Deficiencia_Detalhe WHERE Idf_Pesquisa_Vaga_Deficiencia = @Idf_Pesquisa_Vaga_Deficiencia";
		private const string SPDELETE = "DELETE FROM TAB_Pesquisa_Vaga_Deficiencia WHERE Idf_Pesquisa_Vaga_Deficiencia = @Idf_Pesquisa_Vaga_Deficiencia";
		private const string SPSELECTID = "SELECT * FROM TAB_Pesquisa_Vaga_Deficiencia WITH(NOLOCK) WHERE Idf_Pesquisa_Vaga_Deficiencia = @Idf_Pesquisa_Vaga_Deficiencia";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Mailson</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Deficiencia", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Deficiencia_Detalhe", SqlDbType.Int, 4));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Mailson</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idPesquisaVagaDeficiencia;
			parms[1].Value = this._pesquisaVaga.IdPesquisaVaga;
			parms[2].Value = this._deficiencia.IdDeficiencia;

			if (this._deficienciaDetalhe != null)
				parms[3].Value = this._deficienciaDetalhe.IdfDeficienciaDetalhe;
			else
				parms[3].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de PesquisaVagaDeficiencia no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
						this._idPesquisaVagaDeficiencia = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Vaga_Deficiencia"].Value);
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
		/// Método utilizado para inserir uma instância de PesquisaVagaDeficiencia no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPesquisaVagaDeficiencia = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Vaga_Deficiencia"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PesquisaVagaDeficiencia no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para atualizar uma instância de PesquisaVagaDeficiencia no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para salvar uma instância de PesquisaVagaDeficiencia no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de PesquisaVagaDeficiencia no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para excluir uma instância de PesquisaVagaDeficiencia no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaDeficiencia">Chave do registro.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idPesquisaVagaDeficiencia)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Deficiencia", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaDeficiencia;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PesquisaVagaDeficiencia no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVagaDeficiencia">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idPesquisaVagaDeficiencia, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Deficiencia", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaDeficiencia;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PesquisaVagaDeficiencia no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaDeficiencia">Lista de chaves.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(List<int> idPesquisaVagaDeficiencia)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pesquisa_Vaga_Deficiencia where Idf_Pesquisa_Vaga_Deficiencia in (";

			for (int i = 0; i < idPesquisaVagaDeficiencia.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPesquisaVagaDeficiencia[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaDeficiencia">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idPesquisaVagaDeficiencia)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Deficiencia", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaDeficiencia;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVagaDeficiencia">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idPesquisaVagaDeficiencia, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Deficiencia", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaDeficiencia;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pesquisa_Vaga_Deficiencia, Pes.Idf_Pesquisa_Vaga, Pes.Idf_Deficiencia, Pes.Idf_Deficiencia_Detalhe FROM TAB_Pesquisa_Vaga_Deficiencia Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaVagaDeficiencia a partir do banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaDeficiencia">Chave do registro.</param>
		/// <returns>Instância de PesquisaVagaDeficiencia.</returns>
		/// <remarks>Mailson</remarks>
		public static PesquisaVagaDeficiencia LoadObject(int idPesquisaVagaDeficiencia)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaVagaDeficiencia))
			{
				PesquisaVagaDeficiencia objPesquisaVagaDeficiencia = new PesquisaVagaDeficiencia();
				if (SetInstance(dr, objPesquisaVagaDeficiencia))
					return objPesquisaVagaDeficiencia;
			}
			throw (new RecordNotFoundException(typeof(PesquisaVagaDeficiencia)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaVagaDeficiencia a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVagaDeficiencia">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PesquisaVagaDeficiencia.</returns>
		/// <remarks>Mailson</remarks>
		public static PesquisaVagaDeficiencia LoadObject(int idPesquisaVagaDeficiencia, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaVagaDeficiencia, trans))
			{
				PesquisaVagaDeficiencia objPesquisaVagaDeficiencia = new PesquisaVagaDeficiencia();
				if (SetInstance(dr, objPesquisaVagaDeficiencia))
					return objPesquisaVagaDeficiencia;
			}
			throw (new RecordNotFoundException(typeof(PesquisaVagaDeficiencia)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaVagaDeficiencia a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaVagaDeficiencia))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaVagaDeficiencia a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaVagaDeficiencia, trans))
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
		/// <param name="objPesquisaVagaDeficiencia">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		private static bool SetInstance(IDataReader dr, PesquisaVagaDeficiencia objPesquisaVagaDeficiencia, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objPesquisaVagaDeficiencia._idPesquisaVagaDeficiencia = Convert.ToInt32(dr["Idf_Pesquisa_Vaga_Deficiencia"]);
					objPesquisaVagaDeficiencia._pesquisaVaga = new PesquisaVaga(Convert.ToInt32(dr["Idf_Pesquisa_Vaga"]));
					objPesquisaVagaDeficiencia._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
					if (dr["Idf_Deficiencia_Detalhe"] != DBNull.Value)
						objPesquisaVagaDeficiencia._deficienciaDetalhe = new DeficienciaDetalhe(Convert.ToInt32(dr["Idf_Deficiencia_Detalhe"]));

					objPesquisaVagaDeficiencia._persisted = true;
					objPesquisaVagaDeficiencia._modified = false;

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
				if (dispose)
					dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}