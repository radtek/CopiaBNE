using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Configuration
{
    public class HeaderConfiguration : EntityTypeConfiguration<Model.Header>
    {
        public HeaderConfiguration()
        {
            ToTable("Header", "ApiGateway");
            this.HasKey(n => n.Id);
        }
    }
}
