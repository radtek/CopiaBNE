using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class CurriculoObservacao
    {
        #region SalvarCRM
        public static void SalvarCRM(string descricao, int idCurriculo, string nomeProcessoSistema, LanEntities context)
        {
            if (!string.IsNullOrWhiteSpace(nomeProcessoSistema))
                descricao = string.Concat(descricao, " Efetuado pelo processo:", nomeProcessoSistema);

            var objCurriculoObservacao = new BNE_Curriculo_Observacao
            {
                Dta_Cadastro = DateTime.Now,
                Des_Observacao = descricao,
                Idf_Curriculo = idCurriculo,
                Flg_Sistema = true,
                Idf_Usuario_Filial_Perfil = null
            };

            context.BNE_Curriculo_Observacao.Add(objCurriculoObservacao);
            context.SaveChanges();
        }
        #endregion
    }
}
