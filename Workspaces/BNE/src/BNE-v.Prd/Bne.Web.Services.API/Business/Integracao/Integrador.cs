using Bne.Web.Services.API.DTO.Integracao;
using BNE.BLL;
using BNE.EL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using BNE.Domain.Events.CrossDomainEvents;
using BNE.Domain.Events.Handler;
using Microsoft.Rest;
using Newtonsoft.Json;
using SINE.WebServices.API.Client;
using SINE.WebServices.API.Client.Models;
using Cidade = BNE.BLL.Cidade;
using Curriculo = BNE.BLL.Curriculo;
using Curso = BNE.BLL.Curso;

namespace Bne.Web.Services.API.Business.Integracao
{
    public class Integrador
    {
        public void ExportarCandidato(ExportaCandidatoParam param, out ExportaCandidatoResult result)
        {
            result = new ExportaCandidatoResult();
            #region Verifica se a pessoa já está cadastrada

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    int idPessoaFisica;
                    if (PessoaFisica.ExistePessoaFisica(param.CPF, out idPessoaFisica, trans))
                    {
                        result.ExistePessoaFisica = true;

                        try
                        {
                            var objPessoaFisica = new PessoaFisica(idPessoaFisica);
                            objPessoaFisica.AtualizarDataNascimento(param.DataNascimento, trans);

                            int? idCurriculo = Curriculo.RecuperarIdPorPessoaFisica(objPessoaFisica, trans);

                            if (idCurriculo.HasValue && idCurriculo.Value > 0)
                                result.Id = idCurriculo;

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            BNE.EL.GerenciadorException.GravarExcecao(ex, "Erro ao atualizar dados Pessoa Física. CPF: " + param.CPF);
                        }
                    }

                    #region Se a pessoa não está cadastrada faz um novo cadastro.
                    if (!result.ExistePessoaFisica)
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

                            bool novoCadastro;
                            objCurriculo.SalvarMiniCurriculo(trans,
                                out novoCadastro,
                                objPessoaFisica,
                                listFuncoesPretendidas,
                                objOrigem,
                                null,
                                null,
                                objUsuarioFilialPerfil,
                                objPessoaFisicaComplemento,
                                BNE.BLL.Enumeradores.SituacaoCurriculo.Publicado,
                                objPessoaFisicaFoto,
                                null,
                                false);

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

                            objCurriculo.SalvarDadosPessoais(trans,
                                objPessoaFisica,
                                null,
                                null,
                                null,
                                null,
                                objExp1,
                                objExp2,
                                objExp3,
                                null,
                                null,
                                null,
                                null,
                                null,
                                null,
                                null,
                                BNE.BLL.Enumeradores.SituacaoCurriculo.Publicado);
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



                            // Salva confirmação de e-mail para o QueroVagas
                            if (param.Origem.Equals(BNE.BLL.Enumeradores.Origem.QueroVagas.ToString()))
                            {
                                ParametroCurriculo objParametroCurriculo = null;

                                if (!ParametroCurriculo.CarregarParametroPorCurriculo(
                                    BNE.BLL.Enumeradores.Parametro.ValidacaoEmail, objCurriculo,
                                    out objParametroCurriculo, trans))
                                {
                                    objParametroCurriculo = new ParametroCurriculo
                                    {
                                        Curriculo = objCurriculo,
                                        DataAlteracao = null,
                                        DataCadastro = DateTime.Now,
                                        FlagInativo = false,
                                        Parametro = new Parametro((int)BNE.BLL.Enumeradores.Parametro.ValidacaoEmail),
                                        ValorParametro = "True"
                                    };

                                    objParametroCurriculo.Save(trans);
                                }
                            }


                            trans.Commit();


                            /*Inclui uma lista de alertas para o usuário*/
                            CadastrarAlertas(objPessoaFisica, objCurriculo, listFuncoesPretendidas);
                            result.Id = objCurriculo.IdCurriculo;

                            DomainEventsHandler.Handle(new OnAtualizarCurriculo(objCurriculo.IdCurriculo, objCurriculo.PessoaFisica.EmailPessoa));

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
                    else
                        objFuncaoPretendida.QuantidadeExperiencia = 0;

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

                            if (alertasFuncao.Count > 0)
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

        #region AtualizarUsuario
        public bool AtualizarUsuario(AtualizaUsuario cmd)
        {
            PessoaFisica objPessoaFisica;
            if (PessoaFisica.CarregarPorCPF(cmd.CPF, out objPessoaFisica))
            {
                using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            objPessoaFisica.DataNascimento = cmd.DataNascimento;
                            objPessoaFisica.EmailPessoa = cmd.Email;
                            objPessoaFisica.NumeroDDDCelular = cmd.DDDCelular;
                            objPessoaFisica.NumeroCelular = cmd.Celular;
                            if(cmd.Sexo.HasValue && cmd.Sexo.Value > 0)
                                objPessoaFisica.Sexo = new Sexo(cmd.Sexo.Value);

                            objPessoaFisica.DataAlteracao = DateTime.Now;

                            Curriculo objCurriculo;
                            if (Curriculo.CarregarPorPessoaFisica(trans, objPessoaFisica.IdPessoaFisica, out objCurriculo))
                            {
                                decimal? salarioMinimo = decimal.Parse(Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.SalarioMinimoNacional));

                                //Atualiza se for maior que p mínimo
                                if (salarioMinimo.HasValue && cmd.Salario >= salarioMinimo.Value)
                                    objCurriculo.ValorPretensaoSalarial = cmd.Salario;

                                var funcoesPretendidas = CarregarListaDeFuncoes(cmd.Funcoes, trans);
                                var funcoesPretendidasCadastradas = FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo, trans);

                                bool funcaoExiste = false;
                                foreach (var funcao in funcoesPretendidas)
                                {
                                    foreach (var funcaoExistente in funcoesPretendidasCadastradas)
                                    {
                                        if (funcao == funcaoExistente)
                                            funcaoExiste = true;
                                    }
                                }

                                if (!funcaoExiste && funcoesPretendidasCadastradas.Count == 3)
                                {
                                    //Substituir a mais nova pela do QV
                                    var funcaoPretendida = funcoesPretendidasCadastradas.OrderByDescending(d => d.DataCadastro).First();
                                    var novaFuncaoPretendida = funcoesPretendidasCadastradas.OrderByDescending(d => d.DataCadastro).First();
                                    funcaoPretendida = novaFuncaoPretendida;

                                    funcaoPretendida.Save(trans);
                                }

                                objCurriculo.DataAtualizacao = DateTime.Now;
                                objCurriculo.Save(trans);


                                Cidade objCidade;
                                if (Cidade.CarregarPorNome(cmd.Cidade, out objCidade, trans))
                                {
                                    if (objPessoaFisica.Cidade == null || objPessoaFisica.Cidade.IdCidade != objCidade.IdCidade)
                                    {
                                        objPessoaFisica.Cidade = objCidade;
                                        CurriculoDisponibilidadeCidade objCidadeDisponibilidade;
                                        // Cadastrar disponibilidade com a função antiga
                                        if (!CurriculoDisponibilidadeCidade.CarregarPorCurriculoCidade(objCurriculo, objPessoaFisica.Cidade, out objCidadeDisponibilidade))
                                        {
                                            objCidadeDisponibilidade = new CurriculoDisponibilidadeCidade();
                                            objCidadeDisponibilidade.Cidade = objPessoaFisica.Cidade;
                                            objCidadeDisponibilidade.Curriculo = objCurriculo;
                                            objCidadeDisponibilidade.FlagInativo = false;
                                            objCidadeDisponibilidade.Save(trans);
                                        }

                                        // Atualiza a cidade para a atual

                                    }
                                }
                            }

                            objPessoaFisica.Escolaridade = new Escolaridade(cmd.Escolaridade);

                            objPessoaFisica.Save(trans);


                            ParametroCurriculo objParametroCurriculo = null;
                            if (!ParametroCurriculo.CarregarParametroPorCurriculo(
                                BNE.BLL.Enumeradores.Parametro.ValidacaoEmail, objCurriculo,
                                out objParametroCurriculo, trans))
                            {
                                objParametroCurriculo = new ParametroCurriculo
                                {
                                    Curriculo = objCurriculo,
                                    DataAlteracao = null,
                                    DataCadastro = DateTime.Now,
                                    FlagInativo = false,
                                    Parametro = new Parametro((int)BNE.BLL.Enumeradores.Parametro.ValidacaoEmail),
                                    ValorParametro = "True"
                                };

                                objParametroCurriculo.Save(trans);
                            }

                            trans.Commit();

                            DomainEventsHandler.Handle(new OnAtualizarCurriculo(objCurriculo.IdCurriculo, objCurriculo.PessoaFisica.EmailPessoa));

                            return true;

                        }
                        catch (Exception ex)
                        {
                            GerenciadorException.GravarExcecao(ex, "Erro ao Atualizar Curriculo Integração CPF: " + cmd.CPF + " Esc: " + cmd.Escolaridade + " DN: " + cmd.DataNascimento + " PS: " + cmd.Salario + " CEL: "+ cmd.DDDCelular + cmd.Celular);
                            trans.Rollback();
                            return false;
                        }

                    }

                }
            }

            return false;
        }
        #endregion

        #region ExportarCurriculoSINE
        /// <summary>
        /// Retorna o id do usuário criado no SINE
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns>Id</returns>
        internal static int ExportarCurriculoSINE(decimal cpf)
        {
            MiniCurriculoCandidato param = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        Curriculo objCurriculo;
                        if (!Curriculo.CarregarPorCpf(cpf, out objCurriculo, trans))
                        {
                            throw new Exception("Nenhum cadastro encontrado com o  CPF: " + cpf);
                        }

                        objCurriculo.PessoaFisica.CompleteObject(trans);
                        double? pretencao = null;
                        if (objCurriculo.ValorPretensaoSalarial.HasValue)
                            pretencao = double.Parse(objCurriculo.ValorPretensaoSalarial.Value.ToString());


                        param = new MiniCurriculoCandidato
                        {
                            Nome = objCurriculo.PessoaFisica.NomeCompleto,
                            Email = objCurriculo.PessoaFisica.EmailPessoa,
                            Sexo = objCurriculo.PessoaFisica.Sexo.IdSexo,
                            Salario = pretencao,
                            DDD = objCurriculo.PessoaFisica.NumeroDDDCelular,
                            Celular = objCurriculo.PessoaFisica.NumeroCelular,
                            Cpf = (double?)objCurriculo.PessoaFisica.CPF,
                            DataNascimento = objCurriculo.PessoaFisica.DataNascimento
                        };


                        /*Função*/
                        try
                        {
                            var funcoesPretendidas = FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo, trans);
                            param.Funcao = funcoesPretendidas[0].Funcao.DescricaoFuncao;
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao recuperar Função pretendida => PF: " +
                                                objCurriculo.PessoaFisica.IdPessoaFisica);
                        }


                        /*Cidade*/
                        try
                        {
                            objCurriculo.PessoaFisica.Endereco.CompleteObject(trans);
                            var objEnderecoPessoaFisica = objCurriculo.PessoaFisica.Endereco;

                            objEnderecoPessoaFisica.Cidade.CompleteObject(trans);
                            param.Cidade = objEnderecoPessoaFisica.Cidade.NomeCidade;
                            param.Estado = objEnderecoPessoaFisica.Cidade.Estado.SiglaEstado;

                        }
                        catch
                        {
                            throw new Exception("Erro ao recuperar Cidade => PF: " +
                                                objCurriculo.PessoaFisica.IdPessoaFisica);
                        }


                        /*Formação*/
                        try
                        {
                            objCurriculo.PessoaFisica.Escolaridade.CompleteObject(trans);
                            param.IdNivelFormacao = objCurriculo.PessoaFisica.Escolaridade.IdEscolaridade;
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Erro ao recuperar Formação => PF: " +
                                                objCurriculo.PessoaFisica.IdPessoaFisica);
                        }

                        trans.Commit();

                    }
                }

                using (var client = new ApiIntegracaoSINE(new Uri(ConfigurationManager.AppSettings.Get("SINE-ApiUri"))))
                {
                    try
                    {
                        var response = client.User.CadastrarUsuario(param);
                        var result = JsonConvert.DeserializeObject<dynamic>(response.ToString());

                        return result.Id;
                    }
                    catch (HttpOperationException e)
                    {
                        if (e.Response.Content.Contains("Usuário já existe no sistema!"))
                            throw new Exception("Usuário já cadastrado no SINE => CPF: " + cpf);

                        throw new Exception("Erro ao integrar SINE => CPF: " + cpf);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Erro ao integrar SINE => CPF: " + cpf);
                    }
                }
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                throw;
            }

        }
        #endregion
    }
}