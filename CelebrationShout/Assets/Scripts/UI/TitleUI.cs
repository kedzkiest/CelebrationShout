using TMPro;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    /// <summary>
    /// The title of the game.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI title;

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

    public void SetVisible(bool _isVisible)
    {
        title.enabled = _isVisible;
        instruction.SetVisible(_isVisible);
        guideToNext.enabled = _isVisible;
        bestScore.SetVisible(_isVisible);
    }
}
