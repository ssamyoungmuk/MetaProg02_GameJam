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

            EditorGUILayout.LabelField("사용 설명서 : 깃허브 연결되어있는 상태로 로컬 저장소 주소를 입력한 뒤 커밋 메시지를 입력하고 푸쉬해보세요.");
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("");

            gitLocalAddress = EditorGUILayout.TextField("로컬저장소링크", gitLocalAddress, GUILayout.Width(500f));

            if (GUILayout.Button("현재 연결된 브랜치 확인", GUILayout.Width(150f)))
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
            gitBranch = EditorGUILayout.TextField("변경할 브랜치 명", gitBranch, GUILayout.Width(500f));
            
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
            gitCommit = EditorGUILayout.TextField("커밋메시지 입력", gitCommit, GUILayout.Width(500f));
            if (GUILayout.Button("Commit", GUILayout.Width(150f)))
            {
                if (gitCommit == null) gitCommit = "Auto Push";
                gitCommited = gitCommit;
            }
            EditorGUILayout.LabelField("현재 입력된 커밋 메시지 = " + gitCommited);
            EditorGUILayout.LabelField("");
            
            

            if (GUILayout.Button("Push실행", GUILayout.Width(150f)))
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
            //gitHubAddress = EditorGUILayout.TextField("깃허브링크", gitHubAddress, GUILayout.Width(500f));
        }

    }
}
