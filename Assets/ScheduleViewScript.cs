using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScheduleViewScript : MonoBehaviour {

	public static int acct;
	public static string name;

	DatabaseReference dbRef, root;
	DataSnapshot snap;

	int idxClass, idxName;

	List<string>[] eachClass;

	public Text classDisp, nameDisp, numPersonDisp;

	// Use this for initialization
	void Start () {

		eachClass = new List<string>[8];

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://scheduletracker-ca24b.firebaseio.com/");
		root = dbRef = FirebaseDatabase.DefaultInstance.RootReference;

		dbRef = root.Child ("People");

		idxClass = idxName = 0;

		dbRef.GetValueAsync ().ContinueWith (task => {
			snap = task.Result;

			int cnt = int.Parse((snap.Child ("Cnt").Value).ToString());

			for (int idx = 0; idx < eachClass.Length; idx++) {
				eachClass [idx] = new List<string> ();
		
				string myClass = snap.Child(acct.ToString ()).Child(idx.ToString ()).Value.ToString();

				if (!myClass.Equals ("N/A")) {

					for (int peopleCnt = 1; peopleCnt <= cnt; peopleCnt++) {
						if (peopleCnt != acct) {
							if (snap.Child (peopleCnt.ToString ()).Child (idx.ToString ()).Value.ToString ().Equals (myClass)) {
								
								eachClass [idx].Add (snap.Child (peopleCnt.ToString ()).Child ("Name").Value.ToString ());
							}
						}
					}
				}
			}

			if (eachClass [0].Count != 0) {
				nameDisp.text = (eachClass [0]) [0];
				numPersonDisp.text = ("1/" + eachClass[0].Count);
			}

		});

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void next(bool name){
		if (name) {

			if (idxName < eachClass [idxClass].Count - 1) {
				idxName++;
				nameDisp.text = (eachClass [idxClass]) [idxName];
				numPersonDisp.text = ((idxName + 1).ToString () + "/" + eachClass [idxClass].Count);
			}

		} else {

			if (idxClass < eachClass.Length - 1) {
				idxClass++;
				classDisp.text = "Period " + idxClass;

				idxName = 0;

				if (eachClass [idxClass].Count > 0) {
					nameDisp.text = (eachClass [idxClass]) [idxName];
					numPersonDisp.text = ("1/" + eachClass [idxClass].Count);
				} else {
					nameDisp.text = "Nobody :(";
					numPersonDisp.text = "";
				}
			}
		}
	}

	public void prev (bool name){

		if (name) {

			if (idxName > 0) {
				idxName--;
				nameDisp.text = (eachClass [idxClass]) [idxName];
				numPersonDisp.text = ((idxName + 1).ToString () + "/" + eachClass [idxClass].Count);
			}

		} else {

			if (idxClass > 0) {
				idxClass--;

				if (idxClass == 0) {
					classDisp.text = "Zero Hour";
				} else {
					classDisp.text = "Period " + idxClass;
				}

				idxName = 0;

				if (eachClass [idxClass].Count > 0) {
					nameDisp.text = (eachClass [idxClass]) [idxName];
					numPersonDisp.text = ("1/" + eachClass [idxClass].Count);
				} else {
					nameDisp.text = "Nobody :(";
					numPersonDisp.text = "";
				}
			}

		}

	}

	public void signOut(){
		HomeScreenScript.acct = acct;
		HomeScreenScript.name = name;
		Debug.Log (name);
		SceneManager.LoadScene (3);
	}


}
