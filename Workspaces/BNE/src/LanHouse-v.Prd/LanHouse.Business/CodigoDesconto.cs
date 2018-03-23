using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class CodigoDesconto
    {
        #region CarregarPorId
        public static bool CarregarPorId(int idCodigo, out BNE_Codigo_Desconto objCodigoDesconto)
        {
            using (var entity = new LanEntities())
            {
                objCodigoDesconto = (from codDesc in entity.BNE_Codigo_Desconto
                                        where codDesc.Idf_Codigo_Desconto == idCodigo
                                        select codDesc).FirstOrDefault();

                return objCodigoDesconto != null;

            }
        }
        #endregion

        #region CarregarPorCodigo
        public static bool CarregarPorCodigo(string codigo, out BNE_Codigo_Desconto objCodigoDesconto)
        {
            using (var entity = new LanEntities())
            {
                objCodigoDesconto = (from codDesc in entity.BNE_Codigo_Desconto
                                        where codDesc.Des_Codigo_Desconto == codigo
                                        select codDesc).FirstOrDefault();

                return objCodigoDesconto != null;
            }
        }
        #endregion

        #region ValidarCodigo
        public static string ValidarCodigo(BNE_Codigo_Desconto objCodigoDesconto, out BNE_Plano objPlano)
        {
            objPlano = new BNE_Plano();
            BNE_Tipo_Codigo_Desconto objTipoCodigoDesconto;
            List<BNE_Plano> lstPlanos;

            if (JaUtilizado(objCodigoDesconto))
                return String.Format("O código promocional {0} já foi utilizado", objCodigoDesconto.Des_Codigo_Desconto);

            if (!DentroValidade(objCodigoDesconto))
                return String.Format("O código promocional {0} está fora do período de validade", objCodigoDesconto.Des_Codigo_Desconto);

            if (!TipoDescontoDefinido(objCodigoDesconto, out objTipoCodigoDesconto))
                return String.Format("O código promocional {0} não possui tipo definido", objCodigoDesconto.Des_Codigo_Desconto);

            if (!HaPlanosVinculados(objCodigoDesconto, out lstPlanos))
                return String.Format("O código promocional {0} não possui planos vinculados", objCodigoDesconto.Des_Codigo_Desconto);

            if (lstPlanos.Count() <= 0)
                return String.Format("O código promocional {0} só pode ter um plano vinculado, quantidade vinculada: {1}",
                        objCodigoDesconto.Des_Codigo_Desconto,
                        lstPlanos.Count());

            objPlano = lstPlanos.First();
            return "";
        }
        #endregion

        #region JaUtilizado
        public static bool JaUtilizado(BNE_Codigo_Desconto objCodigoDesconto)
        {
            return objCodigoDesconto.Idf_Status_Codigo_Desconto == (int)Enumeradores.StatusCodigoDesconto.Utilizado;
        }
        #endregion

        #region Reutilizavel
        public static bool Reutilizavel(BNE_Codigo_Desconto objCodigoDesconto)
        {
            return objCodigoDesconto.Idf_Status_Codigo_Desconto == (int)Enumeradores.StatusCodigoDesconto.Reutilizavel;
        }
        #endregion

        #region DentroValidade
        private static bool DentroValidade(BNE_Codigo_Desconto objCodigoDesconto)
        {
            return (objCodigoDesconto.Dta_Validade_Inicio == null || objCodigoDesconto.Dta_Validade_Inicio <= DateTime.Now) &&
                (objCodigoDesconto.Dta_Validade_Fim == null || objCodigoDesconto.Dta_Validade_Fim >= DateTime.Now);
        }
        #endregion

        #region TipoDescontoDefinido
        private static bool TipoDescontoDefinido(BNE_Codigo_Desconto objCodigoDesconto, out BNE_Tipo_Codigo_Desconto objTipoCodigoDesconto)
        {
            objTipoCodigoDesconto = null;

            if (objCodigoDesconto.Idf_Tipo_Codigo_Desconto == null)
                return false;

            return TipoCodigoDesconto.CarregarPorCodigo((int)objCodigoDesconto.Idf_Tipo_Codigo_Desconto, out objTipoCodigoDesconto);
        }
        #endregion

        #region HaPlanosVinculados
        private static bool HaPlanosVinculados(BNE_Codigo_Desconto objCodigoDesconto, out List<BNE_Plano> lstPlanos)
        {
            if (objCodigoDesconto.Idf_Tipo_Codigo_Desconto == null)
            {
                lstPlanos = null;
                return false;
            }

            
            return Plano.CarregarPorTipoCodigoDesconto(
                (int)objCodigoDesconto.Idf_Tipo_Codigo_Desconto,
                out lstPlanos);
        }
        #endregion

        #region CalcularDesconto
        /// <summary>
        /// Calcula o valor já com o desconto concedido pelo objeto CodigoDesconto
        /// </summary>
        /// <param name="valorOriginal">valor original do qual se quer descontar</param>
        /// <returns>false se o tipo do código de desconto for null, true se conseguiu calcular</returns>
        public static bool CalcularDesconto(ref decimal valorOriginal, BNE_Codigo_Desconto objCodigoDesconto, BNE_Plano objPlano = null)
        {
            // obtenção do percentual de desconto:
            // a ideia quando se informa um objeto Plano é "especializar" o desconto
            // caso não haja "especialização", é usado o valor do objeto TipoCodigoDesconto como generalização do desconto
            decimal percentualDesconto = objCodigoDesconto.BNE_Tipo_Codigo_Desconto.Num_Percentual_Desconto;   // valor geral usado a menos que haja especialização

            if (null == objPlano)
            {
                // nao foi informado plano, usa valor existente no TipoCodigoDesconto
            }
            else
            {
                // foi informado um plano
                try
                {
                    BNE_Codigo_Desconto_Plano objCodigoDescontoPlano =
                        CodigoDescontoPlano.CarregarPorPlanoETipoCodigoDesconto(objCodigoDesconto.BNE_Tipo_Codigo_Desconto.Idf_Tipo_Codigo_Desconto, objPlano.Idf_Plano);

                    if (objCodigoDescontoPlano.Num_Percentual_Desconto.HasValue)
                    {
                        // usa especialização
                        percentualDesconto = objCodigoDescontoPlano.Num_Percentual_Desconto.Value;
                    }
                    else
                    {
                        // usa valor existente no TipoCodigoDesconto
                    }
                }
                catch
                {
                    // usa valor existente no TipoCodigoDesconto
                }
            }

            // a partir daqui, aplica o desconto
            if (percentualDesconto >= 1m)
            {
                // calcula novo valor com desconto e sobrepoe o valor original
                valorOriginal *= (100m - percentualDesconto) / 100m;

                if (valorOriginal < 0.01m)
                    valorOriginal = Decimal.Zero;
            }

            return true;
        }
        #endregion

        #region Utilizar
        public static void Utilizar(BNE_Codigo_Desconto objCodigoDesconto, LanEntities context)
        {
            if (Reutilizavel(objCodigoDesconto))
                return;

            objCodigoDesconto.Idf_Status_Codigo_Desconto = (int)Enumeradores.StatusCodigoDesconto.Utilizado;
            objCodigoDesconto.Dta_Utilizacao = DateTime.Now;
            context.SaveChanges();
        }
        #endregion
    }
}
