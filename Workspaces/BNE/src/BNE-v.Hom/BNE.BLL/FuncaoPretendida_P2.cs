//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Text;

namespace BNE.BLL
{
    public partial class FuncaoPretendida // Tabela: BNE_Funcao_Pretendida
    {

        #region Propriedades

        #region MesesExperiencia
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Int16 MesesExperiencia
        {
            get
            {
                if (this._quantidadeExperiencia.HasValue)
                {
                    if (!(this._quantidadeExperiencia.Value % 12).Equals(0))
                        return Convert.ToInt16(this._quantidadeExperiencia.Value - (AnosExperiencia * 12));
                }
                return 0;
            }
        }
        #endregion

        #region AnosExperiencia
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Int16 AnosExperiencia
        {
            get
            {
                if (this._quantidadeExperiencia.HasValue)
                    return Convert.ToInt16(this._quantidadeExperiencia.Value / 12);
                return 0;
            }
        }
        #endregion

        #region Persisted
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public void Persisted(bool persisted)
        {
            this._persisted = persisted;
        }
        #endregion

        #endregion

        #region Consultas

        private const string Spselectnomefuncoesporcurriculo = @"
        SELECT (SELECT    FU.Des_Funcao + ';'
			FROM      BNE.BNE_Funcao_Pretendida (NOLOCK) FPSub
					INNER JOIN plataforma.TAB_Funcao (NOLOCK) FU ON FPSub.Idf_Funcao = FU.Idf_Funcao
			WHERE     FPSub.Idf_Curriculo = @Idf_Curriculo
			ORDER BY  FPSub.Dta_Cadastro DESC
		FOR
			XML PATH(''),TYPE) AS funcoes
        ";

        private const string Spselectfuncaoporcurriculo = @"
        SELECT  * 
        FROM    BNE_Funcao_Pretendida WITH( NOLOCK )
        WHERE   Idf_Curriculo = @Idf_Curriculo
        ORDER BY Idf_Funcao_Pretendida ASC 
        ";

        private const string Spselectfuncaopretendidaporcurriculofuncao = @"
        SELECT  * 
        FROM    BNE_Funcao_Pretendida WITH( NOLOCK )
        WHERE   Idf_Curriculo = @Idf_Curriculo
                AND Idf_Funcao = @Idf_Funcao
        ";

        private const string Spselectfuncoesporcurriculo = @"
        SELECT  TOP 3 * 
        FROM    BNE_Funcao_Pretendida FP WITH(NOLOCK) 
                LEFT JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON FP.Idf_Funcao = F.Idf_Funcao
        WHERE   Idf_Curriculo = @Idf_Curriculo
        ORDER BY Idf_Funcao_Pretendida ASC 
        ";

        private const string Spdeletefuncaopretendidaporcurriculo = @"DELETE BNE_Funcao_Pretendida WHERE Idf_Curriculo = @Idf_Curriculo";

        private const string SpInativafuncaopretendidaporcurriculo = @"update BNE_Funcao_Pretendida set flg_inativo = 1 WHERE Idf_Curriculo = @Idf_Curriculo";

        private const string SpCarregarSimilaresPorIdFuncao = @"BNE.SP_CARREGAR_FUNCOES_SIMILARES_POR_ID";

        private const string SpCarregarSimilaresPorIdFuncaoECidade = @"BNE.SP_Carregar_Funcoes_Similares_Por_ID_e_Cidade";

        #endregion

        #region CarregarPorCurriculo
        /// <summary>
        /// Método responsável por carregar uma instancia de Funcao Pretendida dado determinado Curriculo
        /// </summary>
        /// <param name="idCurriculo">Identificador do Curriculo</param>
        /// <param name="objFuncaoPretendida">Funcao Pretendida</param>
        /// <returns>True caso exista</returns>
        public static bool CarregarPorCurriculo(int idCurriculo, out FuncaoPretendida objFuncaoPretendida)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectfuncaoporcurriculo, parms))
            {
                objFuncaoPretendida = new FuncaoPretendida();
                if (SetInstance(dr, objFuncaoPretendida))
                    return true;
                dr.Close();
            }
            objFuncaoPretendida = null;
            return false;
        }
        #endregion

        #region CarregarPorCurriculoFuncao
        /// <summary>
        /// Método responsável por carregar uma instancia de Funcao Pretendida dado determinado Curriculo
        /// </summary>
        /// <param name="objCurriculo">Identificador do Curriculo</param>
        /// <param name="objFuncao">Funcao</param>
        /// <param name="objFuncaoPretendida">Funcao Pretendida</param>
        /// <returns>FuncaoPretendida caso exista</returns>
        public static bool CarregarPorCurriculoFuncao(Curriculo objCurriculo, Funcao objFuncao, SqlTransaction trans, out FuncaoPretendida objFuncaoPretendida)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4), 
                    new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4)
                };
            parms[0].Value = objCurriculo.IdCurriculo;
            parms[1].Value = objFuncao.IdFuncao;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectfuncaopretendidaporcurriculofuncao, parms))
            {
                objFuncaoPretendida = new FuncaoPretendida();

                if (SetInstance(dr, objFuncaoPretendida))
                    return true;

                dr.Dispose();
            }
            objFuncaoPretendida = null;
            return false;
        }
        #endregion

        #region Carregar nomes de funções pretendidas
        /// <summary>
        /// retorna uma string com o nome das funções pretendidas
        /// </summary>
        /// <param name="objCurriculo"></param>
        /// <returns>string = pedreiro;programador;</returns>
        public static string CarregarNomeDeFuncoesPretendidasPorCurriculo(Curriculo objCurriculo)
        {

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo}
                };

            IDataReader dr;

            String funcoes = string.Empty;
            using (dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectnomefuncoesporcurriculo, parms))
            {
                if (dr.Read())
                    funcoes = Convert.ToString(dr["funcoes"]);
            }
            return funcoes;
        }
        #endregion

        #region CarregarFuncoesPretendidasPorCurriculo
        /// <summary>
        /// Método responsável por recuperar todas as instâncias de Funcao Pretendida dado determinado Curriculo
        /// </summary>
        /// <param name="objCurriculo">Curriculo</param>
        public static List<FuncaoPretendida> CarregarFuncoesPretendidasPorCurriculo(Curriculo objCurriculo)
        {
            return CarregarFuncoesPretendidasPorCurriculo(objCurriculo, null);
        }

        /// <summary>
        /// Método responsável por recuperar todas as instâncias de Funcao Pretendida dado determinado Curriculo
        /// </summary>
        /// <param name="objCurriculo">Curriculo</param>
        /// <param name="trans">Transação</param>
        public static List<FuncaoPretendida> CarregarFuncoesPretendidasPorCurriculo(Curriculo objCurriculo, SqlTransaction trans)
        {
            var listFuncaoPretendida = new List<FuncaoPretendida>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo}
                };

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectfuncoesporcurriculo, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectfuncoesporcurriculo, parms);

            var objFuncaoPretendida = new FuncaoPretendida();
            while (SetInstanceNotDispose(dr, objFuncaoPretendida))
            {
                listFuncaoPretendida.Add(objFuncaoPretendida);
                objFuncaoPretendida = new FuncaoPretendida();
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return listFuncaoPretendida;
        }
        #endregion

        #region SetInstanceNotDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objFuncaoPretendida">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstanceNotDispose(IDataReader dr, FuncaoPretendida objFuncaoPretendida)
        {
            if (dr.Read())
            {
                objFuncaoPretendida._idFuncaoPretendida = Convert.ToInt32(dr["Idf_Funcao_Pretendida"]);
                objFuncaoPretendida._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                if (dr["Idf_Funcao"] != DBNull.Value)
                {
                    objFuncaoPretendida._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));

                    if (dr["Des_Funcao"] != DBNull.Value)
                        objFuncaoPretendida._funcao.DescricaoFuncao = Convert.ToString(dr["Des_Funcao"]);
                }
                if (dr["Qtd_Experiencia"] != DBNull.Value)
                    objFuncaoPretendida._quantidadeExperiencia = Convert.ToInt16(dr["Qtd_Experiencia"]);
                objFuncaoPretendida._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                if (dr["Des_Funcao_Pretendida"] != DBNull.Value)
                    objFuncaoPretendida._descricaoFuncaoPretendida = Convert.ToString(dr["Des_Funcao_Pretendida"]);
                else
                    objFuncaoPretendida._descricaoFuncaoPretendida = string.Empty;

                objFuncaoPretendida._persisted = true;
                objFuncaoPretendida._modified = false;

                return true;
            }
            return false;
        }
        #endregion

        #region DeleteFuncaoPretendidaPorCurriculo
        public static void DeleteFuncaoPretendidaPorCurriculo(int idCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo}
                };

            if (trans != null)
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spdeletefuncaopretendidaporcurriculo, parms);
            else
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spdeletefuncaopretendidaporcurriculo, parms);
        }
        #endregion

        #region SalvarFuncaoPretendidaIntegracao - Velha
        /// <summary>
        /// Método responsável por salvar as Funções pretendidas integradas a partir da WebFoPag, levando em conta as experiencias profissionais do empregado
        /// </summary>
        /// <param name="objExperienciaProfissional"></param>
        /// <param name="objCurriculo"></param>
        public static void SalvarFuncaoPretendidaIntegracao(ExperienciaProfissional objExperienciaProfissional, Curriculo objCurriculo, SqlTransaction trans = null)
        {
            int quantidadeExperiencia;

            //Salvando uma funcao pretendida com os dados da experiencia profissional
            FuncaoPretendida objFuncaoPretendida;
            if (!CarregarPorCurriculoFuncao(objCurriculo, objExperienciaProfissional.Funcao, trans, out objFuncaoPretendida))
            {
                objFuncaoPretendida = new FuncaoPretendida
                    {
                        Curriculo = objCurriculo,
                        Funcao = objExperienciaProfissional.Funcao
                    };

                quantidadeExperiencia = objExperienciaProfissional.DataDemissao.HasValue ?
                    objExperienciaProfissional.DataDemissao.Value.Subtract(objExperienciaProfissional.DataAdmissao).Days :
                    DateTime.Now.Subtract(objExperienciaProfissional.DataAdmissao).Days;

                objFuncaoPretendida.QuantidadeExperiencia = (short)(quantidadeExperiencia / 30);

            }
            else //Ajustar a quantidade de experiencia
            {
                DateTime dataAdmissao = objExperienciaProfissional.DataAdmissao;
                DateTime? dataDemissao = null;

                List<ExperienciaProfissional> listExperienciaPorFuncao = ExperienciaProfissional.ListarExperienciasProfissinaisPorFuncao(objExperienciaProfissional.PessoaFisica, objExperienciaProfissional.Funcao, trans);
                //Recuperando todas as experiencias profissionais na mesma função para recuperar a primeira e a ultima data de admissao demissao
                //E efetuar o cálculo correto da quanitdade de experiencia.
                foreach (ExperienciaProfissional objExp in listExperienciaPorFuncao)
                {
                    if (objExp.DataAdmissao < dataAdmissao) //Se existe uma data de admissão menor que a atual.
                        dataAdmissao = objExp.DataAdmissao;

                    if (!dataDemissao.HasValue)
                        dataDemissao = objExp.DataDemissao;
                    else if (objExp.DataDemissao.HasValue && objExp.DataDemissao > dataDemissao) //Se tem data de demissao e é maior que a atual
                        dataDemissao = objExp.DataDemissao;
                }

                quantidadeExperiencia = dataDemissao.HasValue ? dataDemissao.Value.Subtract(dataAdmissao).Days : DateTime.Now.Subtract(dataAdmissao).Days;

            }

            objFuncaoPretendida.QuantidadeExperiencia = (short)(quantidadeExperiencia / 30);
            if (trans != null)
                objFuncaoPretendida.Save();
            else
                objFuncaoPretendida.Save(trans);
        }
        #endregion

        #region SalvarFuncaoPretendidaIntegracao - Nova
        /// <summary>
        /// Método responsável por salvar as Funções pretendidas integradas a partir da WebFoPag, levando em conta as experiencias profissionais do empregado
        /// </summary>
        /// <param name="objExperienciaProfissional"></param>
        /// <param name="objCurriculo"></param>
        /// <param name="objFuncaoPretendida"> </param>
        public static void SalvarFuncaoPretendidaIntegracao(ExperienciaProfissional objExperienciaProfissional, Curriculo objCurriculo, SqlTransaction trans, out FuncaoPretendida objFuncaoPretendida)
        {
            int quantidadeExperiencia;

            //Salvando uma funcao pretendida com os dados da experiencia profissional
            if (!CarregarPorCurriculoFuncao(objCurriculo, objExperienciaProfissional.Funcao, trans, out objFuncaoPretendida))
            {
                objFuncaoPretendida = new FuncaoPretendida
                {
                    Curriculo = objCurriculo,
                    Funcao = objExperienciaProfissional.Funcao
                };

                quantidadeExperiencia = objExperienciaProfissional.DataDemissao.HasValue ?
                    objExperienciaProfissional.DataDemissao.Value.Subtract(objExperienciaProfissional.DataAdmissao).Days :
                    DateTime.Now.Subtract(objExperienciaProfissional.DataAdmissao).Days;

                objFuncaoPretendida.QuantidadeExperiencia = (short)(quantidadeExperiencia / 30);

            }
            else //Ajustar a quantidade de experiencia
            {
                DateTime dataAdmissao = objExperienciaProfissional.DataAdmissao;
                DateTime? dataDemissao = null;

                List<ExperienciaProfissional> listExperienciaPorFuncao = ExperienciaProfissional.ListarExperienciasProfissinaisPorFuncao(objExperienciaProfissional.PessoaFisica, objExperienciaProfissional.Funcao, trans);
                //Recuperando todas as experiencias profissionais na mesma função para recuperar a primeira e a ultima data de admissao demissao
                //E efetuar o cálculo correto da quanitdade de experiencia.
                foreach (ExperienciaProfissional objExp in listExperienciaPorFuncao)
                {
                    if (objExp.DataAdmissao < dataAdmissao) //Se existe uma data de admissão menor que a atual.
                        dataAdmissao = objExp.DataAdmissao;

                    if (!dataDemissao.HasValue)
                        dataDemissao = objExp.DataDemissao;
                    else if (objExp.DataDemissao.HasValue && objExp.DataDemissao > dataDemissao) //Se tem data de demissao e é maior que a atual
                        dataDemissao = objExp.DataDemissao;
                }

                quantidadeExperiencia = dataDemissao.HasValue ? dataDemissao.Value.Subtract(dataAdmissao).Days : DateTime.Now.Subtract(dataAdmissao).Days;
            }

            objFuncaoPretendida.QuantidadeExperiencia = (short)(quantidadeExperiencia / 30);
        }
        #endregion

        #region CarregarFuncoesSimilaresPorFuncao
        /// <summary>
        /// Método responsável por recuperar todas as instâncias de Funcões Similares de acordo com uma Função Pretendida de determinado Curriculo
        /// </summary>
        /// <param name="objCurriculo">Curriculo</param>
        public static List<Funcao> CarregarFuncoesSimilaresPorFuncao(int Idf_Função)
        {
            var listFuncoesSimilares = new List<Funcao>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Size = 4, Value = Idf_Função}
                };

            IDataReader dr;

            dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, SpCarregarSimilaresPorIdFuncao, parms);

            Funcao objFuncaoSimilar;

            while (dr.Read())
            {
                try
                {
                    objFuncaoSimilar = Funcao.LoadObject(Convert.ToInt32(dr["Idf_Funcao_Similar"].ToString()));
                    listFuncoesSimilares.Add(objFuncaoSimilar);
                }
                catch
                {
                    continue;
                }
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return listFuncoesSimilares;
        }
        #endregion

        #region CarregarFuncoesSimilaresPorFuncaoEQtdeVagas
        public static DataTable CarregarFuncoesSimilaresPorFuncaoECidade(int qtdeMaxRetorno, int? idFuncao = null, int? idCidade = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Count", SqlDbType = SqlDbType.Int, Size = 4, Value = qtdeMaxRetorno},
                    new SqlParameter{ ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Size = 4, Value = idFuncao},
                    new SqlParameter{ ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Size = 4, Value = idCidade}
                };

            if (parms[1].Value == null)
                parms[1].Value = DBNull.Value;

            if (parms[2].Value == null)
                parms[2].Value = DBNull.Value;

            return DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, SpCarregarSimilaresPorIdFuncaoECidade, parms).Tables[0];
        }
        #endregion


        public override string ToString()
        {
            var retorno = new StringBuilder();
            if (this.AnosExperiencia > 0)
            {
                retorno.Append(this.AnosExperiencia + " ano");
                if (this.AnosExperiencia > 1)
                    retorno.Append("s");
            }
            if (this.AnosExperiencia > 0 && this.MesesExperiencia > 0)
            {
                retorno.Append(" e ");
            }
            if (this.MesesExperiencia > 0)
            {
                if (this.MesesExperiencia == 1)
                    retorno.Append(this.MesesExperiencia + " mês");
                else
                    retorno.Append(this.MesesExperiencia + " meses");
            }
            retorno.Append(string.Format(" como {0}<br>", this.DescricaoFuncaoPretendida));

            return retorno.ToString();
        }

        #region InativaFuncaoPretendidaPorCurriculo
        public static void InativaFuncaoPretendidaPorCurriculo(int idCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo}
                };

            if (trans != null)
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SpInativafuncaopretendidaporcurriculo, parms);
            else
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpInativafuncaopretendidaporcurriculo, parms);
        }
        #endregion
    }
}