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
    public Sprite[] sprites;
    public KMSelectable[] Buttons;
    public string version;
#pragma warning disable IDE0052 // Read by Souvenir
    private Sprite souvenirStart;
#pragma warning restore IDE0052

    private static readonly int[][] connections = new int[][]
    {
        new int[] { 1 },
        new int[] { 21 },
        new int[] { 3, 22 },
        new int[] {  },
        new int[] { 5, 24 },
        new int[] {  },
        new int[] { 7 },
        new int[] { 8 },
        new int[] { 9, 28 },
        new int[] {  },
        new int[] { 11, 30 },
        new int[] { 12, 31 },
        new int[] { 13 },
        new int[] { 14 },
        new int[] { 15 },
        new int[] { 16, 35 },
        new int[] {  },
        new int[] { 18, 37 },
        new int[] { 19 },
        new int[] {  },
        new int[] { 21, 40 },
        new int[] { 41 },
        new int[] { 23, 42 },
        new int[] {  },
        new int[] { 25 },
        new int[] { 26 },
        new int[] { 46 },
        new int[] { 47 },
        new int[] { 29 },
        new int[] { 30, 49 },
        new int[] {  },
        new int[] { 51 },
        new int[] { 33, 52 },
        new int[] { 34 },
        new int[] {  },
        new int[] { 36, 55 },
        new int[] {  },
        new int[] { 38 },
        new int[] { 39, 58 },
        new int[] {  },
        new int[] { 60 },
        new int[] { 42 },
        new int[] { 43 },
        new int[] { 44 },
        new int[] { 45, 64 },
        new int[] { 65 },
        new int[] { 47, 66 },
        new int[] { 67 },
        new int[] { 49, 68 },
        new int[] {  },
        new int[] { 51, 70 },
        new int[] { 52 },
        new int[] { 53 },
        new int[] { 54, 73 },
        new int[] { 55, 74 },
        new int[] {  },
        new int[] { 57, 76 },
        new int[] { 77 },
        new int[] { 59, 78 },
        new int[] {  },
        new int[] { 61, 80 },
        new int[] { 62 },
        new int[] { 63, 82 },
        new int[] { 64 },
        new int[] {  },
        new int[] { 66 },
        new int[] {  },
        new int[] { 68, 87 },
        new int[] {  },
        new int[] { 89 },
        new int[] {  },
        new int[] { 72, 91 },
        new int[] { 73 },
        new int[] {  },
        new int[] { 75, 94 },
        new int[] { 76 },
        new int[] {  },
        new int[] { 78 },
        new int[] { 98 },
        new int[] { 99 },
        new int[] { 100 },
        new int[] { 101 },
        new int[] { 102 },
        new int[] { 84, 103 },
        new int[] {  },
        new int[] { 105 },
        new int[] { 87 },
        new int[] { 107 },
        new int[] { 89, 108 },
        new int[] { 90, 109 },
        new int[] { 110 },
        new int[] { 111 },
        new int[] { 93, 112 },
        new int[] { 94 },
        new int[] {  },
        new int[] { 96, 115 },
        new int[] { 97, 116 },
        new int[] {  },
        new int[] { 118 },
        new int[] { 119 },
        new int[] { 120 },
        new int[] { 121 },
        new int[] { 122 },
        new int[] { 104 },
        new int[] { 105 },
        new int[] { 106 },
        new int[] { 107, 126 },
        new int[] { 127 },
        new int[] { 128 },
        new int[] {  },
        new int[] { 111, 130 },
        new int[] { 131 },
        new int[] { 113, 132 },
        new int[] {  },
        new int[] { 115, 134 },
        new int[] {  },
        new int[] { 117 },
        new int[] {  },
        new int[] { 119 },
        new int[] { 139 },
        new int[] { 140 },
        new int[] { 141 },
        new int[] { 142 },
        new int[] { 124, 143 },
        new int[] { 125 },
        new int[] { 145 },
        new int[] { 146 },
        new int[] { 128 },
        new int[] { 129, 148 },
        new int[] { 149 },
        new int[] { 150 },
        new int[] { 151 },
        new int[] { 133, 152 },
        new int[] {  },
        new int[] { 135, 154 },
        new int[] {  },
        new int[] { 156 },
        new int[] { 157 },
        new int[] { 139, 158 },
        new int[] {  },
        new int[] { 141, 160 },
        new int[] {  },
        new int[] { 143 },
        new int[] { 144, 163 },
        new int[] {  },
        new int[] { 146 },
        new int[] {  },
        new int[] { 148, 167 },
        new int[] {  },
        new int[] { 169 },
        new int[] { 170 },
        new int[] { 152 },
        new int[] { 153, 172 },
        new int[] {  },
        new int[] { 155, 174 },
        new int[] { 156, 175 },
        new int[] { 157 },
        new int[] { 158 },
        new int[] { 159, 178 },
        new int[] { 179 },
        new int[] { 161 },
        new int[] { 181 },
        new int[] { 163 },
        new int[] { 164 },
        new int[] { 184 },
        new int[] { 166, 185 },
        new int[] { 167 },
        new int[] {  },
        new int[] { 169, 188 },
        new int[] {  },
        new int[] { 190 },
        new int[] { 172, 191 },
        new int[] { 173 },
        new int[] { 174 },
        new int[] {  },
        new int[] { 176 },
        new int[] { 177 },
        new int[] { 197 },
        new int[] {  },
        new int[] { 199 },
        new int[] { 181 },
        new int[] { 182, 201 },
        new int[] { 202 },
        new int[] { 184, 203 },
        new int[] { 185, 204 },
        new int[] { 186 },
        new int[] { 206 },
        new int[] { 188 },
        new int[] { 189 },
        new int[] { 209 },
        new int[] { 191 },
        new int[] { 211 },
        new int[] { 212 },
        new int[] { 194, 213 },
        new int[] {  },
        new int[] { 196, 215 },
        new int[] { 197 },
        new int[] { 198, 217 },
        new int[] { 218 },
        new int[] { 219 },
        new int[] { 201 },
        new int[] { 221 },
        new int[] { 222 },
        new int[] { 223 },
        new int[] { 205, 224 },
        new int[] {  },
        new int[] { 207 },
        new int[] { 208, 227 },
        new int[] { 209 },
        new int[] { 210 },
        new int[] { 211 },
        new int[] { 212, 231 },
        new int[] { 213 },
        new int[] { 214, 233 },
        new int[] { 215 },
        new int[] {  },
        new int[] { 217, 236 },
        new int[] {  },
        new int[] { 238 },
        new int[] { 239 },
        new int[] { 240 },
        new int[] { 241 },
        new int[] { 223 },
        new int[] {  },
        new int[] { 225 },
        new int[] { 245 },
        new int[] { 246 },
        new int[] { 247 },
        new int[] { 229 },
        new int[] { 230, 249 },
        new int[] {  },
        new int[] { 251 },
        new int[] { 252 },
        new int[] { 234 },
        new int[] {  },
        new int[] { 255 },
        new int[] { 237, 256 },
        new int[] { 257 },
        new int[] { 239 },
        new int[] { 259 },
        new int[] { 241, 260 },
        new int[] { 261 },
        new int[] { 262 },
        new int[] { 244 },
        new int[] { 245 },
        new int[] { 246 },
        new int[] { 247 },
        new int[] { 267 },
        new int[] { 268 },
        new int[] { 250, 269 },
        new int[] { 251 },
        new int[] { 252, 271 },
        new int[] { 272 },
        new int[] { 254, 273 },
        new int[] {  },
        new int[] { 256, 275 },
        new int[] {  },
        new int[] { 277 },
        new int[] { 278 },
        new int[] { 279 },
        new int[] { 280 },
        new int[] { 262 },
        new int[] { 263, 282 },
        new int[] { 264 },
        new int[] { 284 },
        new int[] { 266 },
        new int[] { 267, 286 },
        new int[] { 287 },
        new int[] { 269, 288 },
        new int[] {  },
        new int[] { 271 },
        new int[] {  },
        new int[] { 292 },
        new int[] { 274, 293 },
        new int[] {  },
        new int[] { 276, 295 },
        new int[] { 296 },
        new int[] { 297 },
        new int[] { 279, 298 },
        new int[] {  },
        new int[] { 281 },
        new int[] { 282 },
        new int[] { 283, 302 },
        new int[] { 284 },
        new int[] { 304 },
        new int[] { 305 },
        new int[] { 306 },
        new int[] { 307 },
        new int[] { 308 },
        new int[] { 309 },
        new int[] { 291, 310 },
        new int[] { 292 },
        new int[] {  },
        new int[] { 294, 313 },
        new int[] { 295 },
        new int[] {  },
        new int[] { 316 },
        new int[] {  },
        new int[] { 299, 318 },
        new int[] {  },
        new int[] { 301 },
        new int[] { 302 },
        new int[] { 322 },
        new int[] { 304 },
        new int[] { 305 },
        new int[] { 306, 325 },
        new int[] { 326 },
        new int[] { 308 },
        new int[] { 328 },
        new int[] { 310, 329 },
        new int[] { 311, 330 },
        new int[] { 312, 331 },
        new int[] { 313 },
        new int[] {  },
        new int[] { 315, 334 },
        new int[] { 316 },
        new int[] { 336 },
        new int[] { 318, 337 },
        new int[] { 319 },
        new int[] { 339 },
        new int[] { 321 },
        new int[] { 322, 341 },
        new int[] {  },
        new int[] { 324, 343 },
        new int[] { 325 },
        new int[] {  },
        new int[] { 327 },
        new int[] { 347 },
        new int[] { 329 },
        new int[] { 349 },
        new int[] { 350 },
        new int[] { 332 },
        new int[] { 333 },
        new int[] { 334 },
        new int[] { 335 },
        new int[] { 355 },
        new int[] { 337 },
        new int[] { 338 },
        new int[] { 358 },
        new int[] {  },
        new int[] { 341 },
        new int[] { 361 },
        new int[] { 343 },
        new int[] { 344 },
        new int[] { 345, 364 },
        new int[] { 346 },
        new int[] { 347 },
        new int[] { 348, 367 },
        new int[] { 349 },
        new int[] { 369 },
        new int[] { 370 },
        new int[] { 371 },
        new int[] { 353, 372 },
        new int[] { 354 },
        new int[] {  },
        new int[] { 375 },
        new int[] { 357, 376 },
        new int[] { 377 },
        new int[] { 359, 378 },
        new int[] {  },
        new int[] { 361 },
        new int[] { 362 },
        new int[] { 363, 382 },
        new int[] { 364 },
        new int[] { 365 },
        new int[] { 366, 385 },
        new int[] { 386 },
        new int[] { 387 },
        new int[] { 369, 388 },
        new int[] { 389 },
        new int[] { 390 },
        new int[] { 372, 391 },
        new int[] { 373 },
        new int[] { 374 },
        new int[] {  },
        new int[] { 395 },
        new int[] { 396 },
        new int[] { 378 },
        new int[] { 379 },
        new int[] { 399 },
        new int[] { 381 },
        new int[] { 382 },
        new int[] {  },
        new int[] { 384 },
        new int[] { 385 },
        new int[] {  },
        new int[] { 387 },
        new int[] {  },
        new int[] {  },
        new int[] { 390 },
        new int[] { 391 },
        new int[] { 392 },
        new int[] { 393 },
        new int[] { 394 },
        new int[] { 395 },
        new int[] { 396 },
        new int[] { 397 },
        new int[] {  },
        new int[] { 399 },
        new int[] {  }
    };

    public int y, size, pixelsPerUnit;
    //The index value for the starting bomb
    //The index value for the destination bomb
    private int start, destination;
    private readonly Queue<IEnumerable> queue = new Queue<IEnumerable>();
    //Keep track of when the module is processing an input (don't process any others while ready is false) |
    //Don't allow interactions when !_isActive or solved
    private bool ready = true, _isActive = false, solved, showSolution = true, first = true;

    // Use this for initialization
    void Start()
    {
        _moduleID = _moduleIDCounter++;
        DebugLog("Version {0}", false, version);
        start = Random.Range(0, sprites.Length);
        souvenirStart = sprites[start];
        do
            destination = Random.Range(0, sprites.Length);
        while (destination == start || Loop(start, 0, 24, true, destination, new List<int>()));
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

    bool Loop(int current, int count, int countLimit, bool doCount, int end, List<int> path)
    {
        var adjacent = new List<int>();
        if (current % 20 > 0)
            adjacent.Add(current - 1);
        if (current % 20 < 19)
            adjacent.Add(current + 1);
        if (current / 20 > 0)
            adjacent.Add(current - 20);
        if (current / 20 < 19)
            adjacent.Add(current + 20);

        foreach (var next in adjacent)
        {
            if (((doCount && count < countLimit) || !doCount) && next != end && !path.Contains(next))
            {
                if (!path.Contains(current)) path.Add(current);
                if (Loop(next, count + 1, countLimit, doCount, end, path)) return true;
            }
            else if (next == end)
            {
                DebugLog("Distance is {0} movements", !doCount, count + 1);
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
            if (!_isActive || solved) return false;
            Buttons[i].AddInteractionPunch(0.5f);
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            queue.Enqueue(ButtonPress(i));
            return false;
        };
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
                if (start / 20 > 0 && connections[start - 20].Contains(start))
                {
                    start -= y;
                    strike = false;
                }
                break;
            case 1:
                move = "right";
                oP.x -= 1.1f;
                rP.x += 1.1f;
                if (start % 20 < 19 && connections[start].Contains(start + 1))
                {
                    start++;
                    strike = false;
                }
                break;
            case 2:
                move = "bottom";
                oP.z += 1.1f;
                rP.z -= 1.1f;
                if (start / 20 < 19 && connections[start].Contains(start + 20))
                {
                    start += y;
                    strike = false;
                }
                break;
            case 3:
                move = "left";
                oP.x += 1.1f;
                rP.x -= 1.1f;
                if (start % 20 > 0 && connections[start - 1].Contains(start))
                {
                    start--;
                    strike = false;
                }
                break;
            case 4:
                if (start == destination && !showSolution)
                {
                    solved = true;
                    Module.HandlePass();
                }
                else if (showSolution)
                {
                    IconHolder.sprite = sprites[start];
                    DebugLog("Shown icon is {0}", sprites[start].name);
                    if (first)
                        first = !first;
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
        IconHolder2.sprite = null;
        var b = IconHolder.transform.localPosition;
        if (strike)
        {
            duration /= 2;
            bh1.x /= 2;
            bh1.z /= 2;
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
        try
        {
            DebugLog("Moved {0} from icon [{1}] to [{2}]", move, sprites[oP].name, sprites[start].name);
        }
        catch
        {
            DebugLog("{0}, {1}", (sprites[oP] == null).ToString(), sprites[start] == null);
            DebugLog(sprites[oP].name + " " + sprites[start].name);
        }
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

#pragma warning disable 414
#pragma warning disable IDE0051
    private readonly string TwitchHelpMessage = "Interact with the module using !{0} udlr NSEW, and use !{0} toggle to interact with the screen.";

    private IEnumerator ProcessTwitchCommand(string input)
    {
        input = input.ToLowerInvariant();
        var submit = false;
        var presses = new List<KMSelectable>();
        // You'll always know where you are in the maze, as such there is no need for a reset.
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
        while (!ready)
            yield return true;
        var coroutine = AutoSolve(destination);
        while (coroutine.MoveNext())
        {
            yield return coroutine.Current;
            yield return true;
        }
    }

#pragma warning restore 414
#pragma warning restore IDE0051
    IEnumerator AutoSolve(int curDest)
    {
        if (showSolution) Buttons[4].OnInteract();
        yield return null;
        var curLoc = start;
        var step = 0;
        if (curLoc == curDest)
        {
            Buttons[4].OnInteract();
            yield break;
        }
        var end = false;
        var str = "";
        var movements = new Stack<int>();
        var direction = 0;
        var explored = new List<int>();
        var directions = new int[] { -1, y, 1, -y };
        while (!end)
        {
            var curStep = step;

            bool valid;
            switch (direction)
            {
                case 0: valid = curLoc % 20 > 0 && connections[curLoc - 1].Contains(curLoc); break;
                case 1: valid = curLoc / 20 < 19 && connections[curLoc].Contains(curLoc + 20); break;
                case 2: valid = curLoc % 20 < 19 && connections[curLoc].Contains(curLoc + 1); break;
                default: valid = curLoc / 20 > 0 && connections[curLoc - 20].Contains(curLoc); break;
            }

            if (valid && !explored.Contains(curLoc + directions[direction]))
            {
                if (!movements.Contains(curLoc + directions[direction]))
                {
                    str += direction;
                    movements.Push(curLoc);
                    step++;
                    curLoc += directions[direction];
                    if (curLoc == curDest)
                        end = true;
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
                    explored.Add(curLoc);
                    curLoc += directions[(str.Last() - '0' + 2) % 4];
                    direction = str.Last() - '0' + 1;
                    str = str.Length > 1 ? str.Substring(0, str.Count() - 1) : "";
                    movements.Pop();
                }
            }
            if (direction > 3 && str.Length == 0)
                end = true;
            direction %= 4;
        }
        foreach (char c in str)
        {
            var notC = c - '0';
            var selectables = new[] { Buttons[3], Buttons[2], Buttons[1], Buttons[0] };
            ready = true;
            yield return null;
            selectables[notC].OnInteract();
        }
        while (queue.Count != 0 || !ready)
            yield return true;
        if (curDest != destination)
        {
            var coroutine = AutoSolve(destination);
            while (coroutine.MoveNext())
                yield return coroutine.Current;
            yield break;
        }
        yield return null;
        Buttons[4].OnInteract();
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