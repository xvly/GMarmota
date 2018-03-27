namespace GStd
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Assertions;

    [DisallowMultipleComponent]
    public sealed class GameObjectPool : Singleton<GameObjectPool>
    {
        private Dictionary<AssetID, GameObjectCache> dictionary_0 = new Dictionary<AssetID, GameObjectCache>();
        private Dictionary<GameObject, GameObjectCache> dictionary_1 = new Dictionary<GameObject, GameObjectCache>();
        private Dictionary<GameObject, GameObjectCache> dictionary_2 = new Dictionary<GameObject, GameObjectCache>();
        private float float_0 = 60f;
        [CompilerGenerated]
        private static Func<AssetID, GameObjectCache, bool> func_0;
        [CompilerGenerated]
        private static Func<GameObject, GameObjectCache, bool> func_1;
        [CompilerGenerated]
        private static Func<AssetID, GameObjectCache, bool> func_2;
        [CompilerGenerated]
        private static Func<GameObject, GameObjectCache, bool> func_3;
        private int int_0 = 0x10;
        private static GStd.Logger logger_0 = LogSystem.GetLogger("GameObjectPool");
        private RepeatedTimer repeatedTimer_0;
        private Transform transform_0;

        public GameObjectPool()
        {
            this.repeatedTimer_0 = RepeatedTimer.Repeat(10f, 10f, new Action(this.method_0));
        }

        public void Clear()
        {
            this.dictionary_0.Clear();
            this.dictionary_1.Clear();
            this.dictionary_2.Clear();
        }

        public void ClearAllUnused()
        {
            if (func_0 == null)
            {
                func_0 = new Func<AssetID, GameObjectCache, bool>(GameObjectPool.smethod_0);
            }
            this.dictionary_0.RemoveAll<AssetID, GameObjectCache>(func_0);
            if (func_1 == null)
            {
                func_1 = new Func<GameObject, GameObjectCache, bool>(GameObjectPool.smethod_1);
            }
            this.dictionary_1.RemoveAll<GameObject, GameObjectCache>(func_1);
        }

        ~GameObjectPool()
        {
            this.repeatedTimer_0.Dispose();
        }

        public void Free(GameObject instance)
        {
            if (instance == null)
            {
                logger_0.LogError("Try to free a null GameObject.");
            }
            else
            {
                GameObjectCache cache;
                if (!this.dictionary_2.TryGetValue(instance, out cache))
                {
                    object[] args = new object[] { instance.name };
                    logger_0.LogWarning("Try to free an instance {0} not allocated by this pool.", args);
                    UnityEngine.Object.Destroy(instance);
                }
                else
                {
                    this.dictionary_2.Remove(instance);
                    instance.SetActive(false);
                    if (this.transform_0 == null)
                    {
                        GameObject target = new GameObject("GameObjectPool");
                        UnityEngine.Object.DontDestroyOnLoad(target);
                        this.transform_0 = target.transform;
                    }
                    instance.transform.SetParent(this.transform_0);
                    cache.method_10(instance);
                }
            }
        }

        private void method_0()
        {
            if (func_2 == null)
            {
                func_2 = new Func<AssetID, GameObjectCache, bool>(GameObjectPool.smethod_2);
            }
            this.dictionary_0.RemoveAll<AssetID, GameObjectCache>(func_2);
            if (func_3 == null)
            {
                func_3 = new Func<GameObject, GameObjectCache, bool>(GameObjectPool.smethod_3);
            }
            this.dictionary_1.RemoveAll<GameObject, GameObjectCache>(func_3);
        }

        [CompilerGenerated]
        private static bool smethod_0(AssetID assetID_0, GameObjectCache gameObjectCache_0)
        {
            if (!string.IsNullOrEmpty(gameObjectCache_0.Error))
            {
                return true;
            }
            gameObjectCache_0.method_12();
            if ((!gameObjectCache_0.Loading && (gameObjectCache_0.CacheCount == 0)) && (gameObjectCache_0.SpawnCount == 0))
            {
                gameObjectCache_0.method_7();
                return true;
            }
            return false;
        }

        [CompilerGenerated]
        private static bool smethod_1(GameObject gameObject_0, GameObjectCache gameObjectCache_0)
        {
            if (!string.IsNullOrEmpty(gameObjectCache_0.Error))
            {
                return true;
            }
            gameObjectCache_0.method_12();
            if ((!gameObjectCache_0.Loading && (gameObjectCache_0.CacheCount == 0)) && (gameObjectCache_0.SpawnCount == 0))
            {
                gameObjectCache_0.method_7();
                return true;
            }
            return false;
        }

        [CompilerGenerated]
        private static bool smethod_2(AssetID assetID_0, GameObjectCache gameObjectCache_0)
        {
            if (!string.IsNullOrEmpty(gameObjectCache_0.Error))
            {
                return true;
            }
            gameObjectCache_0.method_11();
            if ((!gameObjectCache_0.Loading && (gameObjectCache_0.CacheCount == 0)) && (gameObjectCache_0.SpawnCount == 0))
            {
                gameObjectCache_0.method_7();
                return true;
            }
            return false;
        }

        [CompilerGenerated]
        private static bool smethod_3(GameObject gameObject_0, GameObjectCache gameObjectCache_0)
        {
            if (!string.IsNullOrEmpty(gameObjectCache_0.Error))
            {
                return true;
            }
            gameObjectCache_0.method_11();
            if ((!gameObjectCache_0.Loading && (gameObjectCache_0.CacheCount == 0)) && (gameObjectCache_0.SpawnCount == 0))
            {
                gameObjectCache_0.method_7();
                return true;
            }
            return false;
        }

        public GameObject Spawn(GameObject prefab, Transform parent)
        {
            GameObjectCache cache;
            Assert.IsNotNull<GameObject>(prefab);
            if (!this.dictionary_1.TryGetValue(prefab, out cache))
            {
                cache = new GameObjectCache(this.dictionary_2);
                cache.method_2(this.float_0);
                cache.method_4(this.int_0);
                this.dictionary_1.Add(prefab, cache);
                cache.method_5(prefab);
            }
            return cache.method_8(parent);
        }

        public T Spawn<T>(T prefab, Transform parent) where T: Component
        {
            Assert.IsNotNull<T>(prefab);
            T component = this.Spawn(prefab.gameObject, parent).GetComponent<T>();
            if (component == null)
            {
                object[] args = new object[] { typeof(T).Name };
                logger_0.LogError("Can not load prefab with componet: {0}", args);
                return null;
            }
            return component;
        }

        public WaitSpawnGameObject SpawnAsset(AssetID assetID)
        {
            return this.SpawnAssetWithQueue(assetID, null, 0);
        }

        public WaitSpawnGameObject SpawnAsset(string bundle, string asset)
        {
            return this.SpawnAssetWithQueue(bundle, asset, null, 0);
        }

        public WaitSpawnGameObject SpawnAssetWithQueue(AssetID assetID, InstantiateQueue instantiateQueue, int instantiatePriority)
        {
            GameObjectCache cache;
            Assert.IsFalse(assetID.IsEmpty);
            if (!this.dictionary_0.TryGetValue(assetID, out cache))
            {
                cache = new GameObjectCache(this.dictionary_2);
                cache.method_2(this.float_0);
                cache.method_4(this.int_0);
                this.dictionary_0.Add(assetID, cache);
                cache.method_6(assetID);
            }
            return new WaitSpawnGameObject(cache, instantiateQueue, instantiatePriority);
        }

        public WaitSpawnGameObject SpawnAssetWithQueue(string bundle, string asset, InstantiateQueue instantiateQueue, int instantiatePriority)
        {
            return this.SpawnAssetWithQueue(new AssetID(bundle, asset), instantiateQueue, instantiatePriority);
        }

        public IDictionary<AssetID, GameObjectCache> AssetCahces
        {
            get
            {
                return this.dictionary_0;
            }
        }

        public int DefaultInstancePoolCount
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
                foreach (KeyValuePair<AssetID, GameObjectCache> pair in this.dictionary_0)
                {
                    pair.Value.method_4(value);
                }
                foreach (KeyValuePair<GameObject, GameObjectCache> pair2 in this.dictionary_1)
                {
                    pair2.Value.method_4(value);
                }
            }
        }

        public float DefaultReleaseAfterFree
        {
            get
            {
                return this.float_0;
            }
            set
            {
                this.float_0 = value;
                foreach (KeyValuePair<AssetID, GameObjectCache> pair in this.dictionary_0)
                {
                    pair.Value.method_2(value);
                }
                foreach (KeyValuePair<GameObject, GameObjectCache> pair2 in this.dictionary_1)
                {
                    pair2.Value.method_2(value);
                }
            }
        }

        public IDictionary<GameObject, GameObjectCache> ObjectCaches
        {
            get
            {
                return this.dictionary_1;
            }
        }

        public float SweepLeftTime
        {
            get
            {
                return this.repeatedTimer_0.LeftTime;
            }
        }

        public float SweepTime
        {
            get
            {
                return this.repeatedTimer_0.RepeatTime;
            }
        }
    }
}

