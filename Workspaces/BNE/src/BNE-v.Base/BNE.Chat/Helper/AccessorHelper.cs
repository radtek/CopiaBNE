using System;
using System.Linq;
using System.Threading;

namespace BNE.Chat.Helper
{
    public static class AccessorHelper
    {
        #region [ Static ]
        public static void LockHelper(ref object syncronization, Action action)
        {
            lock (syncronization)
            {
                action();
            }
        }

        public static void AddEvent(ref EventHandler currentHandler, EventHandler newHandler)
        {
            if (newHandler == null)
                return;

            var eventCopy = currentHandler;
            if (eventCopy == null
                || eventCopy.GetInvocationList().All(obj => (EventHandler)obj != newHandler))
                Interlocked.Exchange(ref currentHandler, currentHandler += newHandler);
        }

        public static void AddEvent<T>(ref EventHandler<T> currentHandler, EventHandler<T> newHandler) where T : EventArgs
        {
            if (newHandler == null)
                return;

            var eventCopy = currentHandler;
            if (eventCopy == null
                || eventCopy.GetInvocationList().All(obj => (EventHandler<T>)obj != newHandler))
                Interlocked.Exchange(ref currentHandler, currentHandler += newHandler);
        }

        public static void RemoveEvent(ref EventHandler currentHandler, EventHandler toRemove)
        {
            if (currentHandler == null)
                return;

            Interlocked.Exchange(ref currentHandler, currentHandler -= toRemove);
        }

        public static void RemoveEvent<T>(ref EventHandler<T> currentHandler, EventHandler<T> toRemove) where T : EventArgs
        {
            if (currentHandler == null)
                return;

            Interlocked.Exchange(ref currentHandler, currentHandler -= toRemove);
        }

        public static void InvokeIfIsNotNull<T>(T referenceObject, Action<T> action) where T : class
        {
            var copy = referenceObject;
            if (copy == null)
                return;

            action(copy);
        }

        #endregion
    }
}