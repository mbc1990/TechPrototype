using UnityEngine;
using System.Collections;

public class Easy_Entry : MonoBehaviour {

	void Start () {
		//increasing radial error makes it easier to enter a star
		Manager.RADIAL_ERROR += 5;
		Manager.TAN_ERROR += 3;
	}
	
	void Update () {
	
	}
}