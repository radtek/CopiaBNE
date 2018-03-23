using System;
using System.Collections.Generic;
using System.Linq;
using BNE.BLL.Custom;

namespace BNE.BLL.DTO
{
    public class CurriculoRastreador
    {
        public int IdCurriculo { get; set; }
        public string NomeCompleto { get; set; }
        public string PrimeiroNome { get { return PessoaFisica.RetornarPrimeiroNome(NomeCompleto); } }
        public int Idade { get; set; }
        public string NomeCidade { get; set; }
        public string SiglaEstado { get; set; }
        public string NomeFuncaoPretendida { get; set; }
        public List<FuncaoPretendida> FuncoesPretendidas { get; set; }

        public void MapearCurriculo(Solr.CurriculoRastreador.Response curriculo)
        {
            IdCurriculo = curriculo.Idf_Curriculo;
            NomeCompleto = curriculo.Nme_Pessoa;
            Idade = Helper.CalcularIdade(curriculo.Dta_Nascimento);
            NomeCidade = curriculo.Nme_Cidade;
            SiglaEstado = curriculo.Sig_Estado;
            NomeFuncaoPretendida = curriculo.Des_Funcao.FirstOrDefault();


            // public string PrimeiroNome { get { return PessoaFisica.RetornarPrimeiroNome(NomeCompleto); } }

            //NumeroCPF = curriculo.Num_CPF,
            //
            //DataNascimento = curriculo.Dta_Nascimento,
            //VIP = curriculo.Flg_VIP,
            //AceitaEstagio = curriculo.Flg_CurriculoAceitaEstagio,
            //DataAtualizacaoCurriculo = curriculo.Dta_Atualizacao,
            //Sexo = curriculo.Des_Sexo,
            //EstadoCivil = curriculo.Des_Estado_Civil,
            //TemFilhos = curriculo.Flg_Filhos,
            //UltimaFormacaoAbreviada = curriculo.RecuperarSiglaMaiorFormacao(),
            //ValorPretensaoSalarial = curriculo.Vlr_Pretensao_Salarial,
            //Logradouro = curriculo.Des_Logradouro,
            //NumeroCEP = curriculo.Num_CEP,
            //Bairro = curriculo.Des_Bairro,

            //NumeroDDDCelular = curriculo.Num_DDD_Celular,
            //NumeroCelular = curriculo.Num_Celular,
            //NomeOperadoraCelular = curriculo.Nme_Operadora_Celular,
            //URLImagemOperadoraCelular = curriculo.Des_URL_Logo,
            //NumeroDDDTelefone = curriculo.Num_DDD_Telefone,
            //NumeroTelefone = curriculo.Num_Telefone,
            //CategoriaHabilitacao = curriculo.Des_Categoria_Habilitacao,
            //Observacao = curriculo.Obs_Curriculo,
            //OutrosConhecimentos = curriculo.Des_Conhecimento,
            //Altura = curriculo.Num_Altura,
            //Peso = curriculo.Num_Peso,
            //Raca = curriculo.Des_Raca,
            //DisponibilidadeViajar = curriculo.Flg_Viagem,
            //Deficiencia = curriculo.Des_Deficiencia,
            //UltimaFormacaoCompleta = curriculo.Des_Escolaridade
            //};

            //return curriculoRetorno;
            //if (carregaEmail)
            //    curriculoRetorno.Email = curriculo.Eml_Pessoa;

            FuncoesPretendidas = new List<DTO.FuncaoPretendida>();
            if (curriculo.Des_Funcao != null)
            {
                for (int i = 0; i < curriculo.Des_Funcao.Count; i++)
                {
                    FuncoesPretendidas.Add(new DTO.FuncaoPretendida
                    {
                        NomeFuncaoPretendida = curriculo.Des_Funcao[i],
                        QuantidadeExperiencia = (short?)(curriculo.Qtd_Experiencia != null ? curriculo.Qtd_Experiencia[i] : 0)
                    });

                }
            }

            //curriculoRetorno.Idiomas = new List<DTO.Idioma>();
            //if (curriculo.Des_Idioma != null)
            //{
            //    for (int i = 0; i < curriculo.Des_Idioma.Count; i++)
            //    {
            //        curriculoRetorno.Idiomas.Add(new DTO.Idioma
            //        {
            //            DescricaoIdioma = curriculo.Des_Idioma[i],
            //            NivelIdioma = curriculo.Des_Nivel_Idioma != null ? curriculo.Des_Nivel_Idioma[i] : null
            //        });
            //    }
            //}

            //curriculoRetorno.Formacoes = new List<DTO.Formacao>();
            //curriculoRetorno.Cursos = new List<DTO.Formacao>();
            //if (curriculo.Idf_Grau_Escolaridade != null)
            //{
            //    for (int i = 0; i < curriculo.Idf_Grau_Escolaridade.Count; i++)
            //    {
            //        var formacao = new DTO.Formacao
            //        {
            //            DescricaoFormacao = curriculo.Des_Escolaridade_Formacao[i],
            //            DescricaoCurso = curriculo.Des_Curso[i],
            //            SiglaFonte = curriculo.Sig_Fonte != null ? curriculo.Sig_Fonte[i] : string.Empty,
            //            NomeFonte = curriculo.Des_Fonte != null ? curriculo.Des_Fonte[i] : string.Empty,
            //            AnoConclusao = curriculo.Ano_Conclusao[i] != null && curriculo.Ano_Conclusao[i] != "0" ? Convert.ToInt16(curriculo.Ano_Conclusao[i]) : (Int16?)null,
            //            SituacaoFormacao = curriculo.Des_Situacao_Formacao != null && curriculo.Des_Situacao_Formacao[i] != null ? curriculo.Des_Situacao_Formacao[i] : string.Empty,
            //            Periodo = curriculo.Num_Periodo != null && curriculo.Num_Periodo[i] != -1 ? curriculo.Num_Periodo[i].ToString() : string.Empty
            //        };

            //        if (curriculo.Idf_Grau_Escolaridade[i] != 5)
            //            curriculoRetorno.Formacoes.Add(formacao);
            //        else
            //            curriculoRetorno.Cursos.Add(formacao);
            //    }
            //}

            //curriculoRetorno.Experiencias = new List<DTO.ExperienciaProfissional>();
            //if (curriculo.Raz_Social != null)
            //{
            //    for (int i = 0; i < 5; i++)
            //    {
            //        if (curriculo.Raz_Social.Count > i && curriculo.Raz_Social[i] != null)
            //        {
            //            curriculoRetorno.Experiencias.Add(new DTO.ExperienciaProfissional
            //            {
            //                RazaoSocial = curriculo.Raz_Social[i],
            //                DataAdmissao = curriculo.Dta_Admissao[i],
            //                DataDemissao = curriculo.Dta_Demissao[i] != null && curriculo.Dta_Demissao[i].ToShortDateString() != "01/01/1900" ? Convert.ToDateTime(curriculo.Dta_Demissao[i]) : (DateTime?)null,
            //                Funcao = curriculo.Des_Funcao_Exercida[i],
            //                DescricaoAtividade = curriculo.Des_Atividade[i],
            //                AreaBNE = curriculo.Des_Atividade_empresa[i],
            //                VlrSalario = curriculo.Vlr_Ultimo_Salario > 0 ? curriculo.Vlr_Ultimo_Salario : (decimal?)null
            //            });
            //        }
            //    }
            //}

            //curriculoRetorno.DisponibilidadesTrabalho = new List<DisponibilidadeTrabalho>();
            //if (curriculo.Des_Disponibilidade != null)
            //{
            //    for (int i = 0; i < curriculo.Des_Disponibilidade.Count; i++)
            //    {
            //        curriculoRetorno.DisponibilidadesTrabalho.Add(new DTO.DisponibilidadeTrabalho
            //        {
            //            Descricao = curriculo.Des_Disponibilidade[i]
            //        });
            //    }
            //}

            //curriculoRetorno.Veiculos = new List<Veiculo>();
            //if (curriculo.Des_Tipo_Veiculo != null)
            //{
            //    for (int i = 0; i < curriculo.Des_Tipo_Veiculo.Count; i++)
            //    {
            //        curriculoRetorno.Veiculos.Add(new DTO.Veiculo
            //        {
            //            Modelo = curriculo.Des_Modelo != null && curriculo.Des_Modelo.Count > i ? curriculo.Des_Modelo[i] : string.Empty,
            //            Tipo = curriculo.Des_Tipo_Veiculo[i],
            //            Ano = curriculo.Ano_Veiculo != null && curriculo.Ano_Veiculo.Count > i ? curriculo.Ano_Veiculo[i] : string.Empty
            //        });
            //    }
            //}

            //return curriculoRetorno;
        }
    }
}
