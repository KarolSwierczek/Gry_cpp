using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    /*
    public GameObject StartMenu;
    public GameObject GameOptionsMenu;

    public GameObject RButtons;
    public GameObject PButtons;

    private List<Transform> RoundButtons = new List<Transform>();
    private List<Transform> PlayerNumber = new List<Transform>();

    private Transform tempObject;
    
    // Start is called before the first frame update
    void Start()
    {
        StartMenu.active = true;
        GameOptionsMenu.active = false;

        int children = RButtons.transform.childCount;
        int tempNumber = 5;

        for (int b = 0; b < children; b++)
        {
            RButtons.transform.GetChild(b).GetComponent<Button>().onClick.AddListener(() =>
            {
                tempObject = RButtons.transform.Find("Rounds" + (tempNumber).ToString());

                tempNumber = tempNumber + 5 * (b + 1);

                //GetChild(1) refers to "Graphic" GameObject in Button
                tempObject.transform.GetChild(1).GetComponent<Image>().color = Color.red;
            });
            RoundButtons.Add(RButtons.transform.GetChild(b));


            PButtons.transform.GetChild(b).GetComponent<Button>().onClick.AddListener(() =>
            {
                tempObject = PButtons.transform.Find("Players" + (b + 2).ToString());

                //GetChild(1) refers to "Graphic" GameObject in Button
                tempObject.transform.GetChild(1).GetComponent<Image>().color = Color.red;
            });
            PlayerNumber.Add(PButtons.transform.GetChild(b));
        }

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
    */
}

