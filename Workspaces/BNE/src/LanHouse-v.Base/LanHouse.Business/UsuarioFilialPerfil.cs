using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class UsuarioFilialPerfil
    {
        #region CarregarPorIdPessoa
        public static bool CarregarPorIdPessoa(int idPessoa, out TAB_Usuario_Filial_Perfil objUsuarioFilialPerfil)
        {
            using (var entity = new LanEntities())
            {
                objUsuarioFilialPerfil = (from ufp in entity.TAB_Usuario_Filial_Perfil
                                          where ufp.Idf_Pessoa_Fisica == idPessoa
                                          && ufp.Flg_Inativo == false
                                          select ufp).FirstOrDefault();

                return objUsuarioFilialPerfil != null;
            }
        }
        #endregion
    }
}
