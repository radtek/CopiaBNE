﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain.Command
{
    public class InformacaoCurriculo
    {
        public bool EhVip { get; set; }
        public int idCurriculo { get; set; }
        public bool EmpresaBloqueada { get; set; }
        public bool JaEnvioCvParaVaga { get; set; }
        public bool EstaNaRegiaoBH { get; set; }
        public bool TemExperienciaProfissional { get; set; }
        public bool TemFormacao { get; set; }
    }
}
