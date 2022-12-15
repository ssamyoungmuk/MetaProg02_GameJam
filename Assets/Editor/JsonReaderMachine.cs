using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

namespace WHBKYK 
{
    public class JsonReaderMachine : EditorWindow
    {
        private readonly string pyPath = Directory.GetCurrentDirectory() + "/JsonParsing/ParsingData.exe";
        private string path = "";
        private string fileName = "";
        private string result = "";
        private ReadJson rj = null;
        private Vector2 scorollPos = Vector2.zero;

        [MenuItem("WBY_Library/JsonReaderMachine")]
        public static void ShowWindow_BK()
        {
            JsonReaderMachine build = (JsonReaderMachine)EditorWindow.GetWindow(typeof(JsonReaderMachine));
            build.minSize = new Vector2(150f, 150f);
            build.maxSize = new Vector2(1000f, 1000f);
            build.Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Input Json Path");
            path = GUILayout.TextField(path, 100);

            GUILayout.Label("Input Json File Name");
            fileName = GUILayout.TextField(fileName, 100);

            if (GUILayout.Button("READ START"))
            {
                FileInfo python = new FileInfo(pyPath);
                if (python.Exists == true)
                {
                    Process.Start(pyPath,(path + " " + fileName)).WaitForExit();
                }
                else
                {
                    UnityEngine.Debug.LogError("Not Found Python File");
                    return;
                }

                rj = new ReadJson(path);
                result = rj.JsonParsing();
            }

            scorollPos = GUILayout.BeginScrollView(scorollPos,false,true);
            GUILayout.TextArea(result, GUILayout.ExpandHeight(true));
            GUILayout.EndScrollView();
            
            GUILayout.FlexibleSpace();


            GUILayout.EndVertical();
        }

    }
}


