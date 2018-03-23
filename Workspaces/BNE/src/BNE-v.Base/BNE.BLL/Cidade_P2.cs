//-- Data: 02/03/2010 09:54
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using BNE.BLL.Custom;
using BNE.Cache;
using BNE.EL;

namespace BNE.BLL
{
    public partial class Cidade // Tabela: TAB_Cidade
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "plataforma.TAB_Cidade";
        private const double SlidingExpiration = 60;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region Cidades
        private static List<CidadeCache> Cidades
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarCidadesCACHE, SlidingExpiration);
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
        private static List<CidadeCache> ListarCidadesCACHE()
        {
            var listaCidades = new List<CidadeCache>();

            const string spselecttodascidades = @"
            SELECT  Cid.Idf_Cidade,
                    Cid.Nme_Cidade,
                    Cid.Sig_Estado 
            FROM    plataforma.TAB_Cidade Cid WITH(NOLOCK) 
            WHERE   Cid.Flg_Inativo = 0
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselecttodascidades, null))
            {
                while (dr.Read())
                {
                    listaCidades.Add(new CidadeCache
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Cidade"]),
                        NomeCidade = dr["Nme_Cidade"].ToString(),
                        SiglaEstado = dr["Sig_Estado"].ToString()
                    });
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return listaCidades;
        }
        #endregion

        #endregion

        #region CacheObjects

        #region CidadeCache
        private class CidadeCache
        {
            public int Identificador { get; set; }
            public string NomeCidade { get; set; }
            public string SiglaEstado { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consultas
        private const string Spselectnomeestado = @"
        SELECT  Cid.* 
        FROM    plataforma.TAB_Cidade Cid WITH(NOLOCK) 
        WHERE   Cid.Nme_Cidade like @Nme_Cidade
                AND Cid.Sig_Estado = @Sig_Estado and Flg_Inativo = 0
                ORDER BY Cid.Nme_Cidade
        ";

        private const string Spselectnomecidadenomeestado = @"
        SELECT  Cid.* 
        FROM    plataforma.TAB_Cidade Cid WITH(NOLOCK) 
        INNER JOIN plataforma.TAB_Estado Est WITH(NOLOCK) ON Cid.Sig_Estado = Est.Sig_Estado
        WHERE   Cid.Nme_Cidade like @Nme_Cidade
                AND (Est.Sig_Estado LIKE @Nme_Estado OR Est.Nme_Estado like @Nme_Estado) and Cid.Flg_Inativo = 0
                ORDER BY Cid.Nme_Cidade
        ";

        private const string Spselectnome = @"
        SELECT  Cid.* 
        FROM    plataforma.TAB_Cidade Cid WITH(NOLOCK) 
        WHERE   Cid.Nme_Cidade like @Nme_Cidade
                AND Flg_Inativo = 0
        ORDER BY Cid.Nme_Cidade
        ";

        private const string Spselectnomecidadeestadoautocomplete = @"  
        SELECT  top(@Numero_Registros) Idf_Cidade, Nme_Cidade, Sig_Estado
        FROM    plataforma.TAB_Cidade C WITH(NOLOCK)
        WHERE   C.Nme_Cidade LIKE + @Nme_Cidade + '%'
                AND (@Sig_Estado is null or C.Sig_Estado = @Sig_Estado )
                AND C.Flg_Inativo = 0
        ORDER BY C.Nme_Cidade, C.Sig_Estado DESC ";

        #region SpRecuperarTaxaISS

        private const string SpRecuperarTaxaISS = @"
        select ci.Txa_ISS
        from plataforma.tab_cidade ci with(nolock)
        where ci.Idf_Cidade = @Idf_Cidade
        ";

        #endregion

        #region Spselectcidadeporestado
        private const string Spselectcidadeporestado = @"
        SELECT  C.* 
        FROM    plataforma.TAB_Cidade C WITH(NOLOCK)
        WHERE   C.Sig_Estado IN ( @Siglas_Estado )
                AND C.Flg_Inativo = 0
        ORDER BY C.Sig_Estado, C.Nme_Cidade";
        #endregion

        #endregion

        #region CarregarPorNome
        /// <summary>
        /// Método utilizado para retornar uma instância de Cidade a partir do banco de dados. Contempla o padrão "Cidade/SIGLAESTADO Ex.: Curitiba/PR"
        /// </summary>
        /// <param name="nomeCidadeSiglaEstado">Nome da Cidade</param>
        /// <param name="objCidade"> </param>
        /// <returns>objCidade</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorNome(string nomeCidadeSiglaEstado, out Cidade objCidade)
        {
            if (!string.IsNullOrEmpty(nomeCidadeSiglaEstado))
            {
                var retorno = false;
                objCidade = new Cidade();

                string nomeCidade = string.Empty;
                string siglaEstado = string.Empty;

                nomeCidadeSiglaEstado = nomeCidadeSiglaEstado.Trim();

                #region Recuperando estado, se tiver
                const string pattern = @"([\w\s'-]+)[/](\w+)"; //Ex. Curitiba/Paraná
                var regex = new Regex(pattern);
                Match match = regex.Match(nomeCidadeSiglaEstado);
                IDataReader dr;
                if (match.Success)
                {
                    nomeCidade = match.Groups[1].Value;
                    siglaEstado = match.Groups[2].Value;
                }
                #endregion

                #region Cache
                if (HabilitaCache)
                {
                    if (string.IsNullOrWhiteSpace(nomeCidade) && string.IsNullOrWhiteSpace(siglaEstado))
                    {
                        var cidade = Cidades.FirstOrDefault(c => c.NomeCidade.NormalizarStringLINQ().Equals(nomeCidadeSiglaEstado.NormalizarStringLINQ()));
                        if (cidade != null)
                        {
                            objCidade = new Cidade { IdCidade = cidade.Identificador, NomeCidade = cidade.NomeCidade, Estado = new Estado(cidade.SiglaEstado) };
                            retorno = true;
                        }
                        else
                            objCidade = null;
                    }
                    else
                    {
                        var cidade = Cidades.FirstOrDefault(c => c.NomeCidade.NormalizarStringLINQ().Equals(nomeCidade.NormalizarStringLINQ()) && c.SiglaEstado.NormalizarStringLINQ().Equals(siglaEstado.NormalizarStringLINQ()));
                        if (cidade != null)
                        {
                            objCidade = new Cidade { IdCidade = cidade.Identificador, NomeCidade = cidade.NomeCidade, Estado = new Estado(cidade.SiglaEstado) };
                            retorno = true;
                        }
                        else
                            objCidade = null;
                    }

                    return retorno;
                }
                #endregion

                var parms = new List<SqlParameter>();

                if (string.IsNullOrWhiteSpace(nomeCidade) && string.IsNullOrWhiteSpace(siglaEstado))
                {
                    parms.Add(new SqlParameter { ParameterName = "@Nme_Cidade", SqlDbType = SqlDbType.VarChar, Size = 80, Value = nomeCidadeSiglaEstado });
                    dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectnome, parms);
                }
                else
                {
                    parms.Add(new SqlParameter { ParameterName = "@Nme_Cidade", SqlDbType = SqlDbType.VarChar, Size = 80, Value = nomeCidade });
                    parms.Add(new SqlParameter { ParameterName = "@Sig_Estado", SqlDbType = SqlDbType.Char, Size = 2, Value = siglaEstado });
                    dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectnomeestado, parms);
                }

                if (SetInstance(dr, objCidade))
                    retorno = true;
                else
                    objCidade = null;

                if (dr != null && !dr.IsClosed)
                    dr.Close();

                if (dr != null) dr.Dispose();

                return retorno;
            }

            objCidade = null;
            return false;
        }
        #endregion

        #region CarregarPorNomeCidadeEstado
        /// <summary>
        /// Método utilizado para retornar uma instância de Cidade a partir do banco de dados.
        /// </summary>
        /// <param name="nomeCidade">Nome da Cidade</param>
        /// <param name="objCidade"> </param>
        /// <returns>objCidade</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorNomeCidadeEstado(string nomeCidade, string nomeEstado, out Cidade objCidade)
        {
            var parms = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(nomeCidade))
            {
                nomeCidade = Regex.Replace(nomeCidade.Trim(), " +", " ");
                nomeEstado = Regex.Replace(nomeEstado.Trim(), " +", " ");

                IDataReader dr;
                parms.Add(new SqlParameter { ParameterName = "@Nme_Cidade", SqlDbType = SqlDbType.VarChar, Size = 80, Value = nomeCidade });
                parms.Add(new SqlParameter { ParameterName = "@Nme_Estado", SqlDbType = SqlDbType.VarChar, Size = 50, Value = nomeEstado });
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectnomecidadenomeestado, parms);

                var retorno = false;

                objCidade = new Cidade();

                if (SetInstance(dr, objCidade))
                    retorno = true;
                else
                    objCidade = null;

                if (dr != null && !dr.IsClosed)
                    dr.Close();

                if (dr != null) dr.Dispose();

                return retorno;
            }

            objCidade = null;
            return false;
        }
        #endregion

        #region RecuperarNomesCidades
        /// <summary>
        /// Método que retorna uma lista de cidades a partir de um estado e parte do nome.
        /// </summary>
        /// <param name="uf">UF ao qual pertence a cidade.</param>
        /// <param name="nomeParcialCidade">Parte do nome da cidade.</param>
        /// <param name="numeroRegistros">Quantidade de registros a serem retornados.</param>
        /// <returns>Lista de nomes das cidades.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Dictionary<int, string> RecuperarNomesCidades(string nomeParcialCidade, string uf, int numeroRegistros)
        {
            #region Cache
            if (HabilitaCache)
            {
                if (string.IsNullOrWhiteSpace(uf))
                    return Cidades.OrderBy(c => c.NomeCidade).Where(c => c.NomeCidade.NormalizarStringLINQ().StartsWith(nomeParcialCidade.NormalizarStringLINQ())).Take(numeroRegistros).ToDictionary(c => c.Identificador, c => c.NomeCidade);

                return Cidades.OrderBy(c => c.NomeCidade).Where(c => c.NomeCidade.NormalizarStringLINQ().StartsWith(nomeParcialCidade.NormalizarStringLINQ()) && c.SiglaEstado.NormalizarStringLINQ().Equals(uf.NormalizarStringLINQ())).Take(numeroRegistros).ToDictionary(c => c.Identificador, c => c.NomeCidade);
            }
            #endregion

            var cidades = new Dictionary<int, string>();

            using (IDataReader dr = ListarPorNomeParcial(nomeParcialCidade, uf, numeroRegistros))
            {

                while (dr.Read())
                    cidades.Add(Convert.ToInt32(dr["Idf_Cidade"]), dr["Nme_Cidade"].ToString());

                if (!dr.IsClosed)
                    dr.Close();
            }

            return cidades;
        }

        #endregion

        #region RecuperarNomesCidadesEstado
        /// <summary>
        /// Método que retorna uma lista de cidades a partir de um estado e parte do nome.
        /// </summary>
        /// <param name="uf">UF ao qual pertence a cidade.</param>
        /// <param name="nomeParcialCidade">Parte do nome da cidade.</param>
        /// <param name="numeroRegistros">Quantidade de registros a serem retornados.</param>
        /// <returns>Lista de nomes das cidades.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Dictionary<int, string> RecuperarNomesCidadesEstado(string nomeParcialCidade, string uf, int numeroRegistros)
        {
            #region Cache
            if (HabilitaCache)
            {
                if (string.IsNullOrWhiteSpace(uf))
                    return Cidades.OrderBy(c => c.NomeCidade).Where(c => c.NomeCidade.NormalizarStringLINQ().StartsWith(nomeParcialCidade.NormalizarStringLINQ())).Take(numeroRegistros)
                       .OrderByDescending(c => c.SiglaEstado).OrderBy(c => c.NomeCidade)
                       .ToDictionary(c => c.Identificador, c => c.NomeCidade + "/" + c.SiglaEstado);

                return Cidades.OrderBy(c => c.NomeCidade).Where(c => c.NomeCidade.NormalizarStringLINQ().StartsWith(nomeParcialCidade.NormalizarStringLINQ()) && c.SiglaEstado.NormalizarStringLINQ().Equals(uf.NormalizarStringLINQ())).Take(numeroRegistros)
                    .OrderByDescending(c => c.SiglaEstado).OrderBy(c => c.NomeCidade)
                    .ToDictionary(c => c.Identificador, c => c.NomeCidade + "/" + c.SiglaEstado);
            }
            #endregion

            var cidades = new Dictionary<int, string>();

            using (IDataReader dr = ListarPorNomeParcial(nomeParcialCidade, uf, numeroRegistros))
            {
                while (dr.Read())
                    cidades.Add(Convert.ToInt32(dr["Idf_Cidade"]), dr["Nme_Cidade"] + "/" + dr["Sig_Estado"]);

                if (!dr.IsClosed)
                    dr.Close();
            }

            return cidades;
        }
        #endregion

        #region ListarPorNomeParcial
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static IDataReader ListarPorNomeParcial(string nome, string siglaEstado, int numeroRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Nme_Cidade", SqlDbType.VarChar, 80), 
                    new SqlParameter("@Sig_Estado", SqlDbType.Char, 2), 
                    new SqlParameter("@Numero_Registros", SqlDbType.Int, 4)
                };

            parms[0].Value = nome;

            if (String.IsNullOrEmpty(siglaEstado))
                parms[1].Value = DBNull.Value;
            else
                parms[1].Value = siglaEstado;

            parms[2].Value = numeroRegistros;

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectnomecidadeestadoautocomplete, parms);
        }
        #endregion

        #region ListarPorEstados
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static IDataReader ListarPorEstados(List<string> siglasEstado)
        {
            string siglas = string.Empty;

            if (siglasEstado.Count > 0)
                siglas = siglasEstado.Aggregate(((anterior, proximo) => anterior + "','" + proximo));

            string ids = String.Format("'{0}'", siglas);

            string query = Spselectcidadeporestado.Replace("@Siglas_Estado", ids);

            return DataAccessLayer.ExecuteReader(CommandType.Text, query, null);
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objCidade">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, Cidade objCidade)
        {
            try
            {
                if (dr.Read())
                {
                    objCidade._idCidade = Convert.ToInt32(dr["Idf_Cidade"]);
                    objCidade._nomeCidade = Convert.ToString(dr["Nme_Cidade"]);
                    objCidade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objCidade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objCidade._estado = new Estado(dr["Sig_Estado"].ToString());
                    objCidade._taxaISS = Convert.ToDecimal(dr["Txa_ISS"]);
                    if (dr["Idf_Tipo_Base"] != DBNull.Value)
                        objCidade._idTipoBase = Convert.ToInt32(dr["Idf_Tipo_Base"]);
                    if (dr["Cod_Rais"] != DBNull.Value)
                        objCidade._codigoRais = Convert.ToDecimal(dr["Cod_Rais"]);

                    objCidade._persisted = true;
                    objCidade._modified = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objCidade">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool SetInstance_NotDispose(IDataReader dr, Cidade objCidade)
        {
            objCidade._idCidade = Convert.ToInt32(dr["Idf_Cidade"]);
            objCidade._nomeCidade = Convert.ToString(dr["Nme_Cidade"]);
            objCidade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
            objCidade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
            objCidade._estado = new Estado(dr["Sig_Estado"].ToString());
            objCidade._taxaISS = Convert.ToDecimal(dr["Txa_ISS"]);
            if (dr["Idf_Tipo_Base"] != DBNull.Value)
                objCidade._idTipoBase = Convert.ToInt32(dr["Idf_Tipo_Base"]);
            if (dr["Cod_Rais"] != DBNull.Value)
                objCidade._codigoRais = Convert.ToDecimal(dr["Cod_Rais"]);

            objCidade._persisted = true;
            objCidade._modified = false;

            return true;
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Cidade a partir do banco de dados.
        /// </summary>
        /// <param name="idCidade">Chave do registro.</param>
        /// <returns>Instância de Cidade.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Cidade LoadObject(int idCidade)
        {
            #region Cache
            if (HabilitaCache)
            {
                var cidade = Cidades.FirstOrDefault(c => c.Identificador == idCidade);

                if (cidade != null)
                    return new Cidade { IdCidade = cidade.Identificador, NomeCidade = cidade.NomeCidade, Estado = new Estado(cidade.SiglaEstado) };

                throw (new RecordNotFoundException(typeof(Cidade)));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idCidade))
            {
                Cidade objCidade = new Cidade();
                if (SetInstance(dr, objCidade))
                    return objCidade;
            }
            throw (new RecordNotFoundException(typeof(Cidade)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Cidade a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCidade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Cidade.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Cidade LoadObject(int idCidade, SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var cidade = Cidades.FirstOrDefault(c => c.Identificador == idCidade);

                if (cidade != null)
                    return new Cidade { IdCidade = cidade.Identificador, NomeCidade = cidade.NomeCidade, Estado = new Estado(cidade.SiglaEstado) };

                throw (new RecordNotFoundException(typeof(Cidade)));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idCidade, trans))
            {
                Cidade objCidade = new Cidade();
                if (SetInstance(dr, objCidade))
                    return objCidade;
            }
            throw (new RecordNotFoundException(typeof(Cidade)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Cidade a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (HabilitaCache)
            {
                var cidade = Cidades.FirstOrDefault(c => c.Identificador == this._idCidade);
                if (cidade != null)
                {
                    this.NomeCidade = cidade.NomeCidade;
                    this.Estado = new Estado(cidade.SiglaEstado);
                    return true;
                }
                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idCidade))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Cidade a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var cidade = Cidades.FirstOrDefault(c => c.Identificador == this._idCidade);
                if (cidade != null)
                {
                    this.NomeCidade = cidade.NomeCidade;
                    this.Estado = new Estado(cidade.SiglaEstado);
                    return true;
                }
                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idCidade, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #region RecuperarTaxaISS
        public static void RecuperarTaxaISS(int idCidade, out decimal vlrTaxaISS) 
        {
            vlrTaxaISS = 0;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Value = idCidade}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarTaxaISS, parms)) 
            {
                if (dr.Read())
                    vlrTaxaISS = Convert.ToDecimal(dr["Txa_ISS"]);
            };
        }
        #endregion

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(NomeCidade))
            {
                if (Estado != null
                    && !string.IsNullOrWhiteSpace(Estado.SiglaEstado))
                {
                    return NomeCidade + "/" + Estado.SiglaEstado;
                }

                return NomeCidade;
            }

            if (IdCidade <= 0)
            {
                return base.ToString();
            }

            return base.ToString() + string.Format(" (ID='{0}')", this.IdCidade);
        }
    }
}