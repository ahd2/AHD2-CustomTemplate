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
    public static TemplateSOManager templateSOManager; //SO管理器
    //消息提示框提示的消息
    string message = "";
    MessageType messageType = MessageType.None;
    
    // 子界面的枚举类型，用于切换
    private enum SubWindowType
    {
        GeneratorWindow,
        SOListWindow
    }

    // 当前选中的子界面类型
    private SubWindowType currentSubWindow = SubWindowType.GeneratorWindow;

    [MenuItem("Tools/Template Generator")]
    public static void ShowWindow()
    {
        GetWindow<TemplateGenerator>("Template Generator");
    }
    
    private void OnEnable()
    {
        // 加载已经存在的TemplateSOManager资产，如果不存在则创建一个新的，目的是记录下整个项目的模板信息。
        templateSOManager = AssetDatabase.LoadAssetAtPath<TemplateSOManager>("Packages/com.ahd2.custom-template/Editor/TemplateGeneration/Scripts/TemplateSOManager.asset");
        if (templateSOManager == null)
        {
            templateSOManager = ScriptableObject.CreateInstance<TemplateSOManager>();
            AssetDatabase.CreateAsset(templateSOManager, "Packages/com.ahd2.custom-template/Editor/TemplateGeneration/Scripts/TemplateSOManager.asset");
            AssetDatabase.SaveAssets();
        }
    }

    #region GUI绘制部分
    void OnGUI()
    {
        // 根据选中的标签页显示不同的子界面
        switch (currentSubWindow)
        {
            case SubWindowType.GeneratorWindow:
                GUILayout.Label("创建一个新模板。", EditorStyles.boldLabel);
                break;
            case SubWindowType.SOListWindow:
                GUILayout.Label("管理已生成的模板。", EditorStyles.boldLabel);
                break;
        }
        // 顶部标签页
        GUILayout.BeginHorizontal();
        currentSubWindow = (SubWindowType)GUILayout.Toolbar((int)currentSubWindow, new string[] { "模板生成", "模板列表" });
        GUILayout.EndHorizontal();
        
        // 根据选中的标签页显示不同的子界面
        switch (currentSubWindow)
        {
            case SubWindowType.GeneratorWindow:
                DrawGeneratorWindow();
                break;
            case SubWindowType.SOListWindow:
                DrawSOListWindow();
                break;
        }
    }
    private void DrawGeneratorWindow()
    {
        codeName = EditorGUILayout.TextField("缩写(只支持字母)", codeName);
        codeName = Regex.Replace(codeName, @"[^a-zA-Z]", ""); //移除codeName中的所有非字母字符
        menuName = EditorGUILayout.TextField("菜单显示名称", menuName);
        format = EditorGUILayout.TextField("格式", format);
        templateText = EditorGUILayout.TextArea(templateText, GUILayout.Height(100));
        //消息盒子
        if (!string.IsNullOrEmpty(message))
        {
            EditorGUILayout.HelpBox(message, messageType);
        }
        
        //模板创建按钮
        if (GUILayout.Button("创建模板"))
        {
            if (string.IsNullOrEmpty(codeName) || string.IsNullOrEmpty(menuName) ||  string.IsNullOrEmpty(format))
            {
                message = "缩写、菜单名和格式不能为空！";
                messageType = MessageType.Error;
                return;
            }

            CreateTemplateTxt();
            CreateTemplateCS();
            CreateTemplateSO();
            message = "模板" + codeName +"创建完成！";
            messageType = MessageType.Info;
        }
        
        // 应用更改
        if (GUI.changed)
        {
            //EditorUtility.SetDirty(templateManager);
        }
    }

    private void DrawSOListWindow()
    {
        Vector2 scrollPosition = Vector2.zero;
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
        if (templateSOManager.templateSOs.Count == 0)
        {
            EditorGUILayout.HelpBox("模板列表为空", MessageType.Info);
        }
        else
        {
            for (int i = 0; i < templateSOManager.templateSOs.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                templateSOManager.templateSOs[i] = (TemplateSO)EditorGUILayout.ObjectField(templateSOManager.templateSOs[i], typeof(TemplateSO), false);
                if (GUILayout.Button("Remove"))
                {
                    templateSOManager.RemoveTemplateSO(templateSOManager.templateSOs[i]);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(templateSOManager);
        }
    }
    #endregion

    #region 逻辑函数部分
    /// <summary>
    /// 创建模板的文本文件
    /// </summary>
    void CreateTemplateTxt()
    {
        //string path = EditorUtility.SaveFilePanel("Save Template File", "", codeName + ".txt", "txt"); //打开文件路径选择窗口

        string path = "Assets/AHD2CustomTemplate/TemplateFile/" + codeName + "Template.txt";
        CreateDirectory("Assets/AHD2CustomTemplate/TemplateFile");//尝试创建路径
        File.WriteAllText(path, templateText);
        AssetDatabase.Refresh();
        //Debug.Log("模板创建完成！");
    }
    
    /// <summary>
    /// 创建模板的脚本文件
    /// </summary>
    void CreateTemplateCS()
    {
        //string pathName = "Packages/com.ahd2.custom-template/Editor/TemplateGeneration/Scripts/Template/" + codeName + "Template.cs";//生成模板脚本文件
        string pathName = "Assets/AHD2CustomTemplate/Scripts/Template/" + codeName + "Template.cs";//生成模板脚本文件
        CreateDirectory("Assets/AHD2CustomTemplate/Scripts/Template");//尝试创建路径
        string fullPath = Path.GetFullPath(pathName);
        StreamReader streamReader = new StreamReader(templateCSResourceFile);//读取模板CS文件文本内容
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
        text = Regex.Replace(text, "#CODENAME#", codeName); //
        text = Regex.Replace(text, "#MENUNAME#", menuName); //
        text = Regex.Replace(text, "#FORMAT#", format); //
        text = Regex.Replace(text, "#RESOURCEFILEPATH#", "Assets/AHD2CustomTemplate/TemplateFile/" + codeName + "Template.txt"); //
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
    
    //创建SO，记录CS和txt路径。
    void CreateTemplateSO()
    {
        string soPath = "Assets/AHD2CustomTemplate/Scripts/TemplateSO/" + codeName +
                        "TemplateSO.asset";
        CreateDirectory("Assets/AHD2CustomTemplate/Scripts/TemplateSO");//尝试创建路径
        TemplateSO newTemplateSo = ScriptableObject.CreateInstance<TemplateSO>();
        newTemplateSo.templateCSName = codeName;
        newTemplateSo.soPath = soPath;
        newTemplateSo.txtPath = "Assets/AHD2CustomTemplate/TemplateFile/" + codeName + "Template.txt";
        newTemplateSo.csPath = "Assets/AHD2CustomTemplate/Scripts/Template/" + codeName + "Template.cs";
        AssetDatabase.CreateAsset(newTemplateSo, soPath);
        AssetDatabase.SaveAssets();
        templateSOManager.AddTemplateSO(newTemplateSo);
    }

    public void CreateDirectory(string path)
    {
        // 检查路径是否已存在
        if (!Directory.Exists(path))
        {
            // 如果路径不存在，创建它
            Directory.CreateDirectory(path);
        }
    }
    #endregion
}