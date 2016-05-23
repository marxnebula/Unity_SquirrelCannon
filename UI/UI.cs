using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Displays all UI buttons including Play, Upgrade, Select Char.
 *  - Also displays the pop up menus.
 *  - If game over it changes play to replay.
 *  - Replay button will load the game play screen.
 */

public class UI : MonoBehaviour {

    // Text for UI
   // public Text coinText;
    public Text nutText;
    public Text coinText;

    // Images for UI
 //   public Image playButton;
 //   public Image endImage;
    public Image retryButton;
    public Image quitButton;
    public Image continueButton;

    // Booleans for drawing UI
    private bool isMainMenu = false;
    private bool isGamePlay = false;
    public bool isGameOver = false;

    // String for scene name
    private string mainMenuScene = "FirstScene";
    private string replayScene = "ReplayScene";



    void Start()
    {
        /*
        // If the scene is main menu
        if (SceneManager.GetActiveScene().name == mainMenuScene)
        {
            // Set game play to false
            isGamePlay = false;

         //   GameControl.control.Save();
            GameControl.control.Load();
        }
        // If the scene is not main menu(this game only has 2 scenes so must be game play scene)
        else
        {
            // Set the game play to true
            isGamePlay = true;
        }
        */

        retryButton.enabled = false;
        quitButton.enabled = false;
        continueButton.enabled = false;
        GameControl.control.SetGameOver(false);
    }
	
	// Update is called once per frame
	void Update () {

        // Display users stats
        DisplayUserStats();


        DisplayGameOver();

        DisplayEndOfLevel();

        // Check the users input
        CheckUserInput();

        print(GameControl.control.GetGameOver());
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
            if (raycastResults[0].gameObject.name == "RetryButton")
            {
                print("Play button hit");


                GameControl.control.ResetVariables();
                retryButton.enabled = false;
                quitButton.enabled = false;


                // Load the replay level
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

                
            }

            if (raycastResults[0].gameObject.name == "ContinueButton")
            {
                print("A level button hit");


                // Load the replay level
              //  SceneManager.LoadScene(raycastResults[0].gameObject.name);


            }

            if (raycastResults[0].gameObject.name == "QuitButton")
            {
                print("A level button hit");


                // Load the replay level
                SceneManager.LoadScene("mainMenu");

                
            }
        }
    }


   


    /*
     * Displays the users stats which are in GameControl.
     */
    void DisplayUserStats()
    {

        nutText.text = "Nuts: " + GameControl.control.GetCurrentNutsCollected() + " / " +
            GameControl.control.GetTotalAmountOfNutsInLevel();


        coinText.text = "Coin: " + GameControl.control.GetCurrentCoinCollected() + " / " +
            1;
    }


    /*
     * 
     */
    void DisplayGameOver()
    {
        if(GameControl.control.GetGameOver() &&
            !GameControl.control.GetIsEndOfLevel())
        {
            // Display replay button, and back button
            retryButton.enabled = true;
            quitButton.enabled = true;

            
        }

    }

    void DisplayEndOfLevel()
    {
        if(GameControl.control.GetIsEndOfLevel())
        {
            print("You beat the level");
        //    endImage.enabled = true;
            // Display stats and % of completed level
            // Display continue to next level and main menu button
            continueButton.enabled = true;
            quitButton.enabled = true;

         //   GameControl.control.SetSavedVariables();
        }

    }


    void SetCurrentLevel()
    {

    }


    /*
     * Sets isGameOver boolean to true.
     */
    public void SetGameOver()
    {
        isGameOver = true;
    }


    /*
     * Sets isGameOver boolean to true.
     */
    public bool GetGamePlay()
    {
        return isGamePlay;
    }
}
