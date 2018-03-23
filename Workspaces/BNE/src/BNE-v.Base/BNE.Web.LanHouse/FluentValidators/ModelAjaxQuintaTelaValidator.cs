using System;
using System.Globalization;
using System.Text.RegularExpressions;
using BNE.Web.LanHouse.EntityFramework;
using FluentValidation;

namespace BNE.Web.LanHouse.FluentValidators
{
    public class ModelAjaxQuintaTelaValidator : AbstractValidator<Models.ModelAjaxQuintaTela>
    {
        public ModelAjaxQuintaTelaValidator()
        {
            RuleFor(x => x.Escolaridade)
                .NotEmpty()
                .InclusiveBetween(4 /*1o grau incompleto*/, 13 /*superior completo*/)
                .WithName("Id de Escolaridade")
                .WithMessage("O número que indica a <b>escolaridade é inválido</b>, precisa estar entre 4 e 13, inclusive");

            RuleFor(x => x.InstituicaoEnsino)
                //.NotEmpty()
                .Must(SerInstituicaoEnsinoValida)
                .WithName("Instituição de ensino")
                .WithMessage("<b>Instituição de ensino inválida</b>");

            RuleFor(x => x.Fonte)
                .Must(SerFonteVaziaOuNumericaEValida)
                .WithName("Id de Fonte")
                .WithMessage("A instituição de ensino <b>precisa ser válida</b>");

            RuleFor(x => x.NomeCurso)
                //.NotEmpty()
                .Must(SerNomeCursoValido)
                .WithName("Nome do curso")
                .WithMessage("<b>Nome do curso não pode estar vazio</b>, favor corrigir");

            /*RuleFor(x => x.Cidade)
                .NotEmpty()
                .Must(SerCidadeValida)
                .WithName("Cidade instituição ensino")
                .WithMessage("<b>A cidade da instituição de ensino não pode ser vazia ou inválida</b>, favor corrigir");*/

            RuleFor(x => x.IdCidade)
                .Must(SerIdCidadeVaziaOuNumericaEValida)
                .WithName("Id da Cidade")
                .WithMessage("O número que indica a cidade da instituição de ensino <b>precisa ser válido</b>");

            RuleFor(x => x.Periodo)
                //.NotEmpty()
                .Must(SerPeriodoValido)
                .WithName("Período")
                .WithMessage("<b>O período precisa estar entre o 1o e o 12o.</b>, favor corrigir");

            RuleFor(x => x.Situacao)
                //.NotEmpty()
                .Must(SerSituacaoValida)
                .WithName("Situação")
                .WithMessage("O número que indica a situação do curso deve estar <b>entre 1 e 3</b>, favor corrigir");

            RuleFor(x => x.AnoConclusao)
                //.NotEmpty()
                .Must(SerAnoConclusaoValido)
                .WithName("Ano de conclusão")
                .WithMessage(String.Format("Favor corrigir o <b>ano de conclusão do curso</b>, que deve estar entre 1900 e {0}", DateTime.Now.Year));

            RuleFor(x => x.NomeEmpresa1)
                .Must(TerOutrosCamposDaEmpresa1Preenchidos)
                .WithName("Nome da última empresa")
                .WithMessage("Preencha todos os campos da <b>última empresa</b> corretamente");

            RuleFor(x => x.NomeEmpresa2)
                .Must(TerOutrosCamposDaEmpresa2Preenchidos)
                .WithName("Nome da penúltima empresa")
                .WithMessage("Preencha todos os campos da <b>penúltima empresa</b> corretamente");

            RuleFor(x => x.NomeEmpresa3)
                .Must(TerOutrosCamposDaEmpresa3Preenchidos)
                .WithName("Nome da antepenúltima empresa")
                .WithMessage("Preencha todos os campos da <b>antepenúltima empresa</b> corretamente");

        }

        private bool TerOutrosCamposDaEmpresa1Preenchidos(Models.ModelAjaxQuintaTela model, string nomeEmpresa1)
        {
            if (!String.IsNullOrEmpty(nomeEmpresa1) && Regex.IsMatch(nomeEmpresa1, @"(\w*\s?)*"))
            {
                DateTime a;
                DateTime d;

                return model.AreaEmpresa1 >= 1 && model.AreaEmpresa1 <= 56 &&
                    DateTime.TryParse(model.DataAdmissao1, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal, out a) &&
                    Regex.IsMatch(model.Atribuicoes1, @"(\w*\s?)*", RegexOptions.Multiline);
            }

            return true;
        }

        private bool TerOutrosCamposDaEmpresa2Preenchidos(Models.ModelAjaxQuintaTela model, string nomeEmpresa2)
        {
            if (!String.IsNullOrEmpty(nomeEmpresa2) && Regex.IsMatch(nomeEmpresa2, @"(\w+\s?)*"))
            {
                DateTime a;
                DateTime d;

                return model.AreaEmpresa2 >= 1 && model.AreaEmpresa2 <= 56 &&
                    DateTime.TryParse(model.DataAdmissao2, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal, out a) &&
                    DateTime.TryParse(model.DataDemissao2, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal, out d) &&
                    Regex.IsMatch(model.Atribuicoes2, @"(\w+\s?)*", RegexOptions.Multiline);
            }

            return true;
        }

        private bool TerOutrosCamposDaEmpresa3Preenchidos(Models.ModelAjaxQuintaTela model, string nomeEmpresa3)
        {
            if (!String.IsNullOrEmpty(nomeEmpresa3) && Regex.IsMatch(nomeEmpresa3, @"(\w+\s?)*"))
            {
                DateTime a;
                DateTime d;

                return model.AreaEmpresa3 >= 1 && model.AreaEmpresa3 <= 56 &&
                    DateTime.TryParse(model.DataAdmissao3, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal, out a) &&
                    DateTime.TryParse(model.DataDemissao3, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal, out d) &&
                    Regex.IsMatch(model.Atribuicoes3, @"(\w+\s?)*", RegexOptions.Multiline);
            }

            return true;
        }

        private bool SerAnoConclusaoValido(Models.ModelAjaxQuintaTela model, int? ano)
        {
            if (EscolaridadeComAnoConclusao(model.Escolaridade))
            {
                if (ano >= 1900 && ano <= DateTime.Now.Year)
                    return true;
                return false;
            }
            return true;
        }

        private bool SerSituacaoValida(Models.ModelAjaxQuintaTela model, int? situacao)
        {
            if (EscolaridadeComSituacao(model.Escolaridade))
            {
                if (situacao >= 1 && situacao <= 3)
                    return true;
                return false;
            }
            return true;
        }

        private bool SerPeriodoValido(Models.ModelAjaxQuintaTela model, int? periodo)
        {
            if (EscolaridadeComInstituicao(model.Escolaridade) && EscolaridadeComSituacao(model.Escolaridade))
            {
                if (periodo >= 1 && periodo <= 12)
                    return true;
                return false;
            }
            return true;
        }

        private bool SerInstituicaoEnsinoValida(Models.ModelAjaxQuintaTela model, string instituicao)
        {
            if (EscolaridadeComInstituicao(model.Escolaridade))
            {
                if (Regex.IsMatch(instituicao, @"(\w*\s?)*"))
                    return true;
                return false;
            }
            return true;
        }

        private bool SerFonteVaziaOuNumericaEValida(Models.ModelAjaxQuintaTela model, int? idFonte)
        {
            if (EscolaridadeComInstituicao(model.Escolaridade))
            {
                TAB_Fonte objFonte;
                if (idFonte.HasValue && BLL.Fonte.CarregarPorId((int)idFonte, out objFonte))
                    return true;
                return false;
            }
            return true;
        }

        private bool SerCidadeValida(Models.ModelAjaxQuintaTela model, string cidade)
        {
            return EscolaridadeComInstituicao(model.Escolaridade) &&
                Regex.IsMatch(cidade, @"(\w*\s?)*");
        }

        private bool SerNomeCursoValido(Models.ModelAjaxQuintaTela model, string curso)
        {
            if (EscolaridadeComInstituicao(model.Escolaridade))
            {
                if (Regex.IsMatch(curso, @"(\w*\s?)*"))
                    return true;
                return false;
            }
            return true;
        }

        private bool SerIdCidadeVaziaOuNumericaEValida(Models.ModelAjaxQuintaTela model, int? idCidade)
        {
            TAB_Cidade objCidade;
            var retorno = !EscolaridadeComInstituicao(model.Escolaridade) ||
                (idCidade.HasValue && idCidade != 0 && BLL.Cidade.CarregarPorId(idCidade.Value, out objCidade));

            return retorno;
        }

        private bool EscolaridadeComSituacao(int idEscolaridade)
        {
            return idEscolaridade == 6 || idEscolaridade == 8 || idEscolaridade == 10 || idEscolaridade == 11; /*curso imcompleto*/
        }

        private bool EscolaridadeComAnoConclusao(int idEscolaridade)
        {
            return idEscolaridade == 7 || idEscolaridade == 9 || idEscolaridade == 12 || idEscolaridade == 13; /*curso concluído*/
        }

        private bool EscolaridadeComInstituicao(int idEscolaridade)
        {
            return idEscolaridade >= 8 /*Técnico incompleto*/ && idEscolaridade <= 13 /*Superior completo*/;
        }
    }
}