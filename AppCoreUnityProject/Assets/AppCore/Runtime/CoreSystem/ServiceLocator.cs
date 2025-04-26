#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AppCore.Runtime
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> InstanceDict = new();
        private static readonly Dictionary<Type, Type> TypeDict = new();

        /// <summary>
        /// 単一インスタンスを登録する
        /// 呼び直すと上書き登録する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="instance">インスタンス</param>
        public static void Register<T>(T instance) where T : class
        {
            InstanceDict[typeof(T)] = instance;
        }

        /// <summary>
        /// 型を登録する
        /// このメソッドで登録するとResolveしたときに都度インスタンス生成する
        /// </summary>
        /// <typeparam name="TContract">抽象型</typeparam>
        /// <typeparam name="TConcrete">具現型</typeparam>
        public static void Register<TContract, TConcrete>() where TContract : class
        {
            TypeDict[typeof(TContract)] = typeof(TConcrete);
        }

        /// <summary>
        /// 型を指定して登録されているインスタンスを取得する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <returns>インスタンス</returns>
        public static T? Resolve<T>() where T : class
        {
            var type = typeof(T);

            if (InstanceDict.TryGetValue(type, out var value))
            {
                // 事前に生成された単一インスタンスを返す
                return value as T;
            }

            if (TypeDict.TryGetValue(type, out var value1))
            {
                // インスタンスを生成して返す
                return Activator.CreateInstance(value1) as T;
            }

            Debug.LogWarning($"ServiceLocator: {typeof(T).Name} not found.");
            return null;
        }

        /// <summary>
        /// Componentを取得する。
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <returns> インスタンス </returns>
        public static T Get<T>() where T : Component
        {
            return ComponentLocator.GetInternal<T>();
        }

        /// <summary>
        /// Componentをキャッシュになければ、追加する
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void TryAddCache<T>(T value) where T : Component
        {
            if (ComponentLocator.IsCached<T>())
                return;

            ComponentLocator.AddCache(value);
        }
        
        #region ComponentLocator

        private static class ComponentLocator
        {
            private const float GCInterval = 31f;
            private static readonly Dictionary<Type, IStaticCache> Cache = new();
            private static float lastGCAt;
            private static readonly List<Type> CompTypesToRemove = new();
            
            public static void AddCache<T>(T value) where T : Component
            {
                StaticCache<T>.Value = value;
                Cache.Add(typeof(T), StaticCache<T>.Instance);
            }

            public static bool IsCached<T>() where T : Component
            {
                return Cache.ContainsKey(typeof(T));
            }

            public static T GetInternal<T>() where T : Component
            {
                var comp = GetOrNull<T>();
                if (comp) return comp;

                var compType = typeof(T);
                var gameObject = new GameObject(compType.Name, compType);
                comp = gameObject.GetComponent<T>();
                if (comp)
                {
                    UnCache<T>();
                    AddCache(comp);
                    return comp;
                }
                return null!;
            }

            private static T? GetOrNull<T>() where T : Component
            {
                GCIfNeeded();

                var value = StaticCache<T>.Value;
                if (value)
                {
                    return value;
                }

                UnCache<T>();

                value = UnityEngine.Object.FindObjectOfType<T>();
                if (value)
                {
                    AddCache(value);
                    return value;
                }

                return null;
            }
            
            private static void UnCache<T>() where T : Component
            {
                StaticCache<T>.Value = null;
                Cache.Remove(typeof(T));
            }

            private static void GCIfNeeded()
            {
                var now = Time.unscaledTime;
                if (GCInterval < now - lastGCAt)
                {
                    lastGCAt = now;
                    GC();
                }
            }

            private static void GC()
            {
                foreach (var kv in Cache.Where(kv => kv.Value == null))
                {
                    CompTypesToRemove.Add(kv.Key);
                }
                foreach (var compType in CompTypesToRemove)
                {
                    Cache.Remove(compType);
                }
                CompTypesToRemove.Clear();
            }

            private interface IStaticCache
            {
                Component? Component { get; }
            }

            private class StaticCache<T> : IStaticCache where T : Component
            {
                internal static readonly StaticCache<T> Instance = new();

                internal static T? Value = null!;
                public Component? Component
                {
                    get => Value;
                    set => Value = value as T;
                }
            }
        }

        #endregion ComponentLocator

    }
}