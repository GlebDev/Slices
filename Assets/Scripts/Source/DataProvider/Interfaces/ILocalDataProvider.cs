namespace Source.DataProvider.Interfaces
{
    public interface ILocalDataProvider
    {
        void Save<T>(T obj);
        T Load<T>();
        bool Exist<T>();
        void Delete<T>();

    }
}