using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class FuncaoConfiguration : EntityTypeConfiguration<Funcao>
    {
        public FuncaoConfiguration()
        {
            ToTable("Funcao", "global");
            Property(p => p.Id).HasColumnName("IdFuncao");
            HasKey(p => p.Id);
            Property(p => p.Descricao).HasMaxLength(100).IsRequired();
            Property(p => p.Prioridade).IsRequired();
            Property(p => p.DataAlteracao).IsOptional();
            Property(p => p.DataCadastro).IsRequired();
            Property(p => p.FlgInativo).IsRequired();

            HasRequired(p => p.CargoGlobal).WithMany().Map(p => p.MapKey("IdCargoGlobal"));
        }
    }
}