//-- Data: 20/02/2013 16:27
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using BNE.Cache;

namespace BNE.BLL
{
    public partial class ParametroBuscaCV // Tabela: BNE_Parametro_Busca_CV
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "bne.BNE_Parametro_Busca_CV";
        private const double SlidingExpiration = 1440;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region ParametroBuscaCV
        private static List<ParametroBuscaCV> Parametros
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarParametrosCache, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos
        #region ListarParametros
        public static List<ParametroBuscaCV> ListarParametrosCache()
        {
            var lista = new List<ParametroBuscaCV>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperartodasbuscas, null))
            {
                while (dr.Read())
                {
                    var objParametroBuscaCV = new ParametroBuscaCV();
                    if (SetInstance_NonDispose(dr, objParametroBuscaCV))
                        lista.Add(objParametroBuscaCV);
                }
            }
            return lista;
        }
        #endregion
        #endregion

        #endregion

        #region Consultas
        private const string Sprecuperartodasbuscas = "SELECT * FROM BNE_Parametro_Busca_CV WITH(NOLOCK) WHERE Flg_Inativo = 0";
        #endregion

        #region Métodos

        #region ListarParametros
        public static List<ParametroBuscaCV> ListarParametros()
        {
            #region Cache
            if (HabilitaCache)
                return Parametros;
            #endregion

            var lista = new List<ParametroBuscaCV>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperartodasbuscas, null))
            {
                while (dr.Read())
                {
                    var objParametroBuscaCV = new ParametroBuscaCV();
                    if (SetInstance_NonDispose(dr, objParametroBuscaCV))
                        lista.Add(objParametroBuscaCV);
                }
            }
            return lista;
        }
        #endregion

        #region SetInstance_NonDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objParametroBuscaCV">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NonDispose(IDataReader dr, ParametroBuscaCV objParametroBuscaCV)
        {
            objParametroBuscaCV._idParametroBuscaCV = Convert.ToInt32(dr["Idf_Parametro_Busca_CV"]);
            objParametroBuscaCV._flagIdfCurriculo = Convert.ToBoolean(dr["Flg_Idf_Curriculo"]);
            objParametroBuscaCV._flagNumCPF = Convert.ToBoolean(dr["Flg_Num_CPF"]);
            objParametroBuscaCV._flagEmlPessoa = Convert.ToBoolean(dr["Flg_Eml_Pessoa"]);
            objParametroBuscaCV._flagIdfFuncao = Convert.ToBoolean(dr["Flg_Idf_Funcao"]);
            objParametroBuscaCV._flagIdfCidade = Convert.ToBoolean(dr["Flg_Idf_Cidade"]);
            objParametroBuscaCV._flagSigEstado = Convert.ToBoolean(dr["Flg_Sig_Estado"]);
            objParametroBuscaCV._flagPesoEscolaridade = Convert.ToBoolean(dr["Flg_Peso_Escolaridade"]);
            objParametroBuscaCV._flagIdfSexo = Convert.ToBoolean(dr["Flg_Idf_Sexo"]);
            objParametroBuscaCV._flagIdadeMin = Convert.ToBoolean(dr["Flg_Idade_Min"]);
            objParametroBuscaCV._flagIdadeMax = Convert.ToBoolean(dr["Flg_Idade_Max"]);
            objParametroBuscaCV._flagSalarioMin = Convert.ToBoolean(dr["Flg_Salario_Min"]);
            objParametroBuscaCV._flagSalarioMax = Convert.ToBoolean(dr["Flg_Salario_Max"]);
            objParametroBuscaCV._flagMesesExp = Convert.ToBoolean(dr["Flg_Meses_Exp"]);
            objParametroBuscaCV._flagNmePessoa = Convert.ToBoolean(dr["Flg_Nme_Pessoa"]);
            objParametroBuscaCV._flagDesBairro = Convert.ToBoolean(dr["Flg_Des_Bairro"]);
            objParametroBuscaCV._flagDesLogradouro = Convert.ToBoolean(dr["Flg_Des_Logradouro"]);
            objParametroBuscaCV._flagNumCEPMin = Convert.ToBoolean(dr["Flg_Num_CEP_Min"]);
            objParametroBuscaCV._flagNumCEPMax = Convert.ToBoolean(dr["Flg_Num_CEP_Max"]);
            objParametroBuscaCV._flagExperienciaEm = Convert.ToBoolean(dr["Flg_Experiencia_Em"]);
            objParametroBuscaCV._flagDesCurso = Convert.ToBoolean(dr["Flg_Des_Curso"]);
            objParametroBuscaCV._flagNmeFonte = Convert.ToBoolean(dr["Flg_Nme_Fonte"]);
            objParametroBuscaCV._flagDesCursoOutros = Convert.ToBoolean(dr["Flg_Des_Curso_Outros"]);
            objParametroBuscaCV._flagNmeFonteOutros = Convert.ToBoolean(dr["Flg_Nme_Fonte_Outros"]);
            objParametroBuscaCV._flagNmeEmpresa = Convert.ToBoolean(dr["Flg_Nme_Empresa"]);
            objParametroBuscaCV._flagIdfAreaBNE = Convert.ToBoolean(dr["Flg_Idf_Area_BNE"]);
            objParametroBuscaCV._flagIdfCategoriaHabilitacao = Convert.ToBoolean(dr["Flg_Idf_Categoria_Habilitacao"]);
            objParametroBuscaCV._flagIdfTipoVeiculo = Convert.ToBoolean(dr["Flg_Idf_Tipo_Veiculo"]);
            objParametroBuscaCV._flagNumDDD = Convert.ToBoolean(dr["Flg_Num_DDD"]);
            objParametroBuscaCV._flagNumTelefone = Convert.ToBoolean(dr["Flg_Num_Telefone"]);
            objParametroBuscaCV._flagIdfDeficiencia = Convert.ToBoolean(dr["Flg_Idf_Deficiencia"]);
            objParametroBuscaCV._flagDesMetaBusca = Convert.ToBoolean(dr["Flg_Des_MetaBusca"]);
            objParametroBuscaCV._flagDesMetabuscaRapida = Convert.ToBoolean(dr["Flg_Des_Metabusca_Rapida"]);
            objParametroBuscaCV._flagIdfOrigem = Convert.ToBoolean(dr["Flg_Idf_Origem"]);
            objParametroBuscaCV._flagIdfEstadoCivil = Convert.ToBoolean(dr["Flg_Idf_Estado_Civil"]);
            objParametroBuscaCV._flagIdfFilial = Convert.ToBoolean(dr["Flg_Idf_Filial"]);
            objParametroBuscaCV._flagIdfsIdioma = Convert.ToBoolean(dr["Flg_Idfs_Idioma"]);
            objParametroBuscaCV._flagIdfsDisponibilidade = Convert.ToBoolean(dr["Flg_Idfs_Disponibilidade"]);
            objParametroBuscaCV._flagIdfRaca = Convert.ToBoolean(dr["Flg_Idf_Raca"]);
            objParametroBuscaCV._flagFlgFilhos = Convert.ToBoolean(dr["Flg_Flg_Filhos"]);
            objParametroBuscaCV._flagIdfVaga = Convert.ToBoolean(dr["Flg_Idf_Vaga"]);
            objParametroBuscaCV._flagIdfRastreador = Convert.ToBoolean(dr["Flg_Idf_Rastreador"]);
            objParametroBuscaCV._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
            if (dr["Nme_SP_Busca"] != DBNull.Value)
                objParametroBuscaCV._nomeSPBusca = Convert.ToString(dr["Nme_SP_Busca"]);
            objParametroBuscaCV._persisted = true;
            objParametroBuscaCV._modified = false;

            return true;
        }
        #endregion

        #endregion

    }
}