//-- Data: 25/04/2014 15:33
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CobrancaBoleto // Tabela: GLO_Cobranca_Boleto
	{
		#region Atributos
		private int _idCobrancaBoleto;
        private CobrancaBoletoTransacao _cobrancaBoletoTransacao;
		private decimal? _numeroCNPJCedente;
		private decimal? _numeroCPFCedente;
		private string _numeroAgenciaBancaria;
		private string _numeroConta;
		private string _numeroDVConta;
		private string _razaoSocialCedente;
		private string _nomePessoaCedente;
		private Banco _banco;
		private bool _flagRegistraBoleto;
		private DateTime? _dataEmissao;
		private DateTime? _dataVencimento;
		private decimal? _valorBoleto;
		private string _numeroNossoNumero;
		private decimal? _numeroCPFSacado;
		private decimal? _numeroCNPJSacado;
		private string _nomePessoaSacado;
		private string _razaoSocialSacado;
		private string _enderecoEmailSacado;
		private string _descricaoLogradouroSacado;
		private string _numeroEndereçoSacado;
		private string _descricaoComplementoSacado;
		private Cidade _cidadeSacado;
		private string _numeroCepSacado;
		private string _descricaoBairroSacado;
		private string _descricaoInstrucaoCaixa;
		private string _codigoBarras;
		private string _arquivoBoleto;
		private MensagemRetornoBoleto _mensagemRetornoBoleto;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCobrancaBoleto
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCobrancaBoleto
		{
			get
			{
				return this._idCobrancaBoleto;
			}
		}
		#endregion 

        #region CobrancaBoletoTransacao
        /// <summary>
		/// Campo opcional.
		/// </summary>
        public CobrancaBoletoTransacao CobrancaBoletoTransacao
		{
			get
			{
                return this._cobrancaBoletoTransacao;
			}
			set
			{
                this._cobrancaBoletoTransacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCNPJCedente
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroCNPJCedente
		{
			get
			{
				return this._numeroCNPJCedente;
			}
			set
			{
				this._numeroCNPJCedente = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCPFCedente
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroCPFCedente
		{
			get
			{
				return this._numeroCPFCedente;
			}
			set
			{
				this._numeroCPFCedente = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroAgenciaBancaria
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string NumeroAgenciaBancaria
		{
			get
			{
				return this._numeroAgenciaBancaria;
			}
			set
			{
				this._numeroAgenciaBancaria = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroConta
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string NumeroConta
		{
			get
			{
				return this._numeroConta;
			}
			set
			{
				this._numeroConta = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroDVConta
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo opcional.
		/// </summary>
		public string NumeroDVConta
		{
			get
			{
				return this._numeroDVConta;
			}
			set
			{
				this._numeroDVConta = value;
				this._modified = true;
			}
		}
		#endregion 

		#region RazaoSocialCedente
		/// <summary>
		/// Tamanho do campo: 60.
		/// Campo opcional.
		/// </summary>
		public string RazaoSocialCedente
		{
			get
			{
				return this._razaoSocialCedente;
			}
			set
			{
				this._razaoSocialCedente = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomePessoaCedente
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string NomePessoaCedente
		{
			get
			{
				return this._nomePessoaCedente;
			}
			set
			{
				this._nomePessoaCedente = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Banco
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Banco Banco
		{
			get
			{
				return this._banco;
			}
			set
			{
				this._banco = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagRegistraBoleto
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagRegistraBoleto
		{
			get
			{
				return this._flagRegistraBoleto;
			}
			set
			{
				this._flagRegistraBoleto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataEmissao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataEmissao
		{
			get
			{
				return this._dataEmissao;
			}
			set
			{
				this._dataEmissao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataVencimento
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataVencimento
		{
			get
			{
				return this._dataVencimento;
			}
			set
			{
				this._dataVencimento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorBoleto
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? ValorBoleto
		{
			get
			{
				return this._valorBoleto;
			}
			set
			{
				this._valorBoleto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroNossoNumero
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo opcional.
		/// </summary>
		public string NumeroNossoNumero
		{
			get
			{
				return this._numeroNossoNumero;
			}
			set
			{
				this._numeroNossoNumero = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCPFSacado
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroCPFSacado
		{
			get
			{
				return this._numeroCPFSacado;
			}
			set
			{
				this._numeroCPFSacado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCNPJSacado
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroCNPJSacado
		{
			get
			{
				return this._numeroCNPJSacado;
			}
			set
			{
				this._numeroCNPJSacado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomePessoaSacado
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string NomePessoaSacado
		{
			get
			{
				return this._nomePessoaSacado;
			}
			set
			{
				this._nomePessoaSacado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region RazaoSocialSacado
		/// <summary>
		/// Tamanho do campo: 60.
		/// Campo opcional.
		/// </summary>
		public string RazaoSocialSacado
		{
			get
			{
				return this._razaoSocialSacado;
			}
			set
			{
				this._razaoSocialSacado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EnderecoEmailSacado
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string EnderecoEmailSacado
		{
			get
			{
				return this._enderecoEmailSacado;
			}
			set
			{
				this._enderecoEmailSacado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoLogradouroSacado
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoLogradouroSacado
		{
			get
			{
				return this._descricaoLogradouroSacado;
			}
			set
			{
				this._descricaoLogradouroSacado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroEndereçoSacado
		/// <summary>
		/// Tamanho do campo: 15.
		/// Campo opcional.
		/// </summary>
		public string NumeroEndereçoSacado
		{
			get
			{
				return this._numeroEndereçoSacado;
			}
			set
			{
				this._numeroEndereçoSacado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoComplementoSacado
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo opcional.
		/// </summary>
		public string DescricaoComplementoSacado
		{
			get
			{
				return this._descricaoComplementoSacado;
			}
			set
			{
				this._descricaoComplementoSacado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CidadeSacado
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Cidade CidadeSacado
		{
			get
			{
				return this._cidadeSacado;
			}
			set
			{
				this._cidadeSacado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCepSacado
		/// <summary>
		/// Tamanho do campo: 8.
		/// Campo opcional.
		/// </summary>
		public string NumeroCepSacado
		{
			get
			{
				return this._numeroCepSacado;
			}
			set
			{
				this._numeroCepSacado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoBairroSacado
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo opcional.
		/// </summary>
		public string DescricaoBairroSacado
		{
			get
			{
				return this._descricaoBairroSacado;
			}
			set
			{
				this._descricaoBairroSacado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoInstrucaoCaixa
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoInstrucaoCaixa
		{
			get
			{
				return this._descricaoInstrucaoCaixa;
			}
			set
			{
				this._descricaoInstrucaoCaixa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoBarras
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string CodigoBarras
		{
			get
			{
				return this._codigoBarras;
			}
			set
			{
				this._codigoBarras = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ArquivoBoleto
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string ArquivoBoleto
		{
			get
			{
				return this._arquivoBoleto;
			}
			set
			{
				this._arquivoBoleto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region MensagemRetornoBoleto
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public MensagemRetornoBoleto MensagemRetornoBoleto
		{
			get
			{
				return this._mensagemRetornoBoleto;
			}
			set
			{
				this._mensagemRetornoBoleto = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CobrancaBoleto()
		{
		}
		public CobrancaBoleto(int idCobrancaBoleto)
		{
			this._idCobrancaBoleto = idCobrancaBoleto;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO GLO_Cobranca_Boleto (Idf_Transacao, Num_CNPJ_Cedente, Num_CPF_Cedente, Num_Agencia_Bancaria, Num_Conta, Num_DV_Conta, Raz_Social_Cedente, Nme_Pessoa_Cedente, Idf_Banco, Flg_Registra_Boleto, Dta_Emissao, Dta_Vencimento, Vlr_Boleto, Num_Nosso_Numero, Num_CPF_Sacado, Num_CNPJ_Sacado, Nme_Pessoa_Sacado, Raz_Social_Sacado, End_Email_Sacado, Des_Logradouro_Sacado, Num_Endereço_Sacado, Des_Complemento_Sacado, Idf_Cidade_Sacado, Num_Cep_Sacado, Des_Bairro_Sacado, Des_Instrucao_Caixa, Cod_Barras, Arq_Boleto, Idf_Mensagem_Retorno_Boleto) VALUES (@Idf_Transacao, @Num_CNPJ_Cedente, @Num_CPF_Cedente, @Num_Agencia_Bancaria, @Num_Conta, @Num_DV_Conta, @Raz_Social_Cedente, @Nme_Pessoa_Cedente, @Idf_Banco, @Flg_Registra_Boleto, @Dta_Emissao, @Dta_Vencimento, @Vlr_Boleto, @Num_Nosso_Numero, @Num_CPF_Sacado, @Num_CNPJ_Sacado, @Nme_Pessoa_Sacado, @Raz_Social_Sacado, @End_Email_Sacado, @Des_Logradouro_Sacado, @Num_Endereço_Sacado, @Des_Complemento_Sacado, @Idf_Cidade_Sacado, @Num_Cep_Sacado, @Des_Bairro_Sacado, @Des_Instrucao_Caixa, @Cod_Barras, @Arq_Boleto, @Idf_Mensagem_Retorno_Boleto);SET @Idf_Cobranca_Boleto = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE GLO_Cobranca_Boleto SET Idf_Transacao = @Idf_Transacao, Num_CNPJ_Cedente = @Num_CNPJ_Cedente, Num_CPF_Cedente = @Num_CPF_Cedente, Num_Agencia_Bancaria = @Num_Agencia_Bancaria, Num_Conta = @Num_Conta, Num_DV_Conta = @Num_DV_Conta, Raz_Social_Cedente = @Raz_Social_Cedente, Nme_Pessoa_Cedente = @Nme_Pessoa_Cedente, Idf_Banco = @Idf_Banco, Flg_Registra_Boleto = @Flg_Registra_Boleto, Dta_Emissao = @Dta_Emissao, Dta_Vencimento = @Dta_Vencimento, Vlr_Boleto = @Vlr_Boleto, Num_Nosso_Numero = @Num_Nosso_Numero, Num_CPF_Sacado = @Num_CPF_Sacado, Num_CNPJ_Sacado = @Num_CNPJ_Sacado, Nme_Pessoa_Sacado = @Nme_Pessoa_Sacado, Raz_Social_Sacado = @Raz_Social_Sacado, End_Email_Sacado = @End_Email_Sacado, Des_Logradouro_Sacado = @Des_Logradouro_Sacado, Num_Endereço_Sacado = @Num_Endereço_Sacado, Des_Complemento_Sacado = @Des_Complemento_Sacado, Idf_Cidade_Sacado = @Idf_Cidade_Sacado, Num_Cep_Sacado = @Num_Cep_Sacado, Des_Bairro_Sacado = @Des_Bairro_Sacado, Des_Instrucao_Caixa = @Des_Instrucao_Caixa, Cod_Barras = @Cod_Barras, Arq_Boleto = @Arq_Boleto, Idf_Mensagem_Retorno_Boleto = @Idf_Mensagem_Retorno_Boleto WHERE Idf_Cobranca_Boleto = @Idf_Cobranca_Boleto";
		private const string SPDELETE = "DELETE FROM GLO_Cobranca_Boleto WHERE Idf_Cobranca_Boleto = @Idf_Cobranca_Boleto";
		private const string SPSELECTID = "SELECT * FROM GLO_Cobranca_Boleto WITH(NOLOCK) WHERE Idf_Cobranca_Boleto = @Idf_Cobranca_Boleto";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_CNPJ_Cedente", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Num_CPF_Cedente", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Num_Agencia_Bancaria", SqlDbType.VarChar, 10));
			parms.Add(new SqlParameter("@Num_Conta", SqlDbType.VarChar, 10));
			parms.Add(new SqlParameter("@Num_DV_Conta", SqlDbType.VarChar, 2));
			parms.Add(new SqlParameter("@Raz_Social_Cedente", SqlDbType.VarChar, 60));
			parms.Add(new SqlParameter("@Nme_Pessoa_Cedente", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Banco", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Registra_Boleto", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Emissao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Vencimento", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Vlr_Boleto", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Num_Nosso_Numero", SqlDbType.VarChar, 20));
			parms.Add(new SqlParameter("@Num_CPF_Sacado", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Num_CNPJ_Sacado", SqlDbType.Decimal, 9));
			parms.Add(new SqlParameter("@Nme_Pessoa_Sacado", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Raz_Social_Sacado", SqlDbType.VarChar, 60));
			parms.Add(new SqlParameter("@End_Email_Sacado", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Logradouro_Sacado", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Num_Endereço_Sacado", SqlDbType.VarChar, 15));
			parms.Add(new SqlParameter("@Des_Complemento_Sacado", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Idf_Cidade_Sacado", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_Cep_Sacado", SqlDbType.VarChar, 8));
			parms.Add(new SqlParameter("@Des_Bairro_Sacado", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Des_Instrucao_Caixa", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Cod_Barras", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Arq_Boleto", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Idf_Mensagem_Retorno_Boleto", SqlDbType.Int, 4));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idCobrancaBoleto;

			if (this._cobrancaBoletoTransacao != null)
				parms[1].Value = this._cobrancaBoletoTransacao.IdCobrancaBoletoTransacao;
			else
				parms[1].Value = DBNull.Value;


			if (this._numeroCNPJCedente.HasValue)
				parms[2].Value = this._numeroCNPJCedente;
			else
				parms[2].Value = DBNull.Value;


			if (this._numeroCPFCedente.HasValue)
				parms[3].Value = this._numeroCPFCedente;
			else
				parms[3].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroAgenciaBancaria))
				parms[4].Value = this._numeroAgenciaBancaria;
			else
				parms[4].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroConta))
				parms[5].Value = this._numeroConta;
			else
				parms[5].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroDVConta))
				parms[6].Value = this._numeroDVConta;
			else
				parms[6].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._razaoSocialCedente))
				parms[7].Value = this._razaoSocialCedente;
			else
				parms[7].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._nomePessoaCedente))
				parms[8].Value = this._nomePessoaCedente;
			else
				parms[8].Value = DBNull.Value;


			if (this._banco != null)
				parms[9].Value = this._banco.IdBanco;
			else
				parms[9].Value = DBNull.Value;

			parms[10].Value = this._flagRegistraBoleto;

			if (this._dataEmissao.HasValue)
				parms[11].Value = this._dataEmissao;
			else
				parms[11].Value = DBNull.Value;


			if (this._dataVencimento.HasValue)
				parms[12].Value = this._dataVencimento;
			else
				parms[12].Value = DBNull.Value;


			if (this._valorBoleto.HasValue)
				parms[13].Value = this._valorBoleto;
			else
				parms[13].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroNossoNumero))
				parms[14].Value = this._numeroNossoNumero;
			else
				parms[14].Value = DBNull.Value;


			if (this._numeroCPFSacado.HasValue)
				parms[15].Value = this._numeroCPFSacado;
			else
				parms[15].Value = DBNull.Value;


			if (this._numeroCNPJSacado.HasValue)
				parms[16].Value = this._numeroCNPJSacado;
			else
				parms[16].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._nomePessoaSacado))
				parms[17].Value = this._nomePessoaSacado;
			else
				parms[17].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._razaoSocialSacado))
				parms[18].Value = this._razaoSocialSacado;
			else
				parms[18].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._enderecoEmailSacado))
				parms[19].Value = this._enderecoEmailSacado;
			else
				parms[19].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoLogradouroSacado))
				parms[20].Value = this._descricaoLogradouroSacado;
			else
				parms[20].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroEndereçoSacado))
				parms[21].Value = this._numeroEndereçoSacado;
			else
				parms[21].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoComplementoSacado))
				parms[22].Value = this._descricaoComplementoSacado;
			else
				parms[22].Value = DBNull.Value;


			if (this._cidadeSacado != null)
				parms[23].Value = this._cidadeSacado.IdCidade;
			else
				parms[23].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroCepSacado))
				parms[24].Value = this._numeroCepSacado;
			else
				parms[24].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoBairroSacado))
				parms[25].Value = this._descricaoBairroSacado;
			else
				parms[25].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoInstrucaoCaixa))
				parms[26].Value = this._descricaoInstrucaoCaixa;
			else
				parms[26].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._codigoBarras))
				parms[27].Value = this._codigoBarras;
			else
				parms[27].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._arquivoBoleto))
				parms[28].Value = this._arquivoBoleto;
			else
				parms[28].Value = DBNull.Value;


			if (this._mensagemRetornoBoleto != null)
				parms[29].Value = this._mensagemRetornoBoleto.IdMensagemRetornoBoleto;
			else
				parms[29].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de CobrancaBoleto no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
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
						this._idCobrancaBoleto = Convert.ToInt32(cmd.Parameters["@Idf_Cobranca_Boleto"].Value);
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
		/// Método utilizado para inserir uma instância de CobrancaBoleto no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCobrancaBoleto = Convert.ToInt32(cmd.Parameters["@Idf_Cobranca_Boleto"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CobrancaBoleto no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para atualizar uma instância de CobrancaBoleto no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para salvar uma instância de CobrancaBoleto no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de CobrancaBoleto no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para excluir uma instância de CobrancaBoleto no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoleto">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoleto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoleto;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CobrancaBoleto no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoleto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoleto, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoleto;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CobrancaBoleto no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoleto">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idCobrancaBoleto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from GLO_Cobranca_Boleto where Idf_Cobranca_Boleto in (";

			for (int i = 0; i < idCobrancaBoleto.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCobrancaBoleto[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoleto">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoleto)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoleto;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoleto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoleto, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoleto;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cob.Idf_Cobranca_Boleto, Cob.Idf_Transacao, Cob.Num_CNPJ_Cedente, Cob.Num_CPF_Cedente, Cob.Num_Agencia_Bancaria, Cob.Num_Conta, Cob.Num_DV_Conta, Cob.Raz_Social_Cedente, Cob.Nme_Pessoa_Cedente, Cob.Idf_Banco, Cob.Flg_Registra_Boleto, Cob.Dta_Emissao, Cob.Dta_Vencimento, Cob.Vlr_Boleto, Cob.Num_Nosso_Numero, Cob.Num_CPF_Sacado, Cob.Num_CNPJ_Sacado, Cob.Nme_Pessoa_Sacado, Cob.Raz_Social_Sacado, Cob.End_Email_Sacado, Cob.Des_Logradouro_Sacado, Cob.Num_Endereço_Sacado, Cob.Des_Complemento_Sacado, Cob.Idf_Cidade_Sacado, Cob.Num_Cep_Sacado, Cob.Des_Bairro_Sacado, Cob.Des_Instrucao_Caixa, Cob.Cod_Barras, Cob.Arq_Boleto, Cob.Idf_Mensagem_Retorno_Boleto FROM GLO_Cobranca_Boleto Cob";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoleto a partir do banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoleto">Chave do registro.</param>
		/// <returns>Instância de CobrancaBoleto.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoleto LoadObject(int idCobrancaBoleto)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoleto))
			{
				CobrancaBoleto objCobrancaBoleto = new CobrancaBoleto();
				if (SetInstance(dr, objCobrancaBoleto))
					return objCobrancaBoleto;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoleto)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoleto a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoleto">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CobrancaBoleto.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoleto LoadObject(int idCobrancaBoleto, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoleto, trans))
			{
				CobrancaBoleto objCobrancaBoleto = new CobrancaBoleto();
				if (SetInstance(dr, objCobrancaBoleto))
					return objCobrancaBoleto;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoleto)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoleto a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoleto))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoleto a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoleto, trans))
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
		/// <param name="objCobrancaBoleto">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, CobrancaBoleto objCobrancaBoleto)
		{
			try
			{
				if (dr.Read())
				{
					objCobrancaBoleto._idCobrancaBoleto = Convert.ToInt32(dr["Idf_Cobranca_Boleto"]);
					if (dr["Idf_Transacao"] != DBNull.Value)
						objCobrancaBoleto._cobrancaBoletoTransacao = new CobrancaBoletoTransacao(Convert.ToInt32(dr["Idf_Transacao"]));
					if (dr["Num_CNPJ_Cedente"] != DBNull.Value)
						objCobrancaBoleto._numeroCNPJCedente = Convert.ToDecimal(dr["Num_CNPJ_Cedente"]);
					if (dr["Num_CPF_Cedente"] != DBNull.Value)
						objCobrancaBoleto._numeroCPFCedente = Convert.ToDecimal(dr["Num_CPF_Cedente"]);
					if (dr["Num_Agencia_Bancaria"] != DBNull.Value)
						objCobrancaBoleto._numeroAgenciaBancaria = Convert.ToString(dr["Num_Agencia_Bancaria"]);
					if (dr["Num_Conta"] != DBNull.Value)
						objCobrancaBoleto._numeroConta = Convert.ToString(dr["Num_Conta"]);
					if (dr["Num_DV_Conta"] != DBNull.Value)
						objCobrancaBoleto._numeroDVConta = Convert.ToString(dr["Num_DV_Conta"]);
					if (dr["Raz_Social_Cedente"] != DBNull.Value)
						objCobrancaBoleto._razaoSocialCedente = Convert.ToString(dr["Raz_Social_Cedente"]);
					if (dr["Nme_Pessoa_Cedente"] != DBNull.Value)
						objCobrancaBoleto._nomePessoaCedente = Convert.ToString(dr["Nme_Pessoa_Cedente"]);
					if (dr["Idf_Banco"] != DBNull.Value)
						objCobrancaBoleto._banco = new Banco(Convert.ToInt32(dr["Idf_Banco"]));
					objCobrancaBoleto._flagRegistraBoleto = Convert.ToBoolean(dr["Flg_Registra_Boleto"]);
					if (dr["Dta_Emissao"] != DBNull.Value)
						objCobrancaBoleto._dataEmissao = Convert.ToDateTime(dr["Dta_Emissao"]);
					if (dr["Dta_Vencimento"] != DBNull.Value)
						objCobrancaBoleto._dataVencimento = Convert.ToDateTime(dr["Dta_Vencimento"]);
					if (dr["Vlr_Boleto"] != DBNull.Value)
						objCobrancaBoleto._valorBoleto = Convert.ToDecimal(dr["Vlr_Boleto"]);
					if (dr["Num_Nosso_Numero"] != DBNull.Value)
						objCobrancaBoleto._numeroNossoNumero = Convert.ToString(dr["Num_Nosso_Numero"]);
					if (dr["Num_CPF_Sacado"] != DBNull.Value)
						objCobrancaBoleto._numeroCPFSacado = Convert.ToDecimal(dr["Num_CPF_Sacado"]);
					if (dr["Num_CNPJ_Sacado"] != DBNull.Value)
						objCobrancaBoleto._numeroCNPJSacado = Convert.ToDecimal(dr["Num_CNPJ_Sacado"]);
					if (dr["Nme_Pessoa_Sacado"] != DBNull.Value)
						objCobrancaBoleto._nomePessoaSacado = Convert.ToString(dr["Nme_Pessoa_Sacado"]);
					if (dr["Raz_Social_Sacado"] != DBNull.Value)
						objCobrancaBoleto._razaoSocialSacado = Convert.ToString(dr["Raz_Social_Sacado"]);
					if (dr["End_Email_Sacado"] != DBNull.Value)
						objCobrancaBoleto._enderecoEmailSacado = Convert.ToString(dr["End_Email_Sacado"]);
					if (dr["Des_Logradouro_Sacado"] != DBNull.Value)
						objCobrancaBoleto._descricaoLogradouroSacado = Convert.ToString(dr["Des_Logradouro_Sacado"]);
					if (dr["Num_Endereço_Sacado"] != DBNull.Value)
						objCobrancaBoleto._numeroEndereçoSacado = Convert.ToString(dr["Num_Endereço_Sacado"]);
					if (dr["Des_Complemento_Sacado"] != DBNull.Value)
						objCobrancaBoleto._descricaoComplementoSacado = Convert.ToString(dr["Des_Complemento_Sacado"]);
					if (dr["Idf_Cidade_Sacado"] != DBNull.Value)
						objCobrancaBoleto._cidadeSacado = new Cidade(Convert.ToInt32(dr["Idf_Cidade_Sacado"]));
					if (dr["Num_Cep_Sacado"] != DBNull.Value)
						objCobrancaBoleto._numeroCepSacado = Convert.ToString(dr["Num_Cep_Sacado"]);
					if (dr["Des_Bairro_Sacado"] != DBNull.Value)
						objCobrancaBoleto._descricaoBairroSacado = Convert.ToString(dr["Des_Bairro_Sacado"]);
					if (dr["Des_Instrucao_Caixa"] != DBNull.Value)
						objCobrancaBoleto._descricaoInstrucaoCaixa = Convert.ToString(dr["Des_Instrucao_Caixa"]);
					if (dr["Cod_Barras"] != DBNull.Value)
						objCobrancaBoleto._codigoBarras = Convert.ToString(dr["Cod_Barras"]);
					if (dr["Arq_Boleto"] != DBNull.Value)
						objCobrancaBoleto._arquivoBoleto = Convert.ToString(dr["Arq_Boleto"]);
					if (dr["Idf_Mensagem_Retorno_Boleto"] != DBNull.Value)
						objCobrancaBoleto._mensagemRetornoBoleto = new MensagemRetornoBoleto(Convert.ToInt32(dr["Idf_Mensagem_Retorno_Boleto"]));

					objCobrancaBoleto._persisted = true;
					objCobrancaBoleto._modified = false;

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