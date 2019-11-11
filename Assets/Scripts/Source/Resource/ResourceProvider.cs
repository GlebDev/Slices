using Source.Pool;
using UnityEngine;

namespace Source.Resource
{
    public class ResourceProvider : IResourceProvider
    {
        private readonly UnityPool _viewEnumPool;

        public ResourceProvider(UnityPool viewEnumPool)
        {
            _viewEnumPool = viewEnumPool;
        }

        public TView Get<TView>() where TView : UnityPoolObject
        {
            return _viewEnumPool.Pop<TView>();
        }

        public TView Get<TView>(Transform parentTransform = null) where TView : UnityPoolObject
        {
            return _viewEnumPool.Pop<TView>(parentTransform);
        }

        public TView Get<TView>(Vector3 position, Quaternion rotation, Transform parentTransform = null) where TView : UnityPoolObject
        {
            return _viewEnumPool.Pop<TView>(position, rotation, parentTransform);
        }
        
        public void Push<TView>(TView view) where TView : UnityPoolObject
        {
            view.Push();
        }
    }
}