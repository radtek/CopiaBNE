//-- Data: 17/03/2016 14:13
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class UsuarioFilialContato : ICloneable // Tabela: BNE_Usuario_Filial_Contato
    {
        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region CPF
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public string CPF
        {
            get
            {
                if (this._numeroCPF <= 0)
                    return string.Empty;
                else
                    return this._numeroCPF.ToString().PadLeft(11, '0').Insert(3, ".").Insert(7, ".").Insert(11, "-");
            }
            set
            {
                if (value == null)
                {
                    this._numeroCPF = null;
                }
                else
                {
                    Decimal numeroCPF;
                    if (Decimal.TryParse(value.Replace(".", "").Replace("-", ""), out numeroCPF))
                    {
                        this._numeroCPF = numeroCPF;
                    }
                    else
                    {
                        this._numeroCPF = null;
                    }
                }
                this._modified = true;
            }
        }
        #endregion

        #region Consultas

        #region SpselectContatosCadastradosporfilial
        private const string SpselectContatosCadastradosporfilial = @"
        SELECT  UFC.Idf_Usuario_Filial_Contato ,
                UFC.Num_CPF ,
                CASE WHEN ( UFC.Des_Funcao IS NULL )
                     THEN ( SELECT  Des_Funcao
                            FROM    plataforma.TAB_Funcao WITH ( NOLOCK )
                            WHERE   Idf_Funcao = UFC.Idf_Funcao
                          )
                     ELSE UFC.Des_Funcao
                END AS Des_Funcao ,
                UFC.Nme_Contato ,
                UFC.DDD_Telefone ,
                UFC.Num_Telefone ,
                UFC.Num_DDD_Celular ,
                UFC.Num_Celular ,
                UFC.Flg_Inativo ,
                UFC.Eml_Contato ,
                TotalCount = COUNT(*) OVER ( )
        FROM    BNE_Usuario_Filial_Contato UFC WITH ( NOLOCK )
        WHERE   UFC.Idf_Filial = @Idf_Filial
                AND ( @Flg_Inativo IS NULL
                      OR UFC.Flg_Inativo = @Flg_Inativo
                    )
        ORDER BY UFC.Dta_Cadastro DESC 
        OFFSET @PageSize * @PageNumber ROWS
        FETCH NEXT @PageSize ROWS ONLY;";
        #endregion

        #region Spinativar
        private const string Spinativar = "UPDATE BNE_Usuario_Filial_Contato SET Flg_Inativo = @Flg_Inativo WHERE Idf_Usuario_Filial_Contato = @Idf_Usuario_Filial_Contato";
        #endregion

        #region [spSelectEmail]
        private const string spSelectEmail = @"select uf.eml_comercial from tab_usuario_filial_perfil ufp with(nolock) 
                    join bne_usuario_filial uf with(nolock) on ufp.idf_usuario_filial_perfil = uf.idf_usuario_filial_Perfil
                    where ufp.idf_pessoa_fisica = @Idf_Pessoa_Fisica 
                    and ufp.flg_inativo = 0 and ufp.idf_filial = @Idf_Filial ";
        #endregion
        #endregion

        #region Métodos

        #region CarregarContatosCadastradosPorFilial
        /// <summary>
        /// Carrega os contatos cadastrados pela filial informada
        /// </summary>
        /// <returns></returns>
        public static DataTable CarregarContatosCadastradosPorFilial(int paginaCorrente, int tamanhoPagina, int idFilial, int idPerfilUsuarioLogado, int idUsuarioFilialPerfilLogado, bool? flagAtivo, out int totalRegistros, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Filial", SqlDbType.Int, 4),
                    new SqlParameter("@PageNumber", SqlDbType.Int, 4),
                    new SqlParameter("@PageSize", SqlDbType.Int, 4),
                    new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1)
                };

            parms[0].Value = idFilial;
            parms[1].Value = paginaCorrente;
            parms[2].Value = tamanhoPagina;

            if (flagAtivo.HasValue)
                parms[3].Value = (bool)flagAtivo ? 0 : 1;
            else
                parms[3].Value = DBNull.Value;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SpselectContatosCadastradosporfilial, parms))
                {
                    dt = new DataTable();
                    dt.Load(dr);

                    if (dt.Rows.Count > 0)
                    {
                        totalRegistros = Convert.ToInt32(dt.Rows[0]["TotalCount"]);
                    }
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }
        #endregion

        #region Inativar
        /// <summary>
        /// Inativa o registro atual
        /// </summary>
        public void Inativar(SqlTransaction trans = null)
        {
            AtivarInativar(true, trans);
        }
        #endregion

        #region Ativar
        public void Ativar(SqlTransaction trans = null)
        {
            AtivarInativar(false, trans);
        }
        #endregion

        #region [EmailComercialPorIdPessoaFisica]
        public static string EmailComercialPorIdPessoaFisica(int IdPessoaFisica, int idFilial)
        {
            var parm = new List<SqlParameter>{
                new SqlParameter{ParameterName="@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Value = IdPessoaFisica},
                new SqlParameter{ParameterName="@Idf_Filial", SqlDbType = SqlDbType.Int, Value = idFilial}
            };
            string email = string.Empty;
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spSelectEmail, parm))
            {
                if (dr.Read())
                    email = dr["Eml_Comercial"].ToString();

                if (!dr.IsClosed)
                    dr.Close();
            }

            return email;
        }
        #endregion

        #endregion

        #region [Métodos Privados]

        #region Ativar/Inativar
        private void AtivarInativar(bool status, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Contato", SqlDbType = SqlDbType.Int, Size = 4, Value = this._idUsuarioFilialContato},
                new SqlParameter { ParameterName = "@Flg_Inativo", SqlDbType = SqlDbType.Bit, Value = status}
            };
            if(trans!= null)
                 DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spinativar, parms);
            else
                 DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spinativar, parms);
        }
        #endregion

        #endregion

    }
}