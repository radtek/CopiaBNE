//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak

using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace BNE.BLL
{
    public partial class SolicitacaoR1 // Tabela: BNE_Solicitacao_R1
    {

        #region Solicitar
        public static bool Solicitar(string nomeSolicitante, string numeroDDDSolicitante, string numeroFoneSolicitante, string emailSolicitante, Funcao objFuncao, Cidade objCidade, string descricaoAtividade, short? idadeDe, short? idadeAte, Escolaridade objEscolaridade, decimal? salarioDe, decimal? salarioAte, short numeroVagas, string descricaoBeneficios, Sexo objSexo, List<Disponibilidade> listaDisponibilidades, CategoriaHabilitacao objCategoriaHabilitacao, string descricaoAdicionais, string descricaoBairro, TipoVinculo objTipoVinculo, UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {

                        #region SolicitacaoR1
                        var objSolicitacaoR1 = new SolicitacaoR1
                            {
                                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                                NomeSolicitante = nomeSolicitante,
                                NumeroDDDSolicitante = numeroDDDSolicitante,
                                NumeroTelefoneSolicitante = numeroFoneSolicitante,
                                EmailSolicitante = emailSolicitante,
                                Funcao = objFuncao,
                                Cidade = objCidade,
                                QuantidadeVagas = numeroVagas,
                                DescricaoAtividade = descricaoAtividade,
                                NumeroIdadeMinima = idadeDe,
                                NumeroIdadeMaxima = idadeAte,
                                Escolaridade = objEscolaridade,
                                ValorSalarioDe = salarioDe,
                                ValorSalarioAte = salarioAte,
                                Sexo = objSexo,
                                DescricaoBeneficio = descricaoBeneficios,
                                CategoriaHabilitacao = objCategoriaHabilitacao,
                                DescricaoInformacaoAdicional = descricaoAdicionais,
                                DescricaoBairro = descricaoBairro,
                                TipoVinculo = objTipoVinculo
                            };

                        objSolicitacaoR1.Save(trans);

                        foreach (var objDisponibilidade in listaDisponibilidades)
                        {
                            var objSolicitacaoR1Disponibilidade = new SolicitacaoR1Disponibilidade
                            {
                                Disponibilidade = objDisponibilidade,
                                SolicitacaoR1 = objSolicitacaoR1
                            };
                            objSolicitacaoR1Disponibilidade.Save(trans);
                        }

                        #endregion

                        #region Mensagem
                        const string campoNaoInformado = "Não informado";

                        if (objUsuarioFilialPerfil != null)
                        {
                            objUsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);

                            if (objUsuarioFilialPerfil.Filial != null)
                            {
                                objUsuarioFilialPerfil.Filial.CompleteObject(trans);

                                UsuarioFilial objUsuarioFilial;
                                if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial, trans))
                                {
                                    numeroDDDSolicitante = objUsuarioFilial.NumeroDDDComercial;
                                    numeroFoneSolicitante = objUsuarioFilial.NumeroComercial;
                                    emailSolicitante = objUsuarioFilial.EmailComercial;
                                }

                                nomeSolicitante = string.Format("{0} ({1} - {2})", objUsuarioFilialPerfil.PessoaFisica.NomeCompleto, objUsuarioFilialPerfil.Filial.RazaoSocial, objUsuarioFilialPerfil.Filial.CNPJ);
                            }
                            else
                            {
                                numeroDDDSolicitante = objUsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular;
                                numeroFoneSolicitante = objUsuarioFilialPerfil.PessoaFisica.NumeroCelular;
                                emailSolicitante = objUsuarioFilialPerfil.PessoaFisica.EmailPessoa;
                                nomeSolicitante = string.Format("{0} ({1})", objUsuarioFilialPerfil.PessoaFisica.NomeCompleto, objUsuarioFilialPerfil.PessoaFisica.NumeroCPF);
                            }
                        }

                        var parametros = new
                        {
                            SolicitanteNome = nomeSolicitante,
                            SolicitanteDDD = numeroDDDSolicitante,
                            SolicitanteFone = numeroFoneSolicitante,
                            SolicitanteEmail = emailSolicitante,
                            VagaFuncao = objFuncao.DescricaoFuncao,
                            VagaCidade = objCidade.NomeCidade,
                            VagaAtividade = descricaoAtividade,
                            VagaIdadeDe = idadeDe.HasValue ? idadeDe.ToString() : string.Empty,
                            VagaIdadeAte = idadeAte.HasValue ? idadeAte.ToString() : string.Empty,
                            VagaEscolaridade = objEscolaridade.DescricaoBNE,
                            VagaSalarioDe = salarioDe,
                            VagaSalarioAte = salarioAte,
                            VagaNumeroVaga = numeroVagas,
                            VagaBeneficio = string.IsNullOrWhiteSpace(descricaoBeneficios) ? campoNaoInformado : descricaoBeneficios,
                            VagaSexo = objSexo == null ? "Indiferente" : objSexo.DescricaoSexo,
                            VagaHorario = (listaDisponibilidades.Count.Equals(0)) ? campoNaoInformado : string.Join(",", listaDisponibilidades.Select(d => d.DescricaoDisponibilidade)),
                            VagaHabilitacao = objCategoriaHabilitacao == null ? campoNaoInformado : objCategoriaHabilitacao.DescricaoCategoriaHabilitacao,
                            VagaInformacoesAdicionais = string.IsNullOrWhiteSpace(descricaoAdicionais) ? campoNaoInformado : descricaoAdicionais,
                            VagaBairro = string.IsNullOrWhiteSpace(descricaoBairro) ? campoNaoInformado : descricaoBairro,
                            VagaTipoVinculo = objTipoVinculo == null ? campoNaoInformado : objTipoVinculo.DescricaoTipoVinculo
                        };

                        string assunto;
                        string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.SolicitacaoR1, out assunto);

                        string mensagem = parametros.ToString(template);

                        string emailDestinatario = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailSolicitacaoR1);
                        string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, emailRemetente, emailDestinatario, trans);
                        #endregion

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

    }
}