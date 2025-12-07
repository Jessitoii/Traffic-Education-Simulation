using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class AICar : MonoBehaviour
{
    [Header("Rota Ayarlarý")]
    public Transform rotaKonteyneri; // BURAYA 'Rota1' OBJESÝNÝ SÜRÜKLEYECEKSÝN
    public List<Transform> rotaNoktalari = new List<Transform>(); // Bu liste otomatik dolacak

    [Header("Diðer Ayarlar")]
    public float beklemeSuresi = 0f;

    private NavMeshAgent ajan;
    private int aktifNoktaIndex = 0;
    private bool duruyor = false;

    private bool hariciBaslatildi = false; // Spawner tarafýndan mý baþlatýldý?

    // Start fonksiyonunu ÞÖYLE DEÐÝÞTÝR:
    void Start()
    {
        ajan = GetComponent<NavMeshAgent>();

        // Eðer Spawner tarafýndan baþlatýlmadýysa (Manuel koyduysan)
        // Eski mantýkla çalýþmaya devam etsin
        if (!hariciBaslatildi && rotaKonteyneri != null)
        {
            RotaNoktalariniBul(); // Bunu aþaðýda ayrý fonksiyona aldýk
            HedefeGit();
        }
    }

    // BU YENÝ FONKSÝYONU SCRIPTÝN ÝÇÝNE EKLE:
    // Rota noktalarýný listeye dolduran yardýmcý fonksiyon
    void RotaNoktalariniBul()
    {
        rotaNoktalari.Clear();
        foreach (Transform cocuk in rotaKonteyneri)
        {
            rotaNoktalari.Add(cocuk);
        }
    }

    // --- SPAWNER'IN ÇAÐIRACAÐI KRÝTÝK FONKSÝYON ---
    public void SpawnerIleBaslat(Transform rotaK, int baslangicIndex)
    {
        hariciBaslatildi = true;
        ajan = GetComponent<NavMeshAgent>(); // Garanti olsun diye tekrar alalým

        rotaKonteyneri = rotaK;
        RotaNoktalariniBul(); // Noktalarý listeye çek

        // Baþlangýç indeksini ayarla
        aktifNoktaIndex = baslangicIndex;

        // Arabayý direkt o noktanýn pozisyonuna ve rotasyonuna ýþýnla (Doðru yöne baksýn)
        // Not: NavMeshAgent bazen ýþýnlanmayý sevmez, o yüzden Warp kullanýyoruz.
        ajan.Warp(rotaNoktalari[aktifNoktaIndex].position);
        transform.rotation = rotaNoktalari[aktifNoktaIndex].rotation;

        // Bir sonraki hedefe kilitlen (Spawn olduðu yerde durmasýn, bir sonrakine gitsin)
        SiradakiNoktayaGec();
    }

    void Update()
    {
        if (duruyor)
        {
            ajan.isStopped = true;
            return;
        }
        else
        {
            ajan.isStopped = false;
        }

        if (!ajan.pathPending && ajan.remainingDistance < 0.5f)
        {
            SiradakiNoktayaGec();
        }

        OnuKontrolEt();
    }

    void HedefeGit()
    {
        if (rotaNoktalari.Count == 0) return;
        ajan.SetDestination(rotaNoktalari[aktifNoktaIndex].position);
    }

    void SiradakiNoktayaGec()
    {
        aktifNoktaIndex = (aktifNoktaIndex + 1) % rotaNoktalari.Count;
        HedefeGit();
    }

    public void TrafikIsigiDurumu(bool durmali)
    {
        duruyor = durmali;
    }

    void OnuKontrolEt()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            if (hit.collider.CompareTag("AI_Araba"))
            {
                // Çarpýþma önleyici basit mantýk buraya eklenebilir
            }
        }
    }
}