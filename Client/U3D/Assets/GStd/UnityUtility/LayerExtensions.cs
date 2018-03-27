using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GStd {

    public static class LayerExtensions {

        static int PowOf2(int value)
        {
            int ret = 0;
            do
            {
                value /= 2;
                ret++;
            } while (value != 0);
            return ret -1; // value==0的时候，会多加1
        }

        public static int[] LayerMaskValueToIntArray(int value)
        {
            List<int> ret = new List<int>();
            do
            {
                int powOf2 = PowOf2(value);
                int vTmp = (int)Mathf.Pow(2, powOf2);
                value -= vTmp;

                ret.Add(powOf2);
            } while (value > 0);

            return ret.ToArray();
        }

        public static int[] ToIntArray(this LayerMask mask)
        {
            return LayerMaskValueToIntArray(mask.value);
        }

        public static int ToInt(this LayerMask mask)
        {
            int[] ints = mask.ToIntArray();
            if (ints.Length > 0)
                return ints[0];
            else
                return -1;
        }
    }

}