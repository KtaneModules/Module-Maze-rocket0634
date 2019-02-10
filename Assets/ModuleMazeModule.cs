using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleMazeModule : MonoBehaviour
{
    private static int _moduleIDCounter = 1;
    private int _moduleID;
    public KMBombInfo Info;
    public KMBombModule Module;
    public KMAudio Audio;
    public KMRuleSeedable RuleSeedable;
    public SpriteRenderer IconHolder, IconHolder2;
    public Sprite[] sprites, gSprites, souvenirSprites;
    public KMSelectable[] Buttons;
    public TextAsset selections;
    public string souvenirStart = "empty", version;
    internal Dictionary<string, ModuleInfo> assignments = new Dictionary<string, ModuleInfo>();
    private Dictionary<int, string> orderedInfo;

    //The index value for the starting bomb
    //The index value for the destination bomb
    public int x, y, yMin, size, pixelsPerUnit;
    private int start, destination;
    private int[] phonies = new int[5];
    float t;
    private Queue<IEnumerable> queue = new Queue<IEnumerable>();
    //Keep track of when the module is processing an input (don't process any others while ready is false) |
    //Don't allow interactions when !_isActive or solved 
    private bool ready = true, _isActive = false, solved, showSolution = true, first = true;

    // Use this for initialization
    void Start()
    {
        _moduleID = _moduleIDCounter++;
        var text = selections.text;
        var settingsPath = System.IO.Path.Combine(Application.persistentDataPath, "Modsettings");
        var path = System.IO.Path.Combine(settingsPath, "ModuleMazeConnections.txt");
        var path2 = System.IO.Path.Combine(settingsPath, "ModuleMazeVersion.txt");
        if (!System.IO.File.Exists(path) || !System.IO.File.Exists(path2) || System.IO.File.ReadAllText(path2) != version)
        {
            System.IO.File.WriteAllText(path, selections.text);
            System.IO.File.WriteAllText(path, version);
        }
        else if (System.IO.File.ReadAllText(path2) == version) text = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Path.Combine(Application.dataPath, "Modsettings"), "ModuleMaze.txt"));
        TextReader.Run(this, text);
        orderedInfo = assignments.Where(x => x.Value.index != -1).ToDictionary(x => x.Value.index + 1, y => y.Key);
        /*if (RuleSeedable.GetRNG().Seed != 1)
        {
            
        } else {*/
            start = UnityEngine.Random.Range(1, sprites.Length);
            Func<int, bool> func = (z) => z == start || !Distance();
            Func<int, bool> func2 = (z) => z % 20 == 0;
            destination = UnityEngine.Random.Range(1, sprites.Length);
            while (func2(start)) start = UnityEngine.Random.Range(1, sprites.Length);
            while (func2(destination) || func(destination)) destination = UnityEngine.Random.Range(1, sprites.Length);
            for (int i = 0; i < phonies.Count(); i++)
            {
                while (func2(phonies[i]) || phonies[i] == start || (i > 0) && phonies.Take(i).Contains(phonies[i]))
                    phonies[i] = UnityEngine.Random.Range(1, sprites.Length);
            }
        //}
        IconHolder.sprite = sprites[destination];
        for (int i = 0; i < Buttons.Length; i++)
        {
            int j = i;
            Buttons[i].OnInteract = ButtonHandler(j);
        }
        Module.OnActivate += delegate ()
        {
            DebugLog("Expected icon is {0}", sprites[destination].name);
            _isActive = true;
        };
        StartCoroutine(WaitForInput());
    }

    bool Distance()
    {
        var current = orderedInfo[start];
        var endingKey = orderedInfo[destination];
        if (!Loop(current, 0, true, endingKey, new List<string>())) return true;
        else return false;
    }

    bool Loop(string current, int count, bool doCount, string end, List<string> path)
    {
        for (int i = 0; i < 4; i++)
        {
            var next = assignments[current].connections[i];
            if (next != null && ((doCount && count < 4) || !doCount) && next != end && !path.Contains(next))
            {
                if (!path.Contains(current)) path.Add(current);
                if (Loop(next, count + 1, doCount, end, path)) return true;
            }
            else if (next == end)
            {
                DebugLog("Distance is {0} movements", count);
                return true;
            }
            else continue;
        }
        return false;
    }

    KMSelectable.OnInteractHandler ButtonHandler(int i)
    {
        return delegate ()
        {
            //if (solved) return false;
            /* Reset isn't necessary here.
             * if (i == 4 && !showSolution)
            {
                var coroutine = Count();
                StartCoroutine(coroutine);
                Buttons[4].OnInteractEnded = delegate
                {
                    StopCoroutine(coroutine);
                    if (t < 2) queue.Enqueue(ButtonPress(4));
                    Buttons[4].OnInteractEnded = null;
                };
                return false;
            }*/
            if (!_isActive || solved) return false;
            Buttons[i].AddInteractionPunch(0.5f);
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            queue.Enqueue(ButtonPress(i));
            return false;
        };
    }

    private IEnumerator Count()
    {
        t = 0f;
        while (isActiveAndEnabled)
        {
            yield return t += Time.deltaTime;
            if (t > 2.0f) break;
        }
        queue.Enqueue(ButtonPress(5));
    }

    private IEnumerator WaitForInput()
    {
        do
        {
            yield return null;
            if (queue.Count > 0)
            {
                IEnumerable press = queue.Dequeue();
                foreach (object item in press) yield return item;
            }
        }
        while (ready);
    }

    IEnumerable ButtonPress(int i)
    {
        var oP = IconHolder.transform.localPosition;
        var cP = start;
        var rP = IconHolder.transform.localPosition;
        var move = "";
        ready = false;
        var strike = true;
        if (showSolution && i != 4)
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, transform);
            ready = true;
            yield break;
        }
        switch (i)
        {
            case 0:
                move = "top";
                oP.z -= 1.1f;
                rP.z += 1.1f;
                if (assignments[orderedInfo[start]].connections[3] != null)
                {
                    start -= y + 1;
                    strike = false;
                }
                break;
            case 1:
                move = "right";
                oP.x -= 1.1f;
                rP.x += 1.1f;
                if (assignments[orderedInfo[start]].connections[2] != null)
                {
                    start++;
                    strike = false;
                }
                break;
            case 2:
                move = "bottom";
                oP.z += 1.1f;
                rP.z -= 1.1f;
                if (assignments[orderedInfo[start]].connections[1] != null)
                {
                    start += y + 1;
                    strike = false;
                }
                break;
            case 3:
                move = "left";
                oP.x += 1.1f;
                rP.x -= 1.1f;
                if (assignments[orderedInfo[start]].connections[0] != null)
                {
                    start--;
                    strike = false;
                }
                break;
            case 4:
                if (start == destination && !showSolution)
                {
                    solved = true;
                    if (souvenirStart.Contains("squares"))
                    {
                        var squares = gSprites.Where(x => x.name.Contains("squares") && x.name != souvenirStart);
                        souvenirSprites = new[] { gSprites.Where(x => x.name == souvenirStart).First() }.Concat(squares).ToArray();
                    }
                    else
                        souvenirSprites = new[] { gSprites.Where(x => x.name == souvenirStart).First(), gSprites.Where(x => x.name == sprites[phonies[0]].name).First(), gSprites.Where(x => x.name == sprites[phonies[1]].name).First(), gSprites.Where(x => x.name == sprites[phonies[2]].name).First(), gSprites.Where(x => x.name == sprites[phonies[3]].name).First(), gSprites.Where(x => x.name == sprites[phonies[4]].name).First() };
                    Module.HandlePass();
                }
                else if (showSolution)
                {
                    IconHolder.sprite = sprites[start];
                    DebugLog("Shown icon is {0}", sprites[start].name);
                    if (first)
                    {
                        souvenirStart = sprites[start].name;
                        first = !first;
                    }
                    showSolution = !showSolution;
                    ready = true;
                }
                else
                {
                    Module.HandleStrike();
                    DebugLog("Strike obtained, showing solution icon.");
                    IconHolder.sprite = sprites[destination];
                    showSolution = !showSolution;
                    ready = true;
                }
                break;
            /*case 5:
                var coroutine = AutoSolve(start, true);
                while (coroutine.MoveNext())
                    yield return coroutine.Current;
                break;*/
        }
        if (i > 3) yield break;
        yield return MoveScreen(oP, rP, cP, move, strike);
        yield return null;
    }

    IEnumerator MoveScreen(Vector3 bh1, Vector3 bh2O, int oP, string m, bool strike = false)
    {
        var t = 0.0f;
        var duration = 0.25f;
        IconHolder2.transform.localPosition = bh2O;
        IconHolder2.sprite = sprites[20];
        var b = IconHolder.transform.localPosition;
        if (strike)
        {
            duration /= 2;
            bh1.x /= 2;
            bh1.z /= 2;
            bh2O.x /= 2;
            bh2O.z /= 2;
            while (t < duration)
            {
                yield return null;
                t = Mathf.Min(t + Time.deltaTime, duration);
                IconHolder.transform.localPosition = Vector3.Lerp(b, bh1, Mathf.SmoothStep(0.0f, 1.0f, t / duration));
                IconHolder2.transform.localPosition = Vector3.Lerp(bh2O, b, Mathf.SmoothStep(0.0f, 1.0f, t / duration));
            }
            Module.HandleStrike();
            //Clear the queue for TP Compatibility
            queue.Clear();
            DebugLog("Wall detected to the {1} from {0}", sprites[start].name, m);
            t = 0;
            while (t < duration)
            {
                yield return null;
                t = Mathf.Min(t + Time.deltaTime, duration);
                IconHolder.transform.localPosition = Vector3.Lerp(bh1, b, Mathf.SmoothStep(0.0f, 1.0f, t / duration));
                IconHolder2.transform.localPosition = Vector3.Lerp(b, bh2O, Mathf.SmoothStep(0.0f, 1.0f, t / duration));
            }
            ready = true;
            yield break;
        }
        var move = m;
        switch (m)
        {
            case "top":
                move = "up";
                break;
            case "bottom":
                move = "down";
                break;
        }
        IconHolder2.sprite = sprites[start];
        while (t < duration)
        {
            yield return null;
            t = Mathf.Min(t + Time.deltaTime, duration);
            IconHolder.transform.localPosition = Vector3.Lerp(b, bh1, Mathf.SmoothStep(0.0f, 1.0f, t / duration));
            IconHolder2.transform.localPosition = Vector3.Lerp(bh2O, b, Mathf.SmoothStep(0.0f, 1.0f, t / duration));
        }
        DebugLog("Moved {0} from icon [{1}] to [{2}]", move, sprites[oP].name, sprites[start].name);
        IconHolder.sprite = sprites[start];
        IconHolder.transform.localPosition = new Vector3(0, 0.55f, 0);
        IconHolder2.transform.localPosition = bh2O;
        yield return null;
        ready = true;
    }

    bool StartsWithOrEndsWithAny(string source, params string[] strings)
    {
        foreach (string str in strings)
        {
            if (source.StartsWith(str) || source.EndsWith(str)) return true;
        }
        return false;
    }

    string ReplaceAll(string source, string result, params string[] strings)
    {
        foreach (string str in strings)
        {
            source = source.Replace(str, result);
        }
        return source;
    }

    private string TwitchHelpMessage = "Interact with the module using !{0} udlr NSEW, and use !{0} toggle to interact with the screen.";

    private IEnumerator ProcessTwitchCommand(string input)
    {
        input = input.ToLowerInvariant();
        var submit = false;
        var presses = new List<KMSelectable>();
        /* You'll always know where you are in the maze, as such there is no need for a reset.
         * if (StartsWithOrEndsWithAny(input, "reset"))
        {
            yield return null;
            Buttons.Last().OnInteract();
            yield return new WaitForSeconds(2.1f);
            Buttons.Last().OnInteractEnded();
            yield break;
        }*/
        if (StartsWithOrEndsWithAny(input, "submit", "select", "toggle"))
        {
            Debug.LogFormat(input);
            submit = true;
            input = ReplaceAll(input, "", "submit", "select", "toggle");
        }
        input = input.Replace("press", "");
        foreach (char c in input)
        {
            switch (c)
            {
                case 'u':
                case 'n':
                    presses.Add(Buttons[0]);
                    break;
                case 'r':
                case 'e':
                    presses.Add(Buttons[1]);
                    break;
                case 'd':
                case 's':
                    presses.Add(Buttons[2]);
                    break;
                case 'l':
                case 'w':
                    presses.Add(Buttons[3]);
                    break;
                case ' ':
                    break;
                default:
                    yield break;
            }
        }
        if (submit) presses.Add(Buttons[4]);
        yield return null;
        yield return presses.ToArray();
        //Focus on the module until the queue is empty
        //This is so solves and strikes are detected properly
        yield return new WaitUntil(() => queue.Count == 0);
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        yield return new WaitUntil(() => _isActive || solved);
        StartCoroutine(AutoSolve(destination));
        yield return null;
    }

    IEnumerator AutoSolve(int curDest)
    {
        //if (!reset) 
        yield return new WaitForSeconds((_moduleID - 1) * 0.5f % 10);
        if (showSolution) Buttons[4].OnInteract();
        var curLoc = start;
        var y = start;
        var step = 0;
        if (curLoc == curDest)// && !reset)
        {
            Buttons[4].OnInteract();
            yield break;
        }
        /*else if (curLoc == curDest)
        {
            DebugLog("You reset from your current location to your current location.");
            ready = true;
            queue.Clear();
            yield break;
        }*/
        var list = new List<string>();
        var end = false;
        var str = "";
        var direction = 0;
        var dirCount = 0;
        list.Add("[0, -1, " + curLoc + ", 0]");
        var func = new Func<int>[] { () => y--, () => y += this.y + 1, () => y++, () => y -= this.y + 1 };
        while (!end)
        {
            var curStep = step;
            if (assignments[orderedInfo[y]].connections[direction] != null)
            {
                var t = str + direction;
                if (!Contains(curLoc, t))
                {
                    str = t;
                    step++;
                    func[direction]();
                    list.Add(string.Format("[{0}, {1}, {2}, {3}]", str, step, y, dirCount));
                    direction = 0;
                }
                else direction++;
            }
            else
                direction++;
            if (direction > 3 && curStep == step && step != 0)
            {
                while (direction > 3 && step > 0)
                {
                    step--;
                    func[(str.Last() - '0' + 2) % 4]();
                    direction = str.Last() - '0' + 1;
                    str = str.Length > 1 ? str.Substring(0, str.Count() - 1) : "";
                }
            }
            if (direction > 3 && str.Length == 0)
                end = true;
            direction %= 4;
            /* Hopefully this won't be needed.
             * dirCount++;
            if (dirCount > 1000)
            {
                DebugLog("Error.", false);
                DebugLog(str, false);
                DebugLog("{0} - {1}", false, curLoc, start);
                DebugLog(string.Join("\n", list.ToArray()), false);
                yield break;
            }*/
        }
        var find = list.Where(x => {
            var test = x.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(test[2]) == curDest;
            }).OrderBy(x => {
                var test = x.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return int.Parse(test[1]);
            });
        var split = find.ToList()[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0].Replace("[","");
        //Freezes in test harness and then moves without moving. Seeing if this might help.
        yield return null;
        foreach (char c in split)
        {
            var notC = c - '0';
            var selectables = new[] { Buttons[3], Buttons[2], Buttons[1], Buttons[0] };
            ready = true;
            selectables[notC].OnInteract();
        }
        /*if (reset)
        {
            DebugLog("Returned to starting location.");
            ready = true;
            yield break;
        }*/
        yield return new WaitUntil(() => queue.Count == 0 && ready);
        if (curDest != destination)
        {
            StartCoroutine(AutoSolve(destination));
            yield break;
        }
        Buttons[4].OnInteract();
        //Buttons[4].OnInteractEnded();
    }

    bool Contains(int curLoc, string str)
    {
        var func = new Func<int>[] { () => curLoc--, () => curLoc += y + 1, () => curLoc++, () => curLoc -= y + 1 };
        var list = new List<int> { curLoc };
        foreach (int num in str.ToCharArray().Select(x => x - '0'))
        {
            func[num]();
            if (list.Contains(curLoc))
            {
                curLoc = list[0];
                return true;
            }
            list.Add(curLoc);
        }
        return false;
    }

    void DebugLog(string log, params object[] args)
    {
        DebugLog(log, true, args);
    }

    void DebugLog(string log, bool show, params object[] args)
    {
        var logData = string.Format(log, args);
        var name = Module.ModuleDisplayName + " #" + _moduleID;
        var showText = show ? string.Format("[{0}]", name) : string.Format("<{0}>", name);
        Debug.LogFormat("{0} {1}", showText, logData);
    }
}

public class ModuleInfo
{
    public Sprite sprite;
    public int index = -1;
    public string[] connections;
}
