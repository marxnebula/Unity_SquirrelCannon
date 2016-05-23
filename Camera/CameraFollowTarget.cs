using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Camera follows main character.
 *  - Doesn't start following until main char position is > 0.
 *  - It also sets the aspect and camera size based on what platform game is running on.
 */


public class CameraFollowTarget : MonoBehaviour {

    // Transform of what the camera follows
    public Transform player;

    // Offset for the camera
    public Vector3 offset;

    public bool isMoving = true;

    // Aspect
    public float aspect;
    
    // Current aspect
    public float currentAspect;


    public float topHeight = 0f;
    public float bottomHeight = 0f;
    private Vector2 adjustment;


    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // Set camera aspect
    //    DetermineCameraAspectAndSize();

     //   GetComponent<Rigidbody2D>().velocity = new Vector2(1f, 0f);

        Screen.SetResolution(1024, 768, false);
        
    }
    
	
	// Update is called once per frame
	void FixedUpdate () {


        followPlayer();

	}


    void followPlayer()
    {
        /*
        if(player.position.y > 1f)
        {
            adjustment = new Vector2(0f, 1.5f);
        }
        else if(player.position.y < -1.29f)
        {
            adjustment = new Vector2(0f, -1.5f);
        }
        else
        {
            adjustment = new Vector2(0f, 0f);
        }
        */
        
        if (player.position.y > 2.9f)
        {
            adjustment = new Vector2(0f, 4.35f);
        }
        else if (player.position.y < -2.9f)
        {
            adjustment = new Vector2(0f, -4.35f);
        }
        else
        {
            adjustment = new Vector2(0f, 0f);
        }
    //    adjustment = new Vector2(0f, 4.35f);
        
        if (!GameControl.control.GetGameOver())
        {


            if (player.GetComponent<Donkey>().GetIsEntered())
            {


                transform.position = Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y, -10),
                new Vector3(player.GetComponent<Donkey>().GetBarrelPosition().x + offset.x, offset.y + adjustment.y, -10),
                ref velocity, 0.15f);

            }
            else
            {

                transform.position = Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y, -10),
                new Vector3(player.position.x + offset.x, offset.y + adjustment.y, -10),
                ref velocity, 0.15f);
            }

        }
        
        

    }

    /*
     * Determine what the camera aspect is based on current device.
     * Then set the aspect and adjust the size to show entire screen.
     * The camera size was determined by trial and error.
     */
    void DetermineCameraAspectAndSize()
    {
        // If .exe then set the resolution
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // Set resolution
            Screen.SetResolution(640, 960, false);

            // Set aspect
            GetComponent<Camera>().aspect = 2f / 3f;

            // Set size
            Camera.main.orthographicSize = 4.82f;
        }
        else
        {


            // Get the current aspect
            currentAspect = (float)Screen.width / (float)Screen.height;


            // 3:4 = 0.75
            if (currentAspect > 0.70)
            {
                // Set aspect
                GetComponent<Camera>().aspect = 3f / 4f;

                // Set size
                Camera.main.orthographicSize = 4.30f;
            }
            // 2:3 = 0.6666666667
            else if (currentAspect > 0.64)
            {
                // Set Aspect
                GetComponent<Camera>().aspect = 2f / 3f;

                // Set size
                Camera.main.orthographicSize = 4.82f;
            }
            // 10:16 = 0.625
            else if (currentAspect > 0.60)
            {
                // Set aspect
                GetComponent<Camera>().aspect = 10f / 16f;

                // Set size
                Camera.main.orthographicSize = 5.15f;
            }
            // 10:17 = 0.5882
            else if (currentAspect > 0.57)
            {
                // Set aspect
                GetComponent<Camera>().aspect = 10f / 17f;

                // Set size
                Camera.main.orthographicSize = 5.72f;
            }
            // 9:16 = 0.5625
            else if (currentAspect > 0.50)
            {
                // Set aspect
                GetComponent<Camera>().aspect = 9f / 16f;

                // Set size
                Camera.main.orthographicSize = 5.7f;
            }
            // Just incase
            else
            {
                // Set aspect
                GetComponent<Camera>().aspect = 9f / 16f;

                // Set size
                Camera.main.orthographicSize = 5.7f;
            }

        }
    }
}
