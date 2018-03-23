//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using BNE.BLL.Common;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.DTO;
using BNE.BLL.Enumeradores;
using BNE.EL;
using Sine.Integracao.Api;
using Sine.Integracao.Client;
using Sine.Integracao.Model;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.BLL.AsyncServices.Enumeradores;

namespace BNE.BLL
{
    public partial class VagaCandidato // Tabela: BNE_Vaga_Candidato
    {
        #region Consultas
        private const string Spselectporvagacurriculo = @"
        SELECT * FROM BNE_Vaga_Candidato WITH(NOLOCK) WHERE Idf_Vaga = @Idf_Vaga AND Idf_Curriculo = @Idf_Curriculo";

        private const string Spselectvagacandidatoporfilial = @"
        SELECT  COUNT(C.Idf_Vaga_Candidato)
        FROM    BNE.BNE_Vaga V WITH ( NOLOCK )
                INNER JOIN BNE.BNE_Vaga_Candidato C WITH ( NOLOCK ) ON C.Idf_Vaga = V.Idf_Vaga
                LEFT JOIN BNE.BNE_Curriculo_Visualizacao cv WITH ( NOLOCK ) ON cv.Idf_Curriculo = C.Idf_Curriculo AND cv.Idf_Filial = @Idf_Filial
        WHERE   V.Idf_Filial = @Idf_Filial
                AND V.Flg_Vaga_Arquivada = 0
                AND V.Flg_Inativo = 0
                AND V.Flg_Vaga_Rapida = 0
                AND cv.Idf_Curriculo_Visualizacao IS NULL";

        private const string Spvagacandidatoquantidade = @"
        SELECT  COUNT(VC.Idf_Curriculo)
        FROM    BNE_Vaga_Candidato VC WITH(NOLOCK)
        WHERE   VC.Idf_Curriculo = @Idf_Curriculo 
                AND VC.Flg_Inativo = 0";

        private const string Spvagacandidatoporcurriculo = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
        SELECT	ROW_NUMBER() OVER (ORDER BY VC.Dta_Cadastro DESC) AS RowID,
		        VC.Dta_Cadastro ,
		        F.Idf_Filial,
		        V.Idf_Vaga,
                FUN.Idf_Funcao,
		        FUN.Des_Funcao ,
                Des_Area_BNE,
                V.Flg_Deficiencia,
                V.Idf_Deficiencia,
		              CASE WHEN Vlr_Salario_De > 0 AND Vlr_Salario_Para > 0
						THEN ''R$ '' + REPLACE(Vlr_Salario_De, ''.'', '','') + '' a '' + ''R$ '' + REPLACE(Vlr_Salario_Para, ''.'', '','')
					WHEN  Vlr_Salario_De > 0
						THEN ''R$ '' + Replace(vlr_salario_De, ''.'','','')
					when Vlr_Salario_Para >0
						THEN ''R$ '' + Replace(vlr_salario_Para, ''.'','','')
			        ELSE ''A combinar''
		        END AS Vlr_Salario ,                                                                                   
		        ( CASE 
                   WHEN ( V.Flg_Confidencial = 1 )
				        THEN ''Empresa Confidencial''
		           WHEN ( O.Idf_Tipo_Origem = 3 )
                        THEN V.Nme_Empresa
                   ELSE F.Raz_Social
		        END ) AS Raz_Social ,
		        ( CASE WHEN ( V.Flg_Confidencial = 1 ) THEN 1 
			          ELSE 0 -- Visible = true
		        END ) AS Img_Empresa_Confidencial_Visible ,
                C.Flg_VIP,
                V.Flg_Inativo,
                V.Flg_Vaga_Arquivada,
                V.Dta_Prazo
          FROM    BNE.BNE_Vaga_Candidato VC WITH(NOLOCK)
		        JOIN BNE.BNE_Curriculo C WITH(NOLOCK) ON VC.Idf_Curriculo = C.Idf_Curriculo
		        JOIN BNE.BNE_Vaga V WITH(NOLOCK) ON VC.Idf_Vaga = V.Idf_Vaga
                JOIN BNE.TAB_Origem O WITH(NOLOCK) ON O.Idf_Origem = V.Idf_Origem
		        JOIN plataforma.TAB_Funcao FUN WITH(NOLOCK) ON V.Idf_Funcao = FUN.Idf_Funcao
		        JOIN BNE.TAB_Filial F WITH(NOLOCK) ON V.Idf_Filial = F.Idf_Filial
				JOIN plataforma.TAB_CNAE_Sub_Classe csc WITH (NOLOCK) ON f.Idf_CNAE_Principal = csc.Idf_CNAE_Sub_Classe
                JOIN plataforma.TAB_CNAE_Classe cl WITH (NOLOCK) ON csc.Idf_CNAE_Classe = cl.Idf_CNAE_Classe
                JOIN plataforma.TAB_CNAE_Grupo gp WITH (NOLOCK) ON cl.Idf_CNAE_Grupo = gp.Idf_CNAE_Grupo
                JOIN plataforma.TAB_CNAE_Divisao d WITH (NOLOCK) ON gp.Idf_CNAE_Divisao = d.Idf_CNAE_Divisao
                JOIN plataforma.TAB_Area_BNE area WITH (NOLOCK) ON d.Idf_Area_BNE = area.Idf_Area_BNE
           LEFT JOIN BNE.BNE_Vaga_Candidato VC2 ON VC2.Idf_Curriculo = C.Idf_Curriculo AND C.Flg_VIP = 0
				 AND VC2.Flg_Auto_Candidatura = 1 AND VC2.Idf_Vaga = VC.Idf_Vaga
        WHERE   VC.Flg_Inativo = 0 AND VC.Idf_Curriculo = ' + CONVERT(VARCHAR, @Idf_Curriculo) + '
          AND VC2.Idf_Vaga IS NULL  and v.Flg_Vaga_Arquivada =  ' + CONVERT(VARCHAR, @Status)
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect	+ ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)

        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";

        private const string Spselectcurriculojacandidatou = @"
        SELECT count(*) FROM BNE_Vaga_Candidato WITH(NOLOCK) WHERE Idf_Vaga = @Idf_Vaga AND Idf_Curriculo = @Idf_Curriculo";

        #region spCandidaturaOportunidade
        private const string spCandidaturaOportunidade = @"select top 20 pf.idf_pessoa_fisica, pf.nme_pessoa, pf.eml_pessoa, pf.dta_nascimento,
					cu.idf_curriculo,
					f.des_funcao,
					c.nme_cidade, c.Sig_Estado
			from bne_vaga_candidato vc with(nolock)
				join bne_curriculo cu with(nolock) on cu.idf_curriculo = vc.idf_curriculo
				join tab_pessoa_fisica pf with(nolock) on pf.idf_pessoa_fisica = cu.idf_pessoa_fisica
				cross apply(
				select top 1 idf_curriculo, idf_funcao, idf_cidade
					 from bne_funcao_pretendida with(nolock)
					 where idf_curriculo = cu.idf_curriculo
		             order by idf_funcao_pretendida asc
					 ) as fp 
				join plataforma.TAB_Funcao f with(nolock) on f.idf_funcao = fp.idf_funcao
				left join plataforma.TAB_Cidade c with(nolock) on c.idf_cidade = pf.idf_cidade
				join bne_vaga v with(nolock) on vc.idf_vaga = v.idf_vaga
            where vc.idf_vaga= @idf_vaga
                and vc.flg_inativo = 0 
				and vc.dta_cadastro > @dta_prazo
				and vc.dta_cadastro > (getdate() -15)
				and fp.idf_funcao = v.idf_funcao
				and fp.idf_cidade = v.idf_cidade
                and cu.idf_situacao_curriculo not in(6,7,12)
            order by vc.dta_cadastro desc

            ";
        #endregion

        #region Spselectvagasporfilial
        private const string Spselectvagasporfilial = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)

        SET @FirstRec = @CurrentPage * @PageSize + 1
        SET @LastRec = @CurrentPage * @PageSize + @PageSize

        SET @iSelect = '
            SELECT 
			        ROW_NUMBER() OVER (ORDER BY Flg_Vaga_Arquivada, V.Dta_Cadastro DESC) AS RowID ,
                    V.Idf_Vaga ,
                    ( BNE.BNE_FUNCAO_VAGA_ORIGEM('+ ISNULL(Convert(VARCHAR,@Idf_Origem), '''''') + ',V.Idf_Vaga) ) AS Des_Funcao, 
                    V.Cod_Vaga ,
                    CONVERT(VARCHAR, CONVERT(DATETIME, V.Dta_Prazo, 103), 103) AS Dta_Prazo ,
                    (   
				        SELECT  COUNT (VCSub.Idf_Vaga_Candidato) 
                        FROM    bne.BNE_Vaga_Candidato VCSub WITH(NOLOCK)
                        WHERE   VCSub.Idf_Vaga = V.Idf_Vaga
                                AND VCSub.Flg_Inativo = 0) as Num_Cvs_Recebidos ,
                    V.Dta_Cadastro ,
                    C.Nme_Cidade ,
                    C.Sig_Estado ,
                    Flg_Vaga_Arquivada ,
                    ( 
				        SELECT  COUNT (VCSub.Idf_Vaga_Candidato) 
                        FROM    bne.BNE_Vaga_Candidato VCSub WITH(NOLOCK)
                        WHERE   VCSub.Idf_Vaga = V.Idf_Vaga 
						        AND VCSub.Idf_Curriculo = ' + CONVERT(VARCHAR, @Idf_Curriculo) + '
                                AND VCSub.Flg_Inativo = 0
                    ) Ja_Candidatou
            FROM    bne.BNE_Vaga V WITH(NOLOCK)
                    LEFT JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON F.Idf_Funcao = V.Idf_Funcao
                    INNER JOIN plataforma.TAB_Cidade C WITH(NOLOCK) ON C.Idf_Cidade = V.Idf_Cidade
            WHERE 
                    V.Idf_Filial = ' + CONVERT(VARCHAR, @Idf_Filial) + '
                    AND V.Flg_Inativo = 0
                    AND V.Flg_Vaga_Rapida = 0
                    AND Flg_Vaga_Arquivada = 0 '

        IF (@ids IS NOT NULL)
            SET @iSelect = @iSelect + ' AND V.Idf_Funcao IN ( '+ @ids +' ) '

        SET @iSelectCount = 'Select Count(1) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect  + ' ) As TblTempPag	Where RowID >= ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID <= ' + CONVERT(VARCHAR, @LastRec)

        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";
        #endregion

        #region Spquantidadecandidaturatotalporvaga
        private const string Spquantidadecandidaturatotalporvaga = @"
        SELECT  COUNT(VC.Idf_Curriculo)
        FROM    BNE_Vaga_Candidato VC WITH(NOLOCK)
        WHERE   VC.Idf_Vaga = @Idf_Vaga";
        #endregion

        #region Sptodoscurriculoscandidatados
        private const string Sptodoscurriculoscandidatados = @"
        SELECT  VC.Idf_Curriculo
        FROM    BNE_Vaga_Candidato VC WITH(NOLOCK)
        WHERE   VC.Idf_Vaga = @Idf_Vaga";
        #endregion

        #region SPCandidaturasDoDia
        private const string SPCandidaturasDoDia = @"
        SET NOCOUNT ON
        DECLARE @date DATETIME
        SET @date = CONVERT(VARCHAR(21), GETDATE(), 127)

        SELECT  cv.Idf_Curriculo ,
                Nme_Pessoa ,
                Eml_Pessoa ,
                v.Idf_Vaga ,
                f.Des_Funcao ,
                Nme_Cidade ,
                c.Sig_Estado,
		        v.Des_Atribuicoes,
		        --CONVERT(DECIMAL(10,2), v.Vlr_Salario_De) AS Vlr_Salario_De,
		        --CONVERT(DECIMAL(10,2),v.Vlr_Salario_Para) AS Vlr_Salario_Para,
                v.Vlr_Salario_De,
                v.Vlr_Salario_Para,
                cv.Idf_Pessoa_Fisica
        FROM    BNE.BNE_Curriculo cv WITH ( NOLOCK )
                JOIN BNE.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON pf.Idf_Pessoa_Fisica = cv.Idf_Pessoa_Fisica
                JOIN BNE.BNE_Vaga_Candidato vc WITH ( NOLOCK ) ON vc.Idf_Curriculo = cv.Idf_Curriculo
                JOIN BNE.BNE_Vaga v WITH ( NOLOCK ) ON v.Idf_Vaga = vc.Idf_Vaga
                JOIN plataforma.TAB_Cidade c WITH ( NOLOCK ) ON c.Idf_Cidade = v.Idf_Cidade
                JOIN plataforma.TAB_Funcao f WITH ( NOLOCK ) ON f.Idf_Funcao = v.Idf_Funcao
        WHERE   vc.Dta_Cadastro BETWEEN CONVERT(VARCHAR(21), DATEADD(DAY,-1, @date),127)
                                AND @date
                                and vc.flg_inativo = 0 
                                and cv.idf_situacao_curriculo not in(6,7,12) ";//Bloqueado - Cancelado - Exclusão Lógica
        #endregion

        #region Spalterarstatus
        private const string Spalterarstatus = @"
        UPDATE BNE_Vaga_Candidato SET Idf_Status_Curriculo_Vaga = @Idf_Status_Curriculo_Vaga WHERE Idf_Vaga_Candidato = @Idf_Vaga_Candidato
        ";
        #endregion

        #region Spcandidaturasparanotificar
        private const string Spcandidaturasparanotificar = @"
 SELECT  V.Idf_Vaga ,
        F.Des_Funcao ,
        Cid.Nme_Cidade ,
        Cid.Sig_Estado ,
        Nme_Pessoa ,
        Dta_Nascimento ,
        VC.Idf_Curriculo ,
        Idf_Vaga_Candidato
FROM    BNE.BNE_Vaga V WITH ( NOLOCK )
        JOIN BNE.TAB_Filial fil ON V.Idf_Filial = fil.Idf_Filial
        JOIN BNE.BNE_Vaga_Candidato VC WITH ( NOLOCK ) ON VC.Idf_Vaga = V.Idf_Vaga
        JOIN plataforma.TAB_Cidade Cid WITH ( NOLOCK ) ON V.Idf_Cidade = Cid.Idf_Cidade
        JOIN plataforma.TAB_Funcao F WITH ( NOLOCK ) ON V.Idf_Funcao = F.Idf_Funcao
        JOIN BNE.BNE_Curriculo C WITH ( NOLOCK ) ON C.Idf_Curriculo = VC.Idf_Curriculo
        JOIN BNE.TAB_Pessoa_Fisica PF WITH ( NOLOCK ) ON PF.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
        LEFT JOIN BNE.BNE_Vaga_Integracao vi WITH(NOLOCK) ON vi.Idf_Vaga = V.Idf_Vaga	
WHERE   VC.Idf_Status_Curriculo_Vaga = 2
        AND VC.Flg_Inativo = 0
        AND C.Flg_Inativo = 0
        AND V.Flg_Inativo = 0
        AND fil.Idf_Situacao_Filial IN ( 1, 2, 3, 4, 7 )
        AND V.flg_vaga_arquivada = 0
        AND vi.Idf_Vaga_Integracao IS null
        AND V.Flg_Receber_Todos_CV = 1 
ORDER BY V.Idf_Vaga DESC
        ";
        #endregion Spcandidaturasparanotificar

        #endregion

        #region Métodos
        public static DataTable CandidaturasDoDia()
        {
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPCandidaturasDoDia, null).Tables[0];
        }

        #region CandidatosOportunidade
        /// <summary>
        ///     Retorna as  candidatuas dos ultimos 15 dias para as vagas de oportunidade(Arquivadas)
        /// </summary>
        /// <returns></returns>
        public static DataTable CandidatosOportunidade(int idfVaga, DateTime dtaPrazo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@idf_Vaga", SqlDbType.Int, 4),
                new SqlParameter("@dta_Prazo", SqlDbType.DateTime)
            };
            parms[0].Value = idfVaga;
            parms[1].Value = dtaPrazo;
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, spCandidaturaOportunidade, parms).Tables[0];
        }
        #endregion

        #region CarregarPorVagaCurriculo
        /// <summary>
        ///     Método responsável por carregar uma instancia de VagaCandidato através do
        ///     identificar de uma vaga e um curriculo
        /// </summary>
        /// <param name="idVaga">Identificador da Vaga</param>
        /// <param name="idCurriculo">Identificador de Curriculo</param>
        /// <param name="objVagaCandidato">Parametro out VagaCandidato </param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorVagaCurriculo(int idVaga, int idCurriculo, out VagaCandidato objVagaCandidato, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4), /* 0 */
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4) /* 1 */
            };
            parms[0].Value = idVaga;
            parms[1].Value = idCurriculo;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectporvagacurriculo, parms))
            {
                objVagaCandidato = new VagaCandidato();
                if (SetInstance(dr, objVagaCandidato))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objVagaCandidato = null;
            return false;
        }
        #endregion

        #region Salvar
        /// <summary>
        /// </summary>
        /// <param name="objVaga"></param>
        /// <param name="objCurriculo"></param>
        /// <param name="objOrigem">
        ///     Como as vagas na pesquisa dentro do STC podem vir do BNE, a origem é passada como parametro,
        ///     para relacionar o CV a origem do STC
        /// </param>
        /// <param name="descricaoIP"></param>
        /// <param name="objIntencaoFilial"></param>
        /// <param name="listVagaPergunta"></param>
        /// <param name="origemCandidatura">Origem da candidatura (site, wsmobile, tanque) </param>
        public void Salvar(Vaga objVaga, Curriculo objCurriculo, Origem objOrigem, string descricaoIP, IntencaoFilial objIntencaoFilial, List<VagaCandidatoPergunta> listVagaPergunta, bool siteSTC, bool siteSTCCandidaturaLivre, bool avulsaMobile, Enumeradores.OrigemCandidatura origemCandidatura, out int? quantidadeCandidaturasGratis)
        {
            quantidadeCandidaturasGratis = null;
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var podeCandidatar = false;
                        ParametroCurriculo objParametroCurriculo = null;

                        if ((siteSTC && siteSTCCandidaturaLivre) || avulsaMobile || ValidaCandidatura(objCurriculo, objVaga, trans))
                            podeCandidatar = true;
                        else
                        {
                            if (siteSTCCandidaturaLivre)
                            {
                                if (objOrigem != null && objVaga.Origem.IdOrigem.Equals(objOrigem.IdOrigem))
                                    podeCandidatar = true;
                                else
                                    quantidadeCandidaturasGratis = 0;
                            }
                            else
                            {
                                //Se tem o parametro, verifica se possui saldo
                                if (ParametroCurriculo.CarregarParametroPorCurriculo(Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, objCurriculo, out objParametroCurriculo, trans))
                                {
                                    var quantidadeCandidaturas = Convert.ToInt32(objParametroCurriculo.ValorParametro);
                                    if (quantidadeCandidaturas > 0)
                                    {
                                        quantidadeCandidaturasGratis = quantidadeCandidaturas;
                                        var candidaturasRestantes = quantidadeCandidaturas - 1;
                                        podeCandidatar = true;
                                        objParametroCurriculo.ValorParametro = candidaturasRestantes.ToString(); //Desconta a candidatura atual
                                    }
                                    else
                                    {
                                        objParametroCurriculo = null;
                                        quantidadeCandidaturasGratis = 0;
                                    }
                                }
                                else
                                {
                                    quantidadeCandidaturasGratis = 0;
                                }
                            }
                        }

                        //Salvando o saldo para o currículo atual
                        if (objParametroCurriculo != null)
                            objParametroCurriculo.Save(trans);

                        if (podeCandidatar) //Se pode candidatar (é vip ou tem saldo)
                        {
                            if (objOrigem != null) //Utilizado quando a Origem é Minha Empresa Oferece Cursos, onde vagas do BNE aparecem na pesquisa de vaga
                            {
                                if (!CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, objOrigem, trans))
                                {
                                    var objCurriculoOrigem = new CurriculoOrigem
                                    {
                                        Curriculo = objCurriculo,
                                        Origem = objOrigem,
                                        DescricaoIP = descricaoIP
                                    };
                                    objCurriculoOrigem.Save(trans);

                                    PlanoAdquirido.ConcederPlanoPF(objCurriculo, new Plano(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPUniversitarioGratis))), trans);
                                }
                            }
                            else
                            {
                                if (!CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, objVaga.Origem, trans))
                                {
                                    var objCurriculoOrigem = new CurriculoOrigem
                                    {
                                        Curriculo = objCurriculo,
                                        Origem = objVaga.Origem,
                                        DescricaoIP = descricaoIP
                                    };
                                    objCurriculoOrigem.Save(trans);
                                }
                            }

                            objIntencaoFilial.Save(trans);

                            OrigemCandidatura = new OrigemCandidatura((int)origemCandidatura);
                            StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.SemEnvio);
                            //Se a vaga exigir envio de todos os curriculos, um service irá enviar o email nestes casos
                            if (Convert.ToBoolean(objVaga.FlagReceberTodosCV) && objVaga.FlagReceberTodosCV.HasValue)
                            {
                                //Pode ser que uma vaga esteja ainda indexada no GOOGLE, não deve enviar e-mail para a empresa
                                if (objVaga.FlagVagaArquivada || objVaga.FlagInativo)
                                    StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.SemEnvio);
                                else
                                    StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.AguardoEnvio);
                            }

                            if (objVaga.CampanhaRecrutamentoNotificarCandidatura(objCurriculo, trans))
                            {
                                try
                                {
                                    HistoricoCandidatura.Logar(this.Curriculo, this.Vaga, string.Format("Campanha Recrutamento - SMS para retornar ligação - Entrou no fluxo."), trans);
                                    StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.SemEnvio);

                                    UsuarioFilial objUsuarioFilial;
                                    if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objVaga.UsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                                    {
                                        HistoricoCandidatura.Logar(this.Curriculo, this.Vaga, string.Format("Campanha Recrutamento - SMS para retornar ligação - Encontrou usuário filial."), trans);

                                        var parametros = new
                                        {
                                            NomeCandidato = objCurriculo.PessoaFisica.PrimeiroNome,
                                            Telefone = BNE.BLL.Custom.Helper.FormatarTelefone(objUsuarioFilial.NumeroDDDComercial, objUsuarioFilial.NumeroComercial),
                                            Funcao = objVaga.DescricaoFuncao
                                        };

                                        var sms = parametros.ToString(CartaSMS.RecuperaValorConteudo(Enumeradores.CartaSMS.CampanhaVagaRetornoParaLigacao));

                                        var idUsuario = Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfUsuarioEnvioCampanha);
                                        objCurriculo.PessoaFisica.CompleteObject();

                                        var objSMSTanque = new List<PessoaFisicaEnvioSMSTanque>
                                        {
                                            new PessoaFisicaEnvioSMSTanque
                                            {
                                                mensagem = sms,
                                                dddCelular = objCurriculo.PessoaFisica.NumeroDDDCelular,
                                                numeroCelular = objCurriculo.PessoaFisica.NumeroCelular,
                                                nomePessoa = objCurriculo.PessoaFisica.PrimeiroNome
                                            }
                                        };

                                        HistoricoCandidatura.Logar(this.Curriculo, this.Vaga, string.Format("Campanha Recrutamento - SMS para retornar ligação - Montou a mensagem para enviar para o tanque"), trans);

                                        Mensagem.EnvioSMSTanque(idUsuario, objSMSTanque, true);

                                        HistoricoCandidatura.Logar(this.Curriculo, this.Vaga, string.Format("Campanha Recrutamento - SMS para retornar ligação - Enviou a mensagem para o tanque"), trans);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GerenciadorException.GravarExcecao(ex, "Falha ao enviar mensagem de retorno para o candidato");
                                }
                            }

                            Save(trans);

                            if (listVagaPergunta.Count > 0)
                            {
                                foreach (var objVagaCandidatoPergunta in listVagaPergunta)
                                    objVagaCandidatoPergunta.Save(trans);
                            }

                            #region FlagReceberCadaCV - Enviar Email
                            //Se a vaga estiver ativa
                            objVaga.Filial.CompleteObject();
                            if (!objVaga.FlagVagaArquivada && !objVaga.FlagInativo && !objVaga.Filial.EmpresaBloqueada())
                            {
                                //Se a vaga exigir envio de cada curriculo por email                        
                                if (objVaga.FlagReceberCadaCV.HasValue && objVaga.FlagReceberCadaCV.Value)
                                {
                                    string curriculoCandidato;
                                    if (objVaga.Origem.IdOrigem.Equals((int)Enumeradores.Origem.BNE))
                                    {
                                        objVaga.UsuarioFilialPerfil.CompleteObject();
                                        objVaga.UsuarioFilialPerfil.PessoaFisica.CompleteObject();
                                        curriculoCandidato = objCurriculo.RecuperarHTMLCurriculo(objIntencaoFilial.Filial.IdFilial, null, objVaga.IdVaga, false, true, false, true, FormatoVisualizacaoImpressao.Empresa, objVaga.Filial.PossuiPlanoAtivo(), objVaga.UsuarioFilialPerfil.PessoaFisica.CPF, objVaga.UsuarioFilialPerfil.PessoaFisica.DataNascimento);
                                    }
                                    else
                                        curriculoCandidato = objCurriculo.RecuperarHTMLCurriculo(objIntencaoFilial.Filial.IdFilial, null, objVaga.IdVaga, true, true, false, false, FormatoVisualizacaoImpressao.Empresa, objVaga.Filial.PossuiPlanoAtivo(), null, null);

                                    Notificar(curriculoCandidato, trans);
                                }
                            }
                            #endregion

                            #region Integração de Candidatura
                            //Verificando se a vaga tem integração
                            //Se tiver, efetua a requisição na url parametrizada
                            try
                            {
                                var parametrosAtividade = new ParametroExecucaoCollection
                                    {
                                        {"IdVaga","IdVaga", objVaga.IdVaga.ToString(), objVaga.IdVaga.ToString()},
                                        {"IdCurriculo","IdCurriculo", objCurriculo.IdCurriculo.ToString(), objCurriculo.IdCurriculo.ToString() }
                                    };
                                ProcessoAssincrono.IniciarAtividade(TipoAtividade.IntegrarCandidaturaSine, parametrosAtividade);
                            }
                            catch (Exception ex)
                            {
                                EL.GerenciadorException.GravarExcecao(ex, "Erro para integrar candidatura");
                                //infileirar candidaturas 

                            }

                            #endregion
                        }
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

        #region ValidaCandidatura
        /// <summary>
        ///     Verificacão de candidatura
        /// </summary>
        /// <returns>Boolean</returns>
        private static bool ValidaCandidatura(Curriculo objCurriculo, Vaga objVaga, SqlTransaction trans)
        {
            if (objCurriculo.FlagVIP) //Se for VIP
                return true;

            if (objVaga.FlagBNERecomenda) //Se for VIP
                return true;

            if (objVaga.Filial.PossuiPlanoAtivo()) //Se empresa possui plano ativo
                return true;

            if (objVaga.PossuiPlanoPublicacaoImediata() != null) //Se empresa comprou plano de publicacao imediata 
                return true;

            if (PlanoAdquiridoDetalhesCurriculo.RecebeuCampanhaVagaPerfil(objCurriculo, objVaga, trans)) //Se o currículo recebeu campanha de recrutamento
                return true;

            if (objVaga.FlagInativo) //Se a vaga está inativa (regra que subiu no natal)
                return true;

            if (objVaga.FlagVagaArquivada) //Se a vaga está arquivada (regra que subiu no natal)
                return true;

            //RETIRADA VAGA PREMIUM TASK: 41857
            // if (ParametroVaga.Premium((int)Enumeradores.Parametro.VagaPremium, objVaga.IdVaga)) //candidatura comprada
            //  return true;

            return false;
        }
        #endregion

        #region CandidatoVagaPorFilial
        /// <summary>
        ///     Carrega a quantidade de curriculos candidatados para as vagas de uma determinada empresa.
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static int CandidatoVagaPorFilial(int idFilial)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Filial", SqlDbType.Int, 4) /* 0 */
            };
            parms[0].Value = idFilial;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectvagacandidatoporfilial, parms));
        }
        #endregion

        #region QuantidadeVagaCandidato
        public static int QuantidadeVagaCandidato(int idCurriculo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
            };
            parms[0].Value = idCurriculo;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spvagacandidatoquantidade, parms));
        }
        #endregion

        #region CarregarVagaCandidatadaPorCurriculo
        public static DataTable CarregarVagaCandidatadaPorCurriculo(int idCurriculo, bool status, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4),
                new SqlParameter("@CurrentPage", SqlDbType.Int, 4),
                new SqlParameter("@PageSize", SqlDbType.Int, 4),
                new SqlParameter("@Status", SqlDbType.Bit)
            };

            parms[0].Value = idCurriculo;
            parms[1].Value = paginaCorrente;
            parms[2].Value = tamanhoPagina;
            parms[3].Value = status;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spvagacandidatoporcurriculo, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();


                    dt.Load(dr);
                    dt.Columns.Add("MediaSalarial", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        row["MediaSalarial"] = Vaga.MediaSalarialComRegra((int)row["idf_Funcao"]);
                    }
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

        #region Notificar
        /// <summary>
        /// Metodo responsável por notificar a candidatura de um currículo para uma vaga
        /// </summary>
        /// <param name="curriculoCandidato"> </param>
        /// <param name="trans">Transaction</param>
        public void Notificar(string curriculoCandidato, SqlTransaction trans)
        {
            #region MensagemCS
            string templateAssunto;
            string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ApresentacaoTopoEmailcomCVAnexoparaEmpresa, out templateAssunto);

            //TODO: Precisa tudo isso?
            Vaga.Filial.CompleteObject(trans);
            Vaga.Cidade.CompleteObject(trans);
            Vaga.Funcao.CompleteObject(trans);

            var linkCurriculo = string.IsNullOrEmpty(curriculoCandidato) ? Curriculo.RetornaLinkVisualizacaoCurriculo(new List<int> { this.Curriculo.IdCurriculo }) : curriculoCandidato;
            var parametros = new
            {
                Vaga.Filial.RazaoSocial,
                Vaga.Cidade,
                NomeFuncao = Vaga.Funcao.DescricaoFuncao,
                LinkCurriculo = linkCurriculo,
                utmcampaign = String.Format("cv_{0}", this.Curriculo.IdCurriculo)

            };
            var assunto = parametros.ToString(templateAssunto);
            var mensagem = parametros.ToString(template);
            var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, trans);

            try
            {
                EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, Enumeradores.CartaEmail.ApresentacaoTopoEmailcomCVAnexoparaEmpresa, emailRemetente, Vaga.EmailVaga);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, "Falha ao enviar mensagem");
            }
            #endregion

            //So altera o status se a vaga não estiver com o flag receber todos os cvs
            if (!Vaga.FlagReceberTodosCV.HasValue || !Vaga.FlagReceberTodosCV.Value)
            {
                AlterarStatus(Enumeradores.StatusCurriculoVaga.Enviado, trans);
            }
        }

        /// <summary>
        /// Metodo responsável por enviar os links dos curriculos por email e atualizar a statusCurriculoVaga para Curriculo enviado
        /// </summary>
        /// <param name="objVaga">Vaga</param>
        /// <param name="candidaturas">Candidaturas</param>
        public static void Notificar(Vaga objVaga, List<DTO.Candidatura> candidaturas)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {

                        var retornoCarta = CartaEmail.RecuperarCarta(Enumeradores.CartaEmail.CandidaturaEnvioTodosCurriculos);
                        var linkCandidato = CartaEmail.RecuperarConteudo(Enumeradores.CartaEmail.CandidaturaEnvioTodosCurriculosLinkCurriculo);
                        var template = retornoCarta.Conteudo;
                        var templateAssunto = retornoCarta.Assunto;

                        objVaga.CompleteObject(trans);
                        objVaga.Filial.CompleteObject(trans);
                        objVaga.Cidade.CompleteObject(trans);
                        objVaga.Funcao.CompleteObject(trans);
                        objVaga.UsuarioFilialPerfil.CompleteObject(trans);
                        objVaga.UsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);
                        var sb = new StringBuilder();

                        foreach (var curriculo in candidaturas)
                        {
                            string urlVisualizacao;
                            if (objVaga.FlagVagaRapida)
                            {
                                var rotaCadastro = "/cadastro-de-empresa-gratis";
                                var email = BNE.BLL.Custom.Helper.ToBase64(objVaga.EmailVaga);
                                urlVisualizacao = string.Format("{0}/{1}?", rotaCadastro, email);
                            }
                            else
                            {
                                urlVisualizacao = string.Format("{0}?idVaga={1}&", SitemapHelper.MontarCaminhoVisualizacaoCurriculo(curriculo.NomeFuncaoPretendida, curriculo.NomeCidade, curriculo.SiglaEstado, curriculo.IdCurriculo).NormalizarURL(), objVaga.IdVaga);
                            }
                            //var url = String.Format("http://{0}/logar/{1}", Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente),
                            //    LoginAutomatico.GerarHashAcessoLoginAutomatico(objVaga.UsuarioFilialPerfil.PessoaFisica.CPF,
                            //    objVaga.UsuarioFilialPerfil.PessoaFisica.DataNascimento, urlVisualizacao));

                            var parametroCandidato = new
                            {
                                Candidato = curriculo.PrimeiroNome,
                                Funcao = curriculo.NomeFuncaoPretendida,
                                Idade = curriculo.Idade,
                                Cidade = BNE.BLL.Custom.Helper.FormatarCidade(curriculo.NomeCidade, curriculo.SiglaEstado),
                                VerCurriculo = string.Format("http://{0}{1}", Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente), urlVisualizacao),
                                utmcampaign = String.Format("cv_{0}", curriculo.IdCurriculo)
                            };
                            sb.Append(parametroCandidato.ToString(linkCandidato));
                        }

                        var parametros = new
                        {
                            Empresa = objVaga.Filial.NomeFantasia.Equals("SINE") ? string.Empty :  objVaga.Filial.RazaoSocial,
                            Cidade = objVaga.Cidade,
                            Funcao = objVaga.Funcao.DescricaoFuncao,
                            Plural = candidaturas.Count > 1 ? "s" : "",
                            PluralEnviar = candidaturas.Count > 1 ? "aram" : "ou",
                            Candidatos = sb.ToString()
                        };
                        var assunto = parametros.ToString(templateAssunto);
                        var mensagem = parametros.ToString(template);
                        var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, trans);

                        try
                        {
                            EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, Enumeradores.CartaEmail.CandidaturaEnvioTodosCurriculos, emailRemetente, objVaga.EmailVaga);
                        }
                        catch (Exception ex)
                        {
                            GerenciadorException.GravarExcecao(ex, "Falha ao enviar mensagem");
                        }

                        candidaturas.ForEach(x => new VagaCandidato(x.IdVagaCandidato).AlterarStatus(Enumeradores.StatusCurriculoVaga.Enviado, trans));

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        GerenciadorException.GravarExcecao(ex);
                        trans.Rollback();
                    }
                }
            }
        }
        #endregion

        #region AlterarStatus
        /// <summary>
        /// Altera o status do registro atual
        /// </summary>
        public void AlterarStatus(Enumeradores.StatusCurriculoVaga statusCurriculoVaga, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga_Candidato", SqlDbType = SqlDbType.Int, Size = 4, Value = _idVagaCandidato },
                    new SqlParameter { ParameterName = "@Idf_Status_Curriculo_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)statusCurriculoVaga },
                };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spalterarstatus, parms);
        }
        #endregion

        #region CurriculoJaCandidatouVaga
        /// <summary>
        ///     Método responsável por verificar se um curriculo já candidatou a uma vaga
        /// </summary>
        /// <param name="objVaga">Identificador da Vaga</param>
        /// <param name="objCurriculo">Identificador de Curriculo</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CurriculoJaCandidatouVaga(Curriculo objCurriculo, Vaga objVaga)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4), /* 0 */
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4) /* 1 */
            };
            parms[0].Value = objVaga.IdVaga;
            parms[1].Value = objCurriculo.IdCurriculo;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectcurriculojacandidatou, parms)) > 0;
        }
        #endregion

        #region AssociarCurriculoVaga
        /// <summary>
        ///     Metodo utilizado para associar um currículo a uma vaga.
        /// </summary>
        public static bool AssociarCurriculoVaga(Curriculo objCurriculo, Vaga objVaga)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        VagaCandidato objVagaCandidato;
                        if (CarregarPorVagaCurriculo(objVaga.IdVaga, objCurriculo.IdCurriculo, out objVagaCandidato))
                        {
                            objVagaCandidato.FlagInativo = false;
                        }
                        else
                        {
                            objVagaCandidato = new VagaCandidato
                            {
                                Curriculo = objCurriculo,
                                Vaga = objVaga,
                                StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.SemEnvio),
                                FlagAutoCandidatura = false
                            };
                        }

                        objVagaCandidato.Save(trans);

                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }
        #endregion

        #region ListarVagasFilialDT
        /// <summary>
        ///     Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListarVagasFilial(int idCurriculo, int idFilial, int? idOrigem, int paginaCorrente, int tamanhoPagina, List<int> listIdFuncoes, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Filial", SqlDbType.Int, 4),
                new SqlParameter("@CurrentPage", SqlDbType.Int, 4),
                new SqlParameter("@PageSize", SqlDbType.Int, 4),
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
            };

            parms[0].Value = idFilial;
            parms[1].Value = paginaCorrente;
            parms[2].Value = tamanhoPagina;
            parms[3].Value = idCurriculo;

            var param = new SqlParameter("@ids", SqlDbType.VarChar, 1600);
            if (listIdFuncoes != null && listIdFuncoes.Count > 0)
                param.Value = string.Join(",", listIdFuncoes.Select(item => item.ToString(CultureInfo.CurrentCulture)).ToArray());
            else
                param.Value = DBNull.Value;

            parms.Add(param);

            var parm = new SqlParameter("@Idf_Origem", SqlDbType.Int, 4);

            if (idOrigem.HasValue)
                parm.Value = idOrigem.Value;
            else
                parm.Value = DBNull.Value;

            parms.Add(parm);

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectvagasporfilial, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
                    dt.Load(dr);

                    if (!dr.IsClosed)
                        dr.Close();
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

        #region Inativar
        public void Inativar()
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        FlagInativo = true;
                        this.StatusCandidatura = new BLL.StatusCandidatura((int)Enumeradores.StatusCandidatura.ExcluidoProcessoSelecao);
                        Save(trans);

                        var objPessoaFisica = new PessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(Curriculo));
                        var emailDestinatario = objPessoaFisica.EmailPessoa;

                        if (!string.IsNullOrEmpty(emailDestinatario)) //Se o candidato tem email 
                        {
                            //TODO: Analisar performance: Método estatico apenas para verificar se a vaga é confidencial ou não
                            var dtoVaga = DTO.Vaga.CarregarVaga(Vaga.IdVaga, trans); //Recupera um objeto com todos os dados da vaga

                            if (!dtoVaga.FlagConfidencial) //Se a vaga não é confidencial
                            {
                                if (Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.EnviaCartaAgradecimentoCandidatura, trans))) //Se é para enviar a carta de agradecimento para todos os clientes
                                {
                                    string cartaFilial;
                                    ParametroFilial objParametroFilial;
                                    if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.EnviaCartaAgradecimentoCandidatura, new Filial(dtoVaga.IdEmpresa), out objParametroFilial, trans))
                                        cartaFilial = objParametroFilial.ValorParametro; //Recuperando o valor setado no banco
                                    else //Se não possuir parametro específico, recupera a carta padrão
                                        cartaFilial = CartaEmail.RecuperarConteudo(Enumeradores.CartaEmail.AgradecimentoCandidatura, trans);

                                    if (!string.IsNullOrEmpty(cartaFilial)) //Se tiver valor manda a carta
                                    {
                                        string templateAssunto;
                                        var templateMensagem = CartaEmail.RetornarConteudoEmpresa(Enumeradores.CartaEmail.AgradecimentoCandidatura, cartaFilial, dtoVaga.CNPJEmpresa, dtoVaga.NomeEmpresa, out templateAssunto);

                                        //Criando os parametros da mensagem e montando a partir do template
                                        var parametros = new
                                        {
                                            NomeCandidato = objPessoaFisica.NomeCompleto,
                                            NomeFuncao = dtoVaga.DescricaoFuncao,
                                            dtoVaga.NomeSelecionadora,
                                            NomeEmpresaSelecionadora = dtoVaga.NomeEmpresa,
                                            DadosVaga = CartaEmail.MontarExtratoVaga(dtoVaga)
                                        };
                                        var descricaoMensagem = parametros.ToString(templateMensagem);
                                        var descricaoAssunto = parametros.ToString(templateAssunto);

                                        var emailRemetente = dtoVaga.EmailVaga;

                                        EmailSenderFactory
                                            .Create(TipoEnviadorEmail.Fila)
                                            .Enviar(descricaoAssunto, descricaoMensagem, Enumeradores.CartaEmail.AgradecimentoCandidatura, emailRemetente, emailDestinatario, trans);
                                    }
                                }
                            }
                        }

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

        public static void Inativar(Vaga objVaga, List<int> list)
        {
            foreach (var idCurriculo in list)
            {
                try
                {
                    VagaCandidato objVagaCandidato;
                    if (CarregarPorVagaCurriculo(objVaga.IdVaga, idCurriculo, out objVagaCandidato))
                        objVagaCandidato.Inativar();
                }
                catch (Exception ex)
                {
                    GerenciadorException.GravarExcecao(ex);
                }
            }
        }
        #endregion

        #region Candidatar
        public static bool Candidatar(Curriculo objCurriculo, Vaga objVaga, Origem objOrigem,
            List<VagaCandidatoPergunta> listPerguntas, string descricaoIP, bool siteSTC,
            bool siteSTCCandidaturaLivre, bool avulsaMobile, Enumeradores.OrigemCandidatura origemCandidatura,
            out int? quantidadeCandidaturasGratis)
        {
            var retorno = false;

            quantidadeCandidaturasGratis = null;

            IntencaoFilial objIntencaoFilial;
            objVaga.CompleteObject();
            objCurriculo.CompleteObject();
            if (!IntencaoFilial.CarregarPorFilialCurriculo(objCurriculo.IdCurriculo, objVaga.Filial.IdFilial, out objIntencaoFilial))
            {
                objIntencaoFilial = new IntencaoFilial { Curriculo = objCurriculo, Filial = new Filial(objVaga.Filial.IdFilial) };
            }
            objIntencaoFilial.FlagInativo = false;

            VagaCandidato objVagaCandidato;
            if (!CarregarPorVagaCurriculo(objVaga.IdVaga, objCurriculo.IdCurriculo, out objVagaCandidato))
            {
                objVagaCandidato = new VagaCandidato { Curriculo = objCurriculo, Vaga = objVaga, FlagInativo = false };

                foreach (var objVagaCandidatoPergunta in listPerguntas)
                {
                    objVagaCandidatoPergunta.VagaCandidato = objVagaCandidato;
                }

                objVagaCandidato.Salvar(objVaga, objCurriculo, objOrigem, descricaoIP, objIntencaoFilial, listPerguntas, siteSTC, siteSTCCandidaturaLivre, avulsaMobile, origemCandidatura, out quantidadeCandidaturasGratis);
            }
            else
            {
                if (objVagaCandidato != null)
                {
                    if (objVagaCandidato.FlagAutoCandidatura == null)
                        objVagaCandidato.FlagAutoCandidatura = false;
                    if (Convert.ToBoolean(objVagaCandidato.FlagAutoCandidatura) && !objCurriculo.FlagVIP)
                        objVagaCandidato.FlagAutoCandidatura = false;
                    objVagaCandidato.Save();
                }
                else
                {
                    CarregarPorVagaCurriculo(objVaga.IdVaga, objCurriculo.IdCurriculo, out objVagaCandidato);
                    if (objVagaCandidato != null && !objCurriculo.FlagVIP)
                    {
                        if (Convert.ToBoolean(objVagaCandidato.FlagAutoCandidatura))
                            objVagaCandidato.FlagAutoCandidatura = false;
                        objVagaCandidato.Save();
                    }
                }
            }

            retorno = true;

            return retorno;
        }
        #endregion

        #region QuantidadeVagaCandidato
        public static int QuantidadeCandidaturas(Vaga objVaga)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga}
            };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spquantidadecandidaturatotalporvaga, parms));
        }
        #endregion

        #region ListaIdsCurriculosCandidatados
        /// <summary>
        ///     Retorna uma lista com os ids dos currículos já candidatados
        /// </summary>
        /// <param name="objVaga"></param>
        /// <returns></returns>
        public static List<int> ListaIdsCurriculosCandidatados(Vaga objVaga)
        {
            var lista = new List<int>();

            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga}
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sptodoscurriculoscandidatados, parms))
            {
                while (dr.Read())
                    lista.Add(Convert.ToInt32(dr["Idf_Curriculo"]));
            }

            return lista;
        }
        #endregion

        #region RecuperarVagasComCandidaturasAguardandoEnvio
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDictionary<int, List<DTO.Candidatura>> RecuperarVagasComCandidaturasAguardandoEnvio()
        {
            var parms = new List<SqlParameter>
                        {
                            new SqlParameter("@Idf_Status_Curriculo_Vaga", SqlDbType.Int, 4)
                        };
            parms[0].Value = (int)Enumeradores.StatusCurriculoVaga.AguardoEnvio;

            var dicionario = new Dictionary<int, List<DTO.Candidatura>>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spcandidaturasparanotificar, parms))
            {
                while (dr.Read())
                {
                    var vaga = Convert.ToInt32(dr["Idf_Vaga"]);
                    var funcao = dr["Des_Funcao"].ToString();
                    var cidade = dr["Nme_Cidade"].ToString();
                    var siglaEstado = dr["Sig_Estado"].ToString();
                    var nome = dr["Nme_Pessoa"].ToString();
                    var idade = Helper.CalcularIdade(Convert.ToDateTime(dr["Dta_Nascimento"]));
                    var idCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
                    var idVagaCandidato = Convert.ToInt32(dr["Idf_Vaga_Candidato"]);

                    if (!dicionario.ContainsKey(vaga))
                    {
                        dicionario.Add(vaga, new List<DTO.Candidatura>());
                    }

                    dicionario[vaga].Add(new DTO.Candidatura(funcao, cidade, siglaEstado, nome, idade, idCurriculo, idVagaCandidato));
                }
            }

            return dicionario;
        }
        #endregion

        #region SalvarVisualizacaoCandidato
        public static void SalvarVisualizacaoCandidato(int idCurriculo, int idVaga)
        {
            var objVagaCandidato = new VagaCandidato();

            if (VagaCandidato.CarregarPorVagaCurriculo(idVaga, idCurriculo, out objVagaCandidato) && !objVagaCandidato.DataVisualizacao.HasValue)
            {
                objVagaCandidato.DataVisualizacao = DateTime.Now;
                objVagaCandidato.Save();
            }
        }
        #endregion

        #endregion

    }

}