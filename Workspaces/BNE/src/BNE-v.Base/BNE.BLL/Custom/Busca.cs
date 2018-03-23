using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BNE.BLL.Custom
{
    public class Busca
    {

        #region Consultas

        #region SPSELECTCLIENTES
        private const string SPSELECTCLIENTES = @"
        SELECT  *
        FROM    (   SELECT  PF.Idf_Pessoa_Fisica ,
		                    NULL AS Idf_Filial ,
                            PF.Nme_Pessoa AS Nome ,
                            Num_CPF AS 'Cadastro Pessoa' ,
                            PF.Dta_Nascimento ,
                            C.Nme_Cidade
                    FROM    BNE.TAB_Pessoa_Fisica PF WITH ( NOLOCK )
                            INNER JOIN BNE.TAB_Endereco E WITH ( NOLOCK ) ON PF.Idf_Endereco = E.Idf_Endereco
                            INNER JOIN plataforma.TAB_Cidade C WITH ( NOLOCK ) ON E.Idf_Cidade = C.Idf_Cidade
                            LEFT JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH ( NOLOCK ) ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
                            LEFT JOIN BNE.BNE_Pagamento P WITH ( NOLOCK ) ON UFP.Idf_Usuario_Filial_Perfil = P.Idf_Usuario_Filial_Perfil
                    WHERE   PF.Num_CPF  LIKE '%' + CONVERT(VARCHAR,@Num_CPF) + '%'
                            OR PF.Eml_Pessoa = @Eml_Pessoa
                            OR PF.Nme_Pessoa LIKE '%' + @NomeEnderecoBoleto + '%'
                            OR PF.Num_Celular LIKE '%' + @Telefone + '%'
                            OR PF.Num_Telefone LIKE '%' + @Telefone + '%'
                            OR E.Des_Logradouro LIKE '%' + @NomeEnderecoBoleto + '%'
                            OR P.Des_Identificador LIKE '%' + @NomeEnderecoBoleto + '%'
                            OR P.Des_Descricao LIKE '%' + @NumeroBoleto + '%'
                    UNION
                    SELECT  NULL ,
                            F.Idf_Filial ,
                            F.Raz_Social ,
                            Num_CNPJ ,
                            NULL ,
                            C.Nme_Cidade
                    FROM    BNE.TAB_Filial F WITH ( NOLOCK )
                            INNER JOIN BNE.TAB_Endereco E WITH ( NOLOCK ) ON F.Idf_Endereco = E.Idf_Endereco
                            INNER JOIN plataforma.TAB_Cidade C WITH ( NOLOCK ) ON E.Idf_Cidade = C.Idf_Cidade
                            LEFT JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH ( NOLOCK ) ON F.Idf_Filial = UFP.Idf_Filial
                            LEFT JOIN BNE.BNE_Usuario_Filial UF WITH ( NOLOCK ) ON UFP.Idf_Usuario_Filial_Perfil = UF.Idf_Usuario_Filial_Perfil
                            LEFT JOIN BNE.BNE_Pagamento P WITH ( NOLOCK ) ON UFP.Idf_Usuario_Filial_Perfil = P.Idf_Usuario_Filial_Perfil
                    WHERE   F.Num_CNPJ LIKE '%' + CONVERT(VARCHAR,@Num_CNPJ) + '%'
                            OR UF.Eml_Comercial = @Eml_Pessoa
                            OR F.Nme_Fantasia LIKE '%' + @NomeEnderecoBoleto + '%'
                            OR F.Raz_Social LIKE '%' + @NomeEnderecoBoleto + '%'
                            OR F.Num_Comercial LIKE '%' + @Telefone + '%'
                            OR F.Num_Comercial2 LIKE '%' + @Telefone + '%'
                            OR UF.Num_Comercial LIKE '%' + @Telefone + '%'
                            OR E.Des_Logradouro LIKE '%' + @NomeEnderecoBoleto + '%'
		                    OR P.Des_Identificador LIKE '%' + @NomeEnderecoBoleto + '%'
                            OR P.Des_Descricao LIKE '%' + @NumeroBoleto + '%'
                ) AS temp
        ORDER BY Nome";
        #endregion

        #region SPSELECTCLIENTESPESSOAJURIDICA
        private const string SPSELECTCLIENTESPESSOAJURIDICA = @"
        SELECT  NULL AS Idf_Pessoa_Fisica,
                F.Idf_Filial ,
                F.Raz_Social AS Nome,
                Num_CNPJ AS 'Cadastro Pessoa',
                NULL AS Dta_Nascimento,
                C.Nme_Cidade
        FROM    BNE.TAB_Filial F WITH ( NOLOCK )
                INNER JOIN BNE.TAB_Endereco E WITH ( NOLOCK ) ON F.Idf_Endereco = E.Idf_Endereco
                INNER JOIN plataforma.TAB_Cidade C WITH ( NOLOCK ) ON E.Idf_Cidade = C.Idf_Cidade
        WHERE   F.Num_CNPJ LIKE '%' + CONVERT(VARCHAR, @Num_CNPJ) + '%'
        ";
        #endregion

        #region SPSELECTCLIENTESPESSOAFISICA
        private const string SPSELECTCLIENTESPESSOAFISICA = @"
        SELECT  PF.Idf_Pessoa_Fisica ,
                NULL AS Idf_Filial ,
                PF.Nme_Pessoa AS Nome ,
                Num_CPF AS 'Cadastro Pessoa' ,
                PF.Dta_Nascimento ,
                C.Nme_Cidade
        FROM    BNE.TAB_Pessoa_Fisica PF WITH ( NOLOCK )
                INNER JOIN BNE.TAB_Endereco E WITH ( NOLOCK ) ON PF.Idf_Endereco = E.Idf_Endereco
                INNER JOIN plataforma.TAB_Cidade C WITH ( NOLOCK ) ON E.Idf_Cidade = C.Idf_Cidade
        WHERE   PF.Num_CPF = @Num_CPF

        ";
        #endregion

        #region SPSELECTCLIENTESBOLETO
        private const string SPSELECTCLIENTESBOLETO = @"
        SELECT  *
        FROM    ( SELECT    PF.Idf_Pessoa_Fisica ,
                            NULL AS Idf_Filial ,
                            PF.Nme_Pessoa AS Nome ,
                            Num_CPF AS 'Cadastro Pessoa' ,
                            PF.Dta_Nascimento ,
                            C.Nme_Cidade
                  FROM      BNE.TAB_Pessoa_Fisica PF WITH ( NOLOCK )
                            INNER JOIN BNE.TAB_Endereco E WITH ( NOLOCK ) ON PF.Idf_Endereco = E.Idf_Endereco
                            INNER JOIN plataforma.TAB_Cidade C WITH ( NOLOCK ) ON E.Idf_Cidade = C.Idf_Cidade
                            LEFT JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH ( NOLOCK ) ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
                            LEFT JOIN BNE.BNE_Pagamento P WITH ( NOLOCK ) ON UFP.Idf_Usuario_Filial_Perfil = P.Idf_Usuario_Filial_Perfil
                            INNER JOIN BNE.GLO_Transacao T WITH(NOLOCK) ON P.Cod_Guid = T.Cod_Guid
							INNER JOIN BNE.GLO_Cobranca_Boleto CB WITH(NOLOCK) ON T.Idf_Transacao = CB.Idf_Transacao
                  WHERE     CB.Num_Nosso_Numero LIKE '%' + @NumeroBoleto
							AND UFP.Idf_Filial IS NULL
                  UNION
                  SELECT    NULL ,
                            F.Idf_Filial ,
                            F.Raz_Social ,
                            Num_CNPJ ,
                            NULL ,
                            C.Nme_Cidade
                  FROM      BNE.TAB_Filial F WITH ( NOLOCK )
                            INNER JOIN BNE.TAB_Endereco E WITH ( NOLOCK ) ON F.Idf_Endereco = E.Idf_Endereco
                            INNER JOIN plataforma.TAB_Cidade C WITH ( NOLOCK ) ON E.Idf_Cidade = C.Idf_Cidade
                            LEFT JOIN BNE.BNE_Pagamento P WITH ( NOLOCK ) ON F.Idf_Filial = P.Idf_Filial
                            INNER JOIN BNE.GLO_Transacao T WITH(NOLOCK) ON P.Cod_Guid = T.Cod_Guid
							INNER JOIN BNE.GLO_Cobranca_Boleto CB WITH(NOLOCK) ON T.Idf_Transacao = CB.Idf_Transacao
                  WHERE     CB.Num_Nosso_Numero LIKE '%' + @NumeroBoleto
                ) AS temp
        ORDER BY Nome";
        #endregion

        #region SPSELECTPAGAMENTOTID
        private const string SPSELECTPAGAMENTOTID = @"
        SELECT  PF.Idf_Pessoa_Fisica,
                fl.Idf_Filial ,
                CASE WHEN pf.Idf_Pessoa_Fisica IS NULL THEN fl.Raz_Social
			        ELSE pf.Nme_Pessoa 
		        END AS Nome ,
		        CASE WHEN pf.Idf_Pessoa_Fisica IS NULL THEN fl.Num_CNPJ
			        ELSE pf.Num_CPF 
		        END AS 'Cadastro Pessoa' ,
                pf.Dta_Nascimento ,
                ci.Nme_Cidade
        FROM    BNE.GLO_Cobranca_Cartao cc
                LEFT JOIN BNE.GLO_Transacao tr WITH (NOLOCK) ON cc.Idf_Transacao = tr.Idf_Transacao
                LEFT JOIN BNE.BNE_Pagamento pg WITH (NOLOCK) ON tr.Cod_Guid = pg.Cod_Guid
                LEFT JOIN BNE.TAB_Pessoa_Fisica pf WITH (NOLOCK) ON pg.Des_Identificador = CONVERT(VARCHAR, pf.Num_CPF)
                LEFT JOIN plataforma.TAB_Cidade ci WITH (NOLOCK) ON PF.Idf_Cidade = ci.Idf_Cidade
                LEFT JOIN BNE.TAB_filial fl WITH (NOLOCK) ON pg.Idf_Filial = fl.Idf_Filial
        WHERE  cc.Cod_TID LIKE '%' + @Cod_TID + '%' ";
        #endregion

        #endregion

        #region PesquisarCliente
        public static DataTable PesquisarCliente(string pesquisa)
        {
            //Confirgurando parametros basicos da pesquisa
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter sqlParamCPF = new SqlParameter("@Num_CPF", SqlDbType.Decimal, 11);
            SqlParameter sqlParamCNPJ = new SqlParameter("@Num_CNPJ", SqlDbType.Decimal, 14);
            SqlParameter sqlParamTelefone = new SqlParameter("@Telefone", SqlDbType.Char, 8);
            SqlParameter sqlParamEmail = new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 100);
            SqlParameter sqlParamNEB = new SqlParameter("@NomeEnderecoBoleto", SqlDbType.VarChar, 120);
            SqlParameter sqlParamNumBoleto = new SqlParameter("@NumeroBoleto", SqlDbType.VarChar, 100);

            sqlParamCPF.Value = DBNull.Value;
            sqlParamCNPJ.Value = DBNull.Value;
            sqlParamTelefone.Value = DBNull.Value;
            sqlParamEmail.Value = DBNull.Value;
            sqlParamNEB.Value = DBNull.Value;
            sqlParamNumBoleto.Value = DBNull.Value;

            if (!String.IsNullOrEmpty(pesquisa))
            {
                //As validações dos Regex foram alterados pois o usuário poderá digitar os CPFs e os CNPjs pela metade com os caracteres especiais
                //29/03/2011

                //Verifica se é um número
                if (Regex.IsMatch(pesquisa, @"^\d*$"))
                {
                    //Não há validação se é cpf ou cnpj, pois se considera os dois numeros na query através do Like
                    decimal decimalValue;
                    if (Decimal.TryParse(pesquisa, out decimalValue))
                    {
                        sqlParamCNPJ.Value = decimalValue;
                        sqlParamCPF.Value = decimalValue;
                    }

                    sqlParamNumBoleto.Value = pesquisa;
                    sqlParamTelefone.Value = pesquisa;
                }
                else if (Regex.IsMatch(pesquisa, @"[.,\d*,-]")) //Senão verifica se é um número com os caracteres - ou .
                {

                    string valor = pesquisa.Replace(".", String.Empty).Replace(",", String.Empty).Replace("-", String.Empty);

                    //Se não converter é porque não é um cpf ou cnpj, pode ser que seja um email.
                    decimal decimalValue;
                    if (Decimal.TryParse(valor, out decimalValue))
                    {
                        sqlParamCNPJ.Value = decimalValue;
                        sqlParamCPF.Value = decimalValue;
                    }
                    else
                    {
                        if (Regex.IsMatch(pesquisa, @"^(\w+([-+.&apos;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)$"))
                            sqlParamEmail.Value = pesquisa;
                        else
                            sqlParamNumBoleto.Value = sqlParamNEB.Value = pesquisa;
                    }

                    sqlParamTelefone.Value = pesquisa.Replace("-", String.Empty);
                }
                else // Senão é texto
                {
                    if (Regex.IsMatch(pesquisa, @"^\d{8}$"))
                        sqlParamTelefone.Value = pesquisa;
                    else if (Regex.IsMatch(pesquisa, @"^(\w+([-+.&apos;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)$"))
                        sqlParamEmail.Value = pesquisa;
                    else
                        sqlParamNumBoleto.Value = sqlParamNEB.Value = pesquisa;
                }
            }

            parms.Add(sqlParamCPF);
            parms.Add(sqlParamCNPJ);
            parms.Add(sqlParamTelefone);
            parms.Add(sqlParamEmail);
            parms.Add(sqlParamNEB);
            parms.Add(sqlParamNumBoleto);

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTCLIENTES, parms).Tables[0];
        }
        #endregion

        #region PesquisarClientePessoaJuridica
        public static DataTable PesquisarClientePessoaJuridica(string pesquisa)
        {
            //Confirgurando parametros basicos da pesquisa
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter sqlParamCNPJ = new SqlParameter("@Num_CNPJ", SqlDbType.Decimal, 14);
            sqlParamCNPJ.Value = DBNull.Value;

            if (!String.IsNullOrEmpty(pesquisa))
            {
                //Verifica se é um número
                if (Regex.IsMatch(pesquisa, @"^\d*$"))
                {
                    //Não há validação se é cpf ou cnpj, pois se considera os dois numeros na query através do Like
                    decimal decimalValue;
                    if (Decimal.TryParse(pesquisa, out decimalValue))
                        sqlParamCNPJ.Value = decimalValue;
                }
                else if (Regex.IsMatch(pesquisa, @"[.,/\d*,-]")) //Senão verifica se é um número com os caracteres - ou . ou , ou /
                {
                    string valor = pesquisa.Replace(".", String.Empty).Replace(",", String.Empty).Replace("-", String.Empty).Replace("/", String.Empty);

                    //Se não converter é porque não é um cpf ou cnpj, pode ser que seja um email.
                    decimal decimalValue;
                    if (Decimal.TryParse(valor, out decimalValue))
                        sqlParamCNPJ.Value = decimalValue;
                }
            }

            parms.Add(sqlParamCNPJ);

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTCLIENTESPESSOAJURIDICA, parms).Tables[0];
        }
        #endregion

        #region PesquisarClientePessoaFisica
        public static DataTable PesquisarClientePessoaFisica(string pesquisa)
        {
            //Confirgurando parametros basicos da pesquisa
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter sqlParamCPF = new SqlParameter("@Num_CPF", SqlDbType.Decimal, 14);
            sqlParamCPF.Value = DBNull.Value;

            if (!String.IsNullOrEmpty(pesquisa))
            {
                //Verifica se é um número
                if (Regex.IsMatch(pesquisa, @"^\d*$"))
                {
                    //Não há validação se é cpf ou cnpj, pois se considera os dois numeros na query através do Like
                    decimal decimalValue;
                    if (Decimal.TryParse(pesquisa, out decimalValue))
                        sqlParamCPF.Value = decimalValue;
                }
                else if (Regex.IsMatch(pesquisa, @"[.,\d*,-]")) //Senão verifica se é um número com os caracteres - ou . ou ,
                {
                    string valor = pesquisa.Replace(".", String.Empty).Replace(",", String.Empty).Replace("-", String.Empty);

                    //Se não converter é porque não é um cpf ou cnpj, pode ser que seja um email.
                    decimal decimalValue;
                    if (Decimal.TryParse(valor, out decimalValue))
                        sqlParamCPF.Value = decimalValue;
                }
            }

            parms.Add(sqlParamCPF);

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTCLIENTESPESSOAFISICA, parms).Tables[0];
        }
        #endregion

        #region PesquisarClienteBoleto
        public static DataTable PesquisarClienteBoleto(string pesquisa)
        {
            //Confirgurando parametros basicos da pesquisa
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter sqlParamNumBoleto = new SqlParameter("@NumeroBoleto", SqlDbType.VarChar, 100);
            sqlParamNumBoleto.Value = DBNull.Value;

            if (!String.IsNullOrEmpty(pesquisa))
            {
                sqlParamNumBoleto.Value = pesquisa;
            }
            parms.Add(sqlParamNumBoleto);

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTCLIENTESBOLETO, parms).Tables[0];
        }
        #endregion

        #region PesquisarPagamentoTID
        public static DataTable PesquisarPagamentoTID(string pesquisa)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter sqlParamTID = new SqlParameter("@Cod_TID", SqlDbType.VarChar);
            sqlParamTID.Value = DBNull.Value;
            if (!String.IsNullOrEmpty(pesquisa))
            {
                sqlParamTID.Value = pesquisa;
            }
            parms.Add(sqlParamTID);

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTPAGAMENTOTID, parms).Tables[0];
        }

        #endregion

    }
}
