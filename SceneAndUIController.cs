using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneAndUIController : MonoBehaviour
{
    public GameObject FadeOut;
    public GameObject FadeIn;
    public GameObject Canvas;
    public GameObject BlockClicks;
    public int MainMenuScene = 0;
    public int CurrentScene;
    // UI's
    public CanvasGroup MainmenuUI;
    public CanvasGroup SettingsUI;
    public CanvasGroup GameFileUI;
    public CanvasGroup GameUI;
    public CanvasGroup PauseUI;
    public CanvasGroup ConfirmationUI;
    public CanvasGroup SettingsVideoUI;

    public bool sceneLoaded;
    public string prevUI;
    public string curUI;
    public CanvasGroup currentUI;
    public CanvasGroup previousUI;
    public bool Paused;

    public static SceneAndUIController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Canvas = GameObject.FindWithTag("MainCanvas");
        MainmenuUI.gameObject.SetActive(false);
        GameFileUI.gameObject.SetActive(false);
        SettingsUI.gameObject.SetActive(false);
        ConfirmationUI.gameObject.SetActive(false);
        GameUI.gameObject.SetActive(false);
        PauseUI.gameObject.SetActive(false);
        SettingsVideoUI.gameObject.SetActive(false);

        Paused = false;
        MainmenuUI.alpha = 0;
        GameFileUI.alpha = 0;
        SettingsUI.alpha = 0;
        ConfirmationUI.alpha = 0;
        GameUI.alpha = 0;
        PauseUI.alpha = 0;
        SettingsVideoUI.alpha = 0;
        Time.timeScale = 1;
        curUI = "mainmenu";
        currentUI = MainmenuUI;
        SetUI("mainmenu");
    }

    private void Update()
    {
        if (Canvas == null)
        {
            Canvas = GameObject.FindWithTag("MainCanvas");
        }

        CurrentScene = SceneManager.GetActiveScene().buildIndex;
        // (If scene isnt main menu)
        if (CurrentScene != MainMenuScene)
        {
            // Pause game from ESC-key
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
    }

    public void SetUI(string UI)
    {
        DisableButtons();
        Invoke("EnableButtons", 2f);
        switch (UI)
        {
            case "loadingScreen":
                StartCoroutine(DisableAndSetUI("loadingScreen", null));
                LoadingScreen.SetActive(true);
                break;

            case "mainmenu":
                StartCoroutine(DisableAndSetUI("mainmenu", MainmenuUI));
                break;

            case "game":
                StartCoroutine(DisableAndSetUI("game", GameUI));
                break;

            case "pause":
                StartCoroutine(DisableAndSetUI("pause", PauseUI));
                break;

            case "savefiles":
                StartCoroutine(DisableAndSetUI("savefiles", GameFileUI));
                break;

            case "confirm":
                StartCoroutine(DisableAndSetUI("confirm", ConfirmationUI));
                break;

            case "settings":
                StartCoroutine(DisableAndSetUI("settings", SettingsUI));
                break;

            default:
                StartCoroutine(DisableAndSetUI("null", null));
                Debug.Log("Error: SetUI went into default");
                break;
        }
    }


    public IEnumerator DisableAndSetUI(string ui, CanvasGroup NewUI)
    {
        // Save previous and current UI's as strings that can be used in SetUi();
        prevUI = curUI;
        curUI = ui;
        // Save previous and current UI's as CanvasGroups that can be used to alter the alpha value (visibility)
        previousUI = currentUI;
        currentUI = NewUI;

        if (!Paused && currentUI != GameUI && previousUI != null && previousUI != PauseUI && previousUI != GameUI)
        {
            // Fade UI out
            while (previousUI.alpha > 0)
            {
                previousUI.alpha -= Time.deltaTime;
                yield return null;
            }
        }

        DisableAllUI();

        if (NewUI != null && !Paused)
        {
            NewUI.gameObject.SetActive(true);
            // Fade the ui in
            while (NewUI.alpha < 1)
            {
                if (NewUI == PauseUI)
                {
                    NewUI.alpha = 1;
                }
                NewUI.alpha += Time.deltaTime;
                yield return null;
            }
        }
        UICustomizations(NewUI);
    }

    public void Cancel()
    {
        // If current scene isnt main menu
        if (CurrentScene == 0)
        {
            SetUI("mainmenu");
        }
        // If current scene isnt main menu AND last ui isnt pause UI
        else
        {
            SetUI(prevUI);
        }
    }

    public void Pause()
    {
        if (Paused)
        {
            Paused = false;
            Time.timeScale = 1;
            SetUI("game");
        }
        else if (!Paused)
        {
            Debug.Log("Disable player input here");
            Paused = true;
            Time.timeScale = 0;
            SetUI("pause");
        }
    }

    public void UICustomizations(CanvasGroup ui)
    {
        if (ui == SettingsUI)
        {
            SettingsVideoUI.gameObject.SetActive(true);
            SettingsVideoUI.alpha = 1;
        }

        if (Paused)
        {
            EnableButtons();
            ui.gameObject.SetActive(true);
            ui.alpha = 1;
        }
    }

    public void DisableAllUI()
    {

        if (CurrentScene == 0)
        {
            GameUI.gameObject.SetActive(false);
            GameUI.alpha = 0;
        }

        PauseUI.gameObject.SetActive(false);
        MainmenuUI.gameObject.SetActive(false);
        GameFileUI.gameObject.SetActive(false);
        SettingsUI.gameObject.SetActive(false);
        ConfirmationUI.gameObject.SetActive(false);
        SettingsVideoUI.gameObject.SetActive(false);
        LoadingScreen.SetActive(false);

        PauseUI.alpha = 0;
        MainmenuUI.alpha = 0;
        GameFileUI.alpha = 0;
        SettingsUI.alpha = 0;
        ConfirmationUI.alpha = 0;
        SettingsVideoUI.alpha = 0;
    }
    public void DisableButtons()
    {
        PauseUI.interactable = false;
        MainmenuUI.interactable = false;
        GameFileUI.interactable = false;
        SettingsUI.interactable = false;
        ConfirmationUI.interactable = false;
        SettingsVideoUI.interactable = false;
    }

    public void EnableButtons()
    {
        PauseUI.interactable = true;
        MainmenuUI.interactable = true;
        GameFileUI.interactable = true;
        SettingsUI.interactable = true;
        ConfirmationUI.interactable = true;
        SettingsVideoUI.interactable = true;
    }


    // LOADING SCREEN

    public GameObject LoadingScreen;

    public void LoadScene(int sceneId)
    {
        Debug.Log("Disable player input here");
        Time.timeScale = 1;
        Paused = false;
        FadeIn.SetActive(true);
        StartCoroutine(LoadsceneAsync(sceneId, 1f));
    }

    IEnumerator LoadsceneAsync(int sceneId, float animDuration)
    {
        yield return new WaitForSeconds(animDuration);
        SetUI("loadingScreen");
        yield return new WaitForSeconds(1);
        // Load the scene and use "operation" to check when the scene finishes loading
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            sceneLoaded = false;
            yield return null;
        }

        if (operation.isDone)
        {
            sceneLoaded = true;
            if (sceneId != 0)
            {
                Debug.Log("Scene finished loading: Switching to game ui");
                SetUI("game");

                FadeIn.SetActive(false);
                FadeOut.SetActive(false);
            }
            else
            {
                FadeIn.SetActive(false);
                FadeOut.SetActive(true);

                Debug.Log("Scene finished loading: Switching to Main menu ui");
                SetUI("mainmenu");
                // Deletes health gameobjects
                GameController.Instance.Player.PH.SetHP(0);

                yield return new WaitForSeconds(1f);
                FadeOut.SetActive(false);
            }
        }
    }
}
