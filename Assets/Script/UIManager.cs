using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject slotContainer;
    [SerializeField] private GameObject LetterContainer;
    [SerializeField] private GameObject bahanDasar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bahanDasar.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.OnItemComplete += ShowCraftingUi;
    }

    private void OnDisable()
    {
        GameManager.OnItemComplete -= ShowCraftingUi;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowCraftingUi()
    {
        slotContainer.SetActive(false);
        LetterContainer.SetActive(false);
        bahanDasar.SetActive(true);
    }

    public void CompleteBackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
