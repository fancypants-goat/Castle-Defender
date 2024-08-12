using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class BuildingAssetEditor : EditorWindow {

    [SerializeField] GameObject buildingPrefab, buildingUIButton, buildMenu;
    [SerializeField] BuildingManager buildingManager;

    [MenuItem("GameTools/BuildingAsset")]
    private static void ShowWindow() 
    {
        var window = GetWindow<BuildingAssetEditor>();
        window.titleContent = new GUIContent("BuildingAsset");
        window.Show();
    }
    void OnEnable() 
    {
        buildingManager = FindObjectOfType<BuildingManager>();
        buildingUIButton = buildingManager.buildingButtonPrefab;
    }
    private void OnGUI() 
    {
        GUILayout.Label("Necessary Objects", EditorStyles.boldLabel);
        
        buildMenu = (GameObject)EditorGUILayout.ObjectField("Build Menu", buildMenu, typeof(GameObject), true);
        GUI.enabled = false;
        buildingUIButton = (GameObject)EditorGUILayout.ObjectField("Button", buildingUIButton, typeof(GameObject), false);
        buildingManager = (BuildingManager)EditorGUILayout.ObjectField("Building Manager", buildingManager, typeof(BuildingManager), true);
        GUI.enabled = true;

        GUILayout.Space(10);

        GUILayout.Label("Building to be Added", EditorStyles.boldLabel);

        buildingPrefab = (GameObject)EditorGUILayout.ObjectField("Building Prefab", buildingPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Generate Building"))
        {
            Undo.RecordObject(buildingManager, "Add Building");

            buildingManager.buildingPrefabs.Add(buildingPrefab);
            GameObject button = Instantiate(buildingUIButton,buildMenu.transform);
            buildingManager.buildingButtons.Add(button.GetComponent<Button>());

            EditorUtility.SetDirty(buildingManager);
        }
    }
}