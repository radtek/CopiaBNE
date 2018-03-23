using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatsJob.Model
{
    public class ChannelLog
    {
        public Channel Channel { get; set; }

        public DateTime Date { get; set; }

        public string Text { get; set; }

        public FaultType FaultType { get; set; }

        public int Id { get; set; }

        public ChannelLog() { }

        public ChannelLog(Channel channel, FaultType faultType, String text)
        {
            Channel = channel;
            Date = DateTime.Now;
            Text = text;
        }
    }
}
