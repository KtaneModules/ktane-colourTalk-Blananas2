using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class colourTalkScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;

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
    private List<float> arrowRot = new List<float> { 0f, 21.18f, 42.35f, 63.53f, 84.71f, 105.88f, 127.06f, 148.24f, 169.41f, 190.59f, 211.76f, 232.94f, 254.12f, 275.29f, 296.47f, 317.65f, 338.82f };
    private List<Char> colorLetters = new List<char> { 'S', 'R', 'O', 'Y', 'H', 'L', 'G', 'C', 'B', 'V', 'M', 'P', 'N', 'W', 'A', 'K', 'X' };
    private List<String> colorNames = new List<string> { "Standard", "Red", "Orange", "Yellow", "Chartreuse", "Lime", "Green", "Cyan", "Blue", "Violet", "Magenta", "Pink", "Brown", "White", "Gray", "Black", "Clear" };
    public Color[] textColorOptions;
    int selectedColor;

    void Awake()
    {
        moduleId = moduleIdCounter++;

        roundButton.OnInteract += delegate () { PressRoundButton(); return false; };
        submitButton.OnInteract += delegate () { PressSubmitButton(); return false; };
    }

    // Use this for initialization
    void Start()
    {
        textColor = UnityEngine.Random.Range(0, 17);
        selectedPhrase = UnityEngine.Random.Range(0, 1000);
        solutionColor = selectedPhrase % 17;
        literalPhrase = phraseList.phrases[selectedPhrase];
        WordWrap();
        screenText.text = wordWrappedPhrase;
        screenText.color = textColorOptions[textColor];
        Debug.LogFormat("[Colo(u)r Talk #{0}] The phrase is: \"{1}\"", moduleId, literalPhrase.Replace("\n", ""));
        Debug.LogFormat("[Colo(u)r Talk #{0}] The solution colo(u)r is: {1} ({2}).", moduleId, colorNames[solutionColor], colorLetters[solutionColor]);
    }

    void PressRoundButton()
    {
        roundButton.AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        if (moduleSolved != true)
        {
            selectedColor = (selectedColor + 1) % 17;
            arrow.transform.localEulerAngles = new Vector3(-90, 0, arrowRot[selectedColor]);
        }
    }

    void PressSubmitButton()
    {
        submitButton.AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        if (moduleSolved != true)
        {
            if (selectedColor == solutionColor)
            {
                Debug.LogFormat("[Colo(u)r Talk #{0}] Selected colo(u)r is {1} ({2}), which is correct. Module solved.", moduleId, colorNames[selectedColor], colorLetters[selectedColor]);
                GetComponent<KMBombModule>().HandlePass();
                moduleSolved = true;
            }
            else
            {
                Debug.LogFormat("[Colo(u)r Talk #{0}] Selected colo(u)r is {1} ({2}), which is incorrect. Module striked.", moduleId, colorNames[selectedColor], colorLetters[selectedColor]);
                GetComponent<KMBombModule>().HandleStrike();
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
            } else
            {
                newPhrase += literalPhrase[i];
            }
        }
        wordWrappedPhrase = newPhrase;
    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} submit <colo(u)r> [Submits the specified colo(u)r] | Valid colo(u)r's are Standard, Red, Orange, Yellow, Chartreuse, Lime, Green, Cyan, Blue, Violet, Magenta, Pink, Brown, White, Gray, Black, or Clear";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if(parameters.Length == 2)
            {
                yield return null;
                if (parameters[1].EqualsIgnoreCase("Standard"))
                {
                    while(selectedColor != 0)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Red"))
                {
                    while (selectedColor != 1)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Orange"))
                {
                    while (selectedColor != 2)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Yellow"))
                {
                    while (selectedColor != 3)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Chartreuse"))
                {
                    while (selectedColor != 4)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Lime"))
                {
                    while (selectedColor != 5)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Green"))
                {
                    while (selectedColor != 6)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Cyan"))
                {
                    while (selectedColor != 7)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Blue"))
                {
                    while (selectedColor != 8)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Violet"))
                {
                    while (selectedColor != 9)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Magenta"))
                {
                    while (selectedColor != 10)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Pink"))
                {
                    while (selectedColor != 11)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Brown"))
                {
                    while (selectedColor != 12)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("White"))
                {
                    while (selectedColor != 13)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Gray"))
                {
                    while (selectedColor != 14)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Black"))
                {
                    while (selectedColor != 15)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else if (parameters[1].EqualsIgnoreCase("Clear"))
                {
                    while (selectedColor != 16)
                    {
                        roundButton.OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    submitButton.OnInteract();
                }
                else
                {
                    yield return "sendtochaterror '"+parameters[1]+"' is not an option on my colo(u)r wheel!";
                }
                yield break;
            }
        }
    }
}