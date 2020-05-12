using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public GameObject StartMenu;
    public GameObject GameOptionsMenu;

    // Start is called before the first frame update
    void Start()
    {
        StartMenu.active = true;
        GameOptionsMenu.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void returnButtonOptionToStartMenu()
    {
        StartMenu.active = true;
        GameOptionsMenu.active = false;
    }

    public void gameOptionMenuButton()
    {
        StartMenu.active = false;
        GameOptionsMenu.active = true;
    }
}

