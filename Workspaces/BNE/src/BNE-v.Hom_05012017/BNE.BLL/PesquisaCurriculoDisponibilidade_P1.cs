//-- Data: 01/10/2010 11:40
//-- Autor: Bruno Flammarion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PesquisaCurriculoDisponibilidade // Tabela: TAB_Pesquisa_Curriculo_Disponibilidade
	{
		#region Atributos
		private int _idPesquisaCurriculoDisponibilidade;
		private PesquisaCurriculo _pesquisaCurriculo;
		private Disponibilidade _disponibilidade;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPesquisaCurriculoDisponibilidade
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPesquisaCurriculoDisponibilidade
		{
			get
			{
				return this._idPesquisaCurriculoDisponibilidade;
			}
		}
		#endregion 

		#region PesquisaCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public PesquisaCurriculo PesquisaCurriculo
		{
			get
			{
				return this._pesquisaCurriculo;
			}
			set
			{
				this._pesquisaCurriculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Disponibilidade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Disponibilidade Disponibilidade
		{
			get
			{
				return this._disponibilidade;
			}
			set
			{
				this._disponibilidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PesquisaCurriculoDisponibilidade()
		{
		}
		public PesquisaCurriculoDisponibilidade(int idPesquisaCurriculoDisponibilidade)
		{
			this._idPesquisaCurriculoDisponibilidade = idPesquisaCurriculoDisponibilidade;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pesquisa_Curriculo_Disponibilidade (Idf_Pesquisa_Curriculo, Idf_Disponibilidade) VALUES (@Idf_Pesquisa_Curriculo, @Idf_Disponibilidade);SET @Idf_Pesquisa_Curriculo_Disponibilidade = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Pesquisa_Curriculo_Disponibilidade SET Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo, Idf_Disponibilidade = @Idf_Disponibilidade WHERE Idf_Pesquisa_Curriculo_Disponibilidade = @Idf_Pesquisa_Curriculo_Disponibilidade";
		private const string SPDELETE = "DELETE FROM TAB_Pesquisa_Curriculo_Disponibilidade WHERE Idf_Pesquisa_Curriculo_Disponibilidade = @Idf_Pesquisa_Curriculo_Disponibilidade";
		private const string SPSELECTID = "SELECT * FROM TAB_Pesquisa_Curriculo_Disponibilidade WHERE Idf_Pesquisa_Curriculo_Disponibilidade = @Idf_Pesquisa_Curriculo_Disponibilidade";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Disponibilidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Disponibilidade", SqlDbType.Int, 4));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Bruno Flammarion</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idPesquisaCurriculoDisponibilidade;
			parms[1].Value = this._pesquisaCurriculo.IdPesquisaCurriculo;
			parms[2].Value = this._disponibilidade.IdDisponibilidade;

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
		/// Método utilizado para inserir uma instância de PesquisaCurriculoDisponibilidade no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
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
						this._idPesquisaCurriculoDisponibilidade = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Curriculo_Disponibilidade"].Value);
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
		/// Método utilizado para inserir uma instância de PesquisaCurriculoDisponibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPesquisaCurriculoDisponibilidade = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Curriculo_Disponibilidade"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PesquisaCurriculoDisponibilidade no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para atualizar uma instância de PesquisaCurriculoDisponibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para salvar uma instância de PesquisaCurriculoDisponibilidade no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de PesquisaCurriculoDisponibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para excluir uma instância de PesquisaCurriculoDisponibilidade no banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculoDisponibilidade">Chave do registro.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idPesquisaCurriculoDisponibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculoDisponibilidade;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PesquisaCurriculoDisponibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaCurriculoDisponibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idPesquisaCurriculoDisponibilidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculoDisponibilidade;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PesquisaCurriculoDisponibilidade no banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculoDisponibilidade">Lista de chaves.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(List<int> idPesquisaCurriculoDisponibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pesquisa_Curriculo_Disponibilidade where Idf_Pesquisa_Curriculo_Disponibilidade in (";

			for (int i = 0; i < idPesquisaCurriculoDisponibilidade.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPesquisaCurriculoDisponibilidade[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculoDisponibilidade">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idPesquisaCurriculoDisponibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculoDisponibilidade;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaCurriculoDisponibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idPesquisaCurriculoDisponibilidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Disponibilidade", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculoDisponibilidade;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pesquisa_Curriculo_Disponibilidade, Pes.Idf_Pesquisa_Curriculo, Pes.Idf_Disponibilidade FROM TAB_Pesquisa_Curriculo_Disponibilidade Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaCurriculoDisponibilidade a partir do banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculoDisponibilidade">Chave do registro.</param>
		/// <returns>Instância de PesquisaCurriculoDisponibilidade.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static PesquisaCurriculoDisponibilidade LoadObject(int idPesquisaCurriculoDisponibilidade)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaCurriculoDisponibilidade))
			{
				PesquisaCurriculoDisponibilidade objPesquisaCurriculoDisponibilidade = new PesquisaCurriculoDisponibilidade();
				if (SetInstance(dr, objPesquisaCurriculoDisponibilidade))
					return objPesquisaCurriculoDisponibilidade;
			}
			throw (new RecordNotFoundException(typeof(PesquisaCurriculoDisponibilidade)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaCurriculoDisponibilidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaCurriculoDisponibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PesquisaCurriculoDisponibilidade.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static PesquisaCurriculoDisponibilidade LoadObject(int idPesquisaCurriculoDisponibilidade, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaCurriculoDisponibilidade, trans))
			{
				PesquisaCurriculoDisponibilidade objPesquisaCurriculoDisponibilidade = new PesquisaCurriculoDisponibilidade();
				if (SetInstance(dr, objPesquisaCurriculoDisponibilidade))
					return objPesquisaCurriculoDisponibilidade;
			}
			throw (new RecordNotFoundException(typeof(PesquisaCurriculoDisponibilidade)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaCurriculoDisponibilidade a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaCurriculoDisponibilidade))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaCurriculoDisponibilidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaCurriculoDisponibilidade, trans))
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
		/// <param name="objPesquisaCurriculoDisponibilidade">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static bool SetInstance(IDataReader dr, PesquisaCurriculoDisponibilidade objPesquisaCurriculoDisponibilidade)
		{
			try
			{
				if (dr.Read())
				{
					objPesquisaCurriculoDisponibilidade._idPesquisaCurriculoDisponibilidade = Convert.ToInt32(dr["Idf_Pesquisa_Curriculo_Disponibilidade"]);
					objPesquisaCurriculoDisponibilidade._pesquisaCurriculo = new PesquisaCurriculo(Convert.ToInt32(dr["Idf_Pesquisa_Curriculo"]));
					objPesquisaCurriculoDisponibilidade._disponibilidade = new Disponibilidade(Convert.ToInt32(dr["Idf_Disponibilidade"]));

					objPesquisaCurriculoDisponibilidade._persisted = true;
					objPesquisaCurriculoDisponibilidade._modified = false;

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