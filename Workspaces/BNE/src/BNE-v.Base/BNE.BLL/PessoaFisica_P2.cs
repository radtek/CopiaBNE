//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using BNE.BLL.Custom;
using BNE.EL;

namespace BNE.BLL
{
    public partial class PessoaFisica : ICloneable // Tabela: TAB_Pessoa_Fisica
    {

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Propriedades

        #region NumeroCPF
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public string NumeroCPF
        {
            get
            {
                if (this._numeroCPF <= 0)
                    return string.Empty;
                else
                    return this._numeroCPF.ToString().PadLeft(11, '0').Insert(3, ".").Insert(7, ".").Insert(11, "-");
            }
            set
            {
                Decimal.TryParse(value.Replace(".", "").Replace("-", ""), out this._numeroCPF);
                this._modified = true;
            }
        }
        #endregion

        #region CPF
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public decimal CPF
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

        #region PrimeiroNome
        /// <summary>
        /// Primeiro nome da pessoa
        /// </summary>
        public string PrimeiroNome
        {
            get
            {
                if (string.IsNullOrEmpty(this._nomePessoa))
                    this._nomePessoa = this.RecuperarNomePessoa();

                return PessoaFisica.RetornarPrimeiroNome(this._nomePessoa);
            }
        }
        #endregion

        #region NomeCompleto
        /// <summary>
        /// Nome completo da pessoa. 
        /// Propriedade para evitar dar um load object na TAB_Pessoa_Fisica apenas para recuperar o nome.
        /// </summary>
        public string NomeCompleto
        {
            get
            {
                if (string.IsNullOrEmpty(this._nomePessoa))
                    this._nomePessoa = this.RecuperarNomePessoa();

                return this._nomePessoa;
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
                if (string.IsNullOrEmpty(this._emailPessoa))
                    this._emailPessoa = this.RecuperarEmailPessoa();

                return this._emailPessoa;
            }
            set
            {
                //Se passar nulo ou sem valor o campo não está confirmado ou se o valor for diferente, também muda a flag
                if (string.IsNullOrWhiteSpace(value) || (!string.IsNullOrWhiteSpace(this._emailPessoa) && !this._emailPessoa.Trim().Equals(value.Trim())))
                {
                    _flagEmailConfirmado = false;
                }

                this._emailPessoa = value;
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
                //Se passar nulo ou sem valor o campo não está confirmado ou se o valor for diferente, também muda a flag
                if (string.IsNullOrWhiteSpace(value) || (!string.IsNullOrWhiteSpace(this._numeroCelular) && !this._numeroCelular.Trim().Equals(value.Trim())))
                {
                    _flagCelularConfirmado = false;
                }

                this._numeroCelular = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas

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

        #region SPPRIMEIROUSUARIOMASTERFILIAL
        private const string SPPRIMEIROUSUARIOMASTERFILIAL = @"SELECT TOP 1
                                                                PF.*
                                                        FROM    TAB_Pessoa_Fisica PF
                                                                JOIN TAB_Usuario_Filial_Perfil UFP ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
                                                        WHERE   Idf_Filial = @Idf_Filial
                                                                AND UFP.Flg_Inativo = 0
                                                                AND UFP.Idf_Perfil = @Idf_Perfil
                                                        ORDER BY UFP.Dta_Cadastro DESC";
        #endregion

        #region Spexistepessoafisica
        private const string Spexistepessoafisica = @"  
        SELECT  Idf_Pessoa_Fisica
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
        SELECT Idf_Pessoa_Fisica, LTRIM(RTRIM(Num_DDD_Celular)) AS DDD, LTRIM(RTRIM(Num_Celular)) AS Numero, Idf_Operadora_Celular
        FROM BNE.TAB_Pessoa_Fisica
        WHERE 1 = 1
        AND   Flg_Inativo = 0
        AND   Num_DDD_Celular   IS NOT NULL
        AND   Num_Celular       IS NOT NULL
		AND   Idf_Pessoa_Fisica BETWEEN @Idf_Pessoa_Fisica_Inicio AND @Idf_Pessoa_Fisica_Fim
";
        #endregion

        #region SpAtualizarOperadoraCelular
        private const string SpAtualizarOperadoraCelular = @"
        UPDATE BNE.TAB_Pessoa_Fisica
        SET Idf_Operadora_Celular = {0}
        WHERE Idf_Pessoa_Fisica IN ({1})
";
        #endregion

        #region SpCarregarPorIdUsuarioFilialPerfil

        private const string SpCarregarPorIdUsuarioFilialPerfil = @"
        select pf.Nme_Pessoa
	        , pf.Num_RG
	        , pf.Num_CPF
            , uf.Eml_Comercial
        from bne.tab_usuario_filial_perfil ufp with (nolock)
	        join bne.tab_pessoa_fisica pf with (nolock) on ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
            join bne.bne_usuario_filial uf with (nolock) on ufp.Idf_Usuario_Filial_Perfil = uf.Idf_Usuario_Filial_Perfil
        where ufp.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
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
	        , pf.EMl_Pessoa
	        , pf.Num_DDD_Celular
	        , pf.Num_Celular
	        , pf.Nme_Pessoa
	        , pf.Num_CPF
			, trans_num_banco.Idf_Banco
			, fl_gestora.Nme_Fantasia as Filial_Gestora
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
	        join bne.TAB_Endereco en with(nolock) on pf.Idf_Endereco = en.Idf_Endereco
	        join plataforma.TAB_Cidade ci with(nolock) on en.Idf_Cidade = ci.Idf_Cidade
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

        #endregion

        #region Métodos

        //TODO: Rever estes métodos CarregarPorCPF, principalmente com a regitirada dos .-
        #region CarregarPorCPF
        /// <summary>
        /// Método responsável por carregar uma instância de PessoaFisica pelo parâmetro CPF.
        /// </summary>
        /// <param name="numCpf">Valor de CPF</param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static int CarregarIdPorCPF(decimal numCpf)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 11));
            parms[0].Value = numCpf;

            var res = DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTIDPORCPF, parms);

            int pfId;
            if (Convert.IsDBNull(res) || res == null || !Int32.TryParse(res.ToString(), out pfId))
                return 0;

            return pfId;
        }

        /// <summary>
        /// Método responsável por carregar uma instância de PessoaFisica pelo parâmetro CPF.
        /// </summary>
        /// <param name="numCpf">Valor de CPF</param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PessoaFisica CarregarPorCPF(decimal numCpf)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 11));
            parms[0].Value = numCpf;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORCPF, parms))
            {
                PessoaFisica objPessoaFisica = new PessoaFisica();
                if (SetInstance(dr, objPessoaFisica))
                    return objPessoaFisica;

                if (!dr.IsClosed)
                    dr.Close();
            }
            throw (new RecordNotFoundException(typeof(PessoaFisica)));
        }
        /// <summary>
        /// Método responsável por carregar uma instância de PessoaFisica pelo parâmetro CPF.
        /// </summary>
        /// <param name="numCpf">Valor de CPF</param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PessoaFisica CarregarPorCPF(string numCpf)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 11));
            parms[0].Value = numCpf.Replace(".", String.Empty).Replace("-", String.Empty);

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORCPF, parms))
            {
                PessoaFisica objPessoaFisica = new PessoaFisica();
                if (SetInstance(dr, objPessoaFisica))
                    return objPessoaFisica;

                if (!dr.IsClosed)
                    dr.Close();
            }
            throw (new RecordNotFoundException(typeof(PessoaFisica)));
        }
        /// <summary>   
        /// Método responsável por carregar uma instância de PessoaFisica pelo parâmetro CPF.
        /// </summary>
        /// <param name="numCpf">Valor de CPF</param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorCPF(decimal numCpf, out PessoaFisica objPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 11));
            parms[0].Value = numCpf;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORCPF, parms))
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
        /// <summary>   
        /// Método responsável por carregar uma instância de PessoaFisica pelo parâmetro CPF.
        /// </summary>
        /// <param name="numCpf">Valor de CPF</param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorCPF(string numCpf, out PessoaFisica objPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 11));
            parms[0].Value = numCpf.Replace(".", String.Empty).Replace("-", String.Empty);

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORCPF, parms))
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

        #region CarregarPorNome
        /// <summary>
        /// Método responsável por carregar uma instância de PessoaFisica pelo nome da pessoa
        /// </summary>
        /// <param name="strNomePessoa">Nome da pessoa</param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PessoaFisica CarregarPorNome(String strNomePessoa)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 100));
            parms[0].Value = strNomePessoa;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORNOME, parms))
            {
                PessoaFisica objPessoaFisica = new PessoaFisica();
                if (SetInstance(dr, objPessoaFisica))
                    return objPessoaFisica;

                if (!dr.IsClosed)
                    dr.Close();
            }
            throw (new RecordNotFoundException(typeof(PessoaFisica)));
        }
        /// <summary>   
        /// Método responsável por carregar uma instância de PessoaFisica pelo nome da pessoa fisica
        /// </summary>
        /// <param name="strNomePessoa">Nome da Pessoa Física</param>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorNome(String strNomePessoa, out PessoaFisica objPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
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
        /// Método responsável por carregar o código identificador de uma pessoa física (Idf_Pessoa_Fisica) através do código identificador do currículo
        /// </summary>
        /// <param name="objCurriculo">Curriculo</param>
        /// <returns></returns>
        public static int RecuperarIdPorCurriculo(Curriculo objCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectidfpessoafisicaporidfcurriculo, parms));
        }

        /// <summary>
        /// Método responsável por carregar a escolaridade de uma pessoa física (Idf_Escolaridade) através do código identificador do currículo
        /// </summary>
        /// <param name="objCurriculoId">Curriculo ID</param>
        /// <returns></returns>
        private static int? RecuperarEscolaridadePorCurriculoId(int objCurriculoId)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculoId }
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
            if (this.Escolaridade == null)
            {
                if (this.IdPessoaFisica <= 0)
                    return false;

                if (!this.CompleteObject())
                    return false;

                if (this.Escolaridade == null)
                    return false;

                return ConsideraEstudantePorEscolaridadeId(this.Escolaridade.IdEscolaridade);
            }

            return ConsideraEstudantePorEscolaridadeId(this.Escolaridade.IdEscolaridade);
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
        /// Retorna uma list com os ids das experiencias profissionais da pessoa fisica
        /// </summary>
        /// <returns></returns>
        public List<int> RecuperarExperienciaProfissional(SqlTransaction trans)
        {
            List<int> list = new List<int>();
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Decimal, 11));
            parms[0].Value = this._idPessoaFisica;

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
        /// Retorna a ultima experiencia profissionais da pessoa fisica
        /// </summary>
        /// <returns></returns>
        public List<ExperienciaProfissional> RecuperarUltimaExperienciaProfissional(SqlTransaction trans, int numeroExperiencia)
        {
            List<ExperienciaProfissional> list = new List<ExperienciaProfissional>();
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Decimal, 11));
            parms.Add(new SqlParameter("@Numero_Experiencias", SqlDbType.Int, 4));

            parms[0].Value = this._idPessoaFisica;
            parms[1].Value = numeroExperiencia;

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTULTIMAEXPERIENCIA, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTULTIMAEXPERIENCIA, parms);

            while (dr.Read())
            {
                ExperienciaProfissional expe = ExperienciaProfissional.LoadObject(Convert.ToInt32(dr[0]));
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
        /// Método utilizado para excluir todas as instancias de Redes Sociais daquela pessoa fisica
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public void DeleteRedesSociais(SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));

            parms[0].Value = this._idPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETEREDESSOCIAIS, parms);
        }
        #endregion

        #region SalvarIntegracao
        public void SalvarIntegracao(DateTime dtaAlteracao)
        {
            if (!this._persisted)
                InsertIntegracao(dtaAlteracao);
            else
                UpdateIntegracao(dtaAlteracao);
        }
        #endregion

        #region InsertIntegracao
        public void InsertIntegracao(DateTime dtaAlteracao)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Criar primeiramente um endereço para a PessoaFisica
                        if (this._endereco != null)
                            this._endereco.Save(trans);

                        List<SqlParameter> parms = GetParameters();
                        SetParameters(parms);

                        //sincroniza a Data de Alteração do Registro para evitar trashing de sincronismo.
                        parms.Find(ParametroDataAlteracao).Value = dtaAlteracao;

                        SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
                        this._idPessoaFisica = Convert.ToInt32(cmd.Parameters["@Idf_Pessoa_Fisica"].Value);
                        cmd.Parameters.Clear();
                        this._persisted = true;
                        this._modified = false;

                        #region Cria o registro de formação para a Pessoa Física
                        if (this._escolaridade != null)
                        {
                            Formacao objFormacao = new Formacao();
                            objFormacao.PessoaFisica = this;
                            objFormacao.Escolaridade = this._escolaridade;
                            objFormacao.FlagNacional = true;
                            objFormacao.FlagInativo = false;
                            objFormacao.Save(trans);
                        }
                        #endregion

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

        #region UpdateIntegracao
        public void UpdateIntegracao(DateTime dtaAlteracao)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {

                        //Sempre atualiza o endereço
                        if (this._endereco != null)
                            this._endereco.Save(trans);

                        #region Atualizar Usuario
                        //Se A data de Nassimento da Pessoa Física foi atualizada, então atualizar a senha do Usuário
                        PessoaFisica objPessoaFisica;
                        if (PessoaFisica.CarregarPorCPF(this.CPF, out objPessoaFisica)
                            && objPessoaFisica.DataNascimento != this.DataNascimento)
                        {
                            UsuarioFilialPerfil objUsuarioFilialPerfil;
                            if (UsuarioFilialPerfil.CarregarPorPessoaFisica(this.IdPessoaFisica, out objUsuarioFilialPerfil))
                            {
                                objUsuarioFilialPerfil.SenhaUsuarioFilialPerfil = this.DataNascimento.ToString("ddMMyyyy");
                                objUsuarioFilialPerfil.Save(trans);
                            }
                        }
                        #endregion

                        #region Criar uma nova formação para a Pessoa Física
                        if (this._escolaridade != null)
                        {
                            Formacao objFormacao;
                            if (!Formacao.CarregarPorPessoaFisicaEscolaridade(trans, this._idPessoaFisica, this._escolaridade.IdEscolaridade, out objFormacao))
                            {
                                objFormacao = new Formacao();
                                objFormacao.PessoaFisica = this;
                                objFormacao.Escolaridade = this._escolaridade;
                                objFormacao.FlagNacional = true;
                                objFormacao.FlagInativo = false;
                                objFormacao.Save(trans);
                            }
                        }
                        #endregion

                        List<SqlParameter> parms = GetParameters();
                        SetParameters(parms);

                        //sincroniza a Data de Alteração do Registro para evitar trashing de sincronismo.
                        parms.Find(ParametroDataAlteracao).Value = dtaAlteracao;

                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
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
        /// Verifica se a pessoa fisica informada é usuário interno. Se for retorna o IdUsuarioFilialPerfil e o IdPerfil do mesmo.
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <param name="idUsuarioFilialPerfil">out</param>
        /// <param name="idPerfil">out</param>
        /// <returns>bool usuarioInterno</returns>
        public static bool VerificaPessoaFisicaUsuarioInterno(int idPessoaFisica, out int idUsuarioFilialPerfil, out int idPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Perfil", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisica;
            parms[1].Value = BNE.BLL.Enumeradores.TipoPerfil.Interno.GetHashCode();

            bool usuarioInterno = false;
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

        #region CarregarTop1PessoaFisicaPerfilMasterPorFilial

        public static PessoaFisica CarregarTop1PessoaFisicaPerfilMasterPorFilial(int idFilial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));

            parms[0].Value = idFilial;
            parms[1].Value = BNE.BLL.Enumeradores.Perfil.AcessoEmpresaMaster.GetHashCode();

            PessoaFisica objPessoaFisica = null;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPPRIMEIROUSUARIOMASTERFILIAL, parms))
            {
                if (dr.Read())
                    objPessoaFisica = PessoaFisica.LoadObject(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));

                if (!dr.IsClosed)
                    dr.Dispose();

                dr.Dispose();
            }

            return objPessoaFisica;
        }

        #endregion

        #region RetornarIdade
        /// <summary>
        /// Calcula a idade da Pessoa
        /// </summary>
        /// <returns></returns>
        public int RetornarIdade()
        {
            return RetornarIdade(this._dataNascimento);
        }
        public static int RetornarIdade(DateTime data)
        {
            var idade = DateTime.Now.Year - data.Year;
            if (DateTime.Now.Month < data.Month || (DateTime.Now.Month.Equals(data.Month) && DateTime.Now.Day < data.Day))
                idade--;
            return idade;
        }
        #endregion

        #region RetornarPrimeiroNome
        /// <summary>
        /// Método utilizado para retornar o primeiro nome da pessoa.
        /// </summary>
        /// <param name="nomeCompleto">String nome completo</param>
        /// <returns>Primeiro nome</returns>
        public static string RetornarPrimeiroNome(string nomeCompleto)
        {
            if (nomeCompleto.IndexOf(' ') != -1)
                return nomeCompleto.Substring(0, nomeCompleto.IndexOf(' '));

            return nomeCompleto;
        }
        #endregion

        #region RecuperarNomePessoa
        /// <summary>
        /// Método que retorna o Nome(Nme_Pessoa)da pessoa fisica
        /// </summary>
        /// <returns>Nme_Pessoa</returns>
        public string RecuperarNomePessoa()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPessoaFisica }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectnomepessoa, parms));
        }

        public KeyValuePair<string, string> RecuperarNomeECelularPessoa()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPessoaFisica }
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

            string nome = Convert.IsDBNull(objDataTable.Rows[0][0]) ? string.Empty : objDataTable.Rows[0][0].ToString();
            string ddd = Convert.IsDBNull(objDataTable.Rows[0][1]) ? string.Empty : objDataTable.Rows[0][1].ToString();
            string celular = Convert.IsDBNull(objDataTable.Rows[0][2]) ? string.Empty : objDataTable.Rows[0][2].ToString();

            return new KeyValuePair<string, string>(nome, ddd + celular);
        }
        #endregion

        #region RecuperarEmailPessoa
        /// <summary>
        /// Método que retorna o Email(Eml_Pessoa)da pessoa fisica
        /// </summary>
        /// <returns>Eml_Pessoa</returns>
        private string RecuperarEmailPessoa()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPessoaFisica }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectemailpessoa, parms));
        }
        #endregion

        #region ExistePessoaFisica
        /// <summary>
        /// Verifica se existe pessoa fisica cadastrada com o cpf informado
        /// </summary>
        /// <param name="numeroCPF"></param>
        /// <param name="idPessoaFisica"></param>
        /// <returns></returns>
        public static bool ExistePessoaFisica(decimal numeroCPF, out int idPessoaFisica)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Num_CPF", SqlDbType = SqlDbType.Decimal, Size = 11, Value = numeroCPF } 
				};
            Object retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, Spexistepessoafisica, parms);

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
            numeroCPF = new Regex("[.\\/-]").Replace(numeroCPF, "");
            return ExistePessoaFisica(Convert.ToDecimal(numeroCPF), out idPessoaFisica);
        }
        #endregion

        #region ValidarCPFDataNascimento
        /// <summary>
        /// Verifica se o CPF e a Data de Nascimento informados batem com os dados que estão salvos no banco
        /// </summary>
        /// <param name="numeroCPF"></param>
        /// <param name="dataNascimento"></param>
        /// <returns></returns>
        public static bool ValidarCPFDataNascimento(decimal numeroCPF, DateTime dataNascimento)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Num_CPF", SqlDbType = SqlDbType.Decimal, Size = 11, Value = numeroCPF } ,
					new SqlParameter { ParameterName = "@Dta_Nascimento", SqlDbType = SqlDbType.DateTime, Value = dataNascimento } 
				};
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spverificarcpfdatanascimentopessoafisica, parms)) > 0;
        }
        public static bool ValidarCPFDataNascimento(string numeroCPF, DateTime dataNascimento)
        {
            numeroCPF = new Regex("[.\\/-]").Replace(numeroCPF, "");
            return ValidarCPFDataNascimento(Convert.ToDecimal(numeroCPF), dataNascimento);
        }
        #endregion

        #region RecuperarSessaoAtual
        /// <summary>
        /// Recupera o id da sessão relacionada ao usuário
        /// </summary>
        /// <returns>String identificadora da sessão</returns>
        public string RecuperarSessaoAtual()
        {
            var parms = new List<SqlParameter> {
                new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4)};

            parms[0].Value = this.IdPessoaFisica;

            return DataAccessLayer.ExecuteScalar(CommandType.Text, SpRecuperarSessao, parms).ToString();
        }
        #endregion

        #region AtualizarDataInteracaoUsuario
        /// <summary>
        /// Método responsável por atualizar a interação do usuário
        /// </summary>
        public void AtualizarDataInteracaoUsuario()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4)
                };

            parms[0].Value = this.IdPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpAtualizarInteracao, parms);
        }
        #endregion

        #region ZerarDataInteracaoUsuario
        /// <summary>
        /// Método responsável por zerar a data de interação do usuário
        /// </summary>
        public void ZerarDataInteracaoUsuario()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4)
                };

            parms[0].Value = this.IdPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpZerarInteracao, parms);
        }
        #endregion

        #region ZerarDataInteracaoTodosUsuarios
        /// <summary>
        /// Método responsável por zerar a data de interação do usuário
        /// </summary>
        public static void ZerarDataInteracaoTodosUsuarios()
        {
            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpZerarInteracaoTodosUsuarios, null);
        }
        #endregion

        #region CarregarOperadoraCelular
        public static bool CarregarOperadoraCelular(int idPessoaFisicaInicial, int idPessoaFisicaFinal, out DataTable objDataTable)
        {
            objDataTable = new DataTable();

            List<SqlParameter> parms = new List<SqlParameter>()
            {
                new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica_Inicio", SqlDbType = SqlDbType.Int, Size = 4, Value = idPessoaFisicaInicial },
                new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica_Fim", SqlDbType = SqlDbType.Int, Size = 4, Value = idPessoaFisicaFinal }
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectOperadoraCelular, parms))
            {
                objDataTable.Load(dr);
            }

            return objDataTable.Rows.Count > 0;
        }
        #endregion

        #region AtualizarOperadoraCelular
        public static void AtualizarOperadoraCelular(int idOperadoraCelular, params int[] idsPessoaFisica)
        {
            string listaIds = String.Join(",", idsPessoaFisica.Select(id => id.ToString()));

            string sql =
                String.Format(SpAtualizarOperadoraCelular,
                    idOperadoraCelular == 0 ? "NULL" : idOperadoraCelular.ToString(),
                    listaIds);

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, sql, null);
        }
        #endregion

        #region CarregarPorIdUsuarioFilialPerfil
        public static void CarregarPorIdUsuarioFilialPerfil(int idUsuarioFilialPerfil, out string nomePessoa, out string numRg, out decimal numCPF, out string email)
        {
            nomePessoa = string.Empty;
            numCPF = 0;
            numRg = string.Empty;
            email = string.Empty;

            var parms = new List<SqlParameter> 
            {
                new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil}
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpCarregarPorIdUsuarioFilialPerfil, parms))
            {
                if (dr.Read())
                {
                    nomePessoa = dr["Nme_Pessoa"].ToString();
                    numCPF = Convert.ToDecimal(dr["Num_CPF"].ToString());
                    numRg = dr["Num_RG"].ToString();
                    email = dr["Eml_Comercial"].ToString();
                }
            }
        }
        #endregion

        #region VerificarEmail
        /// <summary>
        /// Verifica se é permitido enviar e-mail para o endereço informado
        /// </summary>
        /// <param name="emailDestinatario">E-mail do destinatário</param>
        /// <returns>true se for permitido, false se não for permitido</returns>
        public static bool VerificarEmail(string emailDestinatario)
        {
            //Tratando string para o full-text
            //emailDestinatario = "\"" + emailDestinatario + "\"";

            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Eml_Pessoa", SqlDbType = SqlDbType.NVarChar, Size = 100, Value = emailDestinatario }
				};

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpVerificarEmail, parms))
            {
                return !dr.Read();  // se não voltar nenhuma linha, está correto
            }
        }
        #endregion

        #region AtualizarEmailBloqueado
        public static void AtualizarEmailBloqueado(string email)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Eml_Pessoa", SqlDbType = SqlDbType.VarChar, Size = 8000, Value = email }
				};

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpAtualizarEmailBloqueado, parms);
        }
        #endregion

        #region AtualizarEmailBounce
        public static void AtualizarEmailBounce(string email)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Eml_Pessoa", SqlDbType = SqlDbType.VarChar, Size = 8000, Value = email }
				};

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpAtualizarEmailBounce, parms);
        }
        #endregion

        #region AtualizarEmailInvalido
        public static void AtualizarEmailInvalido(string email)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Eml_Pessoa", SqlDbType = SqlDbType.VarChar, Size = 8000, Value = email }
				};

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpAtualizarEmailInvalido, parms);
        }
        #endregion

        #region AtualizarEmailUnsubscribe
        public static void AtualizarEmailUnsubscribe(string email)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Eml_Pessoa", SqlDbType = SqlDbType.VarChar, Size = 8000, Value = email }
				};

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpAtualizarEmailUnsubscribe, parms);
        }
        #endregion

        #region CarregarIdCadastradoEm
        public static int CarregarIdCadastradoEm(DateTime dataCadastro)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Dta_Cadastro", SqlDbType = SqlDbType.DateTime, Value = dataCadastro }
				};

            int retorno = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpCarregarIdCadastradoEm, parms));

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

            var parms = new List<SqlParameter>() 
                {
                    new SqlParameter{ ParameterName = "@Idf_Pagamento", SqlDbType = SqlDbType.Int, Size = 6, Value = idPagamento}
                };

            IDataReader dr = trans == null ? DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarInformacoesIntegracaoFinanceiro, parms) : DataAccessLayer.ExecuteReader(trans, CommandType.Text, SpRecuperarInformacoesIntegracaoFinanceiro, parms);

            if (dr.Read())
            {
                if (dr["Dta_Pagamento"] != null)
                    dataPagamento = Convert.ToDateTime(dr["Dta_Pagamento"]);
                valorPagamento = Convert.ToDecimal(dr["Vlr_Pagamento"]);
                desIdentificador = dr["Des_Identificador"].ToString();
                dataInicioPlano = Convert.ToDateTime(dr["Dta_Inicio_Plano"]);
                dataFimPlano = Convert.ToDateTime(dr["Dta_Fim_Plano"]);
                nomePessoa = dr["Nme_Pessoa"].ToString();
                numCEP = dr["Num_CEP"].ToString();
                rua = dr["Des_Logradouro"].ToString();
                if (!Int32.TryParse(dr["Num_Endereco"].ToString(), out numEndereco))
                {
                    numEndereco = 0;
                }
                if (dr["Des_Complemento"] != DBNull.Value)
                    complemento = dr["Des_Complemento"].ToString();
                bairro = dr["Des_Bairro"].ToString();
                cidade = dr["Nme_Cidade"].ToString();
                uf = dr["Sig_Estado"].ToString();
                emailContato = dr["Eml_Pessoa"].ToString();
                ddd = dr["Num_DDD_Celular"].ToString();
                telefone = dr["Num_Celular"].ToString();
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
        /// Método que invalida o celular dos currículos diferente do informado
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
					new SqlParameter { ParameterName = "@Num_DDD_Celular", SqlDbType = SqlDbType.Char, Size = 2, Value = numDDDCelular },
					new SqlParameter { ParameterName = "@Num_Celular", SqlDbType = SqlDbType.Char, Size = 10, Value = numCelular },
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo }
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
                    string descricaoCRM = string.Format("Celular excluído. Curriculo {0} informou o código de confirmação para o celular {1}!", objCurriculo.IdCurriculo, Helper.FormatarTelefone(numDDDCelular, numCelular));

                    CurriculoObservacao.SalvarCRM(descricaoCRM, new Curriculo(curriculo.Value), nomeProcessoPaiParaSalvarNoCRM, trans);
                }
            }
        }
        #endregion

        #region ValidacaoCelularGerarCodigo
        private static string ValidacaoCelularGerarCodigo(string numeroDDDCelular, string numeroCelular)
        {
            var objValida = new ValidaCelular.ValidaCelular();

            return objValida.GerarCodigo(numeroDDDCelular, numeroCelular);
        }
        #endregion ValidacaoCelularGerarCodigo

        #region ValidacaoCelularEnviarCodigo
        public static bool ValidacaoCelularEnviarCodigo(string numeroDDDCelular, string numeroCelular)
        {
            string codigo = ValidacaoCelularGerarCodigo(numeroDDDCelular, numeroCelular);
            var mensagem = CartaSMS.RecuperaValorConteudo(BLL.Enumeradores.CartaSMS.ValidacaoCelularEnvioCodigo);
            var parametros = new { codigo = codigo };

            var i = BLL.MensagemCS.SalvarSMS(null, null, null, parametros.ToString(mensagem), numeroDDDCelular, numeroCelular);

            return i > 0;
        }
        #endregion ValidacaoCelularEnviarCodigo

        #region ValidacaoCelularValidarCodigo
        public static bool ValidacaoCelularValidarCodigo(string numeroDDDCelular, string numeroCelular, string codigoValidacao)
        {
            var objValida = new ValidaCelular.ValidaCelular();
            return objValida.ValidarCodigo(numeroDDDCelular, numeroCelular, codigoValidacao);
        }
        #endregion ValidacaoCelularValidarCodigo

        #region ValidacaoCelularCodigoEnviado
        public static void ValidacaoCelularCodigoEnviado(string numeroDDDCelular, string numeroCelular, out DateTime? dataEnvio)
        {
            var objValida = new ValidaCelular.ValidaCelular();
            objValida.CodigoEnviado(numeroDDDCelular, numeroCelular, out dataEnvio);
        }
        #endregion ValidacaoCelularValidarCodigo

        #region ValidacaoCelularUtilizarCodigo
        public static bool ValidacaoCelularUtilizarCodigo(string numeroDDDCelular, string numeroCelular, string codigoValidacao)
        {
            var objValida = new ValidaCelular.ValidaCelular();
            return objValida.UtilizarCodigo(numeroDDDCelular, numeroCelular, codigoValidacao);
        }
        #endregion ValidacaoCelularUtilizarCodigo


        #region Validação E-Mail

        #region ValidacaoEmailGerarCodigo
        public string ValidacaoEmailGerarCodigo(SqlTransaction trans)
        {
            string token = Helper.ToBase64(Guid.NewGuid().ToString());

            var objCodigoConfirmacaoEmail = new CodigoConfirmacaoEmail
            {
                CodigoConfirmacao = token,
                DataCriacao = DateTime.Now,
                EmailConfirmacao = this.EmailPessoa
            };

            if (trans != null)
                objCodigoConfirmacaoEmail.Save(trans);
            else
                objCodigoConfirmacaoEmail.Save();


            return Helper.ToBase64(string.Format("userid={0}&codigo={1}", this.IdPessoaFisica, token));
        }
        #endregion ValidacaoEmailGerarCodigo

        #region ValidacaoEmailUtilizarCodigo
        public bool ValidacaoEmailUtilizarCodigo(string codigoValidacao)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var objCodigoConfirmacaoEmail = CodigoConfirmacaoEmail.CarregarPorCodigo(codigoValidacao, trans);

                        if (objCodigoConfirmacaoEmail == null)
                            throw new ArgumentNullException("Código inexistente!");

                        if (objCodigoConfirmacaoEmail.DataUtilizacao.HasValue)
                            throw new ArgumentException("Código já utilizado!");

                        objCodigoConfirmacaoEmail.DataUtilizacao = DateTime.Now;
                        objCodigoConfirmacaoEmail.Save(trans);

                        Curriculo objCurriculo;
                        if (Curriculo.CarregarPorPessoaFisica(trans, this.IdPessoaFisica, out objCurriculo))
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

                        this.EmailSituacaoConfirmacao = EmailSituacao.LoadObject(Convert.ToInt16(Enumeradores.EmailSituacao.Confirmado));
                        this.EmailSituacaoValidacao = EmailSituacao.LoadObject(Convert.ToInt16(Enumeradores.EmailSituacao.Validado));
                        this.FlagEmailConfirmado = true;
                        this.Save(trans);

                        trans.Commit();
                        if (objCurriculo != null)
                            try
                            {
                                if (objCurriculo.OrigemBNE())
                                    objCurriculo.GatilhoCadastroBNE();
                            }
                            catch (Exception ex)
                            {
                                GerenciadorException.GravarExcecao(ex);
                            }

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

        /*
        #region ValidacaoCelularValidarCodigo
        public static bool ValidacaoCelularValidarCodigo(string numeroDDDCelular, string numeroCelular, string codigoValidacao)
        {
            var objValida = new ValidaCelular.ValidaCelular();
            return objValida.ValidarCodigo(numeroDDDCelular, numeroCelular, codigoValidacao);
        }
        #endregion ValidacaoCelularValidarCodigo

        #region ValidacaoCelularCodigoEnviado
        public static void ValidacaoCelularCodigoEnviado(string numeroDDDCelular, string numeroCelular, out DateTime? dataEnvio)
        {
            var objValida = new ValidaCelular.ValidaCelular();
            objValida.CodigoEnviado(numeroDDDCelular, numeroCelular, out dataEnvio);
        }
        #endregion ValidacaoCelularValidarCodigo

        
        */

        #endregion


        /// <summary>
        /// Salvar apenas o objeto Pessoa Fisica
        /// </summary>
        /// <param name="objPessoaFisica"></param>
        /// <returns></returns>
        #region SalvarPessoaFisica
        public void SalvarPessoaFisica(PessoaFisica objPessoaFisica)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        objPessoaFisica.Save();

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
            int? resultId = new int?();
            return new Func<int>(() =>
            {
                if (resultId.HasValue)
                    return resultId.Value;

                resultId = CarregarCidadeId(pessoaFisicaId);
                return resultId.Value;
            });
        }

        public static int? CarregarCidadeId(int pessoaFisicaId)
        {
            const string sql = @"
            SELECT Idf_Cidade FROM BNE.TAB_Pessoa_Fisica
            WHERE Idf_Pessoa_Fisica = @p0";

            var sqlParams = new List<SqlParameter>()
                           { 
                               new SqlParameter("@p0", pessoaFisicaId) { SqlDbType = SqlDbType.Int }
                           };

            var res = DataAccessLayer.ExecuteScalar(CommandType.Text, sql, sqlParams);

            int realValue;
            if (Convert.IsDBNull(res) || res == null || !Int32.TryParse(res.ToString(), out realValue) || realValue <= 0)
            {
                return null;
            }
            return realValue;
        }

        #region UsuarioTanque
        public static DataTable UsuarioTanque(String strcpf)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal));
            parms[0].Value = Convert.ToDecimal(strcpf);

            DataTable dt = new DataTable();
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

        #endregion

    }
}