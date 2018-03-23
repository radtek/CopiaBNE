using BNE.PessoaFisica.Model;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class FuncaoPretendidaConfiguration : EntityTypeConfiguration<FuncaoPretendida>
    {
        public FuncaoPretendidaConfiguration()
        {
            ToTable("FuncaoPretendida", "pessoafisica");
            this.Property(x => x.Id).HasColumnName("IdFuncaoPretendida");
            this.HasKey(x => x.Id);
            this.Property(x=>x.Descricao).HasMaxLength(50).IsOptional();
            this.Property(x=>x.DataCadastro).IsRequired();
            this.Property(x => x.IdFuncao).IsOptional();
            this.Property(x => x.TempoExperiencia).IsRequired();

            this.HasRequired(p => p.Curriculo).WithMany().Map(p => p.MapKey("IdCurriculo"));
        }
    }
}