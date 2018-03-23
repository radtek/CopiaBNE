//-- Data: 04/11/2010 14:03
//-- Autor: Bruno Flammarion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BNE.BLL.Common;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.DTO;
using BNE.BLL.Mensagem.DTO;

namespace BNE.BLL
{
    public partial class UsuarioFilial : ICloneable // Tabela: BNE_Usuario_Filial
    {
        #region Propriedades

        #region DataCadastro
        /// <summary>
        ///     Campo obrigatório.
        /// </summary>
        public DateTime? DataCadastro
        {
            get { return _dataCadastro; }
            set
            {
                _dataCadastro = value;
                _modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas
        private const string Spusuarioporidusuariofilialperfil = @"
        SELECT  * 
        FROM    BNE_Usuario_Filial WITH(NOLOCK)
        WHERE   Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil";

        private const string Spquantidadeusuarioporemail = @"
        SELECT  COUNT(*) 
        FROM    BNE_Usuario_Filial UF WITH(NOLOCK)
                INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON UF.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
        WHERE   Eml_Comercial LIKE @Eml_Comercial
                AND UFP.Flg_Inativo = 0
                AND PF.Flg_Inativo = 0
        ";
        #endregion

        #region Métodos

        #region Clone
        public object Clone()
        {
            return MemberwiseClone();
        }
        #endregion

        #region CarregarUsuarioFilialPorUsuarioFilialPerfil
        public static bool CarregarUsuarioFilialPorUsuarioFilialPerfil(int idUsuarioFilialPerfil, out UsuarioFilial objUsuarioFilial, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil}
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
        public bool SalvarUsuarioFilial(PessoaFisica objPessoaFisica, UsuarioFilialPerfil objUsuarioFilialPerfil, Usuario objUsuario, UsuarioFilialContato objUsuarioFilialContato, out string erro)
        {
            erro = string.Empty;
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var novoUsuario = !_persisted;

                        objPessoaFisica.Save(trans);
                        objUsuario.Save(trans);
                        objUsuarioFilialPerfil.Save(trans);
                        Save(trans);

                        if (objUsuarioFilialContato != null) //Se é um usuário vindo de um contato, então inativa o contato
                        {
                            objUsuarioFilialContato.Inativar(trans);
                        }

                        if (objUsuarioFilialPerfil.FlagUsuarioResponsavel && UsuarioFilialPerfil.QuantidadeUsuarioResponsavel(objUsuarioFilialPerfil.Filial, trans) > 1)
                        {
                            erro = "Esta empresa já possui um usuário responsável";
                            return false;
                        }

                        trans.Commit();

                        try
                        {
                            //Dispara boas vindas se usuário novo e salvo no banco.
                            if (novoUsuario)
                            {
                                #region EnvioSMS
                                var idUFPRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfUfpEnvioSMSTanqueCadastroEmpresa);

                                var smsUm = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.SMSBoasVindasUm);
                                smsUm = smsUm.Replace("{Nome_Usuario}", objPessoaFisica.PrimeiroNome);

                                var listaUsuarios = new List<DestinatarioSMS>();

                                var objUsuarioEnvioSMS = new DestinatarioSMS
                                {
                                    DDDCelular = objPessoaFisica.NumeroDDDCelular,
                                    NumeroCelular = objPessoaFisica.NumeroCelular,
                                    NomePessoa = objPessoaFisica.NomePessoa,
                                    Mensagem = smsUm,
                                    IdDestinatario = objUsuarioFilialPerfil.IdUsuarioFilialPerfil
                                };

                                listaUsuarios.Add(objUsuarioEnvioSMS);

                                MensagemCS.EnvioSMSTanque(idUFPRemetente, listaUsuarios);
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex, "Erro ao enviar sms");
                        }
                        try
                        {
                            Task.Factory.StartNew(() => CelularSelecionador.HabilitarDesabilitarUsuarios(objUsuarioFilialPerfil.Filial));
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex, "Erro ao HabilitarDesabilitarUsuarios");
                        }

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
        public static bool LiberarCIA(PlanoAdquirido objPlanoAdquirido, PlanoParcela objPlanoParcela, out UsuarioFilial objUsuarioFilial, SqlTransaction trans = null)
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

            if (CarregarUsuarioFilialPorUsuarioFilialPerfil(objPlanoAdquirido.UsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
            {
                if (!string.IsNullOrEmpty(objUsuarioFilial.EmailComercial) && !objPlanoAdquirido.PlanoLiberadoNaoPago) //Só envia mensagem caso o usuário possua e-mail
                {
                    if (objPlanoParcela.NumeroParcela() == 1 && !objPlanoAdquirido.Filial.PossuiSTCLanhouse()) //Não enviar para lanhouse
                    {
                        var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, trans);

                        string assunto;
                        var template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ConfirmacaoPagamentoCIA, out assunto);

                        var objVendedor = objPlanoAdquirido.Filial.Vendedor();

                        var parametros = new
                        {
                            Nome = objPlanoAdquirido.Filial.RazaoSocial,
                            objPlanoAdquirido.Plano.DescricaoPlano,
                            Vendedor = objVendedor != null ? objVendedor.ToMailSignature() : string.Empty
                        };
                        var mensagem = parametros.ToString(template);

                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(null, null, objPlanoAdquirido.UsuarioFilialPerfil, assunto, mensagem, Enumeradores.CartaEmail.ConfirmacaoPagamentoCIA, emailRemetente, objUsuarioFilial.EmailComercial, trans);
                    }
                }

                return true;
            }

            return false;
        }
        #endregion

        #region ExisteUsuarioComEmail
        /// <summary>
        ///     Verifica se existe um usuário com o email informado
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ExisteUsuarioComEmail(string email)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Eml_Comercial", SqlDbType = SqlDbType.VarChar, Size = 100, Value = email }
            };
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spquantidadeusuarioporemail, parms)) > 0;
        }
        #endregion

        #endregion

        #region Mapeamento Novo -> Velho
        public void SalvarMigracao(SqlTransaction trans)
        {
            if (!_persisted)
            {
                InsertMigracao(trans);
                Vaga.MigrarVagaRapida(this, trans);
            }
            else
                UpdateMigracao(trans);
        }

        #region InsertMigracao
        /// <summary>
        ///     Método utilizado para inserir uma instância de Filial no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void InsertMigracao(SqlTransaction trans)
        {
            var parms = GetParameters();
            SetParametersMigracao(parms);
            var cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            _idUsuarioFilial = Convert.ToInt32(cmd.Parameters["@Idf_Usuario_Filial"].Value);
            cmd.Parameters.Clear();
            _persisted = true;
            _modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        ///     Método utilizado para atualizar uma instância de Filial no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void UpdateMigracao(SqlTransaction trans)
        {
            if (_modified)
            {
                var parms = GetParameters();
                SetParametersMigracao(parms);
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
                _modified = false;
            }
        }
        #endregion

        #region SetParametersMigracao
        /// <summary>
        ///     Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void SetParametersMigracao(List<SqlParameter> parms)
        {
            parms[0].Value = _idUsuarioFilial;
            parms[1].Value = _usuarioFilialPerfil.IdUsuarioFilialPerfil;

            if (_funcao != null)
                parms[2].Value = _funcao.IdFuncao;
            else
                parms[2].Value = DBNull.Value;


            if (!string.IsNullOrEmpty(_descricaoFuncao))
                parms[3].Value = _descricaoFuncao;
            else
                parms[3].Value = DBNull.Value;


            if (!string.IsNullOrEmpty(_numeroRamal))
                parms[4].Value = _numeroRamal;
            else
                parms[4].Value = DBNull.Value;


            if (!string.IsNullOrEmpty(_numeroDDDComercial))
                parms[5].Value = _numeroDDDComercial;
            else
                parms[5].Value = DBNull.Value;


            if (!string.IsNullOrEmpty(_numeroComercial))
                parms[6].Value = _numeroComercial;
            else
                parms[6].Value = DBNull.Value;


            if (!string.IsNullOrEmpty(_emailComercial))
                parms[7].Value = _emailComercial;
            else
                parms[7].Value = DBNull.Value;


            if (!_persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[8].Value = _dataCadastro;
        }
        #endregion

        #endregion
    }
}