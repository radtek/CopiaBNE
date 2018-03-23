using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CurriculoDisponibilidadeConfiguration : EntityTypeConfiguration<CurriculoDisponibilidade>
    {
        public CurriculoDisponibilidadeConfiguration()
        {
            ToTable("CurriculoDisponibilidade", "pessoafisica");
            this.Property(p => p.IdCurriculo).HasColumnName("IdCurriculo");
            this.Property(p => p.IdDisponibilidadeGlobal).HasColumnName("IdDisponibilidadeGlobal");

            this.HasRequired(p => p.DisponibilidadeGlobal).WithMany().HasForeignKey(p=>p.IdDisponibilidadeGlobal);
            this.HasRequired(p => p.Curriculo).WithMany().HasForeignKey(p => p.IdCurriculo);

            this.HasKey(p => new { p.IdCurriculo,p.IdDisponibilidadeGlobal });
        }
    }
}