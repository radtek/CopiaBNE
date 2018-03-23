using System;
using System.Collections.Generic;
using System.Linq;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL
{
    public static class Funcao
    {
        private static LanEntities entity = new LanEntities();

        public static bool CarregarPorDescricao(string descricao, out TAB_Funcao objFuncao)
        {
            objFuncao =
                entity
                    .TAB_Funcao
                    .FirstOrDefault(f => f.Des_Funcao == descricao);

            if (objFuncao == null)
            {
                BNE_Funcao_Erro_Sinonimo objFuncaoErroSinonimo =
                    entity
                        .BNE_Funcao_Erro_Sinonimo
                        .Include("TAB_Funcao")
                        .FirstOrDefault(fes => fes.Des_Funcao_Erro_Sinonimo == descricao);

                if (objFuncaoErroSinonimo != null)
                    objFuncao = objFuncaoErroSinonimo.TAB_Funcao;
            }

            return objFuncao != null;
        }

        public static bool CarregarPorDescricao(string descricao, out IEnumerable<TAB_Funcao> objFuncoes)
        {
            if (descricao.Length < Parametro.NumeroLetrasInicioAutoCompleteFuncao())
                throw new ArgumentException(
                    String.Format("Descrição da função precisa ter no mínimo {0} caracteres", Parametro.NumeroLetrasInicioAutoCompleteFuncao()),
                    "descricao");

            objFuncoes =
                entity
                    .TAB_Funcao
                    .Where(f => f.Des_Funcao.StartsWith(descricao))
                    .OrderBy(f => f.Des_Funcao)
                    .ToList();

            if (!objFuncoes.Any())
            {
                objFuncoes =
                    entity
                        .BNE_Funcao_Erro_Sinonimo
                        .Include("TAB_Funcao")
                        .Where(fes => fes.Des_Funcao_Erro_Sinonimo.StartsWith(descricao))
                        .Select(fes => fes.TAB_Funcao)
                        .Distinct()
                        .OrderBy(f => f.Des_Funcao)
                        .ToList();
            }

            return objFuncoes.Any();
        }

        public static bool CarregarPorId(int idFuncao, out TAB_Funcao objFuncao)
        {
            objFuncao =
                entity
                    .TAB_Funcao
                    .Find(idFuncao);

            return objFuncao != null;
        }
    }
}