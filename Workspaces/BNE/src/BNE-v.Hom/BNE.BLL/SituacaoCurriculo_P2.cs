//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak

using System;
using System.Data;

namespace BNE.BLL
{
    public partial class SituacaoCurriculo : ICloneable // Tabela: BNE_Situacao_Curriculo
    {

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Consulta

        private const string SELECTSITUACAICURRICULO = @" Select 
                                                        Idf_Situacao_Curriculo,
                                                        Des_Situacao_Curriculo
                                                    From
                                                    BNE_Situacao_Curriculo";

        #endregion

        #region Métodos
        /// <summary>
        /// Método retorna consulta de Des_Deficiencia para o DropDownList ser carregado.
        /// </summary>
        /// <returns></returns>
        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SELECTSITUACAICURRICULO, null);
        }
        #endregion

    }
}