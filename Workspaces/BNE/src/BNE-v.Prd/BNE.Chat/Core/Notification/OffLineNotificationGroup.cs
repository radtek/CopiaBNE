using System;
namespace BNE.Chat.Core.Notification
{
    public class OffLineNotificationGroup : MutableNotificationGroup<OnlineNotificationGroup>
    {
        public bool Expired { get; set; }

        public OffLineNotificationGroup(int keyFact)
            : base(keyFact)
        {
        }

        protected override OnlineNotificationGroup SiblingFactory()
        {
            LastUse = DateTime.Now;

            return new OnlineNotificationGroup(KeyGroupId);
        }
    }
}
