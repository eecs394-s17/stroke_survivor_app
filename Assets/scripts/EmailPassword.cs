using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;



public class EmailPassword : MonoBehaviour
{

	public FirebaseAuth auth;
	public InputField UserNameInput, PasswordInput;
	public Button SignupButton, LoginButton;
	public Text ErrorText;
	public Firebase.Auth.FirebaseUser user;
	public string displayName = "";

	public void InitializeFirebase() {
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged(this, null);
	}

	public void AuthStateChanged(object sender, System.EventArgs eventArgs) {
		print("current user is " + auth.CurrentUser);
		if (auth.CurrentUser != user) {
			bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && user != null) {
				print("Signed out " + user.UserId);
			}
			user = auth.CurrentUser;
			if (signedIn) {
				print("Signed in " + user.UserId); // UserID is the email in Unity Player, uID on phone
				displayName = user.UserId;
				print ("display name is " + user.UserId);
			}
		}
	}
		
	void Start()
	{
		InitializeFirebase ();
		SignupButton.onClick.AddListener(() => Signup(UserNameInput.text, PasswordInput.text));
		LoginButton.onClick.AddListener(() => Login(UserNameInput.text, PasswordInput.text));
	}

	public void Signup(string email, string password)
	{
		if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
		{
			print ("bad request signup");
			return;
		}

		auth.CreateUserWithEmailAndPasswordAsync(UserNameInput.text, PasswordInput.text).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
				return;
			}
			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("Firebase user created successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);
		});
	}

	public void Login(string email, string password)
	{
		auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
			{
				print("in auth");
				if (task.IsCanceled)
				{
					Debug.LogError("SignInWithEmailAndPasswordAsync canceled.");
					return;
				}
				if (task.IsFaulted)
				{
					Debug.LogError("SignInWithEmailAndPasswordAsync error: " + task.Exception);
					if (task.Exception.InnerExceptions.Count > 0)
						UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);
					return;
				}
				FirebaseUser user = task.Result;
				Debug.LogFormat("User signed in successfully: {0} ({1})",
					user.DisplayName, user.UserId);
				PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknown");
				SceneManager.LoadScene("MainMenu");
			});
	}

	private void UpdateErrorMessage(string message)
	{
		ErrorText.text = message;
		Invoke("ClearErrorMessage", 3);
	}








}