namespace GStd
{
    using System;
    using UnityEngine;

    [AddComponentMenu("GStd/Pool/Pool Strategy")]
    public sealed class PoolStrategy : MonoBehaviour
    {
        [Tooltip("The maximum count of the instance will pool."), SerializeField]
        private int instancePoolCount = 5;
        [Header("Instance pool strategy."), SerializeField, Tooltip("The time in second the instance will auto release after it has free.")]
        private float instanceReleaseAfterFree = 60f;
        [SerializeField, Tooltip("The time in second the prefab will auto release after it has free."), Header("Prefab cache strategy.")]
        private float prefabReleaseAfterFree = 120f;

        public int InstancePoolCount
        {
            get
            {
                return this.instancePoolCount;
            }
        }

        public float InstanceReleaseAfterFree
        {
            get
            {
                return this.instanceReleaseAfterFree;
            }
        }

        public float PrefabReleaseAfterFree
        {
            get
            {
                return this.prefabReleaseAfterFree;
            }
        }
    }
}

