using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using BNE.PessoaFisica.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class TipoCurriculoConfiguration : EntityTypeConfiguration<TipoCurriculo>
    {
        public TipoCurriculoConfiguration()
        {
            ToTable("TipoCurriculo","pessoafisica");
            this.Property(x=>x.Id).HasColumnName("IdTipoCurriculo");
            this.HasKey(x => x.Id);
            this.Property(p=>p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(x => x.Descricao).HasMaxLength(50).IsRequired();
        }
    }
}