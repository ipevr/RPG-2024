using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace RPG.Saving
{
    public static class JsonStatics
    {
        public static JToken ToToken(this Vector3 vector)
        {
            var state = new JObject();
            IDictionary<string, JToken> stateDict = state;
            stateDict.Add("x", vector.x);
            stateDict.Add("y", vector.y);
            stateDict.Add("z", vector.z);
            return state;
        }

        public static Vector3 ToVector3(this JToken state)
        {
            var vector = new Vector3();
            if (state is not JObject jObject) return vector;
            
            IDictionary<string, JToken> stateDict = jObject;

            if (stateDict.TryGetValue("x", out var x))
            {
                vector.x = x.ToObject<float>();
            }

            if (stateDict.TryGetValue("y", out var y))
            {
                vector.y = y.ToObject<float>();
            }
                
            if (stateDict.TryGetValue("z", out var z))
            {
                vector.z = z.ToObject<float>();
            }

            return vector;
        }
    }
}