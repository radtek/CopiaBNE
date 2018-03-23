//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System;
using BNE.EL;

namespace BNE.BLL
{
    public partial class Plano // Tabela: BNE_Plano
    {

        #region Propriedades

        #region DescricaoPlano
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoPlano
        {
            get
            {
                if (string.IsNullOrEmpty(this._descricaoPlano))
                    this._descricaoPlano = this.RecuperarDescricao();

                return this._descricaoPlano;
            }
            set
            {
                this._descricaoPlano = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas

        private const string Selectplanos = @" SELECT idf_plano, des_plano, Qtd_Dias_Validade, Vlr_Base FROM BNE_Plano WITH(NOLOCK) WHERE Flg_Inativo = @Inativo ";

        private const string Selectlistarplanos = @"
               
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount NVARCHAR(MAX)
        DECLARE @iSelectPag NVARCHAR(MAX)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
            SELECT  ROW_NUMBER() OVER (ORDER BY Des_Plano ASC) AS RowID,
                idf_plano, des_plano, Qtd_Dias_Validade, Vlr_Base , Flg_Inativo
            FROM BNE_Plano P WITH(NOLOCK)
            '
        IF(@FlgInativo IS NOT NULL)
            SET @iSelect = @iSelect + ' WHERE Flg_Inativo = ' + CONVERT(VARCHAR(1),@FlgInativo)
                                                                    		
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect
            + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect
            + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec)
            + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
        
        EXEC sp_executesql @iSelectCount
        EXEC sp_executesql @iSelectPag";

        private const string Selectlistarplanosportipo = @"
        SELECT 
                DES_PLANO,
                IDF_PLANO 
        FROM    BNE_Plano WITH(NOLOCK)
        WHERE   Idf_Plano_Tipo=@Idf_Plano_Tipo
                AND Flg_Inativo=0
        ORDER BY DES_PLANO";

        #region Spselectdescricaoplano
        private const string Spselectdescricaoplano = @"
        SELECT Des_Plano FROM BNE_Plano WHERE Idf_Plano = @Idf_Plano
        ";
        #endregion

        #region Spselectvalorplano
        private const string Spselectvalorplano = @"
        SELECT Vlr_Base FROM BNE_Plano WHERE Idf_Plano = @Idf_Plano
        ";
        #endregion

        #region SELECT_PLANO_DE_PAGAMENTO
        private const string SELECT_PLANO_DE_PAGAMENTO = @"
        --Parcela de plano normal
        SELECT pl.* 
        FROM BNE.BNE_Plano pl with(nolock)
	        INNER JOIN bne.BNE_Plano_Adquirido pa with(nolock) ON pl.Idf_Plano = pa.Idf_Plano
	        INNER JOIN BNE.BNE_Plano_Parcela pp with(nolock) ON pa.Idf_Plano_Adquirido = pp.Idf_Plano_Adquirido
	        INNER JOIN BNE.BNE_Pagamento pag with(nolock) ON pp.Idf_Plano_Parcela = pag.Idf_Plano_Parcela
        WHERE pag.Idf_Pagamento = @Idf_Pagamento
        UNION
        --Parcela de plano Adicional
        SELECT pl.* FROM BNE.BNE_Plano pl with(nolock)
	        INNER JOIN BNE.BNE_Plano_Adquirido pa with(nolock) ON pl.Idf_Plano = pa.Idf_Plano
	        INNER JOIN BNE.BNE_Adicional_Plano ap with(nolock) ON ap.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
	        INNER JOIN BNE.BNE_Pagamento pag with(nolock) ON ap.Idf_Adicional_Plano = pag.Idf_Adicional_Plano
        WHERE pag.Idf_Pagamento = @Idf_Pagamento;";
        #endregion

        #region Selectplanosvendaciaporvendedor
        private const string Selectplanosvendaciaporvendedor = @" 
        SELECT idf_plano, des_plano FROM BNE_Plano WITH(NOLOCK) WHERE Flg_Habilita_Venda_Personalizada = 1";
        #endregion

        #endregion

        #region Métodos

        #region Listar
        /// <summary>
        /// Método retorna Consulta para carregar DropDownList
        /// </summary>
        /// <param name="planoInativo">Consultar planos inativos ou ativos</param>
        /// <returns>Resultado da pesquisa</returns>
        public static IDataReader Listar(bool planoInativo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Inativo", SqlDbType = SqlDbType.Bit, Size = 1, Value = planoInativo}
                };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Selectplanos, parms);
        }
        public static DataTable Listar(int paginaCorrente, int tamanhoPagina, int? FlgInativo, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente},
                    new SqlParameter{ ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina},
                    new SqlParameter{ ParameterName = "@FlgInativo", SqlDbType = SqlDbType.Bit, Value = FlgInativo}
                };

            if (FlgInativo == null)
                parms[2].Value = DBNull.Value;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Selectlistarplanos, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
                    dt.Load(dr);
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }

        //public static DataTable ListarPorStatus(int paginaCorrente, int tamanhoPagina, bool FlgInativo, out int totalRegistros)
        //{
        //    var parms = new List<SqlParameter>
        //        {
        //            new SqlParameter{ ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente},
        //            new SqlParameter{ ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina},
        //            new SqlParameter{ ParameterName = "@FlgInativo", SqlDbType = SqlDbType.Bit,Value = 0}
        //        };

        //    totalRegistros = 0;
        //    DataTable dt = null;
        //    try
        //    {
        //        using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SelectListarPlanosPorStatus, parms))
        //        {
        //            if (dr.Read())
        //                totalRegistros = Convert.ToInt32(dr[0]);

        //            dr.NextResult();
        //            dt = new DataTable();
        //            dt.Load(dr);
        //        }
        //    }
        //    finally
        //    {
        //        if (dt != null)
        //            dt.Dispose();
        //    }

        //    return dt;
        //}
        #endregion

        #region ListarPlanosVendaCIAPorVendedor
        /// <summary>
        /// Método retorna todos os planos que um vendedor pode vender direto para um cliente CIA
        /// </summary>
        /// <returns>Resultado da pesquisa</returns>
        public static IDataReader ListarPlanosVendaCIAPorVendedor()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, Selectplanosvendaciaporvendedor, null);
        }
        #endregion

        #region ListarPorTipo
        public static IDataReader ListarPorTipo(Enumeradores.PlanoTipo planoTipo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter
                        {
                            ParameterName = "@Idf_Plano_Tipo", 
                            SqlDbType = SqlDbType.Int, 
                            Size = 4, 
                            Value = (int)planoTipo
                        }
                };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Selectlistarplanosportipo, parms);
        }
        #endregion

        #region RecuperarCodigoPlanoMensalPorFuncaoCategoria
        /// <summary>
        /// Recupera o plano correto de acordo com a categoria da função.
        /// </summary>
        /// <param name="objFuncaoCategoria">Categoria da função</param>
        /// <returns></returns>
        public static int RecuperarCodigoPlanoMensalPorFuncaoCategoria(FuncaoCategoria objFuncaoCategoria)
        {
            return RecuperarCodigoPlanoPorFuncaoCategoria(objFuncaoCategoria, true);
        }
        /// <summary>
        /// Recupera o plano correto de acordo com a categoria da função.
        /// </summary>
        /// <param name="objFuncaoCategoria">Categoria da função</param>
        /// <returns></returns>
        public static int RecuperarCodigoPlanoTrimestralPorFuncaoCategoria(FuncaoCategoria objFuncaoCategoria)
        {
            return RecuperarCodigoPlanoPorFuncaoCategoria(objFuncaoCategoria, false);
        }
        /// <summary>
        /// Recupera o plano correto de acordo com a categoria da função.
        /// </summary>
        /// <param name="objFuncaoCategoria">Categoria da função</param>
        /// <param name="planoNormal">Bool para identificar se é um plano normal (30 dias) ou extendido (90 dias)</param>
        /// <returns></returns>
        private static int RecuperarCodigoPlanoPorFuncaoCategoria(FuncaoCategoria objFuncaoCategoria, bool planoNormal)
        {
            int idPlano = -1;

            switch ((Enumeradores.FuncaoCategoria)Enum.Parse(typeof(Enumeradores.FuncaoCategoria), objFuncaoCategoria.IdFuncaoCategoria.ToString(CultureInfo.CurrentCulture)))
            {
                case Enumeradores.FuncaoCategoria.Gestao:
                    idPlano = Convert.ToInt32(planoNormal ? Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPCategoria130Dias) : Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPCategoria190Dias));
                    break;
                case Enumeradores.FuncaoCategoria.Especialista:
                    idPlano = Convert.ToInt32(planoNormal ? Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPCategoria230Dias) : Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPCategoria290Dias));
                    break;
                case Enumeradores.FuncaoCategoria.Apoio:
                    idPlano = Convert.ToInt32(planoNormal ? Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPCategoria330Dias) : Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPCategoria390Dias));
                    break;
                case Enumeradores.FuncaoCategoria.Operacao:
                    idPlano = Convert.ToInt32(planoNormal ? Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPCategoria430Dias) : Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPCategoria490Dias));
                    break;
            }

            return idPlano;
        }
        #endregion

        #region RecuperarDescricao
        private string RecuperarDescricao()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPlano }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectdescricaoplano, parms));
        }
        #endregion

        #region RecuperarValor
        public decimal RecuperarValor()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPlano }
                };

            return Convert.ToDecimal(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectvalorplano, parms));
        }
        #endregion

        /// <summary>
        /// Carrega o plano do pagamento informado.
        /// </summary>
        /// <param name="idPagamento">Id do registro de pagamento</param>
        /// <returns>Instância de Plano do pagamento informado</returns>
        /// <exception cref="RecordNotFoundException">RecordNotFoundException se registro de plano não encontrado</exception>
        public static Plano CarregarPlanoDePagamento(int idPagamento)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Pagamento", SqlDbType = SqlDbType.Int, Size = 4
                        , Value = idPagamento}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SELECT_PLANO_DE_PAGAMENTO, parms))
            {
                Plano objPlano = new Plano();
                if (SetInstance(dr, objPlano))
                    return objPlano;
            }
            throw (new RecordNotFoundException(typeof(Plano)));
        }

        #region SetInstanceNotDipose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objPlano">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        internal static bool SetInstanceNotDipose(IDataReader dr, Plano objPlano)
        {
            try
            {
                objPlano._idPlano = Convert.ToInt32(dr["Idf_Plano"]);
                objPlano._descricaoPlano = Convert.ToString(dr["Des_Plano"]);
                objPlano._quantidadeDiasValidade = Convert.ToInt32(dr["Qtd_Dias_Validade"]);
                objPlano._quantidadeSMS = Convert.ToInt32(dr["Qtd_SMS"]);
                objPlano._quantidadeVisualizacao = Convert.ToInt32(dr["Qtd_Visualizacao"]);
                objPlano._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                objPlano._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                objPlano._valorBase = Convert.ToDecimal(dr["Vlr_Base"]);
                if (dr["Dta_Inicio"] != DBNull.Value)
                    objPlano._dataInicio = Convert.ToDateTime(dr["Dta_Inicio"]);
                if (dr["Dta_Final"] != DBNull.Value)
                    objPlano._dataFinal = Convert.ToDateTime(dr["Dta_Final"]);
                objPlano._planoTipo = new PlanoTipo(Convert.ToInt32(dr["Idf_Plano_Tipo"]));
                objPlano._planoFormaPagamento = new PlanoFormaPagamento(Convert.ToInt32(dr["Idf_Plano_Forma_Pagamento"]));
                objPlano._quantidadeParcela = Convert.ToInt32(dr["Qtd_Parcela"]);
                objPlano._valorDescontoMaximo = Convert.ToInt32(dr["Vlr_Desconto_Maximo"]);
                if (dr["Qtd_SMS_Maxima"] != DBNull.Value)
                    objPlano._quantidadeSMSMaxima = Convert.ToInt32(dr["Qtd_SMS_Maxima"]);
                if (dr["Vlr_Base_Minimo"] != DBNull.Value)
                    objPlano._valorBaseMinimo = Convert.ToDecimal(dr["Vlr_Base_Minimo"]);
                if (dr["Qtd_Prazo_Boleto_Maxima"] != DBNull.Value)
                    objPlano._quantidadePrazoBoletoMaxima = Convert.ToInt32(dr["Qtd_Prazo_Boleto_Maxima"]);
                objPlano._flagBoletoRegistrado = Convert.ToBoolean(dr["Flg_Boleto_Registrado"]);
                if (dr["idf_Tipo_Contrato"] != DBNull.Value)
                    objPlano._tipoContrato = new TipoContrato(Convert.ToInt32(dr["idf_Tipo_Contrato"]));
                objPlano._flagEnviarContrato = Convert.ToBoolean(dr["Flg_Enviar_Contrato"]);
                objPlano._flagHabilitaVendaPersonalizada = Convert.ToBoolean(dr["Flg_Habilita_Venda_Personalizada"]);

                objPlano._persisted = true;
                objPlano._modified = false;

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #endregion

    }
}