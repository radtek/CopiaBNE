﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Mapper.Models.PessoaFisica
{
    public class Formacao
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public int IdEscolaridade { get; set; }
        public int IdCurso { get; set; }
        public int IdInstituicaoEnsino { get; set; }
        public int IdCidade { get; set; }
        public int IdVaga { get; set; }

        public short? AnoConclusao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
        public short CargaHoraria { get; set; }
        public bool Ativo { get; set; }
        public string NomeInstituicao { get; set; }
        public string NomeCurso { get; set; }

        public decimal Cpf { get; set; }
    }
}
