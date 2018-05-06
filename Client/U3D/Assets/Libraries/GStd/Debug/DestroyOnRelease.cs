namespace GStd
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

    public class DestroyOnRelease : MonoBehaviour
    {
        void Awake()
        {
            if (!Debug.isDebugBuild)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

}
