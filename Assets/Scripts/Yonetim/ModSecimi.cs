using UnityEngine;

public class ModSecimi : MonoBehaviour
{
    [Header("Oyuncular")]
    public GameObject arabaOyuncusu; // Arabayý buraya sürükle
    public GameObject yayaOyuncusu;  // Yayayý buraya sürükle

    [Header("UI")]
    public GameObject girisPaneli;   // Siyah paneli buraya sürükle
    public GameObject xrRig;         // MR Interaction Setup'ý buraya sürükle

    void Start()
    {
        // Oyunu dondur (Zamaný durdur)
        Time.timeScale = 0;

        // Paneli aç, oyuncularý kapat
        girisPaneli.SetActive(true);
        arabaOyuncusu.SetActive(false);
        yayaOyuncusu.SetActive(false);
    }

    public void ArabaModunuSec()
    {
        Debug.Log("Araba Modu Seçildi");

        // 1. Arabayý aç
        arabaOyuncusu.SetActive(true);

        // 2. XR Rig'i arabaya oturt (Parent yap ve sýfýrla)
        xrRig.transform.SetParent(arabaOyuncusu.transform); // SurucuKoltugu ismine dikkat!
        xrRig.transform.localPosition = new Vector3(18, 8, -10);
        xrRig.transform.localRotation = Quaternion.Euler(22, 0, 0);

        OyunuBaslat();
    }

    public void YayaModunuSec()
    {
        Debug.Log("Yaya Modu Seçildi");

        // 1. Yayayý aç
        yayaOyuncusu.SetActive(true);

        // 2. XR Rig'i yayanýn kafasýna oturt
        // Yayanýn içinde 'CameraRoot' veya kafa hizasýnda bir obje olmalý
        xrRig.transform.SetParent(yayaOyuncusu.transform);
        xrRig.transform.localPosition = new Vector3(0, 1.6f, 0); // Yerden 1.6m yukarý (Kafa hizasý)
        xrRig.transform.localRotation = Quaternion.identity;

        OyunuBaslat();
    }

    void OyunuBaslat()
    {
        // Paneli kapat
        girisPaneli.SetActive(false);

        // Zamaný akýt
        Time.timeScale = 1;
    }
}