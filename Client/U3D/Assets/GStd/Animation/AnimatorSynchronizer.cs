using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GStd {
    [AddComponentMenu("GStd/Animator/Animator Synchronizer"), RequireComponent(typeof(Animator))]
    public sealed class AnimatorSynchronizer : MonoBehaviour
    {
        [SerializeField, Tooltip("The source animator")]
        private Animator source;

        private Animator animator;
        Dictionary<int, AnimatorControllerParameterType> animatorParams;

        private void Awake()
        {
            this.animator = base.GetComponent<Animator>();

            animatorParams = new Dictionary<int, AnimatorControllerParameterType>();
            foreach (var param in this.animator.parameters)
                animatorParams.Add(param.nameHash, param.type);
        }

        private void Update()
        {
            if (this.source == null || this.animator == null)
                return;

            foreach(var param in this.animator.parameters)
            {
                switch (param.type)
                {
                    case AnimatorControllerParameterType.Bool:
                        this.animator.SetBool(param.nameHash, this.source.GetBool(param.nameHash));
                        break;

                    case AnimatorControllerParameterType.Float:
                        this.animator.SetFloat(param.nameHash, this.source.GetFloat(param.nameHash));
                        break;

                    case AnimatorControllerParameterType.Int:
                        this.animator.SetInteger(param.nameHash, this.source.GetInteger(param.nameHash));
                        break;
                }
            }
        }
    }
}