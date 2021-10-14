using UnityEngine;
using UnityEditor;
using System;
using Altom.Editor;

public class CustomBuild 
{
     [MenuItem("Game Build/Build and Run")]
    static void BuildAndRun()
    {
        try
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new string[] {
                "Assets/Scenes/MainMenu.unity",
                "Assets/Scenes/MainLevel.unity"
            };

            buildPlayerOptions.locationPathName = "./builds/SpaceDefenderGame.exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AutoRunPlayer;
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_4_6);

            //  Setup for AltUnity
            var buildTargetGroup = BuildTargetGroup.Standalone;
            AltUnityBuilder.AddAltUnityTesterInScritpingDefineSymbolsGroup(buildTargetGroup);
            if (buildTargetGroup == UnityEditor.BuildTargetGroup.Standalone)
                AltUnityBuilder.CreateJsonFileForInputMappingOfAxis();
            AltUnityBuilder.InsertAltUnityInScene(buildPlayerOptions.scenes[0]);

            var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
            AltUnityBuilder.RemoveAltUnityTesterFromScriptingDefineSymbols(BuildTargetGroup.Standalone);

        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

}
