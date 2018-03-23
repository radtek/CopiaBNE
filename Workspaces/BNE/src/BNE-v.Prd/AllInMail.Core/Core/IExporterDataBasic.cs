using System;
namespace AllInMail.Core
{
    public interface IExporterDataBasic<T> : IExporterDataBasic
    {
        AllInMail.Core.IAllInDefConverter<T> ConverterTyped { get; }
    }
    public interface IExporterDataBasic
    {
        AllInMail.Core.IAllInDefConverter Converter { get; }
        AllInMail.Template.IMainExporter MainVm { get; }
        System.Threading.Tasks.Task<string> Process(Lazy<System.IO.Stream> lazyOutputStream, System.Threading.CancellationToken token);
    }
}
