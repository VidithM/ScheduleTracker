using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour {

	private string[] classes = new string[8];
	private int idx = 0; //Current period

	public Text periodDisp, altLbl;
	public Button nextClass, prevClass, submit;
	public InputField name, passkey, lastName;

	DatabaseReference root;
	DatabaseReference dbRef;

	DataSnapshot snap;

	// Use this for initialization
	void Start () {

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://scheduletracker-ca24b.firebaseio.com/");
		root = FirebaseDatabase.DefaultInstance.RootReference;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void next(){

		if (lastName.text != null) {
			classes [idx] = lastName.text;
		}
		if (idx < classes.Length - 1) {
			idx++;

			periodDisp.text = "Period " + idx;

			if (classes [idx] == null) {
				lastName.text = "";
			} else {
				lastName.text = classes [idx];
			}

		}
	}

	public void prev(){

		if (lastName.text != null) {
			classes [idx] = lastName.text;
		}
		if (idx > 0) {

			idx--;

			if(idx == 0){
				periodDisp.text = "Zero Hour";
			} else {
				periodDisp.text = "Period " + idx;
			}

			if (classes [idx] == null) {
				lastName.text = "";
			} else {
				lastName.text = classes [idx];
			}
				
		}


	}

	public void submitFunc(){
		
		if (lastName.text != null) {
			classes [idx] = lastName.text;
		}

		root.Child("People").GetValueAsync().ContinueWith(task => {
			
			 if (task.IsCompleted) {
				snap = task.Result;

				int cnt = int.Parse((snap.Child ("Cnt").GetRawJsonValue()));

				if (!(name.text.Trim().Equals ("") || passkey.text.Trim().Equals (""))) {

					foreach (string idx in classes) {
						if (idx == null || idx.Trim().Equals ("")) {
							altLbl.text = "Not all classes entered!";
							return;
						}
					}

					for (int user = 1; user <= cnt; user++) {
						if (snap.Child(user.ToString()).Child("Name").Value.ToString().Trim().Equals(name.text.Trim())) {
							altLbl.text = "A user with the same name already exists!";
							return;
						}
					}



					dbRef = root.Child("People").Child("Cnt");
					dbRef.SetValueAsync(cnt + 1);
					dbRef = root.Child("People").Child((cnt + 1).ToString());
					root = dbRef;

					dbRef = root.Child ("Name");
					dbRef.SetValueAsync(name.text);

					dbRef = root.Child ("Passkey");
					dbRef.SetValueAsync (passkey.text);

					dbRef = root;

					for (int idx = 0; idx < 8; idx++) {
						dbRef = dbRef.Child(idx.ToString());
						dbRef.SetValueAsync (classes [idx].ToUpper());
						dbRef = root;
					}

					/*root = FirebaseDatabase.DefaultInstance.RootReference;
					dbRef = root;
					*/

					ScheduleViewScript.acct = cnt + 1;
					ScheduleViewScript.name = name.text;
					SceneManager.LoadScene(2);

				} else {
					altLbl.text = "Not all fields have been completed!";

				}


				}
			});
			
	}

}
