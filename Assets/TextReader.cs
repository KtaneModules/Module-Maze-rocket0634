using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using UnityEngine;

static class TextReader {
    
    internal static void Run(ModuleMazeModule ModuleMaze, string selections)
    {
        //Select the text to read
        var text = selections;
        //Split at every newline
        var Selections = text.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < Selections.Count(); i++)
        {
            var space = Selections[i].IndexOf(" ");
            var leftPar = Selections[i].IndexOf("(");
            var rightPar = Selections[i].IndexOf(")");
            var parLength = rightPar - leftPar;
            var code = Selections[i].Substring(0, space);
            var name = Selections[i].Substring(leftPar + 1, parLength - 1);
            var selection = Selections[i].Replace(code + " (" + name + ")", "");
            var split = new string[] { };
            ModuleMaze.sprites[i + 1].name = name;
            var moduleInfo = new ModuleInfo();
            if (!ModuleMaze.assignments.ContainsKey(code))
            {
                moduleInfo = new ModuleInfo { connections = new string[4] };
                ModuleMaze.assignments.Add(code, moduleInfo);
            }
            else
                moduleInfo = ModuleMaze.assignments[code];
            moduleInfo.index = i;
            moduleInfo.sprite = ModuleMaze.sprites[i + 1];
            if (selection.Length > 0)
            {
                selection = selection.Replace(" ", "");
                split = selection.Split(new[] { ',' });
                for (int j = 0; j < split.Length; j++)
                {
                    if (split[j].Length < 1) continue;
                    //Only require down and right inputs [Left is assigned by right inputs]
                    //Left is index 0, Down is index 1, and Right is index 2, since entries have a length of 2, add one to each entry so indicies match.
                    if (!moduleInfo.connections.Contains(split[j])) moduleInfo.connections[j + 1] = split[j];
                    if (!ModuleMaze.assignments.ContainsKey(split[j])) ModuleMaze.assignments.Add(split[j], new ModuleInfo { connections = new string[4] });
                    //Add 2 and mod 4 to get the opposite direction.
                    if (!ModuleMaze.assignments[split[j]].connections.Contains(code)) ModuleMaze.assignments[split[j]].connections[(j + 3) % 4] = code;
                }
            }
        }
    }

    internal static void FindImage()
    {
        var path = Application.persistentDataPath;
        var applicable = Directory.GetFiles(path).Where(x => x.EndsWith("ktane.png"));
        var Textures = new List<Texture2D>();
        var GrayTextures = new List<Texture2D>();
        if (applicable.Count() > 0)
        {
            foreach (string p in applicable)
            {
                var tex = new Texture2D(2, 2);
                tex.LoadImage(File.ReadAllBytes(path));
                if (tex.width / 96f % 1 != 0 || tex.height / 96f % 1 != 0) continue;
                else if (tex.width / 32f % 1 != 0 || tex.height / 32f % 1 != 0) GrayTextures.Add(tex);
                else Textures.Add(tex);
            }
        }
    }
}
