using UnityEngine;
using System.Collections;

public class OffsetScroller : MonoBehaviour {

    // Get the cart
    public GameObject mainCharacter;

    // User sets scroll speed
	public float scrollSpeed;

    // Stored offset
    private Vector2 savedOffset;

    private Vector3 velocity = Vector3.zero;

    private float p = 0;


    void Start () {
        // Store the offset
        savedOffset = GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
    }

    void FixedUpdate () {

    //    if (!mainCharacter.GetComponent<Donkey>().GetIsEntered())
      //  {
            // Set the position
        //    GetComponent<Transform>().position = new Vector3(mainCharacter.GetComponent<Transform>().position.x,
        //        GetComponent<Transform>().position.y, GetComponent<Transform>().position.z);




            
            float x = Mathf.Repeat(Time.fixedTime * scrollSpeed, 1);
      //      p = p + -(mainCharacter.GetComponent<Rigidbody2D>().velocity.x/scrollSpeed);

        //    float x = Mathf.Repeat(p , 1);
            Vector2 offset = new Vector2(x, savedOffset.y);
            GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
     //   }

        
    }

    void OnDisable () {
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }

}
