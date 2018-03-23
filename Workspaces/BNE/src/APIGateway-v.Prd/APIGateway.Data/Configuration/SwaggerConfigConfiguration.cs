using APIGateway.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Configuration
{
    public class SwaggerConfigConfiguration : EntityTypeConfiguration<SwaggerConfig>
    {
        public SwaggerConfigConfiguration()
        {
            ToTable("SwaggerConfig", "ApiGateway");
            this.HasKey(n => n.UrlSuffix);
            this.Property(n => n.UrlSuffix).HasMaxLength(50).IsRequired();

            this.Property(n => n.FileUrl).HasMaxLength(150).IsRequired();
            this.Property(n => n.FileName).HasMaxLength(250).IsRequired();
            this.Property(n => n.UIUrl).HasMaxLength(50).IsRequired();
            this.Property(n => n.Theme).HasMaxLength(50).IsRequired();

            this.HasOptional(n => n.Authentication);

            this.Ignore(n => n.FullUIUrl);
            this.Ignore(n => n.FullFileUrl);
        }
    }
}
