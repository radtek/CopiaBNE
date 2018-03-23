using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BNE.Chat.Core.Notification
{
    public abstract class NotificationGroup
    {
        private readonly Dictionary<int, bool> _persistentItems;
        private readonly SortedSet<int> _volatileItems;
        private readonly int _group;
        private DateTime _lastUse;

        protected readonly object Syncronization = new object();

        protected NotificationGroup(int keyGroup)
        {
            if (keyGroup == 0)
                throw new ArgumentOutOfRangeException("keyGroup");

            _group = keyGroup;
            _volatileItems = new SortedSet<int>();
            _persistentItems = new Dictionary<int, bool>();
            _lastUse = DateTime.Now;
        }

        public int KeyGroupId
        {
            get
            {
                return _group;
            }
        }

        public DateTime LastUse
        {
            get { return _lastUse; }
            protected set { _lastUse = value; }
        }

        public virtual bool HasPending(int targetId)
        {
            _lastUse = DateTime.Now;

            lock (Syncronization)
            {
                if (_volatileItems.Contains(targetId))
                    return true;

                bool pending;
                _persistentItems.TryGetValue(targetId, out pending);
                return pending;
            }
        }

        public virtual bool AddPending(int pending, bool alreadyPersistent)
        {
            _lastUse = DateTime.Now;

            lock (Syncronization)
            {
                if (alreadyPersistent)
                {
                    _volatileItems.Remove(pending);
                    _persistentItems[pending] = true;
                    return true;
                }

                bool current;
                if (!_persistentItems.TryGetValue(pending, out current))
                    return _volatileItems.Add(pending);

                if (current)
                    return false;

                _persistentItems[pending] = true;
                return true;
            }
        }

        public virtual bool AddRead(int pending)
        {
            _lastUse = DateTime.Now;

            lock (Syncronization)
            {
                bool current;
                if (!_persistentItems.TryGetValue(pending, out current)) 
                    return _volatileItems.Remove(pending);

                if (current)
                {
                    _persistentItems[pending] = false;
                    return true;
                }

                return _volatileItems.Remove(pending);
            }
        }

        internal virtual void AddReadPersitent(int pendingId)
        {
            _lastUse = DateTime.Now;

            lock (Syncronization)
            {
                _volatileItems.Remove(pendingId);
                _persistentItems[pendingId] = false;
            }
        }

        public IEnumerable<KeyValuePair<int, bool>> GetItemsToPersistence()
        {
            _lastUse = DateTime.Now;

            IList<KeyValuePair<int, bool>> copy;
            lock (Syncronization)
            {
                copy =
                    _persistentItems.OrderBy(a => a.Key).Concat(_volatileItems.Select(a => new KeyValuePair<int, bool>(a, true))).ToArray();
            }

            foreach (var item in copy)
            {
                yield return item;

                _lastUse = DateTime.Now;
            }
        }

        protected IEnumerable<NotificationNode> GetItemsToPersistenceUnsafe()
        {
            _lastUse = DateTime.Now;

            IEnumerable<NotificationNode> copy =
                    _persistentItems.OrderBy(a => a.Key).Select(a => new NotificationNode { ItemId = a.Key, Pending = a.Value, AlreadyPersistent = true })
                        .Concat(_volatileItems.Select(a => new NotificationNode { ItemId = a, Pending = true }))
                        .ToArray();

            foreach (var item in copy)
            {
                yield return item;

                _lastUse = DateTime.Now;
            }
        }

        protected bool RemoveUnsafe(int targetId)
        {
            _lastUse = DateTime.Now;

            return _persistentItems.Remove(targetId) || _volatileItems.Remove(targetId);
        }

        protected class NotificationNode
        {
            public bool AlreadyPersistent { get; set; }
            public bool Pending { get; set; }
            public int ItemId { get; set; }
        }
    }
}