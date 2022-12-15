using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace WHBKYK
{
    public class WhereisScriptMachine : EditorWindow
    {
        string scriptName = "";
        Object[] resultObject = null;
        [MenuItem("WBY_Library/WhereisScriptMachine")]
        public static void ShowWindow_YK()
        {
            WhereisScriptMachine build = (WhereisScriptMachine)GetWindowWithRect(typeof(WhereisScriptMachine), new Rect(0f,0f,200f,300f));
            build.minSize = new Vector2(150f, 150f);
            build.maxSize = new Vector2(1000f, 1000f);
            build.Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Script Name : ");
            scriptName = EditorGUILayout.TextField(scriptName);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Find Script"))
            {
                resultObject = FindObjectsOfTypeByName_YK(scriptName);
            }
            if (resultObject != null)
            {
                for (int i = 0; i < resultObject.Length; i++)
                {
                    EditorGUILayout.TextArea(resultObject[i].name);
                }
            }
        }
        public static Object[] FindObjectsOfTypeByName_YK(string name)
        {
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            Debug.Log(assemblies.Length);
            for (int i = 0; i < assemblies.Length; i++)
            {
                var types = assemblies[i].GetTypes();
                for (int n = 0; n < types.Length; n++)
                {
                    if (typeof(Object).IsAssignableFrom(types[n]) && name == types[n].Name)
                        return Resources.FindObjectsOfTypeAll(types[n]);
                }
            }
            return new Object[0];
        }

        public static Object[] FindObjectsOfTypeByName_Simple(string name)
        {
            var type = System.Type.GetType(name);
            Debug.Log(type);
            return FindObjectsOfType(type);
        }
    }

}
