using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorRoomSelector : MonoBehaviour {

    [MenuItem("Rooms/Girl's Room")]
    static void GotoGirlsRoom()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Testing/GirlRoom/D-GirlsRoom.unity");
    }

    [MenuItem("Rooms/Teen Room")]
    static void GotoTeenRoom()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Testing/Teen Room/D-TeenRoom.unity");
    }

    [MenuItem("Rooms/Depression Room")]
    static void GotoDepressionRoom()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Testing/Depression Room/M_DepressionRoom.unity");
    }

    [MenuItem("Rooms/Doctor's Office")]
    static void GotoDoctorOffice()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Testing/Doctor's Office/DoctorsOffice.unity"); //' is Dean's Shame
    }

    [MenuItem("Rooms/Grocery Store")]
    static void GotoGroceryStore()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Testing/Grocery Store/GroceryStore_Rework.unity");
    }

    [MenuItem("Rooms/Bathroom")]
    static void GotoBathroom()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Testing/Bathroom/D-Bathroom.unity");
    }

    [MenuItem("Rooms/Transition Room")]
    static void GotoTransitionRoom()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Testing/TransitionRoom/D-TransitionRoom.unity");
    }
}
