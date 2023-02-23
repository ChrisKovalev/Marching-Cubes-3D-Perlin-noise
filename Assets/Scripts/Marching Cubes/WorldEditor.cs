using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGenerator))]
public class WorldEditor : Editor
{
    public delegate void generateWorld();
    public static event generateWorld OnGenerateWorld;

    Editor editor;

    bool autoUpdate = true;

    WorldGenerator worldGenerator;
   public override void OnInspectorGUI() {
        // using(var check = new EditorGUI.ChangeCheckScope()) {
        //     base.OnInspectorGUI();
        //     if(check.changed) {
        //         OnGenerateWorld();
        //     }
        // }
    
        autoUpdate = EditorGUILayout.Toggle("Auto Update", autoUpdate);
        GUILayout.Space(10);

        if(GUILayout.Button("Generate World")){
          if(OnGenerateWorld != null) {
            OnGenerateWorld();
          }
        }
        
        GUILayout.Space(10);
        DrawSettingsEditor(worldGenerator.gameObject.GetComponent<NoiseGenerator>().generatorSettings, worldGenerator.onSettingsUpdated, ref editor);
   }

   void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref Editor editor) {
        if(settings != null) {
            using (var check = new EditorGUI.ChangeCheckScope()) {
                CreateCachedEditor(settings, null, ref editor);
                editor.OnInspectorGUI();

                if(autoUpdate) {
                    if(onSettingsUpdated != null) {
                        onSettingsUpdated();
                        OnGenerateWorld();
                    }
                }
            }
        }
   }

   private void OnEnable(){
        worldGenerator = (WorldGenerator)target;
   }
}
