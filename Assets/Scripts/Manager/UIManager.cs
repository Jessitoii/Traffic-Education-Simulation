using UnityEngine;
using TMPro; // TextMeshPro kullanmak için þart
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Elemanlarý")]
    public TextMeshProUGUI puanText;
    public TextMeshProUGUI cezaUyariText;

    // Script aktif olduðunda dinlemeye baþla
    private void OnEnable()
    {
        // GameManager'ýn olaylarýna abone oluyoruz
        GameManager.OnPuanDegisti += PuanGuncelle;
        GameManager.OnCezaYendi += CezayiGoster;
    }

    // Script pasif olduðunda (veya obje yok olduðunda) dinlemeyi býrak
    // BUNU YAPMAZSAN HAFIZA SIZINTISI (MEMORY LEAK) OLUR. ÇOK ÖNEMLÝ.
    private void OnDisable()
    {
        GameManager.OnPuanDegisti -= PuanGuncelle;
        GameManager.OnCezaYendi -= CezayiGoster;
    }

    void Start()
    {
        // Oyun baþlar baþlamaz mevcut puaný ekrana yaz
        PuanGuncelle(GameManager.Instance.toplamPuan);
        cezaUyariText.text = ""; // Baþlangýçta uyarý olmasýn
    }

    // GameManager "OnPuanDegisti" diye baðýrýnca bu çalýþacak
    void PuanGuncelle(int yeniPuan)
    {
        puanText.text = "Puan: " + yeniPuan.ToString();
    }

    // GameManager "OnCezaYendi" diye baðýrýnca bu çalýþacak
    void CezayiGoster(string sebep)
    {
        // Önce durdur ki üst üste binmesin
        StopAllCoroutines();
        // Uyarýyý gösterip 2 saniye sonra gizleyen coroutine'i baþlat
        StartCoroutine(UyariGosterGizle("CEZA! " + sebep));
    }

    IEnumerator UyariGosterGizle(string mesaj)
    {
        cezaUyariText.text = mesaj;
        cezaUyariText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f); // 2 saniye bekle

        cezaUyariText.text = "";
        cezaUyariText.gameObject.SetActive(false);
    }
}