using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BNE.BLL
{
    public class ResultadoBuscaCVSolr
    {
        public ResponseHeader responseHeader { get; set; }
        public Response response { get; set; }
        public Facet facet_counts { get; set; }
        public Stats stats { get; set; }
    }

    public class Facet
    {
        public Dictionary<String, List<object>> facet_fields { get; set; }
    }

    public class ResponseHeader
    {
        public int status { get; set; }
        public int QTime { get; set; }
        [JsonProperty(PropertyName = "params")]
        public Parameters param { get; set; }
    }

    public class Parameters
    {
        public string q { get; set; }
        public string start { get; set; }
        public string rows { get; set; }
    }

    public class Response
    {
        public int numFound { get; set; }
        public int start { get; set; }
        public decimal maxScore { get; set; }
        public List<CvsSolr> docs { get; set; }
    }


    public class Stats
    {
        public StatsFields stats_fields { get; set; }
    }
    public class StatsFields
    {
        public VlrPretensaoSalarial Vlr_Pretensao_Salarial { get; set; }
    }
    public class VlrPretensaoSalarial
    {
        public decimal mean { get; set; }
    }
    
    public class CvsSolr
    {
        public int Idf_Curriculo { get; set; }
        public string Num_CPF { get; set; }
        public string Nme_Pessoa { get; set; }
        public string Eml_Pessoa { get; set; }
        public string Num_DDD_Telefone { get; set; }
        public string Num_Telefone { get; set; }
        public string Num_DDD_Celular { get; set; }
        public string Num_Celular { get; set; }
        public int Idf_Cidade { get; set; }
        public string Nme_Cidade { get; set; }
        public string Sig_Estado { get; set; }
        public string Geo_Localizacao { get; set; }
        public string Num_CEP { get; set; }
        public string Des_Bairro { get; set; }
        public string Des_Logradouro { get; set; }
        public int Idf_Situacao_Curriculo { get; set; }
        public Boolean Flg_VIP { get; set; }
        public decimal Vlr_Pretensao_Salarial { get; set; }
        public decimal Vlr_Ultimo_Salario { get; set; }        
        public DateTime Dta_Cadastro { get; set; }
        public DateTime Dta_Atualizacao { get; set; }
        public DateTime Dta_Nascimento { get; set; }
        public string Sig_Sexo { get; set; }
        public int Idf_Sexo { get; set; }
        public int Idf_Escolaridade { get; set; }
        public string Des_Escolaridade { get; set; }
        public string Des_Abreviada { get; set; }        
        public int Idf_Deficiencia { get; set; }
        public string Des_Deficiencia { get; set; }
        public int Idf_Estado_Civil { get; set; }
        public string Des_Estado_Civil { get; set; }
        public Boolean Flg_Filhos { get; set; }
        public int Idf_Raca { get; set; }
        public string Des_Raca { get; set; }
        public int Idf_Categoria_Habilitacao { get; set; }
        public string Des_Categoria_Habilitacao { get; set; }        
        public int Num_Idade { get; set; }
        public float Num_Altura { get; set; }
        public float Num_Peso { get; set; }
        public string Obs_Curriculo { get; set; }
        public int Total_Experiencia { get; set; }
        public string Nme_Anexo { get; set; }

        public List<int> Idf_Funcao_Pretendida { get; set; }
        public List<int> Idf_Funcao { get; set; }
        public List<string> Des_Funcao { get; set; }
        public List<int> Idf_Funcao_Agrupadora { get; set; }
        public List<string> Des_Funcao_Agrupadora { get; set; }
        public List<int> Idf_Area_BNE { get; set; }
        public List<string> Des_Area_BNE { get; set; }
        public List<int> Qtd_Experiencia { get; set; }
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
        public List<int> Idf_Formacao { get; set; }
        public List<int> Idf_Escolaridade_Formacao { get; set; }
        public List<string> Des_Escolaridade_Formacao { get; set; }
        public List<int> Idf_Curso { get; set; }
        public List<string> Des_Curso { get; set; }
        public List<int> Idf_Fonte { get; set; }
        public List<string> Des_Fonte { get; set; }
        public List<string> Ano_Conclusao { get; set; }
        public List<int> Qtd_Carga_Horaria { get; set; }
        public List<int> Num_Periodo { get; set; }
        public List<int> Idf_Situacao_Formacao { get; set; }
        public List<string> Des_Endereco { get; set; }
        public List<int> Idfs_Cidades { get; set; }
        public List<int> Idfs_Funcoes { get; set; }
        public List<int> Idf_Tipo_Veiculo { get; set; }
        public List<string> Des_Tipo_Veiculo { get; set; }
        public List<int> Idfs_Origens { get; set; }
        public List<int> Idf_Grau_Escolaridade { get; set; }
        public List<string> Des_Grau_Escolaridade { get; set; }

        public int Idf_Curriculo_Classificacao { get; set; }
        public int Idf_Usuario_Filial_Perfil { get; set; }
        public string Des_Observacao { get; set; }
        public int Idf_Avaliacao { get; set; }
        public string Des_Mensagem { get; set; }


        public decimal score { get; set; }
    }   

}
