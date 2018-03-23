using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL.Entity
{
    public sealed class QuintaTela
    {
        #region Propriedades
        public int Escolaridade { get; private set; }
        public string InstituicaoEnsino { get; private set; }
        public int? IdFonte { get; private set; } // o catalogo de instituicoes de ensino no BNE estao na tabela Fonte
        public string NomeCurso { get; private set; }
        public string Cidade { get; private set; }
        public int? IdCidade { get; private set; }
        public int? Periodo { get; private set; }
        public int? Situacao { get; private set; }
        public int? AnoConclusao { get; private set; }

        public IList<ExperienciaProfissional> ExperienciaProfissional { get; private set; }
        #endregion

        public QuintaTela(Models.ModelAjaxQuintaTela model)
        {
            ExperienciaProfissional =
                new List<ExperienciaProfissional>();

            Escolaridade = model.Escolaridade;
            InstituicaoEnsino = model.InstituicaoEnsino;
            IdFonte = model.Fonte <= 0 ? null : (int?)model.Fonte;
            NomeCurso = model.NomeCurso;
            Cidade = model.Cidade;
            IdCidade = model.IdCidade <= 0 ? null : (int?)model.IdCidade;
            Periodo = model.Periodo <= 0 ? null : (int?)model.Periodo;
            Situacao = model.Situacao <= 0 ? null : (int?)model.Situacao;
            AnoConclusao = model.AnoConclusao <= 0 ? null : (int?)model.AnoConclusao;

            VerificarConsistenciaGeral();
            VerificarFonte();
            VerificarCidade();

            if (!String.IsNullOrEmpty(model.NomeEmpresa1))
            {
                DateTime? d = null;
                DateTime dummy;
                if (DateTime.TryParse(model.DataDemissao1, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal, out dummy))
                    d = dummy;

                Decimal salario;
                Decimal.TryParse(model.UltimoSalario, NumberStyles.Number, CultureInfo.GetCultureInfo("pt-br"), out salario);

                var ep = new ExperienciaProfissional
                {
                    DescricaoFuncao = model.FuncaoExercida1,
                    NomeEmpresa = model.NomeEmpresa1,
                    AreaBNE = model.AreaEmpresa1,
                    DataAdmissao = Convert.ToDateTime(model.DataAdmissao1, CultureInfo.GetCultureInfo("pt-br")),
                    DataDemissao = d,
                    UltimoSalario = salario,
                    Atribuicoes = model.Atribuicoes1
                };


                ExperienciaProfissional.Add(ep);
            }

            if (!String.IsNullOrEmpty(model.NomeEmpresa2))
            {
                DateTime d;
                DateTime.TryParse(model.DataDemissao2, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal,
                    out d);

                var ep = new ExperienciaProfissional
                {
                    DescricaoFuncao = model.FuncaoExercida2,
                    NomeEmpresa = model.NomeEmpresa2,
                    AreaBNE = model.AreaEmpresa2,
                    DataAdmissao = Convert.ToDateTime(model.DataAdmissao2, CultureInfo.GetCultureInfo("pt-br")),
                    DataDemissao = d,
                    UltimoSalario = null,
                    Atribuicoes = model.Atribuicoes2
                };

                ExperienciaProfissional.Add(ep);
            }

            if (!String.IsNullOrEmpty(model.NomeEmpresa3))
            {
                DateTime d;
                DateTime.TryParse(model.DataDemissao3, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal, out d);

                var ep = new ExperienciaProfissional
                {
                    DescricaoFuncao = model.FuncaoExercida3,
                    NomeEmpresa = model.NomeEmpresa3,
                    AreaBNE = model.AreaEmpresa3,
                    DataAdmissao = Convert.ToDateTime(model.DataAdmissao3, CultureInfo.GetCultureInfo("pt-br")),
                    DataDemissao = d,
                    UltimoSalario = null,
                    Atribuicoes = model.Atribuicoes3
                };

                ExperienciaProfissional.Add(ep);
            }
        }

        private bool EscolaridadeComInstituicao()
        {
            int idEscolaridade = this.Escolaridade;

            return idEscolaridade >= 8 /*Técnico incompleto*/ && idEscolaridade <= 13 /*Superior completo*/;
        }

        private bool EscolaridadeComAnoConclusao()
        {
            int idEscolaridade = this.Escolaridade;

            return idEscolaridade == 7 || idEscolaridade == 9 || idEscolaridade == 11 || idEscolaridade == 13; /*curso concluído*/
        }

        private bool EscolaridadeComSituacao()
        {
            int idEscolaridade = this.Escolaridade;

            return idEscolaridade == 6 || idEscolaridade == 8 || idEscolaridade == 10 || idEscolaridade == 11; /*curso imcompleto*/
        }

        private bool VerificarConsistenciaGeral()
        {
            bool retorno = true;

            if (EscolaridadeComAnoConclusao() && Situacao.HasValue)
            {
                // apaga situacao
                retorno = false;
                Situacao = null;
            }

            if (EscolaridadeComSituacao() && AnoConclusao.HasValue)
            {
                // apaga ano conclusao
                retorno = false;
                AnoConclusao = null;
            }

            if (!EscolaridadeComInstituicao() &&
                (!String.IsNullOrEmpty(InstituicaoEnsino) || IdFonte.HasValue || !String.IsNullOrEmpty(NomeCurso) || !String.IsNullOrEmpty(Cidade) ||
                IdCidade.HasValue))
            {
                retorno = false;

                // apaga conteudo referente a instituicao de ensino
                InstituicaoEnsino = null;
                IdFonte = null;
                NomeCurso = null;
                Cidade = null;
                IdCidade = null;
            }

            return retorno;
        }

        private bool VerificarFonte()
        {
            bool retorno = false;

            IEnumerable<TAB_Fonte> objFontes;
            if (BLL.Fonte.CarregarPorDescricaoOuSigla(InstituicaoEnsino, out objFontes) && objFontes != null && objFontes.Count() == 1)
            {
                TAB_Fonte objFonte = objFontes.First();

                retorno = true;

                if (IdFonte.HasValue && IdFonte.Value != objFonte.Idf_Fonte)
                {
                    IdFonte = null;
                    retorno = false;
                }
            }

            return retorno;
        }

        private bool VerificarCidade()
        {
            bool retorno = false;

            IEnumerable<TAB_Cidade> objCidades;
            if (BLL.Cidade.CarregarPorDescricao(Cidade, out objCidades) && objCidades != null && objCidades.Count() == 1)
            {
                TAB_Cidade objCidade = objCidades.First();

                retorno = true;

                if (IdCidade.HasValue && IdCidade.Value != objCidade.Idf_Cidade)
                {
                    IdCidade = null;
                    retorno = false;
                }
            }

            return retorno;
        }
    }
}