using UnityEngine;
using System.Collections;

public class Donkey : MonoBehaviour {

    public bool isEntered = false;

    public Vector2 barrelPosition;

    public bool doOnce = true;

    // Gravity variables
    private float gravity = 0f;
    private float gravityTimer = 0f;

    public float accelerationIncrement = 0f;

    public bool isTouchedScreen = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(isEntered && doOnce)
        {
        //    barrelPosition = GetComponent<Transform>().position;
            doOnce = false;
        }

        if(!isEntered)
        {
            doOnce = true;
        }

        DetermineIfDead();


        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                // TouchPhase.Began means a finger touched the screen
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isTouchedScreen = true;
                }
            }
            else
            {
                isTouchedScreen = false;
            }
        }
	}

    void FixedUpdate()
    {
     //   print("Vel: " + GetComponent<Rigidbody2D>().velocity);

        // Da Gravity
        Gravity();


        if(GameControl.control.GetIsEndOfLevel())
        {

            GetComponent<Transform>().position = Vector2.MoveTowards(
                GetComponent<Transform>().position,
                GameObject.FindGameObjectWithTag("End").GetComponent<Transform>().position, 7f * Time.fixedDeltaTime);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
    }

    /*
     * If the main character is not in a barrel then start a timer.  If the timer is greater
     * than the given value then turn on gravity.
     */
    void Gravity()
    {
        // If main character is not in a barrel
        if (!isEntered)
        {
            // Gravity timer
            gravityTimer = gravityTimer + Time.fixedDeltaTime;

            // If gravitytimer is greater than the number then turn gravity on
            if (gravityTimer > 0.35f)
            {
                gravity = 1.5f;

                if (GetComponent<Rigidbody2D>().velocity.x > 0)
                {

                    if (accelerationIncrement > 0)
                    {
                        accelerationIncrement = -accelerationIncrement;
                    }
                }
                else if (GetComponent<Rigidbody2D>().velocity.x < 0)
                {
                    if (accelerationIncrement < 0)
                    {
                        accelerationIncrement = -accelerationIncrement;
                    }

                }
                else
                {
                    accelerationIncrement = 0f;
                }

                GetComponent<Rigidbody2D>().velocity = new Vector2(
                    GetComponent<Rigidbody2D>().velocity.x + accelerationIncrement,
                    GetComponent<Rigidbody2D>().velocity.y);
            }

            // Set the gravity
            GetComponent<Rigidbody2D>().gravityScale = gravity;
        }
        // Main character is in a barrel
        else
        {
            // Gravity is 0
            gravity = 0f;

            // Gravity timer is 0
            gravityTimer = 0f;

            // Set gravity
            GetComponent<Rigidbody2D>().gravityScale = gravity;
        }
    }


    void DetermineIfDead()
    {
        if(transform.position.y < -3.5f)
        {
            GameControl.control.SetGameOver(true);
        }
    }

    public bool GetIsEntered()
    {
        return isEntered;
    }

    public void SetIsEntered(bool b)
    {
        isEntered = b;
    }


    public void SetAcclerationIncrement(float s)
    {
        accelerationIncrement = s;
    }

    public void SetBarrelPosition(Vector2 v)
    {
        barrelPosition = v;
    }

    public Vector2 GetBarrelPosition()
    {
        return barrelPosition;
    }

    public bool GetIsTouchedScreen()
    {
        return isTouchedScreen;
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        // If the other tag is Donkey
        if (other.tag == "End")
        {
            // Set off you beat the level by telling game control
            // Then have UI display stats and click to go onto next level
            // Maybe main char hits pile of nuts and sticks head out 
            print("OVER");
            GameControl.control.SetIsEndOfLevel(true);
        }

    }

}
