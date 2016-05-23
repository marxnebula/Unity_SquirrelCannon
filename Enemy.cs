using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {


    public bool isMoving = false;
    public Vector2[] positions;

    public float stepMovement = 0f;
    public float movingSpeed = 1f;

    public int currentIndex = 0;
    public int indexIncrement = 1;

    void Start()
    {
        StoreEdgeColliderPoints();
    }


    void FixedUpdate()
    {
        LerpMovement();
    }

    void StoreEdgeColliderPoints()
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

        // Disable it
        GetComponent<EdgeCollider2D>().enabled = false;
    }

    void LerpMovement()
    {
        if (isMoving)
        {
            // Step movement
            stepMovement = movingSpeed * Time.fixedDeltaTime;

            if ((new Vector2(transform.position.x, transform.position.y) == positions[currentIndex]))
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
            }

            // Move towards toPosition
            transform.position = Vector2.MoveTowards(transform.position,
                positions[currentIndex], stepMovement);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Donkey")
        {

          //  if (other == other.GetComponent<BoxCollider2D>())
          //  {
                // Play the audio
            //    GetComponent<AudioSource>().Play();

                // Kill main character
                GameControl.control.SetGameOver(true);

                print("Bee attack");
        //    }

        }
    }
}
