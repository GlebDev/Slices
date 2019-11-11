using UnityEngine;

namespace Source.Pool.Interfaces
{

    public interface IPush
    {
        void Push<T>(T obj, Transform parent = null) where T : UnityPoolObject;
    }
}