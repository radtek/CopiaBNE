using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Configuration
{
    public class OAuthConfigConfiguration : EntityTypeConfiguration<Model.OAuthConfig>
    {
        public OAuthConfigConfiguration()
        {
            ToTable("OAuthConfig", "ApiGateway");
            this.HasKey(n => n.Interface);

            this.Property(n => n.AuthenticationEndpoint).HasMaxLength(150).IsRequired();
            this.Property(n => n.Expiration).IsRequired();
            this.Property(n => n.SecretKey).HasMaxLength(16).IsRequired();
            this.Property(n => n.TokenEndpoint).HasMaxLength(50).IsRequired();

        }
    }
}
