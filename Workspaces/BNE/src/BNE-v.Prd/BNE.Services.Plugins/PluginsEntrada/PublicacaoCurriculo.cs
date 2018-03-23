using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using BNE.BLL;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using BNE.Services.Plugins.PluginsEntrada.Publicacao;
using System.Text;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "PublicacaoCurriculo")]
    public class PublicacaoCurriculo : InputPlugin
    {

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var idCurriculo = objParametros["idCurriculo"].ValorInt;
            try
            {
                if (idCurriculo.HasValue)
                {
                    if (!AuditarCurriculo(idCurriculo.Value))
                    {
                        var listaMensagem = new List<MensagemPlugin.MensagemEmail>
                        {
                            new MensagemPlugin.MensagemEmail
                            {
                                Descricao = string.Format("Houve um erro na auditoria do currículo {0}.", idCurriculo),
                                Assunto = "Publicação automática de currículos",
                                To = "gieyson@bne.com.br",
                                From = "ti@bne.com.br"
                            }
                        };
                        return new MensagemPlugin(this, listaMensagem, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Core.LogError(ex);
            }
            return new MensagemPlugin(this, true);
        }
        #endregion

        #region Métodos

        #region PublicarCurriculo
        public static bool AuditarCurriculo(int idCurriculo)
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var encontrouPalavrasProibidas = false;
            var retorno = true;
            StringBuilder PalavrasEncontradas = new StringBuilder();

            var objCurriculo = Curriculo.LoadObject(idCurriculo);
            Contato objContatoTelefone = null;
            Contato objContatoTelefoneCelulcar = null;
            List<ExperienciaProfissional> ListExperiencia = ExperienciaProfissional.CarregarExperienciaPorPessoaFisica(objCurriculo.PessoaFisica.IdPessoaFisica);
            List<Formacao> listFormacao = Formacao.ListarFormacaoList(objCurriculo.PessoaFisica.IdPessoaFisica, true, true);
            List<PessoaFisicaVeiculo> Listveiculo = PessoaFisicaVeiculo.ListarPessoaFisicaVeiculo(objCurriculo.PessoaFisica.IdPessoaFisica);
            
            Contato.CarregarPorPessoaFisicaTipoContato(objCurriculo.PessoaFisica.IdPessoaFisica, (int)BLL.Enumeradores.TipoContato.RecadoFixo, out objContatoTelefone, null);
            Contato.CarregarPorPessoaFisicaTipoContato(objCurriculo.PessoaFisica.IdPessoaFisica, (int)BLL.Enumeradores.TipoContato.RecadoCelular, out objContatoTelefoneCelulcar, null);
            objCurriculo.PessoaFisica.CompleteObject();
            objCurriculo.PessoaFisica.Endereco.CompleteObject();
            PessoaFisicaComplemento objPessoaFisicaComplemento;
            PessoaFisicaComplemento.CarregarPorPessoaFisica(objCurriculo.PessoaFisica.IdPessoaFisica, out objPessoaFisicaComplemento);

            //Recuperando todas as regras de publicação
            var listaTodasRegras = Publicacao.RegraPublicacao.RecuperarRegrasPublicacao(Publicacao.TipoPublicacao.Curriculo);
            if (listaTodasRegras.Count > 0)
            {
                var listaPalavrasProibidas = PalavraProibida.ListarPalavrasProibidas();
                var regexPublicacaoCurriculo = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomatica);
                var regexFormatacaoCurriculoCapitalizacao = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomaticaFormatacaoCapitalizacao);
                var regexFormatacaoCurriculoEspaco = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomaticaFormatacaoEspaco);

                var camposParaAplicarRegra = listaTodasRegras.GroupBy(rp => rp.DescricaoCampo).Select(c => c.First()).ToList();
                foreach (var campo in camposParaAplicarRegra)
                {

                    #region Curriculo

                    var propriedadeCurriculo = objCurriculo.GetType().GetProperty(campo.DescricaoCampo);

                    if (propriedadeCurriculo != null && propriedadeCurriculo.CanWrite)
                    {
                        var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propriedadeCurriculo.Name).ToList();
                        var valorCampo = propriedadeCurriculo.GetValue(objCurriculo, null) ?? string.Empty;

                        string texto = valorCampo.ToString();

                        if (PublicacaoAutomatica.ProcessarTextoCurriculo(ref objCurriculo, ref texto, propriedadeCurriculo.Name, regexPublicacaoCurriculo, regexFormatacaoCurriculoCapitalizacao, regexFormatacaoCurriculoEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas, ref PalavrasEncontradas))
                            propriedadeCurriculo.SetValue(objCurriculo, texto, null);
                        else
                            retorno = false;
                    }

                    #endregion

                    #region [PessoaFisica]
                    var propriedadePessoaFisica = objCurriculo.PessoaFisica.GetType().GetProperty(campo.DescricaoCampo);
                    if (propriedadePessoaFisica != null && propriedadePessoaFisica.CanWrite)
                    {
                        var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propriedadePessoaFisica.Name).ToList();
                        var valorCampo = propriedadePessoaFisica.GetValue(objCurriculo.PessoaFisica, null) ?? string.Empty;

                        string texto = valorCampo.ToString();

                        if (PublicacaoAutomatica.ProcessarTextoCurriculo(ref objCurriculo, ref texto, propriedadePessoaFisica.Name, regexPublicacaoCurriculo, regexFormatacaoCurriculoCapitalizacao, regexFormatacaoCurriculoEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas, ref PalavrasEncontradas))
                            propriedadePessoaFisica.SetValue(objCurriculo.PessoaFisica, texto, null);
                        else
                            retorno = false;
                    }
                    #endregion

                    #region Pessoa Fisica Complemento

                    if (objPessoaFisicaComplemento != null)
                    {
                        var propriedadeFisicaComplemento = objPessoaFisicaComplemento.GetType().GetProperty(campo.DescricaoCampo);

                        if (propriedadeFisicaComplemento != null && propriedadeFisicaComplemento.CanWrite)
                        {
                            var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propriedadeFisicaComplemento.Name).ToList();
                            var valorCampo = propriedadeFisicaComplemento.GetValue(objPessoaFisicaComplemento, null) ?? string.Empty;

                            string texto = valorCampo.ToString();

                            if (PublicacaoAutomatica.ProcessarTextoCurriculo(ref objCurriculo, ref texto, propriedadeFisicaComplemento.Name, regexPublicacaoCurriculo, regexFormatacaoCurriculoCapitalizacao, regexFormatacaoCurriculoEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas, ref PalavrasEncontradas))
                                propriedadeFisicaComplemento.SetValue(objPessoaFisicaComplemento, texto, null);
                            else
                                retorno = false;
                        }
                    }

                    #endregion

                    #region [Contato]
                    if (objContatoTelefone != null)
                    {
                        var propriedadeContato = objContatoTelefone.GetType().GetProperty(campo.DescricaoCampo);

                        if (propriedadeContato != null && propriedadeContato.CanWrite)
                        {
                            var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propriedadeContato.Name).ToList();
                            var valorCampo = propriedadeContato.GetValue(objContatoTelefone, null) ?? string.Empty;

                            string texto = valorCampo.ToString();

                            if (PublicacaoAutomatica.ProcessarTextoCurriculo(ref objCurriculo, ref texto, propriedadeContato.Name, regexPublicacaoCurriculo, regexFormatacaoCurriculoCapitalizacao, regexFormatacaoCurriculoEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas, ref PalavrasEncontradas))
                                propriedadeContato.SetValue(objContatoTelefone, texto, null);
                            else
                                retorno = false;

                            if (encontrouPalavrasProibidas)
                                objContatoTelefone.Save();
                        }


                    }
                    if (objContatoTelefoneCelulcar != null)
                    {
                        var propriedadeContato = objContatoTelefoneCelulcar.GetType().GetProperty(campo.DescricaoCampo);

                        if (propriedadeContato != null && propriedadeContato.CanWrite)
                        {
                            var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propriedadeContato.Name).ToList();
                            var valorCampo = propriedadeContato.GetValue(objContatoTelefoneCelulcar, null) ?? string.Empty;

                            string texto = valorCampo.ToString();

                            if (PublicacaoAutomatica.ProcessarTextoCurriculo(ref objCurriculo, ref texto, propriedadeContato.Name, regexPublicacaoCurriculo, regexFormatacaoCurriculoCapitalizacao, regexFormatacaoCurriculoEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas, ref PalavrasEncontradas))
                                propriedadeContato.SetValue(objContatoTelefoneCelulcar, texto, null);
                            else
                                retorno = false;
                            if (encontrouPalavrasProibidas)
                                objContatoTelefoneCelulcar.Save();
                        }
                    }
                    #endregion

                    #region [Experiencia]

                    foreach (var item in ListExperiencia)
                    {
                        var propriedadeRazaoSocial = item.GetType().GetProperty(campo.DescricaoCampo);

                        if (propriedadeRazaoSocial != null && propriedadeRazaoSocial.CanWrite)
                        {
                            var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propriedadeRazaoSocial.Name).ToList();
                            var valorCampo = propriedadeRazaoSocial.GetValue(item, null) ?? string.Empty;

                            string texto = valorCampo.ToString();

                            if (PublicacaoAutomatica.ProcessarTextoCurriculo(ref objCurriculo, ref texto, propriedadeRazaoSocial.Name, regexPublicacaoCurriculo, regexFormatacaoCurriculoCapitalizacao, regexFormatacaoCurriculoEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas, ref PalavrasEncontradas))
                                propriedadeRazaoSocial.SetValue(item, texto, null);
                            else
                                retorno = false;
                            if (encontrouPalavrasProibidas)
                                item.Save();
                        }
                       
                       
                    }

                    #endregion

                    #region [Formação]
                    foreach (var item in listFormacao)
                    {
                        var propriedadeDescricaoAtividade = item.GetType().GetProperty(campo.DescricaoCampo);

                        if (propriedadeDescricaoAtividade != null && propriedadeDescricaoAtividade.CanWrite)
                        {
                            var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propriedadeDescricaoAtividade.Name).ToList();
                            var valorCampo = propriedadeDescricaoAtividade.GetValue(item, null) ?? string.Empty;

                            string texto = valorCampo.ToString();

                            if (PublicacaoAutomatica.ProcessarTextoCurriculo(ref objCurriculo, ref texto, propriedadeDescricaoAtividade.Name, regexPublicacaoCurriculo, regexFormatacaoCurriculoCapitalizacao, regexFormatacaoCurriculoEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas, ref PalavrasEncontradas))
                                propriedadeDescricaoAtividade.SetValue(item, texto, null);
                            else
                                retorno = false;
                            if (encontrouPalavrasProibidas)
                                item.Save();
                        }
                        
                    }

                    #endregion

                    #region [Veiculo]
                    foreach (var item in Listveiculo)
                    {
                        var propriedadeModeloVeiculo = item.GetType().GetProperty(campo.DescricaoCampo);

                        if (propriedadeModeloVeiculo != null && propriedadeModeloVeiculo.CanWrite)
                        {
                            var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propriedadeModeloVeiculo.Name).ToList();
                            var valorCampo = propriedadeModeloVeiculo.GetValue(item, null) ?? string.Empty;
                            string texto = valorCampo.ToString();

                            if (PublicacaoAutomatica.ProcessarTextoCurriculo(ref objCurriculo, ref texto, propriedadeModeloVeiculo.Name, regexPublicacaoCurriculo, regexFormatacaoCurriculoCapitalizacao, regexFormatacaoCurriculoEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas, ref PalavrasEncontradas))
                                propriedadeModeloVeiculo.SetValue(item, texto, null);
                            else
                                retorno = false;
                            if (encontrouPalavrasProibidas)
                                item.Save();
                        }

                    }
                       
                    #endregion

                    #region [Endereco]
                    var propriedadeEnderecoComplemento = objCurriculo.PessoaFisica.Endereco.GetType().GetProperty(campo.DescricaoCampo);

                    if (propriedadeEnderecoComplemento != null && propriedadeEnderecoComplemento.CanWrite)
                    {
                        var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propriedadeEnderecoComplemento.Name).ToList();
                        var valorCampo = propriedadeEnderecoComplemento.GetValue(objCurriculo.PessoaFisica.Endereco, null) ?? string.Empty;
                        string texto = valorCampo.ToString();

                        if (PublicacaoAutomatica.ProcessarTextoCurriculo(ref objCurriculo, ref texto, propriedadeEnderecoComplemento.Name, regexPublicacaoCurriculo, regexFormatacaoCurriculoCapitalizacao, regexFormatacaoCurriculoEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas, ref PalavrasEncontradas))
                            propriedadeEnderecoComplemento.SetValue(objCurriculo.PessoaFisica.Endereco, texto, null);
                        else
                            retorno = false;
                        if (encontrouPalavrasProibidas)
                            objCurriculo.PessoaFisica.Endereco.Save();

                    }
                    #endregion

                }
            }

            if (encontrouPalavrasProibidas)
            {
                retorno = false;
                objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)BLL.Enumeradores.SituacaoCurriculo.Bloqueado);
                CurriculoCorrecao objCurriculoCorrecao = new CurriculoCorrecao()
                {
                    Curriculo = objCurriculo,
                    UsuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdUsuarioFilialPerfilAuditoriaCurriculo))),
                    DescricaoCorrecao = $"Palavras proibidas no curriculo - {PalavrasEncontradas.ToString()}",
                    FlagCorrigido = false
                };
                objCurriculoCorrecao.Save();
                objCurriculo.PessoaFisica.Endereco.Save();
            }
            else
                objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)BLL.Enumeradores.SituacaoCurriculo.Auditado);

            objCurriculo.Save();

            objCurriculo.PessoaFisica.Save();
            if (objPessoaFisicaComplemento != null)
                objPessoaFisicaComplemento.Save();

            stopWatch.Stop();

            PublicacaoAutomatica.SalvarHistoricoCurriculo(objCurriculo, string.Format("Curriculo auditado pela publicação automática. Tempo de auditoria {0}.", stopWatch.Elapsed));

            return retorno;
        }
        #endregion

        #endregion

    }
}
