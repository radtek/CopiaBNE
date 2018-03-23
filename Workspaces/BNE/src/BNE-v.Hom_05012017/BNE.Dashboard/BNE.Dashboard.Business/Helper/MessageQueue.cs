using BNE.Dashboard.Entities;

namespace BNE.Dashboard.Business.Helper
{
    public class MessageQueue
    {
        private const string DefaultQueuePath = "FormatName:Direct=OS:{0}\\private$\\{1}";

        public static void VerifyBusinessRule(Entities.Watcher watcher)
        {
            var instanceName = string.Format(DefaultQueuePath, watcher.MessageQueue.MessageQueueServer, watcher.MessageQueue.MessageQueueName);

            var queue = new System.Messaging.MessageQueue(instanceName);

            int count = 0;
            var enumerator = queue.GetMessageEnumerator2();
            while (enumerator.MoveNext())
                count++;

            watcher.Amount = count;

            if (watcher.Amount > watcher.MessageQueue.MaximumMessageCount)
                watcher.Status = Status.ERROR;
        }
    }
}
