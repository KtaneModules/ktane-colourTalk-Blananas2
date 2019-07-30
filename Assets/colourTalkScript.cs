using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
}