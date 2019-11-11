using System;
using System.Collections.Generic;
using System.Linq;
using Source.Pool.Interfaces;
using UnityEngine;
using Zenject;

namespace Source.Pool
{
    public class UnityPool : IPush
    {
        private readonly Dictionary<Type, List<UnityPoolObject>> _objects =
            new Dictionary<Type, List<UnityPoolObject>>(TypeComparer);

        private readonly Factory _factory;
        private readonly Transform _root;

        public UnityPool(Factory factory, Transform root = null)
        {
            _factory = factory;
            _root = root;
        }

        public void Push<T>(T value, Transform parent) where T : UnityPoolObject
        {
            value.OnPush();
            value.Transform.SetParent(parent ? parent : _root, false);
            var type = value.GetType();
            if (!_objects.TryGetValue(type, out var list))
                list = new List<UnityPoolObject>();

            if (!list.Contains(value))
                list.Add(value);
            _objects[type] = list;
        }

        public T Pop<T>() where T : UnityPoolObject
        {
            return Pop<T>(Vector3.zero, Quaternion.identity);
        }

        public T Pop<T>(Transform parent) where T : UnityPoolObject
        {
            var type = typeof(T);
            if (!_objects.TryGetValue(type, out var list))
            {
                list = new List<UnityPoolObject>();
              
            }

            T component;
            if (list.Count > 0)
            {
                component = list[0] as T;
                list.RemoveAt(0);
                component.Transform.SetParent(parent ? parent : _root, false);
            }
            else
            {
                component = _factory.Create<T>(parent ? parent : _root);
                component.UnityPoolManager = this;
               
            }

            component.OnPop();
            return component;
        }

        public T Pop<T>(Vector3 position, Quaternion rotation, Transform parentTransform = null)
            where T : UnityPoolObject
        {
            var type = typeof(T);
            if (!_objects.TryGetValue(type, out var list))
                list = new List<UnityPoolObject>();

            T component;
            if (list.Count > 0)
            {
                component = list[0] as T;
                list.RemoveAt(0);
                component.Transform.position = position;
                component.Transform.rotation = rotation;
                component.Transform.SetParent(parentTransform ? parentTransform : _root, false);
            }
            else
            {
                component = _factory.Create<T>(position, rotation, parentTransform ? parentTransform : _root);
                component.UnityPoolManager = this;
            }

            component.OnPop();
            return component;
        }

        public class Factory
        {
            private readonly DiContainer _container;
            private readonly Dictionary<Type, UnityPoolObject> _prefabs;

            public Factory(UnityPoolObject[] prefabs, DiContainer container)
            {
                _container = container;
                _prefabs = prefabs.ToDictionary(o => o.GetType());
            }

            public T Create<T>(Vector3 position, Quaternion rotation, Transform parentTransform = null)
                where T : UnityPoolObject
            {
                var component = _container.InstantiatePrefab(_prefabs[typeof(T)], position, rotation, parentTransform)
                    .GetComponent<UnityPoolObject>();
                return component as T;
            }

            public T Create<T>(Transform parentTransform = null)
                where T : UnityPoolObject
            {
                var component = _container.InstantiatePrefab(_prefabs[typeof(T)], parentTransform)
                    .GetComponent<UnityPoolObject>();
                return component as T;
            }
        }

        private sealed class TypeEqualityComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                return x != null && x == y;
            }

            public int GetHashCode(Type obj)
            {
                return obj.GetHashCode();
            }
        }

        public static IEqualityComparer<Type> TypeComparer { get; } = new TypeEqualityComparer();
    }
}