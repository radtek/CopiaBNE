using APIGateway.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Configuration
{
    public class AuthenticationConfiguration: EntityTypeConfiguration<Authentication>
    {
        public AuthenticationConfiguration()
        {
            ToTable("Authentication", "ApiGateway");
            this.HasKey(n => n.Interface);
            this.Property(n => n.Interface).HasMaxLength(20);
            this.Property(n => n.Descricao).IsRequired().HasMaxLength(100);

            this.HasOptional(n => n.OAuthConfig)
                .WithRequired(oAuth => oAuth.Authentication);
        }
    }
}
