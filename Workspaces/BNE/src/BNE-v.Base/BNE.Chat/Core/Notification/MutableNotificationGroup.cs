using System;

namespace BNE.Chat.Core.Notification
{
    public abstract class MutableNotificationGroup<T> : NotificationGroup where T : NotificationGroup
    {
        private WeakReference _offlineReference;
        private bool _converted;

        protected MutableNotificationGroup(int groupKey)
            : base(groupKey)
        {
        }

        public override bool AddPending(int pending, bool alreadyPersistent)
        {
            T converted;
            if (GetConvertedGroup(out converted))
            {
                return converted.AddPending(pending, alreadyPersistent);
            }
            return base.AddPending(pending, alreadyPersistent);
        }

        public override bool AddRead(int pending)
        {
            T converted;
            if (GetConvertedGroup(out converted))
            {
                return converted.AddRead(pending);
            }

            return base.AddRead(pending);
        }

        public override bool HasPending(int targetId)
        {
            T converted;
            if (GetConvertedGroup(out converted))
            {
                return converted.HasPending(targetId);
            }
            return base.HasPending(targetId);
        }

        protected abstract T SiblingFactory();

        protected virtual bool GetConvertedGroup(out T result)
        {
            LastUse = DateTime.Now;

            if (!_converted)
            {
                result = null;
                return false;
            }

            if (_offlineReference == null || !_offlineReference.IsAlive)
            {
                result = null;
                return false;
            }

            result = _offlineReference.Target as T;
            if (result != null)
                return true;

            return false;
        }

        public virtual T Transform()
        {
            LastUse = DateTime.Now;

            lock (Syncronization)
            {
                T converted;
                if (GetConvertedGroup(out converted))
                    return converted;

                var sibling = SiblingFactory();

                _offlineReference = new WeakReference(sibling);
                _converted = true;

                foreach (var pendingItem in GetItemsToPersistenceUnsafe())
                {
                    if (pendingItem.Pending)
                    {
                        sibling.AddPending(pendingItem.ItemId, pendingItem.AlreadyPersistent);
                    }
                    else
                    {
                        if (pendingItem.AlreadyPersistent)
                        {
                            sibling.AddReadPersitent(pendingItem.ItemId);
                        }
                        else
                        {
                            sibling.AddRead(pendingItem.ItemId);
                        }
                    }

                    RemoveUnsafe(pendingItem.ItemId);
                }


                return sibling;
            }
        }
    }
}