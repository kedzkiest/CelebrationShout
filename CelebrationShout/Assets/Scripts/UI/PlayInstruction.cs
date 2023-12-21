using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayInstruction : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> instructionTextList = new List<TextMeshProUGUI>();

    [SerializeField]
    private List<Image> keycapIconList = new List<Image>();

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
