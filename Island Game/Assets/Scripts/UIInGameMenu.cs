using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIInGameMenu : MonoBehaviour
{
    public Button saveBTN, ExitBTN;
    public GameObject gameMenuPanel,loadingPanel;

    public bool MenuVisible{get => gameMenuPanel.activeSelf; }

    private void Start(){
        GameManager manager = FindObjectOfType<GameManager>();
        saveBTN.onClick.AddListener(manager.SaveGame);
        ExitBTN.onClick.AddListener(manager.ExitToMainMenu);
        gameMenuPanel.SetActive(false);
    }
    public void ToggleMenu(){
        gameMenuPanel.SetActive(!gameMenuPanel.activeSelf);
    }
    public void ToggleLoadingPanel(){
        loadingPanel.SetActive(!loadingPanel.activeSelf);
    }
}
