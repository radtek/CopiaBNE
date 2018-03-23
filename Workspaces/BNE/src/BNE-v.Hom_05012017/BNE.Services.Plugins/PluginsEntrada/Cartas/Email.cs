using BNE.BLL;
using BNE.BLL.Common;
using BNE.BLL.Custom;

namespace BNE.Services.Plugins.PluginsEntrada.Cartas
{
    public class Email
    {
        private static readonly BLL.DTO.CartaEmail TemplateMensagemEmailCvPerfilVaga = CartaEmail.RecuperarCarta(BLL.Enumeradores.CartaEmail.MensagemEmailCvPerfilVaga);

        #region MensagemEmailCvPerfilVaga
        public static string MensagemEmailCvPerfilVaga(string descricaoFuncao, string linkVaga, int idVaga, short? quantidadeVaga,decimal? valorSalarioDe, decimal? valorSalarioPara, string nomeCidade, string siglaEstado, string atribuicoesVaga, string requisitosVaga,
            string nomeCandidato, string linkListaVagas, string linkSalaVIP, string linkVIP, string linkQuemMeViu, string linkCadastroCV, string linkPesquisaVaga, string linkLoginCandidato, out string assunto)
        {
             string vagalivre = string.Empty;
            Vaga objVaga = Vaga.LoadObject(idVaga);
            var objPlanoAdquiridoDetalhes = PlanoAdquiridoDetalhes.PlanoEnvioSmsEmailVagaLiberado(objVaga, objVaga.Filial);
            if (objPlanoAdquiridoDetalhes != null) 
               vagalivre = "<img class=\"responsive_logo\" src=\"http://mailing.bne.com.br/newsletter/padrao/images/livre.png\" alt=\"vaga livre\" style=\"border:none; display:block; -ms-interpolation-mode: bicubic;\" height=\"80\" >";

            var parametros = new
            {
                Funcao_Vaga = descricaoFuncao,
                Qtd_Vaga = quantidadeVaga,
                Idf_Vaga = idVaga,
                Salario = Helper.RetornarDesricaoSalario(valorSalarioDe.HasValue ? valorSalarioDe : null, valorSalarioPara.HasValue ? valorSalarioPara : null),
                Requisitos_Vaga = !string.IsNullOrWhiteSpace(requisitosVaga) ? requisitosVaga : "não informados",
                Nome_Usuario = nomeCandidato,
                Atribuicoes_Vaga = atribuicoesVaga,
                Cidade = nomeCidade,
                UF = siglaEstado,
                Lista_Vagas = linkListaVagas,
                Sala_Vip = linkSalaVIP,
                vip = linkVIP,
                Quem_Me_Viu = linkQuemMeViu,
                Cadastro_Curriculo = linkCadastroCV,
                Pesquisa_Vagas = linkPesquisaVaga,
                login_candidato = linkLoginCandidato,
                Link_Vaga = linkVaga,
                Logo_Vaga_Livre = vagalivre
            };
            assunto = parametros.ToString(TemplateMensagemEmailCvPerfilVaga.Assunto);
            assunto = assunto.Replace("{0}", nomeCandidato);
            return parametros.ToString(TemplateMensagemEmailCvPerfilVaga.Conteudo);
        }
        #endregion

    }
}
