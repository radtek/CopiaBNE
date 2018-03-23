using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Parceiros.Models
{
    public class Cadastro
    {
        public string NmePessoa  { get; set;}
        public string Celular  { get; set;}
        public string Email  { get; set;}
        public string Profissao  { get; set;}
        public int Tipo  { get; set; }
    }
}