//-- Data: 04/11/2010 14:03
//-- Autor: Bruno Flammarion

using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class UsuarioFilial : ICloneable // Tabela: BNE_Usuario_Filial
    {

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Consultas

        private const string Spusuarioporidusuariofilialperfil = @"
        SELECT  * 
        FROM    BNE_Usuario_Filial WITH(NOLOCK)
        WHERE   Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil";
        #endregion

        #region CarregarUsuarioFilialPorUsuarioFilialPerfil
        public static bool CarregarUsuarioFilialPorUsuarioFilialPerfil(int idUsuarioFilialPerfil, out UsuarioFilial objUsuarioFilial, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil }
                };

            IDataReader dr;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spusuarioporidusuariofilialperfil, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spusuarioporidusuariofilialperfil, parms);

            try
            {
                objUsuarioFilial = new UsuarioFilial();
                if (SetInstance(dr, objUsuarioFilial))
                    return true;            

                objUsuarioFilial = null;
                return false;
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
        }
        #endregion

        #region SalvarUsuarioFilial
        public bool SalvarUsuarioFilial(PessoaFisica objPessoaFisica, UsuarioFilialPerfil objUsuarioFilialPerfil, Usuario objUsuario)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        objPessoaFisica.Save(trans);
                        objUsuario.Save(trans);
                        objUsuarioFilialPerfil.Save(trans);
                        Save(trans);

                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region LiberarCIA

        public static bool LiberarCIA(PlanoAdquirido objPlanoAdquirido,PlanoParcela objPlanoParcela,out UsuarioFilial objUsuarioFilial ,SqlTransaction trans = null)
        {
            //if (null == objPlanoAdquirido.UsuarioFilialPerfil || null == objPlanoAdquirido.Filial || null == objPlanoAdquirido.Plano)
            //{
                if (null == trans)
                {
                    objPlanoAdquirido.CompleteObject();
                    objPlanoAdquirido.Filial.CompleteObject();
                    objPlanoAdquirido.Plano.CompleteObject();
                }
                else
                {
                    objPlanoAdquirido.CompleteObject(trans);
                    objPlanoAdquirido.Filial.CompleteObject(trans);
                    objPlanoAdquirido.Plano.CompleteObject(trans);
                }
            //}

            if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objPlanoAdquirido.UsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
            {
                if (!String.IsNullOrEmpty(objUsuarioFilial.EmailComercial)) //Só envia mensagem caso o usuário possua e-mail
                {
                    if (objPlanoParcela.NumeroParcela() == 1) 
                    {
                        string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, trans);

                        string assunto;
                        string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ConfirmacaoPagamentoCIA, out assunto);

                        var parametros = new
                        {
                            Nome = objPlanoAdquirido.Filial.RazaoSocial,
                            DescricaoPlano = objPlanoAdquirido.Plano.DescricaoPlano
                        };
                        string mensagem = parametros.ToString(template);

                        EmailSenderFactory
                            .Create(TipoEnviadorEmail.Fila)
                            .Enviar(null, null, objPlanoAdquirido.UsuarioFilialPerfil, assunto, mensagem, emailRemetente, objUsuarioFilial.EmailComercial, trans);
                    }

                    
                }

                return true;
            }

            return false;
        }

        #endregion

    }
}