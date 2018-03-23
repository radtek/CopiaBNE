//-- Data: 21/10/2014 09:28
//-- Autor: Francisco Ribas

using BNE.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
namespace BNE.BLL
{
	public partial class ContaBancaria // Tabela: TAB_Conta_Bancaria
	{
        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "BNE.TAB_Conta_Bancaria";
        private const double SlidingExpiration = 60;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region ContasBancarias
        private static List<ContaBancaria> ContasBancarias
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarContasBancariasCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarCidadesCACHE
        /// <summary>
        /// Método que retorna uma lista de itens.
        /// </summary>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        private static List<ContaBancaria> ListarContasBancariasCACHE()
        {
            var listaContaBancaria = new List<ContaBancaria>();

            const string spselecttodascidades = @"
            SELECT  *
            FROM    BNE.TAB_Conta_Bancaria CB WITH(NOLOCK)";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselecttodascidades, null))
            {
                ContaBancaria objContaBancaria = new ContaBancaria(); ;
                while (SetInstanceWithoutDispose(dr, objContaBancaria))
                {
                    listaContaBancaria.Add(objContaBancaria);
                    objContaBancaria = new ContaBancaria();
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return listaContaBancaria;
        }
        #endregion

        #endregion

        #endregion

        #region Consultas
        private const string SP_SELECT_TIPO_BANCO = @" SELECT * FROM [TAB_Conta_Bancaria] cb
                                                        WHERE   cb.Idf_Tipo_Pagamento = @idf_tipo_pagamento 
                                                        AND (@idf_banco IS NULL OR cb.Idf_Banco = @idf_banco)";
        #endregion Consultas

        #region RecuperarContaBancariaDeTipoPagamento
        public static ContaBancaria RecuperarContaBancariaDeTipoPagamento(Enumeradores.TipoPagamento TipoPagamento, Enumeradores.Banco? Banco)
        {
            if (!Banco.HasValue && (TipoPagamento == Enumeradores.TipoPagamento.DebitoRecorrente || TipoPagamento == Enumeradores.TipoPagamento.DebitoOnline))
            {
                throw new Exception("Para Débitos, o banco deve ser indicado");
            }

            if (HabilitaCache)
            {
                if (TipoPagamento == Enumeradores.TipoPagamento.DebitoRecorrente || TipoPagamento == Enumeradores.TipoPagamento.DebitoOnline)
	            {
                    return ContasBancarias.FirstOrDefault(c => c.TipoPagamento.IdTipoPagamento == (int)TipoPagamento && c.Banco.IdBanco == (int)Banco);
	            }
                return ContasBancarias.FirstOrDefault(c => c.TipoPagamento.IdTipoPagamento == (int)TipoPagamento);
            }

            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter { ParameterName = "@idf_tipo_pagamento", SqlDbType = SqlDbType.VarChar, Size = 80, Value = (int)TipoPagamento });

            if (TipoPagamento == Enumeradores.TipoPagamento.DebitoRecorrente || TipoPagamento == Enumeradores.TipoPagamento.DebitoOnline)
            {
                parms.Add(new SqlParameter { ParameterName = "@idf_banco", SqlDbType = SqlDbType.VarChar, Size = 80, Value = (int)Banco });
            }

            IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_TIPO_BANCO, parms);

            ContaBancaria objContaBancaria = new ContaBancaria();
            if (!SetInstance(dr, objContaBancaria))
                objContaBancaria = null;

            if (dr != null && !dr.IsClosed)
                dr.Close();

            if (dr != null) dr.Dispose();

            return objContaBancaria;
        }
        #endregion RecuperarContaBancariaDeTipoPagamento

        #region SetInstanceWithoutDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objContaBancaria">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static bool SetInstanceWithoutDispose(IDataReader dr, ContaBancaria objContaBancaria)
        {
            try
            {
                if (dr.Read())
                {
                    objContaBancaria._idContaBancaria = Convert.ToInt32(dr["Idf_Conta_Bancaria"]);
                    objContaBancaria._tipoPagamento = new TipoPagamento(Convert.ToInt32(dr["Idf_Tipo_Pagamento"]));
                    objContaBancaria._banco = new Banco(Convert.ToInt32(dr["Idf_Banco"]));
                    objContaBancaria._descricaoAgencia = Convert.ToString(dr["Des_Agencia"]);
                    objContaBancaria._descricaoConta = Convert.ToString(dr["Des_Conta"]);
                    if (dr["Des_Operacao"] != DBNull.Value)
                        objContaBancaria._descricaoOperacao = Convert.ToString(dr["Des_Operacao"]);

                    objContaBancaria._persisted = true;
                    objContaBancaria._modified = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
	}
}