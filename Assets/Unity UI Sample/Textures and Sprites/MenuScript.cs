using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	// Load an assetbundle which contains scenes.
	// When the user clicks a button the first scene in the assetbundle is
	// loaded and replaces the current scene.


		private AssetBundle myLoadedAssetBundle;
		private string[] scenePaths;

		// Use this for initialization
		void Start()
		{
			myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
			scenePaths = myLoadedAssetBundle.GetAllScenePaths();
		}

		void OnGUI()
		{
			if (GUI.Button(new Rect(10, 10, 100, 30), "Change scene"))
			{
				Debug.Log("scene2 loading: " + scenePaths[0]);
				SceneManager.LoadScene(scenePaths[0], LoadSceneMode.Single);
			}

	}
}


