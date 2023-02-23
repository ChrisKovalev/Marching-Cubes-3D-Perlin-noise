using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Chunk))]
public class ChunkEditor : Editor
{
   Chunk chunk;

   public override void OnInspectorGUI() {

        using (var check =  new EditorGUI.ChangeCheckScope()) {
          base.OnInspectorGUI();
          if(check.changed){
            chunk.generateChunk();
          }
        }

        if(GUILayout.Button("Generate Chunk")){
          chunk.generateChunk();
        }
   }

   void DrawSettingsEditor(Object settings) {
        Editor editor = CreateEditor(settings);
        editor.OnInspectorGUI();
   }

   private void OnEnable(){
        chunk = (Chunk)target;
   }
}
