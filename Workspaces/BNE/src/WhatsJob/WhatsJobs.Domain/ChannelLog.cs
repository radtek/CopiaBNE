using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsJob.Domain
{
    public class ChannelLog
    {
        static BNE.ExceptionLog.DatabaseLogger _logger = new BNE.ExceptionLog.DatabaseLogger();

        public static bool Save(Model.ChannelLog objChannelLog)
        {
            try
            {
                using (var db = new Data.WhatsJobsContext())
                {
                    db.ChannelLog.Add(objChannelLog);
                    db.Entry(objChannelLog.Channel).State = System.Data.Entity.EntityState.Unchanged;
                    return db.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao salvar mensagem (Domain.Message.Save)");
            }

            return false;
        }
    }
}
