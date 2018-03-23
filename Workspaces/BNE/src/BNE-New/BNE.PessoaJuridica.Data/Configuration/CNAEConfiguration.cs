using System.Data.Entity.ModelConfiguration;
using BNE.PessoaJuridica.Domain.Model;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class CNAEConfiguration : EntityTypeConfiguration<CNAE>
    {
        public CNAEConfiguration()
        {
            ToTable("CNAE", "pessoajuridica");
            HasKey(n => n.Codigo);
            Property(n => n.Codigo).HasMaxLength(7);
            Property(n => n.Descricao).HasMaxLength(175).IsRequired();
        }
    }
}