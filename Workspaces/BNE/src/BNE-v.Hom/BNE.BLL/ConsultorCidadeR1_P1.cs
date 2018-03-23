//-- Data: 24/06/2013 16:33
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class ConsultorCidadeR1 // Tabela: BNE_Consultor_Cidade_R1
	{
		#region Atributos
		private ConsultorR1 _consultorR1;
		private Cidade _cidade;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region ConsultorR1
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public ConsultorR1 ConsultorR1
		{
			get
			{
				return this._consultorR1;
			}
			set
			{
				this._consultorR1 = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Cidade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Cidade Cidade
		{
			get
			{
				return this._cidade;
			}
			set
			{
				this._cidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public ConsultorCidadeR1()
		{
		}
		public ConsultorCidadeR1(ConsultorR1 consultorR1, Cidade cidade)
		{
			this._consultorR1 = consultorR1;
			this._cidade = cidade;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Consultor_Cidade_R1 (Idf_Consultor_R1, Idf_Cidade) VALUES (@Idf_Consultor_R1, @Idf_Cidade);";
		private const string SPUPDATE = "UPDATE BNE_Consultor_Cidade_R1 SET  WHERE Idf_Consultor_R1 = @Idf_Consultor_R1 AND Idf_Cidade = @Idf_Cidade";
		private const string SPDELETE = "DELETE FROM BNE_Consultor_Cidade_R1 WHERE Idf_Consultor_R1 = @Idf_Consultor_R1 AND Idf_Cidade = @Idf_Cidade";
		private const string SPSELECTID = "SELECT * FROM BNE_Consultor_Cidade_R1 WHERE Idf_Consultor_R1 = @Idf_Consultor_R1 AND Idf_Cidade = @Idf_Cidade";
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
			parms.Add(new SqlParameter("@Idf_Consultor_R1", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
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
			parms[0].Value = this._consultorR1.IdConsultorR1;
			parms[1].Value = this._cidade.IdCidade;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de ConsultorCidadeR1 no banco de dados.
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
		/// Método utilizado para inserir uma instância de ConsultorCidadeR1 no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de ConsultorCidadeR1 no banco de dados.
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
		/// Método utilizado para atualizar uma instância de ConsultorCidadeR1 no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de ConsultorCidadeR1 no banco de dados.
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
		/// Método utilizado para salvar uma instância de ConsultorCidadeR1 no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de ConsultorCidadeR1 no banco de dados.
		/// </summary>
		/// <param name="idConsultorR1">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idConsultorR1, int idCidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Consultor_R1", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idConsultorR1;
			parms[1].Value = idCidade;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de ConsultorCidadeR1 no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idConsultorR1">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idConsultorR1, int idCidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Consultor_R1", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idConsultorR1;
			parms[1].Value = idCidade;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idConsultorR1">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idConsultorR1, int idCidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Consultor_R1", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idConsultorR1;
			parms[1].Value = idCidade;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idConsultorR1">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idConsultorR1, int idCidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Consultor_R1", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idConsultorR1;
			parms[1].Value = idCidade;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Con.Idf_Consultor_R1, Con.Idf_Cidade FROM BNE_Consultor_Cidade_R1 Con";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de ConsultorCidadeR1 a partir do banco de dados.
		/// </summary>
		/// <param name="idConsultorR1">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <returns>Instância de ConsultorCidadeR1.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static ConsultorCidadeR1 LoadObject(int idConsultorR1, int idCidade)
		{
			using (IDataReader dr = LoadDataReader(idConsultorR1, idCidade))
			{
				ConsultorCidadeR1 objConsultorCidadeR1 = new ConsultorCidadeR1();
				if (SetInstance(dr, objConsultorCidadeR1))
					return objConsultorCidadeR1;
			}
			throw (new RecordNotFoundException(typeof(ConsultorCidadeR1)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de ConsultorCidadeR1 a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idConsultorR1">Chave do registro.</param>
		/// <param name="idCidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de ConsultorCidadeR1.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static ConsultorCidadeR1 LoadObject(int idConsultorR1, int idCidade, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idConsultorR1, idCidade, trans))
			{
				ConsultorCidadeR1 objConsultorCidadeR1 = new ConsultorCidadeR1();
				if (SetInstance(dr, objConsultorCidadeR1))
					return objConsultorCidadeR1;
			}
			throw (new RecordNotFoundException(typeof(ConsultorCidadeR1)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de ConsultorCidadeR1 a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._consultorR1.IdConsultorR1, this._cidade.IdCidade))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de ConsultorCidadeR1 a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._consultorR1.IdConsultorR1, this._cidade.IdCidade, trans))
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
		/// <param name="objConsultorCidadeR1">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, ConsultorCidadeR1 objConsultorCidadeR1)
		{
			try
			{
				if (dr.Read())
				{
					objConsultorCidadeR1._consultorR1 = new ConsultorR1(Convert.ToInt32(dr["Idf_Consultor_R1"]));
					objConsultorCidadeR1._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));

					objConsultorCidadeR1._persisted = true;
					objConsultorCidadeR1._modified = false;

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