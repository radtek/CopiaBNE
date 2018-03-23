//-- Data: 19/09/2017 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CurriculoDenuncia // Tabela: BNE_Curriculo_Denuncia
	{
		#region Atributos
		private CurriculoCorrecao _curriculoCorrecao;
		private PlanoAdquirido _planoAdquirido;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region CurriculoCorrecao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CurriculoCorrecao CurriculoCorrecao
		{
			get
			{
				return this._curriculoCorrecao;
			}
			set
			{
				this._curriculoCorrecao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region PlanoAdquirido
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public PlanoAdquirido PlanoAdquirido
		{
			get
			{
				return this._planoAdquirido;
			}
			set
			{
				this._planoAdquirido = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CurriculoDenuncia()
		{
		}
		public CurriculoDenuncia(CurriculoCorrecao curriculoCorrecao)
		{
			this._curriculoCorrecao = curriculoCorrecao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Curriculo_Denuncia (Idf_Curriculo_Correcao, Idf_Plano_Adquirido) VALUES (@Idf_Curriculo_Correcao, @Idf_Plano_Adquirido);";
		private const string SPUPDATE = "UPDATE BNE_Curriculo_Denuncia SET Idf_Plano_Adquirido = @Idf_Plano_Adquirido WHERE Idf_Curriculo_Correcao = @Idf_Curriculo_Correcao";
		private const string SPDELETE = "DELETE FROM BNE_Curriculo_Denuncia WHERE Idf_Curriculo_Correcao = @Idf_Curriculo_Correcao";
		private const string SPSELECTID = "SELECT * FROM BNE_Curriculo_Denuncia WITH(NOLOCK) WHERE Idf_Curriculo_Correcao = @Idf_Curriculo_Correcao";
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
			parms.Add(new SqlParameter("@Idf_Curriculo_Correcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
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
			parms[0].Value = this._curriculoCorrecao.IdCurriculoCorrecao;

			if (this._planoAdquirido != null)
				parms[1].Value = this._planoAdquirido.IdPlanoAdquirido;
			else
				parms[1].Value = DBNull.Value;


			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CurriculoDenuncia no banco de dados.
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
		/// Método utilizado para inserir uma instância de CurriculoDenuncia no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
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
		/// Método utilizado para atualizar uma instância de CurriculoDenuncia no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CurriculoDenuncia no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CurriculoDenuncia no banco de dados.
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
		/// Método utilizado para salvar uma instância de CurriculoDenuncia no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CurriculoDenuncia no banco de dados.
		/// </summary>
		/// <param name="idCurriculoCorrecao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCurriculoCorrecao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Correcao", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoCorrecao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CurriculoDenuncia no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoCorrecao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCurriculoCorrecao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Correcao", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoCorrecao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CurriculoDenuncia no banco de dados.
		/// </summary>
		/// <param name="curriculoCorrecao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<CurriculoCorrecao> curriculoCorrecao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Curriculo_Denuncia where Idf_Curriculo_Correcao in (";

			for (int i = 0; i < curriculoCorrecao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = curriculoCorrecao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurriculoCorrecao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCurriculoCorrecao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Correcao", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoCorrecao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoCorrecao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCurriculoCorrecao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Correcao", SqlDbType.Int, 4));

			parms[0].Value = idCurriculoCorrecao;

			return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar uma consulta paginada do banco de dados.
		/// </summary>
		/// <param name="colunaOrdenacao">Nome da coluna pela qual será ordenada.</param>
		/// <param name="direcaoOrdenacao">Direção da ordenação (ASC ou DESC).</param>
		/// <param name="paginaCorrente">Número da página que será exibida.</param>
		/// <param name="tamanhoPagina">Adquirido de itens em cada página.</param>
		/// <param name="totalRegistros">Adquirido total de registros na tabela.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
		{
			int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
			int fim = paginaCorrente * tamanhoPagina;

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curriculo_Correcao, Cur.Idf_Plano_Adquirido FROM BNE_Curriculo_Denuncia Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoDenuncia a partir do banco de dados.
		/// </summary>
		/// <param name="idCurriculoCorrecao">Chave do registro.</param>
		/// <returns>Instância de CurriculoDenuncia.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CurriculoDenuncia LoadObject(int idCurriculoCorrecao)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoCorrecao))
			{
				CurriculoDenuncia objCurriculoDenuncia = new CurriculoDenuncia();
				if (SetInstance(dr, objCurriculoDenuncia))
					return objCurriculoDenuncia;
			}
			throw (new RecordNotFoundException(typeof(CurriculoDenuncia)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoDenuncia a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoCorrecao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CurriculoDenuncia.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CurriculoDenuncia LoadObject(int idCurriculoCorrecao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoCorrecao, trans))
			{
				CurriculoDenuncia objCurriculoDenuncia = new CurriculoDenuncia();
				if (SetInstance(dr, objCurriculoDenuncia))
					return objCurriculoDenuncia;
			}
			throw (new RecordNotFoundException(typeof(CurriculoDenuncia)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoDenuncia a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._curriculoCorrecao.IdCurriculoCorrecao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoDenuncia a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._curriculoCorrecao.IdCurriculoCorrecao, trans))
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
		/// <param name="objCurriculoDenuncia">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CurriculoDenuncia objCurriculoDenuncia, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objCurriculoDenuncia._curriculoCorrecao = new CurriculoCorrecao(Convert.ToInt32(dr["Idf_Curriculo_Correcao"]));
					if (dr["Idf_Plano_Adquirido"] != DBNull.Value)
						objCurriculoDenuncia._planoAdquirido = new PlanoAdquirido(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));

					objCurriculoDenuncia._persisted = true;
					objCurriculoDenuncia._modified = false;

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