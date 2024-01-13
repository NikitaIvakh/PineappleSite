namespace PineappleSite.Presentation.Contracts
{
    public interface ILocalStorageService
    {
        void ClearStorage(List<string> keys);

        bool Exists(string key);

        Type GetStorageValue<Type>(string key);

        void SetStorageValue<Type>(string key, Type storageValue);
    }
}