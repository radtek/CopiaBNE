using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BNE.Chat.Helper;
using BNE.Chat.Model;

namespace BNE.Chat.Core
{
    public enum StoreChangedResultType
    {
        None,
        FirstAddition,
        Updated,
        DeletedTarget,
        RemovedAll
    }
    public sealed class ChatStore
    {
        #region [ Atributos ]
        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> CleanUpTime =
            new SetValueOrDefaultFact<HardConfig<int>, int>(
                new HardConfig<int>("chat_history_clean_up_cycle_in_minutes", 10), a => a.Value);
        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> PrivateHistoryTimeout =
            new SetValueOrDefaultFact<HardConfig<int>, int>(
                new HardConfig<int>("chat_lifetime_of_history_without_activity", 30), a => a.Value);

        private readonly ConcurrentDictionary<int, string> _ownerSession;
        private readonly ConcurrentDictionary<string, KeyWrapper<object, ImmutableList<string>>> _connectionBySession;
        private readonly ConcurrentDictionary<int, KeyWrapper<object, ImmutableList<PrivateHistoryChat>>> _chatHistory;

        private DateTime _lastCleanUp;
        private readonly object _sincronization = new object();

        #endregion

        #region [ Construtor ]
        public ChatStore()
        {
            _ownerSession = new ConcurrentDictionary<int, string>();
            _connectionBySession = new ConcurrentDictionary<string, KeyWrapper<object, ImmutableList<string>>>();
            _chatHistory = new ConcurrentDictionary<int, KeyWrapper<object, ImmutableList<PrivateHistoryChat>>>();
            _lastCleanUp = DateTime.Now;
        }
        #endregion
 
        #region [ Public ]
        public StoreChangedResultType AddConnection(int ownerId, string sessionId, string conexaoId)
        {
            _ownerSession[ownerId] = sessionId;

            var res = StoreChangedResultType.FirstAddition;
            _connectionBySession.AddOrUpdate(sessionId,
                keyToAdd => new KeyWrapper<object, ImmutableList<string>>(new object(),
                    new ImmutableList<string>(new[] { conexaoId })),
                (keyToUpdate, valueToUpdate) =>
                {
                    res = StoreChangedResultType.Updated;
                    if (valueToUpdate.Item.Contains(conexaoId))
                        return valueToUpdate;

                    lock (valueToUpdate.Key)
                    {
                        if (valueToUpdate.Item.Contains(conexaoId))
                            return valueToUpdate;

                        valueToUpdate.Item = valueToUpdate.Item.Add(conexaoId);
                        return valueToUpdate;
                    }
                });

            CleanUpIfNeeds();
            return res;
        }

        public StoreChangedResultType RemoveConnection(string sessionId, string conexaoId)
        {
            try
            {
                KeyWrapper<object, ImmutableList<string>> connections;
                if (!_connectionBySession.TryGetValue(sessionId, out connections))
                {
                    var sessionContainConnection =
                        _connectionBySession.FirstOrDefault(obj => obj.Value.Item.Any(a => a == conexaoId));

                    if (sessionContainConnection.Key == null)
                        return StoreChangedResultType.None;

                    connections = sessionContainConnection.Value;

                }

                if (RemoveCacheConnection(sessionId, conexaoId, connections))
                {
                    return StoreChangedResultType.RemovedAll;
                }
                return StoreChangedResultType.DeletedTarget;
            }
            finally
            {
                CleanUpIfNeeds();
            }
        }

        public StoreChangedResultType RemoveConnection(string conexaoId)
        {
            try
            {
                var existente = _connectionBySession.FirstOrDefault(obj => obj.Value.Item.Any(a => a == conexaoId));
                if (existente.Key == null)
                    return StoreChangedResultType.None;

                var valorDoPar = existente.Value;

                if (RemoveCacheConnection(existente.Key, conexaoId, valorDoPar))
                {
                    return StoreChangedResultType.RemovedAll;
                }
                return StoreChangedResultType.DeletedTarget;
            }
            finally
            {
                CleanUpIfNeeds();
            }
        }

        public StoreChangedResultType RemoveSession(string sessionId)
        {
            KeyWrapper<object, ImmutableList<string>> conexoes;

            var existente = _connectionBySession.TryRemove(sessionId, out conexoes);

            StoreChangedResultType res;
            if (existente)
                res = StoreChangedResultType.RemovedAll;
            else
                res = StoreChangedResultType.None;

            var lastPair = _ownerSession.FirstOrDefault(a => a.Value == sessionId);
            string currentSession;
            if (lastPair.Key != 0
                && _ownerSession.TryRemove(lastPair.Key, out currentSession))
            {
                if (sessionId != currentSession)
                {
                    res = res == StoreChangedResultType.RemovedAll
                        ? StoreChangedResultType.DeletedTarget
                        : StoreChangedResultType.None;

                    _ownerSession.AddOrUpdate(lastPair.Key, key => currentSession, (key, actualSession) =>
                    {
                        if (actualSession != currentSession)
                        {
                            return actualSession;
                        }
                        return currentSession;
                    });
                }
            }

            CleanUpIfNeeds();
            return res;
        }

        public IEnumerable<string> GetConnections(string sessionId)
        {
            KeyWrapper<object, ImmutableList<string>> conexoes;
            if (!_connectionBySession.TryGetValue(sessionId, out conexoes))
                yield break;

            foreach (var item in conexoes.Item)
            {
                yield return item;
            }
        }

        public PrivateHistoryChat GetOrAddHistory(int ownerId, int otherId)
        {
            PrivateHistoryChat historyResult = null;

            _chatHistory.AddOrUpdate(ownerId,
                keyFactory => new KeyWrapper<object, ImmutableList<PrivateHistoryChat>>(new object(),
                     new ImmutableList<PrivateHistoryChat>(new[]
                         {
                            historyResult= CreateHistoryChat(ownerId, otherId)
                         })),
                (key, valueToUpdate) =>
                {
                    var messageHistory = valueToUpdate.Item.FirstOrDefault(a => a.PartyWith == otherId);
                    if (messageHistory != null)
                    {
                        historyResult = messageHistory;
                        return valueToUpdate;
                    }

                    lock (valueToUpdate.Key)
                    {
                        messageHistory = valueToUpdate.Item.FirstOrDefault(a => a.PartyWith == otherId);
                        if (messageHistory != null)
                        {
                            historyResult = messageHistory;
                            return valueToUpdate;
                        }

                        historyResult = CreateHistoryChat(ownerId, otherId);
                        valueToUpdate.Item = valueToUpdate.Item.Add(historyResult);
                        return valueToUpdate;
                    }
                });

            return historyResult;
        }

        public IEnumerable<PrivateHistoryChat> GetHistorical(int ownerId)
        {
            KeyWrapper<object, ImmutableList<PrivateHistoryChat>> chats;
            if (!_chatHistory.TryGetValue(ownerId, out chats))
                yield break;

            bool peloMenosUm = false;
            foreach (var item in chats.Item)
            {
                peloMenosUm = true;
                yield return item;
            }

            if (peloMenosUm)
                yield break;

            lock (chats.Key)
            {
                if (!_chatHistory.TryGetValue(ownerId, out chats))
                    yield break;

                if (chats.Item.Count == 0)
                {
                    _chatHistory.TryRemove(ownerId, out chats);
                }
            }
        }

        public void CleanAll()
        {
            _ownerSession.Clear();
            _connectionBySession.Clear();
            _chatHistory.Clear();
        }

        public string GetSession(int ownerId)
        {
            string lastSession;
            if (_ownerSession.TryGetValue(ownerId, out lastSession))
            {
                return lastSession;
            }

            return null;
        }

        public KeyValuePair<PrivateHistoryChat, PrivateHistoryMessage> AddPrivateMessage(int senderId, int listenerOwnerChatId, string guidMessage, bool writerIsTheOwner)
        {
            using (var allHistory = GetHistorical(listenerOwnerChatId).Memoize())
            {
                var newMessage = new PrivateHistoryMessage
                {
                    Guid = guidMessage,
                    CreatorType = writerIsTheOwner ? PrivateHistoryMessage.MessageOwner.Self : PrivateHistoryMessage.MessageOwner.Other
                };

                if (allHistory.Any())
                {
                    var target = allHistory.FirstOrDefault(a => a.PartyWith == senderId);

                    if (target != null)
                    {
                        if (!target.AddMessage(newMessage))
                        {
                            newMessage = target.GetMessageHistory().FirstOrDefault(a => a.Guid == guidMessage) ??
                                         newMessage;
                        }

                        return new KeyValuePair<PrivateHistoryChat, PrivateHistoryMessage>(target, newMessage);
                    }
                }

                var historyChatCreated = GetOrAddHistory(listenerOwnerChatId, senderId);
                return new KeyValuePair<PrivateHistoryChat, PrivateHistoryMessage>(historyChatCreated, newMessage);
            }
        }

        public int GetOwnerOrDefaultBySession(string sessionId)
        {
            if (sessionId == null)
                return 0;

            return _ownerSession.FirstOrDefault(a => a.Value == sessionId).Key;
        }

        public int GetOwnerOrDefaultByConnection(string connectionId)
        {
            if (connectionId == null)
                return 0;

            var session = _connectionBySession.FirstOrDefault(a => a.Value.Item.Any(obj => obj == connectionId));

            return GetOwnerOrDefaultBySession(session.Key);
        }

        //public bool InativateHistory(int ownerId, int targetId)
        //{
        //    KeyWrapper<object, ImmutableList<PrivateHistoryChat>> chats;
        //    if (!_chatHistory.TryGetValue(ownerId, out chats))
        //        return false;

        //    var current = chats.Item.FirstOrDefault(a => a.PartyWith == targetId);
        //    if (current == null)
        //    {
        //        return false;
        //    }

        //    current.Inative();
        //}
        public bool RemoveHistory(int ownerId, int targetId)
        {
            KeyWrapper<object, ImmutableList<PrivateHistoryChat>> chats;
            if (!_chatHistory.TryGetValue(ownerId, out chats))
                return false;

            var current = chats.Item.FirstOrDefault(a => a.PartyWith == targetId);
            if (current == null)
            {
                return false;
            }

            lock (chats.Key)
            {
                var old = chats.Item;

                chats.Item = chats.Item.Remove(current);
                if (old.Count != chats.Item.Count)
                    return true;

                return false;
            }
        }
        #endregion

        #region [ Private ]
        private bool RemoveCacheConnection(string sessionId, string connectionId, KeyWrapper<object, ImmutableList<string>> connections)
        {
            lock (connections.Key)
            {
                connections.Item = connections.Item.Remove(connectionId);
                return TryRemoveParConnectionUnsafe(sessionId, connections);
            }
        }

        private static PrivateHistoryChat CreateHistoryChat(int ownerId, int otherId)
        {
            return new PrivateHistoryChat()
            {
                OwnerId = ownerId,
                PartyWith = otherId
            };
        }

        private void CleanUpIfNeeds()
        {
            if ((DateTime.Now - _lastCleanUp).TotalMinutes < CleanUpTime.Value)
                return;

            lock (_sincronization)
            {
                if ((DateTime.Now - _lastCleanUp).TotalMinutes < CleanUpTime.Value)
                    return;

                _lastCleanUp = DateTime.Now;
            }

            var warStopwatch = Stopwatch.StartNew();
            const int maxDelayTimeToCleanUp = 400;
            foreach (var historic in _chatHistory)
            {
                var pair = historic.Value;

                using (var toClean =
                    pair.Item.Where(
                        a => (a.IsEmpty || (DateTime.Now - a.LastUse).TotalMinutes >= PrivateHistoryTimeout.Value) && a.
                                                                                                                    MinimumLifetimeReached)
                        .Memoize())
                {
                    if (toClean.Any())
                    {
                        lock (pair.Key)
                        {
                            foreach (var a in toClean)
                            {
                                if ((a.IsEmpty || (DateTime.Now - a.LastUse).TotalMinutes >= PrivateHistoryTimeout.Value) && a.MinimumLifetimeReached)
                                {
                                    pair.Item = pair.Item.Remove(a);
                                }
                            }

                            if (pair.Item.Count == 0)
                            {
                                KeyWrapper<object, ImmutableList<PrivateHistoryChat>> aux;
                                _chatHistory.TryRemove(historic.Key, out aux);
                            }
                        }
                    }
                    else
                    {
                        if (pair.Item.Count == 0)
                        {
                            lock (pair.Key)
                            {
                                if (pair.Item.Count == 0)
                                {
                                    KeyWrapper<object, ImmutableList<PrivateHistoryChat>> aux;
                                    _chatHistory.TryRemove(historic.Key, out aux);
                                }
                            }
                        }
                    }
                }

                if (warStopwatch.ElapsedMilliseconds > maxDelayTimeToCleanUp)
                {
                    warStopwatch.Stop();
                    return;
                }
            }
            warStopwatch.Stop();

            warStopwatch = Stopwatch.StartNew();
            foreach (var item in _connectionBySession.ToArray())
            {
                var pair = item.Value;
                if (pair.Item.Count == 0)
                {
                    lock (pair.Key)
                    {
                        TryRemoveParConnectionUnsafe(item.Key, pair);
                    }
                }

                if (warStopwatch.ElapsedMilliseconds > maxDelayTimeToCleanUp)
                {
                    warStopwatch.Stop();
                    return;
                }
            }
            warStopwatch.Stop();
        }

        private bool TryRemoveParConnectionUnsafe(string sessionId, KeyWrapper<object, ImmutableList<string>> pair)
        {
            if (pair.Item.Count == 0)
            {
                _connectionBySession.TryRemove(sessionId, out pair);
                return true;
            }
            return false;
        }
        #endregion

    }

}
