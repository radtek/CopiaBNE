using LanHouse.Entities.BNE;
using LanHouse.Business.Custom;
using System.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.UI.WebControls;
using System.Drawing;

namespace LanHouse.Business
{
    public class Curriculo
    {

        #region InativarExperiencia
        /// <summary>
        /// Inativar uma Experiencia do Curriculo
        /// </summary>
        /// <param name="idExperiencia"></param>
        /// <returns></returns>
        public static bool InativarExperiencia(int idExperiencia)
        {
            var retorno = false;

            using (var context = new LanEntities())
            {
                var objExperiencia = (from f in context.BNE_Experiencia_Profissional
                                   where f.Idf_Experiencia_Profissional == idExperiencia
                                   select f).FirstOrDefault();

                if (objExperiencia != null)
                {
                    objExperiencia.Flg_Inativo = true;

                    context.SaveChanges();
                    retorno = true;
                }
            }

            return retorno;
        }
        #endregion

        #region InativarFormacao
        /// <summary>
        /// Inativar uma formação do Curriculo
        /// </summary>
        /// <param name="idFormacao"></param>
        /// <returns></returns>
        public static bool InativarFormacao(int idFormacaoCandidato)
        {
            var retorno = false;

            using (var context = new LanEntities())
            {
                var objFormacao = (from f in context.BNE_Formacao
                                 where f.Idf_Formacao == idFormacaoCandidato
                                 select f).FirstOrDefault();

                if (objFormacao != null)
                {
                    objFormacao.Flg_Inativo = true;
                    objFormacao.Dta_Alteracao = DateTime.Now;

                    context.SaveChanges();
                    retorno = true;
                }
            }

            return retorno;
        }
        #endregion

        #region InativarIdioma
        /// <summary>
        /// Inativar um idioma da
        /// </summary>
        /// <param name="idIdioma"></param>
        /// <returns></returns>
        public static bool InativarIdioma(int idIdiomaCandidato)
        {
            var retorno = false;

            using (var context = new LanEntities())
            {
                var objIdioma = (from i in context.TAB_Pessoa_Fisica_Idioma
                                where i.Idf_Pessoa_Fisica_Idioma == idIdiomaCandidato
                                select i).FirstOrDefault();

                if (objIdioma != null)
                {
                    objIdioma.Flg_Inativo = true;

                    context.SaveChanges();
                    retorno = true;
                }
            }

            return retorno;
        }
        #endregion

        #region ProcessarLiberacaoVip
        public static string ProcessarLiberacaoVip(int idCurriculo, string codigo)
        {
            try
            {
                //Carregar Objeto Curriculo
                BNE_Curriculo objCurriculo;
                if (!Business.Curriculo.CarregarPorId(idCurriculo, out objCurriculo))
                    return "Curriculo não encontrado";

                //Carregar Objeto Pessoa Fisica
                TAB_Pessoa_Fisica objPessoa;
                if (!Business.PessoaFisica.CarregarPorCV(idCurriculo, out objPessoa))
                    return "Pessoa não encontrada";

                //Carregar Objeto Usuario Filial Perfil
                TAB_Usuario_Filial_Perfil objUsuarioFilialPerfil;
                if (!Business.UsuarioFilialPerfil.CarregarPorIdPessoa(objPessoa.Idf_Pessoa_Fisica, out objUsuarioFilialPerfil))
                    return "UsuárioFilialPerfil da pessoa física não encontrado";

                //Carregar Código de Desconto
                BNE_Codigo_Desconto objCodigoDesconto;
                if (!Business.CodigoDesconto.CarregarPorCodigo(codigo, out objCodigoDesconto))
                    return "Código de desconto não encontrado";

                //Verificar Código de Desconto
                BNE_Plano objPlano;
                string resposta = Business.CodigoDesconto.ValidarCodigo(objCodigoDesconto, out objPlano);
                if (resposta != string.Empty)
                    return resposta;

                //Conceder desconto
                if (!Business.Pagamento.ConcederDescontoIntegral(objCurriculo, objUsuarioFilialPerfil, objPlano, objCodigoDesconto, out resposta))
                    return resposta;


                return "";
            }
            catch
            {
                return "Erro genérico";
            }
        }
        #endregion

        #region ValidaCandidatura
        public static bool ValidaCandidatura(int idCurriculo)
        {
            try
            {
                int quantidadeDisponivel;
                BNE_Curriculo objCurriculo;

                Curriculo.CarregarPorId(idCurriculo, out objCurriculo);
                quantidadeDisponivel = Candidatura.QuantidadeCandidaturasDisponiveisUsuario(idCurriculo);

                return (objCurriculo.Flg_VIP || quantidadeDisponivel > 0);
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region ValidaEnvioCVEmpresa
        public static bool ValidaEnvioCVEmpresa(int idCurriculo)
        {
            try
            {
                int quantidadeDisponivel;
                BNE_Curriculo objCurriculo;

                Curriculo.CarregarPorId(idCurriculo, out objCurriculo);
                quantidadeDisponivel = EnviarCVEmpresa.QuantidadeEnvioDisponiveisUsuario(idCurriculo);

                return (objCurriculo.Flg_VIP || quantidadeDisponivel > 0);
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region CarregarPorId
        /// <summary>
        /// Carregar um objeto de curriculo a partir do seu ID
        /// </summary>
        public static bool CarregarPorId(int idCurriculo, out BNE_Curriculo objCurriculo)
        {
            using (var entity = new LanEntities())
            {
                objCurriculo = (from cv in entity.BNE_Curriculo
                                where cv.Idf_Curriculo == idCurriculo
                                select cv).FirstOrDefault();

                return objCurriculo != null;
            }
        }
        public static bool CarregarPorId(int idCurriculo, out BNE_Curriculo objCurriculo, LanEntities context)
        {
            objCurriculo = (from cv in context.BNE_Curriculo
                            where cv.Idf_Curriculo == idCurriculo
                            select cv).FirstOrDefault();

            return objCurriculo != null;
        }
        #endregion

        #region CarregarPorIdPessoaFisica
        /// <summary>
        /// Carregar um objeto de curriculo a partir do ID da Pessoa Fisica
        /// </summary>
        public static bool CarregarPorIdPessoaFisica(int idPessoaFisica, out BNE_Curriculo objCurriculo)
        {
            using (var entity = new LanEntities())
            {
                objCurriculo = (from cv in entity.BNE_Curriculo
                                where cv.Idf_Pessoa_Fisica == idPessoaFisica
                                select cv).FirstOrDefault();

                return objCurriculo != null;

            }
        }
        #endregion

        #region CarregarPorIdPessoaFisica
        /// <summary>
        /// Carregar um objeto de curriculo a partir do ID da Pessoa Fisica
        /// </summary>
        public static bool CarregarPorIdPessoaFisica(int idPessoaFisica, out BNE_Curriculo objCurriculo, LanEntities context)
        {
            objCurriculo = (from cv in context.BNE_Curriculo
                            where cv.Idf_Pessoa_Fisica == idPessoaFisica
                            select cv).FirstOrDefault();

            return objCurriculo != null;
        }
        #endregion

        #region LiberarVIP
        public static bool LiberarVIP(BNE_Plano_Adquirido objPlanoAdquirido, out BNE_Curriculo objCurriculo, LanEntities context)
        {
            context.Entry(objPlanoAdquirido).Reference(b => b.TAB_Usuario_Filial_Perfil).Load();
            context.Entry(objPlanoAdquirido.TAB_Usuario_Filial_Perfil).Reference(b => b.TAB_Pessoa_Fisica).Load();


            if (!Curriculo.CarregarPorIdPessoaFisica(objPlanoAdquirido.TAB_Usuario_Filial_Perfil.TAB_Pessoa_Fisica.Idf_Pessoa_Fisica, out objCurriculo, context))
                return false;

            objCurriculo.Flg_VIP = true;
            objCurriculo.Idf_Situacao_Curriculo = (int)Enumeradores.SituacaoCurriculo.AguardandoRevisaoVIP;


            //Atualiza o perfil do candidato
            objPlanoAdquirido.TAB_Usuario_Filial_Perfil.Idf_Perfil = (int)Enumeradores.Perfil.AcessoVIP;
            context.SaveChanges();

            //Envia email
            if (!String.IsNullOrEmpty(objPlanoAdquirido.TAB_Usuario_Filial_Perfil.TAB_Pessoa_Fisica.Eml_Pessoa)) //Só envia mensagem caso o usuário possua e-mail
            {
                string emailRemetente = new Parametro().GetById(Convert.ToInt32(Enumeradores.Parametro.EmailMensagens)).Vlr_Parametro;

                #region Confirmação de Pagamento
                string assunto;
                string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ConfirmacaoPagamentoVIP, out assunto);
                var parametros = new
                {
                    NomeCandidato = objPlanoAdquirido.TAB_Usuario_Filial_Perfil.TAB_Pessoa_Fisica.Nme_Pessoa
                };
                string mensagem = parametros.ToString(template);

                MailController.Send(objPlanoAdquirido.TAB_Usuario_Filial_Perfil.TAB_Pessoa_Fisica.Eml_Pessoa, emailRemetente, assunto, mensagem);
                #endregion

                #region CartilhaVIP
                string assuntoCartilhaVIP;
                string mensagemCartilhaVIP = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.CartilhaVIP, out assuntoCartilhaVIP);

                MailController.Send(objPlanoAdquirido.TAB_Usuario_Filial_Perfil.TAB_Pessoa_Fisica.Eml_Pessoa, emailRemetente, assuntoCartilhaVIP, mensagemCartilhaVIP);
                #endregion
            }

            //Envia sms (TODO:Implementar)
            //if (!string.IsNullOrEmpty(objPlanoAdquirido.TAB_Usuario_Filial_Perfil.TAB_Pessoa_Fisica.Num_DDD_Celular) && !string.IsNullOrEmpty(objPlanoAdquirido.TAB_Usuario_Filial_Perfil.TAB_Pessoa_Fisica.Num_Celular))
            //{
            //    MensagemCS.SalvarSMS(null, null, objPlanoAdquirido.TAB_Usuario_Filial_Perfil, CartaSMS.RecuperaValorConteudo((int)Enumeradores.CartaSMS.BoasVindasVIP, context), objPlanoAdquirido.TAB_Usuario_Filial_Perfil.TAB_Pessoa_Fisica.Num_DDD_Celular, objPlanoAdquirido.TAB_Usuario_Filial_Perfil.TAB_Pessoa_Fisica.Num_Celular, context);
            //}

            return true;
        }

        #endregion

        #region SalvarCV
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurriculoDTO">Objeto CV preenchido no cadastro</param>
        /// <param name="passoCadastro">Passo em que foi solicitado o salvar (0 para o ultimo passo, salvando e obrigando todos os dados)</param>
        /// <returns></returns>
        public static string SalvarCV(DTO.Curriculo CurriculoDTO, int passoCadastro)
        {
            try
            {
                switch (passoCadastro)
                {
                    case 1:
                        return SalvarPasso1(CurriculoDTO);
                    case 2:
                        return SalvarPasso2(CurriculoDTO);
                    case 3:
                        return SalvarPasso3(CurriculoDTO);
                    case 4:
                        return SalvarPasso4(CurriculoDTO);
                    case 0:
                        return SalvarCompleto(CurriculoDTO);
                }

                return string.Empty;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region SalvarPasso1
        /// <summary>
        /// Salvar os dado so passo um. Dados pessoais da pessoa
        /// </summary>
        /// <param name="CurriculoDTO"></param>
        /// <returns></returns>
        private static string SalvarPasso1(DTO.Curriculo CurriculoDTO)
        {
            try
            {
                DTO.Curriculo CurriculoDTOOriginal = CurriculoDTO;

                using (var context = new LanEntities())
                {
                    using (var dbTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //Cria instancia do CV
                            BNE_Curriculo objCurriculo;

                            //Verifica existencia do CV
                            if (CurriculoDTO.idCurriculo != 0)
                            {
                                if (!CarregarPorId(CurriculoDTO.idCurriculo, out objCurriculo, context))
                                    return "Currículo não encontrado.";

                                //Carrega o objeto da pessoa caso o CV exista
                                context.Entry(objCurriculo).Reference(p => p.TAB_Pessoa_Fisica).Load();
                            }
                            else
                            {
                                //Caso seja um cadastro novo, instancia um novo objeto de pessoa e cv
                                objCurriculo = new BNE_Curriculo();
                                objCurriculo.TAB_Pessoa_Fisica = new TAB_Pessoa_Fisica();
                            }
                           
                            //Carrega os dados do CV e Pessoa atualizados
                            CarregarObjetoPasso1(CurriculoDTO, objCurriculo);

                            //Caso seja um novo registro, add o objeto na entidade
                            if (CurriculoDTO.idCurriculo == 0)
                            {
                                context.TAB_Pessoa_Fisica.Add(objCurriculo.TAB_Pessoa_Fisica);
                                context.BNE_Curriculo.Add(objCurriculo);
                            }

                            //Salvar o CV e a Pessoa no BD
                            context.SaveChanges();

                            //Adicionar função pretendida
                            AdicionarFuncaoPretendida(CurriculoDTO, objCurriculo.Idf_Curriculo, context);

                            //Salva os dados e commit dos dados no BD
                            context.SaveChanges();

                            if (CurriculoDTO.idCurriculo == 0)
                            { 
                                //Seta a quantidade de candidaturas para 3 (por ser cadastro novo)
                                CurriculoDTO.qtdCandidatura = 3;

                                //Seta a quantidade de envio de CV para empresas para 3 (por ser cadastro novo)
                                CurriculoDTO.qtdEnvioCVEmpresa = 3;

                                //Cria um objeto na Parametro_Curriculo para Candidatura
                                context.TAB_Parametro_Curriculo.Add(new TAB_Parametro_Curriculo()
                                    {
                                        Idf_Curriculo = objCurriculo.Idf_Curriculo,
                                        Idf_Parametro = Convert.ToInt32(Enumeradores.Parametro.QuantidadeCandidaturaDegustacao),
                                        Vlr_Parametro = "3",
                                        Dta_Cadastro = DateTime.Now,
                                        Flg_Inativo = false
                                    });
                                context.SaveChanges();

                                //Cria um objeto na Parametro_Curriculo para empresa
                                context.TAB_Parametro_Curriculo.Add(new TAB_Parametro_Curriculo()
                                {
                                    Idf_Curriculo = objCurriculo.Idf_Curriculo,
                                    Idf_Parametro = Convert.ToInt32(Enumeradores.Parametro.QuantidadeEnvioCurriculoEmpresa),
                                    Vlr_Parametro = "3",
                                    Dta_Cadastro = DateTime.Now,
                                    Flg_Inativo = false
                                });
                                context.SaveChanges();

                                DateTime dtaNacimento;
                                dtaNacimento = objCurriculo.TAB_Pessoa_Fisica.Dta_Nascimento.Value;

                                //Cria um objeto na Usuario_Filial_Perfil
                                context.TAB_Usuario_Filial_Perfil.Add(new TAB_Usuario_Filial_Perfil()
                                {
                                    Idf_Pessoa_Fisica = objCurriculo.TAB_Pessoa_Fisica.Idf_Pessoa_Fisica,
                                    Sen_Usuario_Filial_Perfil = (dtaNacimento.Day.ToString().PadLeft(2, '0').ToString() + dtaNacimento.Month.ToString().PadLeft(2, '0').ToString() + dtaNacimento.Year).ToString(),
                                    Idf_Perfil = 3,
                                    Idf_Filial = null,
                                    Flg_Inativo = false,
                                    Dta_Cadastro = DateTime.Now,
                                    Dta_Alteracao =DateTime.Now
                                });
                                context.SaveChanges();

                                //adicionar Cv Origem
                                context.BNE_Curriculo_Origem.Add(new BNE_Curriculo_Origem()
                                {
                                    Idf_Curriculo = objCurriculo.Idf_Curriculo,
                                    Idf_Origem = CurriculoDTO.idOrigem != null ? CurriculoDTO.idOrigem.Value : 10210, // 1 é o BNE.
                                    Dta_Cadastro = DateTime.Now,
                                    Dta_Alteracao = DateTime.Now
                                });

                                context.SaveChanges();
                            }

                            //Atualiza a DTO com os ids criados
                            CurriculoDTO.idCurriculo = objCurriculo.Idf_Curriculo;
                            CurriculoDTO.idPessoaFisica = objCurriculo.TAB_Pessoa_Fisica.Idf_Pessoa_Fisica;

                            dbTrans.Commit();
                        }
                        catch (Exception ex)
                        {
                            CurriculoDTO = CurriculoDTOOriginal; //Caso ocorra algum erro, devolve o DTO original
                            dbTrans.Rollback();
                            return ex.Message;
                        }
                    }
                }

                return string.Empty;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        
        #region CarregarObjetoPasso1
        private static void CarregarObjetoPasso1(DTO.Curriculo CurriculoDTO, BNE_Curriculo objCurriculo)
        {
            //Carrega os dados da pessoa fisica
            objCurriculo.TAB_Pessoa_Fisica.Nme_Pessoa = CurriculoDTO.nome;
            objCurriculo.TAB_Pessoa_Fisica.Nme_Pessoa_Pesquisa = objCurriculo.TAB_Pessoa_Fisica.Nme_Pessoa_Pesquisa == null ? CurriculoDTO.nome : objCurriculo.TAB_Pessoa_Fisica.Nme_Pessoa_Pesquisa;
            objCurriculo.TAB_Pessoa_Fisica.Num_CPF = Convert.ToDecimal(CurriculoDTO.CPF.Replace(".","").Replace("-","").ToString());
            objCurriculo.TAB_Pessoa_Fisica.Num_DDD_Celular = CurriculoDTO.DDDCelular;
            objCurriculo.TAB_Pessoa_Fisica.Num_Celular = CurriculoDTO.Celular;
            objCurriculo.TAB_Pessoa_Fisica.Num_Telefone = CurriculoDTO.Telefone;
            objCurriculo.TAB_Pessoa_Fisica.Num_DDD_Telefone = CurriculoDTO.DDDTelefone;
            objCurriculo.TAB_Pessoa_Fisica.Eml_Pessoa = CurriculoDTO.email;
            objCurriculo.TAB_Pessoa_Fisica.Idf_Sexo = CurriculoDTO.Sexo;
            objCurriculo.TAB_Pessoa_Fisica.Idf_Deficiencia = CurriculoDTO.idDeficiencia;
            
            if (objCurriculo.Idf_Curriculo == 0)
                objCurriculo.TAB_Pessoa_Fisica.Dta_Cadastro = DateTime.Now;
            
            objCurriculo.TAB_Pessoa_Fisica.Dta_Alteracao = DateTime.Now;
            objCurriculo.TAB_Pessoa_Fisica.Idf_Cidade = Cidade.CarregarCidadePorNomeEstado(CurriculoDTO.Cidade).Idf_Cidade;
            objCurriculo.TAB_Pessoa_Fisica.Dta_Nascimento = Convert.ToDateTime(CurriculoDTO.DataNascimento);

            //Carrega os dados do curriculo
            objCurriculo.Vlr_Pretensao_Salarial = Convert.ToDecimal(CurriculoDTO.PretensaoSalarial);

            if(objCurriculo.Idf_Curriculo ==0)
                objCurriculo.Dta_Cadastro = DateTime.Now;

            objCurriculo.Dta_Atualizacao = DateTime.Now;
            objCurriculo.Flg_Inativo = false;
            objCurriculo.Des_IP = objCurriculo.Des_IP == null ? "" : objCurriculo.Des_IP;
            objCurriculo.Idf_Situacao_Curriculo = objCurriculo.Idf_Situacao_Curriculo == 0 ? 2 : objCurriculo.Idf_Situacao_Curriculo; 
            objCurriculo.Idf_Tipo_Curriculo = 2;

        }
        #endregion

        #region AdicionarFuncaoPretendida
        private static void AdicionarFuncaoPretendida(DTO.Curriculo CurriculoDTO, int idCurriculo, LanEntities context)
        {
            //TODO: Implementar o processo de mais de uma função

            //Verificar existencia da função pretendida
            BNE_Funcao_Pretendida funcaoPretendidaAtual = (from funcoes in context.BNE_Funcao_Pretendida
                                                           where funcoes.Idf_Curriculo == idCurriculo
                                                           orderby funcoes.Dta_Cadastro descending
                                                           select funcoes).FirstOrDefault();

            if (funcaoPretendidaAtual == null)
            {
                //Inserir função pretendida
                BNE_Funcao_Pretendida FuncaoPretendidaNova = new BNE_Funcao_Pretendida();
                FuncaoPretendidaNova.Idf_Curriculo = idCurriculo;
                FuncaoPretendidaNova.Dta_Cadastro = DateTime.Now;

                TAB_Funcao objFuncao = Funcao.RecuperarPorNome(CurriculoDTO.funcao, context);
                
                //Caso a função não seja encontrada, grava apenas a descrição da função
                if (objFuncao == null)
                    FuncaoPretendidaNova.Des_Funcao_Pretendida = CurriculoDTO.funcao;
                else
                    FuncaoPretendidaNova.Idf_Funcao = objFuncao.Idf_Funcao;

                context.BNE_Funcao_Pretendida.Add(FuncaoPretendidaNova);
                context.SaveChanges();
                return;
            }

            //Buscar objeto da função atual (preenchida no formulario)
            var FuncaoPretendidaAtual = Funcao.RecuperarPorNome(CurriculoDTO.funcao, context);

            if (FuncaoPretendidaAtual == null)
            {
                //Função nao encontrada
                funcaoPretendidaAtual.Idf_Funcao = null;
                funcaoPretendidaAtual.Des_Funcao_Pretendida = CurriculoDTO.funcao;
            }
            else
            {
                if (FuncaoPretendidaAtual.Idf_Funcao != funcaoPretendidaAtual.Idf_Funcao)
                {
                    funcaoPretendidaAtual.Idf_Funcao = FuncaoPretendidaAtual.Idf_Funcao;
                    funcaoPretendidaAtual.Des_Funcao_Pretendida = "";                    
                }
            }
            context.SaveChanges();
        }
        #endregion

        #endregion

        #region SalvarPasso2
        /// <summary>
        /// Salvar os dados do passo 2 do cadastro. Experiências profissionais
        /// </summary>
        /// <param name="CurriculoDTO"></param>
        /// <returns></returns>
        private static string SalvarPasso2(DTO.Curriculo CurriculoDTO)
        {
            try
            {
                DTO.Curriculo CurriculoDTOOriginal = CurriculoDTO;

                using (var context = new LanEntities())
                {
                    using (var dbTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //Verificar a ultima experiência
                            if (CurriculoDTO.empresa != null)
                                ProcessarExperiencias(CurriculoDTO, 1, context);
                             
                            //Verificar a penultima experiência
                            if (CurriculoDTO.empresape != null && CurriculoDTO.empresape != "")
                                ProcessarExperiencias(CurriculoDTO, 2, context);

                            //Verificar a terceira experiência
                            if (CurriculoDTO.empresa3 != null && CurriculoDTO.empresa3 != "")
                                ProcessarExperiencias(CurriculoDTO, 3, context);

                            dbTrans.Commit();

                            CurriculoDTO.experiencias = Business.PessoaFisicaExperiencia.ListarExperienciasProfissionais(CurriculoDTO.idPessoaFisica);
                            return "";
                        }
                        catch(Exception ex)
                        {
                            CurriculoDTO = CurriculoDTOOriginal; //Caso ocorra algum erro, devolve o DTO original
                            dbTrans.Rollback();
                            return ex.Message;
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region ProcessarExperiencias
        private static void ProcessarExperiencias(DTO.Curriculo CurriculoDTO, int experiencia, LanEntities context)
        {
            int? idExperiencia = null;
            switch(experiencia)
            {
                case 1:
                    idExperiencia = CurriculoDTO.idExperiencia;
                    break;
                case 2:
                    idExperiencia = CurriculoDTO.idExperienciape;
                    break;
                case 3:
                    idExperiencia = CurriculoDTO.idExperiencia3;
                    break;
            }

            if (idExperiencia != 0)
            {
                //Existe experiencia inserida para este CV
                BNE_Experiencia_Profissional objExperiencia = (from ep in context.BNE_Experiencia_Profissional
                                                                where ep.Idf_Experiencia_Profissional == idExperiencia
                                                                select ep).FirstOrDefault();

                //Carregar Objeto
                SalvarObjetoExperiencia(CurriculoDTO, objExperiencia, experiencia, context);

            }
            else
            {
                //Cria nova experiência
                BNE_Experiencia_Profissional objExperiencia = new BNE_Experiencia_Profissional();

                //Carregar Objeto
                SalvarObjetoExperiencia(CurriculoDTO, objExperiencia, experiencia, context);

            }
            
        }
        #endregion

        #region CarregarObjetoExperiencia
        public static void SalvarObjetoExperiencia(DTO.Curriculo CurriculoDTO, BNE_Experiencia_Profissional objExperiencia, int exeperiencia, LanEntities context)
        {
            TAB_Funcao objFuncao;

            if (objExperiencia.Dta_Cadastro == DateTime.MinValue)
                objExperiencia.Dta_Cadastro = DateTime.Now;

            if (objExperiencia.Idf_Pessoa_Fisica == 0)
                objExperiencia.Idf_Pessoa_Fisica = CurriculoDTO.idPessoaFisica;

            switch(exeperiencia)
            {
                case 1:
                    objExperiencia.Raz_Social = CurriculoDTO.empresa;
                    objExperiencia.Dta_Admissao = new DateTime(Convert.ToInt32(CurriculoDTO.anoInicio), Convert.ToInt32(CurriculoDTO.mesInicio), 1);

                    objExperiencia.Dta_Demissao = null;
                    if (!string.IsNullOrEmpty(CurriculoDTO.anoSaida) && !string.IsNullOrEmpty(CurriculoDTO.mesSaida))
                        objExperiencia.Dta_Demissao = new DateTime(Convert.ToInt32(CurriculoDTO.anoSaida), Convert.ToInt32(CurriculoDTO.mesSaida), 1);

                    //if (!CurriculoDTO.empregoAtual)
                    //{
                    //    if (!string.IsNullOrEmpty(CurriculoDTO.anoSaida) && !string.IsNullOrEmpty(CurriculoDTO.mesSaida))
                    //        objExperiencia.Dta_Demissao = new DateTime(Convert.ToInt32(CurriculoDTO.anoSaida), Convert.ToInt32(CurriculoDTO.mesSaida), 1);
                    //}

                    objExperiencia.Des_Atividade = CurriculoDTO.atividades;
                    objFuncao = Funcao.RecuperarPorNome(CurriculoDTO.funcaoEmpresa, context);

                    //Caso a função não seja encontrada, grava apenas a descrição da função
                    if (objFuncao == null)
                        objExperiencia.Des_Funcao_Exercida = CurriculoDTO.funcaoEmpresa;
                    else 
                      objExperiencia.Idf_Funcao = objFuncao.Idf_Funcao;
                        
                    objExperiencia.Idf_Area_BNE = CurriculoDTO.idAreaBNE;

                    //Adiciona na entidade
                    if (objExperiencia.Idf_Experiencia_Profissional == 0)
                        context.BNE_Experiencia_Profissional.Add(objExperiencia);

                    context.SaveChanges();
                    CurriculoDTO.idExperiencia = objExperiencia.Idf_Experiencia_Profissional;
                    
                    break;


                case 2:
                    objExperiencia.Raz_Social = CurriculoDTO.empresape;
                    objExperiencia.Dta_Admissao = new DateTime(Convert.ToInt32(CurriculoDTO.anoIniciope), Convert.ToInt32(CurriculoDTO.mesIniciope), 1);

                    objExperiencia.Dta_Demissao = null;
                    if (!string.IsNullOrEmpty(CurriculoDTO.anoSaidape) && !string.IsNullOrEmpty(CurriculoDTO.mesSaidape))
                        objExperiencia.Dta_Demissao = new DateTime(Convert.ToInt32(CurriculoDTO.anoSaidape), Convert.ToInt32(CurriculoDTO.mesSaidape), 1);
                    
                    objExperiencia.Des_Atividade = CurriculoDTO.atividadespe;
                    objFuncao = Funcao.RecuperarPorNome(CurriculoDTO.funcaoEmpresape, context);

                    //Caso a função não seja encontrada, grava apenas a descrição da função
                    if (objFuncao == null)
                        objExperiencia.Des_Funcao_Exercida = CurriculoDTO.funcaoEmpresape;
                    else
                        objExperiencia.Idf_Funcao = objFuncao.Idf_Funcao;

                     objExperiencia.Idf_Area_BNE = CurriculoDTO.idAreaBNEpe;

                    //Adiciona na entidade
                    if (objExperiencia.Idf_Experiencia_Profissional == 0)
                        context.BNE_Experiencia_Profissional.Add(objExperiencia);

                    context.SaveChanges();
                    CurriculoDTO.idExperienciape = objExperiencia.Idf_Experiencia_Profissional;

                    break;

                case 3:
                    objExperiencia.Raz_Social = CurriculoDTO.empresa3;
                    objExperiencia.Dta_Admissao = new DateTime(Convert.ToInt32(CurriculoDTO.anoInicio3), Convert.ToInt32(CurriculoDTO.mesInicio3), 1);

                    objExperiencia.Dta_Demissao = null;
                    if (!string.IsNullOrEmpty(CurriculoDTO.anoSaida3) && !string.IsNullOrEmpty(CurriculoDTO.mesSaida3))
                        objExperiencia.Dta_Demissao = new DateTime(Convert.ToInt32(CurriculoDTO.anoSaida3), Convert.ToInt32(CurriculoDTO.mesSaida3), 1);

                    objExperiencia.Des_Atividade = CurriculoDTO.atividades3;                    
                    objFuncao = Funcao.RecuperarPorNome(CurriculoDTO.funcaoEmpresa3, context);

                    //Caso a função não seja encontrada, grava apenas a descrição da função
                    if (objFuncao == null)
                        objExperiencia.Des_Funcao_Exercida = CurriculoDTO.funcaoEmpresa3;
                    else
                        objExperiencia.Idf_Funcao = objFuncao.Idf_Funcao;

                    objExperiencia.Idf_Area_BNE = CurriculoDTO.idAreaBNE3;

                    //Adiciona na entidade
                    if (objExperiencia.Idf_Experiencia_Profissional == 0)
                        context.BNE_Experiencia_Profissional.Add(objExperiencia);

                    context.SaveChanges();
                    CurriculoDTO.idExperiencia3 = objExperiencia.Idf_Experiencia_Profissional;

                    break;
            }
        }
        #endregion

        #endregion

        #region SalvarPasso3
        /// <summary>
        /// Salva os dados do passo 3. Formação. especialização, cursos e idiomas
        /// </summary>
        /// <param name="CurriculoDTO"></param>
        /// <returns></returns>
        private static string SalvarPasso3(DTO.Curriculo CurriculoDTO)
        {
            try
            {
                DTO.Curriculo CurriculoDTOOriginal = CurriculoDTO;

                using (var context = new LanEntities())
                {
                    using (var dbTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            ProcessarFormacoes(CurriculoDTO, context);
                            ProcessarCursos(CurriculoDTO, context);
                            ProcessarIdiomas(CurriculoDTO, context);

                            dbTrans.Commit();
                        }
                        catch(Exception ex)
                        {
                            CurriculoDTO = CurriculoDTOOriginal; //Caso ocorra algum erro, devolve o DTO original
                            dbTrans.Rollback();
                            return ex.Message;
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region ProcessarFormacoes
        /// <summary>
        /// Adiciona ou edita as formações existentes para a pessoa física. OBS: As formações são excluidas no ato do click do botão excluir
        /// </summary>
        /// <param name="CurriculoDTO"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static void ProcessarFormacoes(DTO.Curriculo CurriculoDTO, LanEntities context)
        {

            if (CurriculoDTO.formacoes == null)
                return;

            BNE_Formacao objFormacao;
            foreach(var formacao in CurriculoDTO.formacoes)
            {
                //pegar id do curso pela descricao
                int idCurso = Business.Curso.CarregarIdCursoporDescricao(formacao.curso);

                //pegar a fonte(instituição) pela descrição
                int idFonte = Business.Fonte.CarregarIdInstituicaoPelaDescricao(formacao.instituicao);

                //pegar id da cidade pela descrição
                int idCidade = 0;
                if (!string.IsNullOrEmpty(formacao.cidadeFormacao))
                    idCidade = Business.Cidade.CarregarCidadePorNomeEstado(formacao.cidadeFormacao).Idf_Cidade;

                //pegar id da Situação da Formação
                short idSituacaoFormacao = Business.SituacaoFormacao.CarregarIdSituacaopelaDescricao(formacao.situacao);

                if (formacao.idFormacao == 0)
                {
                    objFormacao = new BNE_Formacao();
                    objFormacao.Idf_Cidade = idCidade > 0 ? idCidade : formacao.idCidade;
                    objFormacao.Idf_Escolaridade = formacao.idEscolaridade;
                    objFormacao.Idf_Pessoa_Fisica = CurriculoDTO.idPessoaFisica;
                    objFormacao.Idf_Situacao_Formacao = idSituacaoFormacao > 0 ? idSituacaoFormacao : formacao.idSituacaoFormacao;
                    objFormacao.Qtd_Carga_Horaria = formacao.cargaHoraria;
                    objFormacao.Dta_Cadastro = DateTime.Now;
                    objFormacao.Ano_Conclusao = formacao.anoConclusao;

                    //nome da instituição
                    if (idFonte > 0)
                        objFormacao.Idf_Fonte = idFonte;
                    else
                        objFormacao.Des_Fonte = formacao.instituicao;

                    //nome do curso
                    if(idCurso >0)
                        objFormacao.Idf_Curso = idCurso;
                    else
                        objFormacao.Des_Curso = formacao.curso;


                    context.BNE_Formacao.Add(objFormacao);

                    context.SaveChanges();
                    formacao.idFormacao = objFormacao.Idf_Formacao;
                }
                else
                {
                    objFormacao = (from f in context.BNE_Formacao
                                   where f.Idf_Formacao == formacao.idFormacao
                                       select f).FirstOrDefault();

                    objFormacao.Idf_Cidade = idCidade > 0 ? idCidade : formacao.idCidade;
                    objFormacao.Idf_Escolaridade = formacao.idEscolaridade;
                    objFormacao.Idf_Situacao_Formacao = idSituacaoFormacao > 0 ? idSituacaoFormacao : formacao.idSituacaoFormacao;
                    objFormacao.Qtd_Carga_Horaria = formacao.cargaHoraria;
                    objFormacao.Dta_Alteracao = DateTime.Now;
                    objFormacao.Ano_Conclusao = formacao.anoConclusao;

                    //nome da instituição
                    if (idFonte > 0)
                        objFormacao.Idf_Fonte = idFonte;
                    else
                        objFormacao.Des_Fonte = formacao.instituicao;

                    //nome do curso
                    if (idCurso > 0)
                        objFormacao.Idf_Curso = idCurso;
                    else
                        objFormacao.Des_Curso = formacao.curso;
                }
            }

            context.SaveChanges();
        }
        #endregion

        #region ProcessarCursos
        /// <summary>
        /// Adiciona ou edita os cursos existentes para a pessoa física. OBS: Os cursos são excluidos no ato do click do botão excluir
        /// </summary>
        /// <param name="CurriculoDTO"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static void ProcessarCursos(DTO.Curriculo CurriculoDTO, LanEntities context)
        {
            if (CurriculoDTO.cursos == null)
                return;
            
            BNE_Formacao objCurso;
            foreach (var curso in CurriculoDTO.cursos)
            {
                //pegar id da cidade pela descrição
                int idCidade = 0;
                if(curso.cidadeFormacao != "")
                    idCidade = Business.Cidade.CarregarCidadePorNomeEstado(curso.cidadeFormacao).Idf_Cidade;

                if (curso.idFormacao == 0)
                {
                    objCurso = new BNE_Formacao();
                    objCurso.Idf_Cidade = idCidade > 0 ? idCidade : curso.idCidade;
                    objCurso.Des_Curso = curso.curso;
                    objCurso.Des_Fonte = curso.instituicao;
                    objCurso.Idf_Escolaridade = 18;
                    objCurso.Idf_Pessoa_Fisica = CurriculoDTO.idPessoaFisica;
                    objCurso.Idf_Situacao_Formacao = curso.idSituacaoFormacao;
                    objCurso.Qtd_Carga_Horaria = curso.cargaHoraria;
                    objCurso.Dta_Cadastro = DateTime.Now;

                    context.BNE_Formacao.Add(objCurso);
                    
                    context.SaveChanges();
                    curso.idFormacao = objCurso.Idf_Formacao;
                }
                else
                {
                    objCurso = (from c in context.BNE_Formacao
                                   where c.Idf_Formacao == curso.idFormacao
                                   select c).FirstOrDefault();
                    objCurso.Idf_Cidade = idCidade > 0 ? idCidade : curso.idCidade;
                    objCurso.Des_Curso = curso.curso;
                    objCurso.Des_Fonte = curso.instituicao;
                    objCurso.Idf_Escolaridade = curso.idEscolaridade;
                    objCurso.Idf_Situacao_Formacao = curso.idSituacaoFormacao;
                    objCurso.Qtd_Carga_Horaria = curso.cargaHoraria;
                    objCurso.Dta_Alteracao = DateTime.Now;
                }
            }

            context.SaveChanges();
        }
        #endregion

        #region ProcessarIdiomas
        /// <summary>
        /// Adiciona ou edita os idiomas existentes para a pessoa física. OBS: Os idiomas são excluidos no ato do click do botão excluir
        /// </summary>
        /// <param name="CurriculoDTO"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static void ProcessarIdiomas(DTO.Curriculo CurriculoDTO, LanEntities context)
        {
            if (CurriculoDTO.idiomasCandidato == null)
                return;

            TAB_Pessoa_Fisica_Idioma objIdioma;
            foreach (var idioma in CurriculoDTO.idiomasCandidato)
            {
                //checar se tem o registro para o idioma
                var objIdiomaNew = (from i in context.TAB_Pessoa_Fisica_Idioma
                                    where i.Idf_Idioma == idioma.idIdioma && i.Idf_Pessoa_Fisica == CurriculoDTO.idPessoaFisica
                                    select i).FirstOrDefault();


                if (idioma.idIdiomaCandidato == 0 && objIdiomaNew == null)
                {
                    objIdioma = new TAB_Pessoa_Fisica_Idioma();
                    objIdioma.Idf_Idioma = idioma.idIdioma;
                    objIdioma.Idf_Nivel_Idioma = idioma.nivel;
                    objIdioma.Idf_Pessoa_Fisica = CurriculoDTO.idPessoaFisica;
                    objIdioma.Dta_Cadastro = DateTime.Now;
                    objIdioma.Flg_Inativo = false;

                    context.TAB_Pessoa_Fisica_Idioma.Add(objIdioma);

                    context.SaveChanges();
                    idioma.idIdiomaCandidato = objIdioma.Idf_Pessoa_Fisica_Idioma;
                }
                else
                {
                    //objIdioma = (from i in context.TAB_Pessoa_Fisica_Idioma
                    //            where i.Idf_Pessoa_Fisica_Idioma == idioma.idIdiomaCandidato
                    //            select i).FirstOrDefault();

                    objIdioma = objIdiomaNew;
                    objIdioma.Idf_Idioma = idioma.idIdioma;
                    objIdioma.Idf_Nivel_Idioma = idioma.nivel;
                    objIdioma.Idf_Pessoa_Fisica = CurriculoDTO.idPessoaFisica;
                    objIdioma.Dta_Cadastro = DateTime.Now;
                    objIdioma.Flg_Inativo = false;
                }
            }

            context.SaveChanges();
        }
        #endregion

        #endregion

        #region SalvarPasso4
        /// <summary>
        /// Salva os dados do passo 4. Período de trabalho, estado civil, observações e foto.
        /// </summary>
        /// <param name="objCurriculo"></param>
        /// <returns></returns>
        private static string SalvarPasso4(DTO.Curriculo CurriculoDTO)
        {
            try
            {
                using (var context = new LanEntities())
                {
                    using (var dbTrans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            BNE_Curriculo objCurriculo;
                            TAB_Pessoa_Fisica objPessoa;

                            Curriculo.CarregarPorId(CurriculoDTO.idCurriculo, out objCurriculo, context);
                            PessoaFisica.CarregarPorCV(CurriculoDTO.idCurriculo, out objPessoa, context);

                            objPessoa.Idf_Estado_Civil = CurriculoDTO.idEstadoCivil;
                            objCurriculo.Obs_Curriculo = CurriculoDTO.Observacao;
                            
                            //Periodos
                            objCurriculo.Flg_Manha = CurriculoDTO.periodoManha;
                            objCurriculo.Flg_Tarde = CurriculoDTO.periodoTarde;
                            objCurriculo.Flg_Noite = CurriculoDTO.periodoNoite;
                            objCurriculo.Flg_Final_Semana = CurriculoDTO.periodoFimdeSemana;


                            //se alterou a imagem vai salvar a nova caso esteja no passo 4
                            if (CurriculoDTO.alterouImagem && !CurriculoDTO.salvarTudo)
                            {
                                //salvar imagem do candidato na base
                                byte[] imagemDados = null;

                                //caminho fisico da imagem
                                string caminhoCompletoImagem = System.Configuration.ConfigurationManager.AppSettings["PathImages"] + CurriculoDTO.imgCandidato.Split('/')[2];

                                //carregar a imagem do disco
                                imagemDados = CarregarArquivoImagem(caminhoCompletoImagem);

                                //salvar imagem no banco de dados
                                bool imgSalvouImagem = Business.PessoaFisica.SalvarImagemCandidato(CurriculoDTO.idPessoaFisica, imagemDados, context);

                                //apagar imagem do disco
                                if (imgSalvouImagem)
                                {
                                    System.IO.File.Delete(caminhoCompletoImagem);
                                }
                            }

                            context.SaveChanges();
                            dbTrans.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTrans.Rollback();
                            return ex.Message;
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region CarregarArquivoImagem

        private static byte[] CarregarArquivoImagem(string caminhoCompletoImagem)
        {
            try
            {
                byte[] imagemBytes = null;
                FileStream fs = new FileStream(caminhoCompletoImagem,FileMode.Open, FileAccess.Read);

                BinaryReader br = new BinaryReader(fs);
                imagemBytes = br.ReadBytes(12000000);
                
                fs.Dispose();
                return imagemBytes;
            }
            catch(Exception ex)
            {
                throw ex;
        }
        }
        #endregion

        #region SalvarCompleto
        private static string SalvarCompleto(DTO.Curriculo CurriculoDTO)
        {
            try
            {
                string retorno;

                //Chama passo 1
                retorno = SalvarPasso1(CurriculoDTO);
                if (retorno != string.Empty)
                    return retorno;

                //Chama passo 2
                retorno = SalvarPasso2(CurriculoDTO);
                if (retorno != string.Empty)
                    return retorno;

                //Chama passo 3
                retorno = SalvarPasso3(CurriculoDTO);
                if (retorno != string.Empty)
                    return retorno;

                //Chama passo 4
                retorno = SalvarPasso4(CurriculoDTO);
                if (retorno != string.Empty)
                    return retorno;

                return string.Empty;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region CarregarNomeCandidato
        public static string CarregarNomeCandidato(string cpf, string dataNascimento)
        {
            string nomeCandidato = "";
            TAB_Pessoa_Fisica objPessoaFisica;
            int existe = Business.PessoaFisica.ValidarCPFeDataNascimento(Convert.ToDecimal(cpf.Replace(".", "").Replace("-", "")), Convert.ToDateTime(dataNascimento), out objPessoaFisica);

            if (existe == 2)
            {
                nomeCandidato = objPessoaFisica.Nme_Pessoa;
            }

            return nomeCandidato;
        }
        #endregion

        #region CarregarCurriculo
        public static Business.DTO.Curriculo CarregarCurriculo(string cpf, string dataNascimento, string nomeCandidato)
        {
            Business.DTO.Curriculo curriculo = null;

            TAB_Pessoa_Fisica objPessoaFisica;
            int existe = Business.PessoaFisica.ValidarCPFeDataNascimento(Convert.ToDecimal(cpf.Replace(".","").Replace("-","")), Convert.ToDateTime(dataNascimento), out objPessoaFisica);

            if (existe == 2)
            {
                string cpf1 = objPessoaFisica.Num_CPF.ToString().Length < 11 ?
                    objPessoaFisica.Num_CPF.ToString().PadLeft(11, '0') : objPessoaFisica.Num_CPF.ToString();

                //get Curriculo
                BNE_Curriculo objCurriculo;
                bool temCV = Business.Curriculo.CarregarPorIdPessoaFisica(objPessoaFisica.Idf_Pessoa_Fisica, out objCurriculo);

                //listar os idiomas do candidato
                List<DTO.IdiomaCandidato> listaIdiomasPessoa = null;
                listaIdiomasPessoa = Business.PessoaFisicaIdioma.ListarIdiomas(objPessoaFisica.Idf_Pessoa_Fisica);

                //listar as formações do candidato
                List<DTO.Formacao> listarFormacoes = null;
                listarFormacoes = Business.PessoaFisicaFormacao.ListarFormacao(objPessoaFisica.Idf_Pessoa_Fisica);

                //listar os cursos do candidato
                List<DTO.Formacao> listaCursos = null;
                listaCursos = Business.PessoaFisicaFormacao.ListarCursos(objPessoaFisica.Idf_Pessoa_Fisica);

                //Listar Experiencias profissionais
                List<DTO.Experiencia> listaExperiencias = null;
                listaExperiencias = Business.PessoaFisicaExperiencia.ListarExperienciasProfissionais(objPessoaFisica.Idf_Pessoa_Fisica);

                //listar as funções pretendidas do candidato
                IList listaFucaoPretendida = null;
                bool temFuncao = Business.FuncaoPretendida.CarregarPorCurriculo(objCurriculo.Idf_Curriculo, out listaFucaoPretendida);

                curriculo = new Business.DTO.Curriculo
                {
                    idPessoaFisica = objPessoaFisica.Idf_Pessoa_Fisica,
                    idCurriculo = objCurriculo.Idf_Curriculo,
                    nome = objPessoaFisica.Nme_Pessoa,
                    email = objPessoaFisica.Eml_Pessoa,
                    DDDCelular = objPessoaFisica.Num_DDD_Celular != null ? objPessoaFisica.Num_DDD_Celular.Trim() : "",
                    Celular = objPessoaFisica.Num_Celular != null ? objPessoaFisica.Num_Celular.Trim() : "",
                    DDDTelefone = objPessoaFisica.Num_DDD_Telefone != null ? objPessoaFisica.Num_DDD_Telefone.Trim() : "",
                    Telefone = objPessoaFisica.Num_Telefone != null ? objPessoaFisica.Num_Telefone.Trim() : "",
                    Sexo = objPessoaFisica.Idf_Sexo,
                    DataNascimento = objPessoaFisica.Dta_Nascimento != null ? objPessoaFisica.Dta_Nascimento.Value.ToString("dd/MM/yyyy") : "",
                    CPF = cpf1, //objPessoaFisica.Num_CPF.ToString(),
                    TemDeficiencia = objPessoaFisica.Idf_Deficiencia != null ? true : false,
                    idDeficiencia = objPessoaFisica.Idf_Deficiencia,
                    idEstadoCivil = objPessoaFisica.Idf_Estado_Civil,

                    isVip = objCurriculo.Flg_VIP,

                    //Quantidade de candidaturas disponíveis
                    qtdCandidatura = Candidatura.QuantidadeCandidaturasDisponiveisUsuario(objCurriculo.Idf_Curriculo),

                    //Quantidade de envio de CV para empresas
                    qtdEnvioCVEmpresa = EnviarCVEmpresa.QuantidadeEnvioDisponiveisUsuario(objCurriculo.Idf_Curriculo),

                    //idiomas PF
                    idiomasCandidato = listaIdiomasPessoa,

                    //formação e cursos
                    formacoes = listarFormacoes,

                    //cursos
                    cursos = listaCursos,

                    //funções pretendidas
                    funcoes = listaFucaoPretendida,

                    //experiencias
                    TemExperienciaProfissional = listaExperiencias.Count > 0,
                    experiencias = listaExperiencias,

                    periodoManha = objCurriculo.Flg_Manha,
                    periodoTarde = objCurriculo.Flg_Tarde,
                    periodoNoite = objCurriculo.Flg_Noite,
                    periodoFimdeSemana = objCurriculo.Flg_Final_Semana,

                    Observacao = objCurriculo.Obs_Curriculo,

                    Cidade = objPessoaFisica.TAB_Cidade.Nme_Cidade + "/" + objPessoaFisica.TAB_Cidade.Sig_Estado,
                    PretensaoSalarial = objCurriculo.Vlr_Pretensao_Salarial.Value,
                };
            }
            else if (existe == 0)
            {
                curriculo = new Business.DTO.Curriculo
                {
                    nome = nomeCandidato,
                    CPF = cpf,
                    DataNascimento = dataNascimento
                };
            }

            return curriculo;
        }
        #endregion


    }
}
