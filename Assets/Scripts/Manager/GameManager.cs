using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static Action<int> OnPuanDegisti;
    public static Action<string> OnCezaYendi;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // OYUN DEGISKENLERI
    public int toplamPuan = 100;
    public bool oyunDevamEdiyor = true;

    [Header("Mode References")]
    public GameObject playerCar;
    public GameObject pedestrian;
    public GameObject xrOrigin;
    public XRCameraFollow cameraFollowScript;

    [Header("UI Panels")]
    public GameObject entryPanel;
    public GameObject cezaUyariText; // Ceza yendiginde acilacak text objesi

    public void StartCarMode()
    {
        if (entryPanel != null) entryPanel.SetActive(false);

        if (playerCar != null)
        {
            playerCar.SetActive(true);
            var carCtrl = playerCar.GetComponent<ArabaKontrol>();
            if (carCtrl != null) carCtrl.enabled = true;
        }

        if (pedestrian != null)
        {
            var pedCtrl = pedestrian.GetComponent<PedestrianController>();
            if (pedCtrl != null) pedCtrl.enabled = false;
        }

        if (cameraFollowScript != null && playerCar != null)
        {
            cameraFollowScript.target = playerCar.transform;
            cameraFollowScript.offset = new Vector3(0, 2.5f, -5f); // Car offset (TPS)
            cameraFollowScript.SnapToTarget();
        }
    }

    public void StartPedestrianMode()
    {
        if (entryPanel != null) entryPanel.SetActive(false);

        if (playerCar != null)
        {
            var carCtrl = playerCar.GetComponent<ArabaKontrol>();
            if (carCtrl != null) carCtrl.enabled = false;
        }

        /*
        if (pedestrian != null)
        {
            pedestrian.SetActive(true);
            var pedCtrl = pedestrian.GetComponent<PedestrianController>();
            if (pedCtrl != null) pedCtrl.enabled = true;
        }*/

        // FPS Mode Setup for Pedestrian
        /*
        if (cameraFollowScript != null && pedestrian != null)
        {
            cameraFollowScript.target = pedestrian.transform;
            // FPS Offset: Sifir (Kafa pozisyonu XR Origin tarafindan ayarlanir)
            // Eger XR Origin ayakta durma modundaysa, Camera Offset zaten yukseklik verir.
            cameraFollowScript.offset = Vector3.zero; 
            cameraFollowScript.SnapToTarget();
        }*/
        cameraFollowScript.enabled = false;
        xrOrigin.transform.position = new Vector3(0, 0, 0);
    }

    // Isim uyumlulugu icin hem CezaVer hem CezaYe tutuyoruz
    public void CezaYe(int miktar)
    {
        CezaVer(miktar, "Kaza/Ihlal");
    }

    public void CezaVer(int cezaMiktari, string sebep)
    {
        if (!oyunDevamEdiyor) return;

        toplamPuan -= cezaMiktari;
        if (toplamPuan < 0) toplamPuan = 0;

        Debug.Log($"Ceza: {sebep}. Yeni Puan: {toplamPuan}");

        OnCezaYendi?.Invoke(sebep);
        OnPuanDegisti?.Invoke(toplamPuan);

        // UI Uyarisini Goster
        if (cezaUyariText != null)
        {
            StopCoroutine(ShowCezaTextRoutine());
            StartCoroutine(ShowCezaTextRoutine());
        }

        if (toplamPuan <= 0)
        {
            OyunBitti();
        }
    }

    private IEnumerator ShowCezaTextRoutine()
    {
        cezaUyariText.SetActive(true);
        yield return new WaitForSeconds(5f);
        cezaUyariText.SetActive(false);
    }

    void OyunBitti()
    {
        oyunDevamEdiyor = false;
        Debug.Log("OYUN BITTI! EHLIYETI KAPTIRDIN.");
        
        // Sahneyi Yeniden Yukle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
