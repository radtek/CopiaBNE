using System;
namespace AllInMail.Template
{
    public interface IStarterSettings
    {
        int BatchSize { get; set; }
        int BufferSize { get; set; }
        DateTime? FromDate { get; set; }
        int MaxQuantity { get; set; }
        int StartAboveTargetId { get; set; }
        bool WriterHeader { get; set; }
    }
}
