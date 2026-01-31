using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject slotContainer;
    [SerializeField] private GameObject LetterContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        GameManager.OnItemComplete += ShowCraftingUi;
    }

    private void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowCraftingUi()
    {
        slotContainer.SetActive(false);
        LetterContainer.SetActive(false);
    }
}
