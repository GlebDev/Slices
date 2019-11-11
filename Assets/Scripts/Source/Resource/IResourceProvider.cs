using Source.Pool;
using UnityEngine;

namespace Source.Resource
{
    public interface IResourceProvider
    {
        TView Get<TView>() where TView : UnityPoolObject;

        TView Get<TView>(Transform parentTransform = null)
            where TView : UnityPoolObject;
        
        TView Get<TView>(Vector3 position, Quaternion rotation, Transform parentTransform = null)
            where TView : UnityPoolObject;

        void Push<TView>(TView view) where TView : UnityPoolObject;
    }
}