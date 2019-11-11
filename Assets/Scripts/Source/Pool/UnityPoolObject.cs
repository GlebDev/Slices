using Source.Pool.Interfaces;
using UnityEngine;

namespace Source.Pool
{
    public class UnityPoolObject : MonoBehaviour
    {
        private Transform _myTransform;
        public Transform Transform => _myTransform ? _myTransform : _myTransform = GetComponent<Transform>();
        public IPush UnityPoolManager { protected get; set; }
        public virtual void OnPop() // Constructor for the pool
        {
            gameObject.SetActive(true);
        }

        public virtual void OnPush() // Destructor for the pool
        {
            gameObject.SetActive(false);
        }

        /// <inheritdoc />
        public virtual void Push()
        {
            UnityPoolManager.Push(this);
        }
    }
}