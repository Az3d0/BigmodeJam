using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadManagers : EditorWindow
{
    [MenuItem("Window/LoadManagers")]
    public static void ShowWindow()
    {
        GetWindow<LoadManagers>("Level Loader");
    }

    void OnGUI()
    {
        EditorGUILayout.Space(10);

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Load Manager Scenes"))
        {
            // Check if the user wants to save any changes first
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // Additionally load everything else 
                // MAKE A CHANGE - Add your new scene to this by copy and pasting and updating an existing one
                EditorSceneManager.OpenScene("Assets/Scenes/Managers.unity", OpenSceneMode.Additive);
            }
        }
    }
}
