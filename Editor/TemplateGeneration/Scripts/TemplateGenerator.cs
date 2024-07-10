using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class TemplateGenerator : EditorWindow
{
    string codeName = "";
    string menuName = "";
    string templateText = "";
    private string format = "";
    private string templateCSResourceFile = "Packages/com.ahd2.custom-template/Editor/TemplateGeneration/TemplateFile/Core/GeneratorTemplate.txt";

    [MenuItem("Tools/Template Generator")]
    public static void ShowWindow()
    {
        GetWindow<TemplateGenerator>("Template Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Create a new template file", EditorStyles.boldLabel);

        codeName = EditorGUILayout.TextField("缩写(只支持字母)", codeName);
        codeName = Regex.Replace(codeName, @"[^a-zA-Z]", ""); //移除codeName中的所有非字母字符
        menuName = EditorGUILayout.TextField("Menu Name", menuName);
        format = EditorGUILayout.TextField("格式", format);
        templateText = EditorGUILayout.TextArea(templateText, GUILayout.Height(100));

        if (GUILayout.Button("Create Template"))
        {
            CreateTemplateTxt();
            CreateTemplateCS();
        }
    }
    
    /// <summary>
    /// 创建模板的文本文件
    /// </summary>
    void CreateTemplateTxt()
    {
        //string path = EditorUtility.SaveFilePanel("Save Template File", "", codeName + ".txt", "txt"); //打开文件路径选择窗口

        string path = "Packages/com.ahd2.custom-template/Editor/TemplateGeneration/TemplateFile/" + codeName + "Template.txt";
        File.WriteAllText(path, templateText);
        AssetDatabase.Refresh();
        Debug.Log("模板创建完成！");
    }
    
    /// <summary>
    /// 创建模板的脚本文件
    /// </summary>
    void CreateTemplateCS()
    {
        string pathName = "Packages/com.ahd2.custom-template/Editor/TemplateGeneration/Scripts/Template/" + codeName + "Template.cs";//生成模板脚本文件
        string fullPath = Path.GetFullPath(pathName);
        StreamReader streamReader = new StreamReader(templateCSResourceFile);//读取模板CS文件文本内容
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
        text = Regex.Replace(text, "#CODENAME#", codeName); //
        text = Regex.Replace(text, "#MENUNAME#", menuName); //
        text = Regex.Replace(text, "#FORMAT#", format); //
        text = Regex.Replace(text, "#RESOURCEFILEPATH#", "Packages/com.ahd2.custom-template/Editor/TemplateGeneration/TemplateFile/" + codeName + "Template.txt"); //
        bool encoderShouldEmitUTF8Identifier = true;
        bool throwOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(pathName);
        AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
    }
}