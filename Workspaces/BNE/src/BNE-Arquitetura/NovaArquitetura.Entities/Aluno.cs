using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NovaArquitetura.Entities
{
    public class Aluno
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource.Resource))]
        [MaxLength(500)]
        public string Nome { get; set; }

        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        
        [JsonIgnore]
        [IgnoreDataMember] 
        public virtual ICollection<Disciplina> Disciplinas { get; set; }

    }
}
