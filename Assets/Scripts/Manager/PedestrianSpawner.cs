using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class PedestrianSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] yayaPrefabs; // Inspector'dan atanacak prefablar
    public int spawnCount = 50;
    public float spawnRadius = 100f;

    [Header("Container")]
    public string containerName = "PedestrianContainer";

    private Transform container;

    void Start()
    {
        SpawnPedestrians();
    }

    void SpawnPedestrians()
    {
        if (yayaPrefabs == null || yayaPrefabs.Length == 0)
        {
            Debug.LogError("PedestrianSpawner: Yaya prefablari atanmamis!");
            return;
        }

        // Container objesini olustur veya bul
        GameObject containerObj = GameObject.Find(containerName);
        if (containerObj == null) containerObj = new GameObject(containerName);
        container = containerObj.transform;

        int spawnedCount = 0;
        int attempts = 0;
        int maxAttempts = spawnCount * 5; // Sonsuz donguyu onlemek icin

        while (spawnedCount < spawnCount && attempts < maxAttempts)
        {
            attempts++;

            // 1. Rastgele bir nokta sec (Spawner etrafinda)
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;
            
            // 2. NavMesh uzerinde gecerli bir nokta mi? (Sadece Walkable alanlar)
            NavMeshHit hit;
            // NavMesh.AllAreas kullaniyoruz, cunku yollar "Not Walkable" ise zaten NavMesh'e dahil degildir.
            // Eger yollar farkli bir Area ID ise ve oraya spawn olmasini istemiyorsaniz Mask kullanmalisiniz.
            // Varsayilan: Walkable (Area 0) -> 1 << 0
            if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
            {
                // 3. Rastgele prefab sec
                GameObject prefabToSpawn = yayaPrefabs[Random.Range(0, yayaPrefabs.Length)];

                // 4. Instantiate
                GameObject newPedestrian = Instantiate(prefabToSpawn, hit.position, Quaternion.identity);
                
                // 5. Parent ayarla
                newPedestrian.transform.SetParent(container);

                // 6. PedestrianAI scriptini kontrol et ve ekle
                PedestrianAI aiScript = newPedestrian.GetComponent<PedestrianAI>();
                if (aiScript == null)
                {
                    aiScript = newPedestrian.AddComponent<PedestrianAI>();
                }

                // 7. NavMeshAgent kontrolu (Prefab'da yoksa ekle)
                NavMeshAgent agent = newPedestrian.GetComponent<NavMeshAgent>();
                if (agent == null)
                {
                    agent = newPedestrian.AddComponent<NavMeshAgent>();
                    // Agent ayarlari (Boyutlar CityPeople karakterlerine gore)
                    agent.height = 1.8f;
                    agent.radius = 0.3f;
                }

                // 8. Tag ve Collider Ayarlari (Carpisma Cezasi Icin)
                newPedestrian.tag = "Pedestrian";
                
                // Collider kontrolu (CapsuleCollider tercih edilir)
                Collider col = newPedestrian.GetComponent<Collider>();
                if (col == null)
                {
                    CapsuleCollider cap = newPedestrian.AddComponent<CapsuleCollider>();
                    cap.center = new Vector3(0, 0.9f, 0);
                    cap.height = 1.8f;
                    cap.radius = 0.3f;
                }

                spawnedCount++;
            }
        }

        Debug.Log($"PedestrianSpawner: {spawnedCount} adet yaya olusturuldu.");
    }
}
