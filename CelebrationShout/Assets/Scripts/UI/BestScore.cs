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
            // If initial value
            bestScore.text = "Best Score:";
        }
        else
        {
            // If user updated value
            bestScore.text = "Best Score: " + SaveManager.Instance.GetQuickestShoutTime().ToString("0.00") + "s";
        }
    }
}
