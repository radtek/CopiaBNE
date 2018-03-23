//-- Data: 05/03/2015 12:05
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class VagaVisualizada // Tabela: BNE_Vaga_Visualizada
    {

        #region Consultas
        private const string selectquantidadevisualizacao = "SELECT COUNT(Idf_Vaga_Visualizada) FROM BNE_Vaga_Visualizada WHERE Idf_Vaga = @Idf_Vaga";
        #endregion

        #region Métodos

        #region SalvarVisualizacaoVaga
        public static bool SalvarVisualizacaoVaga(Vaga objVaga)
        {
            return SalvarVisualizacaoVaga(objVaga, null);
        }

        public static bool SalvarVisualizacaoVaga(Vaga objVaga, Curriculo objCurriculo)
        {
            try
            {
                var objVagaVisualizada = new VagaVisualizada
                {
                    Curriculo = objCurriculo,
                    DataVisualizacao = DateTime.Now,
                    Vaga = objVaga
                };

                objVagaVisualizada.Save();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
            return true;
        }
        #endregion

        #region QuantidadeVisualizacao
        public static int RecuperarQuantidadeVisualizacao(Vaga objVaga)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga } 
				};

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, selectquantidadevisualizacao, parms));
        }
        #endregion

        #endregion

    }
}