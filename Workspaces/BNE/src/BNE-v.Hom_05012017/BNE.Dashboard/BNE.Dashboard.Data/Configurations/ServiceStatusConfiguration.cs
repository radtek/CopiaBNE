using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Dashboard.Data.Configurations
{
    internal class ServiceStatusConfiguration : EntityTypeConfiguration<Entities.Status>
    {

        public ServiceStatusConfiguration()
        {
            //HasKey(m => m.id);
        }

    }
}
