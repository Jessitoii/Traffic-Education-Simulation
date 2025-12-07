using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform anaRota; // Bütün noktalarýn olduðu dev Rota objesi
    public List<GameObject> arabaPrefableri; // 4-5 farklý araba buraya
    public int arabaSayisi = 50; // Baþlangýçta 50 dene, 600 pc'yi yakabilir.

    void Start()
    {
        TrafigiOlustur();
    }

    void TrafigiOlustur()
    {
        // 1. Önce Rota içindeki tüm noktalarý bulalým
        List<Transform> tumNoktalar = new List<Transform>();
        foreach (Transform nokta in anaRota)
        {
            tumNoktalar.Add(nokta);
        }

        // Hata kontrolü
        if (tumNoktalar.Count == 0)
        {
            Debug.LogError("HATA: Rota objesinin altýnda hiç nokta yok!");
            return;
        }

        // 2. Noktalarý Karýþtýr (Shuffle) - Böylece arabalar sýrayla deðil rastgele yerleþir
        // Fisher-Yates Shuffle algoritmasý
        for (int i = 0; i < tumNoktalar.Count; i++)
        {
            Transform temp = tumNoktalar[i];
            int randomIndex = Random.Range(i, tumNoktalar.Count);
            tumNoktalar[i] = tumNoktalar[randomIndex];
            tumNoktalar[randomIndex] = temp;
        }

        // 3. Arabalarý Yarat
        // Eðer araba sayýsý nokta sayýsýndan fazlaysa hata verir, o yüzden limitliyoruz.
        int spawnAdedi = Mathf.Min(arabaSayisi, tumNoktalar.Count);

        for (int i = 0; i < spawnAdedi; i++)
        {
            // A. Rastgele bir araba modeli seç
            GameObject secilenPrefab = arabaPrefableri[Random.Range(0, arabaPrefableri.Count)];

            // B. Arabayý Yarat (Pozisyon önemli deðil, script düzeltecek)
            GameObject yeniAraba = Instantiate(secilenPrefab);

            // C. Arabayý hiyerarþide düzenli tutalým (Spawner'ýn içine atalým)
            yeniAraba.transform.SetParent(this.transform);

            // D. Arabanýn beynine ulaþýp rotayý verelim
            AICar aiScript = yeniAraba.GetComponent<AICar>();
            if (aiScript != null)
            {
                // Hangi noktada doðduysa o noktanýn Orijinal Listesisindeki sýrasýný bulmamýz lazým.
                // Çünkü biz listeyi karýþtýrdýk (Shuffle).
                // Ancak AICar scripti sýrayla gitmek için orijinal sýralamaya muhtaç.

                Transform dogduguNokta = tumNoktalar[i];
                int orijinalIndex = dogduguNokta.GetSiblingIndex(); // Hiyerarþide kaçýncý sýrada?

                // Arabayý baþlat
                aiScript.SpawnerIleBaslat(anaRota, orijinalIndex);
            }
        }

        Debug.Log(spawnAdedi + " adet araba trafiðe katýldý.");
    }
}