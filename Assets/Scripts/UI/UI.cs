using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements;
    
    private UI_Settings _uiSettings;
    private UI_MainMenu  _mainMenu;
    private UI_InGame _uiInGame;

    private void Awake()
    {
        _uiSettings = GetComponentInChildren<UI_Settings>(true);
        _mainMenu = GetComponentInChildren<UI_MainMenu>(true);
        _uiInGame = GetComponentInChildren<UI_InGame>(true);
        
        SwitchTo(_uiSettings.gameObject);
        // SwitchTo(_mainMenu.gameObject);
        SwitchTo(_uiInGame.gameObject);
    }

    public void SwitchTo(GameObject uiToEnable)
    {
        foreach (GameObject menu in uiElements)
        {
            menu.SetActive(false);
        }
        uiToEnable.SetActive(true);
    }

    public void QuitButton()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}
