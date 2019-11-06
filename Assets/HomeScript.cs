using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class HomeScript : MonoBehaviour {

	DatabaseReference dbRef;
	DatabaseReference root;
	DataSnapshot snap;
	public InputField dispName;
	public InputField psskey;

	public Text altLbl;

	// Use this for initialization
	void Start () {
		

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://scheduletracker-ca24b.firebaseio.com/");
		dbRef = root = FirebaseDatabase.DefaultInstance.RootReference;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void login() {

		if (!(dispName.text.Equals ("") || psskey.text.Equals (""))) {

			root.Child ("People").GetValueAsync ().ContinueWith (task => {

				if(task.IsCompleted){
					int targetUser = -1;
					string targetName = "";
					snap = task.Result;
					int cnt = int.Parse((snap.Child ("Cnt").GetRawJsonValue()));
					bool login;
					login = false;
					for(int idx = 1; idx <= cnt; idx++){

						if(snap.Child(idx.ToString()).Child("Name").Value.Equals(dispName.text)){
							
							if(snap.Child(idx.ToString()).Child("Passkey").Value.Equals(psskey.text)){
								targetUser = idx;
								targetName = snap.Child(idx.ToString()).Child("Name").Value.ToString();
								login = true;
								break;
							} else { 
								altLbl.text = "Incorrect passkey!";
								return;
							}
						}
					}
						
					if(login){

						HomeScreenScript.acct = targetUser;
						HomeScreenScript.name = targetName;
						SceneManager.LoadScene(3);

					} else {
						altLbl.text = "User not found!";
					}
				}
			});

		} else {
			altLbl.text = "Not all fields have been completed!";
		}

	}
	public void createAcct(){
		SceneManager.LoadScene (1);
	}



}