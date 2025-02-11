namespace SL2Lib.Data
{
    public interface IDataSaver
    {
        string Path { get; }

        void Persist();
    }
}
