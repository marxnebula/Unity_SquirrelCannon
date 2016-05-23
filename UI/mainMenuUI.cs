using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class mainMenuUI : MonoBehaviour {

    public bool isMainMenu = false;
    public bool isLevelSelect = false;

    public Image continueButton;
    public Image levelSelectButton;
    public Image backButton;

    public Image[] levelButtons;


	// Use this for initialization
	void Start () {

     //   levelButtons = new Image[GameControl.control.GetNumberOfLevels()];

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].enabled = false;
        }

        backButton.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

        CheckUserInput();

	}

    /*
    * Checks users input based on which platform they are on.
    * It then calls a function to check if a certain button was pressed.
    */
    void CheckUserInput()
    {
        // If user is running on android
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                // TouchPhase.Began means a finger touched the screen
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    CheckWhichButtonTouched(Input.GetTouch(0).position);
                }
            }
        }

        // If user is running the editor, windows, or mac
        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // If left mouse button is up
            if (Input.GetMouseButtonUp(0))
            {
                CheckWhichButtonTouched(Input.mousePosition);
            }
        }
    }


    /*
     * Checks which button was pressed if any.
     */
    void CheckWhichButtonTouched(Vector3 pos)
    {

        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = pos;

        // Gets a list of raycast
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);


        if (raycastResults.Count > 0)
        {
            // If it's play button
            if (raycastResults[0].gameObject.name == "ContinueButton")
            {
                print("Play button hit");

                // If this is a replay then load new level
                if (GameControl.control.GetGameOver())
                {
                //    GameControl.control.SetGameOver(false);

                    // Load the replay level
                  //  SceneManager.LoadScene("normal");

                }
            }

            if (raycastResults[0].gameObject.name == "LevelSelectButton")
            {

                DisplayLevelSelect();

                print("Level Select");

            }

            if (raycastResults[0].gameObject.name == "BackButton")
            {
                print("A level button hit");

                BackButtonPressed();

                // Load the replay level
                //   SceneManager.LoadScene(raycastResults[0].gameObject.name);


            }
        }
    }


    void DisplayLevelSelect()
    {

        continueButton.enabled = false;
        levelSelectButton.enabled = false;
        backButton.enabled = true;

        // Display levels
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].enabled = true;
        }

    }

    void BackButtonPressed()
    {
        continueButton.enabled = true;
        levelSelectButton.enabled = true;
        backButton.enabled = false;

        // Display levels
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].enabled = false;
        }
    }


   
}
