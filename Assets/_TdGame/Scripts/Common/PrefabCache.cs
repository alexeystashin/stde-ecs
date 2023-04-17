using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pump.Unity
{
    public interface IUnityPrefabProvider : IDisposable
    {
        GameObject GetPrefab(string path);
    }

    public class PrefabCache : IUnityPrefabProvider
    {
        const string errorPrefabPath = "Error";

        Dictionary<string, GameObject> prefabs = new();

        public Func<string, GameObject> ExternalPrefabSource;

        public GameObject GetPrefab(string path)
        {
            if(string.IsNullOrEmpty(path))
                return GetPrefab(errorPrefabPath);

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

                if (path != errorPrefabPath)
                    return GetPrefab(errorPrefabPath);
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

        public void Dispose()
        {
            prefabs.Clear();
            ExternalPrefabSource = null;
        }
    }
}
