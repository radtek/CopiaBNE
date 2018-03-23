using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    /// <summary>
    /// Classe com as informações para o cadastro de currículo
    /// </summary>
    [DataContract]
    public class CadastroCurriculo
    {
        /// <summary>
        /// Propriedade com as informações básicas do currículo
        /// </summary>
        [DataMember]
        public CadastroMiniCurriculo MiniCurriculo { get; set; }

        /// <summary>
        /// Propriedade com as informações de dados pessoais do currículo
        /// </summary>
        [DataMember]
        public DadosPessoais DadosPessoais { get; set; }

        /// <summary>
        /// Propriedade com as formações do currículo
        /// </summary>
        [DataMember]
        public FormacaoCurriculo Formacao { get; set; }

        /// <summary>
        /// Lista com as experiências profissionais do candidato. 
        /// Somente as 10 primeiras esperiências serão salvas. 
        /// A order das experiências deve ser indicada em ordem crescente de importancia (a de maior importância por primeiro).
        /// </summary>
        [DataMember]
        public CadastroExperienciaProfissional[] Experiencias { get; set; }

        /// <summary>
        /// Dados complementares do candidato
        /// </summary>
        [DataMember(Name = "DadosComplementares")]
        public DadosComplementares DadosComplementares { get; set; }
    }
}