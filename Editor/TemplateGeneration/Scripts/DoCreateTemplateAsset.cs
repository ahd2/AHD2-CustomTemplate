using UnityEditor;
using System.IO;
using System.Text;
using UnityEditor.ProjectWindowCallback;
using System.Text.RegularExpressions;

/// <summary>
/// 在结束创建资产的命名的行为时触发的，主要目的是进行资产创建和内容文本替换。
/// </summary>
class DoCreateTemplateAsset : EndNameEditAction
{
 
 
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        UnityEngine.Object o = CreateScriptAssetFromTemplate(pathName, resourceFile);
        ProjectWindowUtil.ShowCreatedAsset(o);
    }
 
    internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
    {
        string fullPath = Path.GetFullPath(pathName);
        StreamReader streamReader = new StreamReader(resourceFile);
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
        text = Regex.Replace(text, "#NAME#", fileNameWithoutExtension); //把模板文件中为#NAME#的字符串给替换掉
        //string text2 = Regex.Replace(fileNameWithoutExtension, " ", string.Empty);
        //text = Regex.Replace(text, "#SCRIPTNAME#", text2);
        //if (char.IsUpper(text2, 0))
        //{
        //    text2 = char.ToLower(text2[0]) + text2.Substring(1);
        //    text = Regex.Replace(text, "#SCRIPTNAME_LOWER#", text2);
        //}
        //else
        //{
        //    text2 = "my" + char.ToUpper(text2[0]) + text2.Substring(1);
        //    text = Regex.Replace(text, "#SCRIPTNAME_LOWER#", text2);
        //}
        bool encoderShouldEmitUTF8Identifier = true;
        bool throwOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(pathName);
        return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
    }
 
}  