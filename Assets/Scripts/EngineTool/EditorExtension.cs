using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObstaclesGeneratorWindow : EditorWindow
{
    private Terrain terrain;
    private Terrain lastTerrain;
    private List<string> obstacleNames;
    private List<string> selectedObstacles;

    [MenuItem("Window/Terrain Obstacles Generator")]
    public static void OpenWindow()
    {
        ObstaclesGeneratorWindow window = (ObstaclesGeneratorWindow)EditorWindow.GetWindow<ObstaclesGeneratorWindow>("Terrain Obstacles Generator");
        window.minSize = new Vector2(250, 250);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        {
            EditorGUILayout.Space();

            // Terrain field
            EditorGUILayout.LabelField("Terrain", EditorStyles.boldLabel);
            terrain = (Terrain)EditorGUILayout.ObjectField(terrain, typeof(Terrain), true);
            if (terrain != lastTerrain)
            {
                obstacleNames = null;
                selectedObstacles = null;
            }
            if (terrain == null)
            {
                obstacleNames = null;
                selectedObstacles = null;
            }
            else
            {
                EditorGUILayout.Space();

                // Get obstacle names from the terrain
                if (obstacleNames == null)
                {
                    obstacleNames = new List<string>();
                    foreach (var treePrototype in terrain.terrainData.treePrototypes)
                    {
                        obstacleNames.Add(treePrototype.prefab.name);
                    }
                    selectedObstacles = new List<string>();
                }

                // Dropdown for selecting obstacles
                EditorGUILayout.LabelField("Obstacles to take into account", EditorStyles.boldLabel);

                if (EditorGUILayout.DropdownButton(new GUIContent("Select Obstacles"), FocusType.Keyboard))
                {
                    GenericMenu menu = new GenericMenu();

                    // Option to select all
                    menu.AddItem(new GUIContent("Everything"), false, () =>
                    {
                        selectedObstacles.Clear();
                        selectedObstacles.AddRange(obstacleNames);
                    });

                    // Option to deselect all
                    menu.AddItem(new GUIContent("Nothing"), false, () =>
                    {
                        selectedObstacles.Clear();
                    });

                    menu.AddSeparator("");

                    // Options to select individual obstacles
                    foreach (var obstacleName in obstacleNames)
                    {
                        bool isSelected = selectedObstacles.Contains(obstacleName);
                        menu.AddItem(new GUIContent(obstacleName), isSelected, () =>
                        {
                            if (isSelected)
                            {
                                selectedObstacles.Remove(obstacleName);
                            }
                            else
                            {
                                selectedObstacles.Add(obstacleName);
                            }
                        });
                    }

                    menu.ShowAsContext();
                }

                EditorGUILayout.Space();

                // Display selected obstacles
                if (selectedObstacles.Count > 0)
                {
                    EditorGUILayout.LabelField("Selected Obstacles:", EditorStyles.boldLabel);
                    foreach (var obstacle in selectedObstacles)
                    {
                        EditorGUILayout.LabelField(obstacle);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("No obstacles selected.");
                }
            }
            lastTerrain = terrain;

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(20);
                if (GUILayout.Button("Generate Obstacles"))
                {
                    if (terrain == null)
                    {
                        Debug.LogError("Please assign a terrain.");
                        return;
                    }

                    SetTerrainObstaclesStatic.GenerateTreeObstacles(terrain, selectedObstacles.ToArray());
                }
                GUILayout.Space(20);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }
}
