using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
public class PngFormatter : BufferedMediaTypeFormatter
{
    public PngFormatter()
    {
        this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));
    }

    public override bool CanReadType(Type type)
    {
        return false;
    }
}