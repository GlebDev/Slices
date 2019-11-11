using System.Collections.Generic;
using Slice;
using Source.Pool.PoolObjects;
using Source.Resource;
using UnityEngine;
using Zenject;

namespace View
{
    /// <summary>
    ///Class that create and clear slice prefabs on transform
    /// </summary>
    public class SliceDisplay: MonoBehaviour
    {
        private IResourceProvider _resourceProvider;
        private readonly float _angle = 60f;
        private List<SingleSlice> _slicePrefabs = new List<SingleSlice>();
        
        public void SetResourceProvider(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        public void DrawSlice(SliceSet sliceSet)
        {
            var bitmap = (int) sliceSet.Value;
            //Set the start angle 
            var angle = -_angle / 2f;
            while (bitmap > 0)
            {
                //Get the first bit from the bitmap
                if ((bitmap & 1) != 0)
                {
                    var obj = _resourceProvider.Get<SingleSlice>(Vector3.zero, Quaternion.Euler(0f, 0f, angle),
                        this.transform);
                    obj.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                
                    _slicePrefabs.Add(obj);
                }

                angle -= _angle;
                bitmap >>= 1;
            }
        }

        public void Clear()
        {
            foreach (var obj in _slicePrefabs)
            {
                _resourceProvider.Push(obj);
            }

            _slicePrefabs.Clear();
        }
    }
}