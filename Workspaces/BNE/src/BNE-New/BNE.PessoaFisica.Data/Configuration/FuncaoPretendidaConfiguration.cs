using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class FuncaoPretendidaConfiguration : EntityTypeConfiguration<FuncaoPretendida>
    {
        public FuncaoPretendidaConfiguration()
        {
            ToTable("FuncaoPretendida", "pessoafisica");
            Property(x => x.Id).HasColumnName("IdFuncaoPretendida");
            HasKey(x => x.Id);
            Property(x => x.Descricao).HasMaxLength(50).IsOptional();
            Property(x => x.DataCadastro).IsRequired();
            Property(x => x.IdFuncao).IsOptional();
            Property(x => x.TempoExperiencia).IsRequired();

            HasRequired(p => p.Curriculo).WithMany().Map(p => p.MapKey("IdCurriculo"));
        }
    }
}