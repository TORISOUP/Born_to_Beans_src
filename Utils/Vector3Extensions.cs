using UnityEngine;

namespace GGJ.Utils
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// ベクトルを各要素1対1に掛け合わせる
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static Vector3 ProductOneToOne(this Vector3 origin, Vector3 factor)
        {
            return new Vector3(origin.x * factor.x, origin.y * factor.y, origin.z * factor.z);
        }

        public static Vector3 ProductOneToOne(this Vector3 origin, float x, float y, float z)
        {
            return origin.ProductOneToOne(new Vector3(x, y, z));
        }

        /// <summary>
        /// ターゲットとの距離を返す
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float GetDistance(this Vector3 origin, Vector3 target)
        {
            return Mathf.Abs((origin - target).magnitude);
        }

        /// <summary>
        /// ターゲットとの高さを無視した距離を返す
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float GetDistance2D(this Vector3 origin, Vector3 target)
        {
            return Mathf.Abs((origin.SetY(0) - target.SetY(0)).magnitude);
        }

        /// <summary>
        /// 対象を指す方向ベクトルを計算する
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static Vector3 GetTowardNormalizedVector(this Vector3 origin, Vector3 factor)
        {
            return (factor - origin).normalized;
        }

        /// <summary>
        /// Xを上書きしたベクトルを返す
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        public static Vector3 SetX(this Vector3 origin, float X)
        {
            return new Vector3(X, origin.y, origin.z);
        }

        public static Vector3 SetY(this Vector3 origin, float Y)
        {
            return new Vector3(origin.x, Y, origin.z);
        }

        public static Vector3 SetZ(this Vector3 origin, float Z)
        {
            return new Vector3(origin.x, origin.y, Z);
        }

        /// <summary>
        /// Yを０にして、それを正規化したベクトルを返す
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Vector3 SuppressY(this Vector3 origin)
        {
            var v = origin.SetY(0);
            return v.normalized;
        }

        /// <summary>
        /// 指定座標の周辺ランダムで返す
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Vector3 GetAround2D(this Vector3 origin, float radius)
        {
            var rad = UnityEngine.Random.Range(-180, 180);
            var vec = Quaternion.AngleAxis(rad, Vector3.up) * Vector3.forward;
            return origin + vec * radius;
        }
    }
}
