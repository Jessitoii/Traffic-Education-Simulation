using UnityEngine;
using System.Collections;

public class TrafikIsigi : MonoBehaviour
{
    public enum IsikDurumu { Kirmizi, Sari, Yesil }
    public IsikDurumu suankiDurum;

    // ARTIK TEK BİR RENDERER DEĞİL, 3 AYRI LAMBA İSTİYORUZ
    [Header("Modelin Parçaları")]
    public Renderer kirmiziLambaObjesi;
    public Renderer sariLambaObjesi;
    public Renderer yesilLambaObjesi;

    // Işık Parlaklığı için renkler (Emission simülasyonu)
    private Color parlakKirmizi = new Color(1f, 0f, 0f, 1f); // Tam Kırmızı
    private Color parlakSari = new Color(1f, 0.92f, 0.016f, 1f); // Tam Sarı
    private Color parlakYesil = new Color(0f, 1f, 0f, 1f); // Tam Yeşil
    private Color sonukRenk = Color.black; // Sönük hali (Siyah)

    // Süreler
    public float kirmiziSure = 5f;
    public float sariSure = 2f;
    public float yesilSure = 5f;

    void Start()
    {
        // Başlangıçta döngüyü başlat
        StartCoroutine(TrafikIsigiDongusu());
    }

    // --- DIŞARIDAN KONTROL İÇİN YENİ METOT (Kavşak Yöneticisi İçin Hazırlık) ---
    public void DisaridanDurumAta(IsikDurumu yeniDurum)
    {
        StopAllCoroutines(); // Kendi döngüsünü durdur
        IsiklariGuncelle(yeniDurum); // Yeni durumu uygula
    }
    // --------------------------------------------------------------------------

    IEnumerator TrafikIsigiDongusu()
    {
        while (true)
        {
            IsiklariGuncelle(IsikDurumu.Kirmizi);
            yield return new WaitForSeconds(kirmiziSure);

            IsiklariGuncelle(IsikDurumu.Sari);
            yield return new WaitForSeconds(sariSure);

            IsiklariGuncelle(IsikDurumu.Yesil);
            yield return new WaitForSeconds(yesilSure);

            IsiklariGuncelle(IsikDurumu.Sari);
            yield return new WaitForSeconds(sariSure);
        }
    }

    void IsiklariGuncelle(IsikDurumu durum)
    {
        suankiDurum = durum;

        // Önce hepsini söndür (Siyah yap)
        if (kirmiziLambaObjesi) kirmiziLambaObjesi.material.color = sonukRenk;
        if (sariLambaObjesi) sariLambaObjesi.material.color = sonukRenk;
        if (yesilLambaObjesi) yesilLambaObjesi.material.color = sonukRenk;

        // Sadece durumu aktif olanı boya
        switch (durum)
        {
            case IsikDurumu.Kirmizi:
                if (kirmiziLambaObjesi) kirmiziLambaObjesi.material.color = parlakKirmizi;
                break;
            case IsikDurumu.Sari:
                if (sariLambaObjesi) sariLambaObjesi.material.color = parlakSari;
                break;
            case IsikDurumu.Yesil:
                if (yesilLambaObjesi) yesilLambaObjesi.material.color = parlakYesil;
                break;
        }
    }
}