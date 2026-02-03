using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CraftingState { MenungguBahan, MiniGame, Selesai }

public class CraftingSystem : MonoBehaviour, IDropHandler
{
    [Header("Data Level")]
    public MaskData currentLevelData; // Tarik ScriptableObject level ke sini
    public CraftingState craftingState = CraftingState.MenungguBahan;
    public int currentStep = 0;

    [Header("Visual References")]
    public GameObject visualBahanDasar; // Objek Kertas/Daun/Karton di meja
    public GameObject visualTopengJadi; // Objek Topeng Final
    public GameObject[] shadowClues;    // Bayangan alat sebagai petunjuk
    private Transform craftingContainer;

    [Header("Mini-Game References")]
    public GameObject firstMiniGame; // Parent yang menimpa visual bahan dasar
    public GameObject secondMiniGame;

    private void OnEnable()
    {
        DrawCompleted.OnFirstStepComplete += OnMiniGameFinished;
        DrawingTracer._OnLevelComplete += OnMiniGameFinished;
    }

    private void OnDisable()
    {
        DrawCompleted.OnFirstStepComplete -= OnMiniGameFinished;  
        DrawingTracer._OnLevelComplete -= OnMiniGameFinished;
    }

    void Start()
    {
        craftingContainer = transform.Find("CraftingContainer");

        // Sembunyikan semua visual di awal (Meja Kosong)
        visualBahanDasar.SetActive(false);
        visualTopengJadi.SetActive(false);
        firstMiniGame.SetActive(false);
        secondMiniGame.SetActive(false);

        // Aktifkan petunjuk pertama
        UpdateShadowClue();
    }

    public void OnDrop(PointerEventData eventData)
    {
        DragnDrop item = eventData.pointerDrag.GetComponent<DragnDrop>();
        Debug.Log("Item dropped: " + (item != null ? item.itemType : "null"));

        // Validasi: Apakah item benar dan sedang menunggu bahan?
        if (item != null && craftingState == CraftingState.MenungguBahan)
        {
            Debug.Log("Current Step: " + currentStep);
            string targetItem = currentLevelData.craftingRequiredItems[currentStep];

            if (item.itemType == targetItem)
            {
                HandleSuccessfulDrop(item);
            }
            else
            {
               Debug.Log("Item tidak sesuai. Diperlukan: " + targetItem + ", Diberikan: " + item.itemType);
            }
        }
    }

    private void HandleSuccessfulDrop(DragnDrop item)
    {
        // Matikan petunjuk alat saat ini
        if (currentStep < shadowClues.Length) shadowClues[currentStep].SetActive(false);
        Debug.Log("Berhasil bahan");
        // Snap & Sembunyikan item
        item.transform.SetParent(craftingContainer);
        item.transform.localPosition = Vector3.zero;
        item.gameObject.SetActive(false);

        DetermineNextAction();
    }

    private void DetermineNextAction()
    {
        string currentItem = currentLevelData.craftingRequiredItems[currentStep];
        Debug.Log("Menentukan aksi berikutnya untuk item: " + currentItem);

        // Reset semua mini-game agar tidak tumpang tindih
        firstMiniGame.SetActive(false);
        secondMiniGame.SetActive(false);

        if (currentItem == "Paper" || currentItem == "Leaf" || currentItem == "CardBoard")
        {
            Debug.Log("currentItem adalah bahan dasar: ");
            visualBahanDasar.SetActive(true);
            Debug.Log(visualBahanDasar.name + " diaktifkan.");
            PrepareNextStep();
        }
        else if (IsMiniGame1Item(currentItem))
        {
            craftingState = CraftingState.MiniGame;
            firstMiniGame.SetActive(true);
            // Pastikan script di dalam firstMiniGame di-reset kondisinya
        }
        else if (IsMiniGame2Item(currentItem))
        {
            craftingState = CraftingState.MiniGame;
            secondMiniGame.SetActive(true);
        }
    }

    public void OnMiniGameFinished()
    {
        // Hanya proses jika memang sedang dalam state MiniGame
        if (craftingState != CraftingState.MiniGame) return;

        firstMiniGame.SetActive(false);
        secondMiniGame.SetActive(false);

        if (currentStep == currentLevelData.craftingRequiredItems.Length - 1)
        {
            // Kondisi Selesai Level
            visualBahanDasar.SetActive(false);
            Debug.Log("CRAFTING SELESAI! TOPENG JADI!");
            visualTopengJadi.SetActive(true);
            craftingState = CraftingState.Selesai;
        }
        else
        {
            // Lanjut ke item berikutnya (Misal dari Paint ke Brush)
            StartCoroutine(PrepareNextStep());
        }
    }

    IEnumerator PrepareNextStep()
    {
        yield return new WaitForSeconds(2f);
        currentStep++;
        craftingState = CraftingState.MenungguBahan;
        UpdateShadowClue();
    }

    private void UpdateShadowClue()
    {
        // Nyalakan bayangan alat selanjutnya
        if (currentStep < currentLevelData.craftingRequiredItems.Length && currentStep < shadowClues.Length)
        {
            shadowClues[currentStep].SetActive(true);
        }
    }

    private bool IsMiniGame1Item(string name)
    {
        return name == "Scissor";
    }

    private bool IsMiniGame2Item(string name)
    {
        return name == "Glue" || name == "Yarn" || name == "Paint";
    }
}