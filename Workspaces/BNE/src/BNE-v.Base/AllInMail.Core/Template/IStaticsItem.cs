using System;
namespace AllInMail.Template
{
    public interface IStaticsItem
    {
        TimeSpan Average { get; }
        TimeSpan Max { get;  }
        TimeSpan Min { get; }
        int TotalCount { get;  }
        void Increment(TimeSpan timeSpan);

    }
}
