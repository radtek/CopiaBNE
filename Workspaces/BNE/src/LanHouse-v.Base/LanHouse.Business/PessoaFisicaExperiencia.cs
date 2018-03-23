using LanHouse.Entities.BNE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class PessoaFisicaExperiencia
    {
        #region ListarExperienciasProfissionais
        /// <summary>
        /// Listar as experiências profissionais do Candidato
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <returns></returns>
        public static List<DTO.Experiencia> ListarExperienciasProfissionais(int idPessoaFisica)
        {

            using (var entity = new LanEntities())
            {
                var query = (
                    from expe in entity.BNE_Experiencia_Profissional
                    join fun in entity.TAB_Funcao on expe.Idf_Funcao equals fun.Idf_Funcao into ef
                    from x in ef.DefaultIfEmpty()
                    where expe.Idf_Pessoa_Fisica == idPessoaFisica && expe.Flg_Inativo == false
                        select new DTO.Experiencia
                        {
                            idExperienciaProfissional = expe.Idf_Experiencia_Profissional,
                            idAreaBNE = expe.Idf_Area_BNE,
                            Razao = expe.Raz_Social,
                            funcaoEmpresa = expe.Des_Funcao_Exercida != null ? expe.Des_Funcao_Exercida : x.Des_Funcao,
                            dataDemissao = expe.Dta_Demissao,
                            dataAdmissao = expe.Dta_Admissao,

                            DesAtividades = expe.Des_Atividade
                        }).OrderByDescending(o => o.dataAdmissao).Take(3).ToList();

                return query;

            }
        }
        #endregion
    }
}
