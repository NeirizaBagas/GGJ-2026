using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private GameObject panelComingSoon;
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject[] itemImage;
    [SerializeField] private GameObject ButtonLevel;
    [SerializeField] Camera mainCamera;
    [SerializeField] public RectTransform bgPanel;
    [SerializeField] float bgOriPos;
    [SerializeField] float transitionSpeed;
    [SerializeField] GameObject creditPanel;
    [SerializeField] GameObject settingPanel;


    private void Start()
    {
        bgOriPos = bgPanel.localPosition.x;
    }

    public void Scene1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Scene2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Scene3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void PilihLevel()
    {
        Debug.Log("Pilih Level");
        panelMainMenu.SetActive(false);
        ButtonLevel.SetActive(true);
        ItemImageControl(false);
        //bgPanel.localPosition = new Vector3(255, 0, 0) * Time.deltaTime;
        StartCoroutine(MovePanel(new Vector2(255, 0)));
    }

    public void BackToMenu()
    {
        panelMainMenu.SetActive(true);
        ButtonLevel.SetActive(false);
        ItemImageControl(true);
        //bgPanel.localPosition = new Vector3(bgOriPos, 0, 0) * Time.deltaTime;
        StartCoroutine(MovePanel(new Vector2(0, 0)));
    }

    public void ComingSoon()
    {
        StartCoroutine(DelayPanel());
    }

    private void ItemImageControl(bool control)
    {
        for (int i = 0; i < itemImage.Length; i++)
        {
            itemImage[i].SetActive(control);
        }

    }

    public void OpenCreditPanel()
    {
        creditPanel.SetActive(true);
    }

    public void CloseCreditPanel()
    {
        creditPanel.SetActive(false);
    }

    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    IEnumerator DelayPanel()
    {
        panelComingSoon.SetActive(true);
        yield return new WaitForSeconds(2);
        panelComingSoon.SetActive(false);
    }

    IEnumerator MovePanel(Vector2 targetPos)
    {
        // We move until the distance is very small
        while (Vector2.Distance(bgPanel.anchoredPosition, targetPos) > 0.1f)
        {
            bgPanel.anchoredPosition = Vector2.Lerp(bgPanel.anchoredPosition,targetPos,Time.deltaTime * transitionSpeed);
            yield return null; // Wait for the next frame
        }

        // Snap to exact position at the end
        bgPanel.anchoredPosition = targetPos;
    }
}
