using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class PauseMenuEvents : MonoBehaviour
{
    UIDocument document;

    Button resume, quitToMainMenu, quitToDesktop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        document = GetComponent<UIDocument>();

        Controls.active.onPause.AddListener(EnableMenu);
        Controls.active.onResume.AddListener(DisableMenu);

        DisableMenu();
    }

    private void OnDestroy()
    {
        Controls.active.onPause.RemoveListener(EnableMenu);
        Controls.active.onResume.RemoveListener(DisableMenu);

        resume?.UnregisterCallback<ClickEvent>(OnResume);
        quitToMainMenu?.UnregisterCallback<ClickEvent>(OnQuitToMainMenu);
        quitToDesktop?.UnregisterCallback<ClickEvent>(OnQuitToDesktop);
    }

    // Update is called once per frame
    void EnableMenu()
    {
        document.enabled = true;
        resume = document.rootVisualElement.Query<Button>("Resume");
        quitToMainMenu = document.rootVisualElement.Query<Button>("QuitToMain");
        quitToDesktop = document.rootVisualElement.Query<Button>("QuitToDesk");
        resume.RegisterCallback<ClickEvent>(OnResume);
        quitToMainMenu.RegisterCallback<ClickEvent>(OnQuitToMainMenu);
        quitToDesktop.RegisterCallback<ClickEvent>(OnQuitToDesktop);
    }
    void DisableMenu()
    {
        resume?.UnregisterCallback<ClickEvent>(OnResume);
        quitToMainMenu?.UnregisterCallback<ClickEvent>(OnQuitToMainMenu);
        quitToDesktop?.UnregisterCallback<ClickEvent>(OnQuitToDesktop);
        resume = null;
        quitToMainMenu = null;
        quitToDesktop = null;
        document.enabled = false;
    }
    void OnResume(ClickEvent _)
    {
        Controls.active.isPaused = false;
        DisableMenu();
    }
    void OnQuitToMainMenu(ClickEvent _)
    {
        SceneManager.LoadScene("MainMenu");
    }
    void OnQuitToDesktop(ClickEvent _)
    {
        Application.Quit();
    }
}
