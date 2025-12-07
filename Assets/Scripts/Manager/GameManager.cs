using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // SINGLETON YAPISI (Bunu ezberle, her projede kullanýrsýn)
    // Bu sayede diðer scriptlerden 'GameManager.Instance' diyerek buraya ulaþacaðýz.
    public static GameManager Instance { get; private set; }
    // --- EVENTS (OLAYLAR) ---
    // Puan deðiþtiðinde bu olaya abone olan herkes haberdar edilecek.
    // Yanýndaki <int> puanýn yeni deðerini taþýyacak.
    public static Action<int> OnPuanDegisti;

    // Ceza yendiðinde sebebi ekrana yazdýrmak için bir olay.
    public static Action<string> OnCezaYendi;
    private void Awake()
    {
        // Eðer sahnede baþka bir GameManager varsa kendini yok et (Tek patron olmalý)
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // ---------------------------------------------------------

    // OYUN DEÐÝÞKENLERÝ
    public int toplamPuan = 100; // Oyuna 100 puanla baþla
    public bool oyunDevamEdiyor = true;

    public void CezaVer(int cezaMiktari, string sebep)
    {
        if (!oyunDevamEdiyor) return;

        toplamPuan -= cezaMiktari;
        if (toplamPuan < 0) toplamPuan = 0;

        // ESKÝ KOD: Debug.Log(...); (Artýk buna gerek kalmayacak ama dursun)
        Debug.Log($"Ceza: {sebep}. Yeni Puan: {toplamPuan}");

        // --- YENÝ KOD: OLAYLARI TETÝKLE ---

        // 1. Arayüze ceza sebebini gönder
        OnCezaYendi?.Invoke(sebep);

        // 2. Arayüze yeni puaný gönder ki güncellesin
        OnPuanDegisti?.Invoke(toplamPuan);

        // ----------------------------------

        if (toplamPuan <= 0)
        {
            OyunBitti();
        }
    }

    void OyunBitti()
    {
        oyunDevamEdiyor = false;
        Debug.Log("OYUN BÝTTÝ! EHLÝYETÝ KAPTIRDIN.");
        // Ýlerde buraya 'Yeniden Baþlat' menüsünü açan kodu ekleyeceðiz.
    }
}