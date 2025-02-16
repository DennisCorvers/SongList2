namespace SL2Lib.Data
{
    public interface IDataLoaderFactory
    {
        IDataLoader CreateDataLoader(string? filePath);
    }
}
