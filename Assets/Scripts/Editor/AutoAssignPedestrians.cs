using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class AutoAssignPedestrians
{
    public static void Execute()
    {
        GameObject spawnerObj = GameObject.Find("TrafficSpawner");
        if (spawnerObj == null)
        {
            Debug.LogError("TrafficSpawner objesi bulunamadi!");
            return;
        }

        PedestrianSpawner spawner = spawnerObj.GetComponent<PedestrianSpawner>();
        if (spawner == null)
        {
            Debug.LogError("PedestrianSpawner scripti bulunamadi!");
            return;
        }

        // CityPeople prefablarini bul
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/DenysAlmaral/CityPeople/Prefabs" });
        List<GameObject> foundPrefabs = new List<GameObject>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
            {
                // Sadece karakter prefablarini al (Isminde 'casual' veya 'business' gecenler genelde karakterdir)
                // Veya Animator component'i olanlari alalim
                if (prefab.GetComponent<Animator>() != null || prefab.GetComponentInChildren<Animator>() != null)
                {
                    foundPrefabs.Add(prefab);
                }
            }
        }

        if (foundPrefabs.Count > 0)
        {
            spawner.yayaPrefabs = foundPrefabs.ToArray();
            EditorUtility.SetDirty(spawner);
            Debug.Log($"PedestrianSpawner: {foundPrefabs.Count} adet yaya prefabi otomatik atandi.");
        }
        else
        {
            Debug.LogWarning("Hicbir yaya prefabi bulunamadi! Lutfen 'Assets/DenysAlmaral/CityPeople/Prefabs' klasorunu kontrol edin.");
        }
    }
}
