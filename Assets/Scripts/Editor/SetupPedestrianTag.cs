using UnityEngine;
using UnityEditor;

public class SetupPedestrianTag
{
    public static void Execute()
    {
        // "Pedestrian" tagini eklemeye calis (Editor scripti ile zor olabilir ama deneyelim)
        // Runtime'da tag eklenemez, onceden tanimli olmali.
        // Kullaniciya manuel eklemesi gerektigini soylemek daha guvenli.
        Debug.Log("Lutfen Unity Editor'de 'Pedestrian' adinda bir Tag olusturdugunuzdan emin olun!");
        
        // Sahnedeki mevcut yayalari guncelle (Eger varsa)
        GameObject[] peds = GameObject.FindGameObjectsWithTag("Untagged");
        foreach (GameObject go in peds)
        {
            if (go.GetComponent<PedestrianAI>() != null)
            {
                go.tag = "Pedestrian";
                
                if (go.GetComponent<Collider>() == null)
                {
                    CapsuleCollider cap = go.AddComponent<CapsuleCollider>();
                    cap.center = new Vector3(0, 0.9f, 0);
                    cap.height = 1.8f;
                    cap.radius = 0.3f;
                }
            }
        }
    }
}
