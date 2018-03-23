using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Model
{
    public class SistemaCliente
    {
        public Guid Chave
        {
            get;
            set;
        }

        [Required(ErrorMessage="Nome do Sistema não pode ser vazio")]
        //[IsUnique("ApiGateway.SistemaCliente", "Nome", ErrorMessage="O Nome do Sistema deve ser único.")]
        public string Nome
        {
            get;
            set;
        }

        public List<Api> Apis
        {
            get;
            set;
        }

        public List<Header> Headers
        {
            get;
            set;
        }
    }
}
