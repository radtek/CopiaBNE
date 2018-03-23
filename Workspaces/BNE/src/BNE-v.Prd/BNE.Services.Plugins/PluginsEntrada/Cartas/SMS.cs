using BNE.BLL;
using BNE.BLL.Common;
using BNE.BLL.Custom;
using System;

namespace BNE.Services.Plugins.PluginsEntrada.Cartas
{
    public class SMS
    {
        public static string MensagemSMSCvPerfilVaga(string descricaoFuncao, int idVaga, string TemplateMensagemSMSCvPerfilVaga)
        {
            try {
                var parametros = new
                {
                    Funcao = descricaoFuncao,
                    Idf_Vaga = idVaga
                };
                return parametros.ToString(TemplateMensagemSMSCvPerfilVaga);
            }
            catch(Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha na busca da mensagem MensagemSMSCvPerfilVaga");
                throw;
            }
        }

        public static string MensagemPesquisaCurriculoEmpresa(string descricaoFuncao, string descricaoCidade, string nomePessoa, string TemplatePesquisaCurriculoEmpresa)
        {
            try
            {
                var parametros = new
                {
                    Funcao = descricaoFuncao,
                    Cidade = descricaoCidade,
                    Nome = nomePessoa
                };
                return parametros.ToString(TemplatePesquisaCurriculoEmpresa);
            }
            catch(Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha na busca da mensagem MensagemPesquisaCurriculoEmpresa");
                throw;
            }
        }

    }
}
