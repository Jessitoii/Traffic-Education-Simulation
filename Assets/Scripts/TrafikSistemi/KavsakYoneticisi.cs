using UnityEngine;
using System.Collections;

public class KavsakYoneticisi : MonoBehaviour
{
    // Kavþaktaki ýþýk gruplarý
    // Grup 1: Dikey yol (Kuzey-Güney)
    // Grup 2: Yatay yol (Doðu-Batý)
    public TrafikIsigi[] dikeyIsiklar;
    public TrafikIsigi[] yatayIsiklar;

    void Start()
    {
        StartCoroutine(KavsakDongusu());
    }

    IEnumerator KavsakDongusu()
    {
        while (true)
        {
            // 1. Dikey Geçsin, Yatay Beklesin
            IsiklariAyarla(dikeyIsiklar, TrafikIsigi.IsikDurumu.Yesil);
            IsiklariAyarla(yatayIsiklar, TrafikIsigi.IsikDurumu.Kirmizi);
            yield return new WaitForSeconds(10f); // 10 saniye akýþ

            // 2. Dikey Sarý, Yatay Hala Kýrmýzý (Hazýrlýk)
            IsiklariAyarla(dikeyIsiklar, TrafikIsigi.IsikDurumu.Sari);
            yield return new WaitForSeconds(2f);

            // 3. Herkes Kýrmýzý (Güvenlik Payý - Çok önemli!)
            IsiklariAyarla(dikeyIsiklar, TrafikIsigi.IsikDurumu.Kirmizi);
            IsiklariAyarla(yatayIsiklar, TrafikIsigi.IsikDurumu.Kirmizi);
            yield return new WaitForSeconds(1f);

            // 4. Yatay Geçsin, Dikey Beklesin
            IsiklariAyarla(dikeyIsiklar, TrafikIsigi.IsikDurumu.Kirmizi);
            IsiklariAyarla(yatayIsiklar, TrafikIsigi.IsikDurumu.Yesil);
            yield return new WaitForSeconds(10f);

            // 5. Yatay Sarý
            IsiklariAyarla(yatayIsiklar, TrafikIsigi.IsikDurumu.Sari);
            yield return new WaitForSeconds(2f);

            // 6. Herkes Kýrmýzý (Güvenlik Payý)
            IsiklariAyarla(yatayIsiklar, TrafikIsigi.IsikDurumu.Kirmizi);
            yield return new WaitForSeconds(1f);
        }
    }

    void IsiklariAyarla(TrafikIsigi[] isiklar, TrafikIsigi.IsikDurumu durum)
    {
        foreach (var isik in isiklar)
        {
            // TrafikIsigi scriptindeki 'public IsikDurumu suankiDurum' deðiþkenini
            // dýþarýdan deðiþtirebilmek için TrafikIsigi scriptini biraz güncellemen gerekecek.
            // O scriptteki sonsuz while döngüsünü (IEnumerator) silmelisin!
            // Artýk patron bu Kavþak Yöneticisi çünkü.
            isik.DisaridanDurumAta(durum);
        }
    }
}