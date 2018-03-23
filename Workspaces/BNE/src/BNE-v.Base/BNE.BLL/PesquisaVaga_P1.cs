//-- Data: 16/01/2012 13:27
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PesquisaVaga // Tabela: TAB_Pesquisa_Vaga
	{
		#region Atributos
		private int _idPesquisaVaga;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private Curriculo _curriculo;
		private Funcao _funcao;
		private string _descricaoIP;
		private Cidade _cidade;
		private string _descricaoPalavraChave;
		private Estado _estado;
		private Escolaridade _escolaridade;
		private decimal? _numeroSalarioMin;
		private decimal? _numeroSalarioMax;
		private string _descricaoCodVaga;
		private string _razaoSocial;
		private AreaBNE _areaBNE;
		private Deficiencia _deficiencia;
		private DateTime? _dataCadastro;
		private bool _flagPesquisaAvancada;
        private int? _quantidadeVagaRetorno;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPesquisaVaga
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPesquisaVaga
		{
			get
			{
				return this._idPesquisaVaga;
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

		#region Curriculo
		/// <summary>
		/// Campo opcional.
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

		#region Funcao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Funcao Funcao
		{
			get
			{
				return this._funcao;
			}
			set
			{
				this._funcao = value;
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

		#region Cidade
		/// <summary>
		/// Campo opcional.
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

		#region DescricaoPalavraChave
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoPalavraChave
		{
			get
			{
				return this._descricaoPalavraChave;
			}
			set
			{
				this._descricaoPalavraChave = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Estado
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Estado Estado
		{
			get
			{
				return this._estado;
			}
			set
			{
				this._estado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Escolaridade
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Escolaridade Escolaridade
		{
			get
			{
				return this._escolaridade;
			}
			set
			{
				this._escolaridade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroSalarioMin
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroSalarioMin
		{
			get
			{
				return this._numeroSalarioMin;
			}
			set
			{
				this._numeroSalarioMin = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroSalarioMax
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroSalarioMax
		{
			get
			{
				return this._numeroSalarioMax;
			}
			set
			{
				this._numeroSalarioMax = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCodVaga
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCodVaga
		{
			get
			{
				return this._descricaoCodVaga;
			}
			set
			{
				this._descricaoCodVaga = value;
				this._modified = true;
			}
		}
		#endregion 

		#region RazaoSocial
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string RazaoSocial
		{
			get
			{
				return this._razaoSocial;
			}
			set
			{
				this._razaoSocial = value;
				this._modified = true;
			}
		}
		#endregion 

		#region AreaBNE
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public AreaBNE AreaBNE
		{
			get
			{
				return this._areaBNE;
			}
			set
			{
				this._areaBNE = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Deficiencia
		/// <summary>
		/// Campo opcional.
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

		#region FlagPesquisaAvancada
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagPesquisaAvancada
		{
			get
			{
				return this._flagPesquisaAvancada;
			}
			set
			{
				this._flagPesquisaAvancada = value;
				this._modified = true;
			}
		}
		#endregion 

        #region QuantidadeVagaRetorno
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? QuantidadeVagaRetorno
        {
            get
            {
                return this._quantidadeVagaRetorno;
            }
            set
            {
                this._quantidadeVagaRetorno = value;
                this._modified = true;
            }
        }
        #endregion 

		#endregion

		#region Construtores
		public PesquisaVaga()
		{
		}
		public PesquisaVaga(int idPesquisaVaga)
		{
			this._idPesquisaVaga = idPesquisaVaga;
			this._persisted = true;
		}
		#endregion

		#region Consultas
        private const string SPINSERT = "INSERT INTO TAB_Pesquisa_Vaga (Idf_Usuario_Filial_Perfil, Idf_Curriculo, Idf_Funcao, Des_IP, Idf_Cidade, Des_Palavra_Chave, Sig_Estado, Idf_Escolaridade, Num_Salario_Min, Num_Salario_Max, Des_Cod_Vaga, Raz_Social, Idf_Area_BNE, Idf_Deficiencia, Dta_Cadastro, Flg_Pesquisa_Avancada, Qtd_Vaga_Retorno) VALUES (@Idf_Usuario_Filial_Perfil, @Idf_Curriculo, @Idf_Funcao, @Des_IP, @Idf_Cidade, @Des_Palavra_Chave, @Sig_Estado, @Idf_Escolaridade, @Num_Salario_Min, @Num_Salario_Max, @Des_Cod_Vaga, @Raz_Social, @Idf_Area_BNE, @Idf_Deficiencia, @Dta_Cadastro, @Flg_Pesquisa_Avancada, @Qtd_Vaga_Retorno);SET @Idf_Pesquisa_Vaga = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE TAB_Pesquisa_Vaga SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Idf_Curriculo = @Idf_Curriculo, Idf_Funcao = @Idf_Funcao, Des_IP = @Des_IP, Idf_Cidade = @Idf_Cidade, Des_Palavra_Chave = @Des_Palavra_Chave, Sig_Estado = @Sig_Estado, Idf_Escolaridade = @Idf_Escolaridade, Num_Salario_Min = @Num_Salario_Min, Num_Salario_Max = @Num_Salario_Max, Des_Cod_Vaga = @Des_Cod_Vaga, Raz_Social = @Raz_Social, Idf_Area_BNE = @Idf_Area_BNE, Idf_Deficiencia = @Idf_Deficiencia, Dta_Cadastro = @Dta_Cadastro, Flg_Pesquisa_Avancada = @Flg_Pesquisa_Avancada, Qtd_Vaga_Retorno = @Qtd_Vaga_Retorno WHERE Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga";
		private const string SPDELETE = "DELETE FROM TAB_Pesquisa_Vaga WHERE Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga";
		private const string SPSELECTID = "SELECT * FROM TAB_Pesquisa_Vaga WHERE Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga";
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
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_IP", SqlDbType.Char, 15));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Palavra_Chave", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Sig_Estado", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_Salario_Min", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Num_Salario_Max", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Des_Cod_Vaga", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Raz_Social", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Area_BNE", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Pesquisa_Avancada", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Qtd_Vaga_Retorno", SqlDbType.Int, 4));
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
			parms[0].Value = this._idPesquisaVaga;

			if (this._usuarioFilialPerfil != null)
				parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
			else
				parms[1].Value = DBNull.Value;


			if (this._curriculo != null)
				parms[2].Value = this._curriculo.IdCurriculo;
			else
				parms[2].Value = DBNull.Value;


			if (this._funcao != null)
				parms[3].Value = this._funcao.IdFuncao;
			else
				parms[3].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoIP))
				parms[4].Value = this._descricaoIP;
			else
				parms[4].Value = DBNull.Value;


			if (this._cidade != null)
				parms[5].Value = this._cidade.IdCidade;
			else
				parms[5].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoPalavraChave))
				parms[6].Value = this._descricaoPalavraChave;
			else
				parms[6].Value = DBNull.Value;


			if (this._estado != null)
				parms[7].Value = this._estado.SiglaEstado;
			else
				parms[7].Value = DBNull.Value;


			if (this._escolaridade != null)
				parms[8].Value = this._escolaridade.IdEscolaridade;
			else
				parms[8].Value = DBNull.Value;


			if (this._numeroSalarioMin.HasValue)
				parms[9].Value = this._numeroSalarioMin;
			else
				parms[9].Value = DBNull.Value;


			if (this._numeroSalarioMax.HasValue)
				parms[10].Value = this._numeroSalarioMax;
			else
				parms[10].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoCodVaga))
				parms[11].Value = this._descricaoCodVaga;
			else
				parms[11].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._razaoSocial))
				parms[12].Value = this._razaoSocial;
			else
				parms[12].Value = DBNull.Value;


			if (this._areaBNE != null)
				parms[13].Value = this._areaBNE.IdAreaBNE;
			else
				parms[13].Value = DBNull.Value;


			if (this._deficiencia != null)
				parms[14].Value = this._deficiencia.IdDeficiencia;
			else
				parms[14].Value = DBNull.Value;

			parms[16].Value = this._flagPesquisaAvancada;

            if (this._quantidadeVagaRetorno.HasValue)
                parms[17].Value = this._quantidadeVagaRetorno;
            else
                parms[17].Value = DBNull.Value;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[15].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de PesquisaVaga no banco de dados.
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
						this._idPesquisaVaga = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Vaga"].Value);
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
		/// Método utilizado para inserir uma instância de PesquisaVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPesquisaVaga = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Vaga"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PesquisaVaga no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PesquisaVaga no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PesquisaVaga no banco de dados.
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
		/// Método utilizado para salvar uma instância de PesquisaVaga no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PesquisaVaga no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVaga">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPesquisaVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVaga;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PesquisaVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPesquisaVaga, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVaga;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PesquisaVaga no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVaga">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPesquisaVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pesquisa_Vaga where Idf_Pesquisa_Vaga in (";

			for (int i = 0; i < idPesquisaVaga.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPesquisaVaga[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVaga">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPesquisaVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVaga;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPesquisaVaga, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVaga;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pesquisa_Vaga, Pes.Idf_Usuario_Filial_Perfil, Pes.Idf_Curriculo, Pes.Idf_Funcao, Pes.Des_IP, Pes.Idf_Cidade, Pes.Des_Palavra_Chave, Pes.Sig_Estado, Pes.Idf_Escolaridade, Pes.Num_Salario_Min, Pes.Num_Salario_Max, Pes.Des_Cod_Vaga, Pes.Raz_Social, Pes.Idf_Area_BNE, Pes.Idf_Deficiencia, Pes.Dta_Cadastro, Pes.Flg_Pesquisa_Avancada FROM TAB_Pesquisa_Vaga Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaVaga a partir do banco de dados.
		/// </summary>
		/// <param name="idPesquisaVaga">Chave do registro.</param>
		/// <returns>Instância de PesquisaVaga.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PesquisaVaga LoadObject(int idPesquisaVaga)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaVaga))
			{
				PesquisaVaga objPesquisaVaga = new PesquisaVaga();
				if (SetInstance(dr, objPesquisaVaga))
					return objPesquisaVaga;
			}
			throw (new RecordNotFoundException(typeof(PesquisaVaga)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaVaga a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PesquisaVaga.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PesquisaVaga LoadObject(int idPesquisaVaga, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaVaga, trans))
			{
				PesquisaVaga objPesquisaVaga = new PesquisaVaga();
				if (SetInstance(dr, objPesquisaVaga))
					return objPesquisaVaga;
			}
			throw (new RecordNotFoundException(typeof(PesquisaVaga)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaVaga a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaVaga))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaVaga a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaVaga, trans))
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
		/// <param name="objPesquisaVaga">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PesquisaVaga objPesquisaVaga)
		{
			try
			{
				if (dr.Read())
				{
					objPesquisaVaga._idPesquisaVaga = Convert.ToInt32(dr["Idf_Pesquisa_Vaga"]);
					if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
						objPesquisaVaga._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					if (dr["Idf_Curriculo"] != DBNull.Value)
						objPesquisaVaga._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					if (dr["Idf_Funcao"] != DBNull.Value)
						objPesquisaVaga._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
					if (dr["Des_IP"] != DBNull.Value)
						objPesquisaVaga._descricaoIP = Convert.ToString(dr["Des_IP"]);
					if (dr["Idf_Cidade"] != DBNull.Value)
						objPesquisaVaga._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					if (dr["Des_Palavra_Chave"] != DBNull.Value)
						objPesquisaVaga._descricaoPalavraChave = Convert.ToString(dr["Des_Palavra_Chave"]);
					if (dr["Sig_Estado"] != DBNull.Value)
						objPesquisaVaga._estado = new Estado(dr["Sig_Estado"].ToString());
					if (dr["Idf_Escolaridade"] != DBNull.Value)
						objPesquisaVaga._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
					if (dr["Num_Salario_Min"] != DBNull.Value)
						objPesquisaVaga._numeroSalarioMin = Convert.ToDecimal(dr["Num_Salario_Min"]);
					if (dr["Num_Salario_Max"] != DBNull.Value)
						objPesquisaVaga._numeroSalarioMax = Convert.ToDecimal(dr["Num_Salario_Max"]);
					if (dr["Des_Cod_Vaga"] != DBNull.Value)
						objPesquisaVaga._descricaoCodVaga = Convert.ToString(dr["Des_Cod_Vaga"]);
					if (dr["Raz_Social"] != DBNull.Value)
						objPesquisaVaga._razaoSocial = Convert.ToString(dr["Raz_Social"]);
					if (dr["Idf_Area_BNE"] != DBNull.Value)
						objPesquisaVaga._areaBNE = new AreaBNE(Convert.ToInt32(dr["Idf_Area_BNE"]));
					if (dr["Idf_Deficiencia"] != DBNull.Value)
						objPesquisaVaga._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objPesquisaVaga._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objPesquisaVaga._flagPesquisaAvancada = Convert.ToBoolean(dr["Flg_Pesquisa_Avancada"]);
                    if (dr["Qtd_Vaga_Retorno"] != DBNull.Value)
                        objPesquisaVaga._quantidadeVagaRetorno = Convert.ToInt32(dr["Qtd_Vaga_Retorno"]);

					objPesquisaVaga._persisted = true;
					objPesquisaVaga._modified = false;

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