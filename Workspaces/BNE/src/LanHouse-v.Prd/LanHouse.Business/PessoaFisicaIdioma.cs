using LanHouse.Entities.BNE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class PessoaFisicaIdioma
    {
        #region ListarIdiomas
        /// <summary>
        /// Listar os idiomas da Pessoa
        /// </summary>
        /// <returns></returns>
        public static List<DTO.IdiomaCandidato> ListarIdiomas(int idPessoaFisica)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from idiPF in entity.TAB_Pessoa_Fisica_Idioma
                    join idi in entity.TAB_Idioma on idiPF.Idf_Idioma equals idi.Idf_Idioma
                    join nivel in entity.TAB_Nivel_Idioma on idiPF.Idf_Nivel_Idioma equals nivel.Idf_Nivel_Idioma
                    where idiPF.Idf_Pessoa_Fisica == idPessoaFisica && idiPF.Flg_Inativo == false
                    select new DTO.IdiomaCandidato { 
                        idIdiomaCandidato = idiPF.Idf_Pessoa_Fisica_Idioma,
                        idIdioma = idiPF.Idf_Idioma, 
                        text = idi.Des_Idioma, 
                        nivel = idiPF.Idf_Nivel_Idioma,
                        nivelTexto = nivel.Des_Nivel_Idioma
                    }).ToList();

                return query;
            }
        }
        #endregion
    }
}
