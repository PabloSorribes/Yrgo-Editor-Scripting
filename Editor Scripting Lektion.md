# 06 - Editor Scripting

[<img src="https://media1.tenor.com/images/757737201b0c4c0a46bb42bc333a64cb/tenor.gif" alt="01 - Programming Patterns" width="100%">](https://en.wikipedia.org/wiki/GoldenEye)

Lesson by Pablo Sorribes Bernhard - GP17 - [Link to PowerPoint Presentation](https://drive.google.com/file/d/1oJJfbenNUYuvIdMXONzascSeA3PWbY-e/view?usp=sharing)

> **Note:** *Part of this lesson is still waiting for english translation. Text is Swedish are marked with cursive for now.*

## Table of Contents
- [06 - Editor Scripting](#06---editor-scripting)
  - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
    - [Awesome editor scripting tools](#awesome-editor-scripting-tools)
  - [Basics](#basics)
    - [“Editor”-folder namespace](#editor-folder-namespace)
    - [Menu Items](#menu-items)
      - [Menu Index Position / Priority](#menu-index-position--priority)
  - [Custom Editors vs Editor Windows](#custom-editors-vs-editor-windows)
        - [Custom Editors](#custom-editors)
        - [Editor Windows](#editor-windows)
    - [How to: Make your own Custom Inspector (using a Custom Editor)](#how-to-make-your-own-custom-inspector-using-a-custom-editor)
      - [Prerequisites](#prerequisites)
      - [Basic Editor Example](#basic-editor-example)
    - [How to: Make an Editor Window](#how-to-make-an-editor-window)
      - [Prerequisites](#prerequisites-1)
      - [Editor Window Example](#editor-window-example)
  - [Basics: Layout Classes & Labels](#basics-layout-classes--labels)
    - [Layouting](#layouting)
    - [Labels](#labels)
      - [Example: Create a label for a string variable](#example-create-a-label-for-a-string-variable)
  - [Custom Editors: `target` vs Serialized Object](#custom-editors-target-vs-serialized-object)
    - [The `target` variable (aka the “old” way)](#the-target-variable-aka-the-old-way)
    - [Serialized Object (aka the “new” way)](#serialized-object-aka-the-new-way)
      - [Serialized Property](#serialized-property)
  - [Undo - The different ways of undoing something](#undo---the-different-ways-of-undoing-something)
    - [“Normal” Undo](#normal-undo)
    - [Undo Single Object Instantiation](#undo-single-object-instantiation)
    - [Object Deletion with Undo](#object-deletion-with-undo)
    - [BeginChangeCheck() & EndChangeCheck()](#beginchangecheck--endchangecheck)
    - [EditorUtility.SetDirty](#editorutilitysetdirty)
  - [UI Elements - The new UI System](#ui-elements---the-new-ui-system)
- [Nice Classes, Functions and Code Snippets](#nice-classes-functions-and-code-snippets)
  - [SetRandomRotationForObjects - Editor Window](#setrandomrotationforobjects---editor-window)
  - [Selection - How to set and get the Selected Objects](#selection---how-to-set-and-get-the-selected-objects)
  - [Layout tips](#layout-tips)
    - [Layout tips: Make a clickable Button](#layout-tips-make-a-clickable-button)
    - [Layout tips: Nice boxes as sectioning elements](#layout-tips-nice-boxes-as-sectioning-elements)
    - [Layout tips: Format your inspector’s text and styles](#layout-tips-format-your-inspectors-text-and-styles)
    - [Layout tips: Make labels easily using GUIContent](#layout-tips-make-labels-easily-using-guicontent)
  - [EditorUtility.DisplayDialog()](#editorutilitydisplaydialog)
  - [Draw default inspector, but draw some of the elements by yourself](#draw-default-inspector-but-draw-some-of-the-elements-by-yourself)
  - [Serialized Classes: Use custom classes as variables in other classes](#serialized-classes-use-custom-classes-as-variables-in-other-classes)
  - [Serialized Classes on Steroids: Custom Property Drawers](#serialized-classes-on-steroids-custom-property-drawers)
  - [[CanEditMultipleObjects]](#caneditmultipleobjects)
  - [Gizmos](#gizmos)
- [Resources, AssetDatabase & FileUtil](#resources-assetdatabase--fileutil)
  - [AssetPreview.GetAssetPreview(asset: myAsset)](#assetpreviewgetassetpreviewasset-myasset)
- [Misc Editor Code](#misc-editor-code)
  - [Capture Keyboard Input in Editor Windows (ie. make shortcuts)](#capture-keyboard-input-in-editor-windows-ie-make-shortcuts)
  - [UnityEditorInternal.ReorderableList](#unityeditorinternalreorderablelist)
  - [Translate Screen Mouse Coords to World Coords](#translate-screen-mouse-coords-to-world-coords)
  - [Saving settings persistently for Editor Scripts](#saving-settings-persistently-for-editor-scripts)
  - [“typeof()” and “nameof()”](#typeof-and-nameof)
      - [typeof()](#typeof)
      - [nameof()](#nameof)
- [Resources from the Lesson](#resources-from-the-lesson)
  - [Example Scripts & GIFs used during the Lesson](#example-scripts--gifs-used-during-the-lesson)
  - [General links for Editor Scripting](#general-links-for-editor-scripting)
- [Paalo Bonus Goodies](#paalo-bonus-goodies)
  - [EditorGUIHelper](#editorguihelper)
  - [GameObjectExtensions](#gameobjectextensions)
  - [StringExtensions](#stringextensions)
  - [HelperMethods](#helpermethods)
  - [Editor Scene Hierarchy Traversal - Scene Traversal API](#editor-scene-hierarchy-traversal---scene-traversal-api)
  - [SerializedPropertyExtensions](#serializedpropertyextensions)
- [UI Elements](#ui-elements)

---

## Introduction
Editor Scripting is all about extending and improving the editor to make it more useful stuff. Some examples include:

- Adding buttons that calls functions for automated processes.
- Customization for UI and how your components look.
- Tools for assets preperation.
- Improved workflow for designers and artists.
- Debug-tools for the Scene View.
- Basically anything.

*Så oftast används det för att optimera och automatisera vissa arbetsprocesser, och på så vis kan man undvika att mänskliga fel/faktorer råkar sabba en del utav, exempelvis, processen man har för att importera 3D-modeller (eller whatevs) i Unity.*

*Om man automatiserar allting med editor scripts så sparar man tid down the line, och saker blir mer konsistenta i ditt projekt, vilket gör det lättare att navigera, osv*

Here are a few examples of things that can be made with editor scripting:

### Awesome editor scripting tools

**Extreme Examples:**
- [Odin Inspector](https://odininspector.com/) \
[[Picture]](https://drive.google.com/open?id=1UyFQCFSF22Mm_hyJdMXGisaUjFtTj5FC)
- Shader Graph & Amplify Shader \
[[Picture]](https://drive.google.com/open?id=1YSXre9FTyDxB8Z-v3Nkoc7qkU4W6augb)
- All of Unity \
[[Picture]](https://drive.google.com/open?id=1nHaxr3jEiXYzykmSmzT5VS2u_CXhkck0)

**More normal examples:**
- [MinMaxSlider](https://drive.google.com/open?id=1LmeGO2PpT71FycrXLh0LJ3R4k2RKM5XZ) - Set up Ranges of values that designers can use  
- [Show/Hide options depending on enum](https://drive.google.com/open?id=1JWNy-Fa7fuV3Tu_Id8e2IwfNH8b1a2sg) - Different options depending on values in the inspector  
- [LevelCollection](https://drive.google.com/open?id=1vaZ1hbofzJRhdHd4JNuqNJBc1MhuqZS7) - Set up levels/presets in lists with options, which runtime code can read values from
- [MulitCube_SelectCubeType](https://drive.google.com/open?id=1v-eWas-hdO7JLjE3mu56nlEquKTuQVrP) - Switch objects in the scene based on enum  
- Utility functions:
    - [Bulk Rename Utility](https://drive.google.com/open?id=1aaFQdrZ8U3g3Xoz0GnpMwYqQiHQEOgNh) - Rename a bunch of objects at the same time  
    - Add functions to the hierarchy which Unity doesn't support out of the bog
        - [Create Parent for Selected Objects](https://drive.google.com/open?id=1qcur2YXA-6viADt45dByves564AzrUWC)
        - [Move Selection to New Parent with New Relative Position (Special Paste)](https://drive.google.com/open?id=1T57WTk1w8pobEp-fHLFnV0oVYlp0k8a9)

---

## Basics
Ok, let’s go over the basics. What’s needed for writing Editor Code?

### “Editor”-folder namespace
- *Alla editor script måste ligga i en mapp som heter “Editor”. Annars kommer din build att misslyckas när du ska bygga ut ditt spel.*
- Platform dependent compilation
  - *Använd dessa statements för att ha Editor-kod i dina Game-scripts, men som bara kompileras om du är i Editorn.*

```C#
#if UNITY_EDITOR
    //Do editor related stuff
#else
    //Do game related stuff instead (or nothing)
#endif
    //Finalize the if-def statement.
```

### Menu Items
Commands for calling on your static methods, eg. a method used for calling/instantiating your Editor Window. It is also used for the right-click menus both in the Hierarchy `GameObject/YourTool` and in Assets `Assets/YourTool`.

#### Menu Index Position / Priority
- Where your button/option will be displayed based on its index.
- How Unity's index system works: [Guide to Extending Unity Editor’s Menus](https://blog.redbluegames.com/guide-to-extending-unity-editors-menus-b2de47a746db)
- If you want a command in the right click menu, it should go in `GameObject/YourCompanyFolder/YourToolName`

```C#
[MenuItem(itemName: “GameObject/MyRightClickOption”, priority = -100, validate = false)]
private static void FunctionCalledByMenuItem()
{
    //Do stuff here, like call your Editor Window instance, or execute another method, etc.
}
```

---

## Custom Editors vs Editor Windows

##### Custom Editors
- Modifies one component.
- Mostly used to draw your own Custom Inspector.

##### Editor Windows
- Are “stand-alone”, in that they don’t need an extra script to work.
- Generally easier to set up.


### How to: Make your own Custom Inspector (using a Custom Editor)

#### Prerequisites
- 2 scripts
  - `Monobehaviour` (the component you want to draw)
    - Should **NOT** be inside an Editor-folder
  - `Editor` (this is where you write your custom editor)
    - **HAS** to lie inside an Editor-folder
- Inherit from `Editor`
- The tag `[CustomInspector(typeof(YourMonobehaviourClassToInspect))]`
- Write GUI-code in `public override void OnInpectorGUI()`


#### Basic Editor Example
Monobehaviour script to Edit:

```C#
using UnityEngine;

public class BasicBehaviour : MonoBehaviour
{
    //Class can be empty, but should preferably have some
    //variables which you can modify with your custom inspector
}
```

Editor Script:
```C#
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BasicBehaviour))]
public class BasicCustomInspector : Editor
{
	public override void OnInspectorGUI()
	{
		//Draw default Unity-inspector
		base.OnInspectorGUI();

		//Get a reference to your Monobehaviour
		BasicBehaviour script = (BasicBehaviour)target;

		//Draw some buttons
		if (GUILayout.Button("Do something"))
		{
		}

		if (GUILayout.Button("Do another thing"))
		{
		}
	}
}
```

### How to: Make an Editor Window
#### Prerequisites
- 1 script
  - Lies in an `Editor`-folder.
- Inherit from `EditorWindow`
- Has to be instantiated, eg. through a static function, which in turn could be called through a MenuItem.
- Write GUI-code in `private void OnGUI()`


#### Editor Window Example

Editor Window Script:
```C#
using UnityEditor;
using UnityEngine;

public class EditorWindowExample_LiveCoding : EditorWindow
{
	private const string menuItemPath = "YRGO/My Editor Window Example_LiveCoding";
	private static readonly string myWindowTitle = "My Editor Window Example_LiveCoding";
	public string myString = "My awesome string";
	[MenuItem(menuItemPath)]
	public static void SetupWindow()
	{
		//Instantiate the window and set its size.
		var window = GetWindow<EditorWindowExample_LiveCoding>(utility: false, title: myWindowTitle, focus: true);

		window.minSize = new Vector2(400, 175);
		window.maxSize = new Vector2(window.minSize.x + 10, window.minSize.y + 10);
	}

	private void OnGUI()
	{
		//Draw your custom stuff here
		//Draw some buttons
		if (GUILayout.Button("Do something"))
		{
		}

		if (GUILayout.Button("Do another thing"))
		{
		}
	}
```

---

## Basics: Layout Classes & Labels
### Layouting

There are two main ways of drawing the layout for your Custom Editor Window or Custom Inspector:
*   **Auto-layout** - Everything will be neat from the get-go.
*   Classes:
    *   `GUILayout`
    *   `EditorGUILayout`
*   **Manual Layout** - A lot of manual work in positioning your items correctly.
*   Classes:
    *   `GUI`
    *   `EditorGUI`

Other helper classes include:
*   `EditorUtility`
*   `EditorGUIUtility`
*   `GUIContent`
*   `EditorStyles`


### Labels
A term which is used a lot in Editor Scripting is “label”. So what is a label?

Well it’s basically the variable name you’d get from the “normal” Unity Inspector. Only in Editor Scripting you can define the name of the variable to be whatever you want, as well as easily add a tooltip to it.

#### Example: Create a label for a string variable
```C#
//Put this code into an Editor Window to see how it looks

private void OnGUI()
{
    // Create a label using GUIContent
    GUIContent myStringLabel = new GUIContent(text: "My String: ", tooltip: "What's your string all about?");

    //Pass in your label into your Field-layout method
    myString = EditorGUILayout.TextField(label: myStringLabel, text: myString);
}
```

Result:
[https://drive.google.com/open?id=1-Ge8Ipw7z6G9JJfb0uZlXVkKmmKkmTMp](https://drive.google.com/open?id=1-Ge8Ipw7z6G9JJfb0uZlXVkKmmKkmTMp)

---

## Custom Editors: `target` vs Serialized Object
### The `target` variable (aka the “old” way)
There’s a built-in variable called “target” when you’re writing a Custom Editor.

*   It is a reference to the “raw” object of the component which you are inspecting.
*   Thus, it has to be cast to your component type, ie. your Class.
*   Also, Undo must be handled manually

```C#
public override void OnInspectorGUI()
{
    BasicBehaviour script = (BasicBehaviour)target;
}
```

### Serialized Object (aka the “new” way)

There is a newer system for handling undo/redo which is called “Serialized Object”. It is recommended to use this instead of “target”, since this will handle undo/redo of your variable values automatically for you.

```C#
public override void OnInspectorGUI()
{
    //Updates the object's state/view/representation based on
    //the changes from the last frame.
    serializedObject.Update();

    //Optional - draw the default inspector
    base.OnInspectorGUI();

    //Do your editor stuff here

    //Apply the occured changes with this line (allow automatic undo/redo)
    serializedObject.ApplyModifiedProperties();
}
```

#### Serialized Property
*   All serialized variables on your script are “properties”
    *   **Note:** this is NOT the same as C#-properties with the “get” and “set”-functions.
*   Use `serializedObject.FindProperty(“variableName”)` to get a reference to your variable.
*   Handles Undo automatically.

**Tip:**

Use the `nameof()`-keyword for finding your variable’s name. It makes a string from the **name** of the variable, ie. it doesn’t use the value of the variable, but uses the actual **name** of the variable.

This is useful since the reference to your variable will always be updated if you would rename the variable.

If you would hard-code a string in the `FindProperty()`-call for your variable name you would have to manually change the string each time you’d rename the variable. If the searched property doesn’t exist, (ie. Unity didn’t find it cause you named the variable differently than what you’re calling it in the `FindProperty()`-call) you will get a ton of errors.

Using `nameof()` prevents that from ever happening.

```C#
//Assuming that “BasicBehaviour.myVariableProperty” is a public string.
//Get the variable by name, make a field for it and print its value and display name.

var propertyInstance = serializedObject.FindProperty(nameof(BasicBehaviour.myVariableProperty));
EditorGUILayout.PropertyField(propertyInstance);
Debug.Log(propertyInstance.stringValue);
Debug.Log(propertyInstance.displayName);
```

---

## Undo - The different ways of undoing something
### “Normal” Undo
//TODO @Paalo: Innan man gör en ändring på ett/flera objekt så måste man spara undan dem i en variabel:

```C#
var currentObject = Selection.activeObject;
Undo.RecordObject(currentObject, "Name of my operation after this Undo-block");
currentObject.SetActive(false);
```

### Undo Single Object Instantiation

Use `Undo.IncrementCurrentGroup()` when you want to manually separate several undos into separate undo steps.
Use `Undo.IncrementCurrentGroup()` when you want to manually separate several undos into separate undo steps.

```C#
Undo.IncrementCurrentGroup();
obj = (GameObject)EditorUtility.InstantiatePrefab(prefab);
Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
```

### Object Deletion with Undo

```C#
foreach (GameObject obj in Selection.gameObjects)
{
    Undo.DestroyObjectImmediate(obj);
}
```

### BeginChangeCheck() & EndChangeCheck()

`BeginChangeCheck()` is a useful way to see if an inspector variable was changed between `BeginChangeCheck()` and `EndChangeCheck()`. Avoids you having to store each value manually and sums up if any of the variables between the two Check-statements has changed.

Example:
```C#
//Start checking for changes on the following values,
//until it encounters an EndChangeCheck()
EditorGUI.BeginChangeCheck();
var myText = GUILayout.TextField(“myTextField”, GUILayout.Width(120));
var myPos = GUILayout.Vector3Field(“myVectorField”, Vector3.zero);

// If a variable was modified, EndChangeCheck() returns true, so we can do necessary behaviour, like storing the changes.
if (EditorGUI.EndChangeCheck())
{
	//Store state of object with Undo.RecordObject()
	//Modify values of your variables, etc
}
```

### EditorUtility.SetDirty
> **Basically obsolete**

Use `SerializedObject.FindProperty`, `SerializedObject.Update`, `EditorGUILayout.PropertyField`, and `SerializedObject.ApplyModifiedProperties` when using a custom editor to modify serialized properties on a component or an asset.

The only remaining use (which is used rarely) happens if a non-scene object is modified with other means and with no added undo entry. Use of this approach is unlikely.

Unity uses the dirty flag internally to find changed assets that must be saved to disk.

---

## UI Elements - The new UI System
In `Unity 2019.1`, Unity introduced the new UI-system called `UI Elements`. It is mostly based on events and actions. As a contrast `IMGUI` (the old system, "Immediate Mode Graphical User Interface") is based upon a GUI-loop which checks values all the time.

The biggest changes they've introduced is that they have separated different parts of the Editor Scripting process:
*   **C#:** Logic
*   **UXML:** Hierarchy of objects/components in your inspector
*   **USS:** CSS-Styles (ie. the padding for objects, what color your text has, what font to use, etc.)

**IMGUI vs UIElements**

Traditionally, building custom Editor windows and inspectors in Unity meant using IMGUI, an immediate mode API. IMGUI makes it easy to start building user interfaces but fails to scale when you’re building more complex applications. It’s also difficult for the system to optimize rendering because the user can drastically change the structure of the UI at any time within the current frame. Lastly, all UI is declared in C# which means that any future tool for authoring UI visually would need to generate C# code, which is a complicated proposition.

In UIElements, as a retained mode API, the idea is that you build your UI hierarchy of objects and let the system render it for you. This way, the system can optimize what it draws, and when it draws, leading to better overall performance. This paradigm also lets you decouple your hierarchy and styling from functionality, which results in a better separation of concerns and more approachable UI authoring for both artists and programmers.

See **“UIElementsExample”-script** for a simple UIElements-based Editor Window:
[https://drive.google.com/open?id=1q9-SB5Kxfuwo_8A6uHfvnNu2-SEdaSH2](https://drive.google.com/open?id=1q9-SB5Kxfuwo_8A6uHfvnNu2-SEdaSH2)

---

# Nice Classes, Functions and Code Snippets
## SetRandomRotationForObjects - Editor Window
Assign Random Rotation for objects with this simple Editor Wndow-script.

It rotates the chosen objects around one or more axises with X-amount of degrees

Uses: Could be used for level design or similar, when you want to make stuff more random and avoid the copy-paste-symptoms.

See code file for reference:

[https://drive.google.com/open?id=1Dj5AwenMfr4Wot9TsF8O_DCQ1H3EUyEh](https://drive.google.com/open?id=1Dj5AwenMfr4Wot9TsF8O_DCQ1H3EUyEh)


## Selection - How to set and get the Selected Objects
```C#
//The selected gameObjects:
var selectedObjects = Selection.gameObjects;

//The top-roots of the selection (excluding prefabs)
var selectedTopLevelObjects = Selection.transforms;

//The actual selection (ie. you can set your own selection if you want to)
var myCustomSelectionArray = [someArray];
var actualUserSelection = Selection.objects;

//Set the selection to your custom selection.
Selection.objects = myCustomSelectionArray;
```
---

## Layout tips
### Layout tips: Make a clickable Button
```C#
if (GUILayout.Button("My button text"))
{
   //OnClickedButton-code runs here.
}
```

### Layout tips: Nice boxes as sectioning elements
*För att göra ett gäng boxar som sektionerar av en del av dina tools (grafiskt), använd:*

```C#
EditorGUILayout.BeginVertical(GUI.skin.box);
EditorGUILayout.EndVertical();

EditorGUILayout.BeginHorizontal(GUI.skin.box);
EditorGUILayout.EndHorizontal();
```

Example:
```C#
EditorGUILayout.BeginHorizontal(GUI.skin.box);
GUIContent myStringLabel = new GUIContent(text: "My String: ", tooltip: "What's your string all about?");
myString = EditorGUILayout.TextField(myStringLabel, myString);
EditorGUILayout.EndHorizontal();
```

### Layout tips: Format your inspector’s text and styles
Check the “EditorStyles”-class. It is usually used together with the functions that create controls (labels, text, etc) in the GUILayout and EditorGUILayout-classes. Eg:

```C#
//Makes a label text with a **bold **font
GUILayout.Label(“My Bold Label”, EditorStyles.boldLabel);
```

### Layout tips: Make labels easily using GUIContent
```C#
public string myString;

private void OnGUI()
{
    //Create the label text with a tooltip.
    GUIContent myStringLabel = new GUIContent(text:"My String: ", tooltip:"What's your string all about?");

    //Create a text field, supply it with the label you created and insert the value it is supposed to write out and apply to the string-variable
    myString = EditorGUILayout.TextField(myStringLabel, myString);
}
```

---

## EditorUtility.DisplayDialog()

Make a simple popup-check for asking the user if they really want to do a specific task (eg. “Do you really want to remove this item?”)

```C#
//Make a Popup dialogue which you can use to display important info to your user.
EditorUtility.DisplayDialog(title: "No objects selected!",
    message: "You have to select some objects to be able to change their state!",
    ok: "Ok, I'll select some objects :)");
return;
```
---

## Draw default inspector, but draw some of the elements by yourself
Use the tag “HideInInspector” above your field:

```C#
public int myNormallyDrawnVariable;

[HideInInspector]
public int myCustomDrawnVariable;
```

## Serialized Classes: Use custom classes as variables in other classes

Create a class and add the `[System.Serializeable]`-tag to it. This will make unity draw it in your inspectors.

```C#
[System.Serializeable]
public class MyClass
{
	public string myString;
	public int myInt
}
```

## Serialized Classes on Steroids: Custom Property Drawers
Draw your own serialized class in a specific/custom way when it is used by other scripts as a variable. Useful for making variables of a serialized class always appear in the same custom way, without having to write a Custom Editor for each and every script which uses that class as a variable.

See Pablo’s [MinMaxSlider](https://drive.google.com/open?id=1wDx5pI9xPt5haH6G-rpBg1nmVd0-Nmcy) for an extensive and documented way of drawing a custom MinMaxSlider as a Property.

---


## [CanEditMultipleObjects]

`CanEditMultipleObjects` tells Unity that you designed you custom inspector in such a way that when multiple objects of the same type are selected they can all be edited at the same time.

If you use `serializedObject` Unity does this automatically for you. If you are not using `serializedObject` but, for example, you access the component directly by using the `target` variable, you should change your implementation to use the "targets" array instead which contains all selected components.

The `target` variable only accesses the first component.

---

## Gizmos

Draw your own help text and color your triggers for easier use of the Editor when doing Level Design.
[DrawColliderGizmo script + test scene](https://drive.google.com/open?id=1QgT4FoUCQWHgTDwMln3ZiSwAxy51SVgn )

```C#
private void DrawBoxColliderGizmo(Color gizmoBoxColor)
{
    //Get a reference to the Box Collider
    //which we'll use as a base for drawing the gizmo-box.
    var boxCollider = GetComponent<BoxCollider>();

    //Save the color in a temporary variable to not overwrite changes in the inspector.
    var color = gizmoBoxColor;

    //Draws the edges of the BoxCollider
    Gizmos.color = color;
    Gizmos.DrawWireCube(transform.position + boxCollider.center, boxCollider.size);

    //Draws the sides/insides of the BoxCollider, with a tint to the original color.
    color.a = 0.3f;
    Gizmos.color = color;
    Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
}

private void OnDrawGizmos()
{
    DrawBoxColliderGizmo(Color.red);
}

private void OnDrawGizmosSelected()
{
    DrawBoxColliderGizmo(Color.yellow);
}
```

---

# Resources, AssetDatabase & FileUtil
Useful classes for loading data in different ways:
*   [Resources](https://docs.unity3d.com/ScriptReference/Resources.html)
*   [AssetDatabase](https://docs.unity3d.com/ScriptReference/AssetDatabase.html)
*   [FileUtil](https://docs.unity3d.com/ScriptReference/FileUtil.html)


## AssetPreview.GetAssetPreview(asset: myAsset)
Get a `Texture2D` of the Preview Image that Unity automatically renders in the project, which you then can use to display in other places or Editors.

```C#
Object myAsset = null;
Texture2D previewImage = AssetPreview.GetAssetPreview(asset: myAsset);
GUIContent myGUIContentWithImage = new GUIContent(image: previewImage);
```

---

# Misc Editor Code

## Capture Keyboard Input in Editor Windows (ie. make shortcuts)

Run the code for input checks in the `OnGUI()`-function, else it will fail.

```C#
private void HandleKeyboardInput()
{
	//If we press escape while the Window is focused, we Close it.
	if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
	{
		Debug.Log("ESC!");
		this.Close();
	}
}

private void OnGUI()
{
    HandleKeyboardInput();
}
```

## UnityEditorInternal.ReorderableList

**MAY NOT BE NEEDED ANYMORE, SINCE ARRAYS ARE REORDERABLE SINCE UNITY 2021.X**

> **WARNING:** Use at your own risk, there is no guarantee that the classes within `UnityEditorInternal` won't change randomly on an update.

Have to include the `UnityEditorInternal`-namespace to make a `ReorderableList`

A `ReorderableList` is a very fancy implementation to inspect array type variables. Notice that we are adding `using UnityEditorInternal;` at the very top. `ReorderableList` is not usually exposed to the public yet and everything placed in UnityEditorInternal may change until Unity thinks it is ready. But the ReorderableList is so useful that I wanted to demonstrate how to use it here

See “Editor Scripting for n00bs”-tutorial (36:22-40:00) \
[https://youtu.be/9bHzTDIJX_Q?t=2182](https://youtu.be/9bHzTDIJX_Q?t=2182)

Also see “PistonE04PatternEditor.cs” in the [referenced project](http://letscodegames.com/ ) for how to set it up.


## Translate Screen Mouse Coords to World Coords
```C#
Event e = Event.current;
Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
Vector3 mousePos = r.origin;
```
Source (“Step 11”):
[Tutsplus](https://code.tutsplus.com/tutorials/how-to-add-your-own-tools-to-unitys-editor--active-10047)


## Saving settings persistently for Editor Scripts
```C#
//EditorPrefs is the same as PlayerPrefs but it only works in the editor
//This way you can store variables persistantly even if you close the editor
//Set a custom float
EditorPrefs.SetFloat( "MyStoredValue", 1f);

//Get current float value
EditorPrefs.GetFloat( "MyStoredValue");

//Get current float value.
//Define what default float value to return if the key is not in the storage.
EditorPrefs.GetFloat( "MyStoredValue", 0f);
```

**New in 2019:**
**SettingsProvider**
Seems to be a replacement for EditorPrefs and PlayerPrefs. If you like, do more research on that over here:

- [Unity Manual - SettingsProvider](https://docs.unity3d.com/ScriptReference/SettingsProvider.html)

---

## “typeof()” and “nameof()”

#### typeof()

Used when casting etc. In this context it is mostly used for defining the Monobehaviour that a Custom Editor is supposed to look at.

#### nameof()

Use variable's and classe's name for your property strings → if you update (ie. rename) the variabelname, the UI will update automatically and you avoid errors. Good while developing and debugging a plugin.

---

# Resources from the Lesson
## Example Scripts & GIFs used during the Lesson


- [Power Point Presentation](https://drive.google.com/open?id=19g0gLdQRyZTzn9pg3W8q0sYI8MSMSUMp)
- [Code Examples](https://drive.google.com/open?id=1X4Tw9u2fMJNo2YAkwrVxaMzjzS-TbHeQ)
- [GIFs-folder](https://drive.google.com/open?id=1O_KrePs4vI_KyTxNlZlmnkK6ZvgIIyoi)


## General links for Editor Scripting

- Editor Scripting for n00bs
  *   Video tutorial/talk on how to write your own Gizmos and a bunch of other useful stuff.
  *   Video starts at the Gizmo-stuff: [https://youtu.be/9bHzTDIJX_Q?t=400](https://youtu.be/9bHzTDIJX_Q?t=400)
  *   Entire source code: [http://letscodegames.com/](http://letscodegames.com/)


- Menu Item: Menu Index Position / Priority**
  *   Unity Menu Items Priority List - Guide to Extending Unity Editor’s Menus
  *   Gives a great and thorough overview on where the different Built-in Menu Items are, what their index is and how you can add your own Menu Items before/after the built-in ones.
  *   [Extending Unity Editors Menus](https://blog.redbluegames.com/guide-to-extending-unity-editors-menus-b2de47a746db)


- How to Make a Custom Editor Window in Unity
  *   Text Tutorial by “The Knights of Unity”, for IMGUI.
  *   Has code examples and a unitypackage to download with all the required scripts etc.
  *   [The Knights of Unity - Custom Unity Editor Window](https://blog.theknightsofunity.com/custom-unity-editor-window/)


- How to Add Your Own Tools to Unity’s Editor
  *   Creating an editor window, and a color picker whose selection we'll use to draw a grid. We'll also be able to create and delete objects, snapped to this grid, and undo such actions.
  *   [How to add your own tools to Unitys editor](https://code.tutsplus.com/tutorials/how-to-add-your-own-tools-to-unitys-editor--active-10047)


- Editor Scripting - Official Unity Tutorial
  *   Covers the basics of editor scripting, including building custom inspectors, gizmos, and other Editor windows.
  *   [Unity Learn - Editor Scripting](https://learn.unity.com/tutorial/editor-scripting#)


- Creating Basic Editor Tools
  *   Basics of editor scripting, with a focus on creating tools which improve workflow for programmers, as well as for artists and level designers. We'll create simple tools to optimize the asset import workflow and to help place objects in your scene.
  *   [Unity Learn - Creating Basic Editor Tools](https://learn.unity.com/tutorial/creating-basic-editor-tools#)


- How to Handle Persistent Data for Custom Editor Tools in Unity
  *   Medium-length blog post on how to save data for your Editor Tools
  *   [How to handle data for custom editor tools in Unity](https://blog.redbluegames.com/how-to-handle-data-for-custom-editor-tools-in-unity-6b85e9e17715)


- Github repo for “How to Handle Persistent Data for Custom Editor Tools in Unity”
  *   Check out _“AssetDatabaseUtility.cs”_ and _“ScriptableObjectUtility.cs”_ for nifty helper functions when loading assets from Resources.
  *   Also shows how to build an Editor Window for creating new objects based on presets. (Window > Primitive Creator)
  *   [https://github.com/PabloSorribes/unity-custom-tool-example](https://github.com/PabloSorribes/unity-custom-tool-example)
    *   (I forked the original so you guys don’t have to update the project, fix the gitignore, etc.)

---

# Paalo Bonus Goodies
Folder with all the Goodies:
[Google Drive Folder](https://drive.google.com/open?id=1edSI_Yh7CMYIZMRiILdDGDaiKnPZqN-6).

## EditorGUIHelper

Contains helper methods for common (and tedious) operations when writing IMGUI-code.

Most common ones:
*   `CalculateLabelWidth()` - Useful for setting bool-toggles (and other controls) right at the end of the label text.
*   `ResetIndent()` - Useful for when you’ve been messing around with the indentation level. Avoids you having to store the original indentation level somewhere and remember to reassign it.

## GameObjectExtensions
*   Extensions for getting if a gameObject is a prefab directly and other helpful stuff

## StringExtensions
*   How to strip a SubString

## HelperMethods
*   Class containing methods for doing SmoothStep with different Vector-types, as well as other helpful stuff.

## Editor Scene Hierarchy Traversal - Scene Traversal API

Really helpful functions and sorters for traversing the scene hierarchy. I’ve added some extra sorters for:


*   The GameObject with the most ancestors (ie. the Deepest Child) will be sorted as later in the list.
*   Order objects from top to bottom of the hierarchy view.
*   Do a Sort in a Lambda-expression, ie. you can make custom sorters right in the call, without creating a completely new class for each sort.
*   **Source:** [https://github.com/Real-Serious-Games/Unity-Scene-Traversal-Examples](https://github.com/Real-Serious-Games/Unity-Scene-Traversal-Examples)
*   **Article:** [https://www.what-could-possibly-go-wrong.com/scene-traversal-recipes-for-unity/](https://www.what-could-possibly-go-wrong.com/scene-traversal-recipes-for-unity/)

## SerializedPropertyExtensions
*   Extension class for SerializedProperties
*   See also: [http://answers.unity3d.com/questions/627090/convert-serializedproperty-to-custom-class.html](http://answers.unity3d.com/questions/627090/convert-serializedproperty-to-custom-class.html)

---

# UI Elements

- UIElements in Unity 2019.1 - [Quick Rundown of UI Elements](https://youtu.be/GSRVI1HqlF4)
- Customize the Unity Editor with UIElements! - [Short Tutorial / Showcase](https://youtu.be/CZ39btQ0XlE)
- What’s new with UIElements in 2019.1 - [Official Unity Blog post on UIElements](https://blogs.unity3d.com/2019/04/23/whats-new-with-uielements-in-2019-1/)
  *   Contains some code snippets that are handy to have.
- UIElements, a new UI system for the editor - Unite LA - [Live coding session](https://youtu.be/MNNURw0LeoQ)
  *   A talk / live coding session for implementing the same system with IMGUI vs UIElements
