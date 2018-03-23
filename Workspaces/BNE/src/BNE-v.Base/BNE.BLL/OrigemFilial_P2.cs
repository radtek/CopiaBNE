//-- Data: 08/07/2010 19:00
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

        #region Consultas

        #region SPSELECTIDORIGEM
        private const string SPSELECTIDORIGEM = "SELECT * FROM TAB_Origem_Filial WHERE Idf_Origem = @Idf_Origem";
        #endregion

        #region SPSELECTIDFILIAL
        private const string SPSELECTIDFILIAL = "SELECT * FROM TAB_Origem_Filial WHERE Idf_Filial = @Idf_Filial";
        #endregion

        #region SPSELECTPORDESCRICAODIRETORIO
        private const string SPSELECTPORDESCRICAODIRETORIO = "SELECT * FROM TAB_Origem_Filial WHERE Des_Diretorio LIKE @Des_Diretorio";
        #endregion

        #region SPSELECTORIGEMNAOVINCULADACURRICULO
        private const string SPSELECTORIGEMNAOVINCULADACURRICULO = @"SELECT OFL.Idf_Origem,F.Raz_Social,F.Nme_Fantasia FROM TAB_ORIGEM_FILIAL OFL
                                                        INNER JOIN TAB_FILIAL F ON F.IDF_FILIAL=OFL.IDF_FILIAL
                                                         WHERE F.Flg_Inativo=0 AND OFL.Flg_Inativo=0 AND IDF_ORIGEM NOT IN(
                                                        SELECT O.IDF_ORIGEM FROM BNE_CURRICULO_ORIGEM CO
                                                        INNER JOIN TAB_ORIGEM_FILIAL O ON O.IDF_ORIGEM=CO.IDF_ORIGEM
                                                        WHERE CO.IDF_CURRICULO=@Idf_Curriculo) ";
        #endregion

        #region Spselectfuncoescurriculospororigem
        private const string Spselectfuncoescurriculospororigem = @"
        SELECT  Distinct(F.Idf_Funcao), F.Des_Funcao 
        FROM    BNE_Curriculo C WITH(NOLOCK)
                INNER JOIN BNE_Curriculo_Origem CO WITH(NOLOCK) ON C.Idf_Curriculo = CO.Idf_Curriculo
                --INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                INNER JOIN BNE_Funcao_Pretendida FP WITH(NOLOCK) ON FP.Idf_Curriculo = C.Idf_Curriculo
                INNER JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON FP.Idf_Funcao = F.Idf_Funcao
        WHERE	C.Flg_Inativo = 0 
                --AND PF.Flg_Inativo = 0
			    AND (	C.Idf_Situacao_Curriculo = 1 /*Publicado*/ 
					    OR C.Idf_Situacao_Curriculo = 2 /*Aguardando Publicação*/ 
					    OR C.Idf_Situacao_Curriculo = 3 /*Crítica*/ 
					    OR C.Idf_Situacao_Curriculo = 4 /*Aguardando Revisão VIP*/
					    OR C.Idf_Situacao_Curriculo = 9 /*Revisado VIP*/ 
					    OR C.Idf_Situacao_Curriculo = 10 /*Auditado*/
				    )
                AND CO.Idf_Origem = @Idf_Origem
        ORDER BY F.Des_Funcao
        ";
        #endregion

        #endregion

        #region Métodos

        #region LoadDataReaderPorOrigem
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idOrigem">Código da Origem.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReaderPorOrigem(int idOrigem)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Origem", SqlDbType.Int, 4));

            parms[0].Value = idOrigem;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTIDORIGEM, parms);
        }
        #endregion

        #region CarregarPorOrigem
        /// <summary>
        /// Método responsável por carregar um RHOffice a partir do código identificador de uma origem
        /// </summary>
        /// <param name="idOrigem">Código identificador de uma origem</param>
        /// <param name="objOrigemFilial">Objeto Origem Filial</param>
        /// <returns>Bool - Existe ou não existe</returns>
        public static bool CarregarPorOrigem(int idOrigem, out OrigemFilial objOrigemFilial)
        {
            using (IDataReader dr = LoadDataReaderPorOrigem(idOrigem))
            {
                objOrigemFilial = new OrigemFilial();
                if (SetInstance(dr, objOrigemFilial))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }

            objOrigemFilial = null;
            return false;
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de OrigemFilial a partir do banco de dados.
        /// </summary>
        /// <param name="idOrigem">Chave do registro.</param>
        /// <returns>Instância de OrigemFilial.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static OrigemFilial CarregarPorOrigem(int idOrigem)
        {
            using (IDataReader dr = LoadDataReaderPorOrigem(idOrigem))
            {
                OrigemFilial objOrigemFilial = new OrigemFilial();
                if (SetInstance(dr, objOrigemFilial))
                    return objOrigemFilial;
            }
            throw (new RecordNotFoundException(typeof(OrigemFilial)));
        }
        #endregion

        #region CarregarPorFilial
        /// <summary>
        /// Método responsável por carregar um RHOffice a partir do código identificador de uma Filial
        /// </summary>
        /// <param name="idFilial">Código identificador de uma filial</param>
        /// <param name="objOrigemFilial">Objeto Origem Filial</param>
        /// <returns>Bool - Existe ou não existe</returns>
        public static bool CarregarPorFilial(int idFilial, out OrigemFilial objOrigemFilial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms[0].Value = idFilial;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTIDFILIAL, parms))
            {
                objOrigemFilial = new OrigemFilial();
                if (SetInstance(dr, objOrigemFilial))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }

            objOrigemFilial = null;
            return false;
        }
        #endregion

        #region CarregarPorDiretorio
        /// <summary>   
        /// Método responsável por carregar uma instância de Origem Filial a partir do diretorio. Usado para identificar se o diretorio que o usuário
        /// está digitando na url é um RHOffice
        /// </summary>
        /// <param name="strDescricaoDiretorio">String do diretório</param>
        /// <returns>Objeto Origem Filial</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorDiretorio(String strDescricaoDiretorio, out OrigemFilial objOrigemFilial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Des_Diretorio", SqlDbType.VarChar, 100));
            parms[0].Value = strDescricaoDiretorio;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORDESCRICAODIRETORIO, parms))
            {
                objOrigemFilial = new OrigemFilial();
                if (SetInstance(dr, objOrigemFilial))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objOrigemFilial = null;
            return false;
        }
        #endregion

        #region CarregarOrigemNaoVinculadaAoCurriculo
        /// <summary>
        /// Método resposável por retornar todas os idf_origem e empresas não vinculadas ao currículo
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public static IDataReader CarregarOrigemNaoVinculadaAoCurriculo(int idCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTORIGEMNAOVINCULADACURRICULO, parms);
        }
        #endregion

        #region Salvar
        public void Salvar(Origem objOrigem, FilialLogo objFilialLogo)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        objOrigem.Save(trans);
                        this.Origem = objOrigem;
                        this.Save(trans);

                        objFilialLogo.Filial = this.Filial;
                        objFilialLogo.Save(trans);

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

        #region ListarFuncoesCurriculosOrigem
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados com as funções desejadas pelos curriculso que pertencem a origem.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static IDataReader ListarFuncoesCurriculosOrigem(int idOrigem)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Origem", SqlDbType = SqlDbType.Int, Size = 4, Value = idOrigem }
                };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectfuncoescurriculospororigem, parms);
        }
        #endregion

        #endregion

    }
}