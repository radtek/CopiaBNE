using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using BNE.EL;

namespace BNE.BLL
{
    public partial class CodigoDesconto
    {
        #region Consultas

        #region Spcarregarporcodigo
        private const string Spcarregarporcodigo = @"
        SELECT *
        FROM BNE.BNE_Codigo_Desconto WITH (NOLOCK)
        WHERE Des_Codigo_Desconto COLLATE Latin1_General_CI_AS = @Des_Codigo_Desconto
";
        #endregion

        #endregion

        #region Métodos
        /// <summary>
        /// Carrega o codigo de desconto pelo valor no campo Des_Codigo_Desconto da tabela
        /// </summary>
        /// <param name="desCodigoDesconto">valor desejado para o campo Des_Codigo_Desconto</param>
        /// <param name="objCodigoDesconto">saída do objeto CodigoDesconto</param>
        /// <returns>true se conseguiu carregar corretamente, false se não tiver conseguido ou se o objeto for inativo ou cancelado</returns>
        public static bool CarregarPorCodigo(string desCodigoDesconto, out CodigoDesconto objCodigoDesconto)
        {
            bool retorno = false;

            var parms = new List<SqlParameter>
			{
				new SqlParameter("@Des_Codigo_Desconto", SqlDbType.VarChar, 200)
			};

            parms[0].Value = desCodigoDesconto;

            using (IDataReader dr = 
                DataAccessLayer.ExecuteReader(CommandType.Text, Spcarregarporcodigo, parms))
            {
                objCodigoDesconto = new CodigoDesconto();
                if (SetInstance(dr, objCodigoDesconto))
                {
                    if (objCodigoDesconto.StatusCodigoDesconto.IdStatusCodigoDesconto ==
                        (int)Enumeradores.StatusCodigoDesconto.Inativo ||
                        objCodigoDesconto.StatusCodigoDesconto.IdStatusCodigoDesconto ==
                        (int)Enumeradores.StatusCodigoDesconto.Cancelado)
                        
                        retorno = false;

                    else
                        retorno = true;
                }
                else
                {
                    objCodigoDesconto = null;
                    retorno = false;
                }
            }

            return retorno;
        }

        public bool JaUtilizado()
        {
            if (StatusCodigoDesconto == null)
                CompleteObject();

            return StatusCodigoDesconto.IdStatusCodigoDesconto == (int)Enumeradores.StatusCodigoDesconto.Utilizado;
        }

        public bool Reutilizavel()
        {
            if (StatusCodigoDesconto == null)
                CompleteObject();

            return StatusCodigoDesconto.IdStatusCodigoDesconto == (int)Enumeradores.StatusCodigoDesconto.Reutilizavel;
        }

        public bool DentroValidade()
        {
            if (DataValidadeInicio == null || DataValidadeFim == null)
                CompleteObject();

            return (DataValidadeInicio == null || DataValidadeInicio <= DateTime.Now) &&
                (DataValidadeFim == null || DataValidadeFim >= DateTime.Now);
        }

        public bool TipoDescontoDefinido(out TipoCodigoDesconto tipo)
        {
            if (TipoCodigoDesconto == null)
                CompleteObject();

            bool retorno = TipoCodigoDesconto != null;

            tipo = TipoCodigoDesconto;

            return retorno;
        }

        public bool HaPlanosVinculados(out List<Plano> planos)
        {
            CompleteObject();

            if (TipoCodigoDesconto == null)
            {
                planos = null;
                return false;
            }

            return CodigoDescontoPlano.CarregarPorTipoCodigoDesconto(
                TipoCodigoDesconto,
                out planos);
        }

        /// <summary>
        /// Marca o objeto CodigoDesconto com o status de utilizado
        /// </summary>
        /// <param name="trans">transação SQL Server</param>
        public void Utilizar(SqlTransaction trans, int? idUsuarioFilialPerfil = null)
        {
            if (Reutilizavel())
                return;

            if (JaUtilizado())
                throw new InvalidOperationException("Esse código de desconto já foi utilizado: '" + DescricaoCodigoDesconto + "'");

            StatusCodigoDesconto = 
                new StatusCodigoDesconto((int)Enumeradores.StatusCodigoDesconto.Utilizado);
            DataUtilizacao =
                DateTime.Now;

            if (idUsuarioFilialPerfil.HasValue)
                UsuarioFilialPerfil = new UsuarioFilialPerfil(idUsuarioFilialPerfil.Value);

            if (trans != null)
                Save(trans);
            else
                Save();
        }

        /// <summary>
        /// Marca o objeto CodigoDesconto com o status de utilizado
        /// </summary>
        public void Utilizar()
        {
            Utilizar(null);
        }

        /// <summary>
        /// O codigo de desconto permite edição do plano CIA?
        /// </summary>
        /// <returns>true se sim, false se não</returns>
        public bool EdicaoPlanoCIA()
        {
            TipoCodigoDesconto objTipoCodigoDesconto;

            return TipoDescontoDefinido(out objTipoCodigoDesconto) &&
                objTipoCodigoDesconto.IdTipoCodigoDesconto == (int)Enumeradores.TipoCodigoDesconto.EdicaoPlanoCIA;
        }

        /// <summary>
        /// O codigo de desconto é de 3%
        /// </summary>
        /// <returns>true se sim, false se não</returns>
        public bool Desconto03PlanoCIA()
        {
            TipoCodigoDesconto objTipoCodigoDesconto;

            return TipoDescontoDefinido(out objTipoCodigoDesconto) &&
                objTipoCodigoDesconto.IdTipoCodigoDesconto == (int)Enumeradores.TipoCodigoDesconto.Desconto03;
        }

        /// <summary>
        /// O codigo de desconto é de 15%
        /// </summary>
        /// <returns>true se sim, false se não</returns>
        public bool Desconto15PlanoCIA()
        {
            TipoCodigoDesconto objTipoCodigoDesconto;

            return TipoDescontoDefinido(out objTipoCodigoDesconto) &&
                objTipoCodigoDesconto.IdTipoCodigoDesconto == (int)Enumeradores.TipoCodigoDesconto.Desconto15;
        }

        /// <summary>
        /// O codigo de desconto é de 40%
        /// </summary>
        /// <returns>true se sim, false se não</returns>
        public bool Desconto40PlanoCIA()
        {
            TipoCodigoDesconto objTipoCodigoDesconto;

            return TipoDescontoDefinido(out objTipoCodigoDesconto) &&
                objTipoCodigoDesconto.IdTipoCodigoDesconto == (int)Enumeradores.TipoCodigoDesconto.Desconto40;
        }

        /// <summary>
        /// O codigo de desconto é para conceder vantagem à CIA?
        /// </summary>
        /// <returns>true se sim, false se não</returns>
        public bool VantagemCIA()
        {
            TipoCodigoDesconto objTipoCodigoDesconto;

            return TipoDescontoDefinido(out objTipoCodigoDesconto) &&
                objTipoCodigoDesconto.IdTipoCodigoDesconto == (int)Enumeradores.TipoCodigoDesconto.VantagemCIA;
        }

        /// <summary>
        /// Calcula o valor já com o desconto concedido pelo objeto CodigoDesconto
        /// </summary>
        /// <param name="valorOriginal">valor original do qual se quer descontar</param>
        /// <returns>false se o tipo do código de desconto for null, true se conseguiu calcular</returns>
        public bool CalcularDesconto(ref decimal valorOriginal, Plano objPlano = null)
        {
            CompleteObject();

            if (TipoCodigoDesconto == null)
                return false;

            TipoCodigoDesconto.CompleteObject();

            // obtenção do percentual de desconto:
            // a ideia quando se informa um objeto Plano é "especializar" o desconto
            // caso não haja "especialização", é usado o valor do objeto TipoCodigoDesconto como generalização do desconto
            decimal percentualDesconto = TipoCodigoDesconto.NumeroPercentualDesconto;   // valor geral usado a menos que haja especialização

            if (null == objPlano)
            {
                // nao foi informado plano, usa valor existente no TipoCodigoDesconto
            }
            else
            {
                // foi informado um plano
                //Caso o plano seja Recorrente VIp não faz o calculo
                if (objPlano.IdPlano != Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoRecorrenteVIP)))
                {
                    try
                    {
                        CodigoDescontoPlano objCodigoDescontoPlano =
                            CodigoDescontoPlano.LoadObject(TipoCodigoDesconto.IdTipoCodigoDesconto, objPlano.IdPlano);

                        if (objCodigoDescontoPlano.NumeroPercentualDesconto.HasValue)
                        {
                            // usa especialização
                            percentualDesconto = objCodigoDescontoPlano.NumeroPercentualDesconto.Value;
                        }
                        else
                        {
                            // usa valor existente no TipoCodigoDesconto
                        }
                    }
                    catch (RecordNotFoundException)
                    {
                        // usa valor existente no TipoCodigoDesconto
                    }
                }
            }

            // a partir daqui, aplica o desconto
            if (percentualDesconto >= 1m)
            {
                // calcula novo valor com desconto e sobrepoe o valor original
                valorOriginal = Decimal.Round(valorOriginal *= (100m - percentualDesconto) / 100m);

                if (valorOriginal < 0.01m)
                    valorOriginal = Decimal.Zero;
            }

            return true;
        }

        /// <summary>
        /// Calcula a nova quantidade de SMS concedida pelo objeto CodigoDesconto
        /// </summary>
        /// <param name="qtdeOriginal">Quantidade original de SMS</param>
        /// <param name="objPlano">plano referente à quantidade original de SMS</param>
        /// <returns>true se conseguiu calcular, senão false</returns>
        public bool CalcularSMS(ref int qtdeOriginal, Plano objPlano)
        {
            CompleteObject();

            if (TipoCodigoDesconto == null)
                return false;

            CodigoDescontoPlano objCodigoDescontoPlano;
            try
            {
                objCodigoDescontoPlano =
                    CodigoDescontoPlano.LoadObject(TipoCodigoDesconto.IdTipoCodigoDesconto, objPlano.IdPlano);
            }
            catch (RecordNotFoundException)
            {
                return false;
            }

            if (!objCodigoDescontoPlano.QuantidadeSMS.HasValue)
                return false;

            // o cálculo do sms está abaixo e consiste em pegar o valor que está na tabela Codigo_Desconto_Plano e 
            // substitui-lo no lugar da quantidade original
            qtdeOriginal = objCodigoDescontoPlano.QuantidadeSMS.Value;

            return true;
        }

        #endregion
    }
}
