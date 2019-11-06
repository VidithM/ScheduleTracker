using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class HomeScreenScript : MonoBehaviour {

	public static int acct;
	public static string name;

	public Text welcomeTitle;
	// Use this for initialization
	void Start () {

		welcomeTitle.text = "HELLO, " + name.ToUpper();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void seeClasses(){
		ScheduleViewScript.acct = acct;
		ScheduleViewScript.name = name;
		SceneManager.LoadScene (2);
	}

	public void editProfile(){
		EditScreenScript.acct = acct;
		EditScreenScript.name = name;
		SceneManager.LoadScene (4);
	}

	public void signOut(){
		SceneManager.LoadScene (0);

	}
}
