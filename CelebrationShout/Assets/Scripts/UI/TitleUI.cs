using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
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
    /// The title of the game.
    /// </summary>
    [SerializeField]
    private GameObject title;

    /// <summary>
    /// A brief explanation on how to play the game.
    /// </summary>
    [SerializeField]
    private PlayInstruction instruction;

    /// <summary>
    /// A guide shows the step to next sequence.
    /// For example: - Press Any Key -
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI guideToNext;

    /// <summary>
    /// A view of the best score that player have ever got in ingame.
    /// </summary>
    [SerializeField]
    private BestScore bestScore;

    public void Initialize()
    {
        bestScore.Initialize();
    }

    /// <summary>
    /// The method to set visibilities of all fields in this class.
    /// </summary>
    /// <param name="_isVisible"></param>
    public void SetVisible(bool _isVisible)
    {
        background.enabled = _isVisible;
        panel.enabled = _isVisible;
        title.SetActive(_isVisible);
        instruction.SetVisible(_isVisible);
        guideToNext.enabled = _isVisible;
        bestScore.SetVisible(_isVisible);
    }

    public void UpdateBestScore()
    {
        bestScore.UpdateView();
    }
}
