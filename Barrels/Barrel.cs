using UnityEngine;
using System.Collections;

public class Barrel : MonoBehaviour {

    // Main Character
    public GameObject mainCharacter;

    // Strength of force of barrel
    public float barrelStrength = 0f;

    // Determines where barrel initially moves towards
    public int startingPositionIndex = 0;

    // Barrels moving speed
    public float movingSpeed = 0f;

    // Barrels rotating speed
    public float rotateWhenEnterSpeed = 0f;

    // Barrel rotates to this when main character has entered
    public float enterRotateTo = 0f;

    // Timer for how long barrel waits before moving
    public float movingDelayTime = 0f;

    // Main character has entered barrel
    public bool isEntered = false;

    // Barrel is moving
    public bool isMoving = false;
    public bool isMovingAlways = false;

    // Should barrel rotate somewhere if main character enters it
    public bool isLerpWhenEnter = false;
    private bool beginRotate = false;

    // Increment for moveSpeed
    public float increment = 0.01f;
    public bool doOnce = true;

    // Vectors for where the barrel moves towards
    private Vector2 position1;
    private Vector2 position2;
    private Vector2 toPosition;
    public Vector2 middleVector;
    private Vector2 directionVector;

    public bool isHorizontal = false;
    
    // Step variables
    private float stepMovement = 0f;
    private float stepRotateWhenEnter = 0f;


    // Timer
    private float barrelMoveTimer = 0f;

    // Store moving speed
    private float storedMovingSpeed = 0f;

    // Boolean for when you can launch
    public bool isAbleToLaunch = false;



    public Vector2[] positions;
    public int currentIndex = 0;
    public int indexIncrement = 1;
    public float stepBarrel = 0f;
    public float accelerationIncrement = 0.007f;


	// Use this for initialization
	public void Start () {

        // Find the main character
        if(mainCharacter == null)
        {
            mainCharacter = GameObject.FindGameObjectWithTag("Donkey");
        }

        // Store the positions the barrel moves to
        StoreEdgeColliderPositions();


        // Choose the position the barrel initially moves towards
        if (startingPositionIndex == 0)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex = GetComponent<EdgeCollider2D>().pointCount - 1;
        }

        // Store moving speed
        storedMovingSpeed = movingSpeed;



        
	}

    
	
	// Update is called once per frame
	public void FixedUpdate () {

        // Rotation logic
        Rotate();

        // Movement of barrel with stored vector positions
        LerpMovement();

        // Main character follows barrel if inside of it
        MainCharacterInBarrel();

        // Launch the main character from barrel
        Launch();

        // Gravity of main character
    //    Gravity();

	}




    /*
     * Stores the two points from the edge collider.  These points are the vectors the barrel
     * lerps towards.  Disable the edge collider after storing the points.
     */
    void StoreEdgeColliderPositions()
    {
        

        positions = new Vector2[GetComponent<EdgeCollider2D>().pointCount];

        for (int i = 0; i < GetComponent<EdgeCollider2D>().pointCount; i++)
        {
            positions[i] = new Vector2(GetComponent<EdgeCollider2D>().points[i].x +
            GetComponent<EdgeCollider2D>().offset.x +
            transform.position.x,
            GetComponent<EdgeCollider2D>().points[i].y +
            GetComponent<EdgeCollider2D>().offset.y +
            transform.position.y);
        }

        middleVector = (positions[0] + positions[GetComponent<EdgeCollider2D>().pointCount - 1]) / 2;

        directionVector = positions[0] - positions[GetComponent<EdgeCollider2D>().pointCount - 1];

        // Disable it
        GetComponent<EdgeCollider2D>().enabled = false;

        if (Mathf.Abs(directionVector.x) > 0.3f)
        {
            isHorizontal = true;
        }

    }


    /*
     * If the main charater has entered the barrel then follow the barrels position.
     */
    void MainCharacterInBarrel()
    {
        // If main character has entered the barrel
        if (isEntered)
        {
            // Main character follows the barrels position
         //   mainCharacter.GetComponent<Transform>().position = transform.position;

            stepBarrel = 8f * Time.fixedDeltaTime;

            mainCharacter.GetComponent<Transform>().position = Vector2.MoveTowards(
                mainCharacter.GetComponent<Transform>().position,
                transform.position, stepBarrel);

            if (mainCharacter.GetComponent<Transform>().position == transform.position)
            {
                mainCharacter.GetComponent<SpriteRenderer>().enabled = false;
                beginRotate = true;
            }


            if(isHorizontal)
            {
                print("Barrel Pos");
                mainCharacter.GetComponent<Donkey>().SetBarrelPosition(positions[0]);
            }
            else
            {
                print("Char Pos");
                mainCharacter.GetComponent<Donkey>().SetBarrelPosition(mainCharacter.GetComponent<Transform>().position);
            }
            
        }
    }


    /*
     * If isMoving is true then the barrel moves between the two stored vectors.
     * isMoving can be set to true through the unity display and is set true when
     * main character enters the barrel.
     */
    void LerpMovement()
    {

        

        // If barrel is moving
        if ((isMoving && beginRotate) || isMovingAlways)
        {

            // Accelerates and decelerates the barrels speed
            IncrementSpeed();


            // Step movement
            stepMovement = movingSpeed * Time.fixedDeltaTime;

            // If barrel position equals postion1 or position2
            if ((new Vector2(transform.position.x, transform.position.y) == positions[currentIndex]))
            {

                // Start the timer
                barrelMoveTimer = barrelMoveTimer + Time.fixedDeltaTime;

                // If timer is greater than the users moving delay time
                if (barrelMoveTimer > movingDelayTime)
                {

                    if (currentIndex == GetComponent<EdgeCollider2D>().pointCount - 1)
                    {
                        // minus
                        indexIncrement = -1;
                    }
                    else if (currentIndex == 0)
                    {
                        // add
                        indexIncrement = 1;
                    }


                    currentIndex = currentIndex + indexIncrement;

                    // Set timer to 0
                    barrelMoveTimer = 0f;

                    increment = Mathf.Abs(increment);
                }




            }


            

            // Move towards toPosition
            transform.position = Vector2.MoveTowards(transform.position,
                positions[currentIndex], stepMovement);
        }
    }


    /*
     * Once the barrel gets close enough to the toPosition vector then slow it down.
     * Once the barrels position equals toPosition then speed it up again.
     * Don't allow the movingSpeed to exceed its initial value.
     */
    void IncrementSpeed()
    {
        // 0.2f
        if (((Mathf.Abs(transform.position.x - positions[0].x) < 0.58f) &&
            (Mathf.Abs(transform.position.y - positions[0].y) < 0.58f) &&
            doOnce) ||
            ((Mathf.Abs(transform.position.x - positions[positions.Length - 1].x) < 0.58f) &&
            (Mathf.Abs(transform.position.y - positions[positions.Length - 1].y) < 0.58f) &&
            doOnce))
        {
            // Make increment negative of the increment
            increment = -increment;

            // Set boolean to false
            doOnce = false;
        }



        // Decrease moving speed
        movingSpeed = movingSpeed + increment;
        

        // Make sure the speed doesn't exceed the movingSpeed
        if (movingSpeed > storedMovingSpeed)
        {
            movingSpeed = storedMovingSpeed;
        }
        else if(movingSpeed < 0.3f)
        {
            movingSpeed = 0.3f;
        }

        
        

        // Middle vector is the middle point between the two edge collider points
        // This means it is the half way point of the barrels route
        // It doesn't work if you try doing transform.position = middleVector so
        // I used absolute value and subtracted them and checked to see if  <= to small number
        Vector2 tempVect = new Vector2(transform.position.x, transform.position.y) - middleVector;
        if(Mathf.Abs(tempVect.x) <= 0.1f &&
            Mathf.Abs(tempVect.y) <= 0.1f)
        {
            // Set boolean to true
            doOnce = true;
        }

        if(positions.Length > 2 && 
            (new Vector2(transform.position.x, transform.position.y) == positions[(int)(positions.Length - 1)/2]))
        {
            doOnce = true;
        }
        
    }


    /*
     * When the main character enters the barrel, the barrel rotates to the variable
     * enterRotateTo.  Once completed it stops rotating.
     */
    void Rotate()
    {
        // If main character entered barrel
        if (isEntered)
        {
            // If you want it to lerp when main character enters
            if (isLerpWhenEnter && beginRotate)
            {
                // Step rotate
                stepRotateWhenEnter = Time.fixedDeltaTime * rotateWhenEnterSpeed;

                // Rotate the barrel to enterRotateTo
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(0f, 0f, enterRotateTo), stepRotateWhenEnter);

                // Once it has rotated to enterRotateTo then stop rotating it
                if (transform.rotation.eulerAngles.z == enterRotateTo)
                {
                    isLerpWhenEnter = false;
                    isAbleToLaunch = true;
                }

            }
        }
    }


    /*
     * If the main character is inside the barrel then the user can press the space bar 
     * which launches the main character in the direction of the barrels rotation.
     */
    void Launch()
    {
        // If main character is inside barrel and user presses space
        if ((Input.GetKey(KeyCode.Space) || mainCharacter.GetComponent<Donkey>().GetIsTouchedScreen()) &&
            isEntered && isAbleToLaunch && 
            !GameControl.control.GetGameOver())
        {
            // Turn main character sprite renderer on
            mainCharacter.GetComponent<SpriteRenderer>().enabled = true;

            // Set that is has left barrel
            mainCharacter.GetComponent<Donkey>().SetIsEntered(false);
            isEntered = false;

            // Vector for adding force
            Vector2 v = new Vector2(-barrelStrength * Mathf.Sin(transform.rotation.eulerAngles.z * (Mathf.PI / 180)),
                barrelStrength * Mathf.Cos(transform.rotation.eulerAngles.z * (Mathf.PI / 180)));

            // Add the force to the main character
            mainCharacter.GetComponent<Rigidbody2D>().AddForce(v);


            // DEBUG
            print("Z rotation: " + transform.rotation.eulerAngles.z);
            print("sin: " + Mathf.Sin(transform.rotation.eulerAngles.z * (Mathf.PI / 180)));
            print("cos: " + Mathf.Cos(transform.rotation.eulerAngles.z * (Mathf.PI / 180)));
            print("Vector" + v);
        }
    }


    


    /*
     * If the main character enters the barrel then turn off the main characters renderer,
     * turn off gravity, set the main characters velocity to 0, and set isEntered and isMoving bools
     * to true.
     */
    public void OnTriggerEnter2D(Collider2D other)
    {
        // If the other tag is Donkey
        if (other.tag == "Donkey")
        {
            // Set the sprite renderer to false
        //    other.GetComponent<SpriteRenderer>().enabled = false;

            // Set main character has entered barrel
            mainCharacter.GetComponent<Donkey>().SetIsEntered(true);
            isEntered = true;

            // Set gravity to 0
            other.GetComponent<Rigidbody2D>().gravityScale = 0f;

            // Set isMoving to true
            isMoving = true;

            // Turn off velocity
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

            // Set the acceleration
            mainCharacter.GetComponent<Donkey>().SetAcclerationIncrement(accelerationIncrement);
            
        }
    }


    public Vector2 GetPosition1()
    {
        return position1;
    }
}
