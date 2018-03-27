using UnityEngine;

namespace GStd
{
    public class Tween : MonoBehaviour
    {
        public enum TWEEN_TYPE
        {
            PINGPONG
        }

        public enum TRANSLATE_TYPE
        {
            MOVE,
            ROTATE,
        }

        [SerializeField]
        private bool isActiveWhenPlay = true;

        [SerializeField]
        private TWEEN_TYPE tweenType;

        private void Awake()
        {
            if (!isActiveWhenPlay)
            {
                this.enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            switch (tweenType)
            {
                case TWEEN_TYPE.PINGPONG: InitPingpong(); break;
            }
        }

        private void Update()
        {
            switch (tweenType)
            {
                case TWEEN_TYPE.PINGPONG: TweenPingpong(); break;
            }
        }

        public void SetEnable(bool isActive)
        {
            this.enabled = isActive;
        }

        public void SetPingpong(Vector3 direction, float strength = 0.5f, float virbrato = 0.3f, float gap = 0.3f, int gapCount = 1)
        {
            this.PingpongDirection = direction;
            this.PingpongStrength = strength;
            this.PingpongVibrato = virbrato;
            this.PingpongGap = gap;
            this.PingpongGapCount = gapCount;
        }

        [SerializeField]
        private TRANSLATE_TYPE translateType;
        [SerializeField]
        private float PingpongStrength = 0.5f;
        [SerializeField]
        private float PingpongVibrato = 0.3f;
        [SerializeField]
        private Vector3 PingpongDirection;
        [SerializeField]
        private float PingpongGap = 0.3f;
        [SerializeField]
        private int PingpongGapCount = 1;

        private Vector3 PingpongOriginPosition;
        private Quaternion PingpongRotationOrigin;
        private bool PingpongIsNegative = false;
        private float PingpongTime = 1;
        private bool PingpongIsRest = false;
        private float PingpongRemainRestTime = 0;
        private int PingpongRemainGapCount;
        void InitPingpong()
        {
            this.PingpongOriginPosition = this.transform.position;
            this.PingpongRotationOrigin = this.transform.rotation;
            this.PingpongIsNegative = true;
            this.PingpongRemainGapCount = this.PingpongGapCount;

            this.PingpongTime = this.PingpongVibrato;
        }
        void TweenPingpong()
        {
            if (this.PingpongIsRest)
            {
                this.PingpongRemainRestTime -= Time.deltaTime;
                if (this.PingpongRemainRestTime <= 0)
                {
                    this.PingpongIsRest = false;
                }
            }
            else
            {
                var percent = Mathf.Sin(this.PingpongTime / this.PingpongVibrato * Mathf.PI) * (this.PingpongIsNegative ? -1f : 1f);
                switch (this.translateType)
                {
                    case TRANSLATE_TYPE.MOVE:
                        this.transform.position = this.PingpongOriginPosition + PingpongDirection * PingpongStrength * percent;
                        break;
                    case TRANSLATE_TYPE.ROTATE:
                        this.transform.rotation = this.PingpongRotationOrigin * Quaternion.AngleAxis(this.PingpongStrength * percent, this.PingpongDirection);
                        break;
                }

                this.PingpongTime -= Time.deltaTime;
                if (this.PingpongTime < 0)
                {
                    this.PingpongTime = this.PingpongVibrato;
                    this.PingpongIsNegative = !this.PingpongIsNegative;

                    if (this.PingpongIsNegative)
                    {
                        this.PingpongRemainGapCount--;
                        if (this.PingpongRemainGapCount <= 0)
                        {
                            PingpongIsRest = true;
                            this.PingpongRemainRestTime = this.PingpongGap;
                            this.PingpongRemainGapCount = this.PingpongGapCount;
                        }
                    }
                }
            }
        }
    }

}


