using System;
using System.Linq;

namespace BNE.Web.Code.Enumeradores
{
    public class PageRoute
    {
        public enum Routes
        {
            Default,
            //Login Empresa            
            SalaSelecionador,
            VagasAnunciadas,
            ProdutoCIA, //Meu Plano - Empresa sem plano
            ProdutoCIAPlano, //Meu Plano - Empresa Com plano  Redirect("SalaSelecionadorPlanoIlimitado.aspx");
            CampanhaRecrutamento,
            SalaSelecionadorConfiguracoes,
            SalaSelecionadorMensagens,
            PesquisaCurriculoAvancada,
            AnunciarVaga,
            CadastroEmpresaDados,
            CadastroEmpresaUsuario,
            ApresentarR1,
            CompararCurriculo,            
            //Login Candidato
            SalaVIP,
            QuemMeViuVip,
            QuemMeViuSemPlano,
            QuemMeViuNaoVisualizado,
            EscolherEmpresa,
            RelatorioSalarialVip,
            RelatorioSalarialSemPlano,
            AlertaVagas,
            ProdutoVipPlano,
            ProdutoVIP,            
            CadastroCurriculoMini,
            SalaVipMensagens,
            SalaVipJaEnviei,
            SalaVipJaEnvieiNaoCandidatura,
            CadastroCurriculoDados,
            CadastroCurriculoFormacao,
            CadastroCurriculoComplementar,
            CadastroCurriculoRevisao,
            //Geral
            PesquisaVagaAvancada,
            PesquisaCurriculo,
            CurriculosPorFuncao,
            CurriculosPorCidade,
            CurriculosPorFuncaoCidade,
            Curriculo,
            OndeEstamos,
            Agradecimentos,         
            FalePresidente,
            BlackFriday
        }

        public static Routes GetPageRoute(string strRoute)
        {
            var allValues = (Routes[])Enum.GetValues(typeof(Routes));

            Routes route = allValues.FirstOrDefault(r => r.ToString() == strRoute.Trim());

            return route;
        }
    }
}