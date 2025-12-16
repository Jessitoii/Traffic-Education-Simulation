using UnityEngine;
using UnityEditor;

public class BuildingSetup
{
    public static void Execute()
    {
        GameObject buildingsRoot = GameObject.Find("Near Geometry/Map/Buildings");
        if (buildingsRoot == null)
        {
            Debug.LogError("Buildings root not found at 'Near Geometry/Map/Buildings'");
            return;
        }

        int count = 0;
        foreach (Transform child in buildingsRoot.transform)
        {
            // Add MeshCollider if missing
            MeshCollider mc = child.GetComponent<MeshCollider>();
            if (mc == null)
            {
                mc = child.gameObject.AddComponent<MeshCollider>();
            }

            // Set properties
            mc.convex = true;
            mc.isTrigger = true;
            count++;
        }

        Debug.Log($"Processed {count} buildings. Added/Updated MeshColliders (Convex=true, IsTrigger=true).");
    }
}
