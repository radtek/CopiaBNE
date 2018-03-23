using System.Data.Entity.ModelConfiguration;
using BNE.Global.Model;

namespace BNE.Global.Data.Configuration
{
    public class FuncaoSinonimoConfiguration : EntityTypeConfiguration<FuncaoSinonimo>
    {
        public FuncaoSinonimoConfiguration()
        {
            ToTable("FuncaoSinonimo", "global");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("IdFuncaoSinonimo");
            Property(p => p.NomeSinonimo).HasMaxLength(100);
            Property(p => p.DescricaoPesquisa).HasMaxLength(100).IsRequired();
            Property(p => p.IdSinonimoSubstituto).IsOptional();
            Property(p => p.CodigoCBO).HasMaxLength(6).IsRequired();
            Property(p => p.Atribuicoes).IsMaxLength().IsOptional();
            Property(p => p.Responsabilidades).IsMaxLength().IsOptional();
            Property(p => p.DescricaoJob).HasMaxLength(2000).IsOptional();
            Property(p => p.Beneficio).IsMaxLength().IsOptional();
            Property(p => p.FlgAuditada).IsRequired();
            Property(p => p.FlgInativo).IsRequired();

            Property(p => p.DataCadastro).IsRequired();
            Property(p => p.DataAlteracao).IsOptional();

            HasRequired(p => p.TipoFuncaoGlobal).WithMany().Map(p => p.MapKey("IdTipoFuncaoGlobal"));
            HasRequired(p => p.EscolaricadeGlobal).WithMany().Map(p => p.MapKey("IdEscolaricadeGlobal"));
            HasRequired(p => p.FuncaoGlobal).WithMany().Map(p => p.MapKey("IdFuncaoGlobal"));
        }
    }
}