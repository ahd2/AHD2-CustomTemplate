using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using UnityEditor.ProjectWindowCallback;
using System.Text.RegularExpressions;


public class CSTemplate : Template
{
    
    [MenuItem("Assets/Create/自定义模板CustomTemplate/C# Script", false, 80)]
    public static void CreatNewCS()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
        ScriptableObject.CreateInstance<DoCreateTemplateAsset>(),
        GetSelectedPathOrFallback() + "/NewCS.cs",
        EditorGUIUtility.FindTexture("Packages/com.ahd2.custom-template/Editor/TemplateGeneration/TemplateFile/Core/ahd2Icon.png"),
       "Packages/com.ahd2.custom-template/Editor/TemplateGeneration/TemplateFile/C#Template.txt");
    }
}
 
  