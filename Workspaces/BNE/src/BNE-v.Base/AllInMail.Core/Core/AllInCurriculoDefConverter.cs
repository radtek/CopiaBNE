using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllInMail.Helper;
using System.Reflection;
using System.Text.RegularExpressions;
using AllInMail.Core;

namespace AllInMail.Core
{

    public class AllInCurriculoDefConverter : AllInDefConverterStd<AllInCurriculoTemplateModel>
    {
        private readonly string paramDefinied = "id_cadastro;Situacao_Curriculo;Tipo_Curriculo;cpf;nome;DDD_Celular;Celular;DDD_Telefone;telefone;dt_nascimento;dt_cadastro;dt_atualizacao;Dt_Modificacao;Flag_Vip;Dt_Inicio_Vip;Dt_Fim_Vip;nm_email;cep;Logradouro;cidade;uf;Pretensao_Salarial;idade;sexo;estado_civil;Habilitacao;Peso;Altura;Raca;Observacao_Curriculo;Conhecimento_Curriculo;Flag_Filhos;Flag_Viagens;Ultimo_Salario;Deficiencia;Disponibilidade;Funcao_Pretendida_01;FUNCAO_PRETENDIDA_02;Funcao_Pretendida_03;Experiencia_Razao_Social;EXPERIENCIA_ATIVIDADE;Experiencia_Dta_Adminissao;Experiencia_Dta_Demissao;Experiencia_Area_BNE;Experiencia_Funcao;Idioma_Nivel_01;Idioma_Nivel_02;Idioma_Nivel_03;Formacao_01_Descricao_BNE;FORMACAO_01_CURSO;FORMACAO_01_NOME_FONTE;FORMACAO_01_SIGLA_FONTE;FORMACAO_01_SITUACAO_FORMACAO;FORMACAO_01_ANO_CONCLUSAO;FORMACAO_01_NOME_PERIODO;FORMACAO_02_DESCRICAO_BNE;FORMACAO_02_CURSO;FORMACAO_02_NOME_FONTE;FORMACAO_02_SIGLA_FONTE;FORMACAO_02_SITUACAO_FORMACAO;FORMACAO_02_ANO_CONCLUSAO;FORMACAO_02_NOME_PERIODO;Qtd_Qm_Me_Viu";
        private string _delimiter = ";";
        private string _replaceDelimiter = "|";

        public override string Delimiter
        {
            get { return _delimiter; }
            set { _delimiter = value; }
        }
        public override string GetDeclaration()
        {
            return paramDefinied;
        }

        protected override IEnumerable<IDefFilter> InitTreatmentFilters()
        {
            yield return new DefPreFilterBaseFor<AllInCurriculoTemplateModel, int>(a => a.Idade, a => Math.Min(100, a));
        }

        protected override IEnumerable<DefPreParser> PreParsers()
        {
            yield return AllInParser.BoolFormat;
            yield return AllInParser.BoolNullableFormat;
            yield return AllInParser.DatePtBrFormat;
            yield return AllInParser.DatePrBrNullableFormat;
            yield return AllInParser.IntNullableFormat;
            yield return AllInParser.StringNullFormat;
        }

        protected override IEnumerable<DefFilterInit<object, string>> IntermediateFilters()
        {
            yield break;
        }

        protected override IEnumerable<DefPosParser> PosParsers()
        {
            yield return new DefPosParser(a => a.Length > 250 ? new string(a.Take(247).ToArray()) : a);
            yield return new DefPosParser(a => Regex.Replace(a, @"\r\n?|\n", " "));
            yield return new DefPosParser(a => a.Trim());
            yield return new DefPosParser(a => a.Replace(_delimiter, _replaceDelimiter));
        }

        protected override IEnumerable<DefPosFilter> PosFilters()
        {
            yield return new DefPosFilterFor<AllInCurriculoTemplateModel>(a => a.UF,
                                                                a => a.Length > 2 ? new string(a.Take(2).ToArray()) : a);

            yield return new DefPosFilterFor<AllInCurriculoTemplateModel>(a => a.Sexo)
                                                                .UseWith(a => new string(a.Take(1).ToArray()).ToLower())
                                                                .UseTooWith(a => a != "m" && a != "f" ? "" : a);

            yield return new DefPosFilterFor<AllInCurriculoTemplateModel>(a => a.Cep,
                                                                a => a.Length > 8 ? new string(a.Take(8).ToArray()) : a);

            yield return new DefPosFilterFor<AllInCurriculoTemplateModel>(a => a.Peso,
                                                                a => "0".EqualsEx(a) ? string.Empty : a);

            yield return new DefPosFilterFor<AllInCurriculoTemplateModel>(a => a.Altura,
                                                              a => "0".EqualsEx(a) ? string.Empty : a);
        }
    
    }


}
