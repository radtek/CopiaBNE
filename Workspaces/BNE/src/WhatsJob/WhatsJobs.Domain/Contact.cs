using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsJob.Domain
{
    public class Contact
    {
        public static Model.Contact GetContact(string from, string nickName)
        {
            Model.Contact contact;
            using (var db = new Data.WhatsJobsContext())
            {
                contact = db.Contact.FirstOrDefault(c => c.From == from);
                if (contact == null)
                {
                    //Creating contact if it does not exist in the database
                    contact = new Model.Contact()
                    {
                        From = from,
                        NickName = nickName,
                        Number = Util.GetNumberOfFrom(from)
                    };
                    db.Contact.Add(contact);
                    db.SaveChanges();
                }
            }

            return contact;

        }
    }
}
