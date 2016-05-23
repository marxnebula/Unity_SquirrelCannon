using UnityEngine;
using System.Collections;

public class Nut : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Donkey")
        {
            //  if (other == other.GetComponent<BoxCollider2D>())
            //  {
            // Play the audio
            GetComponent<AudioSource>().Play();

            // Add the banana
            GameControl.control.AddOneNut();

            // Destroy the collider
            Destroy(GetComponent<BoxCollider2D>());

            // Unrender the coin
            GetComponent<SpriteRenderer>().enabled = false;

            // Destroy the coin after the sound has played
            Destroy(gameObject, GetComponent<AudioSource>().clip.length);
            //   }
        }
    }
}
