using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class HostGameMenuEvents : MonoBehaviour
{
    UIDocument document;

    TextField seed;
    Button solo, multi, back;

    private void Awake()
    {
        document = GetComponent<UIDocument>();

        seed = document.rootVisualElement.Query<TextField>();
        solo = document.rootVisualElement.Query<Button>("Solo");
        multi = document.rootVisualElement.Query<Button>("Multi");
        back = document.rootVisualElement.Query<Button>("Back");

        solo.RegisterCallback<ClickEvent>(OnSolo);
        multi.RegisterCallback<ClickEvent>(OnMulti);
        back.RegisterCallback<ClickEvent>(OnBack);
    }

    private void OnDisable()
    {
        solo.UnregisterCallback<ClickEvent>(OnSolo);
        multi.UnregisterCallback<ClickEvent>(OnMulti);
        back.UnregisterCallback<ClickEvent>(OnBack);
    }
    private bool TrySetRand()
    {
        string text = seed.text.Trim();
        if (int.TryParse(text, out int value))
        {
            Random.InitState(value);
            return true;
        } 
        else if (text == "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnSolo(ClickEvent _)
    {
        if (TrySetRand()) SceneManager.LoadScene("Game");
    }
    private void OnMulti(ClickEvent _)
    {
        Debug.Log("Multiplayer unimplemented");
    }
    private void OnBack(ClickEvent _)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
