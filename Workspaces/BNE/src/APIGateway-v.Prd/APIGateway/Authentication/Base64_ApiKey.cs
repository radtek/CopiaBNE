using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Security.Authentication;
using APIGateway.Model;
using System.Text.RegularExpressions;
using APIGateway.Authentication.Classes;

namespace APIGateway.Authentication
{
    /// <summary>
    /// Autentica um usuário através do cpf e data de nascimento. 
    /// Se o usuário estiver vinculado a uma empresa, retorna o CNPJ da primeira empresa encontrada.
    /// </summary>
    public class Base64_ApiKey : APIGateway.Authentication.IAuthentication
    {
        const string SP_CONSULTA_USUARIO_DE_EMPRESA = @"SELECT
	                                                        pf.Num_CPF AS CPF,
	                                                        pf.Dta_Nascimento AS Dta_Nascimento,
	                                                        f.Num_CNPJ AS CNPJ,
	                                                        ufp.Idf_Perfil AS Perfil
                                                        FROM 
                                                            BNE.TAB_Pessoa_Fisica pf
                                                            JOIN BNE.TAB_Usuario_Filial_Perfil ufp ON pf.Idf_Pessoa_Fisica = ufp.Idf_Pessoa_Fisica
                                                            LEFT JOIN BNE.TAB_Filial f ON ufp.Idf_Filial = f.Idf_Filial
                                                        WHERE 
                                                            pf.Num_CPF = @cpf
                                                            AND (@cnpj IS NULL OR @cnpj = f.Num_CNPJ)
                                                            AND ufp.Flg_Inativo = 0
                                                            AND (f.Idf_Filial IS NULL OR 
			                                                        (f.Flg_Inativo = 0 AND 
                                                                    f.Idf_Situacao_Filial NOT IN (5,6))) --Não bloqueado e não cancelada
                                                        ORDER BY CNPJ DESC; -- Para aparecer os CNPJS no começo";

        public Usuario Authenticate(System.Net.Http.HttpRequestMessage request, out Model.SistemaCliente sistema)
        {
            //Verifica presenca da apiKey no cabecalho
            if (!request.Headers.Contains("apiKey"))
                throw new InvalidCredentialException("apiKey não informada no cabeçalho");

            string apiKey = request.Headers.GetValues("apiKey").First();
            string cacheKey = "Base64_PessoaJuridica_ApiKey:" + apiKey;

            UserCredentials credentials;
            try
            {
                credentials = DecodeCredentials(apiKey);
            }
            catch (Exception ex)
            {
                throw new InvalidCredentialException("Não foi possível recuperar as informações de autenticação do cabeçalho", ex);
            }

            if (credentials == null || credentials.Sistema == Guid.Empty)
                throw new InvalidCredentialException("Não foi possível recuperar as informações de autenticação do cabeçalho. Verifique a geração do key e tente novamente.");

            sistema = Domain.SistemaCliente.Get(credentials.Sistema);

            ///Efetuando pesquisa do usuário diretamente no BNE
            List<SqlParameter> cmdParms = new List<SqlParameter>{
                new SqlParameter{ ParameterName = "@cpf", SqlDbType = SqlDbType.Decimal, Size = 11, Value = credentials.CPF},
                new SqlParameter{ ParameterName = "@nascimento", SqlDbType = SqlDbType.Date, Size = 11, Value = credentials.DataNascimento},
                new SqlParameter{ ParameterName = "@cnpj", SqlDbType = SqlDbType.Decimal, Size = 14, Value = DBNull.Value},
            };
            if (credentials.CNPJ.HasValue)
                cmdParms[2].Value = credentials.CNPJ.Value;

            using (var dr = Utils.DataAccessLayer.ExecuteReader("BNE", CommandType.Text, SP_CONSULTA_USUARIO_DE_EMPRESA, cmdParms))
            {
                if (!dr.Read())
                    throw new AuthenticationException("Falha na autenticação. Verifique os dados de acesso.");

                var dn = Convert.ToDateTime(dr["Dta_Nascimento"]);

                if(dn.Date != credentials.DataNascimento.Date)
                    throw new AuthenticationException("Falha na autenticação. Data de nascimento não confere com a cadastrada.");

                credentials.CNPJ = dr["CNPJ"] == DBNull.Value ? 
                    null : (decimal?)Convert.ToDecimal(dr["CNPJ"]);
                decimal idPerfilBNE = Convert.ToDecimal(dr["Perfil"]);
            }

            if (credentials.CNPJ.HasValue)
            {
                var usuario = Domain.UsuarioPessoaJuridica.Obter(credentials.CPF, credentials.CNPJ.Value);
                //Adicionando headers a serem repassados à API destino
                usuario.ForwardHeaders = new List<Model.Header> {
                        new Header(){Item = "Num_CPF", Value = credentials.CPF.ToString()},
                        new Header(){Item = "Dta_Nascimento", Value = credentials.DataNascimento.ToString("yyyy-MM-dd")},
                        new Header(){Item = "Num_CNPJ",Value = credentials.CNPJ.ToString() }
                    };
                usuario.PerfilDeAcesso = Model.Perfil.EmpresaBNE;
                return usuario;
            }else
            {
                var usuario = Domain.UsuarioPessoaFisica.Obter(credentials.CPF);
                //Adicionando headers a serem repassados à API destino
                usuario.ForwardHeaders = new List<Model.Header> {
                        new Header(){Item = "Num_CPF", Value = credentials.CPF.ToString()},
                        new Header(){Item = "Dta_Nascimento", Value = credentials.DataNascimento.ToString("yyyy-MM-dd")},
                };
                usuario.PerfilDeAcesso = Model.Perfil.PessoaFisicaBNE;
                return usuario;
            }
        }

        internal static UserCredentials DecodeCredentials(string base64)
        {
            Regex reRetiraZerosEsquerda = new Regex("(?<=\"(CPF|CNPJ)\": *)0+", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            UserCredentials credentials = null;
            try
            {
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                json = reRetiraZerosEsquerda.Replace(json, String.Empty);
                credentials = JsonConvert.DeserializeObject<UserCredentials>(json);
            }
            catch(Exception ex) 
            {
                throw (ex);
            }
            return credentials;
        }
    }


}