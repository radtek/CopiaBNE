using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BNE.PessoaFisica.Data.Configuration
{
    public class ParametroConfiguration : EntityTypeConfiguration<Model.Parametro>
    {
        public ParametroConfiguration()
        {
            ToTable("Parametro", "pessoafisica");
            this.HasKey(n => n.Id);
            this.Property(n => n.Id).HasColumnName("IdParametro").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(n => n.Nome).HasMaxLength(70).IsRequired();
            this.Property(n => n.Valor).IsRequired();
        }

    }
}