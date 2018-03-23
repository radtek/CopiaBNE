using BNE.BLL;
using BNE.EL;
using BNE.Web.Parceiros.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BNE.Web.Parceiros.Business
{
    public static class PessoaFisicaExtension
    {

        private const string QRY_REC_STC_LAN_PF = @"SELECT DISTINCT ufp.Idf_Usuario_Filial_Perfil, pf.Nme_Pessoa, edr.Idf_Cidade FROM BNE.TAB_Pessoa_Fisica pf WITH (NOLOCK) 
                                                    INNER JOIN BNE.TAB_Usuario_Filial_Perfil ufp WITH (NOLOCK) ON ufp.Idf_Pessoa_Fisica =  pf.Idf_Pessoa_Fisica
                                                    INNER JOIN BNE.TAB_Filial f WITH (NOLOCK) ON f.Idf_Filial = ufp.Idf_Filial
                                                    INNER JOIN BNE.TAB_Endereco edr WITH (NOLOCK) ON edr.Idf_Endereco = f.Idf_Endereco
                                                    WHERE f.Idf_Tipo_Parceiro = 90002 AND pf.Num_CPF = @Num_CPF AND pf.Dta_Nascimento = @Dta_Nascimento;";

        public static ItemSession RecuperarPessoaSTCLanHouse(this PessoaFisica pf, decimal numCPF, DateTime dataNascimento)
        {

            ItemSession item = null;

            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Num_CPF", SqlDbType = SqlDbType.Decimal, Value = numCPF },
                new SqlParameter { ParameterName = "@Dta_Nascimento", SqlDbType = SqlDbType.Date, Value = dataNascimento }
            };

            try
            {
                var dr = DataAccessLayer.ExecuteReader(CommandType.Text, QRY_REC_STC_LAN_PF, parms);
                if (dr.Read()) 
                {
                    item = new ItemSession()
                    {
                        idUsuarioFilialPerfil = dr.GetInt32(dr.GetOrdinal("Idf_Usuario_Filial_Perfil")),
                        NmePessoa = dr.GetString(dr.GetOrdinal("Nme_Pessoa")),
                        IdCidade = dr.GetInt32(dr.GetOrdinal("Idf_Cidade"))
                    };
                }
            }
            catch (Exception ex) 
            {
                GerenciadorException.GravarExcecao(ex);
            }

            return item;
        }
    }
}