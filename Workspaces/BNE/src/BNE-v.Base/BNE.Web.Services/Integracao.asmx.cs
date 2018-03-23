using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Services;
using BNE.BLL;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Web.Services.Protocols;
using security = BNE.BLL.Security;

namespace BNE.Web.Services
{
    /// <summary>
    /// Summary description for Integracao
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Integracao : WebService
    {

        public security.ServiceAuthHeader CustomSoapHeader { get; set; }

        #region WebMethods

        #region Carregar
        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public DateTime? CarregarPessoaFisicaPorCpf(decimal numCpf, out PessoaFisica objPessoaFisica)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            if (!PessoaFisica.CarregarPorCPF(numCpf, out objPessoaFisica))
                return null;
        
            if (objPessoaFisica.Endereco != null)
            {
                objPessoaFisica.Endereco.CompleteObject();
            }
        
            return objPessoaFisica.DataAlteracao;
        }

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public DateTime? CarregarPessoaFisicaComplementoPorCpf(decimal numCpf, out PessoaFisicaComplemento objPessoaFisicaComplemento)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            objPessoaFisicaComplemento = null;

            PessoaFisica objPessoaFisica;
            if (PessoaFisica.CarregarPorCPF(numCpf, out objPessoaFisica))
            {
                if (!PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaComplemento))
                    return null;
            }
            else
                return null;

            objPessoaFisicaComplemento.PessoaFisica = null;

            return objPessoaFisicaComplemento.DataAlteracao;
        }
        
        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public DateTime? CarregarContatoPorCpf(decimal numCpf, out Contato objContato)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            PessoaFisica objPessoaFisica = PessoaFisica.CarregarPorCPF(numCpf);
            
            objContato = null;
            if (objPessoaFisica != null && !Contato.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objContato))
                return null;

            if (objContato != null) 
                return objContato.DataAlteracao;

            return null;
        }

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public DateTime? CarregarCurriculoPorCpf(decimal numCpf, out Curriculo objCurriculo)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            if (!Curriculo.CarregarPorCpf(numCpf, out objCurriculo))
                return null;

            return objCurriculo.DataAtualizacao;
        }

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public string CarregarCurriculoPorCpfUrl(decimal numCpf, out PessoaFisica objPessoaFisica)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion
            Curriculo objCurriculo = new Curriculo();

            if (!PessoaFisica.CarregarPorCPF(numCpf, out objPessoaFisica))
                return null;

            if (!Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                return null;

            
            List<int> lista = new List<int>();
            lista.Add(objCurriculo.IdCurriculo);
            string linkCurriculo = Curriculo.RetornaLinkVisualizacaoCurriculo(lista);
                    int inicio = linkCurriculo.IndexOf('"');
                    int fim = linkCurriculo.LastIndexOf('"');
                    linkCurriculo = linkCurriculo.Substring(inicio + 1, fim-inicio-1);

            return linkCurriculo;
        }


        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public bool CarregarFuncao(string descricaoFuncao, out Funcao objFuncao)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            return Funcao.CarregarPorDescricao(descricaoFuncao, out objFuncao);
        }
        #endregion

        #region Salvar
        
        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public bool SalvarPessoaFisica(PessoaFisica objPessoaFisica, DateTime dtaAlteracao)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            if (objPessoaFisica != null && objPessoaFisica.CPF > 0)
            {
                //Atributo local do BNE obrigatório que não existe no WebFoPag
                objPessoaFisica.NomePessoaPesquisa = objPessoaFisica.NomePessoa;

                PessoaFisica objPessoaFisicaLocal;
                
                if (!PessoaFisica.CarregarPorCPF(objPessoaFisica.CPF, out objPessoaFisicaLocal))
                {   //Inserir Pessoa Física
                    //Adicionando valor para a propriedade, pois na folha não tem essa coluna, então não é mapeado, ficando NULL
                    objPessoaFisica.FlagInativo = false;
                    objPessoaFisica.SalvarIntegracao(dtaAlteracao);
                }
                else if (objPessoaFisicaLocal.DataAlteracao < dtaAlteracao)
                {   //Atualizar Pessoa Física
                    if (objPessoaFisicaLocal.Endereco != null)
                        objPessoaFisicaLocal.Endereco.CompleteObject();

                    MapearPropriedadesObjeto(typeof(PessoaFisica), objPessoaFisica, typeof(PessoaFisica), objPessoaFisicaLocal);

                    //Adicionando valor para a propriedade, pois na folha não tem essa coluna, então não é mapeado, ficando NULL
                    objPessoaFisicaLocal.FlagInativo = false;
                    objPessoaFisicaLocal.SalvarIntegracao(dtaAlteracao);
                } //Pessoa fisica já atualizada no banco de dados
                return true;
            } //Objeto de origem sem dados mínimos para atualização
           
            return false;
        }

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public bool SalvarPessoaFisicaComplemento(decimal numCpf, PessoaFisicaComplemento objPessoaFisicaComplemento, Contato objContato, DateTime dtaAlteracao)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            PessoaFisica objPessoaFisicaLocal;
            if (objPessoaFisicaComplemento != null && PessoaFisica.CarregarPorCPF(numCpf, out objPessoaFisicaLocal))
            {
                PessoaFisicaComplemento objPessoaFisicaComplementoLocal;
                
                if (!PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisicaLocal.IdPessoaFisica, out objPessoaFisicaComplementoLocal))
                {   //Inserir Complemento
                    objPessoaFisicaComplemento.PessoaFisica = objPessoaFisicaLocal;
                    objPessoaFisicaComplemento.SalvarIntegracao(dtaAlteracao);
                    if (objContato != null)
                    {
                        objContato.PessoaFisicaComplemento = objPessoaFisicaComplemento;
                        objContato.InsertIntegracao(dtaAlteracao);
                    }
                    //Inserir veículo
                    if (!PessoaFisicaVeiculo.ExisteVeiculo(objPessoaFisicaComplemento.PessoaFisica.IdPessoaFisica) && objPessoaFisicaComplemento.TipoVeiculo != null)
                    {
                        var objPessoaFisicaVeiculo = new PessoaFisicaVeiculo
                            {
                                PessoaFisica = objPessoaFisicaComplemento.PessoaFisica, 
                                AnoVeiculo = objPessoaFisicaComplemento.AnoVeiculo.HasValue ? objPessoaFisicaComplemento.AnoVeiculo.Value : (short) DateTime.Now.Year, 
                                TipoVeiculo = objPessoaFisicaComplemento.TipoVeiculo, 
                                FlagInativo = false
                            };
                        objPessoaFisicaVeiculo.Save();
                    }
                }
                else if (objPessoaFisicaComplementoLocal.DataAlteracao < dtaAlteracao)
                {   //Atualizar Complemento
                    MapearPropriedadesObjeto(typeof(PessoaFisicaComplemento), objPessoaFisicaComplemento, typeof(PessoaFisicaComplemento), objPessoaFisicaComplementoLocal);
                    objPessoaFisicaComplementoLocal.SalvarIntegracao(dtaAlteracao);
                    //Inserir Contato
                    if (objContato != null)
                    {
                        Contato objContatoLocal;
                        
                        if (!Contato.CarregarPorPessoaFisica(objPessoaFisicaComplementoLocal.PessoaFisica.IdPessoaFisica, out objContatoLocal))
                        {
                            objContato.PessoaFisicaComplemento = objPessoaFisicaComplementoLocal;
                            objContato.InsertIntegracao(dtaAlteracao);
                        }
                    }
                    //Inserir veículo
                    if (!PessoaFisicaVeiculo.ExisteVeiculo(objPessoaFisicaComplementoLocal.PessoaFisica.IdPessoaFisica) && objPessoaFisicaComplementoLocal.TipoVeiculo != null)
                    {
                        var objPessoaFisicaVeiculo = new PessoaFisicaVeiculo
                            {
                                PessoaFisica = objPessoaFisicaComplementoLocal.PessoaFisica, 
                                AnoVeiculo = objPessoaFisicaComplementoLocal.AnoVeiculo.HasValue ? objPessoaFisicaComplementoLocal.AnoVeiculo.Value : (short)DateTime.Now.Year, 
                                TipoVeiculo = objPessoaFisicaComplementoLocal.TipoVeiculo, 
                                FlagInativo = false
                            };
                        objPessoaFisicaVeiculo.Save();
                    }
                } //Complemento já atualizada no banco de dados
                return true;
            } //Objeto de origem sem dados mínimos para atualização
            return false;
        }

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        [Obsolete("Ainda não implementado por problema de modelagem. Assinatura comentada para evitar #warning do CodeAnalysis")]
        public bool SalvarContato()//decimal numCpf, Contato objContato, DateTime dtaAlteracao)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            //PessoaFisica objPessoaFisicaLocal;
            //if (objContato != null
            //    && PessoaFisica.CarregarPorCPF(numCpf, out objPessoaFisicaLocal))
            //{
            //    Contato objContatoLocal;
            //    try
            //    {
            //        if (!Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisicaLocal.IdPessoaFisica, (int)enumTipoContato.RecadoFixo, out objContatoLocal))
            //        {   //Inserir Complemento
            //            objContato.PessoaFisicaComplemento.PessoaFisica = objPessoaFisicaLocal;
            //            objContato.SalvarIntegracao(dtaAlteracao);
            //        }
            //        else if (objContatoLocal.DataAlteracao < dtaAlteracao)
            //        {   //Atualizar Complemento
            //            MapearPropriedadesObjeto(typeof(Contato), objContato, typeof(Contato), objContatoLocal);
            //            objContatoLocal.SalvarIntegracao(dtaAlteracao);
            //        }
            //        else
            //        {//Complemento já atualizada no banco de dados
            //        }
            //        return true;

            //    }
            //    catch (Exception ex)
            //    {
            //        throw (ex);
            //    }
            //}
            //else
            //{   //Objeto de origem sem dados mínimos para atualização
            //}
            return false;
        }

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public bool SalvarExperienciaProfissional(decimal numCpf, ExperienciaProfissional objExperienciaProfissional, DateTime dtaAlteracao)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            PessoaFisica objPessoaFisicaLocal;
            if (objExperienciaProfissional != null && PessoaFisica.CarregarPorCPF(numCpf, out objPessoaFisicaLocal))
            {
                List<ExperienciaProfissional> listExperienciaProfissionalLocal;
                
                Curriculo objCurriculo;
                Curriculo.CarregarPorCpf(numCpf, out objCurriculo);

                if (!ExperienciaProfissional.CarregarExperienciaProfissionalImportada(objPessoaFisicaLocal.IdPessoaFisica, out listExperienciaProfissionalLocal))
                {   //Inserir ExperienciaProfissional
                    objExperienciaProfissional.PessoaFisica = objPessoaFisicaLocal;
                    objExperienciaProfissional.FlagImportado = true;
                    objExperienciaProfissional.Save();

                    if (objCurriculo != null)
                        FuncaoPretendida.SalvarFuncaoPretendidaIntegracao(objExperienciaProfissional, objCurriculo);
                }
                else
                {   //Atualizar ou Inserir uma Experiencia Profissional
                    bool flgEncontrado = false;
                    List<ExperienciaProfissional> listExperienciaProfissionalImportadaLocal = listExperienciaProfissionalLocal.FindAll(EncontrarImportados);
                    foreach (ExperienciaProfissional objExperienciaProfissionalImportadaLocal in listExperienciaProfissionalImportadaLocal)
                    {
                        if (objExperienciaProfissionalImportadaLocal.DataAdmissao == objExperienciaProfissional.DataAdmissao
                            && objExperienciaProfissionalImportadaLocal.RazaoSocial == objExperienciaProfissional.RazaoSocial)
                        {
                            MapearPropriedadesObjeto(typeof(ExperienciaProfissional), objExperienciaProfissional, typeof(ExperienciaProfissional), objExperienciaProfissionalImportadaLocal);
                            objExperienciaProfissionalImportadaLocal.Save();
                            flgEncontrado = true;
                            break;
                        }
                    }
                    if (!flgEncontrado)
                    {
                        objExperienciaProfissional.PessoaFisica = objPessoaFisicaLocal;
                        objExperienciaProfissional.FlagImportado = true;
                        objExperienciaProfissional.Save();

                        if (objCurriculo != null)
                            FuncaoPretendida.SalvarFuncaoPretendidaIntegracao(objExperienciaProfissional, objCurriculo);
                    }
                }

                return true;
            }
            return false;
        }

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public bool LiberarVIP(decimal numCpf)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion
            
            #region Liberar vip
            Curriculo objCurriculo;
            if (Curriculo.CarregarPorCpf(numCpf, out objCurriculo))
            {
                Plano objPlano = Plano.LoadObject(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CodigoPlanoVIPWebfopag)));
                PlanoAdquirido.ConcederPlanoPF(objCurriculo, objPlano);
            }
            #endregion

            return true;
        }

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public bool SalvarCurriculo(decimal numCpf, Curriculo objCurriculo, DateTime dtaAlteracao)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            PessoaFisica objPessoaFisicaLocal;
            if (objCurriculo != null
                && PessoaFisica.CarregarPorCPF(numCpf, out objPessoaFisicaLocal))
            {
                Curriculo objCurriculoLocal;
                
                if (!Curriculo.CarregarPorPessoaFisica(objPessoaFisicaLocal.IdPessoaFisica, out objCurriculoLocal))
                {   //Inserir Complemento
                    objCurriculo.PessoaFisica = objPessoaFisicaLocal;
                    //Se Rescindido deveria ser ComCritica, se Ativo Invisivel
                    objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.ComCritica);
                    objCurriculo.TipoCurriculo = new TipoCurriculo((int)Enumeradores.TipoCurriculo.Mini);
                    objCurriculo.SalvarIntegracao(dtaAlteracao);
                }
                else if((objCurriculoLocal.ValorPretensaoSalarial.HasValue 
                    && objCurriculoLocal.ValorPretensaoSalarial > objCurriculo.ValorPretensaoSalarial))
                {   
                    //Se a pretensão for maior que o último salário mantem a pretensão salarial
                    objCurriculo.ValorPretensaoSalarial = objCurriculoLocal.ValorPretensaoSalarial;

                    MapearPropriedadesObjeto(typeof(Curriculo), objCurriculo, typeof(Curriculo), objCurriculoLocal);
                    objCurriculoLocal.SalvarIntegracao(dtaAlteracao);
                }
                else
                {
                    //Atualizar Curriculo
                    MapearPropriedadesObjeto(typeof(Curriculo), objCurriculo, typeof(Curriculo), objCurriculoLocal);
                    objCurriculoLocal.SalvarIntegracao(dtaAlteracao);
                }

                return true;
            } //Objeto de origem sem dados mínimos para atualização
            return false;
        }

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public bool AlterarSituacaoCurriculo(decimal numCpf, bool statusAtivo, DateTime dtaAlteracao)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            Enumeradores.SituacaoCurriculo eSituacaoCurriculo = (statusAtivo) ? Enumeradores.SituacaoCurriculo.ComCritica : Enumeradores.SituacaoCurriculo.Bloqueado;
            Curriculo objCurriculo;
            if (Curriculo.CarregarPorCpf(numCpf, out objCurriculo))
            {
                objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)eSituacaoCurriculo);
                objCurriculo.FlagInativo = !statusAtivo;
                objCurriculo.DataAtualizacao = dtaAlteracao;
                objCurriculo.Salvar();
                return true;
            }
            return false;
        }

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public bool DesativarCurriculo(decimal numCpf, DateTime dtaAlteracao)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            Curriculo objCurriculo;
            if (Curriculo.CarregarPorCpf(numCpf, out objCurriculo))
            {
                objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.Cancelado);
                objCurriculo.FlagInativo = true;
                objCurriculo.DataAtualizacao = dtaAlteracao;
                objCurriculo.Salvar();
                return true;
            }
            return false;
        }

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public bool BloquearCurriculo(decimal numCpf, DateTime dtaAlteracao)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            Curriculo objCurriculo;
            if (Curriculo.CarregarPorCpf(numCpf, out objCurriculo))
            {
                objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.Bloqueado);
                objCurriculo.FlagInativo = true;
                objCurriculo.DataAtualizacao = dtaAlteracao;
                objCurriculo.Salvar();
                return true;
            }
            return false;
        }

        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public bool AtivarCurriculo(decimal numCpf, DateTime dtaAlteracao)
        {

            #region Segurança - Autorização de acesso
            security.ServiceAuth.AcessoAutorizado(this);
            #endregion

            Curriculo objCurriculo;
            if (Curriculo.CarregarPorCpf(numCpf, out objCurriculo))
            {
                objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoPublicacao);
                objCurriculo.FlagInativo = false;
                objCurriculo.DataAtualizacao = dtaAlteracao;
                objCurriculo.Salvar();
                return true;
            }
            return false;
        }

        #endregion

        #endregion

        #region Métodos Auxiliares

        public bool EncontrarImportados(ExperienciaProfissional objExperienciaProfissional)
        {
            if (objExperienciaProfissional.FlagImportado.HasValue && objExperienciaProfissional.FlagImportado.Value)
                return true;

            return false;
        }

        private static void MapearPropriedadesObjeto(Type objTypeIn, object objIn, Type objTypeOut, object objOut)
        {
            //Recuperar as propriedades do objeto do WS
            PropertyInfo[] arrayPiIn = objTypeIn.GetProperties();
            // Para cada Propriedade
            foreach (PropertyInfo objPiIn in arrayPiIn)
            {
                //Tenta recuperar uma propriedade do objeto local com o mesmo nome da propriedade do objeto do WS
                PropertyInfo objPiOut = objTypeOut.GetProperty(objPiIn.Name);    //propriedade local com o mesmo nome da propriedade do WS

                //Se houver uma propriedade local que possa ser setada
                if (objPiOut != null && objPiOut.CanWrite)
                {
                    //Recupera o valor da propriedade no objeto do WS
                    object objValorIn = objPiIn.GetValue(objIn, null);  //valor da propriedde no objeto do WS
                    object objValorOut = objPiOut.GetValue(objOut, null);  //valor da propriedde no objeto do WS

                    //Se for um tipo de Objeto primitivo e permite ler valor
                    if (objPiIn.CanRead && objPiIn.PropertyType.Namespace == typeof(String).Namespace)
                    {
                        // Se for o mesmo tipo de dado e não for nullable ou for nullable com tipo de dado igual
                        if ((!objPiIn.PropertyType.IsGenericType && !objPiOut.PropertyType.IsGenericType && objPiIn.PropertyType.Name == objPiOut.PropertyType.Name)
                            || (objPiIn.PropertyType.IsGenericType && !objPiOut.PropertyType.IsGenericType && Nullable.GetUnderlyingType(objPiIn.PropertyType).Name == objPiOut.PropertyType.Name && objValorIn != null)
                            || (!objPiIn.PropertyType.IsGenericType && objPiOut.PropertyType.IsGenericType && objPiIn.PropertyType.Name == Nullable.GetUnderlyingType(objPiOut.PropertyType).Name)
                            || (objPiIn.PropertyType.IsGenericType && objPiOut.PropertyType.IsGenericType && Nullable.GetUnderlyingType(objPiIn.PropertyType).Name == Nullable.GetUnderlyingType(objPiOut.PropertyType).Name))
                        {
                            if (objPiOut.CanWrite)
                            {
                                //Seta o valor do objeto do WS no objeto local
                                objPiOut.SetValue(objOut, objValorIn, null);
                            }
                        }
                        else
                        {
                            // Tipos de dados diferentes para a mesma propriedade
                            objPiOut.SetValue(objOut, Activator.CreateInstance(objPiOut.PropertyType), null);
                        }
                    }
                    else if (objValorIn != null)
                    {
                        //Objeto customizado
                        if (objValorOut == null)
                        {
                            objValorOut = Activator.CreateInstance(objPiOut.PropertyType);
                        }
                        MapearPropriedadesObjeto(objPiIn.PropertyType, objValorIn, objPiOut.PropertyType, objValorOut);
                        objPiOut.SetValue(objOut, objValorOut, null);
                    }
                }
            }
        }
        
        #endregion

    }
}
