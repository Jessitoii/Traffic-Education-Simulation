using UnityEngine;

public class IhlalBolgesi : MonoBehaviour
{
    public TrafikIsigi bagliTrafikIsigi; // Hangi isigi denetliyoruz?

    [Header("Ceza Ayarlari")]
    public int cezaPuani = 20;
    public string cezaMesaji = "Kirmizi Isik Ihlali";

    private bool cezaKesildi = false;

    // Arabanin etiketi (Tag) mutlaka "Player" olmali.
    private void OnTriggerEnter(Collider other)
    {
        // 1) OYUNCU ISE KONTROL ET
        if (other.CompareTag("Player"))
        {
            KontrolEt();
        }

        // 2) YAPAY ZEKA ISE DURDUR
        if (other.CompareTag("AI_Araba"))
        {
            // AICar referansi yoksa hata vermemesi icin kontrol edelim, 
            // ama proje yapisinda AICar scripti oldugunu varsayiyoruz.
            // Eger AICar scripti yoksa bu kisim calismaz.
            // AICar yapayZeka = other.GetComponent<AICar>();
            // if (yapayZeka != null) ...
            
            // Simdilik sadece Player odakli gidiyoruz, AI mantigi mevcut scriptte kalsin.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Oyuncu kutudan cikinca tekrar ceza kesilebilsin istiyorsan:
        if (other.CompareTag("Player"))
        {
            cezaKesildi = false;
        }
    }

    private void KontrolEt()
    {
        if (bagliTrafikIsigi == null) return;

        if (bagliTrafikIsigi.suankiDurum == TrafikIsigi.IsikDurumu.Kirmizi)
        {
            if (cezaKesildi) return; // spam engelle
            cezaKesildi = true;

            // 1) CEZA UYARISI
            Debug.Log("CEZA! " + cezaMesaji + " -" + cezaPuani);

            // 2) PUAN DUSUR
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CezaVer(cezaPuani, cezaMesaji);
            }
        }
        else
        {
            Debug.Log("Guvenli gecis.");
        }
    }
}
