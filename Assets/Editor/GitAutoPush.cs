using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;
using UnityEngine.UI;

namespace WHBKYK
{

    public class GitAutoPush : EditorWindow
    {
        string line;
        string line2;
        string gitHubAddress;
        string gitLocalAddress;
        string gitBranch;
        string gitCommit;
        string gitCommited;
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/GitAutoPush/";
        string pushBat = "GitAutoPush.bat";
        string branch = "GitBranchChange.bat";
        string currentBranch = "CurrentBranch.txt";
        string switchBranch = "GitSwitch.bat";

        [MenuItem("WBY_Library/gitHubAutoMachine")]
        public static void ShowWindow_WH()
        {
            GitAutoPush build = (GitAutoPush)EditorWindow.GetWindow(typeof(GitAutoPush));
            build.minSize = new Vector2(150f, 150f);
            build.maxSize = new Vector2(1000f, 1000f);
            build.Show();
        }

        private void OnGUI()
        {
            GitHubAddressText();
        }

        void GitHubAddressText()    
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            EditorGUILayout.LabelField("��� ���� : ����� ����Ǿ��ִ� ���·� ���� ����� �ּҸ� �Է��� �� Ŀ�� �޽����� �Է��ϰ� Ǫ���غ�����.");
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("");

            gitLocalAddress = EditorGUILayout.TextField("��������Ҹ�ũ", gitLocalAddress, GUILayout.Width(500f));

            if (GUILayout.Button("���� ����� �귣ġ Ȯ��", GUILayout.Width(150f)))
            {
                StreamWriter sw3 = new StreamWriter(path + branch);
                sw3.WriteLine("cd " + gitLocalAddress);
                sw3.WriteLine("git status > CurrentBranch.txt");
                sw3.Flush();
                sw3.Close();
                Process.Start(path + branch).WaitForExit();
                StreamReader sr = new StreamReader(gitLocalAddress +"/" + currentBranch);
                line = sr.ReadLine();
                line2 = line.Replace("On branch ","");
            }
            EditorGUILayout.LabelField(line2);
            EditorGUILayout.LabelField("");
            /*
            gitBranch = EditorGUILayout.TextField("������ �귣ġ ��", gitBranch, GUILayout.Width(500f));
            
            if (GUILayout.Button("Switch", GUILayout.Width(150f)))
            {
                if (gitBranch == null) return;
                StreamWriter sw = new StreamWriter(path + switchBranch);
                sw.WriteLine("cd " + gitLocalAddress);
                sw.WriteLine("git switch " + gitBranch);
                sw.Flush();
                sw.Close();
                Process.Start(path + switchBranch);
            }
            EditorGUILayout.LabelField("");
            */
            gitCommit = EditorGUILayout.TextField("Ŀ�Ը޽��� �Է�", gitCommit, GUILayout.Width(500f));
            if (GUILayout.Button("Commit", GUILayout.Width(150f)))
            {
                if (gitCommit == null) gitCommit = "Auto Push";
                gitCommited = gitCommit;
            }
            EditorGUILayout.LabelField("���� �Էµ� Ŀ�� �޽��� = " + gitCommited);
            EditorGUILayout.LabelField("");
            
            

            if (GUILayout.Button("Push����", GUILayout.Width(150f)))
            {
                StreamWriter sw2 = new StreamWriter(path+ pushBat);
                sw2.WriteLine("cd " + gitLocalAddress);
                sw2.WriteLine("git add . ");
                sw2.WriteLine("git commit -m \"" + gitCommited + "\"");
                sw2.WriteLine("git push > Result.Log");
                sw2.WriteLine("pause");
                sw2.Flush();
                sw2.Close();
                Process.Start(path + pushBat).WaitForExit();
            }
            EditorGUILayout.LabelField("");
            //gitHubAddress = EditorGUILayout.TextField("����긵ũ", gitHubAddress, GUILayout.Width(500f));
        }

    }
}
