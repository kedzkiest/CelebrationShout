using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayInstruction : MonoBehaviour
{
    /// <summary>
    /// The texts to explain how to play
    /// For example, Press B to say Happy Birthday.
    /// </summary>
    [SerializeField]
    private List<TextMeshProUGUI> instructionTextList = new List<TextMeshProUGUI>();

    /// <summary>
    /// The keycap icons that visually represents which key to press to do what.
    /// For example, place keycap icon "B" next to the play instruction that uses "B" key.
    /// </summary>
    [SerializeField]
    private List<Image> keycapIconList = new List<Image>();

    /// <summary>
    /// The method to set visibilities of all fields in this class.
    /// </summary>
    /// <param name="_isVisible"></param>
    public void SetVisible(bool _isVisible)
    {
        foreach(TextMeshProUGUI instructionText in instructionTextList)
        {
            instructionText.enabled = _isVisible;
        }

        foreach(Image keycapIcon in keycapIconList)
        {
            keycapIcon.enabled = _isVisible;
        }
    }
}
