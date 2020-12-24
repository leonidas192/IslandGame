using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{

public Button newGameBTN,resumeBTN;
    // Start is called before the first frame update
    void Start()
    {
        GameManager manager = FindObjectOfType<GameManager>();
        newGameBTN.onClick.AddListener(manager.StartNextScene);
        resumeBTN.onClick.AddListener(manager.LoadSavedGame);
        resumeBTN.interactable =false;
        if(manager.CheckSavedGameExist()){
            resumeBTN.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
