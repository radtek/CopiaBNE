using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    /// <summary>
    /// Representa uma vaga de emprego no BNE.
    /// </summary>
    public class Vaga
    {
        /// <summary>
        /// (Obrigatório) Lista que deve ser composta pelos seguintes valores.
        /// -> Aprendiz
        /// -> Autônomo
        /// -> Efetivo
        /// -> Estágio
        /// -> Freelancer
        /// -> Temporário
        /// </summary>
        public List<string> TipoVinculo { get; set; }

        /// <summary>
        /// (Obrigatório) Nome completo da função.
        /// </summary>
        [Required(ErrorMessage = "A função da vaga é obrigatória.")]
        [StringLength(200, MinimumLength = 4, ErrorMessage = "A função deve ter no mínimo 4 caracteres e no máximo 200.")]
        public string Funcao { get; set; }


        /// <summary>
        /// (Obrigatório) Nome completo da cidade seguido de barra mais a sigla do estado. Ex.:”Montes Claros/MG”.
        /// </summary>
        [Required(ErrorMessage = "A cidade da vaga é obrigatória.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "A cidade deve ter no mínimo 5 caracteres e no máximo 200.")]
        public string Cidade { get; set; }

        /// <summary>
        /// (Obrigatório) Número de vagas ofertadas.
        /// </summary>
        //[Required(ErrorMessage = "A quantidade de vagas é obrigatória.")]
        //[Range(1, Double.MaxValue, ErrorMessage = "O valor mínimo para a quantidade é 1.")]
        public short? Quantidade { get; set; }

        /// <summary>
        /// Algum dos itens listados:
        /// -> Ensino Fundamental Incompleto
        /// -> Ensino Fundamental Completo
        /// -> Ensino Médio Incompleto
        /// -> Ensino Médio Completo
        /// -> Técnico/Pós-Médio Incompleto
        /// -> Técnico/Pós-Médio Completo
        /// -> Tecnólogo Incompleto
        /// -> Superior Incompleto
        /// -> Tecnólogo Completo
        /// -> Superior Completo
        /// -> Pós Graduação / Especialização
        /// -> Mestrado
        /// -> Doutorado
        /// </summary>
        public string Escolaridade { get; set; }

        /// <summary>
        /// (Opcional) Início da faixa salarial ofertada.
        /// </summary>
        public decimal? SalarioMin { get; set; }

        /// <summary>
        /// (Opcional) Final da faixa salarial ofertada.
        /// </summary>
        public decimal? SalarioMax { get; set; }

        /// <summary>
        /// (Opcional) Descrição dos benefícios oferecidos.
        /// </summary>
        public string Beneficios { get; set; }

        /// <summary>
        /// (Opcional) Faixa etária mínima requerida para vaga.
        /// </summary>
        public short? IdadeMin { get; set; }

        /// <summary>
        /// (Opcional) Faixa etária máxima requerida para a vaga.
        /// </summary>
        public short? IdadeMax { get; set; }

        /// <summary>
        /// (Requerido) Sexo requerido pela vaga "Masculino", "Feminino" ou  "Qualquer".
        /// </summary>
        //[Required(ErrorMessage = "Informe o sexo: \"Masculino\", \"Feminino\" ou  \"Qualquer\"")]
        public string Sexo { get; set; }

        /// <summary>
        /// (Opcional) Requisitos desejados para a vaga.
        /// </summary>
        public string Requisitos { get; set; }

        /// <summary>
        /// (Opcional) Atribuições desejadas para a vaga.
        /// </summary>
        public string Atribuicoes { get; set; }

        /// <summary>
        /// (Opcional) Utilize os itens listados para compor a lista:
        /// -> Manhã
        /// -> Tarde
        /// -> Noite
        /// -> Sábado
        /// -> Domingo
        /// -> Viagem
        ///</summary>
        public List<string> Disponibilidade { get; set; }


        /// <summary>
        /// Nome fantasia da empresa.
        /// </summary>
        //[Required(ErrorMessage = "Nome fantasia é obrigatório.")]
        public string NomeFantasia { get; set; }

        /// <summary>
        /// Número de DDD do telefone.
        /// </summary>
        //[Required(ErrorMessage = "Número de DDD é obrigatório.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O DDD deve ter dois caracteres.")]
        public string NumDDD { get; set; }

        /// <summary>
        /// Número de telefone da empresa.
        /// </summary>
        //[Required(ErrorMessage = "Número de telefone é obrigatório.")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "O Número de telefone deve ter entre 8 e 9 caracteres.")]
        public string Telefone { get; set; }

        /// <summary>
        /// (Obrigatório) Indica se as informações da empresa são confidênciais.
        /// </summary>
        //[Required(ErrorMessage = "Não foi informado a confidencialidade da vaga.")]
        public bool Confidencial { get; set; }

        /// <summary>
        /// Email confidencial de retorno.
        /// </summary>
        //[Required(ErrorMessage = "O email confidencial de retorno é obrigatório.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "O email informádo é inválido.")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// (Obrigatório) Receber CV assim que o candidato se inscrever na vaga.
        /// </summary>
        public bool ReceberCadaCV { get; set; }

        /// <summary>
        /// (Obrigatório) Receber CV de todos os candidatos inscritos no final do dia. 
        /// </summary>
        public bool ReceberTodosCV { get; set; }

        /// <summary>
        /// (Opcional) Palavras chaves que auxiliarão na busca de vagas. Essas palavras devem ser separadas por vírgula.
        /// </summary>
        public string PalavrasChave { get; set; }

        /// <summary>
        /// (Opcional) Lista de objetos do tipo Pergunta.
        /// </summary>
        /// <seealso cref="Bne.Web.Services.API.DTO.Pergunta"/>
        public List<Pergunta> Perguntas { get; set; }

        /// <summary>
        /// (Opcional) Se a vaga é para PCD é necessário alguns dos itens:
        /// -> Auditiva
        /// -> Física
        /// -> Mental
        /// -> Múltipla
        /// -> Nenhuma
        /// -> Qualquer
        /// -> Reabilitado
        /// -> Visual
        /// </summary>
        public string Deficiencia { get; set; }

        public Vaga()
        {


        }
    }
}