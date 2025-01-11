using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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

        public static float PathLength(this NavMeshPath path)
        {
            var pathLength = 0f;
            if (path.corners.Length < 2) return pathLength;
            
            for (var i = 1; i < path.corners.Length; i++)
            {
                pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }

            return pathLength;
        }

    }
}