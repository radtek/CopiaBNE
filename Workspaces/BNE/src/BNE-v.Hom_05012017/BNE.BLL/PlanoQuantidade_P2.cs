//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;
namespace BNE.BLL
{
    public partial class PlanoQuantidade // Tabela: BNE_Plano_Quantidade
    {

        #region Consultas
        private const string Spselectsaldovisualizacao = @" 
        SELECT  Qtd_Visualizacao - Qtd_Visualizacao_Utilizado
        FROM    BNE_Plano_Quantidade PQ
        WHERE   PQ.Idf_Plano_Quantidade = @Idf_Plano_Quantidade
                AND PQ.Flg_Inativo = 0";

        private const string Spselectsaldosms = @" 
        SELECT  PQ.Qtd_SMS - PQ.Qtd_SMS_Utilizado
        FROM    BNE_Plano_Quantidade PQ
        WHERE   PQ.Idf_Plano_Quantidade = @Idf_Plano_Quantidade
                AND PQ.Flg_Inativo = 0";

        private const string Spselectsaldocampanha = @" 
        SELECT  PQ.Qtd_Campanha - PQ.Qtd_Campanha_Utilizado
        FROM    BNE_Plano_Quantidade PQ
        WHERE   PQ.Idf_Plano_Quantidade = @Idf_Plano_Quantidade
                AND PQ.Flg_Inativo = 0";

        private const string Spdescontarsaldovisualizacao = @" 
        UPDATE  PQ
        SET     PQ.Qtd_Visualizacao_Utilizado = PQ.Qtd_Visualizacao_Utilizado + 1
        FROM    BNE_Plano_Quantidade PQ
        WHERE   PQ.Idf_Plano_Quantidade = @Idf_Plano_Quantidade
                AND PQ.Flg_Inativo = 0";

        private const string Spdescontarsaldosms = @" 
        UPDATE  PQ
        SET     PQ.Qtd_SMS_Utilizado = PQ.Qtd_SMS_Utilizado + 1
        FROM    BNE_Plano_Quantidade PQ
        WHERE   PQ.Idf_Plano_Quantidade = @Idf_Plano_Quantidade
                AND PQ.Flg_Inativo = 0";

        private const string Spdescontarsaldocampanha = @" 
        UPDATE  PQ
        SET     PQ.Qtd_Campanha_Utilizado = PQ.Qtd_Campanha_Utilizado + 1
        FROM    BNE_Plano_Quantidade PQ
        WHERE   PQ.Idf_Plano_Quantidade = @Idf_Plano_Quantidade
                AND PQ.Flg_Inativo = 0";

        private const string Spselectidentificadorplanoquantidade = @"
        SELECT  PQ.Idf_Plano_Quantidade
        FROM    BNE_Plano_Adquirido PA
                INNER JOIN BNE_Plano_Quantidade PQ ON PA.Idf_Plano_Adquirido = PQ.Idf_Plano_Adquirido
        WHERE   PA.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
                AND PQ.Flg_Inativo = 0";

        private const string Spselectplanoquantidade = @"
        SELECT  PQ.*
        FROM    BNE_Plano_Adquirido PA
                INNER JOIN BNE_Plano_Quantidade PQ ON PA.Idf_Plano_Adquirido = PQ.Idf_Plano_Adquirido
        WHERE   PA.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
                AND PQ.Flg_Inativo = 0";

        #region Spplanosquantidadeporplanoadquirido
        private const string Spplanosquantidadeporplanoadquirido = @"
        SELECT 
                PQ.* 
        FROM    BNE_Plano_Quantidade AS PQ WITH(NOLOCK) 
        WHERE   Idf_Plano_Adquirido = @Idf_Plano_Adquirido";
        #endregion

        #region SPDESTRAVASMSPLANOEMPLOYER

        private const string SPDESTRAVASMSPLANOEMPLOYER = @"
        UPDATE PQ
        SET PQ.Qtd_SMS = PQ.Qtd_SMS + 200   -- recarrega 200 SMS
           ,PQ.Dta_Alteracao = GETDATE()    -- registro esta sendo alterado agora
        FROM BNE_Plano_Quantidade PQ
        JOIN BNE_Plano_Adquirido PA
             ON PA.Idf_Plano_Adquirido = PQ.Idf_Plano_Adquirido
        WHERE 1 = 1
        AND   PQ.Flg_Inativo = 0            -- ativo?
        AND   PA.Idf_Plano_Situacao = 1     -- liberado?
        AND   GETDATE() BETWEEN PQ.Dta_Inicio_Quantidade AND PQ.Dta_Fim_Quantidade
                                            -- vigente?
        AND   PA.Idf_Plano = 24             -- Plano Employer?
";

        #endregion

        #endregion

        #region Métodos

        #region Saldo

        #region SaldoSMS
        /// <summary>
        /// Recupera o saldo vigente de sms para o plano quantidade atual
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int SaldoSMS(SqlTransaction trans = null)
        {
            int retorno = 0;

            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Plano_Quantidade", SqlDbType = SqlDbType.Int, Size = 4, Value = this._idPlanoQuantidade }
            };

            var obj = DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spselectsaldosms, parms);
            if (obj != DBNull.Value)
            {
                Int32.TryParse(obj.ToString(), out retorno);
            }

            return retorno;
        }
        #endregion

        #region SaldoVisualizacao
        /// <summary>
        /// Recupera o saldo vigente de sms para o plano quantidade atual
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int SaldoVisualizacao(SqlTransaction trans = null)
        {
            int retorno = 0;

            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Plano_Quantidade", SqlDbType = SqlDbType.Int, Size = 4, Value = this._idPlanoQuantidade }
            };

            var obj = DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spselectsaldovisualizacao, parms);
            if (obj != DBNull.Value)
            {
                Int32.TryParse(obj.ToString(), out retorno);
            }

            if (retorno < 0)
                retorno = 0;

            return retorno;
        }
        #endregion

        #region SaldoCampanha
        /// <summary>
        /// Recupera o saldo vigente de campanha para o plano quantidade atual
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int SaldoCampanha(SqlTransaction trans = null)
        {
            int retorno = 0;

            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Plano_Quantidade", SqlDbType = SqlDbType.Int, Size = 4, Value = this._idPlanoQuantidade }
            };

            var obj = DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spselectsaldocampanha, parms);
            if (obj != DBNull.Value)
            {
                Int32.TryParse(obj.ToString(), out retorno);
            }

            return retorno;
        }
        #endregion

        #endregion

        #region Desconto de Saldo

        #region DescontarVisualizacao
        public void DescontarVisualizacao(SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Plano_Quantidade", SqlDbType = SqlDbType.Int, Size = 4, Value = this._idPlanoQuantidade }
            };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spdescontarsaldovisualizacao, parms);
        }
        #endregion

        #region DescontarSMS
        public void DescontarSMS(SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Plano_Quantidade", SqlDbType = SqlDbType.Int, Size = 4, Value = this._idPlanoQuantidade }
            };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spdescontarsaldosms, parms);
        }
        #endregion

        #region DescontarCampanha
        public void DescontarCampanha(SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Plano_Quantidade", SqlDbType = SqlDbType.Int, Size = 4, Value = this._idPlanoQuantidade }
            };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spdescontarsaldocampanha, parms);
        }
        #endregion

        #endregion

        #region CarregarPorPlanoAdquirido
        /// <summary>   
        /// Método responsável por carregar uma instância de PlanoQuantidade vigente por uma filial
        /// </summary>
        /// <returns>Objeto Plano Quantidade</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPlanoAdquirido(PlanoAdquirido objPlanoAdquirido, out PlanoQuantidade objPlanoQuantidade, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlanoAdquirido.IdPlanoAdquirido }
                };

            IDataReader dr;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectplanoquantidade, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectplanoquantidade, parms);

            objPlanoQuantidade = new PlanoQuantidade();
            if (SetInstance(dr, objPlanoQuantidade))
                return true;
            return false;
        }
        #endregion

        #region ReiniciarContagemSaldoRecorrencia
        public static void ReiniciarContagemSaldo(PlanoAdquirido objPlanoAdquirido, SqlTransaction trans = null)
        {
            PlanoQuantidade objPlanoQuantidade = null;

            objPlanoAdquirido.CompleteObject();
            objPlanoAdquirido.Plano.CompleteObject();

            if (CarregarPorPlanoAdquirido(objPlanoAdquirido, out objPlanoQuantidade, trans))
            {
                objPlanoQuantidade.QuantidadeSMSUtilizado = 0;
                objPlanoQuantidade.QuantidadeVisualizacaoUtilizado = 0;
            }
            else
            {
                objPlanoQuantidade.DataInicioQuantidade = objPlanoAdquirido.DataInicioPlano;
                objPlanoQuantidade.DataFimQuantidade = objPlanoAdquirido.DataFimPlano;
                objPlanoQuantidade.QuantidadeSMSUtilizado = 0;
                objPlanoQuantidade.QuantidadeVisualizacaoUtilizado = 0;
                objPlanoQuantidade.PlanoAdquirido = objPlanoAdquirido;
                objPlanoQuantidade.QuantidadeSMS = objPlanoAdquirido.QuantidadeSMS;
                objPlanoQuantidade.QuantidadeVisualizacao = objPlanoAdquirido.Plano.QuantidadeVisualizacao;
            }

            if (trans != null)
                objPlanoQuantidade.Save(trans);
            else
                objPlanoQuantidade.Save();

            objPlanoAdquirido.Filial.CompleteObject();
            CelularSelecionador.HabilitarDesabilitarUsuarios(objPlanoAdquirido.Filial);

        }
        #endregion


        #region ReiniciarContagemSaldoRecorrencia
        public static void ReiniciarContagemSaldoRecorrencia(PlanoAdquirido objPlanoAdquirido,DateTime datafim, SqlTransaction trans = null)
        {
            PlanoQuantidade objPlanoQuantidade = null;
            if (CarregarPorPlanoAdquirido(objPlanoAdquirido, out objPlanoQuantidade, trans))
            {
                objPlanoQuantidade.DataInicioQuantidade = objPlanoAdquirido.DataFimPlano.AddMonths(-1);
                objPlanoQuantidade.DataFimQuantidade = datafim;
                objPlanoQuantidade.QuantidadeSMSUtilizado = 0;
                objPlanoQuantidade.QuantidadeVisualizacaoUtilizado = 0;
            }
            else
            {
                if (objPlanoAdquirido.Plano != null)
                    objPlanoAdquirido.CompleteObject();
                objPlanoAdquirido.Plano.CompleteObject();

                objPlanoQuantidade.DataInicioQuantidade = objPlanoAdquirido.DataFimPlano.AddMonths(-1);
                objPlanoQuantidade.DataFimQuantidade = datafim;
                objPlanoQuantidade.QuantidadeSMSUtilizado = 0;
                objPlanoQuantidade.QuantidadeVisualizacaoUtilizado = 0;
                objPlanoQuantidade.PlanoAdquirido = objPlanoAdquirido;
                objPlanoQuantidade.QuantidadeSMS = objPlanoAdquirido.Plano.QuantidadeSMS;
                objPlanoQuantidade.QuantidadeVisualizacao = objPlanoAdquirido.Plano.QuantidadeVisualizacao;
            }

            if (trans != null)
                objPlanoQuantidade.Save(trans);
            else
                objPlanoQuantidade.Save();

            objPlanoAdquirido.Filial.CompleteObject();
            CelularSelecionador.HabilitarDesabilitarUsuarios(objPlanoAdquirido.Filial);

        }
        #endregion

        #region EncerrarPlanosQuantidadePorPlanoAdquirido
        /// <summary>
        /// Metodo resposável por encerrar todos os planos quantidade de um plano adquirido
        /// </summary>
        public void EncerrarPlanosQuantidadePorPlanoAdquirido(PlanoAdquirido objPlanoAdquirido)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        EncerrarPlanosQuantidadePorPlanoAdquirido(objPlanoAdquirido, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        public static void EncerrarPlanosQuantidadePorPlanoAdquirido(PlanoAdquirido objPlanoAdquirido, SqlTransaction trans)
        {
            List<PlanoQuantidade> listPlanoQuantidade = ListaPlanosQuantidadePorPlanoAdquirido(objPlanoAdquirido, trans);

            foreach (PlanoQuantidade objPlanoQuantidade in listPlanoQuantidade)
            {
                objPlanoQuantidade.FlagInativo = true;
                objPlanoQuantidade.Save(trans);
            }
        }
        #endregion

        #region ListaPlanosQuantidadePorPlanoAdquirido
        public static List<PlanoQuantidade> ListaPlanosQuantidadePorPlanoAdquirido(PlanoAdquirido objPlanoAdquirido, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlanoAdquirido.IdPlanoAdquirido }
                };

            var lstPlanoQuantidade = new List<PlanoQuantidade>();
            var objPlanoQuantidade = new PlanoQuantidade();

            IDataReader dr;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spplanosquantidadeporplanoadquirido, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spplanosquantidadeporplanoadquirido, parms);

            while (SetInstance(dr, objPlanoQuantidade, false))
            {
                lstPlanoQuantidade.Add(objPlanoQuantidade);
                objPlanoQuantidade = new PlanoQuantidade();
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return lstPlanoQuantidade;
        }
        #endregion

        #region RecarregarSMS
        /// <summary>
        /// Recarrega SMS
        /// </summary>
        /// <param name="idPlanoAdquirido">objeto PlanoAdquirido</param>
        /// <param name="qtdeARecarregar">quantidade de SMSs a recarregar</param>
        /// <param name="trans">transação SQL Server</param>
        /// <returns>true se conseguiu, false se não</returns>
        public static bool RecarregarSMS(PlanoAdquirido objPlanoAdquirido, int qtdeARecarregar, SqlTransaction trans = null)
        {
            PlanoQuantidade objPlanoQuantidade;
            if (PlanoQuantidade.CarregarPorPlanoAdquirido(objPlanoAdquirido, out objPlanoQuantidade, trans))
            {
                objPlanoQuantidade.QuantidadeSMS += qtdeARecarregar;

                if (null == trans)
                    objPlanoQuantidade.Save();
                else
                    objPlanoQuantidade.Save(trans);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Recarrega SMS
        /// </summary>
        /// <param name="objAdicionalPlano">objeto AdicionalPlano</param>
        /// <param name="trans">transação SQL Server</param>
        /// <returns>true se conseguiu, false se não</returns>
        public static bool RecarregarSMS(AdicionalPlano objAdicionalPlano, SqlTransaction trans = null)
        {
            if (null == objAdicionalPlano.PlanoAdquirido)
            {
                if (null == trans)
                    objAdicionalPlano.CompleteObject();
                else
                    objAdicionalPlano.CompleteObject(trans);
            }

            return
                RecarregarSMS(objAdicionalPlano.PlanoAdquirido, objAdicionalPlano.QuantidadeAdicional, trans);
        }
        #endregion

        #region RecarregarSMSPlanoEmployer
        /// <summary>
        /// Recarrega 200 SMSs para cada filial Employer vigente
        /// </summary>
        /// <param name="trans">transação SQL Server</param>
        /// <returns>qtde de filiais afetadas</returns>
        public static int RecarregarSMSPlanoEmployer(SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>();

            if (trans != null)
                return DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDESTRAVASMSPLANOEMPLOYER, parms);
            else
                return DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDESTRAVASMSPLANOEMPLOYER, parms);
        }

        #endregion

        #region Construtor
        public PlanoQuantidade(PlanoAdquirido objPlanoAdquirido, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlanoAdquirido.IdPlanoAdquirido }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectidentificadorplanoquantidade, parms))
            {
                if (dr.Read())
                {
                    this._idPlanoQuantidade = Convert.ToInt32(dr["Idf_Plano_Quantidade"]);
                }
            }
        }
        #endregion

        #endregion

    }
}