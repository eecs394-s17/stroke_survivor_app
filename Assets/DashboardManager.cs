using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Auth;


public class DashboardManager : MonoBehaviour {
	public Text GameHistoryText;
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;

	void InitializeFirebase() {
		Debug.Log("Setting up Firebase Auth");
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged(this, null);
	}

	// Track state changes of the auth object.
	void AuthStateChanged(object sender, System.EventArgs eventArgs) {
		if (auth.CurrentUser != user) {
			bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && user != null) {
				Debug.Log("Signed out " + user.UserId);
			}
			user = auth.CurrentUser;
			if (signedIn) {
				Debug.Log("Signed in " + user.UserId);
			}
		}
	}
		
	// Use this for initialization
	void Start () {
		InitializeFirebase ();
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://strokesurvivors-605a1.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		GameHistoryText.text = "\n\n";
		FirebaseDatabase.DefaultInstance
			.GetReference("users")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					Debug.LogError("error while retreiving data");
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					setHighScore (snapshot);
					setHeader ();
					setAllGameData (snapshot);
				}
			});
	}

	void setHighScore (DataSnapshot snapshot)
	{
		GameHistoryText.text += "\t\t\t\tHighscore:" + snapshot.Child (user.UserId).Child ("highScore").Value.ToString ();
	}

	void setHeader ()
	{
		GameHistoryText.text += "\n\n";
		GameHistoryText.text += "\t\tDate\t\t\t\t\t\tReps\t\t\tScore\n";
	}

	void setAllGameData (DataSnapshot snapshot)
	{
		foreach (DataSnapshot game in snapshot.Child (user.UserId).Child ("games").Children) {
			string date = game.Child ("date").Value.ToString ();
			string repCount = game.Child ("repCount").Value.ToString ();
			string score = game.Child ("score").Value.ToString ();
			GameHistoryText.text += date + "\t\t\t   " + repCount + "\t\t\t" + score + "\n";
		}
	}
}
