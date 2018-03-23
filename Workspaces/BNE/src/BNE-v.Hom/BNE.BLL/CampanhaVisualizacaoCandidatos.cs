using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace BNE.BLL
{
    /// <summary>
    ///  Classe que controla as regras de visualização de candidatos para empresas que não possuem plano.
    /// </summary>
    /// <remarks>
    /// A regra de visualização dos candidatos para empresas sem plano ocorre de acordo com a corrente de 
    /// regras no parametro 'CampanhaVisualizacaoRegra'. A leitra da corrente começa de cima para baixo 
    /// (índice 0... até N, onde N é o número de regras), quando é encontrado uma regra compatível com a cidade da empresa
    /// e ela é aplicada e o processamento termina.
    /// 
    /// Exemplo de regras no paramentro:
    /// 
    ///  { "Regras": 
    ///    [
    ///       {
	///           "Nome_Regra" : "Capitais Costeiras",
	///           "Lista_Cidades" : [2916, 678, 4708, 2655],
	///           "Quantidade_Visualizacoes" : 5
    ///       },	
    ///       {
	///           "Nome_Regra" : "Capitais do Sudeste",
	///           "Lista_Cidades" : [1411, 3658, 5345, 882],
	///           "Quantidade_Visualizacoes" : 7
    ///       },
    ///       {
	///           "Nome_Regra" : "Regra Geral do Brasil",
	///           "Lista_Cidades" : [],
	///           "Quantidade_Visualizacoes" : 3
    ///       }  
    ///    ]
    ///  }
    ///    
    /// </remarks>
    public class CampanhaVisualizacaoCandidatos
    {

        private static string QRY_TOTAL_VAGAS = @"DECLARE @Qtd_Visualizados  INT = 0, @Esta_Inscrito	INT = 0;
                                                SELECT @Qtd_Visualizados = COUNT(DISTINCT his.Idf_Curriculo) FROM BNE.BNE_Curriculo_Visualizacao_Historico his  WITH (NOLOCK) 
                                                WHERE his.Flg_Visualizacao_Completa = 1  AND  his.Idf_Filial = @Idf_Filial AND his.Idf_Vaga = @Idf_Vaga;
                                                SELECT @Esta_Inscrito = COUNT(*) FROM BNE.BNE_Vaga_Candidato WHERE Idf_Vaga = @Idf_Vaga AND Idf_Curriculo = @Idf_Curriculo AND Flg_Inativo = 0
                                                SELECT @Esta_Inscrito AS 'Esta_Inscrito',  @Qtd_Visualizados AS 'Qtd_Visualizados'";

        private static string QRY_JA_VIU = @" SELECT COUNT(DISTINCT his.Idf_Curriculo) AS Total  FROM BNE.BNE_Curriculo_Visualizacao_Historico his  WITH (NOLOCK)  
                                              WHERE his.Flg_Visualizacao_Completa = 1  AND  his.Idf_Filial = @Idf_Filial AND his.Idf_Vaga = @Idf_Vaga AND his.Idf_Curriculo = @Idf_Curriculo ;";

        private Filial _filial;
        private static List<CampanhaVisualizacaoRegra> _regras;


        /// <summary>
        /// A quantidade de vizualizações do currículo é feita através da filial e
        /// não do usuário que está fazendo o acesso.
        /// </summary>
        /// <param name="filial">Filial do usuário que está tentando visualizar a vaga</param>
        public CampanhaVisualizacaoCandidatos(Filial filial) 
        {

            if (filial != null) 
            {
                if (filial.IdFilial != 0)
                 this._filial = (filial.CompleteObject()) ? filial : null;

                if (this._filial != null) 
                {
                    this._filial = (this._filial.Endereco.CompleteObject()) ? this._filial : null;
                }                
            }

            if (CampanhaVisualizacaoCandidatos._regras == null) 
            {
                try
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    CampanhaVisualizacaoRegras CvrList = jss.Deserialize<CampanhaVisualizacaoRegras>(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CampanhaVisualizacaoRegra));
                    CampanhaVisualizacaoCandidatos._regras = CvrList.Regras;
                }
                catch (Exception ex) 
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
            }
        }


        /// <summary>
        /// Verifica se a filial já visualizou o curriculo. Se já viu deixa continuar. 
        /// Caso não tenha visto verifica se teve verifica se está dentro do limite de visualizações permitdias.
        /// <param name="c">Currículo que será visualizado</param>
        /// <param name="vg">Em que vaga este currículo está sendo visualizado</param>
        /// </summary>
        public bool PodeVisualizar(Curriculo c, Vaga vg) 
        {
            int limite = 0;
            int total = 0;
            int esta_inscrito = 0;
            if (this.JaViu(c, vg)) 
            {
                return true;
            }
            else 
            {
                try
                {
                    limite = this.QuantidadeDeVisualizacoes();
                    IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, QRY_TOTAL_VAGAS,
                       new List<SqlParameter>() 
                    { 
                        new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = this._filial.IdFilial  },
                        new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = vg.IdVaga  },
                        new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = c.IdCurriculo  },
                    });

                    if (dr.Read())
                    {
                        total         = dr.GetInt32(dr.GetOrdinal("Qtd_Visualizados"));
                        esta_inscrito = dr.GetInt32(dr.GetOrdinal("Esta_Inscrito"));
                        return (!(total >= limite) && (esta_inscrito > 0));
                    }
                    else
                        return false;

                }
                catch (Exception e) 
                {
                    EL.GerenciadorException.GravarExcecao(e);
                    return false;
                }
            }
        }

        #region Métodos privados
        private bool JaViu(Curriculo c, Vaga vg)
        {
            int total = 0;
            try
            {
                IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, QRY_JA_VIU,
                    new List<SqlParameter>() 
                    { 
                        new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = this._filial.IdFilial  },
                        new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = vg.IdVaga  },
                        new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = c.IdCurriculo  },
                    });

                if (dr.Read()) total = dr.GetInt32(dr.GetOrdinal("Total"));
            }
            catch (Exception e)
            {
                EL.GerenciadorException.GravarExcecao(e);
            }

            return (total > 0);
        }

        private int QuantidadeDeVisualizacoes()
        {
            if (CampanhaVisualizacaoCandidatos._regras != null)
            {
                foreach (var reg in CampanhaVisualizacaoCandidatos._regras)
                {
                    if (reg.Lista_Cidades.Contains(this._filial.Endereco.Cidade.IdCidade))
                        return reg.Quantidade_Visualizacoes;
                }
                return CampanhaVisualizacaoCandidatos._regras.Last().Quantidade_Visualizacoes;
            }
            else
                return 0;
        }
        #endregion

        #region Classes de interpretação JSON
        private class CampanhaVisualizacaoRegras
        {
            public List<CampanhaVisualizacaoRegra> Regras { get; set; }
        }

        private class CampanhaVisualizacaoRegra
        {
            public string Nome_Regra { get; set; }
            public int[] Lista_Cidades { get; set; }
            public int Quantidade_Visualizacoes { get; set; }

            public override string ToString()
            {
                return string.Format("<Nome_Regra: {0}, Lista_Cidades: [{1}], Quantidade_Visualizacoes: {2}>", this.Nome_Regra, string.Join(",", this.Lista_Cidades), this.Quantidade_Visualizacoes);
            }

        }
        #endregion
        
    }

}
