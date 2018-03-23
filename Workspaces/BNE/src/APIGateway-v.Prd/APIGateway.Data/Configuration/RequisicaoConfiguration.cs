using APIGateway.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Configuration
{
    public class RequisicaoConfiguration : EntityTypeConfiguration<Requisicao>
    {
        public RequisicaoConfiguration()
        {
            ToTable("Requisicao", "ApiGateway");
            this.HasKey(n => n.Id);
            this.Property(n => n.DataRequisicao).IsRequired();
            this.Property(n => n.Request).IsRequired().IsMaxLength();
            this.Property(n => n.RequestContent).IsMaxLength();
            this.Property(n => n.Response).IsMaxLength();
            this.Property(n => n.ResponseContent).IsMaxLength();
            this.Property(n => n.ResponseStatusCode).IsRequired();
            this.Property(n => n.TempoExecucao).IsRequired();

            this.HasRequired(n => n.Endpoint);
            this.HasOptional(n => n.SistemaCliente);
            this.HasOptional(n => n.Usuario);

            this.Property(n => n.PerfilString).HasColumnName("Perfil").HasMaxLength(50).IsRequired();
            this.Ignore(n => n.Perfil);
        }
    }
}
