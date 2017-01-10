ReadMe

--------------------------------------------------------------------
WEKITPrototypeHL
Current prototype for recording, saving and replaying sensor data on 
the Microsoft HoloLens platform
--------------------------------------------------------------------
General Usage:

The prototype can either be used as it is in the Unity Editor or be 
build, deployed and run for the Microsoft HoloLens (Emulator) and 
can record, save, load and replay basic movement and environment 
data for both platforms for now.

Using the playmode in the Unity editor will let the user use
the following commands via keybindings:

Using the HoloLens Emulator will grant access to the following
commands via tapping the cubes:


r - upper right cube:
start recording the data

t - upper left cube:
stop recording the data

s - lower left cube:
save all data from the temporary storage in a new local file

l - lower right cube:
load the local file's "test0" content into the temporary storage

w - far left cube:
wipe the recorded or opened data in the temporary storage

p - far right cube:
replay the data in the temporary storage from the beginning

no- very far right cube:
pause the replay or continue playing from the paused moment


Note: All commands will stop the recording process. There are very 
      basic replay functions (displaying the position and the gaze
      direction of the user using primitive shapes). Saving data 
      will always result in a new file being created. The files will 
      all be called "testX", where X is the automatically detected 
      file count and can usually be found under the path:

"C:\Users\{user}\AppData\LocalLow\DefaultCompany\WEKITPrototypeHL"
--------------------------------------------------------------------
Scripts:


CubeCommands.cs
- calls a function if the parent object is tapped in HoloLens

How to use:
- add the script to a mesh in the scene that should react to
  being tapped in the HoloLens
- type the name of the to be called function in the "Function
  Name" field in the Unity inspector
- pull a reference to the object with a "UIDisplayAPI" script
  into the "DataManager" field in the Unity inspector

DataPlayer.cs
- manages the displaying of the "ghost tracks" from recorded data

How to use:
- add the script to the mesh that you want to use to represent
  the previously recorded movement

GazeGestureManager.cs
- knows what object the user is gazing at, which gesture he is
  performing and what event should be triggered by that

How to use:
- add the script to any kind of management object in the scene
- additional gestures and events can be added to the script in 
  Visual Studio (see comments for more info)


UIDisplayAPI.cs
- collects, displays, saves, loads and manages different sensor
  data on the users command

How to use:
- add the script to any kind of managament object in the scene
- use the inspector to reference your UI text field and the
  ghost object to be used when replaying recorded data
- additional functions and data inputs can be implemented in the
  script in Visual Studio (see comments for more info)
- currently collects the following data - gaze direction, head
  position, hit info, timestamp
- stores data in a list of custom class instances "SaveData"


WorldCursor:
- 3D cursor that shows where the users gaze is currently hitting

How to use:
- pull the prefab "Cursor" from the folder "Prefabs" into the
  scene and add the script "WorldCursor" to the parent object
--------------------------------------------------------------------