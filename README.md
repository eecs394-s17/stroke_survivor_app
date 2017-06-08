# Stroke Survivors

## Synopsis
Stroke Survivors is a mobile game app that helps patients who have experienced strokes recover through games that encourage exercise. It is built using Unity and Firebase.

## System Requirements

- Unity 5.6 (or higher)
- MacOS, Windows
- Xcode 7 or higher (only needed if deploying to iOS)
- Android SDK (only needed if deploying to Android, see setup instructions for Android Deployment)

## Sample of Code

There is no code for the UI (it is built using the Unity GUI); a portion of the script can be found here:
https://drive.google.com/file/d/0BxlX8gnBsVrcQkNIam9kRnZnOUE/view?usp=sharing


## Installation

* Clone the Repo in a directory of your choosing
```bash
$  git clone https://github.com/djrobertjin/stroke_app.git
```

## Quickstart

* Open Unity

* Click Open in the top right corner, and find and open the directory stroke_survivor_app

* Press the Play Button at the top of the screen

## Setup Instructions for Android Deployment

* Connect your Android device to your computer

* Go to Build Settings (File->Build Settings) 

* Select Android as the platform

* Press Build And Run

You will be asked to setup Android SDK. Download Android SDK as instructed, replace the sdk/tools folder with the following download:

Windows: https://dl.google.com/android/repository/tools_r25.2.3-windows.zip -

MacOSX: https://dl.google.com/android/repository/tools_r25.2.3-macosx.zip -

The directory of sdk/tools on mac is `/Users/chankyuoh/Library/Android/sdk/tools` , where instead of chankyuoh, it should be your user name. Note that the Library folder is hidden, to make it visible in Finder, please refer to the link below.

https://helpx.adobe.com/x-productkb/global/access-hidden-user-library-files.html

For Windows users, the sdk file is located at `C:\Program Files\Android\SDK`

If you do not have an Android directory in the Library/ProgramFiles folder, you may need to download Android Studio first.


## Setup Instructions for iOS Deployment

* NOTE: You MUST have a MacOS with xcode installed in order to deploy to iOS

* Connect your iOS device to your computer

* Go to Build Settings (File->Build Settings) 

* Select iOS as the platform

* Press Build And Run

* If you come across Firebase auth error, please refer to this stackoverflow [post](https://stackoverflow.com/questions/41896479/firebase-xcode-linker-command-error-using-firebase-unity-sdk "post"), and the answer by user Branko de Enero to resolve the issue.


## Editing the code in scenes of Unity:

Step 1) https://drive.google.com/file/d/0BxlX8gnBsVrcODk2WmZnLTZOMzQ/view?usp=sharing

Step 2) https://drive.google.com/file/d/0BxlX8gnBsVrcTHJROUl3UWZseWs/view?usp=sharing

To add items like buttons and input fields, after opening a scene (like MainMenu in step 1), right click in the hierarchy menu (the menu containing text like MainMenu SF Scene Elements, Directional Light), and click UI, and click the type of component you'd like to add. To add a script, go to project menu, in the scripts subfolder (to maintain good style), add a C# script, and edit it accordingly (you may look at other scripts for reference). To link this script to your UI, drag this script from the Project folder to the  scene's MainCamera (preferrable, others may work), which can be found in the Hierarchy menu (when the scene is open). If you referenced items like input fields, open the script in the Inspector, first click the reference in the script (like User Name Input), then click (in the hierarchy) the UI item you'd like to link the script reference to.
https://drive.google.com/file/d/0BxlX8gnBsVrcYzBXZ0l5OXhyb0k/view?usp=sharing



## Using Firebase
* Firebase has a Unity SDK and has very good documentation.
https://firebase.google.com/docs/database/unity/start

The basic Firebase installation is already done. 

For an example of how the API has been applied to use Firebase functions based on post-Firebase-login data, look below (note the imports and initializeFirebase() function in particular):

https://github.com/eecs394-s17/stroke_survivor_app/blob/master/Assets/scripts/GameManager.cs

* If you would like access to Stroke Survivor's firebase console, please email chen.lin1@northwestern.edu with the non-Northwestern google email you would like access for (Northwestern emails don't have access to Firebase)

## Debugging on mobile
https://www.assetstore.unity3d.com/en/#!/content/12047

* The above link is the debug tool that is used for mobile debugging; after installation (via the Unity Asset Store option within Unity, remember to set the "Script Execution Order" (in: "Edit", "Project Settings") of "InGameLog" to be the highest.

* After you deployed the app to your phone, draw a circle on the screen, and the debug console will appear, where all console messages will be logged

* If you have any trouble with this tool, please consult the tool's README



## Known bugs not fixed


* In the dashboard screen, users cannot scroll down to see more of the games played

* On Android devices, screen will timeout during the game, as gameplay doesn't require user to touch the phone.

## Platform Constraints

* Firebase for Unity may require different configurations for Android vs iOS. This may lead to errors where everything works fine on one platform, but does not work on another platform.  Firebase for Unity also does not support retrieving real user data when clicking the in-Unity play button (due to use of mock objects), requiring actual deployment to a device instead.

## Dependencies not handle by the app's package manager

* None, though worth noting the phone live debugger described in the "Debugging on Mobile" section, which makes development a lot easier

