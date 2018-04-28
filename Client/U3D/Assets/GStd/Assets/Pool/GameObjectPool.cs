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
        public List<SpawnedItem> spawned = new List<SpawnedItem>();
        public List<SpawnedItem> despawned = new List<SpawnedItem>();

        public Transform root;

        public GameObject Spawn(Vector3 pos, Quaternion rot)
		{
			SpawnedItem ret;
			if (despawned.Count > 0)
			{
				ret = this.despawned[0];
                this.despawned.RemoveAt(0);
                ret.inst.transform.SetParent(null);
                ret.inst.transform.SetPositionAndRotation(pos, rot);
			}
			else
			{
                ret = new SpawnedItem(){inst = GameObject.Instantiate(prefab, pos, rot)};
			}

			this.spawned.Add(ret);
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

            this.spawned.RemoveAll((x) => {return x.inst==inst;});
            inst.SetActive(false);
            inst.transform.SetParent(this.root);
            inst.transform.localPosition = Vector3.zero;

			this.despawned.Add(
                new SpawnedItem(){
                    inst=inst, 
                    freeTime=Time.realtimeSinceStartup+3
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
        private Dictionary<GameObject, PoolItem> poolObjMap;

        private Transform root;

        public GameObjectPool()
        {
            pool= new Dictionary<string, Dictionary<string, PoolItem>>();
            poolObjMap = new Dictionary<GameObject, PoolItem>();

            var gameObject = new GameObject("GameObjectPool");
            Object.DontDestroyOnLoad(gameObject);

            root = gameObject.transform;
        }

        public GameObject Spawn(string assetBundleName, string assetName)
        {
            return this.Spawn(assetBundleName, assetName, Vector3.zero, Quaternion.identity);
        }

        public GameObject Spawn(string assetBundleName, string assetName, Vector3 pos, Quaternion rot)
        {
            if (!pool.ContainsKey(assetBundleName))
			{
                var prefab = AssetManager.LoadObject<GameObject>(assetBundleName, assetName);
				var item = new PoolItem(){ 
                    prefab = prefab, 
                    root=root 
                    };
				pool.Add(
					assetBundleName, new Dictionary<string, PoolItem>()
					{ 
						{
							assetName, item
						} 
					}
				);

				this.poolObjMap[prefab] = item;

                var ret = item.Spawn(pos, rot);
                poolObjMap[ret] = item;
                return ret;
			}

			if (!pool[assetBundleName].ContainsKey(assetName))
			{
				var prefab = AssetManager.LoadObject<GameObject>(assetBundleName, assetName);
				var item = new PoolItem(){ prefab = prefab, root=root};

				pool[assetBundleName].Add(assetName, item);
				

                var ret = item.Spawn(pos, rot);
                poolObjMap[ret] = item;
                return ret;
			}

            {
                var item = pool[assetBundleName][assetName];
                var ret = item.Spawn(pos, rot);
                poolObjMap[ret] = item;
                return ret;
            }
        }

        public void Despawn(GameObject inst)
        {
            if (!this.poolObjMap.ContainsKey(inst))
			{
				Debug.LogError("despawn failed");
				return;
			}
			poolObjMap[inst].Despawn(inst);
        }

        void Update()
        {
            foreach(var item in this.poolObjMap.Values)
            {
                item.Update();
            }
        }
    }
}