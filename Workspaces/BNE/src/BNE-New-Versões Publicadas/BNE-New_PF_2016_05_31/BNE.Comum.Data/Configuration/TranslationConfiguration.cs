using System.Data.Entity.ModelConfiguration;

namespace BNE.Comum.Data.Configuration
{
    public class TranslationConfiguration : EntityTypeConfiguration<Model.Localizable.Translation>
    {
        public TranslationConfiguration(string schema)
        {
            ToTable("Translation", schema);
            this.HasKey(n => new { n.Type, n.FieldName, n.LanguageCode, n.PrimaryKeyValue });
            this.Property(n => n.Type).HasMaxLength(150);
            this.Property(n => n.FieldName).HasMaxLength(50);
            this.Property(n => n.LanguageCode).HasMaxLength(10);
            this.Property(p => p.PrimaryKeyValue).HasMaxLength(15);
            this.Property(n => n.Text).IsMaxLength();
        }
    }
}