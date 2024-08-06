using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class colourTalkScript : MonoBehaviour
{

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMBombModule Module;
    public KMRuleSeedable RuleSeedable;

    public KMSelectable roundButton;
    public KMSelectable submitButton;
    public TextMesh screenText;
    public GameObject arrow;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    int selectedPhrase;
    int solutionColor;
    int textColor;
    string literalPhrase;
    string wordWrappedPhrase;
    private readonly List<float> arrowRot = new List<float> { 0f, 21.18f, 42.35f, 63.53f, 84.71f, 105.88f, 127.06f, 148.24f, 169.41f, 190.59f, 211.76f, 232.94f, 254.12f, 275.29f, 296.47f, 317.65f, 338.82f };
    private readonly List<char> colorLetters = new List<char> { 'S', 'R', 'O', 'Y', 'H', 'L', 'G', 'C', 'B', 'V', 'M', 'P', 'N', 'W', 'A', 'K', 'X' };
    private readonly List<string> colorNames = new List<string> { "Standard", "Red", "Orange", "Yellow", "Chartreuse", "Lime", "Green", "Cyan", "Blue", "Violet", "Magenta", "Pink", "Brown", "White", "Gray", "Black", "Clear" };
    public Color[] textColorOptions;
    int selectedColor;
    private ColourTalkSettings Settings = new ColourTalkSettings();
    string[] preventedTerms;
    private int[] _numArrayForRS;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        ModConfig<ColourTalkSettings> modConfig = new ModConfig<ColourTalkSettings>("ColourTalkSettings");
        //Read from the settings file, or create one if one doesn't exist
        Settings = modConfig.Settings;
        //Update the settings file incase there was an error during read
        modConfig.Settings = Settings;
        if (Settings.preventTerms.Contains(';'))
            preventedTerms = Settings.preventTerms.Split(';');
        else
            preventedTerms = new string[] { Settings.preventTerms };
        roundButton.OnInteract += delegate () { PressRoundButton(); return false; };
        submitButton.OnInteract += delegate () { PressSubmitButton(); return false; };
    }

    // Use this for initialization
    void Start()
    {

        textColor = UnityEngine.Random.Range(0, 17);
        var rnd = RuleSeedable.GetRNG();
        _numArrayForRS = Enumerable.Range(0, 1000).Select(i => i % 17).ToArray();
        if (rnd.Seed != 1)
            rnd.ShuffleFisherYates(_numArrayForRS);

        redo:
        selectedPhrase = UnityEngine.Random.Range(0, 1000);
        solutionColor = _numArrayForRS[selectedPhrase];
        literalPhrase = phraseList.phrases[selectedPhrase];
        if (preventedTerms[0] != string.Empty || preventedTerms.Length != 1)
        {
            for (int i = 0; i < preventedTerms.Length; i++)
            {
                if (literalPhrase.ToLower().Contains(preventedTerms[i].ToLower()))
                    goto redo;
            }
        }
        WordWrap();
        screenText.text = wordWrappedPhrase;
        screenText.color = textColorOptions[textColor];
        Debug.LogFormat("[Colo(u)r Talk #{0}] The phrase is: \"{1}\"", moduleId, literalPhrase.Replace("\n", "").Replace("  ", " ").Replace("ç", ":eyes:").Replace("Ç", ":orange:"));
        Debug.LogFormat("[Colo(u)r Talk #{0}] The solution colo(u)r is: {1} ({2}).", moduleId, colorNames[solutionColor], colorLetters[solutionColor]);
    }

    void PressRoundButton()
    {
        roundButton.AddInteractionPunch();
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        if (moduleSolved != true)
        {
            var prevColor = selectedColor;
            selectedColor = (selectedColor + 1) % 17;
            StartCoroutine(TurnArrow(prevColor, selectedColor));
        }
    }

    private IEnumerator TurnArrow(int prevc, int selc)
    {
        var elapsed = 0f;
        var duration = .5f;
        var startRotation = arrow.transform.localRotation;
        var endRotation = Quaternion.Euler(-90f, 0f, arrowRot[selc]);
        while (elapsed < duration)
        {
            arrow.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsed / duration);
            yield return null;
            elapsed += Time.deltaTime;
        }
        arrow.transform.localRotation = endRotation;
    }

    void PressSubmitButton()
    {
        submitButton.AddInteractionPunch();
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        if (moduleSolved != true)
        {
            if (selectedColor == solutionColor)
            {
                Debug.LogFormat("[Colo(u)r Talk #{0}] Selected colo(u)r is {1} ({2}), which is correct. Module solved.", moduleId, colorNames[selectedColor], colorLetters[selectedColor]);
                Module.HandlePass();
                moduleSolved = true;
            }
            else
            {
                Debug.LogFormat("[Colo(u)r Talk #{0}] Selected colo(u)r is {1} ({2}), which is incorrect. Module striked.", moduleId, colorNames[selectedColor], colorLetters[selectedColor]);
                Module.HandleStrike();
            }
        }
    }

    void WordWrap()
    {
        string newPhrase = "";
        int letterCount = 0;
        for (int i = 0; i < literalPhrase.Length; i++)
        {
            letterCount = letterCount + 1;
            if (literalPhrase[i] == ' ' && letterCount >= 12)
            {
                newPhrase += "\n";
                letterCount = 0;
            }
            else
            {
                newPhrase += literalPhrase[i];
            }
        }
        wordWrappedPhrase = newPhrase;
    }

    //twitch plays
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} submit <colo(u)r> [Submits the specified colo(u)r] | Valid colo(u)rs are S, R, O, Y, H, L, G, C, B, V, M, P, N, W, A, K, X.";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (parameters.Length > 2)
            {
                yield return "sendtochaterror Too many parameters!";
            }
            else if (parameters.Length == 2)
            {
                string color = parameters[1];
                var ix = colorNames.FindIndex(cn => cn.EqualsIgnoreCase(color));
                if (ix == -1)
                    ix = colorLetters.FindIndex(cn => cn.ToString().EqualsIgnoreCase(color));
                if (ix == -1)
                {
                    yield return "sendtochaterror!f '" + parameters[1] + "' is not an option on my colo(u)r wheel!";
                    yield break;
                }
                while (selectedColor != ix)
                {
                    roundButton.OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }
                submitButton.OnInteract();
            }
            else if (parameters.Length == 1)
            {
                yield return "sendtochaterror Please specify the colo(u)r you wish to submit!";
            }
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        yield return null;
        while (selectedColor != solutionColor)
        {
            roundButton.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        submitButton.OnInteract();
    }

    class ColourTalkSettings
    {
        public string preventTerms = "";
    }

    static Dictionary<string, object>[] TweaksEditorSettings = new Dictionary<string, object>[]
    {
        new Dictionary<string, object>
        {
            { "Filename", "ColourTalkSettings.json" },
            { "Name", "Colo(u)r Talk Settings" },
            { "Listing", new List<Dictionary<string, object>>{
                new Dictionary<string, object>
                {
                    { "Key", "preventTerms" },
                    { "Text", "Will prevent phrases generating with any of these terms. Example: 'alpha;bravo;charlie'" }
                },
            } }
        }
    };
}