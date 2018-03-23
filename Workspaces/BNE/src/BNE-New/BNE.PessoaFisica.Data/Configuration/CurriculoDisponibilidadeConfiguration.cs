using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class CurriculoDisponibilidadeConfiguration : EntityTypeConfiguration<CurriculoDisponibilidade>
    {
        public CurriculoDisponibilidadeConfiguration()
        {
            ToTable("CurriculoDisponibilidade", "pessoafisica");
            Property(p => p.IdCurriculo).HasColumnName("IdCurriculo");
            Property(p => p.IdDisponibilidadeGlobal).HasColumnName("IdDisponibilidadeGlobal");

            HasRequired(p => p.DisponibilidadeGlobal).WithMany().HasForeignKey(p => p.IdDisponibilidadeGlobal);
            HasRequired(p => p.Curriculo).WithMany().HasForeignKey(p => p.IdCurriculo);

            HasKey(p => new {p.IdCurriculo, p.IdDisponibilidadeGlobal});
        }
    }
}