using System;
namespace AllInMail.Template
{
    public interface IMainExporter
    {
        IProgressIntegration Progress { get; set; }
        IStarterSettings StartSettings { get; set; }
    }
}
