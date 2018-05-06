using UnityEngine;
using System.Collections.Generic;


namespace GStd.Asset
{
    public class SpawnedItem
    {
        public GameObject inst;
        public float freeTime;
    }

    public class PoolItem
    {
        public GameObject prefab;
        public string assetBundleName;
        public string assetName;
        public int maxCount;
        public float duration2release;

        public List<SpawnedItem> spawned = new List<SpawnedItem>();
        public List<SpawnedItem> despawned = new List<SpawnedItem>();

        public Transform root;

        public GameObject Spawn(Vector3 pos, Quaternion rot, Transform parent)
		{
			SpawnedItem ret;

			if (despawned.Count > 0)
			{
				ret = this.despawned[0];
                this.despawned.RemoveAt(0);
                ret.freeTime = -1;
                ret.inst.transform.SetParent(parent);
                ret.inst.transform.SetPositionAndRotation(pos, rot);
                ret.inst.SetActive(true);
                this.spawned.Add(ret);
			}
			else
			{
                if (this.maxCount != -1 && this.spawned.Count >= this.maxCount)
                {
                    ret = this.spawned[0];
                    ret.inst.transform.SetParent(parent);
                    ret.inst.transform.SetPositionAndRotation(pos, rot);
                }
                else
                {
                    ret = new SpawnedItem(){inst = GameObject.Instantiate(prefab, pos, rot, parent)};
                    ret.inst.SetActive(true);
                    this.spawned.Add(ret);
                }
			}
			
			return ret.inst;
		}

        public void Despawn(GameObject inst, float time2release=3.0f)
		{
            if (inst == null)
            {
                Debug.LogError("try to despawn null obj");
                return;
            }
            
			if (this.spawned.Count == 0 || !this.spawned.Exists((x) => {return x.inst == inst; }))
			{
				Debug.LogError(string.Format("try to despawn {0} not spawned by this pool.", inst.name));
                Object.Destroy(inst);
				return;
			}

            this.spawned.RemoveAll((x) => {return x.inst == inst;});
            inst.SetActive(false);
            inst.transform.SetParent(this.root);
            inst.transform.localPosition = Vector3.zero;

			this.despawned.Add(
                new SpawnedItem(){
                    inst = inst, 
                    freeTime = Time.realtimeSinceStartup + time2release
                    }
                );
		}

        public void Update()
        {
            this.despawned.RemoveAll((x) => {
                if (x.freeTime <= Time.realtimeSinceStartup)
                {
                    Object.Destroy(x.inst);
                    return true;
                }
                return false;
                });
        }
    }

    public class GameObjectPool 
    {
        private Dictionary<string, Dictionary<string, PoolItem>> pool;
        private Dictionary<GameObject, PoolItem> objMap;
        private Dictionary<GameObject, PoolItem> prefabMap;

        private Transform root;

        public GameObjectPool()
        {
            this.pool= new Dictionary<string, Dictionary<string, PoolItem>>();
            this.objMap = new Dictionary<GameObject, PoolItem>();
            this.prefabMap = new Dictionary<GameObject, PoolItem>();

            var gameObject = GameObject.Find("GameObjectPool");
            if (gameObject == null)
            {
                gameObject = new GameObject("GameObjectPool");
            }
            Object.DontDestroyOnLoad(gameObject);

            root = gameObject.transform;
        }

        public GameObject Spawn(string assetBundleName, string assetName)
        {
            return this.Spawn(assetBundleName, assetName, Vector3.zero, Quaternion.identity, null);
        }

        public GameObject Spawn(string assetBundleName, string assetName, Vector3 pos, Quaternion rot, Transform parent)
        {
            if (!pool.ContainsKey(assetBundleName))
			{
                var prefab = AssetManager.LoadAsset<GameObject>(assetBundleName, assetName);
                
                float duration2release = 3;
                int maxCount = -1;
                var entity = prefab.GetComponent<GameObjectPoolEntity>();
                if (entity != null)
                {
                    duration2release = entity.duration2release;
                    maxCount = entity.maxCount;
                }

				var item = new PoolItem(){ 
                    prefab = prefab, 
                    root=root,
                    duration2release = duration2release,
                    maxCount = maxCount
                    };

				pool.Add(
					assetBundleName, 
                    new Dictionary<string, PoolItem>(){{assetName, item}}
				);

				this.objMap[prefab] = item;

                var ret = item.Spawn(pos, rot, parent);
                objMap[ret] = item;
                return ret;
			}

			if (!pool[assetBundleName].ContainsKey(assetName))
			{
				var prefab = AssetManager.LoadAsset<GameObject>(assetBundleName, assetName);
               
                float duration2release = 3;
                int maxCount = -1;
                var entity = prefab.GetComponent<GameObjectPoolEntity>();
                if (entity != null)
                {
                    duration2release = entity.duration2release;
                    maxCount = entity.maxCount;
                }

				var item = new PoolItem(){ 
                    prefab = prefab, 
                    root=root,
                    duration2release = duration2release,
                    maxCount = maxCount
                    };

				pool[assetBundleName].Add(assetName, item);

                var ret = item.Spawn(pos, rot, parent);
                objMap[ret] = item;
                return ret;
			}

            {
                var item = pool[assetBundleName][assetName];
                var ret = item.Spawn(pos, rot, parent);
                objMap[ret] = item;
                return ret;
            }
        }

        public GameObject Spawn(GameObject prefab)
        {
            return this.Spawn(prefab, Vector3.zero, Quaternion.identity, null);
        }

        public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent)
        {   
            if (prefab == null)
            {
                Debug.LogError("try to spawn null prefab");
                return null;
            }

            if (!this.prefabMap.ContainsKey(prefab))
            {
                float duration2release = 3;
                int maxCount = -1;
                var entity = prefab.GetComponent<GameObjectPoolEntity>();
                if (entity != null)
                {
                    duration2release = entity.duration2release;
                    maxCount = entity.maxCount;
                }

                var item = new PoolItem(){ 
                    prefab = prefab, 
                    root=root,
                    duration2release = duration2release,
                    maxCount = maxCount
                    };
                
                var ret = item.Spawn(pos, rot, parent);

                objMap[ret] = item;
                prefabMap[prefab] = item;

                return ret;
            }

            {
                var item = this.prefabMap[prefab];
                var ret = item.Spawn(pos, rot, parent);
                objMap[ret] = item;
                return ret;
            }
        }

        public void Despawn(GameObject inst)
        {
            if (!this.objMap.ContainsKey(inst))
			{
				Debug.LogError("despawn failed");
				return;
			}
			objMap[inst].Despawn(inst);
        }

        void Update()
        {
            foreach(var item in this.objMap.Values)
            {
                item.Update();
            }
        }
    }
}