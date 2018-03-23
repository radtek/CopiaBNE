using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using BNE.BLL.AsyncServices;
using BNE.Services.Base.ProcessosAssincronos;

namespace BNE.BLL
{
    public partial class VagaIntegracao
    {

        #region Propriedades

        public List<VagaDisponibilidade> Disponibilidades { get; set; }
        public List<VagaTipoVinculo> TiposVinculo { get; set; }
        public String FuncaoImportada { get; set; }
        public String CidadeImportada { get; set; }
        public String EscolaridadeImportada { get; set; }
        public String DeficienciaImportada { get; set; }
        public Boolean VagaParaDeficiente { get; set; }

        #endregion

        #region Consultas

        #region SP_SELECT_CODIGO_VAGA_INTEGRADOR
        private const string SP_SELECT_CODIGO_VAGA_INTEGRADOR = @"
        SELECT  *
        FROM    BNE_Vaga_Integracao VI WITH(NOLOCK)
        WHERE   VI.Cod_Vaga_Integrador LIKE @CodigoVagaIntegrador
                AND VI.Idf_Integrador = @Idf_Integrador
        ";
        #endregion

        #region SP_SELECT_POR_VAGA
        private const string SP_SELECT_POR_VAGA = @"SELECT * FROM BNE.BNE_Vaga_Integracao VI WHERE VI.Idf_Vaga = @Idf_Vaga";
        #endregion

        #region SP_SELECT_COUNT_POR_VAGA
        private const string SP_SELECT_COUNT_POR_VAGA = @"SELECT COUNT(*) FROM BNE.BNE_Vaga_Integracao VI WHERE VI.Idf_Vaga = @Idf_Vaga";
        #endregion

        #endregion

        #region Métodos

        #region CarregarPorCodigoOrigem
        /// <summary>
        /// Método utilizado para retornar o registro VagaIntegracao pelo código enviado pelo integrador.
        /// </summary>
        /// <param name="codVagaIntegrador">Código da Vaga definido pela origem da vaga(integrador)</param>
        /// <param name="objIntegrador">Integrador(origem) da vaga.</param>
        /// <param name="objVagaIntegracao">Parametro de retorno </param>
        /// <returns>Retorna verdadeiro em caso de sucesso no carregamento</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static bool CarregarPorCodigoIntegrador(string codVagaIntegrador, Integrador objIntegrador, out VagaIntegracao objVagaIntegracao)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@CodigoVagaIntegrador", SqlDbType = SqlDbType.VarChar, Size = 50, Value = codVagaIntegrador } ,
                    new SqlParameter { ParameterName = "@Idf_Integrador", SqlDbType = SqlDbType.Int, Size = 4, Value = objIntegrador.IdIntegrador }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_CODIGO_VAGA_INTEGRADOR, parms))
            {
                objVagaIntegracao = new VagaIntegracao();
                if (SetInstance(dr, objVagaIntegracao))
                    return true;
            }
            objVagaIntegracao = null;
            return false;
        }
        #endregion

        #region RecuperarIntegradorPorVaga
        /// <summary>
        /// Recupera o objeto Integrador da vaga.
        /// </summary>
        /// <param name="idVaga">Código da vaga</param>
        /// <param name="objIntegrador">Objeto Integrador a ser populado</param>
        /// <returns>True se o integrador foi encontrado.</returns>
        public static bool RecuperarIntegradorPorVaga(int idVaga, out VagaIntegracao objVagaIntegracao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));

            parms[0].Value = idVaga;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_POR_VAGA, parms))
            {
                objVagaIntegracao = new VagaIntegracao();
                if (SetInstance(dr, objVagaIntegracao))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Publicar
        public void Publicar()
        {
            var parametros = new ParametroExecucaoCollection
            {
                {"idVaga", "Vaga", this.Vaga.IdVaga.ToString(CultureInfo.InvariantCulture), this.Vaga.CodigoVaga} 
            };

            ProcessoAssincrono.IniciarAtividade(AsyncServices.Enumeradores.TipoAtividade.PublicacaoVaga, parametros);
        }
        #endregion

        #region VagaPossuiIntegracao
        /// <summary>
        /// Possui integracao
        /// </summary>
        /// <param name="objVaga">Vaga</param>
        /// <returns>True se o integrador foi encontrado.</returns>
        public static bool VagaPossuiIntegracao(Vaga objVaga)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga }
                };

            return (int)DataAccessLayer.ExecuteScalar(CommandType.Text, SP_SELECT_COUNT_POR_VAGA, parms) > 0;
        }
        #endregion

        #endregion

    }
}
