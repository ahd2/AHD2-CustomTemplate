using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理所有模板的SO的类
/// </summary>
public class TemplateSOManager
{
    public List<TemplateSO> templateSOs = new List<TemplateSO>();//SO列表

    public void AddTemplateSO(TemplateSO template)
    {
        templateSOs.Add(template);
    }

    public void RemoveTemplateSO(TemplateSO template)
    {
        templateSOs.Remove(template);
    }
}