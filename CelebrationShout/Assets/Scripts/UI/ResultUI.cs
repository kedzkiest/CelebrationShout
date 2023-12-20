using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour, ISceneUI
{
    [SerializeField]
    private TextMeshProUGUI text;

    public void SetVisible(bool _isVisible)
    {
        text.enabled = _isVisible;
    }
}
