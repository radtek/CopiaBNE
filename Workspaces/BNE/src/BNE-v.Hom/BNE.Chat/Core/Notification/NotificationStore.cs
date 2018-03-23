using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BNE.Chat.Helper;

namespace BNE.Chat.Core.Notification
{
    public class NotificationStore
    {
        public enum NotificationGroupState
        {
            New,
            Existent,
            DeepCached
        }

        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> TimeoutAccess =
          new HardConfig<int>("chat_timeout_concorrencia_em_ms", 468).Wrap(a => a.Value);

        private readonly ConcurrentDictionary<int, OnlineNotificationGroup> _currentNotifications;
        private readonly ConcurrentDictionary<int, OffLineNotificationGroup> _cachedNotifications;
        private readonly ConcurrencyManager<int> _concurrencyManager;

        public NotificationStore()
        {
            _concurrencyManager = new ConcurrencyManager<int>(TimeoutAccess.Value);
            _cachedNotifications = new ConcurrentDictionary<int, OffLineNotificationGroup>();
            _currentNotifications = new ConcurrentDictionary<int, OnlineNotificationGroup>();
        }

        public ConcurrencyManager<int> ConcurrencyManager
        {
            get { return _concurrencyManager; }
        }

        public IEnumerable<OnlineNotificationGroup> GetAllOnline()
        {
            foreach (var item in _currentNotifications)
            {
                yield return item.Value;
            }
        }

        public IEnumerable<OffLineNotificationGroup> GetAllOffline()
        {
            foreach (var item in _cachedNotifications)
            {
                yield return item.Value;
            }
        }

        public Tuple<OnlineNotificationGroup, NotificationGroupState> GetOrCreateOnlineGroup(int key)
        {
            OnlineNotificationGroup onlineItem;
            if (_currentNotifications.TryGetValue(key, out onlineItem))
            {
                return new Tuple<OnlineNotificationGroup, NotificationGroupState>(onlineItem, NotificationGroupState.Existent);
            }

            using (ConcurrencyManager.GetToken(key))
            {
                OffLineNotificationGroup offLineItem;
                if (_cachedNotifications.TryRemove(key, out offLineItem))
                {
                    bool stateNew = false;
                    var res = _currentNotifications.AddOrUpdate(key, (keyFact) =>
                    {
                        stateNew = true;
                        offLineItem.Expired = true;
                        return offLineItem.Transform();
                    },
                    (updateFact, currentValue) =>
                    {
                        stateNew = false;
                        return currentValue;
                    });

                    return new Tuple<OnlineNotificationGroup, NotificationGroupState>(res,
                        stateNew ? NotificationGroupState.New : NotificationGroupState.DeepCached);
                }
            }

            return new Tuple<OnlineNotificationGroup, NotificationGroupState>(GetOrAddOnline(key), NotificationGroupState.New);
        }

        public NotificationGroup ForceMoveToOffLine(int key)
        {
            using (ConcurrencyManager.GetToken(key))
            {
                OnlineNotificationGroup onlineItem;
                if (_currentNotifications.TryRemove(key, out onlineItem))
                {
                    var res = _cachedNotifications.AddOrUpdate(key, keyFact => onlineItem.Transform(), (updateKey, currentValue) => currentValue);
                    return res;
                }
               
                return GetOrAddOffline(key);
            }
        }

        public bool TryMoveToOffLine(int key, out NotificationGroup group)
        {
            using (ConcurrencyManager.GetToken(key))
            {
                OnlineNotificationGroup onlineItem;
                if (_currentNotifications.TryRemove(key, out onlineItem))
                {
                    var res = _cachedNotifications.AddOrUpdate(key, keyFact => onlineItem.Transform(), (updateKey, currentValue) => currentValue);
                    group = res;
                    return true;
                }

                OffLineNotificationGroup auxGroup;
                _cachedNotifications.TryGetValue(key, out auxGroup);
                group = auxGroup;
                return false;
            }
        }


        public Tuple<OffLineNotificationGroup, NotificationGroupState> GetOrCreateOffLineGroup(int key)
        {
            using (ConcurrencyManager.GetToken(key))
            {
                OffLineNotificationGroup offItem;
                if (_cachedNotifications.TryGetValue(key, out offItem))
                {
                    return new Tuple<OffLineNotificationGroup, NotificationGroupState>(offItem, NotificationGroupState.DeepCached);
                }

                return new Tuple<OffLineNotificationGroup, NotificationGroupState>(GetOrAddOffline(key), NotificationGroupState.New);
            }
        }

        public bool TryRemoveFromOfflineCache(int key, out NotificationGroup offLineNotificationGroup)
        {
            OffLineNotificationGroup groupItem;
            var ret = _cachedNotifications.TryRemove(key, out groupItem);
            offLineNotificationGroup = groupItem;
            return ret;
        }

        protected OnlineNotificationGroup GetOrAddOnline(int key)
        {
            return _currentNotifications.GetOrAdd(key, keyFact => new OnlineNotificationGroup(keyFact));
        }

        protected OffLineNotificationGroup GetOrAddOffline(int key)
        {
            return _cachedNotifications.GetOrAdd(key, keyFact => new OffLineNotificationGroup(keyFact));
        }

        public bool GetOnlineGroup(int key, out OnlineNotificationGroup onlineGroup)
        {
            return _currentNotifications.TryGetValue(key, out onlineGroup);
        }
    }
}