namespace GStd
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    [CreateAssetMenu(
            fileName = "@AnimatorOptimize",
            menuName = "GStd/AnimatorOptimize")]
    public class AnimatorOptimize : ScriptableObject
    {
        [SerializeField]
        public string[] args;
    }
}
