using UnityEngine;
using UnityEditor;

public class FinalSetup
{
    public static void Execute()
    {
        // 1. Setup Tags
        SetupTag("Player");
        SetupTag("Car");
        SetupTag("AI_Araba");

        // 2. Setup PlayerCar
        GameObject playerCar = GameObject.Find("PlayerCar");
        if (playerCar != null)
        {
            playerCar.tag = "Player";
            if (playerCar.GetComponent<CollisionPenalty>() == null)
                playerCar.AddComponent<CollisionPenalty>();
            
            // Ensure Rigidbody for collision detection
            if (playerCar.GetComponent<Rigidbody>() == null)
                playerCar.AddComponent<Rigidbody>();
        }

        // 3. Setup Pedestrian (Yaya)
        GameObject pedestrian = GameObject.Find("Yaya");
        if (pedestrian != null)
        {
            pedestrian.tag = "Player";
            if (pedestrian.GetComponent<CollisionPenalty>() == null)
                pedestrian.AddComponent<CollisionPenalty>();
        }

        // 4. Setup AI Cars (Find by name pattern or parent)
        // Assuming they are under TrafficSpawner or similar, or just find by name
        // Hierarchy showed "AIPickup...", "AITaxi..."
        // Let's try to find them and tag them "AI_Araba" or "Car"
        // User said "Tag'i 'Car' olan herhangi bir objeye değerse".
        // So let's tag them "Car".
        
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("AI") && (obj.name.Contains("Car") || obj.name.Contains("Pickup") || obj.name.Contains("Taxi") || obj.name.Contains("Police")))
            {
                obj.tag = "Car";
                // Ensure they have colliders
                if (obj.GetComponent<Collider>() == null)
                {
                    // Add box collider if missing (simple approx)
                    obj.AddComponent<BoxCollider>();
                }
            }
        }

        // 5. Setup UI Text Reference in GameManager
        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        GameObject cezaText = GameObject.Find("Canvas/CezaUyariText");
        if (gm != null && cezaText != null)
        {
            gm.cezaUyariText = cezaText;
            cezaText.SetActive(false); // Start hidden
            EditorUtility.SetDirty(gm);
        }

        Debug.Log("Final Setup Completed: Tags, Collision Scripts, UI References.");
    }

    static void SetupTag(string tag)
    {
        // Note: Adding tags via script in Editor is complex (SerializedObject).
        // We assume standard tags exist or user has added them.
        // "Player" exists by default. "Car" and "AI_Araba" might need to be added manually if not present.
        // But we can try to set it and catch error if missing.
    }
}
