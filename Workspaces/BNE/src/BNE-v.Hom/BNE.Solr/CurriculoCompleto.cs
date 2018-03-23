using System;
using System.Collections.Generic;
using System.Linq;

namespace BNE.Solr
{
    public class CurriculoCompleto : SolrResponse<CurriculoCompleto.Response>
    {
        public class Response : Docs
        {
            public int Idf_Curriculo { get; set; }
            public decimal Num_CPF { get; set; }
            public string Nme_Pessoa { get; set; }
            public string Eml_Pessoa { get; set; }
            public string Num_DDD_Telefone { get; set; }
            public string Num_Telefone { get; set; }
            public string Num_DDD_Celular { get; set; }
            public string Num_Celular { get; set; }
            public string Nme_Operadora_Celular { get; set; }
            public string Des_URL_Logo { get; set; }
            public int Idf_Cidade { get; set; }
            public string Nme_Cidade { get; set; }
            public string Sig_Estado { get; set; }
            public string Geo_Localizacao { get; set; }
            public string Num_CEP { get; set; }
            public string Des_Bairro { get; set; }
            public string Des_Logradouro { get; set; }
            public int Idf_Situacao_Curriculo { get; set; }
            public Boolean Flg_VIP { get; set; }
            public Boolean Flg_CurriculoAceitaEstagio { get; set; }
            public decimal Vlr_Pretensao_Salarial { get; set; }
            public decimal Vlr_Ultimo_Salario { get; set; }
            public DateTime Dta_Cadastro { get; set; }
            public DateTime Dta_Atualizacao { get; set; }
            public DateTime Dta_Nascimento { get; set; }
            public string Sig_Sexo { get; set; }
            public string Des_Sexo { get; set; }
            public int Idf_Sexo { get; set; }
            public int Idf_Escolaridade { get; set; }
            public string Des_Escolaridade { get; set; }
            public string Des_Abreviada { get; set; }
            public int Idf_Deficiencia { get; set; }
            public string Des_Deficiencia { get; set; }
            public int Idf_Estado_Civil { get; set; }
            public string Des_Estado_Civil { get; set; }
            public Boolean? Flg_Filhos { get; set; }
            public int Idf_Raca { get; set; }
            public string Des_Raca { get; set; }
            public int Idf_Categoria_Habilitacao { get; set; }
            public string Des_Categoria_Habilitacao { get; set; }
            public int Num_Idade { get; set; }
            public decimal Num_Altura { get; set; }
            public decimal Num_Peso { get; set; }
            public string Obs_Curriculo { get; set; } //Observações
            public string Des_Conhecimento { get; set; } //Outros conhecimentos
            public int Total_Experiencia { get; set; }
            public string Nme_Anexo { get; set; }
            public Boolean? Flg_Viagem { get; set; }

            public List<int> Idf_Funcao_Pretendida { get; set; }
            public List<int> Idf_Funcao { get; set; }
            public List<string> Des_Funcao { get; set; }
            public List<int> Idf_Funcao_Agrupadora { get; set; }
            public List<string> Des_Funcao_Agrupadora { get; set; }
            public List<int> Idf_Area_BNE { get; set; }
            public List<string> Des_Area_BNE { get; set; }
            public List<short> Qtd_Experiencia { get; set; }
            public List<string> Raz_Social { get; set; }
            public List<string> Des_Funcao_Exercida { get; set; }
            public List<DateTime> Dta_Admissao { get; set; }
            public List<DateTime> Dta_Demissao { get; set; }
            public List<string> Des_Atividade { get; set; }
            public List<string> Des_Atividade_empresa { get; set; }
            public List<int> Idf_Area_BNE_Atividade_empresa { get; set; }
            public List<decimal> Vlr_Salario { get; set; }
            public List<int> Idf_Disponibilidade { get; set; }
            public List<string> Des_Disponibilidade { get; set; }
            public List<int> Idf_Idioma { get; set; }
            public List<string> Des_Idioma { get; set; }
            public List<string> Des_Nivel_Idioma { get; set; }
            public List<int> Idf_Formacao { get; set; }
            public List<int> Idf_Escolaridade_Formacao { get; set; }
            public List<string> Des_Escolaridade_Formacao { get; set; }
            public List<string> Des_Situacao_Formacao { get; set; }
            public List<int> Idf_Curso { get; set; }
            public List<string> Des_Curso { get; set; }
            public List<int> Idf_Fonte { get; set; }
            public List<string> Des_Fonte { get; set; }
            public List<string> Sig_Fonte { get; set; }
            public List<string> Ano_Conclusao { get; set; }
            public List<int> Qtd_Carga_Horaria { get; set; }
            public List<int> Num_Periodo { get; set; }
            public List<int> Idf_Situacao_Formacao { get; set; }
            public List<string> Des_Endereco { get; set; }
            public List<int> Idfs_Cidades { get; set; }
            public List<int> Idfs_Funcoes { get; set; }
            public List<int> Idf_Tipo_Veiculo { get; set; }
            public List<string> Des_Tipo_Veiculo { get; set; }
            public List<string> Des_Modelo { get; set; }
            public List<string> Ano_Veiculo { get; set; }
            public List<int> Idfs_Origens { get; set; }
            public List<int> Idf_Grau_Escolaridade { get; set; }
            public List<string> Des_Grau_Escolaridade { get; set; }

            public int Idf_Curriculo_Classificacao { get; set; }
            public int Idf_Usuario_Filial_Perfil { get; set; }
            public string Des_Observacao { get; set; } //Observação da filial
            public int Idf_Avaliacao { get; set; }
            public string Des_Mensagem { get; set; }
            public bool Dentro_Perfil { get; set; }//utilizado nos candidatos da vaga.
            public bool Flg_Auto_Candidatura { get; set; }

            public Boolean? Flg_WhatsApp { get; set; }

            public void RecuperarMaiorFormacao(out int idMaiorEscolaridade, out string descricaoEscolaridade, out int? identificadorGrauEscolaridadeFormacao, out int? identificadorEscolaridadeFormacao, out string descricaoGrauEscolaridadeFormacao, out string descricaoCursoFormacao, out string descricaoFonteFormacao, out string dataConclusaoFormacao)
            {
                identificadorGrauEscolaridadeFormacao = null;
                identificadorEscolaridadeFormacao = null;
                descricaoGrauEscolaridadeFormacao = string.Empty;
                descricaoCursoFormacao = string.Empty;
                descricaoFonteFormacao = string.Empty;
                dataConclusaoFormacao = string.Empty;

                if (Idf_Grau_Escolaridade != null)
                {
                    identificadorGrauEscolaridadeFormacao = Idf_Grau_Escolaridade.Where(ge => ge != 5).OrderByDescending(ob => ob).FirstOrDefault();
                    if (identificadorGrauEscolaridadeFormacao.HasValue && identificadorGrauEscolaridadeFormacao > 0)
                    {
                        //Unindo grau de escolaridade e escolaridade para identificar a maior escolaridade
                        var tuple = Idf_Grau_Escolaridade.Zip(Idf_Escolaridade_Formacao, Tuple.Create);
                        //Recuperando a maior escolaridade
                        int? formacao = identificadorGrauEscolaridadeFormacao;
                        int maiorEscolaridade = tuple.Where(s => s.Item1 == formacao).OrderByDescending(ob => ob.Item2).First().Item2;
                        //Recuperando o índice da sequencia
                        int index = Idf_Escolaridade_Formacao.IndexOf(maiorEscolaridade);

                        if (Idf_Escolaridade_Formacao != null)
                            identificadorEscolaridadeFormacao = Idf_Escolaridade_Formacao[index];

                        if (Des_Grau_Escolaridade != null)
                            descricaoGrauEscolaridadeFormacao = Des_Grau_Escolaridade[index];

                        if (Des_Curso != null)
                            descricaoCursoFormacao = Des_Curso[index];

                        if (Des_Fonte != null)
                            descricaoFonteFormacao = Des_Fonte[index];

                        if (Ano_Conclusao != null && Ano_Conclusao[index] != "0")
                            dataConclusaoFormacao = Ano_Conclusao[index];
                    }
                }

                if (identificadorEscolaridadeFormacao.HasValue && identificadorEscolaridadeFormacao > Idf_Escolaridade)
                {
                    idMaiorEscolaridade = identificadorEscolaridadeFormacao.Value;
                    descricaoEscolaridade = descricaoGrauEscolaridadeFormacao;
                }
                else
                {
                    idMaiorEscolaridade = Idf_Escolaridade;
                    descricaoEscolaridade = Des_Abreviada;
                }
            }

            public string RecuperarSiglaMaiorFormacao()
            {
                int idMaiorEscolaridade;
                string descricaoMaiorEscolaridade, descricaoGrauEscolaridadeFormacao, descricaoCursoFormacao, descricaoFonteFormacao, dataConclusaoFormacao;
                int? identificadorGrauEscolaridadeFormacao, identificadorEscolaridadeFormacao;

                RecuperarMaiorFormacao(out idMaiorEscolaridade, out descricaoMaiorEscolaridade, out identificadorGrauEscolaridadeFormacao, out identificadorEscolaridadeFormacao, out descricaoGrauEscolaridadeFormacao, out descricaoCursoFormacao, out descricaoFonteFormacao, out dataConclusaoFormacao);

                return descricaoMaiorEscolaridade;
            }

        }
    }
}
