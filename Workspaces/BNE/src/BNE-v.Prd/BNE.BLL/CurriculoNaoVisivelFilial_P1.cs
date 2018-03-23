//-- Data: 28/03/2016 15:45
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CurriculoNaoVisivelFilial // Tabela: BNE_Curriculo_Nao_Visivel_Filial
	{
		#region Atributos
		private Filial _filial;
		private Curriculo _curriculo;
		private DateTime? _dataCadastro;
		private string _descricaoMotivoCurriculoNaoVisivelFilial;
		private int _motivoBloqueio;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region Filial
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Filial Filial
		{
			get
			{
				return this._filial;
			}
			set
			{
				this._filial = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Curriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Curriculo Curriculo
		{
			get
			{
				return this._curriculo;
			}
			set
			{
				this._curriculo = value;
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

		#region DescricaoMotivoCurriculoNaoVisivelFilial
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoMotivoCurriculoNaoVisivelFilial
		{
			get
			{
				return this._descricaoMotivoCurriculoNaoVisivelFilial;
			}
			set
			{
				this._descricaoMotivoCurriculoNaoVisivelFilial = value;
				this._modified = true;
			}
		}
		#endregion 

		#region MotivoBloqueio
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int MotivoBloqueio
		{
			get
			{
				return this._motivoBloqueio;
			}
			set
			{
				this._motivoBloqueio = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CurriculoNaoVisivelFilial()
		{
		}
		public CurriculoNaoVisivelFilial(Filial filial, Curriculo curriculo)
		{
			this._filial = filial;
			this._curriculo = curriculo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Curriculo_Nao_Visivel_Filial (Idf_Filial, Idf_Curriculo, Dta_Cadastro, Des_Motivo_Curriculo_Nao_Visivel_Filial, Idf_Motivo_Bloqueio) VALUES (@Idf_Filial, @Idf_Curriculo, @Dta_Cadastro, @Des_Motivo_Curriculo_Nao_Visivel_Filial, @Idf_Motivo_Bloqueio);";
		private const string SPUPDATE = "UPDATE BNE_Curriculo_Nao_Visivel_Filial SET Dta_Cadastro = @Dta_Cadastro, Des_Motivo_Curriculo_Nao_Visivel_Filial = @Des_Motivo_Curriculo_Nao_Visivel_Filial, Idf_Motivo_Bloqueio = @Idf_Motivo_Bloqueio WHERE Idf_Filial = @Idf_Filial AND Idf_Curriculo = @Idf_Curriculo";
		private const string SPDELETE = "DELETE FROM BNE_Curriculo_Nao_Visivel_Filial WHERE Idf_Filial = @Idf_Filial AND Idf_Curriculo = @Idf_Curriculo";
		private const string SPSELECTID = "SELECT * FROM BNE_Curriculo_Nao_Visivel_Filial WITH(NOLOCK) WHERE Idf_Filial = @Idf_Filial AND Idf_Curriculo = @Idf_Curriculo";
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
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Motivo_Curriculo_Nao_Visivel_Filial", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Motivo_Bloqueio", SqlDbType.Int, 4));
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
			parms[0].Value = this._filial.IdFilial;
			parms[1].Value = this._curriculo.IdCurriculo;

			if (!String.IsNullOrEmpty(this._descricaoMotivoCurriculoNaoVisivelFilial))
				parms[3].Value = this._descricaoMotivoCurriculoNaoVisivelFilial;
			else
				parms[3].Value = DBNull.Value;

			parms[4].Value = this.MotivoBloqueio;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CurriculoNaoVisivelFilial no banco de dados.
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
            //Inativa as candidatuas feitas para aquela empresa
            InativarCandidaturas(this._curriculo.IdCurriculo, this._filial.IdFilial);
		}
		/// <summary>
		/// Método utilizado para inserir uma instância de CurriculoNaoVisivelFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para atualizar uma instância de CurriculoNaoVisivelFilial no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CurriculoNaoVisivelFilial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CurriculoNaoVisivelFilial no banco de dados.
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
		/// Método utilizado para salvar uma instância de CurriculoNaoVisivelFilial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CurriculoNaoVisivelFilial no banco de dados.
		/// </summary>
		/// <param name="idFilial">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idFilial, int idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idFilial;
			parms[1].Value = idCurriculo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
            //Ativar de volta as candidaturas
            AtivarCandidaturas(idCurriculo, idFilial);

		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CurriculoNaoVisivelFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFilial">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idFilial, int idCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idFilial;
			parms[1].Value = idCurriculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idFilial">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idFilial, int idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idFilial;
			parms[1].Value = idCurriculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFilial">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idFilial, int idCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idFilial;
			parms[1].Value = idCurriculo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Filial, Cur.Idf_Curriculo, Cur.Dta_Cadastro, Cur.Des_Motivo_Curriculo_Nao_Visivel_Filial, Cur.Idf_Motivo_Bloqueio FROM BNE_Curriculo_Nao_Visivel_Filial Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoNaoVisivelFilial a partir do banco de dados.
		/// </summary>
		/// <param name="idFilial">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <returns>Instância de CurriculoNaoVisivelFilial.</returns>
		/// <remarks>Mailson</remarks>
		public static CurriculoNaoVisivelFilial LoadObject(int idFilial, int idCurriculo)
		{
			using (IDataReader dr = LoadDataReader(idFilial, idCurriculo))
			{
				CurriculoNaoVisivelFilial objCurriculoNaoVisivelFilial = new CurriculoNaoVisivelFilial();
				if (SetInstance(dr, objCurriculoNaoVisivelFilial))
					return objCurriculoNaoVisivelFilial;
			}
			throw (new RecordNotFoundException(typeof(CurriculoNaoVisivelFilial)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoNaoVisivelFilial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFilial">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CurriculoNaoVisivelFilial.</returns>
		/// <remarks>Mailson</remarks>
		public static CurriculoNaoVisivelFilial LoadObject(int idFilial, int idCurriculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idFilial, idCurriculo, trans))
			{
				CurriculoNaoVisivelFilial objCurriculoNaoVisivelFilial = new CurriculoNaoVisivelFilial();
				if (SetInstance(dr, objCurriculoNaoVisivelFilial))
					return objCurriculoNaoVisivelFilial;
			}
			throw (new RecordNotFoundException(typeof(CurriculoNaoVisivelFilial)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoNaoVisivelFilial a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._filial.IdFilial, this._curriculo.IdCurriculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoNaoVisivelFilial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._filial.IdFilial, this._curriculo.IdCurriculo, trans))
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
		/// <param name="objCurriculoNaoVisivelFilial">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		private static bool SetInstance(IDataReader dr, CurriculoNaoVisivelFilial objCurriculoNaoVisivelFilial, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objCurriculoNaoVisivelFilial._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
					objCurriculoNaoVisivelFilial._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objCurriculoNaoVisivelFilial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Des_Motivo_Curriculo_Nao_Visivel_Filial"] != DBNull.Value)
						objCurriculoNaoVisivelFilial._descricaoMotivoCurriculoNaoVisivelFilial = Convert.ToString(dr["Des_Motivo_Curriculo_Nao_Visivel_Filial"]);
					objCurriculoNaoVisivelFilial._motivoBloqueio = Convert.ToInt32(dr["Idf_Motivo_Bloqueio"]);

					objCurriculoNaoVisivelFilial._persisted = true;
					objCurriculoNaoVisivelFilial._modified = false;

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