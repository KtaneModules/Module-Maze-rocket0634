#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

[CustomEditor(typeof(SpriteCreator))]
public class SpriteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Start"))
            ((SpriteCreator)target).CreateSprites();
        if (GUILayout.Button("Get Names"))
            ((SpriteCreator)target).ReadNames();
        if (GUILayout.Button("Set Names"))
            ((SpriteCreator)target).SetNames();
    }
}

public class SpriteData
{
    public readonly int serializedVersion = 2;
    public string name = "";
    public class Rect
    {
        public readonly int serializedVersion = 2;
        public int x, y, width, height;
    };
    public Rect rect = new Rect();
    public readonly int alignment = 0;
    public readonly string pivot = "{x: 0.5, y: 0.5}";
    public readonly string border = "{x: 0, y: 0, z: 0, w: 0}";
    public readonly string outline = "[]";
    public readonly string physicsShape = "[]";
    public readonly int tessellationDetail = 0;
}

public class DictIndex
{
    public int curIndex;
    public string name;
}

public class SpriteCreator : MonoBehaviour
{
    public Texture2D SpriteTexture;
    public int x, y;
    public string path;
    private string meta;
    public List<string> names;
    public string fullName;
    private Dictionary<int, DictIndex> spriteNames = new Dictionary<int, DictIndex>();

    public void CreateSprites()
    {
        if (File.Exists(path))
        {
            meta = File.ReadAllText(path);
            meta = meta.Substring(meta.IndexOf("sprites"), meta.IndexOf("\n    outline") - meta.IndexOf("sprites"));
        }
        else return;
        var jsonFull = new List<string>();
        var width = (float)SpriteTexture.width / x;
        var height = (float)SpriteTexture.height / y;
        if (width % 1 != 0 || height % 1 != 0)
        {
            Debug.LogFormat("Invalid filesize [must be an equally divisible amount]");
            return;
        }

        for (int i = 0; i < (SpriteTexture.width / x) * (SpriteTexture.height / y); i++)
        {
            //Keep "i" within 0 and how many times "x" goes into the width of the sheet
            var j = i % (SpriteTexture.width / x);
            //"i" only changes every "y" values, based on how many times "y" goes into the height of the sheet
            var k = i / (SpriteTexture.height / y);
            //Origin is on bottom left, but sprites are counted from the top left. As such, "i" values must count down, not up.
            k = SpriteTexture.height / y - k;
            var spriteData = new SpriteData
            {
                rect = new SpriteData.Rect
                {
                    x = x * j,
                    //When reversing "i" values, we end up with "y" values of 20-1, but what we really need is 19-0, so subtract 1 from "y"
                    y = y * (k - 1),
                    width = x,
                    height = y
                }
            };
            var json = JsonConvert.SerializeObject(spriteData, Formatting.Indented);
            while (json.Contains("\"") || json.Contains(",") || json.Contains(((char)13).ToString()))
                json = json.Replace("\"", "").Replace(",", "").Replace(((char)13).ToString(), "");
            json = json.Replace("{\n  serialized", "- serialized").Replace("rect: {", "rect:").Replace("  }\n", "}").Replace("\n}", "\n");
            json = json.Replace("x: 0.5", "x: 0.5,").Replace("0 y: 0 z: 0", "0, y: 0, z: 0,");
            jsonFull.Add(json);
        }

        meta = File.ReadAllText(path);
        meta = meta.Remove(meta.IndexOf("sprites") + 7, meta.IndexOf("\n    outline", meta.IndexOf("sprites")) - meta.IndexOf("sprites") + 7);
        meta = meta.Insert(meta.IndexOf("sprites") + 7, ":\n  " + string.Join("", jsonFull.ToArray()));
        File.WriteAllText(path, meta);
    }

    public void GetNames()
    {
        if (File.Exists(path))
        {
            meta = File.ReadAllText(path);
            meta = meta.Substring(meta.IndexOf("sprites"), meta.IndexOf("\n    outline") - meta.IndexOf("sprites"));
        }
        var currentIndex = meta.IndexOf("name");
        var dirIndex = 0;
        while (currentIndex < meta.Length && currentIndex > 0)
        {
            currentIndex += 6;
            var nextIndex = meta.IndexOf("\n", currentIndex);
            if (nextIndex > 0) spriteNames.Add(dirIndex, new DictIndex { curIndex = currentIndex, name = meta.Substring(currentIndex, nextIndex - currentIndex) });
            else return;
            currentIndex = meta.IndexOf("name", nextIndex);
            dirIndex++;
            if (dirIndex > 430)
            {
                Debug.LogFormat(currentIndex + " " + nextIndex + " " + (currentIndex < meta.Length).ToString() + " " + (currentIndex > 0).ToString());
                break;
            }
        }
    }

    public void ReadNames()
    {
        spriteNames.Clear();
        GetNames();
        names.Clear();
        for (int i = 0; i < spriteNames.Count; i++)
            names.Add(spriteNames[i].name);
        fullName = string.Join(";", names.ToArray());
    }

    public void SetNames()
    {
        if (spriteNames.Count < 1)
        {
            spriteNames.Clear();
            GetNames();
        }
        meta = File.ReadAllText(path);
        var smallMeta = meta.Substring(meta.IndexOf("sprites"), meta.IndexOf("\n    outline") - meta.IndexOf("sprites"));
        var oldMeta = smallMeta;
        var curIndex = 0;
        names = fullName.Split(new[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
        while (names.Count > spriteNames.Count) names.RemoveAt(names.Count - 1);
        for (int i = 0; i < spriteNames.Count; i++)
        {
            spriteNames[i].name = names[i];
            curIndex = smallMeta.IndexOf("name", curIndex) + 6;
            var nextNewIndex = smallMeta.IndexOf("\n", curIndex) - curIndex;
            if (nextNewIndex > 0) smallMeta = smallMeta.Remove(curIndex, nextNewIndex);
            smallMeta = smallMeta.Insert(curIndex, spriteNames[i].name);
            curIndex = smallMeta.IndexOf("\n", curIndex);
        }
        meta = meta.Replace(oldMeta, smallMeta);
        File.WriteAllText(path, meta);
    }
}
#endif