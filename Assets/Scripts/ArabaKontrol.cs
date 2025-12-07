using UnityEngine;

public class ArabaKontrol : MonoBehaviour
{
    [Header("S�r�� Ayarlar�")]
    public float hizlanmaGucu = 15f;
    public float donusHizi = 100f;
    public float maksimumHiz = 20f;

    [Header("Sinyal Sistemi")]
    public GameObject solSinyalGrubu; // Hiyerar�ideki o ���k grubu
    public GameObject sagSinyalGrubu;

    // Sinyal durumlar� (D��ar�dan okumak gerekebilir)
    public bool solSinyalAcik { get; private set; }
    public bool sagSinyalAcik { get; private set; }

    private Rigidbody rb;
    private float gazGiris;
    private float direksiyonGiris;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Ba�lang��ta sinyalleri kapat
        SinyalKapat();
    }

    void Update()
    {
        // Girdileri Al (WASD veya Ok Tu�lar�)
        gazGiris = Input.GetAxis("Vertical");   // W/S veya Yukar�/A�a��
        direksiyonGiris = Input.GetAxis("Horizontal"); // A/D veya Sa�/Sol

        // Sinyal Kontrol� (Q ve E tu�lar�)
        if (Input.GetKeyDown(KeyCode.Q)) SolSinyalYak();
        if (Input.GetKeyDown(KeyCode.E)) SagSinyalYak();
    }

    void FixedUpdate()
    {
        // Hareket (Fizik motoruna g�� uygula)
        if (Mathf.Abs(gazGiris) > 0.1f)
        {
            // �leri/Geri itme g�c�
            rb.AddForce(transform.forward * gazGiris * hizlanmaGucu, ForceMode.Acceleration);
        }

        // D�n�� (Sadece hareket ederken d�nebilsin)
        if (rb.linearVelocity.magnitude > 1f)
        {
            // Geri giderken direksiyon ters d�nmeli mi? Evet.
            float yonCarpan = gazGiris >= 0 ? 1 : -1;
            float donus = direksiyonGiris * donusHizi * Time.fixedDeltaTime * yonCarpan;

            // Arabay� oldu�u yerde d�nd�r (Y ekseninde)
            Quaternion donusMiktari = Quaternion.Euler(0f, donus, 0f);
            rb.MoveRotation(rb.rotation * donusMiktari);
        }

        // H�z Limiti (�ok u�mas�n)
        if (rb.linearVelocity.magnitude > maksimumHiz)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maksimumHiz;
        }
    }

    // --- S�NYAL FONKS�YONLARI ---
    void SolSinyalYak()
    {
        solSinyalAcik = !solSinyalAcik; // Varsa kapat, yoksa a� (Toggle)
        sagSinyalAcik = false; // Sa�� kesin kapat
        GorselGuncelle();
    }

    void SagSinyalYak()
    {
        sagSinyalAcik = !sagSinyalAcik;
        solSinyalAcik = false;
        GorselGuncelle();
    }

    void SinyalKapat()
    {
        solSinyalAcik = false;
        sagSinyalAcik = false;
        GorselGuncelle();
    }

    void GorselGuncelle()
    {
        if (solSinyalGrubu) solSinyalGrubu.SetActive(solSinyalAcik);
        if (sagSinyalGrubu) sagSinyalGrubu.SetActive(sagSinyalAcik);
    }
}