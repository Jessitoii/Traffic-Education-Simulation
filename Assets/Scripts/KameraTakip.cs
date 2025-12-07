using UnityEngine;

public class KameraTakip : MonoBehaviour
{
    public Transform hedef; // Takip edilecek araba

    [Header("Ayarlar")]
    public Vector3 mesafe = new Vector3(0, 5, -10); // Arkadan ve yukarýdan bakýþ
    public float takipYumusakligi = 5f;
    public float donusYumusakligi = 10f;

    void FixedUpdate()
    {
        if (hedef == null) return;

        // 1. Pozisyon Takibi
        // Arabanýn arkasýndaki hedef pozisyonu belirle
        Vector3 hedefPozisyon = hedef.TransformPoint(mesafe);

        // Oraya yumuþakça kay (Lerp)
        transform.position = Vector3.Lerp(transform.position, hedefPozisyon, takipYumusakligi * Time.deltaTime);

        // 2. Rotasyon Takibi (Arabaya bak)
        // Arabanýn baktýðý yere bak ama çok sarsýlma
        Quaternion hedefRotasyon = Quaternion.LookRotation(hedef.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, hedefRotasyon, donusYumusakligi * Time.deltaTime);
    }
}