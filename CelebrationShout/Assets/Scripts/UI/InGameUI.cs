using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    /// <summary>
    /// The background image of title.
    /// </summary>
    [SerializeField]
    private Image background;

    /// <summary>
    /// The text that tell the result to player.
    /// For example, "You shouted in X.XX seconds", "You failed", etc..
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text;

    /// <summary>
    /// The method to set visibilities of all fields in this class.
    /// </summary>
    /// <param name="_isVisible"></param>
    public void SetVisible(bool _isVisible)
    {
        background.enabled = _isVisible;
        text.enabled = _isVisible;
    }
}
