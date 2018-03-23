//-- Data: 31/07/2013 15:07
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.BLL.Enumeradores;

namespace BNE.BLL
{
    public partial class CurriculoQuemMeViu // Tabela: BNE_Curriculo_Quem_Me_Viu
    {

        #region Consultas

        #region Spselectcurriculosvisualizados
        private const string Spselectcurriculosvisualizados = @"
        SELECT  q.Idf_Curriculo, q.Dta_Quem_Me_Viu, pf.nme_pessoa
        FROM    BNE_Curriculo_Quem_Me_Viu q WITH(NOLOCK)
		left join tab_usuario_filial_perfil ufp with(nolock) on ufp.idf_usuario_filial_perfil = q.idf_usuario_filial_perfil
		left join tab_Pessoa_fisica pf with(nolock) on pf.idf_Pessoa_fisica = ufp.idf_pessoa_fisica
        WHERE   q.Idf_Filial = @Idf_Filial 
                AND DATEADD(DAY,@Dias_Visualizacao ,q.Dta_Quem_Me_Viu) >= GETDATE() 
            order by dta_quem_me_viu desc ";
        #endregion

        #region Spverificasalvarnovoacesso
        private const string Spverificasalvarnovoacesso = @"
        SELECT  COUNT(*)
        FROM    BNE_Curriculo_Quem_Me_Viu
        WHERE   DATEDIFF(DAY,Dta_Quem_Me_Viu,GETDATE()) < 1
                AND Idf_Filial = @Idf_Filial 
                AND Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region Spselectvisualizacaoporcurriculo
        private const string Spselectvisualizacaoporcurriculo = @"
       	 DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
        SELECT ROW_NUMBER() OVER (ORDER BY CV.Dta_Quem_Me_Viu DESC) AS RowID,
                F.Idf_Filial ,
                CV.Idf_Curriculo_Quem_Me_Viu,
                CASE    WHEN ( C.Flg_VIP = 0 )--SE NÃO FOR VIP 
                            THEN ''Informação disponível somente para cliente ''
                        ELSE F.Raz_Social
                END AS Raz_Social ,
                Dta_Quem_Me_Viu,
                CI.Idf_Cidade,
                ( CI.Nme_Cidade + ''/'' + CI.Sig_Estado ) AS Cidade_Estado ,
                ( 
                    CASE 
                        WHEN ( C.Flg_VIP = 0 ) THEN 1 ELSE 0
                    END 
                ) AS Img_VIP_Visible
        FROM    BNE_Curriculo_Quem_Me_Viu CV WITH(NOLOCK)
                JOIN BNE_Curriculo C WITH(NOLOCK) ON CV.Idf_Curriculo = C.Idf_Curriculo
                JOIN TAB_Filial F WITH(NOLOCK) ON CV.Idf_Filial = F.Idf_Filial
                JOIN TAB_Endereco E WITH(NOLOCK) ON F.Idf_Endereco = E.Idf_Endereco
                JOIN plataforma.TAB_Cidade CI WITH(NOLOCK) ON E.Idf_Cidade = CI.Idf_Cidade
                LEFT JOIN bne.bne_curriculo_nao_visivel_filial cvf on cvf.idf_filial = f.idf_filial and cvf.idf_curriculo ='  + CONVERT(VARCHAR, @Idf_Curriculo) + '
        WHERE   cvf.idf_filial is null 
                AND CV.Idf_Curriculo ='  + CONVERT(VARCHAR, @Idf_Curriculo)
				
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)

        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)
        ";
        #endregion

        #region Spselectvisualizacaoporcurriculoadministrador
        private const string Spselectvisualizacaoporcurriculoadministrador = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
        SELECT ROW_NUMBER() OVER (ORDER BY CV.Dta_Quem_Me_Viu DESC) AS RowID,
                F.Idf_Filial ,
                CV.Idf_Curriculo_Quem_Me_Viu ,
                F.Raz_Social ,
                Dta_Quem_Me_Viu,
                ( CI.Nme_Cidade + ''/'' + CI.Sig_Estado ) AS Cidade_Estado
        FROM    BNE_Curriculo_Quem_Me_Viu CV WITH(NOLOCK)
                JOIN BNE_Curriculo C WITH(NOLOCK) ON CV.Idf_Curriculo = C.Idf_Curriculo
                JOIN TAB_Filial F WITH(NOLOCK) ON CV.Idf_Filial = F.Idf_Filial
                JOIN TAB_Endereco E WITH(NOLOCK) ON F.Idf_Endereco = E.Idf_Endereco
                JOIN plataforma.TAB_Cidade CI WITH(NOLOCK) ON E.Idf_Cidade = CI.Idf_Cidade
        WHERE   CV.Idf_Curriculo = ' + CONVERT(VARCHAR, @Idf_Curriculo)
				if(@Dta_Inicio is not null and @Dta_Fim is not null)
			 begin
			 SET @iSelect = @iSelect + ' AND  CV.Dta_Quem_Me_Viu BETWEEN convert(date, ''' + @Dta_Inicio + ''')
                                   AND     '''+ @Dta_Fim +''''
			 end
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)

        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)
        ";
        #endregion

        #region RelatorioVisualizacaoPorCurriculo
        private const string SpRelatorioVisualizacaoPorCurriculo = @"
        SELECT qm.Idf_Curriculo as Idf_Curriculo, Count(qm.Idf_Curriculo) as Total FROM BNE_Curriculo c with (nolock)
        INNER JOIN BNE_Curriculo_Quem_Me_Viu qm with (nolock) ON qm.Idf_Curriculo = c.Idf_Curriculo
        WHERE c.Flg_Vip = 0 
        AND c.Flg_Inativo = 0 
        AND c.Idf_Situacao_Curriculo NOT IN (5,6,12) 
        AND qm.Dta_Quem_Me_Viu > @Dta_Quem_Me_Viu 
        GROUP BY qm.Idf_Curriculo
        HAVING Count(qm.Idf_Curriculo) > @Qtd_Visualizacao";
        #endregion

        #region Spcountcurriculovisualizado
        private const string Spcountcurriculovisualizado = @"
        SELECT  COUNT(QM.Idf_Curriculo)
        FROM    BNE_Curriculo_Quem_Me_Viu QM WITH(NOLOCK)
                JOIN BNE_Curriculo C WITH(NOLOCK) ON QM.Idf_Curriculo = C.Idf_Curriculo
                 LEFT JOIN bne_curriculo_nao_visivel_filial cvf on cvf.idf_curriculo = c.idf_curriculo and cvf.Idf_Filial = qm.idf_filial
        WHERE   C.Idf_Curriculo = @Idf_Curriculo
                and cvf.idf_filial is null ";
        #endregion

        #region SpselectvisualizacaoporcurriculoSite
        private const string SpselectvisualizacaoporcurriculoSite = @"
       	 DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
        SELECT ROW_NUMBER() OVER (ORDER BY CV.Dta_Quem_Me_Viu DESC) AS RowID,
                F.Idf_Filial ,
                CV.Idf_Curriculo_Quem_Me_Viu,
                C.Flg_VIP,
                 F.Raz_Social,
                Dta_Quem_Me_Viu,
                CI.Nme_Cidade,
                 CI.Sig_Estado,
                f.Qtd_Funcionarios,
				f.dta_cadastro,
				f.num_ddd_comercial,
				e.Des_Bairro,
                f.num_cnpj,
				f.num_comercial,
				vagas.TotalVagas,
				visualizacao.VisualizacaoEmpresa,
                area.Des_Area_BNE,
                pf.Vlr_Parametro
           FROM    bne.BNE_Curriculo_Quem_Me_Viu CV WITH(NOLOCK)
                JOIN bne.BNE_Curriculo C WITH(NOLOCK) ON CV.Idf_Curriculo = C.Idf_Curriculo
                JOIN bne.TAB_Filial F WITH(NOLOCK) ON CV.Idf_Filial = F.Idf_Filial
                JOIN bne.TAB_Endereco E WITH(NOLOCK) ON F.Idf_Endereco = E.Idf_Endereco
                JOIN plataforma.TAB_Cidade CI WITH(NOLOCK) ON E.Idf_Cidade = CI.Idf_Cidade
                LEFT JOIN bne.bne_curriculo_nao_visivel_filial cvf with(nolock) on cvf.idf_filial = f.idf_filial and cvf.idf_curriculo ='  + CONVERT(VARCHAR, @Idf_Curriculo) + '
                     outer apply (select count(1) as VisualizacaoEmpresa from 
								bne.bne_curriculo_visualizacao with(nolock)
								where idf_Filial = F.idf_Filial) as visualizacao
				outer apply (select count(1) as totalVagas from bne.bne_vaga with(nolock)
								where idf_filial = f.idf_Filial
								and flg_auditada = 1) as vagas
									JOIN plataforma.TAB_CNAE_Sub_Classe csc WITH (NOLOCK) ON f.Idf_CNAE_Principal = csc.Idf_CNAE_Sub_Classe
				JOIN plataforma.TAB_CNAE_Classe cl WITH (NOLOCK) ON csc.Idf_CNAE_Classe = cl.Idf_CNAE_Classe
                JOIN plataforma.TAB_CNAE_Grupo gp WITH (NOLOCK) ON cl.Idf_CNAE_Grupo = gp.Idf_CNAE_Grupo
                JOIN plataforma.TAB_CNAE_Divisao d WITH (NOLOCK) ON gp.Idf_CNAE_Divisao = d.Idf_CNAE_Divisao
                JOIN plataforma.TAB_Area_BNE area WITH (NOLOCK) ON d.Idf_Area_BNE = area.Idf_Area_BNE
                left join bne.tab_parametro_filial pf with(nolock) on pf.idf_Filial = f.idf_Filial
				 and pf.idf_parametro = '+ Convert(varchar, @Idf_Parametro) +'
        WHERE   cvf.idf_filial is null 
                AND CV.Idf_Curriculo ='  + CONVERT(VARCHAR, @Idf_Curriculo)
				
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)

        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)
        ";

        #endregion
        #endregion

        #region Métodos

        #region ListarCurriculoVisualizados
        /// <summary>
        /// Método utilizado para retornar os curriculo vizualidados pela empresa, levando em conta se o currículo foi atualizado após a vizualização ou não.
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static List<Tuple<int, string, DateTime>> ListarCurriculoVisualizados(int idFilial)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial },
                    new SqlParameter {ParameterName = "@Dias_Visualizacao", SqlDbType = SqlDbType.Int, Size=4, Value = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PeriodoReVisualizacaoCurriculo)) }
                };

            var listCurriculos = new List<Tuple<int, string, DateTime>>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectcurriculosvisualizados, parms))
            {
                while (dr.Read())
                {
                    listCurriculos.Add(new Tuple<int, string, DateTime>(Convert.ToInt32(dr["Idf_Curriculo"]),
                        dr["nme_pessoa"] != DBNull.Value ? dr["nme_pessoa"].ToString() : string.Empty,
                        Convert.ToDateTime(dr["Dta_Quem_Me_Viu"])));
                }
            }

            return listCurriculos;
        }
        #endregion

        #region PodeSalvarSalvarQuemMeViu
        /// <summary>
        /// Metodo responsavel por verificar se foi é possível salvar um novo quem me viu
        /// </summary>
        /// <returns></returns>
        public static bool PodeSalvarSalvarQuemMeViu(Filial objFilial, Curriculo objCurriculo, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial },
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo },
                };

            //Se o retorno for 0 (zero) então é possível enviar uma mensagem para o candidato.
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spverificasalvarnovoacesso, parms)).Equals(0);
        }
        #endregion

        #region SalvarQuemMeViu
        /// <summary>
        /// Método responsável por salvar o acesso de uma filial a um currículo e dispara uma notificação de visualização para o CV
        /// </summary>
        /// <param name="objFilial">Filial que está acessando o CV</param>
        /// <param name="objCurriculo">Currículo que está sendo visualizado</param>
        public static CurriculoQuemMeViu SalvarQuemMeViuEmail(Filial objFilial, Curriculo objCurriculo)
        {
            return SalvarQuemMeViu(objFilial, objCurriculo, OrigemQuemMeViu.Email, null);
        }
        public static CurriculoQuemMeViu SalvarQuemMeViuSite(Filial objFilial, Curriculo objCurriculo, int? idUsuarioFilialPerfil)
        {
            return SalvarQuemMeViu(objFilial, objCurriculo, OrigemQuemMeViu.Site, idUsuarioFilialPerfil);
        }
        private static CurriculoQuemMeViu SalvarQuemMeViu(Filial objFilial, Curriculo objCurriculo, OrigemQuemMeViu origemQuemMeViu, int? idUsuarioFilialPerfil)
        {
            CurriculoQuemMeViu objCurriculoQuemMeViu = null;
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (PodeSalvarSalvarQuemMeViu(objFilial, objCurriculo, trans))
                        {
                            objCurriculoQuemMeViu = new CurriculoQuemMeViu
                            {
                                DataQuemMeViu = DateTime.Now,
                                Filial = objFilial,
                                Curriculo = objCurriculo,
                                FlagInativo = false,
                                OrigemCurriculoQuemMeViu = new OrigemCurriculoQuemMeViu((int)origemQuemMeViu),
                                UsuarioFilialPerfil = idUsuarioFilialPerfil.HasValue ? new UsuarioFilialPerfil(idUsuarioFilialPerfil.Value) : null

                            };
                            objCurriculoQuemMeViu.Save(trans);

                            if (origemQuemMeViu == OrigemQuemMeViu.Site)
                            {
                                MensagemCS.EnviarNotificacaoVisualizacaoCurriculo(new PessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(objCurriculo)), objFilial.NomeFantasia, objCurriculo.VIP(), trans);
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
            return objCurriculoQuemMeViu;
        }
        #endregion

        #region RecuperarQuemMeViu
        /// <summary>
        /// </summary>
        /// <param name="idCurriculo">Id do Curriculo da pessoa fisica logada</param>
        /// <param name="paginaCorrente">pagina corrente</param>
        /// <param name="tamanhoPagina">Tamanho de registros por pagina</param>
        /// <param name="totalRegistros">Total de registros encontrados</param>
        /// <returns>DataTable com os registros encontrados</returns>
        public static DataTable RecuperarQuemMeViu(int idCurriculo, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo},
                    new SqlParameter{ ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente},
                    new SqlParameter{ ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina},
                };

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectvisualizacaoporcurriculo, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
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

        #region RecuperarQuemMeViuAdministrador
        /// <summary>
        /// </summary>
        /// <param name="idCurriculo">Id do Curriculo da pessoa fisica logada</param>
        /// <param name="paginaCorrente">pagina corrente</param>
        /// <param name="tamanhoPagina">Tamanho de registros por pagina</param>
        /// <param name="totalRegistros">Total de registros encontrados</param>
        /// <returns>DataTable com os registros encontrados</returns>
        public static DataTable RecuperarQuemMeViuAdministrador(int idCurriculo, int paginaCorrente, int tamanhoPagina, out int totalRegistros, DateTime? DtaInicio = null, DateTime? DtaFim = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo},
                    new SqlParameter{ ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente},
                    new SqlParameter{ ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina},
                };

            if (DtaInicio.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Dta_Inicio", SqlDbType = SqlDbType.VarChar, Value = DtaInicio.Value.ToString("MM/dd/yyyy") });
            else
                parms.Add(new SqlParameter { ParameterName = "@Dta_Inicio", SqlDbType = SqlDbType.VarChar, Value = DBNull.Value });

            if (DtaFim.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Dta_Fim", SqlDbType = SqlDbType.VarChar, Value = Convert.ToDateTime(DtaFim.Value).AddHours(23).AddMinutes(59).ToString("MM/dd/yyyy HH:mm") });
            else
                parms.Add(new SqlParameter { ParameterName = "@Dta_Fim", SqlDbType = SqlDbType.VarChar, Value = DBNull.Value });

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectvisualizacaoporcurriculoadministrador, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
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

        #region QuantidadeVisualizacaoCurriculo
        public static int QuantidadeVisualizacaoCurriculo(Curriculo objCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo}
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spcountcurriculovisualizado, parms));
        }
        #endregion

        public class RelatorioQuemMeViuModel
        {
            public int IdCurriculo { get; set; }
            public int Total { get; set; }
        }

        public static IEnumerable<RelatorioQuemMeViuModel> RelatorioQuantidadePorCurriculo(DateTime dataInicial, int visualizacoesMaiorQue = 0)
        {
            visualizacoesMaiorQue = Math.Max(0, visualizacoesMaiorQue);
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Dta_Quem_Me_Viu", SqlDbType = SqlDbType.Date, Value = dataInicial},
                    new SqlParameter{ ParameterName = "@Qtd_Visualizacao", SqlDbType = SqlDbType.Int, Size = 4, Value = visualizacoesMaiorQue}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRelatorioVisualizacaoPorCurriculo, parms))
            {
                while (dr.Read())
                {
                    yield return new RelatorioQuemMeViuModel
                    {
                        IdCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]),
                        Total = Convert.ToInt32(dr["Total"])
                    };
                }
            }

        }
        #endregion


        #region RecuperarQuemMeViu
        /// <summary>
        /// </summary>
        /// <param name="idCurriculo">Id do Curriculo da pessoa fisica logada</param>
        /// <param name="paginaCorrente">pagina corrente</param>
        /// <param name="tamanhoPagina">Tamanho de registros por pagina</param>
        /// <param name="totalRegistros">Total de registros encontrados</param>
        /// <returns>DataTable com os registros encontrados</returns>
        public static DataTable RecuperarQuemMeViuSite(int idCurriculo, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo},
                    new SqlParameter{ ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente},
                    new SqlParameter{ ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina},
                    new SqlParameter{ ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)Enumeradores.Parametro.EmpresaConfidencial},
                };

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpselectvisualizacaoporcurriculoSite, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
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
    }
}