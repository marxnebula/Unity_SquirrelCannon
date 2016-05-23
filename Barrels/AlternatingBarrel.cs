using UnityEngine;
using System.Collections;

public class AlternatingBarrel : Barrel {

    // Time for how long it holds position before rotating
    public float delayTime = 0f;

    // Speed of rotation
    public float rotateBetweenPointsSpeed = 0f;

    // Barrel starts at this position(barrel rotates back to this position)
    public float startRotatePoint = 0f;

    // Barrel rotates to this position
    public float endRotatePoint = 0f;

    // Stores timer
    public float barrelDelayTimer = 0f;

    // Step rotation
    private float stepRotateBetweenPoints = 0f;

    // Stores current rotate to position
    private float currentRotationTowards = 0f;


    void Start()
    {
        // Call base Start
        base.Start();
    }

	// Update is called once per frame
	void FixedUpdate () {

        // Update the base
        base.FixedUpdate();

        // Alternating rotation
        AlternatingRotation();
	}


    /*
     * Barrel rotates towards the endRotateTo.  It stays at that rotation equal to
     * the delay timer.  Then it rotates back to the starting rotation and stays there equal to
     * the delay timer.  This repeats as long as the main character is in the barrel.
     */
    void AlternatingRotation()
    {

        // If main character has entered the barrel
        if (isEntered)
        {
            // Step rotate
            stepRotateBetweenPoints = Time.fixedDeltaTime * rotateBetweenPointsSpeed;

            // If transform is at current rotation
            if (transform.rotation.eulerAngles.z == currentRotationTowards)
            {

                // If it is the starting rotation
                if (currentRotationTowards == startRotatePoint)
                {
                    // Barrel delay timer
                    barrelDelayTimer = barrelDelayTimer + Time.fixedDeltaTime;

                    // If barrelDelayTimer is greater than delayTime
                    if (barrelDelayTimer > delayTime)
                    {
                        // Set current rotation to endRotatePoint
                        currentRotationTowards = endRotatePoint;

                        // Set timer to 0
                        barrelDelayTimer = 0f;
                    }


                }
                // If it is the ending rotation
                else
                {
                    // Barrel delay timer
                    barrelDelayTimer = barrelDelayTimer + Time.fixedDeltaTime;

                    // If barrelDelayTimer is greater tha delayTime
                    if (barrelDelayTimer > delayTime)
                    {
                        // Set current rotation to startRotatePoint
                        currentRotationTowards = startRotatePoint;

                        // Set timer to 0
                        barrelDelayTimer = 0f;
                    }


                }
            }

            // Rotate towards the current rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.Euler(0f, 0f, currentRotationTowards), stepRotateBetweenPoints);
        }
    }


    /*
     * Resets the timer if main character enters the barrel
     */
    void OnTriggerEnter2D(Collider2D other)
    {
        // Call Base OnTriggerEnter2D
        base.OnTriggerEnter2D(other);

        // If other tag is Donkey
        if (other.tag == "Donkey")
        {
            // Set barrelDelayTimer to delayTime
            barrelDelayTimer = delayTime;
        }
    }


    }

