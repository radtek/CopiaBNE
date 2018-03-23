//-- Data: 08/03/2012 17:24
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class OrigemFilial // Tabela: TAB_Origem_Filial
	{
		#region Atributos
		private int _idOrigemFilial;
		private Filial _filial;
		private Origem _origem;
		private DateTime _dataCadastro;
		private DateTime _dataAlteracao;
		private string _descricaoIP;
		private bool _flagInativo;
		private bool _flagCurriculoPublico;
		private string _descricaoDiretorio;
		private byte[] _imagemLogo;
		private Template _template;
		private string _descricaoMensagemCandidato;
		private string _descricaoPaginaInicial;
		private bool? _flagTodasFuncoes;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdOrigemFilial
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdOrigemFilial
		{
			get
			{
				return this._idOrigemFilial;
			}
		}
		#endregion 

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

		#region Origem
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Origem Origem
		{
			get
			{
				return this._origem;
			}
			set
			{
				this._origem = value;
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

		#region DescricaoIP
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoIP
		{
			get
			{
				return this._descricaoIP;
			}
			set
			{
				this._descricaoIP = value;
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

		#region FlagCurriculoPublico
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagCurriculoPublico
		{
			get
			{
				return this._flagCurriculoPublico;
			}
			set
			{
				this._flagCurriculoPublico = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoDiretorio
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoDiretorio
		{
			get
			{
				return this._descricaoDiretorio;
			}
			set
			{
				this._descricaoDiretorio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ImagemLogo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public byte[] ImagemLogo
		{
			get
			{
				return this._imagemLogo;
			}
			set
			{
				this._imagemLogo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Template
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Template Template
		{
			get
			{
				return this._template;
			}
			set
			{
				this._template = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMensagemCandidato
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string DescricaoMensagemCandidato
		{
			get
			{
				return this._descricaoMensagemCandidato;
			}
			set
			{
				this._descricaoMensagemCandidato = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPaginaInicial
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string DescricaoPaginaInicial
		{
			get
			{
				return this._descricaoPaginaInicial;
			}
			set
			{
				this._descricaoPaginaInicial = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagTodasFuncoes
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagTodasFuncoes
		{
			get
			{
				return this._flagTodasFuncoes;
			}
			set
			{
				this._flagTodasFuncoes = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public OrigemFilial()
		{
		}
		public OrigemFilial(int idOrigemFilial)
		{
			this._idOrigemFilial = idOrigemFilial;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Origem_Filial (Idf_Filial, Idf_Origem, Dta_Cadastro, Dta_Alteracao, Des_IP, Flg_Inativo, Flg_Curriculo_Publico, Des_Diretorio, Img_Logo, Idf_Template, Des_Mensagem_Candidato, Des_Pagina_Inicial, Flg_Todas_Funcoes) VALUES (@Idf_Filial, @Idf_Origem, @Dta_Cadastro, @Dta_Alteracao, @Des_IP, @Flg_Inativo, @Flg_Curriculo_Publico, @Des_Diretorio, @Img_Logo, @Idf_Template, @Des_Mensagem_Candidato, @Des_Pagina_Inicial, @Flg_Todas_Funcoes);SET @Idf_Origem_Filial = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Origem_Filial SET Idf_Filial = @Idf_Filial, Idf_Origem = @Idf_Origem, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Des_IP = @Des_IP, Flg_Inativo = @Flg_Inativo, Flg_Curriculo_Publico = @Flg_Curriculo_Publico, Des_Diretorio = @Des_Diretorio, Img_Logo = @Img_Logo, Idf_Template = @Idf_Template, Des_Mensagem_Candidato = @Des_Mensagem_Candidato, Des_Pagina_Inicial = @Des_Pagina_Inicial, Flg_Todas_Funcoes = @Flg_Todas_Funcoes WHERE Idf_Origem_Filial = @Idf_Origem_Filial";
		private const string SPDELETE = "DELETE FROM TAB_Origem_Filial WHERE Idf_Origem_Filial = @Idf_Origem_Filial";
		private const string SPSELECTID = "SELECT * FROM TAB_Origem_Filial WHERE Idf_Origem_Filial = @Idf_Origem_Filial";
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
			parms.Add(new SqlParameter("@Idf_Origem_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Origem", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_IP", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Curriculo_Publico", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_Diretorio", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Img_Logo", SqlDbType.VarBinary));
			parms.Add(new SqlParameter("@Idf_Template", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Mensagem_Candidato", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Des_Pagina_Inicial", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Flg_Todas_Funcoes", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idOrigemFilial;
			parms[1].Value = this._filial.IdFilial;
			parms[2].Value = this._origem.IdOrigem;
			parms[5].Value = this._descricaoIP;
			parms[6].Value = this._flagInativo;
			parms[7].Value = this._flagCurriculoPublico;

			if (!String.IsNullOrEmpty(this._descricaoDiretorio))
				parms[8].Value = this._descricaoDiretorio;
			else
				parms[8].Value = DBNull.Value;


			if (this._imagemLogo != null)
				parms[9].Value = this._imagemLogo;
			else
				parms[9].Value = DBNull.Value;


			if (this._template != null)
				parms[10].Value = this._template.IdTemplate;
			else
				parms[10].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoMensagemCandidato))
				parms[11].Value = this._descricaoMensagemCandidato;
			else
				parms[11].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoPaginaInicial))
				parms[12].Value = this._descricaoPaginaInicial;
			else
				parms[12].Value = DBNull.Value;


			if (this._flagTodasFuncoes.HasValue)
				parms[13].Value = this._flagTodasFuncoes;
			else
				parms[13].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[3].Value = this._dataCadastro;
			this._dataAlteracao = DateTime.Now;
			parms[4].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de OrigemFilial no banco de dados.
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
						this._idOrigemFilial = Convert.ToInt32(cmd.Parameters["@Idf_Origem_Filial"].Value);
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
		/// Método utilizado para inserir uma instância de OrigemFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idOrigemFilial = Convert.ToInt32(cmd.Parameters["@Idf_Origem_Filial"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de OrigemFilial no banco de dados.
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
		/// Método utilizado para atualizar uma instância de OrigemFilial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de OrigemFilial no banco de dados.
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
		/// Método utilizado para salvar uma instância de OrigemFilial no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de OrigemFilial no banco de dados.
		/// </summary>
		/// <param name="idOrigemFilial">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idOrigemFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Filial", SqlDbType.Int, 4));

			parms[0].Value = idOrigemFilial;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de OrigemFilial no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOrigemFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idOrigemFilial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Filial", SqlDbType.Int, 4));

			parms[0].Value = idOrigemFilial;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de OrigemFilial no banco de dados.
		/// </summary>
		/// <param name="idOrigemFilial">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idOrigemFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Origem_Filial where Idf_Origem_Filial in (";

			for (int i = 0; i < idOrigemFilial.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idOrigemFilial[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idOrigemFilial">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idOrigemFilial)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Filial", SqlDbType.Int, 4));

			parms[0].Value = idOrigemFilial;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOrigemFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idOrigemFilial, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Origem_Filial", SqlDbType.Int, 4));

			parms[0].Value = idOrigemFilial;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ori.Idf_Origem_Filial, Ori.Idf_Filial, Ori.Idf_Origem, Ori.Dta_Cadastro, Ori.Dta_Alteracao, Ori.Des_IP, Ori.Flg_Inativo, Ori.Flg_Curriculo_Publico, Ori.Des_Diretorio, Ori.Img_Logo, Ori.Idf_Template, Ori.Des_Mensagem_Candidato, Ori.Des_Pagina_Inicial, Ori.Flg_Todas_Funcoes FROM TAB_Origem_Filial Ori";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de OrigemFilial a partir do banco de dados.
		/// </summary>
		/// <param name="idOrigemFilial">Chave do registro.</param>
		/// <returns>Instância de OrigemFilial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static OrigemFilial LoadObject(int idOrigemFilial)
		{
			using (IDataReader dr = LoadDataReader(idOrigemFilial))
			{
				OrigemFilial objOrigemFilial = new OrigemFilial();
				if (SetInstance(dr, objOrigemFilial))
					return objOrigemFilial;
			}
			throw (new RecordNotFoundException(typeof(OrigemFilial)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de OrigemFilial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idOrigemFilial">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de OrigemFilial.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static OrigemFilial LoadObject(int idOrigemFilial, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idOrigemFilial, trans))
			{
				OrigemFilial objOrigemFilial = new OrigemFilial();
				if (SetInstance(dr, objOrigemFilial))
					return objOrigemFilial;
			}
			throw (new RecordNotFoundException(typeof(OrigemFilial)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de OrigemFilial a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idOrigemFilial))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de OrigemFilial a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idOrigemFilial, trans))
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
		/// <param name="objOrigemFilial">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, OrigemFilial objOrigemFilial)
		{
			try
			{
				if (dr.Read())
				{
					objOrigemFilial._idOrigemFilial = Convert.ToInt32(dr["Idf_Origem_Filial"]);
					objOrigemFilial._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
					objOrigemFilial._origem = new Origem(Convert.ToInt32(dr["Idf_Origem"]));
					objOrigemFilial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objOrigemFilial._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					objOrigemFilial._descricaoIP = Convert.ToString(dr["Des_IP"]);
					objOrigemFilial._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objOrigemFilial._flagCurriculoPublico = Convert.ToBoolean(dr["Flg_Curriculo_Publico"]);
					if (dr["Des_Diretorio"] != DBNull.Value)
						objOrigemFilial._descricaoDiretorio = Convert.ToString(dr["Des_Diretorio"]);
					if (dr["Img_Logo"] != DBNull.Value)
						objOrigemFilial._imagemLogo = (byte[])(dr["Img_Logo"]);
					if (dr["Idf_Template"] != DBNull.Value)
						objOrigemFilial._template = new Template(Convert.ToInt32(dr["Idf_Template"]));
					if (dr["Des_Mensagem_Candidato"] != DBNull.Value)
						objOrigemFilial._descricaoMensagemCandidato = Convert.ToString(dr["Des_Mensagem_Candidato"]);
					if (dr["Des_Pagina_Inicial"] != DBNull.Value)
						objOrigemFilial._descricaoPaginaInicial = Convert.ToString(dr["Des_Pagina_Inicial"]);
					if (dr["Flg_Todas_Funcoes"] != DBNull.Value)
						objOrigemFilial._flagTodasFuncoes = Convert.ToBoolean(dr["Flg_Todas_Funcoes"]);

					objOrigemFilial._persisted = true;
					objOrigemFilial._modified = false;

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