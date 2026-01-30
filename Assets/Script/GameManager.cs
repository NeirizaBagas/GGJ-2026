using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private MaskData maskData;

    [Header("Prefabs")]
    [SerializeField] GameObject letterSlotPrefab;
    [SerializeField] GameObject letterObjekPrefab;

    [Header("UI Elements")]
    [SerializeField] private Transform slotContainer;
    [SerializeField] private Transform tableArea;
    [SerializeField] private Transform maskVisualContainer;

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
        if (maskData != null)
        {
            StartNewIngredient();
        }

        foreach (Transform bahan in maskVisualContainer)
        {
            bahan.gameObject.SetActive(false);
        }
    }

    private void StartNewIngredient()
    {
        // bersihkan slot dan area meja
        foreach (Transform child in slotContainer) Destroy(child.gameObject);
        foreach (Transform child in tableArea) Destroy(child.gameObject);
        activeSlots.Clear();

        // set mask visual
        string wordToSpell = maskData.ingredientNames[currentMaskIndex];
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
            GameObject newLetter = Instantiate(letterObjekPrefab, tableArea);
            newLetter.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString();

            newLetter.GetComponent<DragnDrop>().hurufSaya = c;

            RectTransform rect = newLetter.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(Random.Range(-400, 400), Random.Range(-250, 250));
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
        if (correctCount == maskData.ingredientNames[currentMaskIndex].Length)
        {
            Debug.Log("Satu bahan selesai dieja!");
            currentMaskIndex++;

            // Update visual topeng di tengah berdasarkan progres
            UpdateIngredientVisual();

            if (currentMaskIndex < maskData.ingredientNames.Length)
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
        if (currentMaskIndex - 1 < maskVisualContainer.childCount)
        {
            // currentIngredientIndex - 1 karena index array/child mulai dari 0
            // sedangkan currentIngredientIndex bertambah SETELAH ejaan selesai
            int visualIndex = currentMaskIndex - 1;

            // Set aktif objek visual bahan yang baru saja selesai dieja
            maskVisualContainer.GetChild(visualIndex).gameObject.SetActive(true);

            Debug.Log("Bahan " + maskData.ingredientNames[visualIndex] + " Muncul!");
        }
    }

    void FinishLevel()
    {
        Debug.Log("SEMUA BAHAN SELESAI! TOPENG JADI!");
        // Munculkan tombol Next Level atau Panel Win di sini
    }
}
