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
        
        public static AudioClip GetRandomClip(this AudioClip[] clips)
        {
            var clipIndex = Random.Range(0, clips.Length);
            return clips[clipIndex];
        }

    }
}