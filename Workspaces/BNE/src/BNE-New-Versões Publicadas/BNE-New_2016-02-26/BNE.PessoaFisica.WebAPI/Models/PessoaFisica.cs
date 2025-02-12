﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.PessoaFisica.WebAPI.Models
{
    public class PessoaFisica
    {
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public string Email { get; set; }
        public string ConfirmarEmail { get; set; }
        public string Celular { get; set; }
        public string Cidade { get; set; }
        public string Funcao { get; set; }
        public decimal? PretensaoSalarial { get; set; }
        public int? TempoExperienciaAnos { get; set; }
        public int? TempoExperienciaMeses { get; set; }
        public int IdEscolaridade { get; set; }
        public int? IdDeficiencia { get; set; }

        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
    }
}