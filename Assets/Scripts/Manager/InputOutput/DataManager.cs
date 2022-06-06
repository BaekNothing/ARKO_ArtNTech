using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;
using Consts;

public class DataManager : SingleTone<DataManager>
{
    protected override void Init()
    {
        base.Init();
        
    }   

    public enum ResourceCategories
    {
        GameObject = 0,
        Interface
    }

    public T PrefabLoad<T>(ResourceCategories categories, string name) where T : UnityEngine.Object
    {
        string path = "Prefabs/" + categories.ToString() + "/" + name;
        T result = Resources.Load<T>(path);
        if (result)
            return result;
        return Consts.Utility.LogAndNull("Resource load fail") as T;
    }

    public List<DialogManager.DialogInfoStruct> DialogLoad(int code)
    {
        List<DialogManager.DialogInfoStruct> result = new List<DialogManager.DialogInfoStruct>();
        List<Dictionary<string, object>> dataDialog = CSVReader.Read("TableData/Dialog");
        foreach(Dictionary<string, object> keyPair in dataDialog)
        {
            if(keyPair.ContainsKey("Code") && (int)keyPair["Code"] == code)
            {
                DialogManager.DialogInfoStruct dialogInfoStruct = new DialogManager.DialogInfoStruct(
                    keyPair.ContainsKey("mainText") ? (string)keyPair["mainText"] : null,
                    keyPair.ContainsKey("Code") ? (int)keyPair["Code"] : 0,
                    keyPair.ContainsKey("name") ? (string)keyPair["name"] : null,
                    keyPair.ContainsKey("left") ? (string)keyPair["left"] : null,
                    keyPair.ContainsKey("right") ? (string)keyPair["right"] : null
                    );
                result.Add(dialogInfoStruct);
            }
        }
        return result;
    }

 
public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}

//data Load
public CoreData.charStatus LoadUserData()
    {
        CoreData.charStatus charStatus = new CoreData.charStatus();
        #if PLATFORM_STANDALONE_WIN
            string path = Application.dataPath + "/Resources";
        #elif UNITY_EDITOR
            string path = Application.dataPath + "/Resources";
        #elif UNITY_ANDROID
            string path = Application.persistentDataPath;
        #elif UNITY_IOS
            string path = Application.persistentDataPath;
        #endif
    
        string dataName = "/userData.json";
        if(!File.Exists(path + dataName))
            SaveUserData(charStatus);
    
        FileStream fileStream = new FileStream(path + dataName, FileMode.Open);
        StreamReader streamReader = new StreamReader(fileStream);

        if (File.Exists(path + dataName))
            charStatus = JsonUtility.FromJson<CoreData.charStatus>(streamReader.ReadToEnd());
        streamReader.Dispose();
        streamReader.Close();
        fileStream.Close();
        return charStatus;
    }

    public void SaveUserData(CoreData.charStatus charStatus)
    {
        #if PLATFORM_STANDALONE_WIN
            string path = Application.dataPath + "/Resources";
        #elif UNITY_EDITOR
            string path = Application.dataPath + "/Resources";
        #elif UNITY_ANDROID
            string path = Application.persistentDataPath;
        #elif UNITY_IOS
            string path = Application.persistentDataPath;
        #endif
    
        string dataName = "/userData.json";

        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);

        FileStream fileStream = new FileStream(path + dataName, FileMode.Create);
        StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        streamWriter.Write(JsonUtility.ToJson(charStatus));
        streamWriter.Flush();
        streamWriter.Close();
        fileStream.Close();
    }
}
