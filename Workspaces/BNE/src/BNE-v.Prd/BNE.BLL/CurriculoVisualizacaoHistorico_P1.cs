//-- Data: 18/01/2016 15:40
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CurriculoVisualizacaoHistorico // Tabela: BNE_Curriculo_Visualizacao_Historico
	{
		#region Atributos
		private Int64 _idCurriculoVisualizacao;
		private Filial _filial;
		private Curriculo _curriculo;
		private DateTime _dataVisualizacao;
		private string _descricaoIP;
		private bool _flagCurriculoVIP;
		private bool? _flagVisualizacaoCompleta;
		private string _descricaoMensagemErro;
		private PesquisaCurriculo _pesquisaCurriculo;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private Vaga _vaga;
		private RastreadorCurriculo _rastreadorCurriculo;
		private bool? _flagBaseGratis;

        private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCurriculoVisualizacao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public Int64 IdCurriculoVisualizacao
		{
			get
			{
				return this._idCurriculoVisualizacao;
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

		#region DataVisualizacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataVisualizacao
		{
			get
			{
				return this._dataVisualizacao;
			}
			set
			{
				this._dataVisualizacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoIP
		/// <summary>
		/// Tamanho do campo: 15.
		/// Campo opcional.
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

		#region FlagCurriculoVIP
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagCurriculoVIP
		{
			get
			{
				return this._flagCurriculoVIP;
			}
			set
			{
				this._flagCurriculoVIP = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagVisualizacaoCompleta
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagVisualizacaoCompleta
		{
			get
			{
				return this._flagVisualizacaoCompleta;
			}
			set
			{
				this._flagVisualizacaoCompleta = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMensagemErro
		/// <summary>
		/// Tamanho do campo: 255.
		/// Campo opcional.
		/// </summary>
		public string DescricaoMensagemErro
		{
			get
			{
				return this._descricaoMensagemErro;
			}
			set
			{
				this._descricaoMensagemErro = value;
				this._modified = true;
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

		#region UsuarioFilialPerfil
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public UsuarioFilialPerfil UsuarioFilialPerfil
		{
			get
			{
				return this._usuarioFilialPerfil;
			}
			set
			{
				this._usuarioFilialPerfil = value;
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

		#region RastreadorCurriculo
		/// <summary>
		/// Campo opcional.
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

        #region FlagBaseGratis
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagBaseGratis
        {
            get
            {
                return this._flagBaseGratis;
            }
            set
            {
                this._flagBaseGratis = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public CurriculoVisualizacaoHistorico()
		{
		}
		public CurriculoVisualizacaoHistorico(Int64 idCurriculoVisualizacao)
		{
			this._idCurriculoVisualizacao = idCurriculoVisualizacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Curriculo_Visualizacao_Historico (Idf_Filial, Idf_Curriculo, Dta_Visualizacao, Des_IP, Flg_Curriculo_VIP, Flg_Visualizacao_Completa, Des_Mensagem_Erro, Idf_Pesquisa_Curriculo, Idf_Usuario_Filial_Perfil, Idf_Vaga, Idf_Rastreador_Curriculo, Flg_Base_Gratis) VALUES (@Idf_Filial, @Idf_Curriculo, @Dta_Visualizacao, @Des_IP, @Flg_Curriculo_VIP, @Flg_Visualizacao_Completa, @Des_Mensagem_Erro, @Idf_Pesquisa_Curriculo, @Idf_Usuario_Filial_Perfil, @Idf_Vaga, @Idf_Rastreador_Curriculo, @Flg_Base_Gratis);SET @Idf_Curriculo_Visualizacao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Curriculo_Visualizacao_Historico SET Idf_Filial = @Idf_Filial, Idf_Curriculo = @Idf_Curriculo, Dta_Visualizacao = @Dta_Visualizacao, Des_IP = @Des_IP, Flg_Curriculo_VIP = @Flg_Curriculo_VIP, Flg_Visualizacao_Completa = @Flg_Visualizacao_Completa, Des_Mensagem_Erro = @Des_Mensagem_Erro, Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Idf_Vaga = @Idf_Vaga, Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo, Flg_Base_Gratis = @Flg_Base_Gratis WHERE Idf_Curriculo_Visualizacao = @Idf_Curriculo_Visualizacao";
		private const string SPDELETE = "DELETE FROM BNE_Curriculo_Visualizacao_Historico WHERE Idf_Curriculo_Visualizacao = @Idf_Curriculo_Visualizacao";
		private const string SPSELECTID = "SELECT * FROM BNE_Curriculo_Visualizacao_Historico WITH(NOLOCK) WHERE Idf_Curriculo_Visualizacao = @Idf_Curriculo_Visualizacao";
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
			parms.Add(new SqlParameter("@Idf_Curriculo_Visualizacao", SqlDbType.BigInt, 8));
			parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Visualizacao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_IP", SqlDbType.VarChar, 15));
			parms.Add(new SqlParameter("@Flg_Curriculo_VIP", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Visualizacao_Completa", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_Mensagem_Erro", SqlDbType.VarChar, 255));
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Base_Gratis", SqlDbType.Bit, 1));
            return (parms);
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
			parms[0].Value = this._idCurriculoVisualizacao;
			parms[1].Value = this._filial.IdFilial;
			parms[2].Value = this._curriculo.IdCurriculo;
			parms[3].Value = this._dataVisualizacao;

			if (!String.IsNullOrEmpty(this._descricaoIP))
				parms[4].Value = this._descricaoIP;
			else
				parms[4].Value = DBNull.Value;

			parms[5].Value = this._flagCurriculoVIP;

			if (this._flagVisualizacaoCompleta.HasValue)
				parms[6].Value = this._flagVisualizacaoCompleta;
			else
				parms[6].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoMensagemErro))
				parms[7].Value = this._descricaoMensagemErro;
			else
				parms[7].Value = DBNull.Value;


			if (this._pesquisaCurriculo != null)
				parms[8].Value = this._pesquisaCurriculo.IdPesquisaCurriculo;
			else
				parms[8].Value = DBNull.Value;


			if (this._usuarioFilialPerfil != null)
				parms[9].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
			else
				parms[9].Value = DBNull.Value;


			if (this._vaga != null)
				parms[10].Value = this._vaga.IdVaga;
			else
				parms[10].Value = DBNull.Value;


			if (this._rastreadorCurriculo != null)
				parms[11].Value = this._rastreadorCurriculo.IdRastreadorCurriculo;
			else
				parms[11].Value = DBNull.Value;

            if (this._flagBaseGratis.HasValue)
                parms[12].Value = this._flagBaseGratis;
            else
                parms[12].Value = DBNull.Value;

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
		/// Método utilizado para inserir uma instância de CurriculoVisualizacaoHistorico no banco de dados.
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
						this._idCurriculoVisualizacao = Convert.ToInt64(cmd.Parameters["@Idf_Curriculo_Visualizacao"].Value);
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
		/// Método utilizado para inserir uma instância de CurriculoVisualizacaoHistorico no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCurriculoVisualizacao = Convert.ToInt64(cmd.Parameters["@Idf_Curriculo_Visualizacao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CurriculoVisualizacaoHistorico no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CurriculoVisualizacaoHistorico no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CurriculoVisualizacaoHistorico no banco de dados.
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
		/// Método utilizado para salvar uma instância de CurriculoVisualizacaoHistorico no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CurriculoVisualizacaoHistorico no banco de dados.
		/// </summary>
		/// <param name="idCurriculoVisualizacao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(Int64 idCurriculoVisualizacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Visualizacao", SqlDbType.BigInt, 8));

			parms[0].Value = idCurriculoVisualizacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CurriculoVisualizacaoHistorico no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoVisualizacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(Int64 idCurriculoVisualizacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Visualizacao", SqlDbType.BigInt, 8));

			parms[0].Value = idCurriculoVisualizacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CurriculoVisualizacaoHistorico no banco de dados.
		/// </summary>
		/// <param name="idCurriculoVisualizacao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<Int64> idCurriculoVisualizacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Curriculo_Visualizacao_Historico where Idf_Curriculo_Visualizacao in (";

			for (int i = 0; i < idCurriculoVisualizacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.BigInt, 8));
				parms[i].Value = idCurriculoVisualizacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurriculoVisualizacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(Int64 idCurriculoVisualizacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Visualizacao", SqlDbType.BigInt, 8));

			parms[0].Value = idCurriculoVisualizacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoVisualizacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(Int64 idCurriculoVisualizacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo_Visualizacao", SqlDbType.BigInt, 8));

			parms[0].Value = idCurriculoVisualizacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curriculo_Visualizacao, Cur.Idf_Filial, Cur.Idf_Curriculo, Cur.Dta_Visualizacao, Cur.Des_IP, Cur.Flg_Curriculo_VIP, Cur.Flg_Visualizacao_Completa, Cur.Des_Mensagem_Erro, Cur.Idf_Pesquisa_Curriculo, Cur.Idf_Usuario_Filial_Perfil, Cur.Idf_Vaga, Cur.Idf_Rastreador_Curriculo FROM BNE_Curriculo_Visualizacao_Historico Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoVisualizacaoHistorico a partir do banco de dados.
		/// </summary>
		/// <param name="idCurriculoVisualizacao">Chave do registro.</param>
		/// <returns>Instância de CurriculoVisualizacaoHistorico.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CurriculoVisualizacaoHistorico LoadObject(Int64 idCurriculoVisualizacao)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoVisualizacao))
			{
				CurriculoVisualizacaoHistorico objCurriculoVisualizacaoHistorico = new CurriculoVisualizacaoHistorico();
				if (SetInstance(dr, objCurriculoVisualizacaoHistorico))
					return objCurriculoVisualizacaoHistorico;
			}
			throw (new RecordNotFoundException(typeof(CurriculoVisualizacaoHistorico)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CurriculoVisualizacaoHistorico a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculoVisualizacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CurriculoVisualizacaoHistorico.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CurriculoVisualizacaoHistorico LoadObject(Int64 idCurriculoVisualizacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurriculoVisualizacao, trans))
			{
				CurriculoVisualizacaoHistorico objCurriculoVisualizacaoHistorico = new CurriculoVisualizacaoHistorico();
				if (SetInstance(dr, objCurriculoVisualizacaoHistorico))
					return objCurriculoVisualizacaoHistorico;
			}
			throw (new RecordNotFoundException(typeof(CurriculoVisualizacaoHistorico)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoVisualizacaoHistorico a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoVisualizacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CurriculoVisualizacaoHistorico a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculoVisualizacao, trans))
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
		/// <param name="objCurriculoVisualizacaoHistorico">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CurriculoVisualizacaoHistorico objCurriculoVisualizacaoHistorico, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objCurriculoVisualizacaoHistorico._idCurriculoVisualizacao = Convert.ToInt64(dr["Idf_Curriculo_Visualizacao"]);
					objCurriculoVisualizacaoHistorico._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
					objCurriculoVisualizacaoHistorico._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objCurriculoVisualizacaoHistorico._dataVisualizacao = Convert.ToDateTime(dr["Dta_Visualizacao"]);
					if (dr["Des_IP"] != DBNull.Value)
						objCurriculoVisualizacaoHistorico._descricaoIP = Convert.ToString(dr["Des_IP"]);
					objCurriculoVisualizacaoHistorico._flagCurriculoVIP = Convert.ToBoolean(dr["Flg_Curriculo_VIP"]);
					if (dr["Flg_Visualizacao_Completa"] != DBNull.Value)
						objCurriculoVisualizacaoHistorico._flagVisualizacaoCompleta = Convert.ToBoolean(dr["Flg_Visualizacao_Completa"]);
					if (dr["Des_Mensagem_Erro"] != DBNull.Value)
						objCurriculoVisualizacaoHistorico._descricaoMensagemErro = Convert.ToString(dr["Des_Mensagem_Erro"]);
					if (dr["Idf_Pesquisa_Curriculo"] != DBNull.Value)
						objCurriculoVisualizacaoHistorico._pesquisaCurriculo = new PesquisaCurriculo(Convert.ToInt32(dr["Idf_Pesquisa_Curriculo"]));
					if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
						objCurriculoVisualizacaoHistorico._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					if (dr["Idf_Vaga"] != DBNull.Value)
						objCurriculoVisualizacaoHistorico._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
					if (dr["Idf_Rastreador_Curriculo"] != DBNull.Value)
						objCurriculoVisualizacaoHistorico._rastreadorCurriculo = new RastreadorCurriculo(Convert.ToInt32(dr["Idf_Rastreador_Curriculo"]));

					objCurriculoVisualizacaoHistorico._persisted = true;
					objCurriculoVisualizacaoHistorico._modified = false;

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