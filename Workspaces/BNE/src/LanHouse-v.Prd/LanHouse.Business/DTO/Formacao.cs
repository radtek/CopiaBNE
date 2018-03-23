using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business.DTO
{
    public class Formacao
    {
        public int idFormacao {get;set;}
        public int idEscolaridade { get; set; }
        public string nivel { get; set; } //Descrição da escolaridade
        public int? idCurso { get; set; }
        public string curso { get; set; }
        public int grau { get; set; }
        public int? idCidade { get; set; }
        public string cidadeFormacao { get; set; }
        public short? cargaHoraria { get; set; }
        public short? idSituacaoFormacao { get; set; }
        public string descricaoCurso { get; set; }
        public string situacao { get; set; }
        public string instituicao {get;set;}
        public short? anoConclusao { get; set; }
        
    }
}
