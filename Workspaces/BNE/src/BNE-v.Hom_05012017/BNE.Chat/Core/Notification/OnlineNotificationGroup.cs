using System;

namespace BNE.Chat.Core.Notification
{
    public class OnlineNotificationGroup : MutableNotificationGroup<OffLineNotificationGroup>
    {
        private bool _populated;

        public OnlineNotificationGroup(int keyGroup)
            : base(keyGroup)
        {

        }

        public bool Populated
        {
            get
            {
                return _populated;
            }
            set
            {
                _populated = value;
                LastUse = DateTime.Now;
            }
        }

        protected override OffLineNotificationGroup SiblingFactory()
        {
            LastUse = DateTime.Now;

            return new OffLineNotificationGroup(KeyGroupId);
        }

        public bool ExecuteSync(Func<OnlineNotificationGroup, bool> permission, Action<OnlineNotificationGroup> syncronizedAction)
        {
            LastUse = DateTime.Now;

            permission = permission ?? (a => true);

            if (!permission(this))
                return false;

            lock (Syncronization)
            {
                if (!permission(this))
                    return false;

                syncronizedAction(this);
                return true;
            }
        }

    }
}