using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;

public class EditScreenScript : MonoBehaviour {

	DatabaseReference dbRef, root;
	DataSnapshot snap;

	string[] classes;

	public static int acct;
	public static string name;

	public InputField nameIn, classIn;
	public Text periodDisp, alertLbl;
	int idx = 0;
	// Use this for initialization
	void Start () {

		classes = new string[8];

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://scheduletracker-ca24b.firebaseio.com/");
		root = dbRef = FirebaseDatabase.DefaultInstance.RootReference;
		dbRef = root.Child ("People").Child(acct.ToString());

		dbRef.GetValueAsync ().ContinueWith (task => {
			snap = task.Result;
	

			for (int idx = 0; idx < classes.Length; idx++) {
				classes[idx] = snap.Child(idx.ToString()).Value.ToString();
			}

			classIn.text = classes [0];
			nameIn.text = snap.Child ("Name").Value.ToString ();
		});
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void next(){

		if (idx < classes.Length - 1) {
			classes [idx] = classIn.text;
			idx++;
			periodDisp.text = "Period " + idx;
			classIn.text = classes [idx];
		}
	}

	public void prev(){

		if (idx > 0) {
			classes [idx] = classIn.text;
			idx--;
			classIn.text = classes [idx];

			if (idx == 0) {
				periodDisp.text = "Zero Hour";
			} else {
				periodDisp.text = "Period " + idx;
			}
		}
	}

	public void done(){
		
		classes [idx] = classIn.text;

		if (!(nameIn.text.Trim().Equals ("") || nameIn.text == null)) {

			dbRef = root.Child ("People");
			dbRef.GetValueAsync ().ContinueWith (task => {
				snap = task.Result;
			

				foreach (string index1 in classes) {
					if (index1.Trim().Equals ("") || index1 == null) {
						alertLbl.text = "Not all classes entered!";
						return;
					}

				}

					for (int check = 1; check <= int.Parse ((snap.Child ("Cnt").Value.ToString ())); check++) {
						if (check != acct) {
						if (snap.Child (check.ToString()).Child ("Name").Value.ToString().Trim().Equals (nameIn.text.Trim())) {
							alertLbl.text = "A user with the same name already exists!";
								return;
							}
						}
					}
					dbRef = dbRef.Child (acct.ToString ());

					dbRef.Child ("Name").SetValueAsync (nameIn.text);
					int cnt = 0;
					foreach (string index2 in classes) {
					dbRef.Child (cnt.ToString ()).SetValueAsync (index2.ToUpper());
						cnt++;
					}
					
				HomeScreenScript.acct = acct;
				HomeScreenScript.name = nameIn.text;
				SceneManager.LoadScene(3);

			});

		} else {
			alertLbl.text = "Not all fields have been completed";
		}

	}


}
