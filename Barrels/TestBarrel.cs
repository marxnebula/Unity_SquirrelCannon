using UnityEngine;
using System.Collections;

public class TestBarrel : MonoBehaviour {

    public GameObject donkey;

    public float enterRotateTo = 0f;

    public float startRotatePoint = 0f;
    public float endRotatePoint = 0f;

    public float autoBarrelRotateTo = 0f;

    public float barrelStrength = 0f;

    public float movingSpeed = 0f;

    private float stepMovement = 0f;

    private float stepRotateWhenEnter = 0f;

    private float stepRotateBetweenPoints = 0f;

    public float rotate360Speed = 0f;

    public float rotateWhenEnterSpeed = 0f;

    public float rotateBetweenPointsSpeed = 0f;

    public int startingIndex = 0;

    public float delayTime = 0f;

    public bool isAutoBarrel = false;

    public bool is360Rotate = false;

    public bool isLerpRotate = false;

    public bool isLerpWhenEnter = false;

    public bool isMoving = false;

    public bool isDelayed = false;

    public bool isEntered = false;
    private Vector2 position1;
    private Vector2 position2;
    private Vector2 toPosition;
    private float gravity = 0f;
    private float gravityTimer = 0f;
    private float barrelDelayTimer = 0f;
    private float currentRotationTowards = 0f;


	// Use this for initialization
	void Start () {

        position1 = new Vector2(GetComponent<EdgeCollider2D>().points[0].x +
            GetComponent<EdgeCollider2D>().offset.x +
            transform.position.x,
            GetComponent<EdgeCollider2D>().points[0].y +
            GetComponent<EdgeCollider2D>().offset.y +
            transform.position.y);
        position2 = new Vector2(GetComponent<EdgeCollider2D>().points[1].x +
            GetComponent<EdgeCollider2D>().offset.x +
            transform.position.x,
            GetComponent<EdgeCollider2D>().points[1].y +
            GetComponent<EdgeCollider2D>().offset.y +
            transform.position.y);


        GetComponent<EdgeCollider2D>().enabled = false;


        if(startingIndex == 0)
        {
            toPosition = position1;
        }
        else
        {
            toPosition = position2;
        }



        currentRotationTowards = startRotatePoint;
	}
	
	// Update is called once per frame
	void Update () {

        AutoBarrel();

        Rotate();

        LerpMovement();

        MainCharacterInBarrel();

        Launch();

        Gravity();

	}


    void AutoBarrel()
    {
        if(isAutoBarrel)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(0f, 0f, autoBarrelRotateTo), Time.deltaTime * rotateWhenEnterSpeed);
        }
    }

    void MainCharacterInBarrel()
    {
        if (isEntered)
        {
            donkey.GetComponent<Transform>().position = transform.position;
        }
    }

    void LerpMovement()
    {

        if (isMoving)
        {


            stepMovement = movingSpeed * Time.deltaTime;


            if ((new Vector2(transform.position.x, transform.position.y) ==
                position1) ||
                new Vector2(transform.position.x, transform.position.y) ==
                (position2))
            {
                if (toPosition == position1)
                {
                    toPosition = position2;
                }
                else
                {
                    toPosition = position1;
                }


            }

            transform.position = Vector2.MoveTowards(transform.position,
                toPosition, stepMovement);
        }
    }


    void Rotate()
    {
        if (isEntered)
        {
            if (is360Rotate)
            {
                transform.Rotate(new Vector3(0f, 0f, -rotate360Speed) * Time.deltaTime);
            }


            if (isLerpRotate)
            {
                
                stepRotateBetweenPoints = Time.deltaTime * rotateBetweenPointsSpeed;

                if(transform.rotation.eulerAngles.z == currentRotationTowards)
                {
                    if(currentRotationTowards == startRotatePoint)
                    {
                        if(isDelayed)
                        {

                            barrelDelayTimer = barrelDelayTimer + Time.deltaTime;

                            if(barrelDelayTimer > delayTime)
                            {
                                currentRotationTowards = endRotatePoint;
                                barrelDelayTimer = 0f;
                            }
                        }
                        else
                        {
                            currentRotationTowards = endRotatePoint;
                        }
                        
                    }
                    else
                    {
                        if(isDelayed)
                        {
                            barrelDelayTimer = barrelDelayTimer + Time.deltaTime;

                            if (barrelDelayTimer > delayTime)
                            {
                                currentRotationTowards = startRotatePoint;
                                barrelDelayTimer = 0f;
                            }
                        }
                        else
                        {
                            currentRotationTowards = startRotatePoint;
                        }
                        
                    }
                }

                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(0f, 0f, currentRotationTowards), stepRotateBetweenPoints);
            }


            if (isLerpWhenEnter)
            {

                stepRotateWhenEnter = Time.deltaTime * rotateWhenEnterSpeed;

                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(0f, 0f, enterRotateTo), stepRotateWhenEnter);


                if (transform.rotation.eulerAngles.z == enterRotateTo)
                {
                    isLerpWhenEnter = false;
                }

            }
        }
    }


    void Launch()
    {
        if (Input.GetKey(KeyCode.Space) && isEntered)
        {
            donkey.GetComponent<SpriteRenderer>().enabled = true;
            donkey.GetComponent<Donkey>().SetIsEntered(false);
            isEntered = false;

            Vector2 v = new Vector2(-barrelStrength * Mathf.Sin(transform.rotation.eulerAngles.z * (Mathf.PI/180)),
                barrelStrength * Mathf.Cos(transform.rotation.eulerAngles.z * (Mathf.PI/180)));
            donkey.GetComponent<Rigidbody2D>().AddForce(v);

            print("Z rotation: " + transform.rotation.eulerAngles.z);
            print("sin: " + Mathf.Sin(transform.rotation.eulerAngles.z * (Mathf.PI/180)));
            print("cos: " + Mathf.Cos(transform.rotation.eulerAngles.z * (Mathf.PI/180)));
            print("Vector" + v);
        }
    }


    void Gravity()
    {
        if(!donkey.GetComponent<Donkey>().GetIsEntered())
        {
            gravityTimer = gravityTimer + Time.deltaTime;

            if(gravityTimer > 0.35f)
            {
                gravity = 1.5f;
            }

            donkey.GetComponent<Rigidbody2D>().gravityScale = gravity;
        }
        else
        {
            gravity = 0f;
            gravityTimer = 0f;
            donkey.GetComponent<Rigidbody2D>().gravityScale = gravity;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Donkey")
        {
            other.GetComponent<SpriteRenderer>().enabled = false;
            donkey.GetComponent<Donkey>().SetIsEntered(true);
            isEntered = true;
            other.GetComponent<Rigidbody2D>().gravityScale = 0f;
            isMoving = true;
            barrelDelayTimer = delayTime;

            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
    }



}
