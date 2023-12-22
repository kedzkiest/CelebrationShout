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

    [SerializeField]
    private TextMeshProUGUI score_real;

    [SerializeField]
    private TextMeshProUGUI score_fake;

    [SerializeField]
    private List<TextMeshProUGUI> guidesToNext = new List<TextMeshProUGUI>();

    public void Initialize()
    {
        GameManager.Instance.OnShoutEnd += UpdateScore;
    }

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
        Debug.LogWarning("called with shouttime: " + _shoutTime);

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
