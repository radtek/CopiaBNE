namespace BNE.Dashboard.Entities
{
    public class MessageQueue
    {
        public int MessageQueueId { get; set; }

        public string MessageQueueName { get; set; }

        public string MessageQueueServer { get; set; }

        public int MaximumMessageCount { get; set; }

    }
}
