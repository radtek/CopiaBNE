using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Configuration
{
    public class AuthorizationConfiguration : EntityTypeConfiguration<Model.Authorization>
    {
        public AuthorizationConfiguration()
        {
            ToTable("Authorization", "ApiGateway");
            this.HasKey(n => new { n.EndpointId, n.Perfil });
            this.HasRequired(n => n.Endpoint).WithMany().HasForeignKey(n => n.EndpointId);
        }
    }
}
