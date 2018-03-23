using Bne.Web.Services.API.DTO.Empresas;
using BNE.BLL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bne.Web.Services.API.Business
{
    public class Empresa
    {
        #region Consultas

        #region QUERY_SELECIONADORA
        const string QUERY_SELECIONADORA = @"
                    SELECT pf.Idf_Pessoa_Fisica,
		                    pf.Num_CPF,
		                    pf.Nme_Pessoa,
		                    pf.Idf_Sexo,
		                    pf.Dta_Nascimento,
		                    pf.Num_DDD_Celular AS DDDCelularPessoa,
		                    pf.Num_Celular AS NumCelularPessoa,
		                    pf.Num_DDD_Telefone AS DDDTelefonePessoa,
		                    pf.Num_Telefone AS NumTelefonePessoa,
		                    pf.Eml_Pessoa AS EmailPessoa,
		                    f.Num_CNPJ,
		                    f.Raz_Social,
		                    f.Nme_Fantasia,
		                    f.Flg_Matriz,
		                    f.End_Site,
		                    f.Num_DDD_Comercial AS DDDTelefoneEmpresa,
		                    f.Num_Comercial AS NumTelefoneEmpresa,
		                    endF.Num_CEP,
		                    endF.Des_Logradouro,
		                    endF.Num_Endereco,
		                    endF.Des_Bairro,
		                    cid.Nme_Cidade,
		                    cid.Sig_Estado
                    FROM BNE.TAB_Pessoa_Fisica pf
                    JOIN BNE.TAB_Usuario_Filial_Perfil ufp ON pf.Idf_Pessoa_Fisica = ufp.Idf_Pessoa_Fisica
                    JOIN bne.TAB_Filial f ON ufp.Idf_Filial = f.Idf_Filial
                    JOIN BNE.TAB_Endereco endF ON f.Idf_Endereco = endF.Idf_Endereco
                    JOIN plataforma.TAB_Cidade cid ON endF.Idf_Cidade = cid.Idf_Cidade
                    WHERE
                    f.Flg_Inativo = 0
                    AND ufp.Flg_Inativo = 0
                    AND pf.Flg_Inativo = 0
                    AND pf.Num_CPF = @Num_CPF";

        #endregion QUERY_SELECIONADORA

        #endregion Consultas

        public static Selecionadora ObterSelecionadora(decimal cpf)
        {
            Selecionadora retorno = null;
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Num_CPF", SqlDbType.Decimal) {  Value = cpf },
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, QUERY_SELECIONADORA, parms))
            {
                if (dr.Read())
                {
                    retorno = new Selecionadora(dr);

                    do
                    {
                        retorno.Empresas.Add(new DTO.Empresas.EmpresaSelecionadora(dr));
                    } while (dr.Read());
                }
            }
            return retorno;
        }
    }
}