//-- Data: 01/02/2016 14:13
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class RastreadorCurriculoHistorico // Tabela: BNE_Rastreador_Curriculo_Historico
	{
		#region Atributos
		private int _idRastreadorCurriculoHistorico;
		private RastreadorCurriculo _rastreadorCurriculo;
		private DateTime _dataProcessamento;
		private Int16 _quantidadeTotal;
		private Int16 _quantidadeNovos;
		private TimeSpan _hrsDuracaoProcessamento;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRastreadorCurriculoHistorico
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRastreadorCurriculoHistorico
		{
			get
			{
				return this._idRastreadorCurriculoHistorico;
			}
		}
		#endregion 

		#region RastreadorCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public RastreadorCurriculo RastreadorCurriculo
		{
			get
			{
				return this._rastreadorCurriculo;
			}
			set
			{
				this._rastreadorCurriculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataProcessamento
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataProcessamento
		{
			get
			{
				return this._dataProcessamento;
			}
			set
			{
				this._dataProcessamento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeTotal
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Int16 QuantidadeTotal
		{
			get
			{
				return this._quantidadeTotal;
			}
			set
			{
				this._quantidadeTotal = value;
				this._modified = true;
			}
		}
		#endregion 

		#region QuantidadeNovos
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Int16 QuantidadeNovos
		{
			get
			{
				return this._quantidadeNovos;
			}
			set
			{
				this._quantidadeNovos = value;
				this._modified = true;
			}
		}
		#endregion 

		#region HrsDuracaoProcessamento
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TimeSpan HrsDuracaoProcessamento
		{
			get
			{
				return this._hrsDuracaoProcessamento;
			}
			set
			{
				this._hrsDuracaoProcessamento = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public RastreadorCurriculoHistorico()
		{
		}
		public RastreadorCurriculoHistorico(int idRastreadorCurriculoHistorico)
		{
			this._idRastreadorCurriculoHistorico = idRastreadorCurriculoHistorico;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Rastreador_Curriculo_Historico (Idf_Rastreador_Curriculo, Dta_Processamento, Qtd_Total, Qtd_Novos, Hrs_Duracao_Processamento) VALUES (@Idf_Rastreador_Curriculo, @Dta_Processamento, @Qtd_Total, @Qtd_Novos, @Hrs_Duracao_Processamento);SET @Idf_Rastreador_Curriculo_Historico = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Rastreador_Curriculo_Historico SET Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo, Dta_Processamento = @Dta_Processamento, Qtd_Total = @Qtd_Total, Qtd_Novos = @Qtd_Novos, Hrs_Duracao_Processamento = @Hrs_Duracao_Processamento WHERE Idf_Rastreador_Curriculo_Historico = @Idf_Rastreador_Curriculo_Historico";
		private const string SPDELETE = "DELETE FROM BNE_Rastreador_Curriculo_Historico WHERE Idf_Rastreador_Curriculo_Historico = @Idf_Rastreador_Curriculo_Historico";
		private const string SPSELECTID = "SELECT * FROM BNE_Rastreador_Curriculo_Historico WITH(NOLOCK) WHERE Idf_Rastreador_Curriculo_Historico = @Idf_Rastreador_Curriculo_Historico";
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
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Historico", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Processamento", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Qtd_Total", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Qtd_Novos", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Hrs_Duracao_Processamento", SqlDbType.Time, 5));
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
			parms[0].Value = this._idRastreadorCurriculoHistorico;
			parms[1].Value = this._rastreadorCurriculo.IdRastreadorCurriculo;
			parms[2].Value = this._dataProcessamento;
			parms[3].Value = this._quantidadeTotal;
			parms[4].Value = this._quantidadeNovos;
			parms[5].Value = this._hrsDuracaoProcessamento;

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
		/// Método utilizado para inserir uma instância de RastreadorCurriculoHistorico no banco de dados.
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
						this._idRastreadorCurriculoHistorico = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Curriculo_Historico"].Value);
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
		/// Método utilizado para inserir uma instância de RastreadorCurriculoHistorico no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRastreadorCurriculoHistorico = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Curriculo_Historico"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RastreadorCurriculoHistorico no banco de dados.
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
		/// Método utilizado para atualizar uma instância de RastreadorCurriculoHistorico no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de RastreadorCurriculoHistorico no banco de dados.
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
		/// Método utilizado para salvar uma instância de RastreadorCurriculoHistorico no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de RastreadorCurriculoHistorico no banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculoHistorico">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreadorCurriculoHistorico)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Historico", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculoHistorico;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RastreadorCurriculoHistorico no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorCurriculoHistorico">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreadorCurriculoHistorico, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Historico", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculoHistorico;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de RastreadorCurriculoHistorico no banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculoHistorico">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idRastreadorCurriculoHistorico)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Rastreador_Curriculo_Historico where Idf_Rastreador_Curriculo_Historico in (";

			for (int i = 0; i < idRastreadorCurriculoHistorico.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRastreadorCurriculoHistorico[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculoHistorico">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreadorCurriculoHistorico)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Historico", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculoHistorico;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorCurriculoHistorico">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreadorCurriculoHistorico, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Historico", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculoHistorico;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ras.Idf_Rastreador_Curriculo_Historico, Ras.Idf_Rastreador_Curriculo, Ras.Dta_Processamento, Ras.Qtd_Total, Ras.Qtd_Novos, Ras.Hrs_Duracao_Processamento FROM BNE_Rastreador_Curriculo_Historico Ras";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RastreadorCurriculoHistorico a partir do banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculoHistorico">Chave do registro.</param>
		/// <returns>Instância de RastreadorCurriculoHistorico.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RastreadorCurriculoHistorico LoadObject(int idRastreadorCurriculoHistorico)
		{
			using (IDataReader dr = LoadDataReader(idRastreadorCurriculoHistorico))
			{
				RastreadorCurriculoHistorico objRastreadorCurriculoHistorico = new RastreadorCurriculoHistorico();
				if (SetInstance(dr, objRastreadorCurriculoHistorico))
					return objRastreadorCurriculoHistorico;
			}
			throw (new RecordNotFoundException(typeof(RastreadorCurriculoHistorico)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RastreadorCurriculoHistorico a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorCurriculoHistorico">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RastreadorCurriculoHistorico.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RastreadorCurriculoHistorico LoadObject(int idRastreadorCurriculoHistorico, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRastreadorCurriculoHistorico, trans))
			{
				RastreadorCurriculoHistorico objRastreadorCurriculoHistorico = new RastreadorCurriculoHistorico();
				if (SetInstance(dr, objRastreadorCurriculoHistorico))
					return objRastreadorCurriculoHistorico;
			}
			throw (new RecordNotFoundException(typeof(RastreadorCurriculoHistorico)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RastreadorCurriculoHistorico a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRastreadorCurriculoHistorico))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RastreadorCurriculoHistorico a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRastreadorCurriculoHistorico, trans))
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
		/// <param name="objRastreadorCurriculoHistorico">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, RastreadorCurriculoHistorico objRastreadorCurriculoHistorico, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objRastreadorCurriculoHistorico._idRastreadorCurriculoHistorico = Convert.ToInt32(dr["Idf_Rastreador_Curriculo_Historico"]);
					objRastreadorCurriculoHistorico._rastreadorCurriculo = new RastreadorCurriculo(Convert.ToInt32(dr["Idf_Rastreador_Curriculo"]));
					objRastreadorCurriculoHistorico._dataProcessamento = Convert.ToDateTime(dr["Dta_Processamento"]);
					objRastreadorCurriculoHistorico._quantidadeTotal = Convert.ToInt16(dr["Qtd_Total"]);
					objRastreadorCurriculoHistorico._quantidadeNovos = Convert.ToInt16(dr["Qtd_Novos"]);
					objRastreadorCurriculoHistorico._hrsDuracaoProcessamento = TimeSpan.Parse(dr["Hrs_Duracao_Processamento"].ToString());

					objRastreadorCurriculoHistorico._persisted = true;
					objRastreadorCurriculoHistorico._modified = false;

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