//-- Data: 04/04/2013 15:24
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Integracao // Tabela: plataforma.BNE_Integracao
	{
		#region Atributos
		private int _idIntegracao;
		private decimal? _numeroCPF;
		private string _nomePessoa;
		private string _apelidoPessoa;
		private Sexo _sexo;
		private DateTime _dataNascimento;
		private string _nomeMae;
		private string _nomePai;
		private string _numeroRG;
		private DateTime? _dataExpedicaoRG;
		private string _nomeOrgaoEmissor;
		private string _siglaUFEmissaoRG;
		private Raca _raca;
		private Deficiencia _deficiencia;
		private string _descricaoLogradouro;
		private string _descricaoComplemento;
		private string _numeroEndereco;
		private string _numeroCEP;
		private string _descricaoBairro;
		private Cidade _cidade;
		private string _numeroDDDTelefone;
		private string _numeroTelefone;
		private string _numeroDDDCelular;
		private string _numeroCelular;
		private string _emailPessoa;
		private Escolaridade _escolaridade;
		private EstadoCivil _estadoCivil;
		private string _razaoSocial;
		private Funcao _funcao;
		private DateTime? _dataAdmissao;
		private DateTime? _dataSaidaPrevista;
		private decimal _valorSalario;
		private string _numeroHabilitacao;
		private CategoriaHabilitacao _categoriaHabilitacao;
		private bool? _flagfilhos;
		private TipoVeiculo _tipoVeiculo;
		private int? _anoVeiculo;
		private DateTime _dataCadastro;
		private DateTime? _dataIntegracao;
		private IntegracaoSituacao _integracaoSituacao;
		private TipoVinculoIntegracao _tipoVinculoIntegracao;
		private MotivoRescisao _motivoRescisao;
        private bool _flg_Admitido;
        private string _des_CS;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdIntegracao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdIntegracao
		{
			get
			{
				return this._idIntegracao;
			}
		}
		#endregion 

		#region NumeroCPF
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroCPF
		{
			get
			{
				return this._numeroCPF;
			}
			set
			{
				this._numeroCPF = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomePessoa
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string NomePessoa
		{
			get
			{
				return this._nomePessoa;
			}
			set
			{
				this._nomePessoa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ApelidoPessoa
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo opcional.
		/// </summary>
		public string ApelidoPessoa
		{
			get
			{
				return this._apelidoPessoa;
			}
			set
			{
				this._apelidoPessoa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Sexo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Sexo Sexo
		{
			get
			{
				return this._sexo;
			}
			set
			{
				this._sexo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataNascimento
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataNascimento
		{
			get
			{
				return this._dataNascimento;
			}
			set
			{
				this._dataNascimento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeMae
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string NomeMae
		{
			get
			{
				return this._nomeMae;
			}
			set
			{
				this._nomeMae = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomePai
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string NomePai
		{
			get
			{
				return this._nomePai;
			}
			set
			{
				this._nomePai = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroRG
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo opcional.
		/// </summary>
		public string NumeroRG
		{
			get
			{
				return this._numeroRG;
			}
			set
			{
				this._numeroRG = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataExpedicaoRG
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataExpedicaoRG
		{
			get
			{
				return this._dataExpedicaoRG;
			}
			set
			{
				this._dataExpedicaoRG = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeOrgaoEmissor
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo opcional.
		/// </summary>
		public string NomeOrgaoEmissor
		{
			get
			{
				return this._nomeOrgaoEmissor;
			}
			set
			{
				this._nomeOrgaoEmissor = value;
				this._modified = true;
			}
		}
		#endregion 

		#region SiglaUFEmissaoRG
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo opcional.
		/// </summary>
		public string SiglaUFEmissaoRG
		{
			get
			{
				return this._siglaUFEmissaoRG;
			}
			set
			{
				this._siglaUFEmissaoRG = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Raca
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Raca Raca
		{
			get
			{
				return this._raca;
			}
			set
			{
				this._raca = value;
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

		#region DescricaoLogradouro
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoLogradouro
		{
			get
			{
				return this._descricaoLogradouro;
			}
			set
			{
				this._descricaoLogradouro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoComplemento
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo opcional.
		/// </summary>
		public string DescricaoComplemento
		{
			get
			{
				return this._descricaoComplemento;
			}
			set
			{
				this._descricaoComplemento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroEndereco
		/// <summary>
		/// Tamanho do campo: 15.
		/// Campo opcional.
		/// </summary>
		public string NumeroEndereco
		{
			get
			{
				return this._numeroEndereco;
			}
			set
			{
				this._numeroEndereco = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCEP
		/// <summary>
		/// Tamanho do campo: 8.
		/// Campo opcional.
		/// </summary>
		public string NumeroCEP
		{
			get
			{
				return this._numeroCEP;
			}
			set
			{
				this._numeroCEP = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoBairro
		/// <summary>
		/// Tamanho do campo: 80.
		/// Campo opcional.
		/// </summary>
		public string DescricaoBairro
		{
			get
			{
				return this._descricaoBairro;
			}
			set
			{
				this._descricaoBairro = value;
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

		#region NumeroDDDTelefone
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo opcional.
		/// </summary>
		public string NumeroDDDTelefone
		{
			get
			{
				return this._numeroDDDTelefone;
			}
			set
			{
				this._numeroDDDTelefone = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroTelefone
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string NumeroTelefone
		{
			get
			{
				return this._numeroTelefone;
			}
			set
			{
				this._numeroTelefone = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroDDDCelular
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo opcional.
		/// </summary>
		public string NumeroDDDCelular
		{
			get
			{
				return this._numeroDDDCelular;
			}
			set
			{
				this._numeroDDDCelular = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCelular
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string NumeroCelular
		{
			get
			{
				return this._numeroCelular;
			}
			set
			{
				this._numeroCelular = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EmailPessoa
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string EmailPessoa
		{
			get
			{
				return this._emailPessoa;
			}
			set
			{
				this._emailPessoa = value;
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

		#region EstadoCivil
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public EstadoCivil EstadoCivil
		{
			get
			{
				return this._estadoCivil;
			}
			set
			{
				this._estadoCivil = value;
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

		#region Funcao
		/// <summary>
		/// Campo obrigatório.
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

		#region DataAdmissao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataAdmissao
		{
			get
			{
				return this._dataAdmissao;
			}
			set
			{
				this._dataAdmissao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataSaidaPrevista
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataSaidaPrevista
		{
			get
			{
				return this._dataSaidaPrevista;
			}
			set
			{
				this._dataSaidaPrevista = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorSalario
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public decimal ValorSalario
		{
			get
			{
				return this._valorSalario;
			}
			set
			{
				this._valorSalario = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroHabilitacao
		/// <summary>
		/// Tamanho do campo: 15.
		/// Campo opcional.
		/// </summary>
		public string NumeroHabilitacao
		{
			get
			{
				return this._numeroHabilitacao;
			}
			set
			{
				this._numeroHabilitacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CategoriaHabilitacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public CategoriaHabilitacao CategoriaHabilitacao
		{
			get
			{
				return this._categoriaHabilitacao;
			}
			set
			{
				this._categoriaHabilitacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Flagfilhos
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? Flagfilhos
		{
			get
			{
				return this._flagfilhos;
			}
			set
			{
				this._flagfilhos = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoVeiculo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public TipoVeiculo TipoVeiculo
		{
			get
			{
				return this._tipoVeiculo;
			}
			set
			{
				this._tipoVeiculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region AnoVeiculo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? AnoVeiculo
		{
			get
			{
				return this._anoVeiculo;
			}
			set
			{
				this._anoVeiculo = value;
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

		#region DataIntegracao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataIntegracao
		{
			get
			{
				return this._dataIntegracao;
			}
			set
			{
				this._dataIntegracao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IntegracaoSituacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public IntegracaoSituacao IntegracaoSituacao
		{
			get
			{
				return this._integracaoSituacao;
			}
			set
			{
				this._integracaoSituacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoVinculoIntegracao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TipoVinculoIntegracao TipoVinculoIntegracao
		{
			get
			{
				return this._tipoVinculoIntegracao;
			}
			set
			{
				this._tipoVinculoIntegracao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region MotivoRescisao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public MotivoRescisao MotivoRescisao
		{
			get
			{
				return this._motivoRescisao;
			}
			set
			{
				this._motivoRescisao = value;
				this._modified = true;
			}
		}
        #endregion

        #region FlgAdmitido
        public bool FlgAdmitido
        {
            get
            {
                return this._flg_Admitido;
            }
            set
            {
                this._flg_Admitido = value;
                this._modified = true;
            }
        }
        #endregion

        #region DesCS
        public string DesCS
        {
            get
            {
                return this._des_CS;
            }
            set
            {
                this._des_CS = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public Integracao()
		{
		}
		public Integracao(int idIntegracao)
		{
			this._idIntegracao = idIntegracao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.BNE_Integracao (Num_CPF, Nme_Pessoa, Ape_Pessoa, Idf_Sexo, Dta_Nascimento, Nme_Mae, Nme_Pai, Num_RG, Dta_Expedicao_RG, Nme_Orgao_Emissor, Sig_UF_Emissao_RG, Idf_Raca, Idf_Deficiencia, Des_Logradouro, Des_Complemento, Num_Endereco, Num_CEP, Des_Bairro, Idf_Cidade, Num_DDD_Telefone, Num_Telefone, Num_DDD_Celular, Num_Celular, Eml_Pessoa, Idf_Escolaridade, Idf_Estado_Civil, Raz_Social, Idf_funcao, Dta_Admissao, Dta_Saida_Prevista, Vlr_Salario, Num_Habilitacao, Idf_Categoria_Habilitacao, Flg_filhos, Idf_Tipo_Veiculo, Ano_Veiculo, Dta_Cadastro, Dta_Integracao, Idf_Integracao_Situacao, Idf_Tipo_Vinculo_Integracao, Idf_Motivo_Rescisao) VALUES (@Num_CPF, @Nme_Pessoa, @Ape_Pessoa, @Idf_Sexo, @Dta_Nascimento, @Nme_Mae, @Nme_Pai, @Num_RG, @Dta_Expedicao_RG, @Nme_Orgao_Emissor, @Sig_UF_Emissao_RG, @Idf_Raca, @Idf_Deficiencia, @Des_Logradouro, @Des_Complemento, @Num_Endereco, @Num_CEP, @Des_Bairro, @Idf_Cidade, @Num_DDD_Telefone, @Num_Telefone, @Num_DDD_Celular, @Num_Celular, @Eml_Pessoa, @Idf_Escolaridade, @Idf_Estado_Civil, @Raz_Social, @Idf_funcao, @Dta_Admissao, @Dta_Saida_Prevista, @Vlr_Salario, @Num_Habilitacao, @Idf_Categoria_Habilitacao, @Flg_filhos, @Idf_Tipo_Veiculo, @Ano_Veiculo, @Dta_Cadastro, @Dta_Integracao, @Idf_Integracao_Situacao, @Idf_Tipo_Vinculo_Integracao, @Idf_Motivo_Rescisao, @Flg_Admitido, @Des_CS);SET @Idf_Integracao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE plataforma.BNE_Integracao SET Num_CPF = @Num_CPF, Nme_Pessoa = @Nme_Pessoa, Ape_Pessoa = @Ape_Pessoa, Idf_Sexo = @Idf_Sexo, Dta_Nascimento = @Dta_Nascimento, Nme_Mae = @Nme_Mae, Nme_Pai = @Nme_Pai, Num_RG = @Num_RG, Dta_Expedicao_RG = @Dta_Expedicao_RG, Nme_Orgao_Emissor = @Nme_Orgao_Emissor, Sig_UF_Emissao_RG = @Sig_UF_Emissao_RG, Idf_Raca = @Idf_Raca, Idf_Deficiencia = @Idf_Deficiencia, Des_Logradouro = @Des_Logradouro, Des_Complemento = @Des_Complemento, Num_Endereco = @Num_Endereco, Num_CEP = @Num_CEP, Des_Bairro = @Des_Bairro, Idf_Cidade = @Idf_Cidade, Num_DDD_Telefone = @Num_DDD_Telefone, Num_Telefone = @Num_Telefone, Num_DDD_Celular = @Num_DDD_Celular, Num_Celular = @Num_Celular, Eml_Pessoa = @Eml_Pessoa, Idf_Escolaridade = @Idf_Escolaridade, Idf_Estado_Civil = @Idf_Estado_Civil, Raz_Social = @Raz_Social, Idf_funcao = @Idf_funcao, Dta_Admissao = @Dta_Admissao, Dta_Saida_Prevista = @Dta_Saida_Prevista, Vlr_Salario = @Vlr_Salario, Num_Habilitacao = @Num_Habilitacao, Idf_Categoria_Habilitacao = @Idf_Categoria_Habilitacao, Flg_filhos = @Flg_filhos, Idf_Tipo_Veiculo = @Idf_Tipo_Veiculo, Ano_Veiculo = @Ano_Veiculo, Dta_Cadastro = @Dta_Cadastro, Dta_Integracao = @Dta_Integracao, Idf_Integracao_Situacao = @Idf_Integracao_Situacao, Idf_Tipo_Vinculo_Integracao = @Idf_Tipo_Vinculo_Integracao, Idf_Motivo_Rescisao = @Idf_Motivo_Rescisao, @Flg_Admitido = Flg_Admitido ,@Des_CS =Des_CS WHERE Idf_Integracao = @Idf_Integracao";
		private const string SPDELETE = "DELETE FROM plataforma.BNE_Integracao WHERE Idf_Integracao = @Idf_Integracao";
		private const string SPSELECTID = "SELECT * FROM plataforma.BNE_Integracao WITH(NOLOCK) WHERE Idf_Integracao = @Idf_Integracao";
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
			parms.Add(new SqlParameter("@Idf_Integracao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Ape_Pessoa", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Idf_Sexo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Nascimento", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Nme_Mae", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Nme_Pai", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Num_RG", SqlDbType.VarChar, 20));
			parms.Add(new SqlParameter("@Dta_Expedicao_RG", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Nme_Orgao_Emissor", SqlDbType.VarChar, 20));
			parms.Add(new SqlParameter("@Sig_UF_Emissao_RG", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Idf_Raca", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Logradouro", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Complemento", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Num_Endereco", SqlDbType.VarChar, 15));
			parms.Add(new SqlParameter("@Num_CEP", SqlDbType.Char, 8));
			parms.Add(new SqlParameter("@Des_Bairro", SqlDbType.VarChar, 80));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_DDD_Telefone", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Telefone", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Num_DDD_Celular", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Celular", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Estado_Civil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Raz_Social", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Admissao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Saida_Prevista", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Vlr_Salario", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Num_Habilitacao", SqlDbType.VarChar, 15));
			parms.Add(new SqlParameter("@Idf_Categoria_Habilitacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_filhos", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Tipo_Veiculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Ano_Veiculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Integracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Integracao_Situacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Vinculo_Integracao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Motivo_Rescisao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Admitido", SqlDbType.Bit));
            parms.Add(new SqlParameter("@Des_CS", SqlDbType.VarChar, 200));
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
			parms[0].Value = this._idIntegracao;

			if (this._numeroCPF.HasValue)
				parms[1].Value = this._numeroCPF;
			else
				parms[1].Value = DBNull.Value;

			parms[2].Value = this._nomePessoa;

			if (!String.IsNullOrEmpty(this._apelidoPessoa))
				parms[3].Value = this._apelidoPessoa;
			else
				parms[3].Value = DBNull.Value;

			parms[4].Value = this._sexo.IdSexo;
			parms[5].Value = this._dataNascimento;

			if (!String.IsNullOrEmpty(this._nomeMae))
				parms[6].Value = this._nomeMae;
			else
				parms[6].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._nomePai))
				parms[7].Value = this._nomePai;
			else
				parms[7].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroRG))
				parms[8].Value = this._numeroRG;
			else
				parms[8].Value = DBNull.Value;


			if (this._dataExpedicaoRG.HasValue)
				parms[9].Value = this._dataExpedicaoRG;
			else
				parms[9].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._nomeOrgaoEmissor))
				parms[10].Value = this._nomeOrgaoEmissor;
			else
				parms[10].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._siglaUFEmissaoRG))
				parms[11].Value = this._siglaUFEmissaoRG;
			else
				parms[11].Value = DBNull.Value;


			if (this._raca != null)
				parms[12].Value = this._raca.IdRaca;
			else
				parms[12].Value = DBNull.Value;


			if (this._deficiencia != null)
				parms[13].Value = this._deficiencia.IdDeficiencia;
			else
				parms[13].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoLogradouro))
				parms[14].Value = this._descricaoLogradouro;
			else
				parms[14].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoComplemento))
				parms[15].Value = this._descricaoComplemento;
			else
				parms[15].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroEndereco))
				parms[16].Value = this._numeroEndereco;
			else
				parms[16].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroCEP))
				parms[17].Value = this._numeroCEP;
			else
				parms[17].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoBairro))
				parms[18].Value = this._descricaoBairro;
			else
				parms[18].Value = DBNull.Value;

			parms[19].Value = this._cidade.IdCidade;

			if (!String.IsNullOrEmpty(this._numeroDDDTelefone))
				parms[20].Value = this._numeroDDDTelefone;
			else
				parms[20].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroTelefone))
				parms[21].Value = this._numeroTelefone;
			else
				parms[21].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroDDDCelular))
				parms[22].Value = this._numeroDDDCelular;
			else
				parms[22].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroCelular))
				parms[23].Value = this._numeroCelular;
			else
				parms[23].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._emailPessoa))
				parms[24].Value = this._emailPessoa;
			else
				parms[24].Value = DBNull.Value;


			if (this._escolaridade != null)
				parms[25].Value = this._escolaridade.IdEscolaridade;
			else
				parms[25].Value = DBNull.Value;


			if (this._estadoCivil != null)
				parms[26].Value = this._estadoCivil.IdEstadoCivil;
			else
				parms[26].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._razaoSocial))
				parms[27].Value = this._razaoSocial;
			else
				parms[27].Value = DBNull.Value;

			parms[28].Value = this._funcao.IdFuncao;

			if (this._dataAdmissao.HasValue)
				parms[29].Value = this._dataAdmissao;
			else
				parms[29].Value = DBNull.Value;


			if (this._dataSaidaPrevista.HasValue)
				parms[30].Value = this._dataSaidaPrevista;
			else
				parms[30].Value = DBNull.Value;

			parms[31].Value = this._valorSalario;

			if (!String.IsNullOrEmpty(this._numeroHabilitacao))
				parms[32].Value = this._numeroHabilitacao;
			else
				parms[32].Value = DBNull.Value;


			if (this._categoriaHabilitacao != null)
				parms[33].Value = this._categoriaHabilitacao.IdCategoriaHabilitacao;
			else
				parms[33].Value = DBNull.Value;


			if (this._flagfilhos.HasValue)
				parms[34].Value = this._flagfilhos;
			else
				parms[34].Value = DBNull.Value;


			if (this._tipoVeiculo != null)
				parms[35].Value = this._tipoVeiculo.IdTipoVeiculo;
			else
				parms[35].Value = DBNull.Value;


			if (this._anoVeiculo.HasValue)
				parms[36].Value = this._anoVeiculo;
			else
				parms[36].Value = DBNull.Value;


			if (this._dataIntegracao.HasValue)
				parms[38].Value = this._dataIntegracao;
			else
				parms[38].Value = DBNull.Value;

			parms[39].Value = this._integracaoSituacao.IdIntegracaoSituacao;
			parms[40].Value = this._tipoVinculoIntegracao.IdTipoVinculoIntegracao;

            if (this._motivoRescisao != null)
			    parms[41].Value = this._motivoRescisao.IdMotivoRescisao;
            else
                parms[41].Value = DBNull.Value;

            parms[42].Value = this._flg_Admitido;

            if (this.DesCS != null)
                parms[43].Value = this._des_CS;
            else
                parms[43].Value = DBNull.Value;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[37].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Integracao no banco de dados.
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
						this._idIntegracao = Convert.ToInt32(cmd.Parameters["@Idf_Integracao"].Value);
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
		/// Método utilizado para inserir uma instância de Integracao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idIntegracao = Convert.ToInt32(cmd.Parameters["@Idf_Integracao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Integracao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Integracao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Integracao no banco de dados.
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
		/// Método utilizado para salvar uma instância de Integracao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Integracao no banco de dados.
		/// </summary>
		/// <param name="idIntegracao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idIntegracao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Integracao", SqlDbType.Int, 4));

			parms[0].Value = idIntegracao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Integracao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idIntegracao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idIntegracao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Integracao", SqlDbType.Int, 4));

			parms[0].Value = idIntegracao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Integracao no banco de dados.
		/// </summary>
		/// <param name="idIntegracao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idIntegracao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.BNE_Integracao where Idf_Integracao in (";

			for (int i = 0; i < idIntegracao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idIntegracao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idIntegracao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idIntegracao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Integracao", SqlDbType.Int, 4));

			parms[0].Value = idIntegracao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idIntegracao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idIntegracao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Integracao", SqlDbType.Int, 4));

			parms[0].Value = idIntegracao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Int.Idf_Integracao, Int.Num_CPF, Int.Nme_Pessoa, Int.Ape_Pessoa, Int.Idf_Sexo, Int.Dta_Nascimento, Int.Nme_Mae, Int.Nme_Pai, Int.Num_RG, Int.Dta_Expedicao_RG, Int.Nme_Orgao_Emissor, Int.Sig_UF_Emissao_RG, Int.Idf_Raca, Int.Idf_Deficiencia, Int.Des_Logradouro, Int.Des_Complemento, Int.Num_Endereco, Int.Num_CEP, Int.Des_Bairro, Int.Idf_Cidade, Int.Num_DDD_Telefone, Int.Num_Telefone, Int.Num_DDD_Celular, Int.Num_Celular, Int.Eml_Pessoa, Int.Idf_Escolaridade, Int.Idf_Estado_Civil, Int.Raz_Social, Int.Idf_funcao, Int.Dta_Admissao, Int.Dta_Saida_Prevista, Int.Vlr_Salario, Int.Num_Habilitacao, Int.Idf_Categoria_Habilitacao, Int.Flg_filhos, Int.Idf_Tipo_Veiculo, Int.Ano_Veiculo, Int.Dta_Cadastro, Int.Dta_Integracao, Int.Idf_Integracao_Situacao, Int.Idf_Tipo_Vinculo_Integracao, Int.Idf_Motivo_Rescisao, Int.Flg_Admitido, Int.Des_CS  FROM plataforma.BNE_Integracao Int";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Integracao a partir do banco de dados.
		/// </summary>
		/// <param name="idIntegracao">Chave do registro.</param>
		/// <returns>Instância de Integracao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Integracao LoadObject(int idIntegracao)
		{
			using (IDataReader dr = LoadDataReader(idIntegracao))
			{
				Integracao objIntegracao = new Integracao();
				if (SetInstance(dr, objIntegracao))
					return objIntegracao;
			}
			throw (new RecordNotFoundException(typeof(Integracao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Integracao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idIntegracao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Integracao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Integracao LoadObject(int idIntegracao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idIntegracao, trans))
			{
				Integracao objIntegracao = new Integracao();
				if (SetInstance(dr, objIntegracao))
					return objIntegracao;
			}
			throw (new RecordNotFoundException(typeof(Integracao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Integracao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idIntegracao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Integracao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idIntegracao, trans))
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
		/// <param name="objIntegracao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Integracao objIntegracao)
		{
			try
			{
				if (dr.Read())
				{
					objIntegracao._idIntegracao = Convert.ToInt32(dr["Idf_Integracao"]);
					if (dr["Num_CPF"] != DBNull.Value)
						objIntegracao._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
					objIntegracao._nomePessoa = Convert.ToString(dr["Nme_Pessoa"]);
					if (dr["Ape_Pessoa"] != DBNull.Value)
						objIntegracao._apelidoPessoa = Convert.ToString(dr["Ape_Pessoa"]);
					objIntegracao._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
					objIntegracao._dataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]);
					if (dr["Nme_Mae"] != DBNull.Value)
						objIntegracao._nomeMae = Convert.ToString(dr["Nme_Mae"]);
					if (dr["Nme_Pai"] != DBNull.Value)
						objIntegracao._nomePai = Convert.ToString(dr["Nme_Pai"]);
					if (dr["Num_RG"] != DBNull.Value)
						objIntegracao._numeroRG = Convert.ToString(dr["Num_RG"]);
					if (dr["Dta_Expedicao_RG"] != DBNull.Value)
						objIntegracao._dataExpedicaoRG = Convert.ToDateTime(dr["Dta_Expedicao_RG"]);
					if (dr["Nme_Orgao_Emissor"] != DBNull.Value)
						objIntegracao._nomeOrgaoEmissor = Convert.ToString(dr["Nme_Orgao_Emissor"]);
					if (dr["Sig_UF_Emissao_RG"] != DBNull.Value)
						objIntegracao._siglaUFEmissaoRG = Convert.ToString(dr["Sig_UF_Emissao_RG"]);
					if (dr["Idf_Raca"] != DBNull.Value)
						objIntegracao._raca = new Raca(Convert.ToInt32(dr["Idf_Raca"]));
					if (dr["Idf_Deficiencia"] != DBNull.Value)
						objIntegracao._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
					if (dr["Des_Logradouro"] != DBNull.Value)
						objIntegracao._descricaoLogradouro = Convert.ToString(dr["Des_Logradouro"]);
					if (dr["Des_Complemento"] != DBNull.Value)
						objIntegracao._descricaoComplemento = Convert.ToString(dr["Des_Complemento"]);
					if (dr["Num_Endereco"] != DBNull.Value)
						objIntegracao._numeroEndereco = Convert.ToString(dr["Num_Endereco"]);
					if (dr["Num_CEP"] != DBNull.Value)
						objIntegracao._numeroCEP = Convert.ToString(dr["Num_CEP"]);
					if (dr["Des_Bairro"] != DBNull.Value)
						objIntegracao._descricaoBairro = Convert.ToString(dr["Des_Bairro"]);
					objIntegracao._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					if (dr["Num_DDD_Telefone"] != DBNull.Value)
						objIntegracao._numeroDDDTelefone = Convert.ToString(dr["Num_DDD_Telefone"]);
					if (dr["Num_Telefone"] != DBNull.Value)
						objIntegracao._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);
					if (dr["Num_DDD_Celular"] != DBNull.Value)
						objIntegracao._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
					if (dr["Num_Celular"] != DBNull.Value)
						objIntegracao._numeroCelular = Convert.ToString(dr["Num_Celular"]);
					if (dr["Eml_Pessoa"] != DBNull.Value)
						objIntegracao._emailPessoa = Convert.ToString(dr["Eml_Pessoa"]);
					if (dr["Idf_Escolaridade"] != DBNull.Value)
						objIntegracao._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
					if (dr["Idf_Estado_Civil"] != DBNull.Value)
						objIntegracao._estadoCivil = new EstadoCivil(Convert.ToInt32(dr["Idf_Estado_Civil"]));
					if (dr["Raz_Social"] != DBNull.Value)
						objIntegracao._razaoSocial = Convert.ToString(dr["Raz_Social"]);
					objIntegracao._funcao = new Funcao(Convert.ToInt32(dr["Idf_funcao"]));
					if (dr["Dta_Admissao"] != DBNull.Value)
						objIntegracao._dataAdmissao = Convert.ToDateTime(dr["Dta_Admissao"]);
					if (dr["Dta_Saida_Prevista"] != DBNull.Value)
						objIntegracao._dataSaidaPrevista = Convert.ToDateTime(dr["Dta_Saida_Prevista"]);
					objIntegracao._valorSalario = Convert.ToDecimal(dr["Vlr_Salario"]);
					if (dr["Num_Habilitacao"] != DBNull.Value)
						objIntegracao._numeroHabilitacao = Convert.ToString(dr["Num_Habilitacao"]);
					if (dr["Idf_Categoria_Habilitacao"] != DBNull.Value)
						objIntegracao._categoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(dr["Idf_Categoria_Habilitacao"]));
					if (dr["Flg_filhos"] != DBNull.Value)
						objIntegracao._flagfilhos = Convert.ToBoolean(dr["Flg_filhos"]);
					if (dr["Idf_Tipo_Veiculo"] != DBNull.Value)
						objIntegracao._tipoVeiculo = new TipoVeiculo(Convert.ToInt32(dr["Idf_Tipo_Veiculo"]));
					if (dr["Ano_Veiculo"] != DBNull.Value)
						objIntegracao._anoVeiculo = Convert.ToInt32(dr["Ano_Veiculo"]);
					objIntegracao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Dta_Integracao"] != DBNull.Value)
						objIntegracao._dataIntegracao = Convert.ToDateTime(dr["Dta_Integracao"]);
					objIntegracao._integracaoSituacao = new IntegracaoSituacao(Convert.ToInt32(dr["Idf_Integracao_Situacao"]));
					objIntegracao._tipoVinculoIntegracao = new TipoVinculoIntegracao(Convert.ToInt32(dr["Idf_Tipo_Vinculo_Integracao"]));

                    if(dr["Idf_Motivo_Rescisao"]!= DBNull.Value)
					    objIntegracao._motivoRescisao = new MotivoRescisao(Convert.ToInt32(dr["Idf_Motivo_Rescisao"]));

                    objIntegracao._flg_Admitido = Convert.ToBoolean(dr["Flg_Admitido"]);

                    if (dr["Des_CS"] != DBNull.Value)
                        objIntegracao.DesCS = Convert.ToString(dr["Des_CS"]);

					objIntegracao._persisted = true;
					objIntegracao._modified = false;

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