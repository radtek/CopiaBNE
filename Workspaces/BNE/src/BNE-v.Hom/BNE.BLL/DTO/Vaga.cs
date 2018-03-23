using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using BNE.EL;
using Microsoft.SqlServer.Types;

namespace BNE.BLL.DTO
{
    public class Vaga
    {

        public int IdVaga { get; set; }
        public string CodigoVaga { get; set; }
        public int IdFuncao { get; set; }
        public string SiglaEstado { get; set; }
        public int IdCidade { get; set; }
        public string NomeCidade { get; set; }

        public string EmailVaga { get; set; }

        public int? IdSexo { get; set; }
        public string DescricaoEscolaridade { get; set; }

        public int? IdadeMinima { get; set; }
        public int? IdadeMaxima { get; set; }
        public string DescricaoFuncao { get; set; }
        public int QuantidadeVaga { get; set; }
        public string DescricaoRequisito { get; set; }
        public string DescricaoAtribuicoes { get; set; }
        public string DescricaoBeneficio { get; set; }
        public decimal? ValorSalarioInicial { get; set; }
        public decimal? ValorSalarioFinal { get; set; }
        public string DescricaoDisponibilidade { get; set; }
        public string DescricaoTipoVinculo { get; set; }

        public string NomeSelecionadora { get; set; }
        public int IdEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public decimal CNPJEmpresa { get; set; }

        public bool FlagConfidencial { get; set; }

        //public SqlGeography Localizacao { get; set; }

        #region CarregarVaga
        /// <summary>
        /// Método utilizado para retornar uma instância completa de Vaga a partir do banco de dados.
        /// </summary>
        /// <param name="idVaga">Chave do registro.</param>
        /// <returns>Instância de Filial.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Vaga CarregarVaga(int idVaga)
        {
            using (IDataReader dr = RetornarDataReader(idVaga))
            {
                var objVaga = new DTO.Vaga();
                if (SetInstance(dr, objVaga))
                    return objVaga;
            }
            throw (new RecordNotFoundException(typeof(DTO.Vaga)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Filial a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idVaga">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Filial.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Vaga CarregarVaga(int idVaga, SqlTransaction trans)
        {
            using (IDataReader dr = RetornarDataReader(idVaga, trans))
            {
                var objVaga = new DTO.Vaga();
                if (SetInstance(dr, objVaga))
                    return objVaga;
            }
            throw (new RecordNotFoundException(typeof(DTO.Vaga)));
        }
        #endregion

        #region RetornarDataReader
        public static IDataReader RetornarDataReader(int idVaga)
        {
            return RetornarDataReader(idVaga, null);
        }
        public static IDataReader RetornarDataReader(int idVaga, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                };

            #region spselectVaga
            const string spselectVaga = @"
                SELECT  V.Idf_Vaga, 
                        V.Cod_Vaga,
                        V.Idf_Funcao, 
                        Cid.Idf_Cidade, 
                        V.Idf_Sexo, 
                        V.Idf_Escolaridade, 
                        F.Des_Funcao,    
                        V.Des_Atribuicoes,
                        V.Des_Requisito,
                        V.Des_Beneficio,
                        V.Num_Idade_Minima, 
                        V.Num_Idade_Maxima, 
                        V.Vlr_Salario_De, 
                        V.Vlr_Salario_Para,
                        V.Eml_Vaga,
                        V.Flg_Confidencial,
                        Cid.Nme_Cidade,
                        Cid.Sig_Estado,
                        F.Des_Funcao,
                        V.Qtd_Vaga ,
                        (
			                SELECT	D.Des_Disponibilidade + ' '
			                FROM	BNE_Vaga_Disponibilidade VD WITH(NOLOCK) 
					                LEFT JOIN TAB_Disponibilidade D WITH(NOLOCK) ON VD.Idf_Disponibilidade = D.Idf_Disponibilidade
			                WHERE	V.Idf_Vaga = VD.Idf_Vaga
			                FOR XML PATH('')
                        ) [Des_Disponibilidade] ,
                        (
			                SELECT	TV.Des_Tipo_Vinculo + ' '
			                FROM	BNE_Vaga_Tipo_Vinculo VTV WITH(NOLOCK)
					                LEFT JOIN BNE_Tipo_Vinculo TV WITH(NOLOCK) ON TV.Idf_Tipo_Vinculo = VTV.Idf_Tipo_Vinculo
			                WHERE	VTV.Idf_Vaga = V.Idf_Vaga
			                FOR XML PATH('')
                        ) [Des_Tipo_Vinculo],
                        --V.Des_Localizacao ,
                        PF.Nme_Pessoa ,
                        Fil.Nme_Fantasia ,
                        Fil.Num_CNPJ ,
                        Esc.Des_BNE ,
                        V.Flg_Confidencial ,
                        Fil.Idf_Filial
                FROM    BNE_Vaga V WITH(NOLOCK)
                        INNER JOIN plataforma.TAB_Cidade Cid WITH(NOLOCK) ON V.Idf_Cidade = Cid.Idf_Cidade
                        INNER JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON V.Idf_Funcao = F.Idf_Funcao
                        INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON V.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                        INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                        INNER JOIN TAB_Filial Fil WITH(NOLOCK) ON V.Idf_Filial = Fil.Idf_Filial
                        LEFT JOIN plataforma.TAB_Escolaridade Esc WITH(NOLOCK) ON V.Idf_Escolaridade = Esc.Idf_Escolaridade
                WHERE   V.Idf_Vaga = @Idf_Vaga
                ";
            #endregion

            if (trans != null)
                return DataAccessLayer.ExecuteReader(trans, CommandType.Text, spselectVaga, parms);

            return DataAccessLayer.ExecuteReader(CommandType.Text, spselectVaga, parms);
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objVaga">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, Vaga objVaga)
        {
            try
            {
                if (dr.Read())
                {
                    objVaga.IdVaga = Convert.ToInt32(dr["Idf_Vaga"]);

                    if (dr["Cod_Vaga"] != DBNull.Value)
                        objVaga.CodigoVaga = dr["Cod_Vaga"].ToString();

                    if (dr["Idf_Funcao"] != DBNull.Value)
                        objVaga.IdFuncao = Convert.ToInt32(dr["Idf_Funcao"]);

                    if (dr["Idf_Cidade"] != DBNull.Value)
                        objVaga.IdCidade = Convert.ToInt32(dr["Idf_Cidade"]);

                    if (dr["Idf_Sexo"] != DBNull.Value)
                        objVaga.IdSexo = Convert.ToInt32(dr["Idf_Sexo"]);

                    if (dr["Des_BNE"] != DBNull.Value)
                        objVaga.DescricaoEscolaridade = Convert.ToString(dr["Des_BNE"]);

                    if (dr["Des_Funcao"] != DBNull.Value)
                        objVaga.DescricaoFuncao = Convert.ToString(dr["Des_Funcao"]);

                    if (dr["Qtd_Vaga"] != DBNull.Value)
                        objVaga.QuantidadeVaga = Convert.ToInt16(dr["Qtd_Vaga"]);

                    if (dr["Des_Atribuicoes"] != DBNull.Value)
                        objVaga.DescricaoAtribuicoes = Convert.ToString(dr["Des_Atribuicoes"]);

                    if (dr["Des_Requisito"] != DBNull.Value)
                        objVaga.DescricaoRequisito = Convert.ToString(dr["Des_Requisito"]);

                    if (dr["Des_Beneficio"] != DBNull.Value)
                        objVaga.DescricaoBeneficio = Convert.ToString(dr["Des_Beneficio"]);

                    if (dr["Num_Idade_Minima"] != DBNull.Value)
                        objVaga.IdadeMinima = Convert.ToInt32(dr["Num_Idade_Minima"]);

                    if (dr["Num_Idade_Maxima"] != DBNull.Value)
                        objVaga.IdadeMaxima = Convert.ToInt32(dr["Num_Idade_Maxima"]);

                    if (dr["Vlr_Salario_De"] != DBNull.Value)
                        objVaga.ValorSalarioInicial = Convert.ToInt32(dr["Vlr_Salario_De"]);

                    if (dr["Vlr_Salario_Para"] != DBNull.Value)
                        objVaga.ValorSalarioFinal = Convert.ToInt32(dr["Vlr_Salario_Para"]);

                    if (dr["Eml_Vaga"] != DBNull.Value)
                        objVaga.EmailVaga = Convert.ToString(dr["Eml_Vaga"]);

                    if (dr["Nme_Cidade"] != DBNull.Value)
                        objVaga.NomeCidade = Convert.ToString(dr["Nme_Cidade"]);

                    if (dr["Sig_Estado"] != DBNull.Value)
                        objVaga.SiglaEstado = Convert.ToString(dr["Sig_Estado"]);

                    if (dr["Des_Disponibilidade"] != DBNull.Value)
                        objVaga.DescricaoDisponibilidade = Convert.ToString(dr["Des_Disponibilidade"]);

                    if (dr["Des_Tipo_Vinculo"] != DBNull.Value)
                        objVaga.DescricaoTipoVinculo = Convert.ToString(dr["Des_Tipo_Vinculo"]);

                    //if (dr["Des_Localizacao"] != DBNull.Value)
                    //    objVaga.Localizacao = (SqlGeography)dr["Des_Localizacao"];

                    if (dr["Nme_Pessoa"] != DBNull.Value)
                        objVaga.NomeSelecionadora = Convert.ToString(dr["Nme_Pessoa"]);

                    if (dr["Nme_Fantasia"] != DBNull.Value)
                        objVaga.NomeEmpresa = Convert.ToString(dr["Nme_Fantasia"]);

                    if (dr["Num_CNPJ"] != DBNull.Value)
                        objVaga.CNPJEmpresa = Convert.ToDecimal(dr["Num_CNPJ"]);

                    if (dr["Flg_Confidencial"] != DBNull.Value)
                        objVaga.FlagConfidencial = Convert.ToBoolean(dr["Flg_Confidencial"]);

                    if (dr["Idf_Filial"] != DBNull.Value)
                        objVaga.IdEmpresa = Convert.ToInt32(dr["Idf_Filial"]);

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
            finally
            {
                dr.Dispose();
            }
        }
        #endregion

    }
}
