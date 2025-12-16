using UnityEngine;

public class CollisionPenalty : MonoBehaviour
{
    [Header("Settings")]
    public int buildingPenalty = 10;
    public int carPenalty = 10;
    public int pedestrianPenalty = 20; // Yayaya carpma cezasi
    public string buildingMessage = "Binaya Carptiniz!";
    public string carMessage = "Arabaya Carptiniz!";
    public string pedestrianMessage = "Yayaya Carptiniz!";

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other.gameObject);
    }

    private void HandleCollision(GameObject hitObject)
    {
        // 1. BINA CARPMASI
        bool isBuilding = false;
        if (hitObject.transform.parent != null && hitObject.transform.parent.parent != null)
        {
             if (hitObject.transform.parent.name == "Buildings" || hitObject.transform.parent.parent.name == "Buildings")
             {
                 isBuilding = true;
             }
        }
        
        if (isBuilding)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CezaVer(buildingPenalty, buildingMessage);
            }
            return; // Ayni anda birden fazla ceza yememek icin
        }

        // 2. ARABA CARPMASI (Yaya Modundayken veya Araba Modundayken baska arabaya)
        if (hitObject.CompareTag("Car") || hitObject.CompareTag("AI_Araba"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CezaVer(carPenalty, carMessage);
            }
            return;
        }

        // 3. YAYA CARPMASI (Araba Modundayken)
        // Yayalarin tagi "Pedestrian" olmali veya ozel bir bileseni olmali
        if (hitObject.CompareTag("Pedestrian") || hitObject.GetComponent<PedestrianAI>() != null)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CezaVer(pedestrianPenalty, pedestrianMessage);
            }
        }
    }
}
