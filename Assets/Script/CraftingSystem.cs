using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CraftingState { MenungguBahan, MiniGame, Selesai }

public class CraftingSystem : MonoBehaviour, IDropHandler
{
    public CraftingState craftingState = CraftingState.MenungguBahan;
    public int currentStep = 0; //Index bahan yang dibutuhkan
    public string[] requiredItems; //Daftar nama bahan yang dibutuhkan
    //public string[] currentItemStates;

    [Header("UI References")]
    public GameObject[] shadowObjects; // Array bayangan bahan (shadow1, shadow2, dst)
    public GameObject dotMiniGamePotong;
    public GameObject dotMiniGameLem;
    public Transform maskProgressVisual; // Tempat sprite topeng berubah

    private Transform craftingContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        craftingContainer = transform.Find("CraftingContainer");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop di CraftingSystem");
        
        DragnDrop _dragndrop = eventData.pointerDrag.GetComponent<DragnDrop>();

        if (_dragndrop != null && craftingState == CraftingState.MenungguBahan)
        {
            if (_dragndrop.itemType == requiredItems[currentStep])
            {
                HandleSuccessfulDrop(_dragndrop);
            }
        }
    }

    private void HandleSuccessfulDrop(DragnDrop item)
    {
        shadowObjects[currentStep].SetActive(false);

        item.transform.SetParent(craftingContainer);
        item.transform.localPosition = Vector3.zero;
        item.gameObject.SetActive(false);

        DetermineNextAction();
    }

    private void DetermineNextAction()
    {
        string currentItem = requiredItems[currentStep];

        switch (craftingState)
        {
            case CraftingState.MenungguBahan:
                UpdateMaskVisual(currentItem);
                break;
            case CraftingState.MiniGame:
                break;
            case CraftingState.Selesai:
                break;
        }
    }

    private void UpdateMaskVisual(string item)
    {
        if (item == "Kertas")
        {

        }
        else if (item == "Daun")
        {
        }
        else if (item == "Karton")
        {
        }
    }

    private void StartMiniGame(string item)
    {
        if (item == "Gunting")
        {
            dotMiniGamePotong.SetActive(true);
        }
        else if (item == "Lem")
        {
            dotMiniGameLem.SetActive(true);
        }
    }

    private void CompleteCrafting()
    {
        Debug.Log("Crafting Selesai!");
    }
}
