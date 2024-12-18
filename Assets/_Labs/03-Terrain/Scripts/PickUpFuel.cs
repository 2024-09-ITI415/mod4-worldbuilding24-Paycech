using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpFuel : MonoBehaviour
{
    public Text countText;
	public Text planeText;
    // Start is called before the first frame update
    private int count;
    void Start ()
    {
        count = 0;

        SetCountText ();

        planeText.text = "";
    }

    void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			// Make the other game object (the pick up) inactive, to make it disappear
			other.gameObject.SetActive (false);

			// Add one to the score variable 'count'
			count = count + 1;

			// Run the 'SetCountText()' function (see below)
			SetCountText ();
		}
	}
    void SetCountText()
	{
		// Update the text field of our 'countText' variable
		countText.text = "Fuel: " + count.ToString () + "/4";

		// Check if our 'count' is equal to or exceeded 12
		if (count >= 4) 
		{
			// Set the text value of our 'winText'
			planeText.text = "You have all the fuel! Get back to the plane!";
		}
	}
}
