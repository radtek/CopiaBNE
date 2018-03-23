using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace BNE.Busca
{
    public partial class CLR
    {
        
        [SqlFunction(DataAccess = DataAccessKind.Read)]
        [return: SqlFacet(MaxSize=-1)]
        public static SqlString BNE_BuscaInfoPF(SqlInt32 idPessoaFisica)
        {
            StringBuilder valor = new StringBuilder(null);

            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT abne.Des_Area_BNE_Pesquisa + ' ' + xp.Raz_Social + ' ' + xp.Des_Atividade AS frase FROM BNE.BNE_Experiencia_Profissional xp JOIN plataforma.TAB_Area_BNE abne ON xp.Idf_Area_BNE = abne.Idf_Area_BNE WHERE xp.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica", conn))
                {
                    SqlParameter parm = new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int);
                    parm.Value = idPessoaFisica.Value;

                    cmd.Parameters.Add(parm);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            valor.Append(dr["frase"].ToString().ToLower());
                            valor.Append(" ");
                        }
                        dr.Close();
                        dr.Dispose();
                    }
                }


                using (SqlCommand cmd = new SqlCommand("SELECT tt.frase FROM (SELECT ISNULL(fo.Nme_Fonte,'') + ' ' + ISNULL(fo.Sig_Fonte,'') + ' ' + ISNULL(c.Des_Curso,F.Des_Curso) frase FROM BNE.BNE_Formacao f LEFT JOIN BNE.TAB_Fonte fo ON f.Idf_Fonte = fo.Idf_Fonte LEFT JOIN BNE.TAB_Curso c ON f.Idf_Curso = c.Idf_Curso WHERE f.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica) AS tt WHERE tt.frase <> ''", conn))
                {
                    SqlParameter parm = new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int);
                    parm.Value = idPessoaFisica.Value;

                    cmd.Parameters.Add(parm);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string valorAux = dr["frase"].ToString().Trim().ToLower();
                            if (!String.IsNullOrEmpty(valorAux))
                            {
                                valor.Append(valorAux);
                                valor.Append(" ");
                            }
                        }
                        dr.Close();
                        dr.Dispose();
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT Des_Idioma FROM BNE.TAB_Pessoa_Fisica_Idioma pfi JOIN BNE.TAB_Idioma i ON pfi.Idf_Idioma = i.Idf_Idioma WHERE pfi.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica", conn))
                {
                    SqlParameter parm = new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int);
                    parm.Value = idPessoaFisica.Value;

                    cmd.Parameters.Add(parm);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string valorAux = dr["Des_Idioma"].ToString().Trim().ToLower();
                            if (!String.IsNullOrEmpty(valorAux))
                            {
                                valor.Append(valorAux);
                                valor.Append(" ");
                            }
                        }
                        dr.Close();
                    }
                }

            }
            return valor.ToString();
        }

        [SqlFunction(DataAccess = DataAccessKind.Read)]
        public static SqlString BNE_BuscaInfoPFVeiculo(SqlInt32 idPessoaFisica)
        {
            StringBuilder valor = new StringBuilder(null);

            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT TV.Des_Tipo_Veiculo FROM BNE.TAB_Pessoa_Fisica_Veiculo PFV INNER JOIN plataforma.TAB_Tipo_Veiculo TV ON PFV.Idf_Tipo_Veiculo = TV.Idf_Tipo_Veiculo WHERE PFV.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND PFV.Flg_Inativo = 0", conn))
                {
                    SqlParameter parm = new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int);
                    parm.Value = idPessoaFisica.Value;

                    cmd.Parameters.Add(parm);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            valor.Append(dr["Des_Tipo_Veiculo"].ToString().ToLower());
                            valor.Append(" ");
                        }
                        dr.Close();
                    }
                }
            }
            return valor.ToString();
        }

        [SqlFunction(DataAccess = DataAccessKind.Read)]
        [return: SqlFacet(MaxSize = -1)]
        public static SqlString BNE_BuscaInfoPFExp(SqlInt32 idPessoaFisica)
        {
            StringBuilder valor = new StringBuilder(null);

            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT EP.Des_Atividade FROM BNE.BNE_Experiencia_Profissional EP WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND Flg_Inativo = 0", conn))
                {
                    SqlParameter parm = new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int);
                    parm.Value = idPessoaFisica.Value;

                    cmd.Parameters.Add(parm);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            valor.Append(dr["Des_Atividade"].ToString().ToLower());
                            valor.Append(" ");
                        }
                        if (!dr.IsClosed)
                            dr.Close();
                    }
                }
            }
            return valor.ToString();
        }

        [SqlFunction(DataAccess = DataAccessKind.Read)]
        public static SqlString BNE_BuscaInfoPFCurso(SqlInt32 idPessoaFisica)
        {
            StringBuilder valor = new StringBuilder(null);

            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(C.Des_Curso, f.Des_Curso) Des_Curso FROM BNE.BNE_Formacao f LEFT JOIN BNE.TAB_Curso C ON f.Idf_Curso = C.Idf_Curso WHERE f.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND f.Flg_Inativo = 0", conn))
                {
                    SqlParameter parm = new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int);
                    parm.Value = idPessoaFisica.Value;

                    cmd.Parameters.Add(parm);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            valor.Append(dr["Des_Curso"].ToString().ToLower());
                            valor.Append(" ");
                        }
                        dr.Close();
                    }
                }
            }
            return valor.ToString();
        }

        [SqlFunction(DataAccess = DataAccessKind.Read)]
        public static SqlString BNE_BuscaInfoPFFonte(SqlInt32 idPessoaFisica)
        {
            StringBuilder valor = new StringBuilder(null);

            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(Fo.Nme_Fonte, f.Des_Fonte) Des_Fonte FROM BNE.BNE_Formacao f LEFT JOIN BNE.TAB_Fonte Fo ON f.Idf_Fonte = Fo.Idf_Fonte WHERE f.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND f.Flg_Inativo = 0", conn))
                {
                    SqlParameter parm = new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int);
                    parm.Value = idPessoaFisica.Value;

                    cmd.Parameters.Add(parm);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            valor.Append(dr["Des_Fonte"].ToString().ToLower());
                            valor.Append(" ");
                        }
                        if (!dr.IsClosed)
                            dr.Close();
                    }
                }
            }
            return valor.ToString();
        }

        [SqlFunction(DataAccess = DataAccessKind.Read)]
        public static SqlString BNE_BuscaInfoPFEmpresa(SqlInt32 idPessoaFisica)
        {
            StringBuilder valor = new StringBuilder(null);

            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT EP.Raz_Social FROM BNE.BNE_Experiencia_Profissional EP WHERE EP.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND EP.Flg_Inativo = 0", conn))
                {
                    SqlParameter parm = new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int);
                    parm.Value = idPessoaFisica.Value;

                    cmd.Parameters.Add(parm);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            valor.Append(dr["Raz_Social"].ToString().ToLower());
                            valor.Append(" ");
                        }
                        dr.Close();
                    }
                }
            }
            return valor.ToString();
        }

        [SqlFunction]
        public static SqlString BNE_BuscaMontaFT(SqlString valor)
        {
            if (valor.IsNull)
                return "(\"\")";
            // Entrada: ingles, pucpr  // ou // ingles pucpr
            // Saída: ("ingles" AND "PUCPR")

            String[] valores = ((String)valor).Replace("--", " ").Replace(",", " ").Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (valores.Length > 0)
                return "(\"" + String.Join("\" AND \"", valores) + "\")";


            return "(\"\")";
        }

        [SqlFunction]
        public static SqlString BNE_Busca_MontaFormsOfComInflectional(SqlString valor)
        {
            if (valor.IsNull)
                return "(\"\")";

            // Entrada: Técnico em Mineração
            // Saída: FORMSOF(INFLECTIONAL, "Técnico") AND FORMSOF(INFLECTIONAL, "Mineração")

            StringBuilder sb = new StringBuilder((String)valor);
            sb.Replace("--", " ");
            sb.Replace(",", " ");
            
            //Limpando stop words
            sb.Replace(" de ", " ");
            sb.Replace(" em ", " ");

            String[] valores = sb.ToString().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < valores.Length; i++)
            {
                //Caso seja a última palavra
                if (i.Equals(valores.Length - 1))
                    valores[i] = String.Format("( FORMSOF(INFLECTIONAL, \"{0}\") OR \"{0}*\" )", valores[i].Trim());
                else //Se não for a última palavras
                    valores[i] = String.Format("FORMSOF(INFLECTIONAL, \"{0}\")", valores[i].Trim());
            }

            if (valores.Length > 0)
                return "(" + String.Join(" AND ", valores) + ")";

            return "(\"\")";
        }

        [SqlFunction]
        public static SqlString BNE_BuscaMontaFT_OR(SqlString valor)
        {
            if (valor.IsNull)
                return "(\"\")";
            // Entrada: ingles, pucpr  // ou // ingles pucpr
            // Saída: ("ingles" OR "PUCPR")

            String[] valores = ((String)valor).Replace("--", " ").Replace(",", " ").Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (valores.Length > 0)
                return "(\"" + String.Join("\" OR \"", valores) + "\")";


            return "(\"\")";
        }

        [SqlFunction]
        public static SqlString BNE_BuscaMontaFT_Bairro(SqlString valor)
        {
            if (valor.IsNull)
                return "(\"\")";
            // Entrada: Centro, Bom Retiro  // ou // Centro; Bom Retiro
            // Saída: ("Centro" OR "Bom Retiro")

            String[] valores = ((String)valor).Replace("--", " ").Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < valores.Length; i++)
            {
                valores[i] = valores[i].Trim();
            }

            if (valores.Length > 0)
                return "(\"" + String.Join("\" OR \"", valores) + "\")";


            return "(\"\")";
        }

        [SqlFunction(DataAccess = DataAccessKind.Read)]
        [return: SqlFacet(MaxSize = -1)]
        public static SqlString BNE_BuscaInfoVaga(SqlInt32 idVaga)
        {
            StringBuilder valor = new StringBuilder(null);

            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT Cod_Vaga + ' ' + ISNULL(Des_Atribuicoes, '') + ' ' + ISNULL(Des_Beneficio, '') + ' ' + ISNULL(Des_Requisito, '') AS Descricao FROM BNE.BNE_Vaga WHERE Idf_Vaga = @Idf_Vaga", conn))
                {
                    SqlParameter parm = new SqlParameter("@Idf_Vaga", SqlDbType.Int);
                    parm.Value = idVaga.Value;

                    cmd.Parameters.Add(parm);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            valor.Append(dr["Descricao"].ToString().ToLower());
                            valor.Append(" ");
                        }
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }

            return valor.ToString();
        }

        #region MontarUrlCurriculo
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeCidade"></param>
        /// <param name="siglaEstado"></param>
        /// <param name="identificadorCurriculo"></param>
        /// <returns></returns>
        public static string MontarUrlCurriculo(SqlString nomeFuncao, SqlString nomeCidade, SqlString siglaEstado, SqlString identificadorCurriculo)
        {
            return String.Empty;
        }
        #endregion

    }

}