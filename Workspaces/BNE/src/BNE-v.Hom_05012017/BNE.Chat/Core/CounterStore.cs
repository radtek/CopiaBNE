using System;
using System.Threading;

namespace BNE.Chat.Core
{
    public class CounterStore
    {
        public enum TransportType
        {
            WebSockets,
            ServerSentEvents,
            ForeverFrame,
            LongPolling
        }

        private int _webSockets;
        private int _serverSentEvents;
        private int _foreverFrame;
        private int _longPolling;

        public int WebSockets
        {
            get { return _webSockets; }
        }

        public int ServerSentEvents
        {
            get { return _serverSentEvents; }
        }

        public int ForeverFrame
        {
            get { return _foreverFrame; }
        }

        public int LongPolling
        {
            get { return _longPolling; }
        }

        public int ChangeCounter(TransportType transportType, bool toUp)
        {
            switch (transportType)
            {
                case TransportType.WebSockets:
                    return toUp ? Interlocked.Increment(ref _webSockets) : Interlocked.Decrement(ref _webSockets);
                case TransportType.ServerSentEvents:
                    return toUp ? Interlocked.Increment(ref _serverSentEvents) : Interlocked.Decrement(ref _serverSentEvents);
                case TransportType.ForeverFrame:
                    return toUp ? Interlocked.Increment(ref _foreverFrame) : Interlocked.Decrement(ref _foreverFrame);
                case TransportType.LongPolling:
                    return toUp ? Interlocked.Increment(ref _longPolling) : Interlocked.Decrement(ref _longPolling);
                default:
                    throw new ArgumentOutOfRangeException("transportType");
            }
        }

        public int Total
        {
            get { return _webSockets + _serverSentEvents + _foreverFrame + _longPolling; }
        }

        public static TransportType GetTransportType(string transportDescription)
        {
            if (transportDescription == null)
                throw new NullReferenceException("transportDescription");

            switch (transportDescription.ToLower())
            {
                case "websockets":
                    return TransportType.WebSockets;
                case "serversentevents":
                    return TransportType.ServerSentEvents;
                case "foreverframe":
                    return TransportType.ForeverFrame;
                case "longpolling":
                    return TransportType.LongPolling;
                default:
                    throw new ArgumentOutOfRangeException("transportDescription");
            }
            
        }

    }
}
