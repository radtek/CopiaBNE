using System;
using System.Collections.Generic;
using System.Linq;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL
{
    public static class PessoaFisica
    {

        #region Campos
        private static LanEntities entity = new LanEntities();
        #endregion

        #region ContarCelularesRepetidos
        public static int ContarCelularesRepetidos(string ddd, string numCelular)
        {
            entity = new LanEntities();

            return entity.TAB_Pessoa_Fisica.Count(pf => pf.Num_DDD_Celular == ddd && pf.Num_Celular == numCelular);
        }
        #endregion ContarCelularesRepetidos

        #region CarregarPorCpfDataNascimento
        public static bool CarregarPorCpfDataNascimento(decimal cpf, DateTime dataNasc, out TAB_Pessoa_Fisica objPessoaFisica)
        {
            entity = new LanEntities();

            DateTime dataNascimento = dataNasc.Date;

            objPessoaFisica = entity.TAB_Pessoa_Fisica.FirstOrDefault(pf => pf.Num_CPF == cpf && pf.Dta_Nascimento == dataNascimento);

            return objPessoaFisica != null;
        }
        #endregion CarregarPorCpfDataNascimento

        #region CarregarPorCpf
        public static bool CarregarPorCpf(decimal cpf, out TAB_Pessoa_Fisica objPessoaFisica)
        {
            entity = new LanEntities();

            objPessoaFisica = entity.TAB_Pessoa_Fisica.FirstOrDefault(pf => pf.Num_CPF == cpf);

            return objPessoaFisica != null;
        }
        #endregion CarregarPorCpf

        #region CarregarPorId
        public static bool CarregarPorId(int idPessoaFisica, out TAB_Pessoa_Fisica objPessoaFisica)
        {
            entity = new LanEntities();

            objPessoaFisica =
                entity
                .TAB_Pessoa_Fisica
                .Find(idPessoaFisica);

            return objPessoaFisica != null;
        }
        #endregion

    }
}