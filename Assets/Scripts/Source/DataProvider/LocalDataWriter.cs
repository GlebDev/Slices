using System;
using System.IO;
using System.Linq;
using Source.DataProvider.Interfaces;
using UnityEngine;

namespace Source.DataProvider
{
    public class LocalDataWriter : ILocalDataWriter
    {
        public void CreateDirectory(string path, PathType pathType = PathType.Persistent)
        {
            try
            {
                path = FullPath(path, pathType);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public void DeleteDirectory(string path, PathType pathType = PathType.Persistent)
        {
            try
            {
                path = FullPath(path, pathType);
                var files = Directory.GetFiles(path);
                var dirs = Directory.GetDirectories(path);

                foreach (var file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (var dir in dirs)
                    DeleteDirectory(dir);

                Directory.Delete(path, false);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public void Delete(string path, PathType pathType = PathType.Persistent)
        {
            try
            {
                path = FullPath(path, pathType);
                if (File.Exists(path))
                {
                    File.SetAttributes(path, FileAttributes.Normal);
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public bool FileExists(string path, PathType pathType = PathType.Persistent)
        {
            path = FullPath(path, pathType);
            return File.Exists(path);
        }

        public bool DirectoryExists(string path, PathType pathType = PathType.Persistent)
        {
            path = FullPath(path, pathType);
            return Directory.Exists(path);
        }

        public void WriteAllBytes(string path, byte[] binaryData, PathType pathType = PathType.Persistent)
        {
            try
            {
                path = FullPath(path, pathType);
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                    File.SetAttributes(path, FileAttributes.Normal);
                }

                File.WriteAllBytes(path, binaryData);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public void WriteAllText(string path, string text, PathType pathType = PathType.Persistent)
        {
            try
            {
                path = FullPath(path, pathType);
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                    File.SetAttributes(path, FileAttributes.Normal);
                }

                File.WriteAllText(path, text);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public FileInfo[] GetFileInfos(string path, PathType pathType = PathType.Persistent)
        {
            path = FullPath(path, pathType);
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] files;
            if (Directory.Exists(path))
                files = d.GetFiles();
            else
            {
                Debug.LogError($"The specified path does not exist: {path}");
                files = new FileInfo[0];
            }

            return files;
        }

        public byte[] ReadAllBytes(string path, PathType pathType = PathType.Persistent)
        {
            try
            {
                path = FullPath(path, pathType);
                var binaryData = File.ReadAllBytes(path);
                return binaryData;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            return new byte[0];
        }

        public string ReadAllText(string path, PathType pathType = PathType.Persistent)
        {
            path = FullPath(path, pathType);
            string text;
            if (File.Exists(path))
                text = File.ReadAllText(path);
            else
            {
                Debug.LogError($"The specified path does not exist: {path}");
                text = string.Empty;
            }

            return text;
        }

        public string[] GetFiles(string path, PathType pathType = PathType.Persistent)
        {
            path = FullPath(path, pathType);
            string[] files;
            if (Directory.Exists(path))
                files = Directory.GetFiles(path);
            else
            {
                Debug.LogError($"The specified path does not exist: {path}");
                files = new string[0];
            }

            return files;
        }

        public string[] GetFileNames(string path, string filter = "*", PathType pathType = PathType.Persistent)
        {
            path = FullPath(path, pathType);
            string[] files;
            if (Directory.Exists(path))
                files = Directory.GetFiles(path, filter).Select(Path.GetFileNameWithoutExtension).ToArray();
            else
            {
                Debug.LogError($"The specified path does not exist: {path}");
                files = new string[0];
            }

            return files;
        }

        private static string FullPath(string path, PathType pathType)
        {
            switch (pathType)
            {
                case PathType.Persistent:
                    return $"{Application.persistentDataPath}{path}";
                case PathType.Cash:
                    return $"{Application.temporaryCachePath}{path}";
                case PathType.Streaming:
                    return $"{Application.streamingAssetsPath}{path}";
                case PathType.Application:
                    return $"{Application.dataPath}{path}";
                case PathType.Console:
                    return $"{Application.consoleLogPath}{path}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(pathType), pathType, null);
            }
        }
    }
}