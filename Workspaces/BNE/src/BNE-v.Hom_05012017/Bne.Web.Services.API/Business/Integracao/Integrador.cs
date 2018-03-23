using Bne.Web.Services.API.DTO.Integracao;
using BNE.BLL;
using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Bne.Web.Services.API.Business.Integracao
{
    public class Integrador
    {
        public void ExportarCandidato(ExportaCandidatoParam param)
        {
            #region Verifica se a pessoa já está cadastrada
            bool existePessoa = false;

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    int idPessoaFisica;
                    if (PessoaFisica.ExistePessoaFisica(param.CPF, out idPessoaFisica, trans))
                    {
                        existePessoa = true;

                        try
                        {
                            var objPessoaFisica = new PessoaFisica(idPessoaFisica);
                            objPessoaFisica.AtualizarDataNascimento(param.DataNascimento, trans);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            BNE.EL.GerenciadorException.GravarExcecao(ex, "Erro ao atualizar dados Pessoa Física. CPF: " + param.CPF);
                        }
                    }

                    #region Se a pessoa não está cadastrada faz um novo cadastro.
                    if (!existePessoa)
                    {

                        try
                        {
                            var objPessoaFisica = new PessoaFisica();
                            var objCurriculo = new Curriculo();
                            objPessoaFisica.Endereco = new Endereco();
                            var objPessoaFisicaComplemento = new PessoaFisicaComplemento();
                            var objPessoaFisicaFoto = new PessoaFisicaFoto();
                            var objUsuarioFilialPerfil = new UsuarioFilialPerfil { Perfil = new Perfil((int)BNE.BLL.Enumeradores.Perfil.AcessoNaoVIP) };


                            var listFuncoesPretendidas = CarregarListaDeFuncoes(param.Funcoes, trans);

                            if (listFuncoesPretendidas.Count == 0)
                                throw new Exception("Nenhuma função preetendida foi informada. A operação será cancelada.");


                            objPessoaFisica.CPF = param.CPF;
                            objPessoaFisica.DataNascimento = param.DataNascimento;
                            objPessoaFisica.NomePessoa = param.Nome;
                            objPessoaFisica.NomePessoaPesquisa = RemoverAcentos(param.Nome);

                            try
                            {
                                objPessoaFisica.Escolaridade = Escolaridade.LoadObject(param.Escolaridade, trans);
                            }
                            catch (RecordNotFoundException)
                            {
                                throw new Exception("A escolaridade informada é inválida. A operação será cancelada.");
                            }


                            if (param.Sexo != null)
                            {
                                if (param.Sexo.Value < 0 || param.Sexo.Value > 2)
                                    throw new Exception("Sexo informado é inválido. A operação será cancelada.");
                                else
                                    objPessoaFisica.Sexo = new Sexo(param.Sexo.Value);
                            }

                            objPessoaFisica.NumeroDDDCelular = param.CelularDDD;
                            objPessoaFisica.NumeroCelular = param.Celular;
                            objPessoaFisica.FlagInativo = false;
                            objPessoaFisica.DescricaoIP = objCurriculo.DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                            objPessoaFisica.EmailPessoa = param.Email;

                            Cidade objCidade;
                            if (!Cidade.CarregarPorNome(param.Cidade, out objCidade, trans))
                                throw new Exception("A cidade informada é inválida. A operação será cancelada.");

                            objPessoaFisica.Endereco.Cidade = objCidade;
                            objPessoaFisica.Endereco.DescricaoBairro = param.Bairro;

                            //De acordo com o bug 11720
                            objPessoaFisica.Cidade = objCidade;

                            //Currículo 
                            objCurriculo.ValorPretensaoSalarial = param.PretensaoSalarial;

                            Origem objOrigem = new Origem();
                            if (!objOrigem.RecuperarPorDescricao(param.Origem, trans))
                                throw new Exception("Não foi possível recuperar a origem informada. A operação será cancelada.");


                            objCurriculo.SalvarMiniCurriculo(trans, objPessoaFisica, listFuncoesPretendidas, objOrigem, null, objUsuarioFilialPerfil, objPessoaFisicaComplemento, BNE.BLL.Enumeradores.SituacaoCurriculo.Publicado, objPessoaFisicaFoto, null, false);

                            #region Experiencia Profissional
                            List<ExperienciaProfissional> ListaExps = new List<ExperienciaProfissional>();
                            foreach (var experiencia in param.Experiencias)
                            {
                                ExperienciaProfissional objExperienciaProfissional;
                                if (!String.IsNullOrEmpty(experiencia.Empresa))
                                {
                                    objExperienciaProfissional = new ExperienciaProfissional();

                                    objExperienciaProfissional.RazaoSocial = experiencia.Empresa;
                                    objExperienciaProfissional.AreaBNE = new AreaBNE(experiencia.AreaBNE);


                                    if (experiencia.DataAdmissao.HasValue)
                                        objExperienciaProfissional.DataAdmissao = experiencia.DataAdmissao.Value;
                                    else
                                        break;

                                    if (experiencia.DataDemissao.HasValue)
                                        objExperienciaProfissional.DataDemissao = experiencia.DataDemissao.Value;

                                    if (objExperienciaProfissional.DataAdmissao != null && objExperienciaProfissional.DataDemissao != null)
                                    {
                                        if (objExperienciaProfissional.DataAdmissao >= objExperienciaProfissional.DataDemissao)
                                        {
                                            DateTime bucket = objExperienciaProfissional.DataAdmissao;
                                            objExperienciaProfissional.DataAdmissao = objExperienciaProfissional.DataDemissao.Value;
                                            objExperienciaProfissional.DataDemissao = bucket;
                                        }
                                    }

                                    Funcao objFuncao;
                                    if (Funcao.CarregarPorDescricao(experiencia.Funcao, out objFuncao, trans))
                                    {
                                        objExperienciaProfissional.Funcao = objFuncao;
                                        objExperienciaProfissional.DescricaoFuncaoExercida = String.Empty;
                                    }
                                    else
                                    {
                                        objExperienciaProfissional.Funcao = null;
                                        objExperienciaProfissional.DescricaoFuncaoExercida = experiencia.Funcao;
                                    }

                                    objExperienciaProfissional.DescricaoAtividade = "";
                                }
                                else
                                {
                                    objExperienciaProfissional = null;
                                }

                                ListaExps.Add(objExperienciaProfissional);
                            }

                            ExperienciaProfissional objExp1 = null;
                            ExperienciaProfissional objExp2 = null;
                            ExperienciaProfissional objExp3 = null;

                            if (ListaExps.ElementAtOrDefault(0) != null)
                                objExp1 = ListaExps[0];
                            if (ListaExps.ElementAtOrDefault(1) != null)
                                objExp2 = ListaExps[1];
                            if (ListaExps.ElementAtOrDefault(2) != null)
                                objExp3 = ListaExps[2];

                            objCurriculo.SalvarDadosPessoais(trans, objPessoaFisica, null, null, null, null, objExp1, objExp2, objExp3, null, null, null, null, null, null, null, BNE.BLL.Enumeradores.SituacaoCurriculo.Publicado);
                            #endregion

                            #region Escolaridades
                            foreach (var escolaridade in param.Formacoes)
                            {
                                try
                                {

                               
                                Formacao objFormacao = new Formacao();
                                objFormacao.PessoaFisica = objPessoaFisica;
                                objFormacao.Escolaridade = new BNE.BLL.Escolaridade(escolaridade.CodigoEscolaridade);

                                if (!String.IsNullOrEmpty(escolaridade.NomeInstituicao))
                                {
                                    Fonte objFonte;
                                    if (Fonte.CarregarPorNome(escolaridade.NomeInstituicao, out objFonte, trans))
                                    {
                                        objFormacao.Fonte = objFonte;
                                        objFormacao.DescricaoFonte = String.Empty;
                                    }
                                    else
                                    {
                                        objFormacao.Fonte = null;
                                        objFormacao.DescricaoFonte = escolaridade.NomeInstituicao;
                                    }
                                }
                                else
                                    objFormacao.Fonte = null;

                                //Quando não existe curso na formação define-a como null
                                int[] semCurso = { 4, 5, 6, 7 };
                                if (semCurso.Contains(escolaridade.CodigoEscolaridade))
                                {
                                    objFormacao.Curso = null;
                                    objFormacao.DescricaoCurso = String.Empty;
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(escolaridade.TituloCurso))
                                    {
                                        Curso objCurso;
                                        if (Curso.CarregarPorNome(escolaridade.TituloCurso, out objCurso, trans))
                                        {
                                            objFormacao.Curso = objCurso;
                                            objFormacao.DescricaoCurso = String.Empty;
                                        }
                                        else
                                        {
                                            objFormacao.Curso = null;
                                            objFormacao.DescricaoCurso = escolaridade.TituloCurso;
                                        }
                                    }
                                    else
                                        objFormacao.Curso = null;
                                }


                                //Lista de formações incompletas.
                                int[] incompletas = { 4, 6, 8, 10, 11 };

                                //Caso a formação seja incompleta o campo de ano de conclusão deve receber o valor NULO
                                //caso contrário busca o valor informado no campo.
                                if (incompletas.Contains(escolaridade.CodigoEscolaridade))
                                {
                                    objFormacao.AnoConclusao = null;
                                }
                                else
                                {
                                    if (escolaridade.Conclusao.HasValue)
                                        objFormacao.AnoConclusao = escolaridade.Conclusao;
                                    else
                                        objFormacao.AnoConclusao = null;
                                }


                                Cidade CidadeFormacao = null;
                                if (Cidade.CarregarPorNome(escolaridade.Cidade, out objCidade, trans))
                                    objFormacao.Cidade = CidadeFormacao;
                                else
                                    objFormacao.Cidade = null;

                                objFormacao.Save(trans);

                                }
                                catch (Exception ex)
                                {
                                    BNE.EL.GerenciadorException.GravarExcecao(ex, "Erro ao Incluir Formação Usuário SINE -> " + param.CPF);
                                }
                            }
                            #endregion

                            #region Publicação Currículo
                            var enfileiraPublicacao = Convert.ToBoolean(BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.EnfileiraPublicacaoAutomaticaCurriculo, trans));
                            if (enfileiraPublicacao)
                            {
                                var parametros = new BNE.Services.Base.ProcessosAssincronos.ParametroExecucaoCollection
                                    {
                                        {"idCurriculo", "Curriculo", objCurriculo.IdCurriculo.ToString(CultureInfo.InvariantCulture), objCurriculo.IdCurriculo.ToString(CultureInfo.InvariantCulture)}
                                    };

                                BNE.Services.Base.ProcessosAssincronos.ProcessoAssincrono.IniciarAtividade(BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.PublicacaoCurriculo, parametros);
                            }
                            #endregion



                            trans.Commit();


                            /*Inclui uma lista de alertas para o usuário*/
                            CadastrarAlertas(objPessoaFisica, objCurriculo, listFuncoesPretendidas);
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            BNE.EL.GerenciadorException.GravarExcecao(ex, "Erro ao Exportar Usuário SINE -> BNE. CPF: " + param.CPF);
                        }
                    }
                    #endregion
                }

                conn.Close();
                conn.Dispose();
            }
            #endregion

        }

        #region CarregarListaDeFuncoes
        private List<FuncaoPretendida> CarregarListaDeFuncoes(List<ExportaCandidatoFuncoesParam> funcoes, SqlTransaction trans = null)
        {
            List<FuncaoPretendida> listFuncoesPretendidas = new List<FuncaoPretendida>();
            foreach (var f in funcoes)
            {
                if (!String.IsNullOrEmpty(f.Funcao))
                {
                    var objFuncaoPretendida = new FuncaoPretendida();
                    if (f.Experiencia.HasValue)
                        objFuncaoPretendida.QuantidadeExperiencia = f.Experiencia.Value;

                    Funcao objFuncao;
                    FuncaoErroSinonimo objFuncaoErroSinonimo;

                    if (Funcao.CarregarFuncaoBNEPorDescricaoSINE(f.Funcao, out objFuncao, trans))
                    {
                        objFuncaoPretendida.Funcao = objFuncao;
                        objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                        listFuncoesPretendidas.Add(objFuncaoPretendida);
                    }
                    else if (FuncaoErroSinonimo.CarregarPorDescricao(f.Funcao, out objFuncaoErroSinonimo, trans))
                    {
                        objFuncaoPretendida.Funcao = objFuncaoErroSinonimo.Funcao;
                        objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                        listFuncoesPretendidas.Add(objFuncaoPretendida);
                    }
                }
            }
            return listFuncoesPretendidas;
        }
        #endregion

        #region RemoverAcentos
        private string RemoverAcentos(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        #endregion

        #region CadastrarAlertas
        protected void CadastrarAlertas(PessoaFisica objPessoaFisica, Curriculo objCurriculo, List<FuncaoPretendida> funcoesPretendidas)
        {

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        #region Alerta Curriculo
                        var alertaCurriculo = new BNE.BLL.Notificacao.AlertaCurriculos();
                        try
                        {
                            alertaCurriculo.EmailPessoa = objPessoaFisica.EmailPessoa;
                            alertaCurriculo.FlagVIP = false;
                            alertaCurriculo.IdCurriculo = objCurriculo.IdCurriculo;
                            alertaCurriculo.NomePessoa = objPessoaFisica.NomeCompleto;
                            alertaCurriculo.ValorPretensaoSalarial = objCurriculo.ValorPretensaoSalarial;

                            alertaCurriculo.Save(trans);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Erro ao incluir alerta para o usuário Integração SINE -> BNE");
                        }

                        #endregion

                        #region Alerta Cidade
                        try
                        {
                            /*Recupera todas as cidades da região pmetropolitana para o alerta*/
                            var cidades = Cidade.RecuperarCidadesRegiaoMetropolitana(objPessoaFisica.Cidade.IdCidade);

                            foreach (var cidade in cidades)
                            {
                                try
                                {
                                    var alertaCidade = new BNE.BLL.Notificacao.AlertaCidades { IdCidade = cidade.IdCidade, FlagInativo = false, NomeCidade = cidade.NomeCidade, SiglaEstado = cidade.Estado.SiglaEstado, AlertaCurriculos = alertaCurriculo };
                                    alertaCidade.Save(trans);

                                }
                                catch 
                                {
                                  
                                }
                            }

                        }
                        catch (Exception)
                        {
                            throw new Exception("Erro ao salvar alertas por cidade Usuário SINE");
                        }

                        #endregion

                        #region Alerta Funcao
                        try
                        {

                            var alertasFuncao = new List<BNE.BLL.Notificacao.AlertaFuncoes>();

                            if (funcoesPretendidas.Count > 0)
                            {
                                foreach (var funcao in funcoesPretendidas)
                                {
                                    try
                                    {
                                        if (funcao.Funcao.CompleteObject())
                                        {
                                            var funcoes = Funcao.RecuperarFuncoesSinonimo(funcao.Funcao.IdFuncao);

                                            foreach (var fs in funcoes)
                                            {
                                                try
                                                {
                                                    alertasFuncao.Add(new BNE.BLL.Notificacao.AlertaFuncoes { IdFuncao = fs.IdFuncao, AlertaCurriculos = alertaCurriculo, DescricaoFuncao = fs.DescricaoFuncao, FlagInativo = false, FlagSimilar = false });

                                                }
                                                catch
                                                {

                                                }
                                            }

                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                            }

                            if(alertasFuncao.Count > 0)
                                BNE.BLL.Notificacao.AlertaFuncoes.SalvarListaAlertaFuncoesCurriculo(alertasFuncao, trans);

                        }
                        catch (Exception)
                        {
                            throw new Exception("Erro ao salvar alertas por função Usuário SINE");
                        }

                        #endregion

                        #region Dias da Semana para Alerta
                        try
                        {
                            //salvar dias da semana para receber o jornal
                            var diasDaSemana = new Dictionary<int, bool>();
                            /*Domingo*/
                            diasDaSemana.Add(1, true);
                            /*Segunda*/
                            diasDaSemana.Add(2, true);
                            /*Terça*/
                            diasDaSemana.Add(3, true);
                            /*Quarta*/
                            diasDaSemana.Add(4, true);
                            /*Quinta*/
                            diasDaSemana.Add(5, true);
                            /*Sexta*/
                            diasDaSemana.Add(6, true);
                            /*Sábado*/
                            diasDaSemana.Add(7, true);

                            BNE.BLL.Notificacao.AlertaCurriculosAgenda.SalvarDiasDaSemana(objCurriculo.IdCurriculo, diasDaSemana, trans);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Erro ao salvar dias da emana para Usuário SINE");
                        }

                        #endregion


                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        BNE.EL.GerenciadorException.GravarExcecao(ex, "Erro ao salvar alertas curriculo Usuário SINE -> BNE. CPF: " + objPessoaFisica.NumeroCPF);
                    }

                }

            }

        }
        #endregion

    }
}