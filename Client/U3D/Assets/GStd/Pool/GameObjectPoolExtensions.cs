namespace GStd
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class GameObjectPoolExtensions
    {
        [DebuggerHidden]
        private static IEnumerator smethod_0(GameObjectPool gameObjectPool_0, AssetID assetID_0, InstantiateQueue instantiateQueue_0, int int_0, Action<GameObject> action_0)
        {
            return new Class62 { gameObjectPool_0 = gameObjectPool_0, assetID_0 = assetID_0, instantiateQueue_0 = instantiateQueue_0, int_0 = int_0, action_0 = action_0, gameObjectPool_1 = gameObjectPool_0, assetID_1 = assetID_0, instantiateQueue_1 = instantiateQueue_0, int_2 = int_0, action_1 = action_0 };
        }

        public static void SpawnAsset(this GameObjectPool pool, AssetID assetID, Action<GameObject> complete)
        {
            Scheduler.RunCoroutine(smethod_0(pool, assetID, null, 0, complete));
        }

        public static void SpawnAsset(this GameObjectPool pool, string assetBundle, string assetName, Action<GameObject> complete)
        {
            AssetID tid = new AssetID(assetBundle, assetName);
            Scheduler.RunCoroutine(smethod_0(pool, tid, null, 0, complete));
        }

        public static void SpawnAssetWithQueue(this GameObjectPool pool, AssetID assetID, InstantiateQueue instantiateQueue, int instantiatePriority, Action<GameObject> complete)
        {
            Scheduler.RunCoroutine(smethod_0(pool, assetID, instantiateQueue, instantiatePriority, complete));
        }

        public static void SpawnAssetWithQueue(this GameObjectPool pool, string assetBundle, string assetName, InstantiateQueue instantiateQueue, int instantiatePriority, Action<GameObject> complete)
        {
            AssetID tid = new AssetID(assetBundle, assetName);
            Scheduler.RunCoroutine(smethod_0(pool, tid, instantiateQueue, instantiatePriority, complete));
        }

        [CompilerGenerated]
        private sealed class Class62 : IEnumerator<object>, IEnumerator, IDisposable
        {
            internal Action<GameObject> action_0;
            internal Action<GameObject> action_1;
            internal AssetID assetID_0;
            internal AssetID assetID_1;
            internal GameObjectPool gameObjectPool_0;
            internal GameObjectPool gameObjectPool_1;
            internal InstantiateQueue instantiateQueue_0;
            internal InstantiateQueue instantiateQueue_1;
            internal int int_0;
            internal int int_1;
            internal int int_2;
            internal object object_0;
            internal WaitSpawnGameObject waitSpawnGameObject_0;

            [DebuggerHidden]
            public void Dispose()
            {
                this.int_1 = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.int_1;
                this.int_1 = -1;
                switch (num)
                {
                    case 0:
                        this.waitSpawnGameObject_0 = this.gameObjectPool_0.SpawnAssetWithQueue(this.assetID_0, this.instantiateQueue_0, this.int_0);
                        this.object_0 = this.waitSpawnGameObject_0;
                        this.int_1 = 1;
                        return true;

                    case 1:
                        if (this.waitSpawnGameObject_0.Error == null)
                        {
                            this.action_0(this.waitSpawnGameObject_0.Instance);
                            this.int_1 = -1;
                            break;
                        }
                        UnityEngine.Debug.LogError("GameObjectPool.Spawn failed: " + this.waitSpawnGameObject_0.Error);
                        this.action_0(null);
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.object_0;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.object_0;
                }
            }
        }
    }
}

