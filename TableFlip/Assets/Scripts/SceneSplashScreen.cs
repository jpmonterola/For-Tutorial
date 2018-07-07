using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSplashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void ToGameScreen(){
		SceneManager.LoadScene (1);
	}
}
