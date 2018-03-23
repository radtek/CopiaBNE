using APIGateway.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Configuration
{
    public class ApiConfiguration : EntityTypeConfiguration<Api>
    {
        public ApiConfiguration()
        {
            ToTable("Api", "ApiGateway");
            this.HasKey(n => n.UrlSuffix);
            this.Property(n => n.UrlSuffix).HasMaxLength(50).IsRequired();
            this.Property(n => n.BaseUrl).HasMaxLength(150).IsRequired();
            this.Property(n => n.DevUrl).HasMaxLength(150).IsRequired();
            this.HasOptional(n => n.AuthenticationType);
            this.HasMany<Endpoint>(a => a.Endpoints)
                .WithRequired(e => e.Api);
            this.HasMany<SistemaCliente>(a => a.Sistemas)
                .WithMany(s => s.Apis)
                .Map(sa =>
                {
                    sa.MapLeftKey("UrlSuffix_Api");
                    sa.MapRightKey("Chave_Sistema");
                    sa.ToTable("ApiSistema", "ApiGateway");
                });

            this.HasOptional<SwaggerConfig>(a => a.SwaggerConfig)
                .WithRequired(s => s.Api);

            this.Ignore(n => n.Url);
        }
    }
}
