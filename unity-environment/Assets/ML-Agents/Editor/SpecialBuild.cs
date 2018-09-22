// C# example.
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpecialBuild
{

    private const string executableName = "Basic1";
    private const string executableName2 = "Basic2";
    private const string executableName3 = "Basic3";
    private const string path = "C:/Users/Sebas/Desktop/Workspace/UnityProjects/ml-agents - Copy/python/Builds/" ;

    private static string[] levels = new string[] { "Assets/ML-Agents/Examples/Matchmaking/Matchmaking.unity" };
    //private static string[] levels = new string[] { "Assets/GGJ.unity" };

    private static float startWaitingTime = -1;

    private static List<System.Diagnostics.Process> processes = new List<System.Diagnostics.Process>();

    [MenuItem("MyTools/BuildGame1 %g")]
    private static bool BuildGame1()
    {
        return BuildGame(executableName, "1/");
    }

    [MenuItem("MyTools/BuildGame2 %h")]
    private static bool BuildGame2()
    {
        return BuildGame(executableName2, "2/");
    }

    [MenuItem("MyTools/BuildGame3 %j")]
    private static bool BuildGame3()
    {
        return BuildGame(executableName3, "3/");
    }



    private static bool BuildGame(string exeName, string folder)
    {
        string fullPath = path + folder + exeName + ".exe";
        PrepareBuild(exeName);

        string x = BuildPipeline.BuildPlayer(levels, fullPath, BuildTarget.StandaloneWindows64, BuildOptions.Development);
        if (x.Contains("cancelled") || x.Contains("error"))
        {
            Debug.LogError(x);
            return false;
        }
        Debug.Log(x);
        return true;

    }

    private static void PrepareBuild(string exeName)
    {
        KillGames(exeName);
        EditorApplication.isPlaying = false;
    }

    //[MenuItem("MyTools/RunGame %q")]
    //private static void RunGame()
    //{
    //    var proc = new System.Diagnostics.Process();
    //    processes.Add(proc);
    //    proc.StartInfo.FileName = path;
    //    proc.Start();
    //}

    [MenuItem("MyTools/KillGames %k")]
    private static void KillGames(string exeName)
    {
        foreach (System.Diagnostics.Process process in System.Diagnostics.Process.GetProcessesByName(exeName))
        {
            process.Kill();
        }
    }


    private static void ChangeScene(string sceneName, bool tried = false)
    {
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            try
            {
                EditorSceneManager.SaveOpenScenes();// SaveScene();// SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(sceneName);
            }
            catch(InvalidOperationException)
            {
                if (!tried)
                {
                    ChangeScene(sceneName, true);
                }
            }
        }
    }
}
