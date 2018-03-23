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
        private const string Spselectsaldo = @" 
        SELECT  PQ.*
        FROM    BNE_Plano_Quantidade PQ WITH (NOLOCK)                                                        
                INNER JOIN BNE_Plano_Adquirido PA WITH (NOLOCK) ON PQ.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
        WHERE   PA.Idf_Filial = @Idf_Filial 
                AND CONVERT(DATETIME, CONVERT(VARCHAR, PA.Dta_Fim_Plano, 103), 103) >= CONVERT(DATETIME, CONVERT(VARCHAR, PQ.Dta_Inicio_Quantidade, 103) , 103) 
                AND PA.Idf_Plano_Situacao = 1 /* Liberado */
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

        #region CarregarPlanoAtualVigente
        /// <summary>   
        /// Método responsável por carregar uma instância de PlanoQuantidade vigente por uma filial
        /// </summary>
        /// <param name="idFilial">Valor de Filial</param>
        /// <param name="objPlanoQuantidade"> Instância do Plano Quantidade </param>
        /// <returns>Objeto Plano Quantidade</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPlanoAtualVigente(int idFilial, out PlanoQuantidade objPlanoQuantidade)
        {
            return CarregarPlanoAtualVigente(null, new Filial(idFilial), out objPlanoQuantidade);
        }
        public static bool CarregarPlanoAtualVigente(SqlTransaction trans, Filial objFilial, out PlanoQuantidade objPlanoQuantidade)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectsaldo, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectsaldo, parms);

            bool retorno = false;
            objPlanoQuantidade = new PlanoQuantidade();

            if (SetInstance(dr, objPlanoQuantidade))
                retorno = true;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            if (!retorno)
                objPlanoQuantidade = null;

            return retorno;
        }

        #endregion

        #region CarregarPorPlano
        /// <summary>   
        /// Método responsável por carregar uma instância de PlanoQuantidade vigente por uma filial
        /// </summary>
        /// <returns>Objeto Plano Quantidade</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPlano(int idPlanoAdquirido, out PlanoQuantidade objPlanoQuantidade, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = idPlanoAdquirido }
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

            while (SetInstanceNonDispose(dr, objPlanoQuantidade))
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
            if (PlanoQuantidade.CarregarPorPlano(objPlanoAdquirido.IdPlanoAdquirido, out objPlanoQuantidade, trans))
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

        #region SetInstanceNonDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objPlanoQuantidade">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstanceNonDispose(IDataReader dr, PlanoQuantidade objPlanoQuantidade)
        {
            if (dr.Read())
            {
                objPlanoQuantidade._idPlanoQuantidade = Convert.ToInt32(dr["Idf_Plano_Quantidade"]);
                objPlanoQuantidade._dataInicioQuantidade = Convert.ToDateTime(dr["Dta_Inicio_Quantidade"]);
                objPlanoQuantidade._dataFimQuantidade = Convert.ToDateTime(dr["Dta_Fim_Quantidade"]);
                objPlanoQuantidade._quantidadeSMS = Convert.ToInt32(dr["Qtd_SMS"]);
                objPlanoQuantidade._quantidadeVisualizacao = Convert.ToInt32(dr["Qtd_Visualizacao"]);
                objPlanoQuantidade._quantidadeSMSUtilizado = Convert.ToInt32(dr["Qtd_SMS_Utilizado"]);
                objPlanoQuantidade._quantidadeVisualizacaoUtilizado = Convert.ToInt32(dr["Qtd_Visualizacao_Utilizado"]);
                objPlanoQuantidade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                objPlanoQuantidade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                objPlanoQuantidade._planoAdquirido = new PlanoAdquirido(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));
                if (dr["Dta_Alteracao"] != DBNull.Value)
                    objPlanoQuantidade._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);

                objPlanoQuantidade._persisted = true;
                objPlanoQuantidade._modified = false;

                return true;
            }
            return false;
        }

        #endregion

        #endregion

    }
}