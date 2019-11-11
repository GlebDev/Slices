using System.IO;

namespace Source.DataProvider.Interfaces
{
    public interface ILocalDataWriter
    {
        void CreateDirectory(string path, PathType pathType = PathType.Persistent);
        void DeleteDirectory(string path, PathType pathType = PathType.Persistent);
        void Delete(string path, PathType pathType = PathType.Persistent);
        bool DirectoryExists(string path, PathType pathType = PathType.Persistent);
        bool FileExists(string path, PathType pathType = PathType.Persistent);
        void WriteAllBytes(string path, byte[] binaryData, PathType pathType = PathType.Persistent);
        void WriteAllText(string path, string text, PathType pathType = PathType.Persistent);
        byte[] ReadAllBytes(string path, PathType pathType = PathType.Persistent);
        string ReadAllText(string path, PathType pathType = PathType.Persistent);
        string[] GetFiles(string path, PathType pathType = PathType.Persistent);
        FileInfo[] GetFileInfos(string path, PathType pathType = PathType.Persistent);
        string[] GetFileNames(string path, string filter = "*", PathType pathType = PathType.Persistent);
    }
}