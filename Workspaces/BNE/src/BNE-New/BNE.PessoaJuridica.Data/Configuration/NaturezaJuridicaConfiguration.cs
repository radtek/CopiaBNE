using System.Data.Entity.ModelConfiguration;
using BNE.PessoaJuridica.Domain.Model;

namespace BNE.PessoaJuridica.Data.Configuration
{
    public class NaturezaJuridicaConfiguration : EntityTypeConfiguration<NaturezaJuridica>
    {
        public NaturezaJuridicaConfiguration()
        {
            ToTable("NaturezaJuridica", "pessoajuridica");
            HasKey(n => n.Codigo);
            Property(n => n.Codigo).HasMaxLength(4);
            Property(n => n.Descricao).HasMaxLength(75).IsRequired();
        }
    }
}