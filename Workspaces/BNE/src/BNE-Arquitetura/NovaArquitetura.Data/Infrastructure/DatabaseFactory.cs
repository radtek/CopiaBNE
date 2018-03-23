namespace NovaArquitetura.Data.Infrastructure 
{
public class DatabaseFactory : Disposable
{
    private NovaArquiteturaEntities _dataContext;
    public NovaArquiteturaEntities Get()
    {
        return _dataContext ?? (_dataContext = new NovaArquiteturaEntities());
    }
    protected override void DisposeCore()
    {
        if (_dataContext != null)
            _dataContext.Dispose();
    }
}
}
