//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using BNE.BLL.Custom;
using BNE.BLL.DTO.OperadoraCelular;
using BNE.BLL.Enumeradores;
using BNE.EL;


namespace BNE.BLL
{
    public partial class PessoaFisica : ICloneable // Tabela: TAB_Pessoa_Fisica
    {
        #region Clone
        public object Clone()
        {
            return MemberwiseClone();
        }
        #endregion

        #region Propriedades

        #region NumeroCPF
        /// <summary>
        ///     Campo obrigatório.
        /// </summary>
        public string NumeroCPF
        {
            get
            {
                if (_numeroCPF <= 0)
                    return string.Empty;
                return _numeroCPF.ToString().PadLeft(11, '0').Insert(3, ".").Insert(7, ".").Insert(11, "-");
            }
            set
            {
                decimal.TryParse(value.Replace(".", "").Replace("-", ""), out _numeroCPF);
                _modified = true;
            }
        }
        #endregion

        #region CPF
        /// <summary>
        ///     Campo obrigatório.
        /// </summary>
        public decimal CPF
        {
            get { return _numeroCPF; }
            set
            {
                _numeroCPF = value;
                _modified = true;
            }
        }
        #endregion

        #region PrimeiroNome
        /// <summary>
        ///     Primeiro nome da pessoa
        /// </summary>
        public string PrimeiroNome
        {
            get
            {
                if (string.IsNullOrEmpty(_nomePessoa))
                    _nomePessoa = RecuperarNomePessoa();

                return RetornarPrimeiroNome(_nomePessoa);
            }
        }
        #endregion

        #region NomeCompleto
        /// <summary>
        ///     Nome completo da pessoa.
        ///     Propriedade para evitar dar um load object na TAB_Pessoa_Fisica apenas para recuperar o nome.
        /// </summary>
        public string NomeCompleto
        {
            get
            {
                if (string.IsNullOrEmpty(_nomePessoa))
                    _nomePessoa = RecuperarNomePessoa();

                return _nomePessoa;
            }
        }
        #endregion

        #region EmailPessoa
        /// <summary>
        ///     Tamanho do campo: 100.
        ///     Campo opcional.
        /// </summary>
        public string EmailPessoa
        {
            get
            {
                if (string.IsNullOrEmpty(_emailPessoa))
                    _emailPessoa = RecuperarEmailPessoa();

                return _emailPessoa;
            }
            set
            {
                //Se passar nulo ou sem valor o campo não está confirmado ou se o valor for diferente, também muda a flag
                if (string.IsNullOrWhiteSpace(value) || (!string.IsNullOrWhiteSpace(_emailPessoa) && !_emailPessoa.Trim().Equals(value.Trim())))
                {
                    _flagEmailConfirmado = false;
                }

                _emailPessoa = value;
                _modified = true;
            }
        }
        #endregion

        #region NumeroCelular
        /// <summary>
        ///     Tamanho do campo: 10.
        ///     Campo opcional.
        /// </summary>
        public string NumeroCelular
        {
            get { return _numeroCelular; }
            set
            {
                //Se passar nulo ou sem valor o campo não está confirmado ou se o valor for diferente, também muda a flag
                if (string.IsNullOrWhiteSpace(value) || (!string.IsNullOrWhiteSpace(_numeroCelular) && !_numeroCelular.Trim().Equals(value.Trim())))
                {
                    _flagCelularConfirmado = false;
                }

                _numeroCelular = value;
                _modified = true;
            }
        }
        #endregion

        #region DataCadastro
        /// <summary>
        ///     Campo obrigatório.
        /// </summary>
        public DateTime DataCadastro
        {
            get { return _dataCadastro; }
            set
            {
                _dataCadastro = value;
                _modified = true;
            }
        }
        #endregion

        #region DataAlteracao
        /// <summary>
        ///     Campo obrigatório.
        /// </summary>
        [Display(Name = "IgnoreData")]
        public DateTime DataAlteracao
        {
            get { return _dataAlteracao; }
            set
            {
                _dataAlteracao = value;
                _modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas

        #region DESCONFIRMAR_NUMERO_CELULAR
        private const string DESCONFIRMAR_NUMERO_CELULAR = @"UPDATE BNE.TAB_Pessoa_Fisica SET Flg_Celular_Confirmado = 0, Num_DDD_Celular = @Num_DDD_Celular, Num_Celular = @Num_Celular WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        #endregion

        #region SPSELECTPORCPF
        private const string SPSELECTIDPORCPF = @"
            SELECT  Idf_Pessoa_Fisica 
            FROM    TAB_Pessoa_Fisica WITH(NOLOCK) 
            WHERE Num_Cpf = @Num_Cpf AND Flg_Inativo = 0";

        private const string SPSELECTPORCPF = @"
            SELECT  * 
            FROM    TAB_Pessoa_Fisica WITH(NOLOCK) 
            WHERE Num_Cpf = @Num_Cpf";
        #endregion

        #region Spselectidfpessoafisicaporidfcurriculo
        private const string Spselectidfpessoafisicaporidfcurriculo = @"
        SELECT  Idf_Pessoa_Fisica 
        FROM    BNE_Curriculo C WITH(NOLOCK)
        WHERE   C.Idf_Curriculo = @Idf_Curriculo";

        private const string Spselectescolaridadepessoafisicaporidfcurriculo = @"
        SELECT  PF.Idf_Escolaridade 
        FROM    BNE_Curriculo as C WITH(NOLOCK)
		INNER JOIN TAB_Pessoa_Fisica as PF
		ON PF.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
        WHERE   C.Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region SPSELECTPORNOME
        private const string SPSELECTPORNOME = "SELECT * FROM TAB_Pessoa_Fisica WHERE Nme_Pessoa = @Nme_Pessoa";
        #endregion

        #region SPSELECTEXPERIENCIA
        private const string SPSELECTEXPERIENCIA = @"DECLARE @Top int =10
                                                        SELECT TOP (@Top) Idf_Experiencia_Profissional , 
                                                        CASE 
											                WHEN Dta_Demissao IS NULL THEN 1
											                WHEN Dta_Demissao IS NOT NULL THEN 2
										                END AS 'Ordem'
                                                    FROM BNE_Experiencia_Profissional 
                                                    WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                                                        AND Flg_Inativo = 0
                                                    ORDER BY Ordem ASC, Dta_Demissao DESC, Dta_Admissao DESC";
        #endregion

        #region SPSELECTULTIMAEXPERIENCIA
        private const string SPSELECTULTIMAEXPERIENCIA = @"--DECLARE @Numero_Experiencias int =1
                                                        SELECT TOP (@Numero_Experiencias) Idf_Experiencia_Profissional, Des_Atividade, Raz_Social
                                                    FROM BNE_Experiencia_Profissional 
                                                    WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                                                        AND Flg_Inativo = 0
                                                    --ORDER BY Dta_Demissao DESC, Dta_Admissao DESC
                                                    ORDER BY case when [Dta_Demissao] is not null then 1 else 0 end, Dta_Demissao DESC, Dta_Admissao DESC";
        #endregion

        #region SPDELETEREDESSOCIAIS
        private const string SPDELETEREDESSOCIAIS = "DELETE TAB_Pessoa_Fisica_Rede_Social WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        #endregion

        #region Spselectnomepessoa
        private const string Spselectnomepessoa = @"
        SELECT  Nme_Pessoa
        FROM    TAB_Pessoa_Fisica WITH(NOLOCK)
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";

        private const string Spselectnomepessoaporcpfdatanascimento = @"
        SELECT  Nme_Pessoa
        FROM    TAB_Pessoa_Fisica WITH(NOLOCK)
        WHERE   Num_CPF = @Num_CPF
                AND Dta_Nascimento = @Dta_Nascimento";

        private const string Spselectnomepessoaecelular = @"
        SELECT  Nme_Pessoa,  Num_DDD_Celular, Num_Celular
        FROM    TAB_Pessoa_Fisica WITH(NOLOCK)
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        #endregion

        #region Spselectemailpessoa
        private const string Spselectemailpessoa = @"
        SELECT  Eml_Pessoa
        FROM    TAB_Pessoa_Fisica WITH(NOLOCK)
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        #endregion

        #region SPVERIFICAUSUARIOINTERNOPORPF
        private const string SPVERIFICAUSUARIOINTERNOPORPF = @"SELECT   UFP.Idf_Usuario_Filial_Perfil,
                                                                        P.Idf_Perfil
                                                                FROM    BNE.TAB_Pessoa_Fisica PF
                                                                        JOIN TAB_Usuario_Filial_Perfil UFP ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
                                                                        JOIN TAB_Perfil P ON UFP.Idf_Perfil = P.Idf_Perfil
                                                                        JOIN BNE_Tipo_Perfil BTP ON P.Idf_Tipo_Perfil = BTP.Idf_Tipo_Perfil
                                                                WHERE   BTP.Idf_Tipo_Perfil = @Idf_Tipo_Perfil
                                                                        AND PF.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                                                                        AND PF.Flg_Inativo = 0 
                                                                        AND UFP.Flg_Inativo = 0 ";
        #endregion

        #region Spexistepessoafisica
        private const string Spexistepessoafisica = @"  
        SELECT  Idf_Pessoa_Fisica
        FROM    TAB_Pessoa_Fisica PF WITH(NOLOCK)
        WHERE   PF.Num_CPF = @Num_CPF";
        #endregion

        #region Sppessoafisicainativa
        private const string Sppessoafisicainativa = @"  
        SELECT  Flg_Inativo
        FROM    TAB_Pessoa_Fisica PF WITH(NOLOCK)
        WHERE   PF.Num_CPF = @Num_CPF";
        #endregion

        #region Spverificarcpfdatanascimentopessoafisica
        private const string Spverificarcpfdatanascimentopessoafisica = @"  
        SELECT  count(1)
        FROM    TAB_Pessoa_Fisica PF WITH(NOLOCK)
        WHERE   PF.Num_CPF = @Num_CPF
                AND PF.Dta_Nascimento = @Dta_Nascimento";
        #endregion

        #region SpRecuperarSessao
        private const string SpRecuperarSessao = @"
        SELECT Des_Session_ID FROM BNE_Usuario WITH(NOLOCK) WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
        ";
        #endregion

        #region SpAtualizarInteracao
        private const string SpAtualizarInteracao = @"
        UPDATE BNE_Usuario SET Dta_Ultima_Atividade = GETDATE() WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
        ";
        #endregion

        #region SpZerarInteracao
        private const string SpZerarInteracao = @"
        UPDATE BNE_Usuario SET Dta_Ultima_Atividade = null WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
        ";
        #endregion

        #region SpZerarInteracaoTodosUsuarios
        private const string SpZerarInteracaoTodosUsuarios = @"
        UPDATE BNE_Usuario SET Dta_Ultima_Atividade = null WHERE Dta_Ultima_Atividade IS NOT NULL 
        ";
        #endregion

        #region SpSelectOperadoraCelular
        private const string SpSelectOperadoraCelular = @"
        SELECT PF.Idf_Pessoa_Fisica, LTRIM(RTRIM(PF.Num_DDD_Celular)) AS DDD, LTRIM(RTRIM(PF.Num_Celular)) AS Numero, PF.Idf_Operadora_Celular, C.Idf_Curriculo
        FROM BNE.TAB_Pessoa_Fisica PF
        LEFT JOIN BNE.BNE_Curriculo C ON PF.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
        WHERE 1 = 1
        AND   PF.Flg_Inativo = 0
        AND   PF.Num_DDD_Celular   IS NOT NULL
        AND   PF.Num_Celular       IS NOT NULL
        AND   PF.Idf_Pessoa_Fisica BETWEEN @Idf_Pessoa_Fisica_Inicio AND @Idf_Pessoa_Fisica_Fim
";
        #endregion

        #region SpAtualizarOperadoraCelular
        private const string SpAtualizarOperadoraCelular = @"
        UPDATE BNE.TAB_Pessoa_Fisica
        SET Idf_Operadora_Celular = {0}
        WHERE Idf_Pessoa_Fisica IN ({1})
";
        #endregion

        #region SpVerificarEmail
        private const string SpVerificarEmail = @"
        DECLARE @email VARCHAR(4000)
        SET @email = [BNE].[BNE_BuscaMontaFT](@Eml_Pessoa)

        SELECT TOP ( 1 )
                1 -- so retorna um registro, com coluna unica e com valor igual a 1
        FROM    bne.tab_pessoa_fisica pf WITH ( NOLOCK )
        WHERE   1 = 1
                AND CONTAINS ( pf.Eml_Pessoa, @email )
                AND ( pf.Idf_Email_Situacao_Confirmacao = 2
                      OR pf.Idf_Email_Situacao_Validacao = 4
                      OR pf.Idf_Email_Situacao_Bloqueio = 5
                    )";
        #endregion

        #region SpAtualizarEmailBloqueado
        private const string SpAtualizarEmailBloqueado = @"
        update bne.tab_pessoa_fisica
        set idf_email_situacao_bloqueio = 5     --bloqueado
           ,idf_email_situacao_confirmacao = 1  --confirmado
           ,idf_email_situacao_validacao = 3    --validado
        where 1 = 1
        and   contains(Eml_Pessoa, @Eml_Pessoa)
";
        #endregion

        #region SpAtualizarEmailBounce
        private const string SpAtualizarEmailBounce = @"
        update bne.tab_pessoa_fisica
        set idf_email_situacao_confirmacao = 2  --nao confirmado
           ,idf_email_situacao_validacao = 4    --nao validado
           ,idf_email_situacao_bloqueio = null  --indiferente
        where 1 = 1
        and   contains(Eml_Pessoa, @Eml_Pessoa)
";
        #endregion

        #region SpAtualizarEmailInvalido
        private const string SpAtualizarEmailInvalido = @"
        update bne.tab_pessoa_fisica
        set idf_email_situacao_confirmacao = null --desconhecido se esta confirmado
           ,idf_email_situacao_validacao = 4      --nao validado 
           ,idf_email_situacao_bloqueio = null    --indiferente
        where 1 = 1
        and   contains(Eml_Pessoa, @Eml_Pessoa)
";
        #endregion

        #region SpAtualizarEmailUnsubscribe
        private const string SpAtualizarEmailUnsubscribe = @"
        update bne.tab_pessoa_fisica
        set idf_email_situacao_confirmacao = 1    --confirmado
           ,idf_email_situacao_validacao = 3      --validado
           ,idf_email_situacao_bloqueio = NULL    --a princípio não está bloqueado
        where 1 = 1
        and   contains(Eml_Pessoa, @Eml_Pessoa)
";
        #endregion

        #region SpCarregarIdCadastradoEm
        private const string SpCarregarIdCadastradoEm = @"
        SELECT TOP (1) Idf_Pessoa_Fisica
        FROM BNE.TAB_Pessoa_Fisica
        WHERE Dta_Cadastro BETWEEN @Dta_Cadastro AND GETDATE()
        ORDER BY Idf_Pessoa_Fisica
";
        #endregion SpCarregarIdCadastradoEm

        #region SpRecuperarInformacoesDePessoaFisicaDePagamento
        private const string SpRecuperarInformacoesDePessoaFisicaDePagamento = @"
        SELECT  PF.*
        FROM    BNE.BNE_Pagamento PG WITH ( NOLOCK )
                JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH ( NOLOCK ) ON UFP.Idf_Usuario_Filial_Perfil = PG.Idf_Usuario_Filial_Perfil
				JOIN BNE.TAB_Pessoa_Fisica PF WITH ( NOLOCK ) ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
        WHERE   PG.Idf_Filial IS NULL
                AND Idf_Pagamento = @Idf_Pagamento";
        #endregion

        #region SpRecuperarInformacoesIntegracaoFinanceiro
        private const string SpRecuperarInformacoesIntegracaoFinanceiro = @"
        select pp.Dta_Pagamento
	        , pg.Vlr_Pagamento
	        , pg.Des_identificador
	        , pa.Dta_Inicio_Plano
	        , pa.Dta_Fim_Plano
	        , en.Num_CEP
	        , en.Des_Logradouro
	        , en.Num_Endereco
	        , en.Des_Complemento
	        , en.Des_Bairro
	        , ci.Nme_Cidade
	        , ci.Sig_Estado
	        , (CASE WHEN pp.Eml_Envio_Boleto IS NOT NULL AND PP.Eml_Envio_Boleto <> '' THEN  PP.Eml_Envio_Boleto ELSE PF.Eml_Pessoa END) AS Eml_Pessoa
	        , pf.Num_DDD_Celular
	        , pf.Num_Celular
	        , pf.Nme_Pessoa
	        , pf.Num_CPF
			, trans_num_banco.Idf_Banco
			, fl_gestora.Nme_Fantasia as Filial_Gestora
            , en.Idf_Endereco
        from bne.BNE_Pagamento pg with(nolock)
			outer apply (
				SELECT TOP 1 trans.Idf_Banco, trans.idf_transacao FROM BNE.BNE_Transacao trans with(nolock)
				WHERE trans.Idf_Pagamento = pg.Idf_Pagamento AND trans.Idf_Tipo_Pagamento = pg.Idf_Tipo_Pagamento
				ORDER BY trans.Dta_Cadastro) trans_num_banco
	        join bne.BNE_Plano_Parcela pp with(nolock) on pg.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
	        join bne.bne_plano_adquirido pa with(nolock) on pp.idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
			left join bne.bne_plano_adquirido_detalhes pad with(nolock) on pad.idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
	        left join bne.tab_filial fl_gestora with(nolock) on pad.Idf_Filial_Gestora = fl_gestora.Idf_Filial
	        join bne.tab_usuario_filial_perfil ufp with(nolock) on pa.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
	        join bne.TAB_Pessoa_Fisica pf with(nolock) on ufp.idf_pessoa_fisica = pf.Idf_Pessoa_Fisica
	        left join bne.TAB_Endereco en with(nolock) on pf.Idf_Endereco = en.Idf_Endereco
	        left join plataforma.TAB_Cidade ci with(nolock) on en.Idf_Cidade = ci.Idf_Cidade
        where pg.Idf_Pagamento = @Idf_Pagamento    
        ";
        #endregion

        #region SpInvalidarCelular
        private const string SpInvalidarCelular = @"
        SELECT  PF.Idf_Pessoa_Fisica, C.Idf_Curriculo	
        FROM	BNE.TAB_Pessoa_Fisica PF 
                LEFT JOIN BNE.BNE_Curriculo C  ON PF.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
        WHERE   PF.Num_DDD_Celular = @Num_DDD_Celular 
	            AND PF.Num_Celular = @Num_Celular 
	            AND (C.Idf_Curriculo IS NULL OR C.Idf_Curriculo <> @Idf_Curriculo)
        ";
        private const string SpLimparCelular = @"
        UPDATE  PF
        SET     Num_DDD_Celular = null, Num_Celular = null, Flg_Celular_Confirmado = 0
        FROM	BNE.TAB_Pessoa_Fisica PF
        WHERE   PF.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
        ";
        #endregion

        #region SpUsuarioTanque
        private const string SpUsuarioTanque = @"
        select ufp.Idf_Usuario_Filial_Perfil 
              , pf.Nme_Pessoa
              , fl.Raz_Social
              , fl.Num_CNPJ
		      ,fl.Num_DDD_Comercial+fl.Num_Comercial as 'Telefone'
        from bne.tab_pessoa_fisica pf with (nolock)
              join bne.tab_usuario_filial_perfil ufp with (nolock) on ufp.Idf_pessoa_fisica = pf.Idf_Pessoa_Fisica
              join bne.tab_filial fl with (nolock) on ufp.Idf_Filial = fl.Idf_Filial
        where pf.Num_CPF = @Num_CPF ​
        AND ufp.Flg_Inativo = 0";
        #endregion

        #region SprecuperarUltimaEmpresaCadastrada
        private const string SprecuperarUltimaEmpresaCadastrada = @"
        select  TOP 1 ufp.Idf_Filial
        from    tab_pessoa_fisica pf with (nolock)
                join tab_usuario_filial_perfil ufp with (nolock) on ufp.Idf_pessoa_fisica = pf.Idf_Pessoa_Fisica
                join tab_filial F with(nolock) on UFP.Idf_Filial = F.Idf_Filial
        where   pf.Num_CPF = @Num_CPF ​
                AND ufp.Flg_Inativo = 0
                AND f.Flg_Inativo = 0
        ORDER BY F.Dta_Cadastro DESC";
        #endregion

        #region SpAtualizarDataNascimento
        private const string SpAtualizarDataNascimento = @"
        UPDATE  TAB_Pessoa_Fisica 
                SET Dta_Nascimento = @Dta_Nascimento
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
        ";
        #endregion

        #region SpRecuperarDataNascimento
        private const string SpRecuperarDataNascimento = @"
        SELECT  Dta_Nascimento
        FROM    TAB_Pessoa_fisica WITH(NOLOCK)
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
        ";
        #endregion

        #endregion

        #region Métodos

        //TODO: Rever estes métodos CarregarPorCPF, principalmente com a regitirada dos .-

        #region CarregarIdPorCPF
        /// <summary>
        ///     Método responsável por carregar uma instância de PessoaFisica pelo parâmetro CPF.
        /// </summary>
        /// <param name="numCpf">Valor de CPF</param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static int CarregarIdPorCPF(decimal numCpf, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Num_CPF", SqlDbType = SqlDbType.Decimal, Size = 11, Value = numCpf}
            };

            var res = DataAccessLayer.ExecuteScalar(trans, CommandType.Text, SPSELECTIDPORCPF, parms);

            int pfId;
            if (Convert.IsDBNull(res) || res == null || !int.TryParse(res.ToString(), out pfId))
                return 0;

            return pfId;
        }
        #endregion

        #region CarregarPorCPF
        public static PessoaFisica CarregarPorCPF(decimal numCpf, SqlTransaction trans = null)
        {
            PessoaFisica objPessoaFisica;
            if (CarregarPorCPF(numCpf, out objPessoaFisica, trans))
                return objPessoaFisica;
            throw (new RecordNotFoundException(typeof(PessoaFisica)));
        }
        /// <summary>
        ///     Método responsável por carregar uma instância de PessoaFisica pelo parâmetro CPF.
        /// </summary>
        /// <param name="numCpf">Valor de CPF</param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PessoaFisica CarregarPorCPF(string numCpf)
        {
            return CarregarPorCPF(Convert.ToDecimal(Helper.LimparMascaraCPFCNPJ(numCpf)));
        }

        /// <summary>
        ///     Método responsável por carregar uma instância de PessoaFisica pelo parâmetro CPF.
        /// </summary>
        /// <param name="numCpf">Valor de CPF</param>
        /// <param name="objPessoaFisica">Objeto que será inicializado se houver pessoa física</param>
        /// <param name="trans">Transação, pode ser null</param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorCPF(decimal numCpf, out PessoaFisica objPessoaFisica, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Num_CPF", SqlDbType = SqlDbType.Decimal, Size = 11, Value = numCpf}
            };

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTPORCPF, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORCPF, parms);

            var retorno = false;
            objPessoaFisica = new PessoaFisica();
            if (SetInstance(dr, objPessoaFisica))
            {
                retorno = true;
            }
            else
            {
                objPessoaFisica = null;
            }

            dr.Close();
            dr.Dispose();

            return retorno;
        }

        /// <summary>
        ///     Método responsável por carregar uma instância de PessoaFisica pelo parâmetro CPF.
        /// </summary>
        /// <param name="numCpf">Valor de CPF</param>
        /// <param name="objPessoaFisica"></param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorCPF(string numCpf, out PessoaFisica objPessoaFisica)
        {
            return CarregarPorCPF(Convert.ToDecimal(Helper.LimparMascaraCPFCNPJ(numCpf)), out objPessoaFisica);
        }
        #endregion

        #region CarregarPorNome
        /// <summary>
        ///     Método responsável por carregar uma instância de PessoaFisica pelo nome da pessoa
        /// </summary>
        /// <param name="strNomePessoa">Nome da pessoa</param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PessoaFisica CarregarPorNome(string strNomePessoa)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 100));
            parms[0].Value = strNomePessoa;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORNOME, parms))
            {
                var objPessoaFisica = new PessoaFisica();
                if (SetInstance(dr, objPessoaFisica))
                    return objPessoaFisica;

                if (!dr.IsClosed)
                    dr.Close();
            }
            throw (new RecordNotFoundException(typeof(PessoaFisica)));
        }

        /// <summary>
        ///     Método responsável por carregar uma instância de PessoaFisica pelo nome da pessoa fisica
        /// </summary>
        /// <param name="strNomePessoa">Nome da Pessoa Física</param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorNome(string strNomePessoa, out PessoaFisica objPessoaFisica)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 100));
            parms[0].Value = strNomePessoa;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORNOME, parms))
            {
                objPessoaFisica = new PessoaFisica();
                if (SetInstance(dr, objPessoaFisica))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objPessoaFisica = null;
            return false;
        }
        #endregion

        #region RecuperarIdPorCurriculo
        /// <summary>
        ///     Método responsável por carregar o código identificador de uma pessoa física (Idf_Pessoa_Fisica) através do código
        ///     identificador do currículo
        /// </summary>
        /// <param name="objCurriculo">Curriculo</param>
        /// <returns></returns>
        public static int RecuperarIdPorCurriculo(Curriculo objCurriculo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo}
            };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectidfpessoafisicaporidfcurriculo, parms));
        }

        /// <summary>
        ///     Método responsável por carregar a escolaridade de uma pessoa física (Idf_Escolaridade) através do código
        ///     identificador do currículo
        /// </summary>
        /// <param name="objCurriculoId">Curriculo ID</param>
        /// <returns></returns>
        private static int? RecuperarEscolaridadePorCurriculoId(int objCurriculoId)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculoId}
            };
            var escolaridadeId = DataAccessLayer.ExecuteScalar(CommandType.Text,
                Spselectescolaridadepessoafisicaporidfcurriculo, parms);
            if (escolaridadeId == null
                || Convert.IsDBNull(escolaridadeId))
                return null;

            return Convert.ToInt32(escolaridadeId);
        }

        public static bool ConsideraEstudantePorCurriculoId(int idCurriculo)
        {
            if (idCurriculo <= 0)
                return false;

            var escolaridadeId = RecuperarEscolaridadePorCurriculoId(idCurriculo);

            if (escolaridadeId == null)
                return false;

            return ConsideraEstudantePorEscolaridadeId(escolaridadeId.Value);
        }

        public bool ConsideraEstudante()
        {
            if (Escolaridade == null)
            {
                if (IdPessoaFisica <= 0)
                    return false;

                if (!CompleteObject())
                    return false;

                if (Escolaridade == null)
                    return false;

                return ConsideraEstudantePorEscolaridadeId(Escolaridade.IdEscolaridade);
            }

            return ConsideraEstudantePorEscolaridadeId(Escolaridade.IdEscolaridade);
        }

        private static bool ConsideraEstudantePorEscolaridadeId(int escolaridadeId)
        {
            if (escolaridadeId <= 0)
                return false;

            var escolaridade = (Enumeradores.Escolaridade)escolaridadeId;

            switch (escolaridade)
            {
                case Enumeradores.Escolaridade.EnsinoFundamentalIncompleto:
                    break;
                case Enumeradores.Escolaridade.EnsinoFundamentalCompleto:
                    break;
                case Enumeradores.Escolaridade.TecnicoPosMedioCompleto:
                    break;
                case Enumeradores.Escolaridade.EnsinoMedioIncompleto:
                    return true;
                case Enumeradores.Escolaridade.TecnicoPosMedioIncompleto:
                    return true;
                case Enumeradores.Escolaridade.TecnologoIncompleto:
                    return true;
                case Enumeradores.Escolaridade.SuperiorIncompleto:
                    return true;
                case Enumeradores.Escolaridade.EnsinoMedioCompleto:
                    break;
                case Enumeradores.Escolaridade.TecnologoCompleto:
                    break;
                case Enumeradores.Escolaridade.SuperiorCompleto:
                    break;
                case Enumeradores.Escolaridade.PosGraduacaoEspecializacao:
                    break;
                case Enumeradores.Escolaridade.Mestrado:
                    break;
                case Enumeradores.Escolaridade.Doutorado:
                    break;
                case Enumeradores.Escolaridade.PosDoutorado:
                    break;
                case Enumeradores.Escolaridade.OutrosCursos:
                    break;
            }

            return false;
        }
        #endregion

        #region RecuperarExperienciaProfissional
        /// <summary>
        ///     Retorna uma list com os ids das experiencias profissionais da pessoa fisica
        /// </summary>
        /// <returns></returns>
        public List<int> RecuperarExperienciaProfissional(SqlTransaction trans)
        {
            var list = new List<int>();
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Decimal, 11));
            parms[0].Value = _idPessoaFisica;

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTEXPERIENCIA, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTEXPERIENCIA, parms);

            while (dr.Read())
            {
                list.Add(Convert.ToInt32(dr[0]));
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return list;
        }
        #endregion

        #region RecuperarUltimaExperienciaProfissional
        /// <summary>
        ///     Retorna a ultima experiencia profissionais da pessoa fisica
        /// </summary>
        /// <returns></returns>
        public List<ExperienciaProfissional> RecuperarUltimaExperienciaProfissional(SqlTransaction trans, int numeroExperiencia)
        {
            var list = new List<ExperienciaProfissional>();
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Decimal, 11));
            parms.Add(new SqlParameter("@Numero_Experiencias", SqlDbType.Int, 4));

            parms[0].Value = _idPessoaFisica;
            parms[1].Value = numeroExperiencia;

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTULTIMAEXPERIENCIA, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTULTIMAEXPERIENCIA, parms);

            while (dr.Read())
            {
                var expe = ExperienciaProfissional.LoadObject(Convert.ToInt32(dr[0]));
                list.Add(expe);
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return list;
        }
        #endregion

        #region DeleteRedesSociais
        /// <summary>
        ///     Método utilizado para excluir todas as instancias de Redes Sociais daquela pessoa fisica
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public void DeleteRedesSociais(SqlTransaction trans)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));

            parms[0].Value = _idPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETEREDESSOCIAIS, parms);
        }
        #endregion

        #region ParametroDataAlteracao
        private static bool ParametroDataAlteracao(SqlParameter parm)
        {
            if (parm.ParameterName == "@Dta_Alteracao")
                return true;

            return false;
        }
        #endregion

        #region VerificaPessoaFisicaUsuarioInterno
        /// <summary>
        ///     Verifica se a pessoa fisica informada é usuário interno. Se for retorna o IdUsuarioFilialPerfil e o IdPerfil do
        ///     mesmo.
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <param name="idUsuarioFilialPerfil">out</param>
        /// <param name="idPerfil">out</param>
        /// <returns>bool usuarioInterno</returns>
        public static bool VerificaPessoaFisicaUsuarioInterno(int idPessoaFisica, out int idUsuarioFilialPerfil, out int idPerfil)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Perfil", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisica;
            parms[1].Value = Enumeradores.TipoPerfil.Interno.GetHashCode();

            var usuarioInterno = false;
            idUsuarioFilialPerfil = 0;
            idPerfil = 0;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPVERIFICAUSUARIOINTERNOPORPF, parms))
            {
                if (dr.Read())
                {
                    idPerfil = Convert.ToInt32(dr["Idf_Perfil"]);
                    idUsuarioFilialPerfil = Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]);
                    usuarioInterno = true;
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return usuarioInterno;
        }
        #endregion

        #region RetornarIdade
        /// <summary>
        ///     Calcula a idade da Pessoa
        /// </summary>
        /// <returns></returns>
        public int RetornarIdade()
        {
            return Helper.CalcularIdade(_dataNascimento);
        }
        #endregion

        #region RetornarPrimeiroNome
        /// <summary>
        ///     Método utilizado para retornar o primeiro nome da pessoa.
        /// </summary>
        /// <param name="nomeCompleto">String nome completo</param>
        /// <returns>Primeiro nome</returns>
        public static string RetornarPrimeiroNome(string nomeCompleto)
        {
            return Helper.RetornarPrimeiroNome(nomeCompleto);
        }
        #endregion

        #region RecuperarNomePessoa
        /// <summary>
        ///     Método que retorna o Nome(Nme_Pessoa)da pessoa fisica
        /// </summary>
        /// <returns>Nme_Pessoa</returns>
        public string RecuperarNomePessoa()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPessoaFisica}
            };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectnomepessoa, parms));
        }
        #endregion

        #region RecuperarNomePessoa
        /// <summary>
        ///     Método que retorna o Nome(Nme_Pessoa)da pessoa fisica
        /// </summary>
        /// <returns>Nme_Pessoa</returns>
        public string RecuperarNomePessoaPorCPFDataNascimento(decimal numCPF, DateTime dataNascimento)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Num_CPF", SqlDbType = SqlDbType.Decimal, Size = 11, Value = numCPF},
                new SqlParameter {ParameterName = "@Dta_Nascimento", SqlDbType = SqlDbType.Date, Value = dataNascimento}
            };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectnomepessoaporcpfdatanascimento, parms));
        }
        #endregion


        #region RecuperarNomeECelularPessoa
        public KeyValuePair<string, string> RecuperarNomeECelularPessoa()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPessoaFisica}
            };

            var objDataTable = new DataTable();

            using (var select = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectnomepessoaecelular, parms))
            {
                objDataTable.Load(select);
            }

            if (objDataTable.Rows.Count == 0)
            {
                return new KeyValuePair<string, string>(string.Empty, string.Empty);
            }

            var nome = Convert.IsDBNull(objDataTable.Rows[0][0]) ? string.Empty : objDataTable.Rows[0][0].ToString();
            var ddd = Convert.IsDBNull(objDataTable.Rows[0][1]) ? string.Empty : objDataTable.Rows[0][1].ToString();
            var celular = Convert.IsDBNull(objDataTable.Rows[0][2]) ? string.Empty : objDataTable.Rows[0][2].ToString();

            return new KeyValuePair<string, string>(nome, ddd + celular);
        }
        #endregion

        #region RecuperarEmailPessoa
        /// <summary>
        ///     Método que retorna o Email(Eml_Pessoa)da pessoa fisica
        /// </summary>
        /// <returns>Eml_Pessoa</returns>
        private string RecuperarEmailPessoa()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPessoaFisica}
            };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectemailpessoa, parms));
        }
        #endregion

        #region ExistePessoaFisica
        /// <summary>
        ///     Verifica se existe pessoa fisica cadastrada com o cpf informado
        /// </summary>
        /// <param name="numeroCPF"></param>
        /// <param name="idPessoaFisica"></param>
        /// <returns></returns>
        public static bool ExistePessoaFisica(decimal numeroCPF, out int idPessoaFisica, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Num_CPF", SqlDbType = SqlDbType.Decimal, Size = 11, Value = numeroCPF}
            };

            object retorno;
            if (trans != null)
            {
                retorno = DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spexistepessoafisica, parms);
            }
            else
            {
                retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, Spexistepessoafisica, parms);
            }

            if (retorno != null)
            {
                idPessoaFisica = Convert.ToInt32(retorno);
                return true;
            }

            idPessoaFisica = 0;
            return false;
        }
        public static bool ExistePessoaFisica(string numeroCPF, out int idPessoaFisica)
        {
            numeroCPF = Helper.LimparMascaraCPFCNPJ(numeroCPF);
            Decimal cpf; idPessoaFisica = 0;
            if (Decimal.TryParse(numeroCPF, out cpf))
                return ExistePessoaFisica(cpf, out idPessoaFisica);
            else
                return false;
        }
        #endregion

        #region Inativo
        /// <summary>
        ///     Verifica se a pessoa fisica cadastrada com o cpf informado está inativo
        /// </summary>
        /// <param name="numeroCPF"></param>
        /// <returns></returns>
        public bool Inativo(decimal numeroCPF)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Num_CPF", SqlDbType = SqlDbType.Decimal, Size = 11, Value = numeroCPF}
            };
            var retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, Sppessoafisicainativa, parms);

            if (retorno != null && retorno != DBNull.Value)
            {
                return Convert.ToBoolean(retorno);
            }

            return false;
        }
        #endregion

        #region ValidarCPFDataNascimento
        /// <summary>
        ///     Verifica se o CPF e a Data de Nascimento informados batem com os dados que estão salvos no banco
        /// </summary>
        /// <param name="numeroCPF"></param>
        /// <param name="dataNascimento"></param>
        /// <returns></returns>
        public static bool ValidarCPFDataNascimento(decimal numeroCPF, DateTime dataNascimento)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Num_CPF", SqlDbType = SqlDbType.Decimal, Size = 11, Value = numeroCPF},
                new SqlParameter {ParameterName = "@Dta_Nascimento", SqlDbType = SqlDbType.DateTime, Value = dataNascimento}
            };
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spverificarcpfdatanascimentopessoafisica, parms)) > 0;
        }

        public static bool ValidarCPFDataNascimento(string numeroCPF, DateTime dataNascimento)
        {
            numeroCPF = new Regex("[.\\/-]").Replace(numeroCPF, "");
            return ValidarCPFDataNascimento(Convert.ToDecimal(numeroCPF), dataNascimento);
        }
        #endregion

        #region AtualizarDataNascimento
        public void AtualizarDataNascimento(DateTime dataNascimento, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@Dta_Nascimento", SqlDbType = SqlDbType.DateTime, Value = dataNascimento},
                new SqlParameter{ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Value = this._idPessoaFisica}
            };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SpAtualizarDataNascimento, parms);
        }
        #endregion

        #region RecuperarSessaoAtual
        /// <summary>
        ///     Recupera o id da sessão relacionada ao usuário
        /// </summary>
        /// <returns>String identificadora da sessão</returns>
        public string RecuperarSessaoAtual()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4)
            };

            parms[0].Value = IdPessoaFisica;

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, SpRecuperarSessao, parms));
        }
        #endregion

        #region AtualizarDataInteracaoUsuario
        /// <summary>
        ///     Método responsável por atualizar a interação do usuário
        /// </summary>
        public void AtualizarDataInteracaoUsuario()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4)
            };

            parms[0].Value = IdPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpAtualizarInteracao, parms);
        }
        #endregion

        #region ZerarDataInteracaoUsuario
        /// <summary>
        ///     Método responsável por zerar a data de interação do usuário
        /// </summary>
        public void ZerarDataInteracaoUsuario()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4)
            };

            parms[0].Value = IdPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpZerarInteracao, parms);
        }
        #endregion

        #region ZerarDataInteracaoTodosUsuarios
        /// <summary>
        ///     Método responsável por zerar a data de interação do usuário
        /// </summary>
        public static void ZerarDataInteracaoTodosUsuarios()
        {
            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpZerarInteracaoTodosUsuarios, null);
        }
        #endregion

        #region CarregarOperadoraCelular
        public static List<PessoaFisicaOperadoraCelular> CarregarOperadoraCelular(int idPessoaFisicaInicial, int idPessoaFisicaFinal)
        {
            var lista = new List<PessoaFisicaOperadoraCelular>();

            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Pessoa_Fisica_Inicio", SqlDbType = SqlDbType.Int, Size = 4, Value = idPessoaFisicaInicial},
                new SqlParameter {ParameterName = "@Idf_Pessoa_Fisica_Fim", SqlDbType = SqlDbType.Int, Size = 4, Value = idPessoaFisicaFinal}
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectOperadoraCelular, parms))
            {
                while (dr.Read())
                {
                    lista.Add(new PessoaFisicaOperadoraCelular
                    {
                        IdPessoaFisica = Convert.ToInt32(dr["Idf_Pessoa_Fisica"]),
                        IdOperadoraCelular = dr["Idf_Operadora_Celular"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Operadora_Celular"]) : (int?)null,
                        DDD = dr["DDD"].ToString(),
                        Numero = dr["Numero"].ToString(),
                        IdCurriculo = dr["Idf_Curriculo"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Curriculo"]) : (int?)null
                    });
                }
            }

            return lista;
        }
        #endregion

        #region AtualizarOperadoraCelular
        public static void AtualizarOperadoraCelular(int idOperadoraCelular, List<int> lista)
        {
            var listaIds = string.Join(",", lista);

            var sql =
                string.Format(SpAtualizarOperadoraCelular,
                    idOperadoraCelular == 0 ? "NULL" : idOperadoraCelular.ToString(),
                    listaIds);

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, sql, null);
        }
        #endregion

        #region VerificarEmail
        /// <summary>
        ///     Verifica se é permitido enviar e-mail para o endereço informado
        /// </summary>
        /// <param name="emailDestinatario">E-mail do destinatário</param>
        /// <returns>true se for permitido, false se não for permitido</returns>
        public static bool VerificarEmail(string emailDestinatario)
        {
            //Tratando string para o full-text
            //emailDestinatario = "\"" + emailDestinatario + "\"";

            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Eml_Pessoa", SqlDbType = SqlDbType.NVarChar, Size = 100, Value = emailDestinatario}
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpVerificarEmail, parms))
            {
                return !dr.Read(); // se não voltar nenhuma linha, está correto
            }
        }
        #endregion

        #region AtualizarEmailBloqueado
        public static void AtualizarEmailBloqueado(string email)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Eml_Pessoa", SqlDbType = SqlDbType.VarChar, Size = 8000, Value = email}
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpAtualizarEmailBloqueado, parms);
        }
        #endregion

        #region AtualizarEmailBounce
        public static void AtualizarEmailBounce(string email)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Eml_Pessoa", SqlDbType = SqlDbType.VarChar, Size = 8000, Value = email}
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpAtualizarEmailBounce, parms);
        }
        #endregion

        #region AtualizarEmailInvalido
        public static void AtualizarEmailInvalido(string email)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Eml_Pessoa", SqlDbType = SqlDbType.VarChar, Size = 8000, Value = email}
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpAtualizarEmailInvalido, parms);
        }
        #endregion

        #region AtualizarEmailUnsubscribe
        public static void AtualizarEmailUnsubscribe(string email)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Eml_Pessoa", SqlDbType = SqlDbType.VarChar, Size = 8000, Value = email}
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpAtualizarEmailUnsubscribe, parms);
        }
        #endregion

        #region CarregarIdCadastradoEm
        public static int CarregarIdCadastradoEm(DateTime dataCadastro)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Dta_Cadastro", SqlDbType = SqlDbType.DateTime, Value = dataCadastro}
            };

            var retorno = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpCarregarIdCadastradoEm, parms));

            return retorno;
        }
        #endregion CarregarIdCadastradoEm

        #region RecuperarInformacoesIntegracaoFinanceiro
        public static bool RecuperarInformacoesIntegracaoFinanceiro(int idPagamento, out DateTime dataPagamento, out decimal valorPagamento, out string desIdentificador, out DateTime dataInicioPlano, out DateTime dataFimPlano, out string numCPF, out string nomePessoa, out string numCEP, out string rua, out int numEndereco, out string complemento, out string bairro, out string cidade, out string uf, out string emailContato, out string ddd, out string telefone, out int numeroBanco, out string filialGestora, SqlTransaction trans = null)
        {
            var retorno = false;
            dataPagamento = new DateTime();
            valorPagamento = 0;
            desIdentificador = string.Empty;
            dataInicioPlano = new DateTime();
            dataFimPlano = new DateTime();
            numCPF = string.Empty;
            nomePessoa = string.Empty;
            numCEP = string.Empty;
            rua = string.Empty;
            numEndereco = 0;
            complemento = string.Empty;
            bairro = string.Empty;
            cidade = string.Empty;
            uf = string.Empty;
            emailContato = string.Empty;
            ddd = string.Empty;
            telefone = string.Empty;
            numeroBanco = (int)Enumeradores.Banco.HSBC; //Default HSBC
            filialGestora = "BNE"; //Default BNE

            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Pagamento", SqlDbType = SqlDbType.Int, Size = 6, Value = idPagamento}
            };

            IDataReader dr = trans == null ? DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarInformacoesIntegracaoFinanceiro, parms) : DataAccessLayer.ExecuteReader(trans, CommandType.Text, SpRecuperarInformacoesIntegracaoFinanceiro, parms);

            if (dr.Read())
            {
                if (dr["Dta_Pagamento"] != null && !string.IsNullOrEmpty(dr["Dta_Pagamento"].ToString()))
                    dataPagamento = Convert.ToDateTime(dr["Dta_Pagamento"]);
                if (dr["Vlr_Pagamento"] != null)
                    valorPagamento = Convert.ToDecimal(dr["Vlr_Pagamento"]);
                if (dr["Des_Identificador"] != null)
                    desIdentificador = dr["Des_Identificador"].ToString();
                if (dr["Dta_Inicio_Plano"] != null)
                    dataInicioPlano = Convert.ToDateTime(dr["Dta_Inicio_Plano"]);
                if (dr["Dta_Fim_Plano"] != null)
                    dataFimPlano = Convert.ToDateTime(dr["Dta_Fim_Plano"]);
                if (dr["Nme_Pessoa"] != null)
                    nomePessoa = dr["Nme_Pessoa"].ToString();

                //CASO NÃO ENCONTRE OS DADOS USE O DO BNE
                if (string.IsNullOrEmpty(dr["Idf_Endereco"].ToString()))
                {
                    numCEP = "1411001";
                    rua = "R PADRE JOAO MANOEL";
                    numEndereco = 0;
                    bairro = "JARDIM PAULISTA";
                    cidade = "São Paulo";
                    uf = "SP";
                }
                else
                {
                    if (dr["Num_CEP"] != null)
                        numCEP = dr["Num_CEP"].ToString();
                    if (dr["Des_Logradouro"] != null)
                        rua = dr["Des_Logradouro"].ToString();
                    if (!int.TryParse(dr["Num_Endereco"].ToString(), out numEndereco))
                    {
                        numEndereco = 0;
                    }
                    if (dr["Des_Complemento"] != DBNull.Value)
                        complemento = dr["Des_Complemento"].ToString();
                    if (dr["Des_Bairro"] != null)
                        bairro = dr["Des_Bairro"].ToString();
                    if (dr["Nme_Cidade"] != null)
                        cidade = dr["Nme_Cidade"].ToString();
                    if (dr["Sig_Estado"] != null)
                        uf = dr["Sig_Estado"].ToString();
                }
                if (dr["Eml_Pessoa"] != null)
                    emailContato = dr["Eml_Pessoa"].ToString();
                if (dr["Num_DDD_Celular"] != null)
                    ddd = dr["Num_DDD_Celular"].ToString();
                if (dr["Num_Celular"] != null)
                    telefone = dr["Num_Celular"].ToString();
                if (dr["Num_CPF"] != null)
                    numCPF = dr["Num_CPF"].ToString();
                if (dr["Filial_Gestora"] != DBNull.Value)
                    filialGestora = dr["Filial_Gestora"].ToString();
                if (dr["Idf_Banco"] != DBNull.Value)
                    numeroBanco = Convert.ToInt32(dr["Idf_Banco"].ToString());

                retorno = true;
            }

            if (!dr.IsClosed)
            {
                dr.Close();
            }
            dr.Dispose();

            return retorno;
        }
        #endregion

        #region CelularValidado
        /// <summary>
        ///     Método que invalida o celular dos currículos diferente do informado
        /// </summary>
        /// <param name="objCurriculo">Pessoa Física que informou o celular correto</param>
        /// <param name="numDDDCelular">DDD do Celular</param>
        /// <param name="numCelular">Número do Celular</param>
        /// <param name="trans">Transação</param>
        /// <returns></returns>
        internal static void CelularValidado(Curriculo objCurriculo, string numDDDCelular, string numCelular, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Num_DDD_Celular", SqlDbType = SqlDbType.Char, Size = 2, Value = numDDDCelular},
                new SqlParameter {ParameterName = "@Num_Celular", SqlDbType = SqlDbType.Char, Size = 10, Value = numCelular},
                new SqlParameter {ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo}
            };

            var lista = new List<Tuple<int, int?>>();
            using (var dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SpInvalidarCelular, parms))
            {
                while (dr.Read())
                    lista.Add(new Tuple<int, int?>(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]), Convert.IsDBNull(dr["Idf_Curriculo"]) ? (int?)null : Convert.ToInt32(dr["Idf_Curriculo"])));
            }

            const string nomeProcessoPaiParaSalvarNoCRM = "InvalidarCelular";
            foreach (var tuple in lista)
            {
                var pessoaFisica = tuple.Item1;
                var curriculo = tuple.Item2;

                parms.Clear();

                parms.Add(new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = pessoaFisica });

                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SpLimparCelular, parms);

                if (curriculo.HasValue)
                {
                    var descricaoCRM = string.Format("Celular excluído. Curriculo {0} informou o código de confirmação para o celular {1}!", objCurriculo.IdCurriculo, Helper.FormatarTelefone(numDDDCelular, numCelular));

                    CurriculoObservacao.SalvarCRM(descricaoCRM, new Curriculo(curriculo.Value), nomeProcessoPaiParaSalvarNoCRM, trans);
                }
            }
        }
        #endregion

        #region Validação E-Mail

        #region ValidacaoEmailGerarCodigo
        public string ValidacaoEmailGerarCodigo(SqlTransaction trans)
        {
            var token = Helper.ToBase64(Guid.NewGuid().ToString());

            var objCodigoConfirmacaoEmail = new CodigoConfirmacaoEmail
            {
                CodigoConfirmacao = token,
                DataCriacao = DateTime.Now,
                EmailConfirmacao = EmailPessoa
            };

            if (trans != null)
                objCodigoConfirmacaoEmail.Save(trans);
            else
                objCodigoConfirmacaoEmail.Save();


            return Helper.ToBase64(string.Format("userid={0}&codigo={1}", IdPessoaFisica, token));
        }
        #endregion ValidacaoEmailGerarCodigo

        #region ValidacaoEmailUtilizarCodigo
        public bool ValidacaoEmailUtilizarCodigo(string codigoValidacao)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var objCodigoConfirmacaoEmail = CodigoConfirmacaoEmail.CarregarPorCodigo(codigoValidacao, trans);

                        if (objCodigoConfirmacaoEmail == null)
                            throw new ArgumentNullException("Código inexistente!");

                        if (objCodigoConfirmacaoEmail.DataUtilizacao.HasValue)
                            return true;

                        objCodigoConfirmacaoEmail.DataUtilizacao = DateTime.Now;
                        objCodigoConfirmacaoEmail.Save(trans);

                        Curriculo objCurriculo;
                        if (Curriculo.CarregarPorPessoaFisica(trans, IdPessoaFisica, out objCurriculo))
                        {
                            ParametroCurriculo objParametroCurriculo;
                            if (!ParametroCurriculo.CarregarParametroPorCurriculo(Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, objCurriculo, out objParametroCurriculo, trans))
                            {
                                // mant?m as candidaturas gr?tis para os clientes do STC sem VIP
                                objParametroCurriculo = new ParametroCurriculo
                                {
                                    Curriculo = objCurriculo,
                                    Parametro = new Parametro((int)Enumeradores.Parametro.QuantidadeCandidaturaDegustacao),
                                    ValorParametro = Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, trans)
                                };

                                objParametroCurriculo.Save(trans);
                            }
                        }

                        EmailSituacaoConfirmacao = EmailSituacao.LoadObject(Convert.ToInt16(Enumeradores.EmailSituacao.Confirmado));
                        EmailSituacaoValidacao = EmailSituacao.LoadObject(Convert.ToInt16(Enumeradores.EmailSituacao.Validado));
                        FlagEmailConfirmado = true;
                        Save(trans);

                        trans.Commit();

                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion ValidacaoEmailUtilizarCodigo

        #endregion

        #region SalvarPessoaFisica
        /// <summary>
        ///     Salvar apenas o objeto Pessoa Fisica
        /// </summary>
        /// <param name="objPessoaFisica"></param>
        /// <returns></returns>
        public void SalvarPessoaFisica(PessoaFisica objPessoaFisica)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        objPessoaFisica.Save(trans);

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
        #endregion

        public static Func<int> PesquisarIdCidadeAcessor(int pessoaFisicaId)
        {
            var resultId = new int?();
            return () =>
            {
                if (resultId.HasValue)
                    return resultId.Value;

                resultId = CarregarCidadeId(pessoaFisicaId);
                return resultId.Value;
            };
        }

        public static int? CarregarCidadeId(int pessoaFisicaId)
        {
            const string sql = @"
            SELECT Idf_Cidade FROM BNE.TAB_Pessoa_Fisica
            WHERE Idf_Pessoa_Fisica = @p0";

            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@p0", pessoaFisicaId) {SqlDbType = SqlDbType.Int}
            };

            var res = DataAccessLayer.ExecuteScalar(CommandType.Text, sql, sqlParams);

            int realValue;
            if (Convert.IsDBNull(res) || res == null || !int.TryParse(res.ToString(), out realValue) || realValue <= 0)
            {
                return null;
            }
            return realValue;
        }

        #region UsuarioTanque
        public static DataTable UsuarioTanque(string strcpf)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal));
            parms[0].Value = Convert.ToDecimal(strcpf);

            var dt = new DataTable();
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpUsuarioTanque, parms))
                {
                    dt.Load(dr);
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }
        #endregion

        #region SetInstanceNotDipose
        internal static bool SetInstanceNotDipose(IDataReader dr, PessoaFisica objPessoaFisica)
        {
            try
            {
                objPessoaFisica._idPessoaFisica = Convert.ToInt32(dr["Idf_Pessoa_Fisica"]);
                objPessoaFisica._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
                objPessoaFisica._nomePessoa = Convert.ToString(dr["Nme_Pessoa"]);
                if (dr["Ape_Pessoa"] != DBNull.Value)
                    objPessoaFisica._apelidoPessoa = Convert.ToString(dr["Ape_Pessoa"]);
                if (dr["Idf_Sexo"] != DBNull.Value)
                    objPessoaFisica._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
                if (dr["Idf_Nacionalidade"] != DBNull.Value)
                    objPessoaFisica._nacionalidade = new Nacionalidade(Convert.ToInt32(dr["Idf_Nacionalidade"]));
                if (dr["Idf_Cidade"] != DBNull.Value)
                    objPessoaFisica._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
                if (dr["Dta_Chegada_Brasil"] != DBNull.Value)
                    objPessoaFisica._dataChegadaBrasil = Convert.ToDateTime(dr["Dta_Chegada_Brasil"]);
                objPessoaFisica._dataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]);
                if (dr["Nme_Mae"] != DBNull.Value)
                    objPessoaFisica._nomeMae = Convert.ToString(dr["Nme_Mae"]);
                if (dr["Nme_Pai"] != DBNull.Value)
                    objPessoaFisica._nomePai = Convert.ToString(dr["Nme_Pai"]);
                if (dr["Num_RG"] != DBNull.Value)
                    objPessoaFisica._numeroRG = Convert.ToString(dr["Num_RG"]);
                if (dr["Dta_Expedicao_RG"] != DBNull.Value)
                    objPessoaFisica._dataExpedicaoRG = Convert.ToDateTime(dr["Dta_Expedicao_RG"]);
                if (dr["Nme_Orgao_Emissor"] != DBNull.Value)
                    objPessoaFisica._nomeOrgaoEmissor = Convert.ToString(dr["Nme_Orgao_Emissor"]);
                if (dr["Sig_UF_Emissao_RG"] != DBNull.Value)
                    objPessoaFisica._siglaUFEmissaoRG = Convert.ToString(dr["Sig_UF_Emissao_RG"]);
                if (dr["Num_PIS"] != DBNull.Value)
                    objPessoaFisica._numeroPIS = Convert.ToString(dr["Num_PIS"]);
                if (dr["Num_CTPS"] != DBNull.Value)
                    objPessoaFisica._numeroCTPS = Convert.ToString(dr["Num_CTPS"]);
                if (dr["Des_Serie_CTPS"] != DBNull.Value)
                    objPessoaFisica._descricaoSerieCTPS = Convert.ToString(dr["Des_Serie_CTPS"]);
                if (dr["Sig_UF_CTPS"] != DBNull.Value)
                    objPessoaFisica._siglaUFCTPS = Convert.ToString(dr["Sig_UF_CTPS"]);
                if (dr["Idf_Raca"] != DBNull.Value)
                    objPessoaFisica._raca = new Raca(Convert.ToInt32(dr["Idf_Raca"]));
                if (dr["Idf_Deficiencia"] != DBNull.Value)
                    objPessoaFisica._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
                if (dr["Idf_Endereco"] != DBNull.Value)
                    objPessoaFisica._endereco = new Endereco(Convert.ToInt32(dr["Idf_Endereco"]));
                if (dr["Num_DDD_Telefone"] != DBNull.Value)
                    objPessoaFisica._numeroDDDTelefone = Convert.ToString(dr["Num_DDD_Telefone"]);
                if (dr["Num_Telefone"] != DBNull.Value)
                    objPessoaFisica._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);
                if (dr["Num_DDD_Celular"] != DBNull.Value)
                    objPessoaFisica._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
                if (dr["Num_Celular"] != DBNull.Value)
                    objPessoaFisica._numeroCelular = Convert.ToString(dr["Num_Celular"]);
                if (dr["Eml_Pessoa"] != DBNull.Value)
                    objPessoaFisica._emailPessoa = Convert.ToString(dr["Eml_Pessoa"]);
                if (dr["Flg_Possui_Dependentes"] != DBNull.Value)
                    objPessoaFisica._flagPossuiDependentes = Convert.ToBoolean(dr["Flg_Possui_Dependentes"]);
                objPessoaFisica._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                objPessoaFisica._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
                if (dr["Flg_Importado"] != DBNull.Value)
                    objPessoaFisica._flagImportado = Convert.ToBoolean(dr["Flg_Importado"]);
                if (dr["Idf_Escolaridade"] != DBNull.Value)
                    objPessoaFisica._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
                objPessoaFisica._nomePessoaPesquisa = Convert.ToString(dr["Nme_Pessoa_Pesquisa"]);
                if (dr["Idf_Estado_Civil"] != DBNull.Value)
                    objPessoaFisica._estadoCivil = new EstadoCivil(Convert.ToInt32(dr["Idf_Estado_Civil"]));
                if (dr["Sig_Estado"] != DBNull.Value)
                    objPessoaFisica._siglaEstado = Convert.ToString(dr["Sig_Estado"]);
                if (dr["Flg_Inativo"] != DBNull.Value)
                    objPessoaFisica._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                if (dr["Des_IP"] != DBNull.Value)
                    objPessoaFisica._descricaoIP = Convert.ToString(dr["Des_IP"]);
                if (dr["Idf_Operadora_Celular"] != DBNull.Value)
                    objPessoaFisica._operadoraCelular = new OperadoraCelular(Convert.ToInt32(dr["Idf_Operadora_Celular"]));
                if (dr["Idf_Email_Situacao_Confirmacao"] != DBNull.Value)
                    objPessoaFisica._emailSituacaoConfirmacao = new EmailSituacao(Convert.ToInt32(dr["Idf_Email_Situacao_Confirmacao"]));
                if (dr["Idf_Email_Situacao_Validacao"] != DBNull.Value)
                    objPessoaFisica._emailSituacaoValidacao = new EmailSituacao(Convert.ToInt32(dr["Idf_Email_Situacao_Validacao"]));
                if (dr["Idf_Email_Situacao_Bloqueio"] != DBNull.Value)
                    objPessoaFisica._emailSituacaoBloqueio = new EmailSituacao(Convert.ToInt32(dr["Idf_Email_Situacao_Bloqueio"]));
                if (dr["Flg_Celular_Confirmado"] != DBNull.Value)
                    objPessoaFisica._flagCelularConfirmado = Convert.ToBoolean(dr["Flg_Celular_Confirmado"]);
                if (dr["Flg_Email_Confirmado"] != DBNull.Value)
                    objPessoaFisica._flagEmailConfirmado = Convert.ToBoolean(dr["Flg_Email_Confirmado"]);

                objPessoaFisica._persisted = true;
                objPessoaFisica._modified = false;

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Enviar codigo de verificação do celular
        public bool EnviarCodigoAtivacaoCelular(string ddd, string celular)
        {
            try
            {
                var validaCel = ValidaCelular.Carregar(IdPessoaFisica, ddd, celular);

                Curriculo cv;
                Curriculo.CarregarPorPessoaFisica(IdPessoaFisica, out cv);
                UsuarioFilialPerfil ufp;
                UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(this, out ufp);

                var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Num_DDD_Celular", SqlDbType = SqlDbType.Int, Value = ddd},
                new SqlParameter {ParameterName = "@Num_Celular", SqlDbType = SqlDbType.Int, Value = celular},
                new SqlParameter {ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Value = IdPessoaFisica}
            };

                if (validaCel != null)
                {
                    //caso o celular já tenha recebido alguma mensagem para confirmação
                    //Se o número já tiver recebido uma mensagem hoje não reenvia a MSG.
                    if (validaCel.DataEnvio > DateTime.Today)
                    {
                        //Não envia SMS apenas define o número 
                        if (DataAccessLayer.ExecuteNonQuery(CommandType.Text, DESCONFIRMAR_NUMERO_CELULAR, parms) > 0)
                            return true;
                    }
                    else //Caso tenha se passado mais de um dia tenta enviar novamente 
                    {
                        if (DataAccessLayer.ExecuteNonQuery(CommandType.Text, DESCONFIRMAR_NUMERO_CELULAR, parms) > 0)
                        {
                            var textoMensagem = new Mensagem.CampanhaTanque().GetTextoCampanha(Mensagem.Enumeradores.CampanhaTanque.ConfirmacaoCelular);
                            if (!string.IsNullOrWhiteSpace(textoMensagem?.mensagem))
                            {
                                var i = MensagemCS.EnviaSMSTanque(cv, this, ufp, ufp, textoMensagem.mensagem, ddd, celular, UsuarioSistemaTanque.ValidacaoCelCV, null, textoMensagem.id);
                                if (i > 0)
                                {
                                    var valida = new ValidaCelular { IdPessoaFisica = IdPessoaFisica, DataEnvio = DateTime.Now, DDD = ddd, Numero = celular };
                                    valida.Save();
                                    return true;
                                }
                            }
                        }
                    }
                }
                else //Caso o número de celular nunca tenha recebido uma mensagem para confirmação
                {
                    if (DataAccessLayer.ExecuteNonQuery(CommandType.Text, DESCONFIRMAR_NUMERO_CELULAR, parms) > 0)
                    {
                        var textoMensagem = new Mensagem.CampanhaTanque().GetTextoCampanha(Mensagem.Enumeradores.CampanhaTanque.ConfirmacaoCelular);
                        if (!string.IsNullOrWhiteSpace(textoMensagem?.mensagem))
                        {
                            var i = MensagemCS.EnviaSMSTanque(cv, this, ufp, ufp, textoMensagem.mensagem, ddd, celular, UsuarioSistemaTanque.ValidacaoCelCV, null, textoMensagem.id);
                            if (i > 0)
                            {
                                var valida = new ValidaCelular { IdPessoaFisica = IdPessoaFisica, DataEnvio = DateTime.Now, DDD = ddd, Numero = celular };
                                valida.Save();
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "EnviarCodigoAtivacaoCelular");
                return false;
            }
        }
        #endregion

        #region RecuperarPessoaFisicaDePagamento
        public static PessoaFisica RecuperarPessoaFisicaDePagamento(Pagamento objPagamento)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Pagamento", SqlDbType = SqlDbType.Int, Size = 6, Value = objPagamento.IdPagamento}
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarInformacoesDePessoaFisicaDePagamento, parms))
            {
                var objPessoaFisica = new PessoaFisica();
                if (SetInstance(dr, objPessoaFisica))
                    return objPessoaFisica;
            }
            return null;
        }
        #endregion

        #region RecuperarUltimaFilialCadastrada
        public int RecuperarUltimaFilialCadastrada()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Num_CPF", SqlDbType = SqlDbType.Decimal, Size = 11, Value = this.CPF}
            };
            var retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, SprecuperarUltimaEmpresaCadastrada, parms);

            if (retorno != DBNull.Value)
            {
                return Convert.ToInt32(retorno);
            }

            return 0;
        }
        #endregion

        #region RecuperarDataNascimento
        /// <summary>
        ///     Recupera o id da sessão relacionada ao usuário
        /// </summary>
        /// <returns>String identificadora da sessão</returns>
        public DateTime RecuperarDataNascimento()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size =4, Value = this._idPessoaFisica}
            };

            return Convert.ToDateTime(DataAccessLayer.ExecuteScalar(CommandType.Text, SpRecuperarDataNascimento, parms));
        }
        #endregion


        #region [AtualizandoCadastroExlusaoLogicaParaAtivo]
        /// <summary>
        /// Inativa os registros antigos, e apaga/exclui os registros que não podem ser inativadas. exemplo: idf_raca da pessoa fisica.
        /// </summary>
        public void AtualizandoCadastroExlusaoLogicaParaAtivo(PessoaFisica objPessoaFisica, Curriculo objCurriculo,
             PessoaFisicaComplemento objPessoaFisicaComplemento, SqlTransaction trans)
        {
            //Invativar Endereco
            objPessoaFisica.Endereco = null;
            //FuncaoPretendida.InativaFuncaoPretendidaPorCurriculo(objCurriculo.IdCurriculo, trans);
            //Inativar formações
            Formacao.InativarFormacaoPorPessoaFisica(this.IdPessoaFisica, trans);
            //Inativar Experiencias
            ExperienciaProfissional.InativarExperienciaPorPessoaFisica(this.IdPessoaFisica, trans);
            //Inativar idiomas
            PessoafisicaIdioma.InativarTodosIdiomasPessoaFisica(this.IdPessoaFisica, trans);
            //deletar disponibilidade.
            CurriculoDisponibilidade.DeletePorCurriculo(objCurriculo.IdCurriculo, trans);
            //deletar contatos
            Contato.DeletePorPessoaFisica(this.IdPessoaFisica, trans);
            //Deleter disponibilidade
            CurriculoDisponibilidadeCidade.DeleteDisponibilidadePorCurriculo(objCurriculo.IdCurriculo, trans);
            //Deletar veiculos
            PessoaFisicaVeiculo.DeleteVeiculoPorPessoaFisica(this.IdPessoaFisica, trans);
            //Apagar campos da tabela tab_Pessoa_fisica
            #region [Apagar Campos PF]
            objPessoaFisica.Raca = null;
            objPessoaFisica.NumeroRG = null;
            objPessoaFisica.NomeOrgaoEmissor = null;
            objPessoaFisica.DataExpedicaoRG = null;
            objPessoaFisica.NumeroTelefone = null;
            objPessoaFisica.NumeroDDDTelefone = null;
            objPessoaFisica.ApelidoPessoa = null;
            objPessoaFisica.Nacionalidade = null;
            objPessoaFisica.DataChegadaBrasil = null;
            objPessoaFisica.NomeMae = null;
            objPessoaFisica.NomePai = null;
            //verificar campos
            objPessoaFisica.Deficiencia = null;
            objPessoaFisica.FlagPossuiDependentes = null;
            objPessoaFisica.EstadoCivil = null;
            objPessoaFisica.EmailSituacaoValidacao = null;
            objPessoaFisica.EmailSituacaoBloqueio = null;
            objPessoaFisica.FlagEmailConfirmado = false;
            objPessoaFisica.FlagCelularConfirmado = false;

            #endregion

            //Apagar Campos da tabela TAB_Pessoa_Fisica_Complemento
            #region [Apagar Campos PFC]
            objPessoaFisicaComplemento.AnoVeiculo = null;
            //apagar o arquivo em anexo.
            objPessoaFisicaComplemento.ArquivoAnexo = null;
            objPessoaFisicaComplemento.CategoriaHabilitacao = null;
            objPessoaFisicaComplemento.NumeroHabilitacao = null;
            objPessoaFisicaComplemento.DataValidadeHabilitacao = null;
            objPessoaFisicaComplemento.NumeroTituloEleitoral = null;
            objPessoaFisicaComplemento.NumeroZonaEleitoral = null;
            objPessoaFisicaComplemento.NumeroSecaoEleitoral = null;
            objPessoaFisicaComplemento.NumeroRegistroConselho = null;
            objPessoaFisicaComplemento.IdConselho = null;
            objPessoaFisicaComplemento.IdTipoConselho = null;
            objPessoaFisicaComplemento.DataValidadeConselho = null;
            objPessoaFisicaComplemento.FlagVeiculoProprio = null;
            objPessoaFisicaComplemento.AnoVeiculo = null;
            objPessoaFisicaComplemento.DescricaoPlacaVeiculo = null;
            objPessoaFisicaComplemento.NumeroRenavam = null;
            objPessoaFisicaComplemento.NumeroDocReservista = null;
            objPessoaFisicaComplemento.FlagAposentado = null;
            objPessoaFisicaComplemento.DataAposentadoria = null;
            objPessoaFisicaComplemento.TipoSanguineo = null;
            objPessoaFisicaComplemento.FlagDoador = null;
            objPessoaFisicaComplemento.FlagSeguroVeiculo = null;
            objPessoaFisicaComplemento.DataVencimentoSeguro = null;
            objPessoaFisicaComplemento.IdMotivoAposentadoria = null;
            objPessoaFisicaComplemento.IdCID = null;
            objPessoaFisicaComplemento.NumeroCEI = null;
            objPessoaFisicaComplemento.NumeroAltura = null;
            objPessoaFisicaComplemento.NumeroPeso = null;
            objPessoaFisicaComplemento.TipoVeiculo = null;
            objPessoaFisicaComplemento.NumeroRegistroHabilitacao = null;
            objPessoaFisicaComplemento.DescricaoConhecimento = null;
            objPessoaFisicaComplemento.DescricaoComplementoDeficiencia = null;
            objPessoaFisicaComplemento.FlagFilhos = null;
            objPessoaFisicaComplemento.FlagViagem = null;
            objPessoaFisicaComplemento.FlagMudanca = null;
            objPessoaFisicaComplemento.NomeAnexo = null;

            //objPessoaFisicaComplemento.Save(trans);
            #endregion

            objCurriculo.ObservacaoCurriculo = string.Empty;
            objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.Publicado);
            objCurriculo.ValorUltimoSalario = null;
            objCurriculo.FlagInativo = false;
        }
        #endregion

        #endregion

        #region [ Migration Mapping ]
        /// <summary>
        /// Médodos e atributos auxiliares à migração de dados para o novo
        /// domínio.
        /// </summary>
        public int MigrationId
        {
            set
            {
                this._idPessoaFisica = value;
            }
            get { return this._idPessoaFisica; }
        }

        PessoaFisicaComplemento _migrationPessoaFisicaComplemento;
        public PessoaFisicaComplemento MigrationPessoaFisicaComplemento
        {
            set
            {
                this._migrationPessoaFisicaComplemento = value;
                this._migrationPessoaFisicaComplemento.PessoaFisica = this;
            }
            get
            {
                return this._migrationPessoaFisicaComplemento;
            }
        }

        private List<ExperienciaProfissional> _migrationExperienciasProfissionais;
        public List<ExperienciaProfissional> MigrationExperienciasProfissionais
        {
            set
            {
                this._migrationExperienciasProfissionais = value;
            }
            get
            {
                return this._migrationExperienciasProfissionais;
            }
        }
        #endregion
    }
}