using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BNE.PessoaFisica.Domain.Model;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class ParametroConfiguration : EntityTypeConfiguration<Parametro>
    {
        public ParametroConfiguration()
        {
            ToTable("Parametro", "pessoafisica");
            HasKey(n => n.Id);
            Property(n => n.Id).HasColumnName("IdParametro").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(n => n.Nome).HasMaxLength(70).IsRequired();
            Property(n => n.Valor).IsRequired();
        }
    }
}