using TMPro;
using UnityEngine;

public class BestScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bestScore;

    public void Initialize()
    {
        UpdateView();

        GameManager.Instance.OnBestScoreUpdated += UpdateView;
        GameManager.Instance.OnGameReset += UpdateView;
    }

    public void SetVisible(bool _isVisible)
    {
        bestScore.enabled = _isVisible;
    }

    private void UpdateView()
    {
        float quickestShoutTime = SaveManager.Instance.GetQuickestShoutTime();
        
        if(quickestShoutTime >= float.MaxValue)
        {
            bestScore.text = "Best Score:";
        }
        else
        {
            bestScore.text = "Best Score: " + SaveManager.Instance.GetQuickestShoutTime().ToString(".00") + "s";
        }
    }
}
