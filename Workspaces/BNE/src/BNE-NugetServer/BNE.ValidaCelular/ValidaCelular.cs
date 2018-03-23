using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using BNE.ValidaCelular.Context;
using BNE.ValidaCelular.Model;

namespace BNE.ValidaCelular
{
    public class ValidaCelular : IValidaCelular
    {
        private enum Acao
        {
            Validar,
            Utilizar
        }
        public string GerarCodigo(string numeroDDD, string numeroCelular)
        {
            var core = new Core.Codigo();

            var codigoConfirmacao = core.GerarCodigo();
            using (var dbContext = new ValidaCelularContext())
            {
                var objCodigoConfirmacacaoCelular = new CodigoConfirmacaoCelular
                {
                    DataCriacao = DateTime.Now,
                    NumeroDDDCelular = numeroDDD,
                    NumeroCelular = numeroCelular,
                    CodigoConfirmacao = codigoConfirmacao
                };

                dbContext.CodigoConfirmacaoCelular.Add(objCodigoConfirmacacaoCelular);
                dbContext.SaveChanges();
            }
            return core.AplicarMascaraCodigo(codigoConfirmacao);
        }

        public bool ValidarCodigo(string numeroDDD, string numeroCelular, string codigoValidacao)
        {
            return UtilizarValidarCodigo(numeroDDD, numeroCelular, codigoValidacao, Acao.Validar);
        }

        public bool UtilizarCodigo(string numeroDDD, string numeroCelular, string codigoValidacao)
        {
            return UtilizarValidarCodigo(numeroDDD, numeroCelular, codigoValidacao, Acao.Utilizar);
        }

        private bool UtilizarValidarCodigo(string numeroDDD, string numeroCelular, string codigoValidacao, Acao enumAcao)
        {
            codigoValidacao = Regex.Replace(codigoValidacao, @"\s", "");

            using (var dbContext = new ValidaCelularContext())
            {
                var objCodigoConfirmacacaoCelular = dbContext.CodigoConfirmacaoCelular.FirstOrDefault(c => c.NumeroDDDCelular.Equals(numeroDDD) && c.NumeroCelular.Equals(numeroCelular) && c.CodigoConfirmacao.Equals(codigoValidacao));
                if (objCodigoConfirmacacaoCelular != null)
                {
                    if (objCodigoConfirmacacaoCelular.DataUtilizacao.HasValue)
                        throw new ArgumentException("Código de valição já foi usado para esse número!");

                    if (enumAcao.Equals(Acao.Utilizar))
                    {
                        objCodigoConfirmacacaoCelular.DataUtilizacao = DateTime.Now;
                        dbContext.Entry(objCodigoConfirmacacaoCelular).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    throw new ArgumentException("Código de valição não existe para esse número!");
                }
            }

            return true;
        }

        public bool CodigoEnviado(string numeroDDD, string numeroCelular, out DateTime? dataEnvio)
        {
            dataEnvio = null;

            bool retorno;
            using (var dbContext = new ValidaCelularContext())
            {
                var objCodigoConfirmacaoCelular = dbContext.CodigoConfirmacaoCelular.FirstOrDefault(c => c.NumeroDDDCelular.Equals(numeroDDD) && c.NumeroCelular.Equals(numeroCelular) && c.DataUtilizacao == null);

                if (objCodigoConfirmacaoCelular != null)
                {
                    retorno = true;
                    dataEnvio = objCodigoConfirmacaoCelular.DataCriacao;
                }
                else
                {
                    retorno = false;
                }
            }
            return retorno;
        }
    }
}
