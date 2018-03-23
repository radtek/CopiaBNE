using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL.DTO
{
    public class MediaSalarial
    {
        public int idFuncaoSalarioBR { get; set; }
        public string NomeFuncao { get; set; }
        public string DescricaoFuncao { get; set; }
        public DetalhesFuncao DetalhesFuncao { get; set; }

    }

    public class DetalhesFuncao{
        public List<string> Curriculos { set; get; }
        public string ObjetivosDoCargo { get; set; }
        public SalarioPequena SalarioPequena { get; set; }
        public SalarioMedia SalarioMedia { get; set; }
        public SalarioGrande SalarioGrande { get; set; }
        }

    public class SalarioPequena
    {
        public decimal Trainee { get; set; }
        public decimal Junior { get; set; }
        public decimal Pleno { get; set; }
        public decimal Senior { get; set; }
        public decimal Master { get; set; }
    }

    public class SalarioMedia
    {
        public decimal Trainee { get; set; }
        public decimal Junior { get; set; }
        public decimal Pleno { get; set; }
        public decimal Senior { get; set; }
        public decimal Master { get; set; }
    }

    public class SalarioGrande
    {
        public decimal Trainee { get; set; }
        public decimal Junior { get; set; }
        public decimal Pleno { get; set; }
        public decimal Senior { get; set; }
        public decimal Master { get; set; }
    }
}
