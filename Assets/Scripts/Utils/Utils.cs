using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static Transform FindByTag(this Transform transform, string tag)
        {
            return transform.Cast<Transform>().FirstOrDefault(t => t.CompareTag(tag));
        }
    }
}