using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class FilialObservacao
    {
        #region SalvarCRM
        public static void SalvarCRM(string descricao, int idFilial, string nomeProcessoSistema, LanEntities context)
        {
            if (!string.IsNullOrWhiteSpace(nomeProcessoSistema))
                descricao = string.Concat(descricao, " Efetuado pelo processo:", nomeProcessoSistema);

            var objFilialObservacao = new TAB_Filial_Observacao
            {
                Dta_Cadastro = DateTime.Now,
                Des_Observacao = descricao,
                Idf_Filial = idFilial,
                Flg_Sistema = true,
                Idf_Usuario_Filial_Perfil = null
            };

            context.TAB_Filial_Observacao.Add(objFilialObservacao);
            context.SaveChanges();
        }
        #endregion
    }
}
