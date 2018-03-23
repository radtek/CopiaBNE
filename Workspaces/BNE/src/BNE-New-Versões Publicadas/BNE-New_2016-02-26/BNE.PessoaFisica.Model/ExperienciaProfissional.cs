using System;

namespace BNE.PessoaFisica.Model
{
    public class ExperienciaProfissional
    {
        public Int64 id { get; set; }
        public string NomeEmpresa { get; set; }
        public DateTime? DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public DateTime DataCadastro { get; set; }

        public string AtividadesExercidas { get; set; }
        public string FuncaoExercida { get; set; }
        public bool Ativo { get; set; }
        public bool FlgImportado { get; set; }
        public decimal? UltimoSalario { get; set; }

        public virtual Global.Model.RamoAtividadeGlobal RamoAtividadeGlobal { get; set; }
        //public virtual Global.Model.Funcao Funcao { get; set; }
        public virtual PessoaFisica PessoaFisica { get; set; }
    }
}