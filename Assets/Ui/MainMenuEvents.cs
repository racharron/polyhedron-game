using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuEvents : MonoBehaviour
{
    UIDocument document;

    Button newGame, joinGame, quit;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        
        newGame = document.rootVisualElement.Query<Button>("New");
        joinGame = document.rootVisualElement.Query<Button>("Join");
        quit = document.rootVisualElement.Query<Button>("Quit");

        newGame.RegisterCallback<ClickEvent>(OnNew);
        joinGame.RegisterCallback<ClickEvent>(OnJoin);
        quit.RegisterCallback<ClickEvent>(OnQuit);
    }

    private void OnDisable()
    {
        newGame.UnregisterCallback<ClickEvent>(OnNew);
        joinGame.UnregisterCallback<ClickEvent>(OnJoin);
        quit.UnregisterCallback<ClickEvent>(OnQuit);
    }

    private void OnNew(ClickEvent _)
    {
        SceneManager.LoadScene("NewGameMenu");
    }
    private void OnJoin(ClickEvent _)
    {
        Debug.Log("Multiplayer unimplemented");
    }
    private void OnQuit(ClickEvent _)
    {
        Application.Quit();
    }
}
