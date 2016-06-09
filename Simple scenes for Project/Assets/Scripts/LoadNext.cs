using UnityEngine;
using System.Collections;

public class LoadNext : MonoBehaviour {
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			if (Application.loadedLevel != 2){
				Application.LoadLevel (Application.loadedLevel + 1);
			}
			else{
				Application.LoadLevel (0);
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();
	}
}
