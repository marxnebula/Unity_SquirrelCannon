using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Class for storing saved variables.
 *  - Doesn't delete when you change scenes.
 *  - Also saves user data to a file in all platforms except the web.
 *  - Must put this class in each scene so it is a PREFAB.
 *  - Save the nuts/coin if you beat the level.
 */

public class GameControl : MonoBehaviour {

    public static GameControl control;
    private int numberOfLevels = 1;
    private int currentLevel = 1;

    /* Variables for saving */
    public int[] nutsCollectedInLevel;
    public int[] coinCollectedInLevel;
    public bool[] isBeatLevel;

    public int totalAmountOfNutsInLevel = 0;
    public int currentNutsCollected = 0;
    public int currentCoinCollected = 0;
    public float time = 0;

    

    // Booleans for drawing which GUI
    private bool isMainMenu = false;
    public bool isGamePlay = false;
    private bool isGameOver = false;
    private bool isEndOfLevel = false;

    public bool isNewNutsRecord = false;
    public bool isNewCoinRecord = false;
    public bool isBeatLevelFirstTime = false;




	// Awake happens before Start()
	void Awake () {

        /*
         * So there can only be 1 control.(singleton)
         */
        // If control doesn't exist
        if(control == null)
        {
            // Doesn't destroy the game object when you change scenes
            DontDestroyOnLoad(gameObject);

            // Set control to this
            control = this;

        }
        // If control does exist but it is not this
        else if(control != this)
        {
            // Destroy gameobject because one already exists
            Destroy(gameObject);
        }
        
	}

    void Start()
    {
        if(nutsCollectedInLevel.Length == 0)
        {
            nutsCollectedInLevel = new int[numberOfLevels];
        }

        if (coinCollectedInLevel.Length == 0)
        {
            coinCollectedInLevel = new int[numberOfLevels];
        }

        if(isBeatLevel.Length == 0)
        {
            isBeatLevel = new bool[numberOfLevels];
        }
        
        
        // Might need to be in update
        totalAmountOfNutsInLevel = GameObject.FindGameObjectsWithTag("Nut").Length;
    }


    // Dat update
    void Update()
    {
        Timer();

        GameOver();

        print(control.isGameOver);
        
    }

    void GameOver()
    {
        if(isGameOver)
        {
            control.nutsCollectedInLevel[currentLevel - 1] = currentNutsCollected;
        }
    }

    public int GetNumberOfLevels()
    {
        return control.numberOfLevels;
    }

    public void AddOneNut()
    {
        control.currentNutsCollected++;
    }

    

    public void AddCoin()
    {
        control.currentCoinCollected = 1;
    }

    public int GetCurrentCoinCollected()
    {
        return control.currentCoinCollected;
    }

    public void SetCurrentCoinCollected(int coin)
    {
        control.currentCoinCollected = coin;
    }

    public int GetCurrentNutsCollected()
    {
        return control.currentNutsCollected;
    }

    public void SetCurrentNutsCollected(int nuts)
    {
        control.currentNutsCollected = nuts;
    }

    public int GetTotalAmountOfNutsInLevel()
    {
        return control.totalAmountOfNutsInLevel;
    }

    public void SetCurrentLevel(int cl)
    {
        control.currentLevel = cl;
    }

    /*
     * 
     */
    public void SetGameOver(bool b)
    {
        control.isGameOver = b;
    }

    /*
     * 
     */
    public bool GetGameOver()
    {
        return control.isGameOver;
    }

    /*
     * 
     */
    public void SetIsEndOfLevel(bool b)
    {
        control.isEndOfLevel = b;
    }

    /*
     * 
     */
    public bool GetIsEndOfLevel()
    {
        return control.isEndOfLevel;
    }


    /*
     * 
     */
    public void SetSavedVariables()
    {

        if(currentNutsCollected > nutsCollectedInLevel[currentLevel])
        {
            nutsCollectedInLevel[currentLevel] = currentNutsCollected;
            isNewNutsRecord = true;
        }
        else
        {
            isNewNutsRecord = false;
        }

        if(currentCoinCollected > coinCollectedInLevel[currentLevel])
        {
            coinCollectedInLevel[currentLevel] = currentCoinCollected;
            isNewCoinRecord = true;
        }
        else
        {
            isNewCoinRecord = false;
        }
        
        if(isBeatLevel[currentLevel] == false)
        {
            isBeatLevel[currentLevel] = true;
            isBeatLevelFirstTime = true;
        }
        else
        {
            isBeatLevelFirstTime = false;
        }
        
    }

    public void ResetVariables()
    {
        control.SetGameOver(false);
        control.SetCurrentNutsCollected(0);
        control.SetCurrentCoinCollected(0);
    }


    /*
     * 
     */
    void Timer()
    {
        if(control.time < 0)
        {
            control.time = 0;
        }
        else
        {
            control.time = (control.time - Time.deltaTime);
        }
        
    }



	

    /*
     * Saves data out into a file. This works on all platforms except the web.
     * You could save file as playerInfo.anything or just playerInfo
     */
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();

        // Create the file.
        // Application.persistentDataPath is the folder its going to and
        // "/playerInfo.dat" is the file name.
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        

        // Set the variables
        PlayerData data = new PlayerData();
        data.nutsCollectedInLevel = nutsCollectedInLevel;
        data.coinCollectedInLevel = coinCollectedInLevel;
        data.isBeatLevel = isBeatLevel;
        data.time = time;

        // Save the data to the file
        bf.Serialize(file, data);

        file.Close();
    }


    /*
     * Loads data from a file.  Make sure to check if file exist.
     * This works on all platforms except the web.
     */
    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            // Need to cast meaning specify that this is a player data object.
            // So it pulls the PlayerData data out of the file
            PlayerData data = (PlayerData)bf.Deserialize(file);

            file.Close();

            // Set your variables to the loaded data
            nutsCollectedInLevel = data.nutsCollectedInLevel;
            coinCollectedInLevel = data.coinCollectedInLevel;
            isBeatLevel = data.isBeatLevel;
            time = data.time;
        }
    }

}

/* 
 * This class needs to be serializable... meaning this data can be written to a file.
 */
[Serializable]
class PlayerData
{
    public int[] nutsCollectedInLevel;
    public int[] coinCollectedInLevel;
    public bool[] isBeatLevel;
    public float time;
}
