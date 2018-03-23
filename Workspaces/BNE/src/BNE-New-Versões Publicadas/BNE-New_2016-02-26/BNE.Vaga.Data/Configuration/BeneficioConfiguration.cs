using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Vaga.Data.Configuration
{
    public class BeneficioConfiguration : EntityTypeConfiguration<Model.Beneficio>
    {
        public BeneficioConfiguration()
        {
            this.Map(m => { m.MapInheritedProperties(); m.ToTable("Beneficio", "vaga"); });
            this.HasKey(n => n.Id);
            this.Property(n => n.Descricao).HasMaxLength(100);
            this.Ignore(n => n.TranslationState);
        }
    }
}
