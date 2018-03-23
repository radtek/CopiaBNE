//-- Data: 25/08/2010 17:07
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Escolaridade // Tabela: plataforma.TAB_Escolaridade
	{
		#region Atributos
		private int _idEscolaridade;
		private string _descricaoGeral;
		private string _descricaoBNE;
		private string _descricaoRais;
		private decimal _codigoRais;
		private int _codigoCaged;
		private int _codigoGRRF;
		private bool _flagBNE;
		private bool _flagFolha;
		private int? _sequenciaBNE;
		private string _descricaoAbreviada;
		private GrauEscolaridade _grauEscolaridade;
		private bool _flagEscolaridadeCompleta;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private Int16? _sequenciaPeso;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdEscolaridade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdEscolaridade
		{
			get
			{
				return this._idEscolaridade;
			}
			set
			{
				this._idEscolaridade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoGeral
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoGeral
		{
			get
			{
				return this._descricaoGeral;
			}
			set
			{
				this._descricaoGeral = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoBNE
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoBNE
		{
			get
			{
				return this._descricaoBNE;
			}
			set
			{
				this._descricaoBNE = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoRais
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoRais
		{
			get
			{
				return this._descricaoRais;
			}
			set
			{
				this._descricaoRais = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoRais
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal CodigoRais
		{
			get
			{
				return this._codigoRais;
			}
			set
			{
				this._codigoRais = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoCaged
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int CodigoCaged
		{
			get
			{
				return this._codigoCaged;
			}
			set
			{
				this._codigoCaged = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoGRRF
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int CodigoGRRF
		{
			get
			{
				return this._codigoGRRF;
			}
			set
			{
				this._codigoGRRF = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagBNE
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagBNE
		{
			get
			{
				return this._flagBNE;
			}
			set
			{
				this._flagBNE = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagFolha
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagFolha
		{
			get
			{
				return this._flagFolha;
			}
			set
			{
				this._flagFolha = value;
				this._modified = true;
			}
		}
		#endregion 

		#region SequenciaBNE
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? SequenciaBNE
		{
			get
			{
				return this._sequenciaBNE;
			}
			set
			{
				this._sequenciaBNE = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoAbreviada
		/// <summary>
		/// Tamanho do campo: 8.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoAbreviada
		{
			get
			{
				return this._descricaoAbreviada;
			}
			set
			{
				this._descricaoAbreviada = value;
				this._modified = true;
			}
		}
		#endregion 

		#region GrauEscolaridade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public GrauEscolaridade GrauEscolaridade
		{
			get
			{
				return this._grauEscolaridade;
			}
			set
			{
				this._grauEscolaridade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagEscolaridadeCompleta
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagEscolaridadeCompleta
		{
			get
			{
				return this._flagEscolaridadeCompleta;
			}
			set
			{
				this._flagEscolaridadeCompleta = value;
				this._modified = true;
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

		#region SequenciaPeso
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Int16? SequenciaPeso
		{
			get
			{
				return this._sequenciaPeso;
			}
			set
			{
				this._sequenciaPeso = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Escolaridade()
		{
		}
		public Escolaridade(int idEscolaridade)
		{
			this._idEscolaridade = idEscolaridade;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Escolaridade (Idf_Escolaridade, Des_Geral, Des_BNE, Des_Rais, Cod_Rais, Cod_Caged, Cod_GRRF, Flg_BNE, Flg_Folha, Seq_BNE, Des_Abreviada, Idf_Grau_Escolaridade, Flg_Escolaridade_Completa, Flg_Inativo, Dta_Cadastro, Seq_Peso) VALUES (@Idf_Escolaridade, @Des_Geral, @Des_BNE, @Des_Rais, @Cod_Rais, @Cod_Caged, @Cod_GRRF, @Flg_BNE, @Flg_Folha, @Seq_BNE, @Des_Abreviada, @Idf_Grau_Escolaridade, @Flg_Escolaridade_Completa, @Flg_Inativo, @Dta_Cadastro, @Seq_Peso);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Escolaridade SET Des_Geral = @Des_Geral, Des_BNE = @Des_BNE, Des_Rais = @Des_Rais, Cod_Rais = @Cod_Rais, Cod_Caged = @Cod_Caged, Cod_GRRF = @Cod_GRRF, Flg_BNE = @Flg_BNE, Flg_Folha = @Flg_Folha, Seq_BNE = @Seq_BNE, Des_Abreviada = @Des_Abreviada, Idf_Grau_Escolaridade = @Idf_Grau_Escolaridade, Flg_Escolaridade_Completa = @Flg_Escolaridade_Completa, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Seq_Peso = @Seq_Peso WHERE Idf_Escolaridade = @Idf_Escolaridade";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Escolaridade WHERE Idf_Escolaridade = @Idf_Escolaridade";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Escolaridade WHERE Idf_Escolaridade = @Idf_Escolaridade";
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
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Geral", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Des_BNE", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Des_Rais", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Cod_Rais", SqlDbType.Decimal, 5));
			parms.Add(new SqlParameter("@Cod_Caged", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Cod_GRRF", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_BNE", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Folha", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Seq_BNE", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Abreviada", SqlDbType.VarChar, 8));
			parms.Add(new SqlParameter("@Idf_Grau_Escolaridade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Escolaridade_Completa", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Seq_Peso", SqlDbType.Int, 2));
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
			parms[0].Value = this._idEscolaridade;

			if (!String.IsNullOrEmpty(this._descricaoGeral))
				parms[1].Value = this._descricaoGeral;
			else
				parms[1].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoBNE))
				parms[2].Value = this._descricaoBNE;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoRais))
				parms[3].Value = this._descricaoRais;
			else
				parms[3].Value = DBNull.Value;

			parms[4].Value = this._codigoRais;
			parms[5].Value = this._codigoCaged;
			parms[6].Value = this._codigoGRRF;
			parms[7].Value = this._flagBNE;
			parms[8].Value = this._flagFolha;

			if (this._sequenciaBNE.HasValue)
				parms[9].Value = this._sequenciaBNE;
			else
				parms[9].Value = DBNull.Value;

			parms[10].Value = this._descricaoAbreviada;
			parms[11].Value = this._grauEscolaridade.IdGrauEscolaridade;
			parms[12].Value = this._flagEscolaridadeCompleta;
			parms[13].Value = this._flagInativo;

			if (this._sequenciaPeso.HasValue)
				parms[15].Value = this._sequenciaPeso;
			else
				parms[15].Value = DBNull.Value;


			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[14].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Escolaridade no banco de dados.
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
		/// Método utilizado para inserir uma instância de Escolaridade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de Escolaridade no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Escolaridade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Escolaridade no banco de dados.
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
		/// Método utilizado para salvar uma instância de Escolaridade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Escolaridade no banco de dados.
		/// </summary>
		/// <param name="idEscolaridade">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idEscolaridade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));

			parms[0].Value = idEscolaridade;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Escolaridade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEscolaridade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idEscolaridade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));

			parms[0].Value = idEscolaridade;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Escolaridade no banco de dados.
		/// </summary>
		/// <param name="idEscolaridade">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idEscolaridade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Escolaridade where Idf_Escolaridade in (";

			for (int i = 0; i < idEscolaridade.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idEscolaridade[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idEscolaridade">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idEscolaridade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));

			parms[0].Value = idEscolaridade;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEscolaridade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idEscolaridade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));

			parms[0].Value = idEscolaridade;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Esc.Idf_Escolaridade, Esc.Des_Geral, Esc.Des_BNE, Esc.Des_Rais, Esc.Cod_Rais, Esc.Cod_Caged, Esc.Cod_GRRF, Esc.Flg_BNE, Esc.Flg_Folha, Esc.Seq_BNE, Esc.Des_Abreviada, Esc.Idf_Grau_Escolaridade, Esc.Flg_Escolaridade_Completa, Esc.Flg_Inativo, Esc.Dta_Cadastro, Esc.Seq_Peso FROM plataforma.TAB_Escolaridade Esc";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objEscolaridade">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Escolaridade objEscolaridade)
		{
			try
			{
				if (dr.Read())
				{
					objEscolaridade._idEscolaridade = Convert.ToInt32(dr["Idf_Escolaridade"]);
					if (dr["Des_Geral"] != DBNull.Value)
						objEscolaridade._descricaoGeral = Convert.ToString(dr["Des_Geral"]);
					if (dr["Des_BNE"] != DBNull.Value)
						objEscolaridade._descricaoBNE = Convert.ToString(dr["Des_BNE"]);
					if (dr["Des_Rais"] != DBNull.Value)
						objEscolaridade._descricaoRais = Convert.ToString(dr["Des_Rais"]);
					objEscolaridade._codigoRais = Convert.ToDecimal(dr["Cod_Rais"]);
					objEscolaridade._codigoCaged = Convert.ToInt32(dr["Cod_Caged"]);
					objEscolaridade._codigoGRRF = Convert.ToInt32(dr["Cod_GRRF"]);
					objEscolaridade._flagBNE = Convert.ToBoolean(dr["Flg_BNE"]);
					objEscolaridade._flagFolha = Convert.ToBoolean(dr["Flg_Folha"]);
					if (dr["Seq_BNE"] != DBNull.Value)
						objEscolaridade._sequenciaBNE = Convert.ToInt32(dr["Seq_BNE"]);
					objEscolaridade._descricaoAbreviada = Convert.ToString(dr["Des_Abreviada"]);
					objEscolaridade._grauEscolaridade = new GrauEscolaridade(Convert.ToInt32(dr["Idf_Grau_Escolaridade"]));
					objEscolaridade._flagEscolaridadeCompleta = Convert.ToBoolean(dr["Flg_Escolaridade_Completa"]);
					objEscolaridade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objEscolaridade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Seq_Peso"] != DBNull.Value)
						objEscolaridade._sequenciaPeso = Convert.ToInt16(dr["Seq_Peso"]);

					objEscolaridade._persisted = true;
					objEscolaridade._modified = false;

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