using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class DashboardManager : MonoBehaviour {
	public Text GameHistoryText;

	// Use this for initialization
	void Start () {

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://strokesurvivors-605a1.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		GameHistoryText.text = "\n\n";



		FirebaseDatabase.DefaultInstance
			.GetReference("games")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					// Handle the error...
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					// Do something with snapshot...
//					print(snapshot.Children);
//					print(snapshot.Child("-KkgCwU06xh0wsMT7q0Q").GetRawJsonValue());
					GameHistoryText.text += "\t\t\t\tHighscore:" + snapshot.Child("highscore").GetRawJsonValue() + " reps";
					GameHistoryText.text += "\n\n";
					GameHistoryText.text += "\t\t\t\tDate\t\t\t\t\t\t\t\t\tReps\n";
					long highScore = 0;
					foreach(DataSnapshot item in snapshot.Children)
					{
						// do something with entry.Value or entry.Key
//						print(item.Child("date").Value);
						string date = item.Child("date").Value.ToString();
						string repCount = item.Child("repCount").Value.ToString();
						if((long)item.Child("repCount").Value > (long)highScore) {
//							print("high score updated");
							highScore = (long)item.Child("repCount").Value;
//							print("high score now " + highScore);
//							GameHistoryText.text += "high score" + "\t\t\t" + highScore + "\n";
						}	


						GameHistoryText.text += date + "\t\t\t" + repCount + "\n";

					}
					print("got here");
					GameHistoryText.text += "here";

				}
			});
	}
	
	// Update is called once per frame
	void Update () {



	}
}
