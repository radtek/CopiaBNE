//-- Data: 21/07/2015 12:01
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CampanhaRecrutamento // Tabela: BNE_Campanha_Recrutamento
	{
		#region Atributos
		private int _idCampanhaRecrutamento;
		private Int16 _quantidadeRetorno;
		private TipoRetornoCampanhaRecrutamento _tipoRetornoCampanhaRecrutamento;
		private Vaga _vaga;
		private DateTime _dataCadastro;
		private PesquisaCurriculo _pesquisaCurriculo;
		private MotivoCampanhaFinalizada _motivoCampanhaFinalizada;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCampanhaRecrutamento
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCampanhaRecrutamento
		{
			get
			{
				return this._idCampanhaRecrutamento;
			}
		}
		#endregion 

		#region QuantidadeRetorno
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Int16 QuantidadeRetorno
		{
			get
			{
				return this._quantidadeRetorno;
			}
			set
			{
				this._quantidadeRetorno = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoRetornoCampanhaRecrutamento
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public TipoRetornoCampanhaRecrutamento TipoRetornoCampanhaRecrutamento
		{
			get
			{
				return this._tipoRetornoCampanhaRecrutamento;
			}
			set
			{
				this._tipoRetornoCampanhaRecrutamento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Vaga
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Vaga Vaga
		{
			get
			{
				return this._vaga;
			}
			set
			{
				this._vaga = value;
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

		#region PesquisaCurriculo
		/// <summary>
		/// Campo opcional.
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

		#region MotivoCampanhaFinalizada
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public MotivoCampanhaFinalizada MotivoCampanhaFinalizada
		{
			get
			{
				return this._motivoCampanhaFinalizada;
			}
			set
			{
				this._motivoCampanhaFinalizada = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CampanhaRecrutamento()
		{
		}
		public CampanhaRecrutamento(int idCampanhaRecrutamento)
		{
			this._idCampanhaRecrutamento = idCampanhaRecrutamento;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Campanha_Recrutamento (Qtd_Retorno, Idf_Tipo_Retorno_Campanha_Recrutamento, Idf_Vaga, Dta_Cadastro, Idf_Pesquisa_Curriculo, Idf_Motivo_Campanha_Finalizada) VALUES (@Qtd_Retorno, @Idf_Tipo_Retorno_Campanha_Recrutamento, @Idf_Vaga, @Dta_Cadastro, @Idf_Pesquisa_Curriculo, @Idf_Motivo_Campanha_Finalizada);SET @Idf_Campanha_Recrutamento = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Campanha_Recrutamento SET Qtd_Retorno = @Qtd_Retorno, Idf_Tipo_Retorno_Campanha_Recrutamento = @Idf_Tipo_Retorno_Campanha_Recrutamento, Idf_Vaga = @Idf_Vaga, Dta_Cadastro = @Dta_Cadastro, Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo, Idf_Motivo_Campanha_Finalizada = @Idf_Motivo_Campanha_Finalizada WHERE Idf_Campanha_Recrutamento = @Idf_Campanha_Recrutamento";
		private const string SPDELETE = "DELETE FROM BNE_Campanha_Recrutamento WHERE Idf_Campanha_Recrutamento = @Idf_Campanha_Recrutamento";
		private const string SPSELECTID = "SELECT * FROM BNE_Campanha_Recrutamento WITH(NOLOCK) WHERE Idf_Campanha_Recrutamento = @Idf_Campanha_Recrutamento";
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
			parms.Add(new SqlParameter("@Idf_Campanha_Recrutamento", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Qtd_Retorno", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Idf_Tipo_Retorno_Campanha_Recrutamento", SqlDbType.Int, 1));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Motivo_Campanha_Finalizada", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCampanhaRecrutamento;
			parms[1].Value = this._quantidadeRetorno;

			if (this._tipoRetornoCampanhaRecrutamento != null)
				parms[2].Value = this._tipoRetornoCampanhaRecrutamento.IdTipoRetornoCampanhaRecrutamento;
			else
				parms[2].Value = DBNull.Value;


			if (this._vaga != null)
				parms[3].Value = this._vaga.IdVaga;
			else
				parms[3].Value = DBNull.Value;


			if (this._pesquisaCurriculo != null)
				parms[5].Value = this._pesquisaCurriculo.IdPesquisaCurriculo;
			else
				parms[5].Value = DBNull.Value;


			if (this._motivoCampanhaFinalizada != null)
				parms[6].Value = this._motivoCampanhaFinalizada.IdMotivoCampanhaFinalizada;
			else
				parms[6].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[4].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CampanhaRecrutamento no banco de dados.
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
						this._idCampanhaRecrutamento = Convert.ToInt32(cmd.Parameters["@Idf_Campanha_Recrutamento"].Value);
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
		/// Método utilizado para inserir uma instância de CampanhaRecrutamento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCampanhaRecrutamento = Convert.ToInt32(cmd.Parameters["@Idf_Campanha_Recrutamento"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CampanhaRecrutamento no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CampanhaRecrutamento no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CampanhaRecrutamento no banco de dados.
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
		/// Método utilizado para salvar uma instância de CampanhaRecrutamento no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CampanhaRecrutamento no banco de dados.
		/// </summary>
		/// <param name="idCampanhaRecrutamento">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCampanhaRecrutamento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campanha_Recrutamento", SqlDbType.Int, 4));

			parms[0].Value = idCampanhaRecrutamento;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CampanhaRecrutamento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCampanhaRecrutamento">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCampanhaRecrutamento, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campanha_Recrutamento", SqlDbType.Int, 4));

			parms[0].Value = idCampanhaRecrutamento;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CampanhaRecrutamento no banco de dados.
		/// </summary>
		/// <param name="idCampanhaRecrutamento">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCampanhaRecrutamento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Campanha_Recrutamento where Idf_Campanha_Recrutamento in (";

			for (int i = 0; i < idCampanhaRecrutamento.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCampanhaRecrutamento[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCampanhaRecrutamento">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCampanhaRecrutamento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campanha_Recrutamento", SqlDbType.Int, 4));

			parms[0].Value = idCampanhaRecrutamento;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCampanhaRecrutamento">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCampanhaRecrutamento, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campanha_Recrutamento", SqlDbType.Int, 4));

			parms[0].Value = idCampanhaRecrutamento;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cam.Idf_Campanha_Recrutamento, Cam.Qtd_Retorno, Cam.Idf_Tipo_Retorno_Campanha_Recrutamento, Cam.Idf_Vaga, Cam.Dta_Cadastro, Cam.Idf_Pesquisa_Curriculo, Cam.Idf_Motivo_Campanha_Finalizada FROM BNE_Campanha_Recrutamento Cam";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CampanhaRecrutamento a partir do banco de dados.
		/// </summary>
		/// <param name="idCampanhaRecrutamento">Chave do registro.</param>
		/// <returns>Instância de CampanhaRecrutamento.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CampanhaRecrutamento LoadObject(int idCampanhaRecrutamento)
		{
			using (IDataReader dr = LoadDataReader(idCampanhaRecrutamento))
			{
				CampanhaRecrutamento objCampanhaRecrutamento = new CampanhaRecrutamento();
				if (SetInstance(dr, objCampanhaRecrutamento))
					return objCampanhaRecrutamento;
			}
			throw (new RecordNotFoundException(typeof(CampanhaRecrutamento)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CampanhaRecrutamento a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCampanhaRecrutamento">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CampanhaRecrutamento.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CampanhaRecrutamento LoadObject(int idCampanhaRecrutamento, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCampanhaRecrutamento, trans))
			{
				CampanhaRecrutamento objCampanhaRecrutamento = new CampanhaRecrutamento();
				if (SetInstance(dr, objCampanhaRecrutamento))
					return objCampanhaRecrutamento;
			}
			throw (new RecordNotFoundException(typeof(CampanhaRecrutamento)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CampanhaRecrutamento a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCampanhaRecrutamento))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CampanhaRecrutamento a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCampanhaRecrutamento, trans))
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
		/// <param name="objCampanhaRecrutamento">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CampanhaRecrutamento objCampanhaRecrutamento, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objCampanhaRecrutamento._idCampanhaRecrutamento = Convert.ToInt32(dr["Idf_Campanha_Recrutamento"]);
					objCampanhaRecrutamento._quantidadeRetorno = Convert.ToInt16(dr["Qtd_Retorno"]);
					if (dr["Idf_Tipo_Retorno_Campanha_Recrutamento"] != DBNull.Value)
						objCampanhaRecrutamento._tipoRetornoCampanhaRecrutamento = new TipoRetornoCampanhaRecrutamento(Convert.ToInt16(dr["Idf_Tipo_Retorno_Campanha_Recrutamento"]));
					if (dr["Idf_Vaga"] != DBNull.Value)
						objCampanhaRecrutamento._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
					objCampanhaRecrutamento._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Idf_Pesquisa_Curriculo"] != DBNull.Value)
						objCampanhaRecrutamento._pesquisaCurriculo = new PesquisaCurriculo(Convert.ToInt32(dr["Idf_Pesquisa_Curriculo"]));
					if (dr["Idf_Motivo_Campanha_Finalizada"] != DBNull.Value)
						objCampanhaRecrutamento._motivoCampanhaFinalizada = new MotivoCampanhaFinalizada(Convert.ToInt32(dr["Idf_Motivo_Campanha_Finalizada"]));

					objCampanhaRecrutamento._persisted = true;
					objCampanhaRecrutamento._modified = false;

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