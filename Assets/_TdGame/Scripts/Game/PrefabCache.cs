using System;
using System.Collections.Generic;
using UnityEngine;

namespace TdGame
{
    public class PrefabCache
    {
        #region singleton
        static PrefabCache _instance;
        public static PrefabCache instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PrefabCache();
                return _instance;
            }
        }
        PrefabCache() { }
        #endregion

        Dictionary<string, GameObject> prefabs = new();

        public Func<string, GameObject> ExternalPrefabSource;

        public GameObject GetPrefab(string path)
        {
            if(string.IsNullOrEmpty(path))
                return PrefabCache.instance.GetPrefab("Error");

            if (prefabs.ContainsKey(path))
                return prefabs[path];

            //Debug.LogWarning("Load prefab: " + path);

            // try load from external source
            var prefab = ExternalPrefabSource?.Invoke(path);
            // try load from Resources/
            prefab ??= Resources.Load<GameObject>(path);

            if (prefab == null)
            {
                Debug.LogWarning("Prefab not found: " + path);

                if (path != "Error")
                    return PrefabCache.instance.GetPrefab("Error");
                else
                    return null;
            }

            prefabs[path] = prefab;
            return prefab;
        }

        public void AddPrefabs(Dictionary<string, GameObject> newPrefabs)
        {
            foreach (var prefab in prefabs)
                prefabs.Add(prefab.Key, prefab.Value);
        }
    }
}
