using Hanssens.Net;
using PineappleSite.Presentation.Contracts;

namespace PineappleSite.Presentation.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly LocalStorage _localStorage;

        public LocalStorageService()
        {
            var config = new LocalStorageConfiguration
            {
                AutoLoad = true,
                AutoSave = true,
                Filename = "PineappleSite"
            };

            _localStorage = new LocalStorage(config);
        }

        public void ClearStorage(List<string> keys)
        {
            foreach (var key in keys)
            {
                _localStorage.Remove(key);
            }
        }

        public bool Exists(string key)
        {
            return _localStorage.Exists(key);
        }

        public Type GetStorageValue<Type>(string key)
        {
            return _localStorage.Get<Type>(key);
        }

        public void SetStorageValue<Type>(string key, Type storageValue)
        {
            _localStorage.Store(key, storageValue);
            _localStorage.Persist();
        }
    }
}