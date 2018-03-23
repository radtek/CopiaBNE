//-- Data: 05/10/2012 14:44
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;

namespace BNE.BLL.AsyncServices
{
    public partial class TipoAtividade // Tabela: TAB_Tipo_Atividade
	{

        #region RelacaoTipoAtividade
        /// <summary>
        /// Objeto representando a relação de Tipo de atividae
        /// </summary>
        public class RelacaoTipoAtividade
        {
            public String DesTipoAtividade { get; set; }
            public Enumeradores.TipoAtividade TipoAtividade { get; set; }

            public RelacaoTipoAtividade(string desTipoAtividade, int tipoAtividade)
            {
                DesTipoAtividade = desTipoAtividade;
                TipoAtividade = (Enumeradores.TipoAtividade)tipoAtividade;
            }
        }
        #endregion 

        #region Consultas

        #region Splistartiposporcs
        private const String Splistartiposporcs = @"
        select  ta.Des_Tipo_Atividade, 
                ta.Idf_Tipo_Atividade
        from 
          TAB_Tipo_Atividade ta 
        where 
          ta.Flg_Inativo = 0
        ";
        #endregion

        #endregion 

        #region Métodos

        #region ListarTiposAtividade
        public static List<RelacaoTipoAtividade> ListarTiposAtividade()
        {
            var lst = new List<RelacaoTipoAtividade>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Splistartiposporcs, null))
            {
                while (dr.Read())
                {
                    lst.Add(new RelacaoTipoAtividade(
                        Convert.ToString(dr["Des_Tipo_Atividade"]),
                        Convert.ToInt32(dr["Idf_Tipo_Atividade"])));
                }
            }

            return lst;
        }
        #endregion

        #endregion

	}
}