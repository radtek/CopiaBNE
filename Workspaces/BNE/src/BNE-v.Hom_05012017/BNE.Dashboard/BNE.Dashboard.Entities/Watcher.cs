namespace BNE.Dashboard.Entities
{
    public class Watcher
    {

        public int WatcherId { get; set; }

        public Status Status { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        //public int TimeToRefresh { get; set; }

        public virtual WindowsService WindowsService { get; set; }
        public virtual MessageQueue MessageQueue { get; set; }
        public virtual SiteResponse SiteResponse { get; set; }

    }
}
