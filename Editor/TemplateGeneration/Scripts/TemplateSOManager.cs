using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 管理所有模板的SO的类
/// </summary>
public class TemplateSOManager : ScriptableObject
{
    public List<TemplateSO> templateSOs = new List<TemplateSO>();//SO列表

    public void AddTemplateSO(TemplateSO template)
    {
        templateSOs.Add(template);
    }

    public void RemoveTemplateSO(TemplateSO template)
    {
        AssetDatabase.DeleteAsset(template.txtPath);
        AssetDatabase.DeleteAsset(template.csPath);
        AssetDatabase.DeleteAsset(template.soPath);
        templateSOs.Remove(template);
    }
}