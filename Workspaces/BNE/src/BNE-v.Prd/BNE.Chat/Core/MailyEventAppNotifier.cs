using System.Threading;
using BNE.Chat.Core.Interface;
using System;
using System.Linq;
using BNE.Chat.Core.EventModel;

namespace BNE.Chat.Core
{
    public sealed class MailyEventAppNotifier : INotifySessionEnd, INotifyAppBeginRequest, INotifyEndApp, IDisposable
    {
        private EventHandler<CurrentSessionEventArgs> _receiveSessionEnd;
        public event EventHandler<CurrentSessionEventArgs> ReceiveSessionEnd
        {
            add
            {
                if (value == null)
                    return;

                var eventCopy = _receiveSessionEnd;
                if (eventCopy == null
                    || eventCopy.GetInvocationList().All(obj => (EventHandler<CurrentSessionEventArgs>)obj != value))
                    Interlocked.Exchange(ref _receiveSessionEnd, _receiveSessionEnd += value);
            }
            remove
            {
                if (_receiveSessionEnd == null)
                    return;
                Interlocked.Exchange(ref _receiveSessionEnd, _receiveSessionEnd -= value);
            }
        }

        private void OnReceiveSessionEnd(object realSender, CurrentSessionEventArgs realArgs)
        {
            var handler = _receiveSessionEnd;
            if (handler != null)
                handler(realSender, realArgs);
        }

        public void RaiseSessionEnd(object realSender, CurrentSessionEventArgs realArgs)
        {
            OnReceiveSessionEnd(realSender, realArgs);
        }

        public void Dispose()
        {
            _receiveSessionEnd = null;
            _receiveAppBeginRequest = null;
            _receiveEndOfApp = null;
        }

        private EventHandler _receiveAppBeginRequest;

        public event EventHandler ReceiveAppBeginRequest
        {
            add
            {
                if (value == null)
                    return;

                var eventCopy = _receiveAppBeginRequest;
                if (eventCopy == null
                    || eventCopy.GetInvocationList().All(obj => (EventHandler)obj != value))
                {
                    Interlocked.Exchange(ref _receiveAppBeginRequest, _receiveAppBeginRequest += value);
                }

            }
            remove
            {
                if (_receiveAppBeginRequest == null)
                    return;

                Interlocked.Exchange(ref _receiveAppBeginRequest, _receiveAppBeginRequest -= value);
            }
        }

        private void OnReceiveAppBeginRequest(object realSender, EventArgs realArgs)
        {
            EventHandler handler = _receiveAppBeginRequest;
            if (handler != null)
                handler(realSender, realArgs);
        }

        public void RaiseBeginRequest(object realSender, EventArgs realArgs)
        {
            OnReceiveAppBeginRequest(realSender, realArgs);
        }

        private EventHandler _receiveEndOfApp;
        public event EventHandler ReceiveEndOfApp
        {
            add
            {
                if (value == null)
                    return;

                var eventCopy = _receiveEndOfApp;
                if (eventCopy == null
                    || eventCopy.GetInvocationList().All(obj => (EventHandler)obj != value))
                {
                    Interlocked.Exchange(ref _receiveEndOfApp, _receiveEndOfApp += value);
                }
                
            }
            remove
            {
                if (_receiveEndOfApp == null)
                    return;

                Interlocked.Exchange(ref _receiveEndOfApp, _receiveEndOfApp -= value);
            }
        }

        private void OnReceiveEndOfApp(object realSender, EventArgs realArgs)
        {
            var handler = _receiveEndOfApp;
            if (handler != null) handler(realSender, realArgs);
        }

        public void RaiseEndOfApp(object realSender, EventArgs realArgs)
        {
            OnReceiveEndOfApp(realSender, realArgs);
        }
    }

}