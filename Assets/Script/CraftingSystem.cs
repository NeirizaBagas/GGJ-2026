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
    }

    private void OnDisable()
    {
        DrawCompleted.OnFirstStepComplete -= OnMiniGameFinished;    
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
            string targetItem = currentLevelData.craftingRequiredItems[currentStep];

            if (item.itemType == targetItem)
            {
                HandleSuccessfulDrop(item);
            }
        }
    }

    private void HandleSuccessfulDrop(DragnDrop item)
    {
        // Matikan petunjuk alat saat ini
        if (currentStep < shadowClues.Length) shadowClues[currentStep].SetActive(false);

        // Snap & Sembunyikan item
        item.transform.SetParent(craftingContainer);
        item.transform.localPosition = Vector3.zero;
        item.gameObject.SetActive(false);

        DetermineNextAction();
    }

    private void DetermineNextAction()
    {
        string currentItem = currentLevelData.craftingRequiredItems[currentStep];

        // 1. Jika Bahan Dasar (Kertas, Daun, Karton)
        if (currentItem == "Paper" || currentItem == "Leaf" || currentItem == "CardBoard")
        {
            visualBahanDasar.SetActive(true); // Meja sekarang ada bahan
            PrepareNextStep();
        }
        // 2. Jika Alat Mini-game
        else if (IsMiniGame1Item(currentItem))
        {
            craftingState = CraftingState.MiniGame;
            firstMiniGame.SetActive(true); // Mini-game menimpa tampilan meja
            //StartMiniGame(currentItem);
        }
        else if (IsMiniGame2Item(currentItem))
        {
            craftingState = CraftingState.MiniGame;
            secondMiniGame.SetActive(true); // Mini-game menimpa tampilan meja
            //StartMiniGame(currentItem);
        }
    }

    //private void StartMiniGame(string tool)
    //{
    //    // Matikan semua dulu, nyalakan yang sesuai
    //    dotGunting.SetActive(tool == "Gunting");
    //    dotLem.SetActive(tool == "Lem");
    //    dotBenang.SetActive(tool == "Benang");
    //    dotCat.SetActive(tool == "Cat");
    //}

    public void OnMiniGameFinished()
    {
        firstMiniGame.SetActive(false); // Sembunyikan mini-game
        secondMiniGame.SetActive(false);
        Debug.Log("Mini-game selesai untuk langkah " + currentStep);
        // Jika ini langkah terakhir, ubah jadi topeng jadi
        if (currentStep == currentLevelData.craftingRequiredItems.Length - 1)
        {
            visualBahanDasar.SetActive(false);
            visualTopengJadi.SetActive(true); // Bahan dasar berubah jadi topeng
            craftingState = CraftingState.Selesai;
            Debug.Log("Level Selesai!");
        }
        else
        {
            PrepareNextStep();
        }
    }

    private void PrepareNextStep()
    {
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
        return name == "Glue" || name == "Paint" || name == "Yarn";
    }
}