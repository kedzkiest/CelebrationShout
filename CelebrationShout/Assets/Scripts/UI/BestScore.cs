using TMPro;
using UnityEngine;

public class BestScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bestScore;

    public void Initialize()
    {
        UpdateView();
    }

    public void SetVisible(bool _isVisible)
    {
        bestScore.enabled = _isVisible;
    }

    public void UpdateView()
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
