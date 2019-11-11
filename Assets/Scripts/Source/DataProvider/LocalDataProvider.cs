using Source.DataProvider.Interfaces;
using UnityEngine;

namespace Source.DataProvider
{
    public class LocalDataProvider : ILocalDataProvider
    {
        private readonly ILocalDataWriter _localDataWriter;
        private const string PathFolder = "AppData";

        public LocalDataProvider(ILocalDataWriter localDataWriter)
        {
            _localDataWriter = localDataWriter;
        }

        public void Save<T>(T obj)
        {
            var pathFolder = $"/{PathFolder}";
            if (!_localDataWriter.DirectoryExists(pathFolder))
                _localDataWriter.CreateDirectory(pathFolder);

            var path = $"{pathFolder}/{typeof(T)}";
            var str = JsonUtility.ToJson(obj);
            _localDataWriter.WriteAllText(path, str);
        }

        public T Load<T>()
        {
            var path = $"/{PathFolder}/{typeof(T)}";
            if (!_localDataWriter.FileExists(path))
            {
                Debug.LogError($"The specified path does not exist: {path}");
                return default;
            }

            var str = _localDataWriter.ReadAllText(path);
            var result = JsonUtility.FromJson<T>(str); //todo use parser
            return result;
        }

        public bool Exist<T>()
        {
            return _localDataWriter.FileExists($"/{PathFolder}/{typeof(T)}");
        }

        public void Delete<T>()
        {
            if (Exist<T>())
                _localDataWriter.Delete($"/{PathFolder}/{typeof(T)}");
        }
    }
}