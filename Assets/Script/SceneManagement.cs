using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private GameObject panelComingSoon;
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject ButtonLevel;
    [SerializeField] Camera mainCamera;

    public void Scene1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void PilihLevel()
    {
        Debug.Log("Pilih Level");
        panelMainMenu.SetActive(false);
        ButtonLevel.SetActive(true);
    }

    public void BackToMenu()
    {
        panelMainMenu.SetActive(true);
        ButtonLevel.SetActive(false);
    }

    public void ComingSoon()
    {
        StartCoroutine(DelayPanel());
    }

    IEnumerator DelayPanel()
    {
        panelComingSoon.SetActive(true);
        yield return new WaitForSeconds(2);
        panelComingSoon.SetActive(false);
    }
}
