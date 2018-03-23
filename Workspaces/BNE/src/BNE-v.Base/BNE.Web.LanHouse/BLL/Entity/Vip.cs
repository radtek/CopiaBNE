using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL.Entity
{
    public class Vip
    {
        #region Classes privadas
        private class VerificarCodigoDesconto
        {
            public string Erro { get; set; }
            public BNE.BLL.CodigoDesconto CodigoDesconto { get; private set; }
            public BNE.BLL.Plano Plano { get; set; }

            public VerificarCodigoDesconto(BNE.BLL.CodigoDesconto objCodigoDesconto)
            {
                this.CodigoDesconto = objCodigoDesconto;
                this.Erro = null;
            }

            public bool Verificar()
            {
                bool retorno = false;

                BNE.BLL.TipoCodigoDesconto objTipoCodigoDesconto;
                List<BNE.BLL.Plano> objPlanos;

                if (CodigoDesconto.JaUtilizado())
                    this.Erro = 
                        String.Format("O código promocional {0} já foi utilizado", this.CodigoDesconto.DescricaoCodigoDesconto);

                if (!CodigoDesconto.DentroValidade())
                    this.Erro =
                        String.Format("O código promocional {0} está fora do período de validade", this.CodigoDesconto.DescricaoCodigoDesconto);

                if (!CodigoDesconto.TipoDescontoDefinido(out objTipoCodigoDesconto))
                    this.Erro =
                        String.Format("O código promocional {0} não possui tipo definido", this.CodigoDesconto.DescricaoCodigoDesconto);

                if (!CodigoDesconto.HaPlanosVinculados(out objPlanos))
                    this.Erro =
                        String.Format("O código promocional {0} não possui planos vinculados", this.CodigoDesconto.DescricaoCodigoDesconto);

                if (objPlanos.Count() != 1)
                    this.Erro =
                        String.Format("O código promocional {0} só pode ter um plano vinculado, quantidade vinculada: {1}",
                            this.CodigoDesconto.DescricaoCodigoDesconto,
                            objPlanos.Count());

                if (String.IsNullOrEmpty(this.Erro))
                {
                    this.Plano = objPlanos.First();
                    retorno = true;
                }

                return retorno;
            }
        }
        #endregion Classes privadas

        #region Métodos públicos
        public static bool Autorizar(int idPessoaFisica, string codigoDesconto)
        {
            BNE_Curriculo objCurriculo;
            if (!Curriculo.CarregarPorPessoaFisica(idPessoaFisica, out objCurriculo))
                throw new ArgumentException(String.Format("Currículo da pessoa física \"{0}\" não encontrado", idPessoaFisica), "idPessoaFisica");

            BNE.BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
            if (!BNE.BLL.UsuarioFilialPerfil.CarregarPorPessoaFisica(idPessoaFisica, out objUsuarioFilialPerfil))
                throw new ArgumentException(String.Format("UsuárioFilialPerfil da pessoa física \"{0}\" não encontrado", idPessoaFisica), "idPessoaFisica"); 

            BNE.BLL.CodigoDesconto objCodigoDesconto;
            if (!BNE.BLL.CodigoDesconto.CarregarPorCodigo(codigoDesconto, out objCodigoDesconto))
                throw new ArgumentException("Código de desconto não existe!");

            VerificarCodigoDesconto contexto = 
                new VerificarCodigoDesconto(objCodigoDesconto);
            
            if (!contexto.Verificar())
                throw new InvalidOperationException(contexto.Erro);
            
            BNE.BLL.Plano objPlano = contexto.Plano;

            string erro;
            bool sucesso =
                BNE.BLL.Pagamento.ConcederDescontoIntegral(
                    new BNE.BLL.Curriculo(objCurriculo.Idf_Curriculo),
                    new BNE.BLL.UsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil),
                    new BNE.BLL.Plano(objPlano.IdPlano),
                    objCodigoDesconto,
                    out erro);

            if (!sucesso)
                throw new InvalidOperationException(erro);

            return true;
        }
        #endregion Métodos públicos
    }
}