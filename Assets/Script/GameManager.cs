using UnityEngine;
using System.Collections.Generic;
using TMPro;
using NUnit.Framework;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private MaskData maskData;

    [Header("Prefabs")]
    [SerializeField] GameObject letterSlotPrefab;
    [SerializeField] GameObject letterObjekPrefab;

    [Header("UI Elements")]
    [SerializeField] private Transform slotContainer;
    [SerializeField] private Transform[] tableArea;
    [SerializeField] private Transform maskVisualContainer;
    [SerializeField] private GameObject[] spriteJadi;
    [SerializeField] private GameObject[] spriteBelumJadi;

    public static Action OnItemComplete;

    private int currentMaskIndex = 0;
    private List<LetterSlot> activeSlots = new List<LetterSlot>();

    private void OnEnable()
    {
        LetterSlot.OnLetterPlaced += CheckIngredientComplete;
    }

    private void OnDisable()
    {
        LetterSlot.OnLetterPlaced -= CheckIngredientComplete;
    }

    private void Start()
    {
        foreach (Transform bahan in maskVisualContainer)
        {
            bahan.gameObject.SetActive(false);
        }

        for (int i = 0; i < maskVisualContainer.childCount / 2; i++)
        {
            Debug.Log("Reset visual bahan " + maskVisualContainer.GetChild(i).name);
            maskVisualContainer.GetChild(i).gameObject.SetActive(true);
        }

        if (maskData != null)
        {
            StartNewIngredient();
        }

    }

    private void StartNewIngredient()
    {
        // bersihkan slot dan area meja
        foreach (Transform child in slotContainer) Destroy(child.gameObject);
        
        //foreach (Transform child in tableArea) Destroy(child.gameObject);
        activeSlots.Clear();

        // set mask visual
        string wordToSpell = maskData.bahanList[currentMaskIndex].ingredientName;
        char[] characters = wordToSpell.ToCharArray();

        // spawn slot
        for (int i = 0; i < characters.Length; i++)
        {
            GameObject newSlot = Instantiate(letterSlotPrefab, slotContainer);
            LetterSlot slot = newSlot.GetComponent<LetterSlot>();
            slot.hurufTarget = characters[i];
            activeSlots.Add(slot);
        }

        // spawn letters di area meja
        foreach (char c in characters)
        {
            // 1. SETIAP huruf akan memilih antara index 0 (kiri) atau 1 (kanan)
            int randomIndex = UnityEngine.Random.Range(0, tableArea.Length);
            Transform tablePick = tableArea[randomIndex];

            // 2. Instantiate ke area yang terpilih
            GameObject newLetter = Instantiate(letterObjekPrefab, tablePick);

            // 3. Setup data huruf
            newLetter.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString();
            newLetter.GetComponent<DragnDrop>().hurufSaya = c;

            // 4. Atur posisi acak di DALAM area yang terpilih tersebut
            RectTransform areaRect = tablePick.GetComponent<RectTransform>();
            RectTransform letterRect = newLetter.GetComponent<RectTransform>();

            // Ambil batas aman agar huruf tidak keluar dari kotak putih di gambar
            float xLimit = (areaRect.rect.width / 2) * 0.8f;
            float yLimit = (areaRect.rect.height / 2) * 0.8f;

            letterRect.anchoredPosition = new Vector2(
                UnityEngine.Random.Range(-xLimit, xLimit),
                UnityEngine.Random.Range(-yLimit, yLimit)
            );
        }


    }

    void CheckIngredientComplete()
    {
        int correctCount = 0;

        foreach (LetterSlot slot in activeSlots)
        {
            if (slot.transform.childCount > 0)
            {
                // Cek komponen di child (huruf yang nempel)
                var letter = slot.GetComponentInChildren<DragnDrop>();
                if (letter != null && letter.hurufSaya == slot.hurufTarget)
                {
                    correctCount++;
                }
            }
        }

        // Jika satu kata bahan sudah benar semua
        if (correctCount == maskData.bahanList[currentMaskIndex].ingredientName.Length)
        {
            Debug.Log("Satu bahan selesai dieja!");
            currentMaskIndex++;

            // Update visual topeng di tengah berdasarkan progres
            UpdateIngredientVisual();

            if (currentMaskIndex < maskData.bahanList.Count)
            {
                // Lanjut ke bahan berikutnya setelah delay sedikit
                Invoke("StartNewIngredient", 1.5f);
            }
            else
            {
                FinishLevel();
            }
        }
    }

    void UpdateIngredientVisual()
    {
        // Cek apakah index sekarang masih dalam rentang jumlah child yang tersedia
        // Ini mencegah error "Index Out of Range" kalau jumlah child di Container beda sama di SO
        if (currentMaskIndex - 1 < maskVisualContainer.childCount / 2)
        {
            int visualIndex = currentMaskIndex - 1;
            int totalChild = maskVisualContainer.childCount;
            int offset = totalChild / 2; // Ini adalah jarak ke setengah bagian sisanya

            // 1. Matikan objek bayangan (di setengah pertama)
            GameObject objekBayangan = maskVisualContainer.GetChild(visualIndex).gameObject;
            objekBayangan.SetActive(false);
            Debug.Log("Mematikan " + objekBayangan.name);

            // 2. Nyalakan objek jadi (di posisi visualIndex + offset)
            // Jika 8 child, visualIndex 0 pasangannya adalah 0 + 4 = 4
            // Jika 6 child, visualIndex 0 pasangannya adalah 0 + 3 = 3
            GameObject objekJadi = maskVisualContainer.GetChild(visualIndex + offset).gameObject;
            objekJadi.SetActive(true);

            Debug.Log("Menukar " + objekBayangan.name + " dengan " + objekJadi.name);
        }
    }

    void FinishLevel()
    {
        Debug.Log("SEMUA BAHAN SELESAI! TOPENG JADI!");
        OnItemComplete?.Invoke();
        // Munculkan tombol Next Level atau Panel Win di sini
    }
}
