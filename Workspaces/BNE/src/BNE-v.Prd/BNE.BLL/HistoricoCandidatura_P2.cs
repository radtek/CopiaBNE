//-- Data: 08/06/2016 14:57
//-- Autor: Gieyson Stelmak

using System;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class HistoricoCandidatura // Tabela: BNE_Historico_Candidatura
    {
        public static void Logar(Curriculo objCurriculo, Vaga objVaga, string descricao, SqlTransaction trans)
        {
            var objHistoricoCandidatura = new HistoricoCandidatura
            {
                Curriculo = objCurriculo,
                Vaga = objVaga,
                DescricaoCandidatura = descricao,
                DataCandidatura = DateTime.Now
            };

            objHistoricoCandidatura.Save(trans);
        }
    }
}