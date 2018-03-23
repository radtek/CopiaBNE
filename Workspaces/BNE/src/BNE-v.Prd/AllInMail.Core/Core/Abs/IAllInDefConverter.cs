using System;
namespace AllInMail.Core
{
    public interface IAllInDefConverter
    {
        string Delimiter { get; set; }
        string GetDeclaration();
        string[] GetDefiniedFields();
        string Parse(object model);
    }

    public interface IAllInDefConverter<T> : IAllInDefConverter
    {
        string Parse(T modelToParse);
    }
}
