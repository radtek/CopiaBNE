using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BNE.BLL.Common;
using BNE.BLL.Custom;
using BNE.BLL.Integracoes.Google;

namespace BNE.BLL
{
    public class UsuarioParceiro
    {

        private const string URL_HEADER = @"http://www.bne.com.br/";
        private const string URL_TAIL = @"vagas-de-emprego-para-{0}-em-{1}-{2}";

        private const string FIND_ULT_FUN = @"SELECT TOP 1 Idf_Vaga FROM BNE.BNE_Vaga WITH (NOLOCK) WHERE Idf_Funcao = @Idf_Funcao AND Dta_Abertura > @Dta_Abertura AND @Idf_Cidade = @Idf_Cidade ORDER BY Dta_Abertura DESC;";

        private const string UPDATE_ENVIO_SMS = @"UPDATE BNE.BNE_Usuario_Parceiro SET Dta_Ultimo_Envio_SMS = GETDATE() WHERE Idf_Usuario_Parceiro = @Idf_Usuario_Parceiro";


        private const string INSERT = @"INSERT INTO BNE.BNE_Usuario_Parceiro (Idf_Tipo_Usuario_Parceiro ,Idf_Usuario_Filial_Perfil, Idf_Funcao, Idf_Cidade,
                                        Dta_Cadastro, Eml_Usuario_Parceiro, Num_DDD_Celular, Num_Celular, Nme_Usuario) VALUES  ( @Idf_Tipo_Usuario_Parceiro, @Idf_Usuario_Filial_Perfil,
                                        @Idf_Funcao, @Idf_Cidade , @Dta_Cadastro, @Eml_Usuario_Parceiro,  @Num_DDD_Celular,  @Num_Celular, @Nme_Usuario); SELECT SCOPE_IDENTITY()";



        private const string GRT_SEM_ENVIO = @"SELECT UP.Idf_Usuario_Parceiro, TUP.Idf_Tipo_Usuario_Parceiro, TUP.Des_Tipo_Usuario_Parceiro, UP.Idf_Usuario_Filial_Perfil, FUN.Idf_Funcao, FUN.Des_Funcao,
                                               CID.Idf_Cidade, CID.Nme_Cidade, CID.Sig_Estado, UP.Dta_Cadastro, UP.Eml_Usuario_Parceiro, UP.Num_DDD_Celular, UP.Num_Celular, UP.Nme_Usuario, UP.Dta_Ultimo_Envio_SMS
                                               FROM BNE.BNE_Usuario_Parceiro UP WITH (NOLOCK)
                                               INNER JOIN BNE.BNE_Tipo_Usuario_Parceiro TUP WITH (NOLOCK) ON UP.Idf_Tipo_Usuario_Parceiro = TUP.Idf_Tipo_Usuario_Parceiro
                                               INNER JOIN plataforma.TAB_Funcao FUN WITH (NOLOCK) ON FUN.Idf_Funcao = UP.Idf_Funcao
                                               INNER JOIN plataforma.TAB_Cidade CID WITH (NOLOCK) ON CID.Idf_Cidade = UP.Idf_Cidade
                                               WHERE  UP.Idf_Tipo_Usuario_Parceiro = 1 AND  UP.Dta_Ultimo_Envio_SMS IS NULL;";



        private const string PAG_SEM_ENVIO = @"SELECT UP.Idf_Usuario_Parceiro, TUP.Idf_Tipo_Usuario_Parceiro, TUP.Des_Tipo_Usuario_Parceiro, UP.Idf_Usuario_Filial_Perfil, FUN.Idf_Funcao, FUN.Des_Funcao,
                                               CID.Idf_Cidade, CID.Nme_Cidade, CID.Sig_Estado, UP.Dta_Cadastro, UP.Eml_Usuario_Parceiro, UP.Num_DDD_Celular, UP.Num_Celular, UP.Nme_Usuario, UP.Dta_Ultimo_Envio_SMS
                                               FROM BNE.BNE_Usuario_Parceiro UP WITH (NOLOCK) 
                                               INNER JOIN BNE.BNE_Tipo_Usuario_Parceiro TUP WITH (NOLOCK) ON UP.Idf_Tipo_Usuario_Parceiro = TUP.Idf_Tipo_Usuario_Parceiro
                                               INNER JOIN plataforma.TAB_Funcao FUN WITH (NOLOCK) ON FUN.Idf_Funcao = UP.Idf_Funcao
                                               INNER JOIN plataforma.TAB_Cidade CID WITH (NOLOCK) ON CID.Idf_Cidade = UP.Idf_Cidade
                                               WHERE  UP.Idf_Tipo_Usuario_Parceiro = 2 AND UP.Dta_Cadastro > DATEADD(DAY, -30, CAST(GETDATE() AS DATE)) AND UP.Dta_Ultimo_Envio_SMS IS NULL;";


        private const string PAG_COM_ENVIO = @"SELECT DISTINCT UP.Idf_Usuario_Parceiro, TUP.Idf_Tipo_Usuario_Parceiro, TUP.Des_Tipo_Usuario_Parceiro, UP.Idf_Usuario_Filial_Perfil, FUN.Idf_Funcao, FUN.Des_Funcao,
                                              CID.Idf_Cidade, CID.Nme_Cidade, CID.Sig_Estado, UP.Dta_Cadastro, UP.Eml_Usuario_Parceiro, UP.Num_DDD_Celular, UP.Num_Celular, UP.Nme_Usuario, UP.Dta_Ultimo_Envio_SMS
                                              FROM BNE.BNE_Usuario_Parceiro UP WITH (NOLOCK)
                                              INNER JOIN BNE.BNE_Tipo_Usuario_Parceiro TUP WITH (NOLOCK) ON UP.Idf_Tipo_Usuario_Parceiro = TUP.Idf_Tipo_Usuario_Parceiro
                                              INNER JOIN plataforma.TAB_Funcao FUN WITH (NOLOCK) ON FUN.Idf_Funcao = UP.Idf_Funcao
                                              INNER JOIN plataforma.TAB_Cidade CID WITH (NOLOCK) ON CID.Idf_Cidade = UP.Idf_Cidade
                                              WHERE  UP.Idf_Tipo_Usuario_Parceiro = 2 AND UP.Dta_Cadastro > DATEADD(DAY, -30, CAST(GETDATE() AS DATE)) AND Dta_Ultimo_Envio_SMS IS NOT NULL";


        public int Id { get; set; }
        public TipoUsuarioParceiro TipoUsuarioParceiro { get; set; }
        public int IdUsuarioFilialPerfil { get; set; }
        public Funcao Funcao { get; set; }
        public Cidade Cidade { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Email { get; set; }
        public string NumDDDCelular { get; set; }
        public string NumCelular { get; set; }
        public string NmeUsuario { get; set; }
        public DateTime? DataUltimoEnvioSMS { get; set; }


        public static List<UsuarioParceiro> RecuperarGratisSemEnvio() 
        {
            return RecuperarItems(GRT_SEM_ENVIO);
        }

        public static List<UsuarioParceiro> RecuperarPagoSemEnvio()
        {
            return RecuperarItems(PAG_SEM_ENVIO);
        }

        public static List<UsuarioParceiro> RecuperarPagoComEnvio()
        {
            return RecuperarItems(PAG_COM_ENVIO);
        }

        private static List<UsuarioParceiro> RecuperarItems(string Query)
        {
            List<UsuarioParceiro> usuarios = new List<UsuarioParceiro>();
            try
            {
                var parms = new List<SqlParameter>();
                var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Query , parms);
                while (dr.Read())
                {
                    usuarios.Add(buildObject(dr));
                }
            }
            catch (Exception ex) { GerenciadorException.GravarExcecao(ex); }
            return usuarios;
        }


        private static  UsuarioParceiro buildObject(IDataReader dr) 
        {
            return new UsuarioParceiro()
            {
                Id = dr.GetInt32(dr.GetOrdinal("Idf_Usuario_Parceiro")),
                TipoUsuarioParceiro = new TipoUsuarioParceiro() { Id = (int)dr.GetByte(dr.GetOrdinal("Idf_Tipo_Usuario_Parceiro")), Descricao = dr.GetString(dr.GetOrdinal("Des_Tipo_Usuario_Parceiro")) },
                IdUsuarioFilialPerfil = dr.GetInt32(dr.GetOrdinal("Idf_Usuario_Filial_Perfil")),
                Funcao = new Funcao() { IdFuncao = dr.GetInt32(dr.GetOrdinal("Idf_Funcao")), DescricaoFuncao = dr.GetString(dr.GetOrdinal("Des_Funcao")) },
                Cidade = new Cidade() { IdCidade = dr.GetInt32(dr.GetOrdinal("Idf_Cidade")), NomeCidade = dr.GetString(dr.GetOrdinal("Nme_Cidade")), Estado = new Estado(dr.GetString(dr.GetOrdinal("Sig_Estado"))) },
                DataCadastro = dr.GetDateTime(dr.GetOrdinal("Dta_Cadastro")),
                Email =  (!dr.IsDBNull(dr.GetOrdinal("Eml_Usuario_Parceiro"))) ? dr.GetString(dr.GetOrdinal("Eml_Usuario_Parceiro")) : "",
                NumDDDCelular = (!dr.IsDBNull(dr.GetOrdinal("Num_DDD_Celular"))) ?  dr.GetString(dr.GetOrdinal("Num_DDD_Celular")) : "",
                NumCelular =   (!dr.IsDBNull(dr.GetOrdinal("Num_Celular"))) ? dr.GetString(dr.GetOrdinal("Num_Celular")) : "",
                NmeUsuario = (!dr.IsDBNull(dr.GetOrdinal("Nme_Usuario"))) ?  dr.GetString(dr.GetOrdinal("Nme_Usuario")) : "",
                DataUltimoEnvioSMS = (!dr.IsDBNull(dr.GetOrdinal("Dta_Ultimo_Envio_SMS"))) ? dr.GetDateTime(dr.GetOrdinal("Dta_Ultimo_Envio_SMS")) : (DateTime?)null
            };
        }


        public bool Save()
        {
            try
            {
                var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "Idf_Tipo_Usuario_Parceiro", SqlDbType = SqlDbType.TinyInt, Value = this.TipoUsuarioParceiro.Id },
                    new SqlParameter { ParameterName = "Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Value = this.IdUsuarioFilialPerfil },
                    new SqlParameter { ParameterName = "Idf_Funcao", SqlDbType = SqlDbType.Int, Value = this.Funcao.IdFuncao },
                    new SqlParameter { ParameterName = "Idf_Cidade", SqlDbType = SqlDbType.Int, Value = this.Cidade.IdCidade },
                    new SqlParameter { ParameterName = "Eml_Usuario_Parceiro", SqlDbType = SqlDbType.VarChar, IsNullable = true, Value = (this.Email != null) ? this.Email : (Object) DBNull.Value },
                    new SqlParameter { ParameterName = "Num_DDD_Celular", SqlDbType = SqlDbType.VarChar, Value = this.NumDDDCelular },
                    new SqlParameter { ParameterName = "Num_Celular", SqlDbType = SqlDbType.VarChar, Value = this.NumCelular },
                    new SqlParameter { ParameterName = "Dta_Cadastro", SqlDbType = SqlDbType.DateTime, Value = this.DataCadastro },
                    new SqlParameter { ParameterName = "Nme_Usuario", SqlDbType = SqlDbType.VarChar , Value = this.NmeUsuario.Capitalize() }
                };
                this.Id =  Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, INSERT, parms));
                return true;
            }
            catch (Exception ex) 
            {
                GerenciadorException.GravarExcecao(ex);
                return false;
            }
        }

        public bool UpdateEnvioSMS() 
        {
            try
            {
                var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "Idf_Usuario_Parceiro", SqlDbType = SqlDbType.Int, Value = this.Id }
                };
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, UPDATE_ENVIO_SMS, parms);
                return true;
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                return false;
            }
        }


        public int  RecuperarUltimaFuncaoId()
        {
            int vaga = 0;
            var parms = new List<SqlParameter> 
            {
                new SqlParameter { ParameterName = "Idf_Funcao", SqlDbType = SqlDbType.Int, Value = this.Funcao.IdFuncao },
                new SqlParameter { ParameterName = "Dta_Abertura", SqlDbType = SqlDbType.DateTime, Value = this.DataUltimoEnvioSMS },
                new SqlParameter { ParameterName = "Idf_Cidade", SqlDbType = SqlDbType.Int, Value = this.Cidade.IdCidade },
            };
           
            var dr =  DataAccessLayer.ExecuteReader(CommandType.Text, FIND_ULT_FUN, parms);

            if (dr.Read()) 
            {
                vaga = dr.GetInt32(0);
            }

            return vaga;
        }


        public void DispararSMS() 
        {
            try
            {
                var celular = string.Format("{0}{1}", this.NumDDDCelular.Trim(), this.NumCelular.Trim());

                var f = this.Funcao.DescricaoFuncao.Trim().Replace(' ', '-');
                var c = this.Cidade.NomeCidade.Trim().Replace(' ', '-');
                var e = this.Cidade.Estado.SiglaEstado;

                var url_tail = string.Format(URL_TAIL, f, c, e).NormalizarURL();
                var surl = EncurtadorDeURL.Encurtar(URL_HEADER + url_tail);

                var msgSMS = string.Format("{0}, seu jornal de vagas esta ativo, acesse {1} e confira as vagas na sua região!", this.NmeUsuario.Split(' ')[0].Trim().RemoveDiacritics(), surl);

                EnviarSMS(msgSMS, this.NmeUsuario, celular);
                this.UpdateEnvioSMS();
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }


        private void EnviarSMS(string msg, string nomePessoa, string celular)
        {
            using (BLL.BNETanqueService.AppClient objWsTanque = new BLL.BNETanqueService.AppClient())
            {
                objWsTanque.Open();
                List<BNE.BLL.BNETanqueService.Mensagem> listaSMS = new List<BNE.BLL.BNETanqueService.Mensagem>();

                var mensagem = new BNE.BLL.BNETanqueService.Mensagem();
                mensagem.ci = celular.Trim();
                mensagem.np = nomePessoa.Trim();
                mensagem.nc = Convert.ToDecimal(celular.Trim());
                mensagem.dm = msg;

                listaSMS.Add(mensagem);
                var receberMensagem = new BNE.BLL.BNETanqueService.InReceberMensagem { l = listaSMS.ToArray(), cu = "EnvioSMSParceirosLAN" };
                try
                {
                    var retorno = objWsTanque.ReceberMensagem(receberMensagem);
                    if (retorno.l == null)
                        GerenciadorException.GravarExcecao(new Exception("Problema ao enviar SMS em UsuarioParceiro"));
                    else if (retorno.l.Count() == 0)
                        GerenciadorException.GravarExcecao(new Exception("Problema ao enviar SMS em UsuarioParceiro"));
                }
                catch (Exception ex) { throw ex; }
            }
        }


    }
}