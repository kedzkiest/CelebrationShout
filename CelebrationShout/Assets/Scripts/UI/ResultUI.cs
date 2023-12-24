using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    /// <summary>
    /// The background image of title.
    /// </summary>
    [SerializeField]
    private Image background;

    /// <summary>
    /// The panel to separate texts and background.
    /// </summary>
    [SerializeField]
    private Image panel;

    /// <summary>
    /// The actual score text to be updated according to player shout time.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI score_real;

    /// <summary>
    /// The decoration score text.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI score_fake;

    /// <summary>
    /// Guides shows the step to next sequence.
    /// For example: - Press Space to Retry - , - Press Escape to Title -
    /// </summary>
    [SerializeField]
    private List<TextMeshProUGUI> guidesToNext = new List<TextMeshProUGUI>();

    public void Initialize()
    {
        GameManager.Instance.OnShoutEnd += UpdateScore;
    }

    /// <summary>
    /// The method to set visibilities of all fields in this class.
    /// </summary>
    /// <param name="_isVisible"></param>
    public void SetVisible(bool _isVisible)
    {
        background.enabled = _isVisible;
        panel.enabled = _isVisible;
        score_real.enabled = _isVisible;
        score_fake.enabled = _isVisible;

        foreach(TextMeshProUGUI guide in guidesToNext)
        {
            guide.enabled = _isVisible;
        }
    }

    private void UpdateScore(bool _isWrongShout, float _shoutTime)
    {
        Debug.Log("shouttime: " + _shoutTime);

        // in case player did wrong shout
        if(_isWrongShout)
        {
            score_real.text = "You failed...";
            return;
        }

        // in case player did correct shout
        score_real.text = "You shouted in: " + _shoutTime.ToString("0.00") + "s";
    }
}
