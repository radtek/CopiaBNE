using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bne.Web.Services.API.DTO
{
    /// <summary>
    /// Representa uma vaga de emprego no BNE com informações extras que devem ser exibidas somente para a empresa anunciante.
    /// </summary>
    public class VagaEmpresa : Vaga
    {
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
        /// (Opcional) Lista de objetos do tipo Pergunta.
        /// </summary>
        /// <seealso cref="Bne.Web.Services.API.DTO.Pergunta"/>
        public new List<PerguntaEmpresa> Perguntas { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public VagaEmpresa() { }

        /// <summary>
        /// Contrutor para uma instancia baseado em uma instância BNE.BLL.Vaga
        /// </summary>
        /// <param name="objVaga">Instância com as informações para o novo objeto</param>
        public VagaEmpresa(BNE.BLL.Vaga objVaga)
            : base(objVaga)
        {
            Email = objVaga.EmailVaga;
            IdadeMax = objVaga.NumeroIdadeMaxima;
            IdadeMin = objVaga.NumeroIdadeMinima;
            NumDDD = objVaga.NumeroDDD;
            Telefone = objVaga.NumeroTelefone;
            ReceberCadaCV = objVaga.FlagReceberCadaCV.HasValue && objVaga.FlagReceberCadaCV.Value;
            ReceberTodosCV = objVaga.FlagReceberTodosCV.HasValue && objVaga.FlagReceberTodosCV.Value;
            if (objVaga.Sexo != null)
            {
                if (string.IsNullOrEmpty(objVaga.Sexo.DescricaoSexo)) objVaga.Sexo.CompleteObject();
                Sexo = objVaga.Sexo.DescricaoSexo;
            }

            var perguntas = BNE.BLL.VagaPergunta.RecuperarListaPerguntas(objVaga.IdVaga, null);
            Perguntas = new List<PerguntaEmpresa>();
            foreach (var vagaPergunta in perguntas)
            {
                Perguntas.Add(new PerguntaEmpresa()
                {
                    IdPergunta = vagaPergunta.IdVagaPergunta,
                    Texto = vagaPergunta.DescricaoVagaPergunta,
                    Resposta = (vagaPergunta.FlagResposta.HasValue && vagaPergunta.FlagResposta.Value) ? "Sim" : "Não"
                });
            }
            
        }
    }
}