using APIGateway.Model;
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
    public class EndpointConfiguration : EntityTypeConfiguration<Endpoint>
    {
        public EndpointConfiguration()
        {
            ToTable("Endpoint", "ApiGateway");
            this.HasKey(n => n.Id);

            this.Property(n => n.RelativePath).HasMaxLength(200).IsRequired();
            this.Property(n => n.DestinationRelativePath).HasMaxLength(200);
            this.Property(n => n.MethodString).HasMaxLength(10).IsRequired();
            this.Property(n => n.LogErro).IsRequired();
            this.Property(n => n.LogSucesso).IsRequired();
            this.Property(n => n.LogResponse).IsRequired();
            this.Property(n => n.AllowAnonymous).IsRequired();

            this.HasRequired(n => n.Api)
                .WithMany()
                .HasForeignKey(e => e.ApiUrlSuffix);

            //Definindo Unique Key para Api, GatewayRelativePath e Method
            //Não usada chave natural para economizar espaço em disco nos logs de chamada
            this.Property(n => n.ApiUrlSuffix).IsRequired().HasColumnAnnotation(IndexAnnotation.AnnotationName,
                                                            new IndexAnnotation(
                                                                new IndexAttribute("UQ_Endpoint", 1) { IsUnique = true }));

            this.Property(n => n.RelativePath).IsRequired().HasColumnAnnotation(IndexAnnotation.AnnotationName,
                                                            new IndexAnnotation(
                                                                new IndexAttribute("UQ_Endpoint", 2) { IsUnique = true }));
            this.Property(n => n.MethodString).IsRequired().HasColumnAnnotation(IndexAnnotation.AnnotationName,
                                                            new IndexAnnotation(
                                                                new IndexAttribute("UQ_Endpoint", 3) { IsUnique = true }));
            
            //Mapeando Method para string
            this.Property(n => n.MethodString).HasColumnName("Method").IsRequired();
            this.Ignore(n => n.Method);
        }
    }
}
