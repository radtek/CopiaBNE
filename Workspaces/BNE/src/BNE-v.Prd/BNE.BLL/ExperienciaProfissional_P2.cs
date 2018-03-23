//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using BNE.BLL.Custom;

namespace BNE.BLL
{
    public partial class ExperienciaProfissional // Tabela: BNE_Experiencia_Profissional
    {

        #region Consultas

        #region SELPORCPF
        private const string SELPORCPF =
                    @"SELECT *
                    FROM BNE_Experiencia_Profissional	
                    WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                        AND (@Flg_Importado IS NULL OR Flg_Importado = @Flg_Importado)";
        #endregion

        #region SPSELECTPORPESSOAFISICA
        private const string SPSELECTPORPESSOAFISICA = @"
                                SELECT  *,
                                        CASE 
											WHEN E.Dta_Demissao IS NULL THEN 1
											WHEN E.Dta_Demissao IS NOT NULL THEN 2
										END AS 'Ordem'
                                FROM    BNE_Experiencia_Profissional E
                                        INNER JOIN BNE.BNE_Curriculo C ON C.Idf_Pessoa_Fisica = E.Idf_Pessoa_Fisica
                                        LEFT JOIN plataforma.TAB_Area_BNE AB ON E.Idf_Area_BNE = AB.Idf_Area_BNE
                                        LEFT JOIN plataforma.TAB_Funcao F ON E.Idf_Funcao = F.Idf_Funcao
                                WHERE   E.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                                        AND E.Flg_Inativo = 0
                                        ORDER BY Ordem ASC, E.Dta_Demissao DESC, E.Dta_Admissao DESC";

        #endregion

        #region SPSELECTEXPERIENCIA
        private const string SPSELECTEXPERIENCIA = @"
                                SELECT  TOP 5 E.Raz_Social ,
                                        AB.Des_Area_BNE ,
                                        E.Idf_Experiencia_Profissional ,
                                        Dta_Admissao ,
                                        Dta_Demissao ,
                                        ISNULL(F.Des_Funcao,E.Des_Funcao_Exercida) AS Des_Funcao,
                                        E.Des_Atividade ,
                                        C.Vlr_Ultimo_Salario ,
                                        E.Vlr_Salario,
                                        CASE 
											WHEN E.Dta_Demissao IS NULL THEN 1
											WHEN E.Dta_Demissao IS NOT NULL THEN 2
										END AS 'Ordem'
                                FROM    BNE_Experiencia_Profissional E
                                        INNER JOIN BNE.BNE_Curriculo C ON C.Idf_Pessoa_Fisica = E.Idf_Pessoa_Fisica
                                        LEFT JOIN plataforma.TAB_Area_BNE AB ON E.Idf_Area_BNE = AB.Idf_Area_BNE
                                        LEFT JOIN plataforma.TAB_Funcao F ON E.Idf_Funcao = F.Idf_Funcao
                                WHERE   E.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                                        AND E.Flg_Inativo = 0
                                        ORDER BY Ordem ASC, E.Dta_Demissao DESC, E.Dta_Admissao DESC";

        #endregion

        #region SPSELECTEXPERIENCIAPORFUNCAO
        private const string SPSELECTEXPERIENCIAPORFUNCAO = @"
                        SELECT  *
                        FROM    BNE_Experiencia_Profissional E  with(nolock)
                        WHERE   E.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                                AND E.Idf_Funcao = @Idf_Funcao
                                AND E.Flg_Inativo = 0";

        #endregion

        #region SPSELECTEXPERIENCIAS
        private const string SPSELECTEXPERIENCIAS = @"
        SELECT  *,
                CASE 
					WHEN E.Dta_Demissao IS NULL THEN 1
					WHEN E.Dta_Demissao IS NOT NULL THEN 2
				END AS 'Ordem'
        FROM    BNE_Experiencia_Profissional E WITH(NOLOCK)
        WHERE   E.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                AND E.Flg_Inativo = 0
                ORDER BY Ordem ASC, E.Dta_Demissao DESC, E.Dta_Admissao DESC
        ";
        #endregion

        #region SPSELECTCURRICULOSEMEXPERIENCIA

        private const string SPSELECTCURRICULOSEMEXPERIENCIA = @"
        SELECT pf.Idf_Pessoa_Fisica,
        pf.Nme_Pessoa, 
        pf.Eml_Pessoa,
        pf.Num_DDD_Celular,
        pf.Num_Celular 
        FROM
        bne.TAB_Pessoa_Fisica as pf with(nolock) 
        inner join bne.BNE_Curriculo as c with(nolock) on c.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
        outer apply ( select top 1 idf_funcao_Pretendida 
						from bne.BNE_Funcao_Pretendida as f with(nolock) 
							 where f.Idf_Curriculo = c.Idf_Curriculo 
							      AND f.Idf_Funcao = 5748 
						)as FunPre
        left join bne.BNE_Experiencia_Profissional as ep with(nolock) on ep.Idf_Pessoa_Fisica = c.Idf_Pessoa_Fisica
        WHERE c.Idf_Situacao_Curriculo NOT IN (6,7,8,11,12)
        AND c.Flg_Inativo = 0
        and ep.Idf_Pessoa_Fisica is null
		and FunPre.idf_funcao_Pretendida is null
        and c.Dta_Cadastro  between @Dta_Inicial and @Dta_Fim";
        #endregion

        #region SPSELECTCURRICULOCOMEXPERIENCIAFRACAMEDIA

        private const string SPSELECTCURRICULOCOMEXPERIENCIAFRACAMEDIA = @"
        select pf.Idf_Pessoa_Fisica,
        pf.Eml_Pessoa, 
        pf.Nme_Pessoa, 
        pf.Num_DDD_Celular,
        pf.Num_Celular, 
        Expe.Raz_Social,
        Expe.Des_Atividade
        from TAB_Pessoa_Fisica as pf with(nolock)
        inner join BNE_Curriculo as c with(nolock) on c.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
        outer apply (select top 1 idf_funcao_Pretendida 
				          from BNE_Funcao_Pretendida as f with(nolock)
				             where f.Idf_Curriculo = c.Idf_Curriculo 
						           and f.Idf_Funcao = 5748) as FunPre
        outer apply (select top 5 ep.Idf_Pessoa_Fisica, ep.Des_Atividade, ep.Raz_Social
				        FROM BNE_Experiencia_Profissional as ep with(nolock)
					        where ep.Idf_Pessoa_Fisica = c.Idf_Pessoa_Fisica) as Expe
        where c.Idf_Situacao_Curriculo NOT IN (6,7,8,11,12) 
        and Expe.Idf_Pessoa_Fisica is not null
        and FunPre.idf_funcao_Pretendida is null
        and c.Flg_Inativo = 0
        and c.Dta_Cadastro between @Dta_Inicial and @Dta_Fim 
        and len(Expe.Des_Atividade) < 140";
        #endregion

        #region SPSELECTULTIMAEXPERIENCIAPROFISSIONAL

        private const string SPSELECTULTIMAEXPERIENCIAPROFISSIONAL = @"
        SELECT TOP 1 * 
        --Idf_Experiencia_Profissional,
        --Raz_Social,Des_Atividade 
        FROM BNE.BNE_Experiencia_Profissional  with(nolock)
        WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
        ORDER BY Dta_Admissao DESC
        ";

        #endregion

        #region [spInativarExperienciaPorPessoaFisica]
        private const string spInativarExperienciaPorPessoaFisica = @"update bne_experiencia_profissional set flg_inativo = 1 where idf_pessoa_fisica = @Idf_Pessoa_Fisica";
        #endregion

        #region SPSELECTULTIMAEXPERIENCIAPROFISSIONAL

        private const string SPDELETEPORPESSOA = @"
        DELETE FROM BNE.BNE_Experiencia_Profissional
        WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
        ";

        #endregion

        #endregion

        #region Métodos

        public static void ExcluirExperienciasDePessoa(int idPessoaFisica, SqlTransaction trans)
        {
            if (idPessoaFisica <= 0)
                throw new ArgumentException("idPessoaFisica tem que ser maior que zero");

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETEPORPESSOA, parms);
        }

        #region CarregarUltimaExperienciaProfissional

        /// <summary>
        /// Carregar a última experiência profissional do currículo
        /// </summary>
        /// <returns></returns>
        public static ExperienciaProfissional CarregarUltimaExperienciaProfissional(int idPessoaFisica)
        {
            List<ExperienciaProfissional> listExperiencia = null;
            ExperienciaProfissional objExperiencia = new ExperienciaProfissional();

            var parms = new List<SqlParameter> { new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4) };
            parms[0].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTULTIMAEXPERIENCIAPROFISSIONAL, parms))
            {
                ExperienciaProfissional objExp = new ExperienciaProfissional();

                while (SetInstance_NotDispose(dr, objExperiencia))
                {
                    //listExperiencia.Add(objExp);
                }
            }

            return objExperiencia;
        }

        #endregion

        #region ListarCurriculosComExperienciaProfissionalFraca

        // <summary>
        /// Método responsável por retornar um datareader com os dados do candidato com as Experiencias Profissionais fraca ou media
        /// </summary>
        /// <param name="numeroDias">indica o número de dias que o curriculo foi cadastrado</param>
        /// <returns></returns>
        public static DataTable ListarCurriculosComExperienciaProfissionalFraca(int numeroDias, string DataInicio, string DataFim)
        {
            var parms = new List<SqlParameter> { new SqlParameter("@NumeroDias", SqlDbType.Int, 4),
            new SqlParameter("@Dta_Inicial", SqlDbType.DateTime,8),
            new SqlParameter("@Dta_Fim", SqlDbType.DateTime,8)};
            parms[0].Value = numeroDias;
            parms[1].Value = DataInicio;
            parms[2].Value = DataFim;

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTCURRICULOCOMEXPERIENCIAFRACAMEDIA, parms).Tables[0];
        }
        #endregion

        #region ListarCurriculosSemExperienciaProfissional
        public static DataTable ListarCurriculosSemExperienciaProfissional(int numeroDias, string DataInicio, string DataFim)
        {
            var parms = new List<SqlParameter> { new SqlParameter("@NumeroDias", SqlDbType.Int, 4),
            new SqlParameter("@Dta_Inicial", SqlDbType.DateTime,8),
            new SqlParameter("@Dta_Fim", SqlDbType.DateTime,8)};
            parms[0].Value = numeroDias;
            parms[1].Value = DataInicio;
            parms[2].Value = DataFim;


            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTCURRICULOSEMEXPERIENCIA, parms).Tables[0];
        }
        #endregion

        #region ListarExperienciaPorPessoaFisica
        /// <summary>
        /// Método responsável por retornar um datareader com todas as instâncias de ExperienciaProfissional
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static IDataReader ListarExperienciaPorPessoaFisica(int idPessoaFisica)
        {
            var parms = new List<SqlParameter> { new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4) };
            parms[0].Value = idPessoaFisica;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTEXPERIENCIA, parms);
        }
        #endregion

        #region CarregarExperienciaPorPessoaFisica
        /// <summary>
        /// Método responsável por retornar um list com todas as instâncias de ExperienciaProfissional
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static List<ExperienciaProfissional> CarregarExperienciaPorPessoaFisica(int idPessoaFisica)
        {
            var listExperiencia = new List<ExperienciaProfissional>();

            var parms = new List<SqlParameter> { new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4) };
            parms[0].Value = idPessoaFisica;

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESSOAFISICA, parms))
            {
                var objExp = new ExperienciaProfissional();

                while (SetInstance_NotDispose(dr, objExp) &&
                        AreaBNE.SetInstance_NotDispose(dr, objExp.AreaBNE))
                {
                    if (string.IsNullOrEmpty(objExp.DescricaoFuncaoExercida))
                        Funcao.SetInstance_NotDispose(dr, objExp.Funcao);
                    listExperiencia.Add(objExp);
                    objExp = new ExperienciaProfissional();
                }
            }

            return listExperiencia;
        }
        #endregion

        #region ListarExperienciaPorPessoaFisicaDT
        /// <summary>
        /// Método responsável por retornar um datareader com todas as instâncias de ExperienciaProfissional
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns>Data Table</returns>
        public static DataTable ListarExperienciaPorPessoaFisicaDT(int idPessoaFisica)
        {
            var parms = new List<SqlParameter> { new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4) };
            parms[0].Value = idPessoaFisica;

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTEXPERIENCIA, parms).Tables[0];
        }
        #endregion

        #region ListarExperienciasProfissinaisPorFuncao
        /// <summary>
        /// Método responsável por retornar um datareader com todas as instâncias de ExperienciaProfissional
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns>Data Table</returns>
        public static List<ExperienciaProfissional> ListarExperienciasProfissinaisPorFuncao(PessoaFisica objPessoaFisica, Funcao objFuncao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
            parms[0].Value = objPessoaFisica.IdPessoaFisica;
            parms[1].Value = objFuncao.IdFuncao;

            List<ExperienciaProfissional> listExp = new List<ExperienciaProfissional>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTEXPERIENCIAPORFUNCAO, parms))
            {
                ExperienciaProfissional objExp = new ExperienciaProfissional();

                while (SetInstance_NotDispose(dr, objExp))
                {
                    listExp.Add(objExp);
                    objExp = new ExperienciaProfissional();
                }
            }

            return listExp;
        }
        #endregion

        #region SetInstance_NotDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// Alterado para não dealocar oDataReader e permitir carregar todas as Experiências Profissionais.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objExperienciaProfissional">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Eduardo Ruthes</remarks>
        private static bool SetInstance_NotDispose(IDataReader dr, ExperienciaProfissional objExperienciaProfissional)
        {
            if (dr.Read())
            {
                objExperienciaProfissional._idExperienciaProfissional = Convert.ToInt32(dr["Idf_Experiencia_Profissional"]);
                if (dr["Idf_Area_BNE"] != DBNull.Value)
                    objExperienciaProfissional._areaBNE = new AreaBNE(Convert.ToInt32(dr["Idf_Area_BNE"]));
                objExperienciaProfissional._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
                objExperienciaProfissional._razaoSocial = Convert.ToString(dr["Raz_Social"]);
                if (dr["Des_Funcao_Exercida"] != DBNull.Value)
                    objExperienciaProfissional._descricaoFuncaoExercida = Convert.ToString(dr["Des_Funcao_Exercida"]);
                if (dr["Idf_Funcao"] != DBNull.Value)
                    objExperienciaProfissional._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
                objExperienciaProfissional._dataAdmissao = Convert.ToDateTime(dr["Dta_Admissao"]);
                if (dr["Dta_Demissao"] != DBNull.Value)
                    objExperienciaProfissional._dataDemissao = Convert.ToDateTime(dr["Dta_Demissao"]);
                if (dr["Des_Atividade"] != DBNull.Value)
                    objExperienciaProfissional._descricaoAtividade = Convert.ToString(dr["Des_Atividade"]);
                objExperienciaProfissional._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                objExperienciaProfissional._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                if (dr["Vlr_Salario"] != DBNull.Value)
                    objExperienciaProfissional._vlrSalario = Convert.ToDecimal(dr["Vlr_Salario"]);

                if (dr["Flg_Importado"] != DBNull.Value)
                    objExperienciaProfissional._flagImportado = Convert.ToBoolean(dr["Flg_Importado"]);

                objExperienciaProfissional._persisted = true;
                objExperienciaProfissional._modified = false;

                return true;
            }
            return false;
        }

        #endregion

        #region CarregarExperienciaProfissionalImportada
        public static bool CarregarExperienciaProfissionalImportada(int idfPessoaFisica, out List<ExperienciaProfissional> listExperienciaProfissionalImportada)
        {
            listExperienciaProfissionalImportada = new List<ExperienciaProfissional>();
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Importado", SqlDbType.Bit));
            parms[0].Value = idfPessoaFisica;
            parms[1].Value = true;

            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SELPORCPF, parms))
                {
                    ExperienciaProfissional objEP = new ExperienciaProfissional();

                    while (SetInstance_NotDispose(dr, objEP))
                    {
                        listExperienciaProfissionalImportada.Add(objEP);
                        objEP = new ExperienciaProfissional();
                    }
                }
                if (listExperienciaProfissionalImportada.Count <= 0)
                {
                    listExperienciaProfissionalImportada = null;
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion 

        #region Inserção em Massa
        /// <summary>
        /// Crie uma tabela para inserir em massa.
        /// Passe um DataTable nulo para e a classe em populada. 
        /// Ela irá instanciar um novo DataTable já com colunas definidas apartir dos parâmetros sql definidos na classe.
        /// Os valores setados nas propriedades são transformados em uma linha na tabela.
        /// </summary>
        /// <param name="dt"></param>
        public void AddBulkTable(ref DataTable dt)
        {
            DataAccessLayer.AddBulkTable(ref dt, this);
        }
        /// <summary>
        /// Realiza inserção em massa.
        /// </summary>
        /// <param name="dt">Tabela criada pelo método AddBulkTable</param>
        /// <param name="trans">Transação</param>
        public static void SaveBulkTable(DataTable dt, SqlTransaction trans)
        {
            DataAccessLayer.SaveBulkTable(dt, "BNE_Experiencia_Profissional", trans);
        }
        #endregion

        #region InativarExperienciaPorPessoaFisica
        /// <summary>
        /// Inativa todas as experiencias da pessoa fisica.
        /// </summary>
        /// <param name="Idf_Pessoa_Fisica"></param>
        /// <param name="trans"></param>
        public static void InativarExperienciaPorPessoaFisica(int Idf_Pessoa_Fisica, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = Idf_Pessoa_Fisica}
                };

            if (trans != null)
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, spInativarExperienciaPorPessoaFisica, parms);
            else
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, spInativarExperienciaPorPessoaFisica, parms);
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
                this._idExperienciaProfissional = value;
            }
            get { return this._idExperienciaProfissional; }
        }

        public DateTime MigrationDataCadastro
        {
            set
            {
                this._dataCadastro = value;
            }
            get { return this._dataCadastro; }
        }
        #endregion
    }
}