using LanHouse.Entities.BNE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class Vaga
    {
        /// <summary>
        /// Retornar uma lista de Vagas Qualquer
        /// </summary>
        /// <param name="funcao"></param>
        public static IList RecuperarVagas()
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from vaga in entity.BNE_Vaga
                    join funcao in entity.TAB_Funcao on vaga.Idf_Funcao equals funcao.Idf_Funcao
                    join cidade in entity.TAB_Cidade on vaga.Idf_Cidade equals cidade.Idf_Cidade
                    where vaga.Idf_Cidade != null
                        && vaga.Idf_Filial != null
                        && vaga.Des_Beneficio != null
                        && vaga.Des_Atribuicoes != null
                        && vaga.Des_Requisito != null
                    select new DTO.Vaga
                    {
                        id = vaga.Idf_Vaga,
                        requisitos = vaga.Des_Requisito,
                        beneficios = vaga.Des_Beneficio,
                        funcao = funcao.Des_Funcao,
                        atribuicoes = vaga.Des_Atribuicoes,
                        dataAbertura = vaga.Dta_Abertura,
                        dataCadastro = vaga.Dta_Cadastro,
                        codigo = vaga.Cod_Vaga,
                        cidade = cidade.Nme_Cidade + "-" + cidade.Sig_Estado,
                        salario = "Salário de " + vaga.Vlr_Salario_De + "até" + vaga.Vlr_Salario_Para
                    });

                return query.Take(30).ToList();
            }
        }
    }
}
