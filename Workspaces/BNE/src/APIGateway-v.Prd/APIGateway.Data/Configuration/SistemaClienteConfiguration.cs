using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Configuration
{
    public class SistemaClienteConfiguration : EntityTypeConfiguration<APIGateway.Model.SistemaCliente>
    {
        public SistemaClienteConfiguration()
        {
            ToTable("SistemaCliente", "ApiGateway");
            this.HasKey(n => n.Chave);
            this.Property(n => n.Nome)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnAnnotation(
                        IndexAnnotation.AnnotationName,
                        new IndexAnnotation(
                            new IndexAttribute("UQ_SistemaClienteNome") { IsUnique = true }));
        }
    }
}
