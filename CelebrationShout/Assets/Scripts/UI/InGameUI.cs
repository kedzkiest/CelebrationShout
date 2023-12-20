using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour, ISceneUI
{
    [SerializeField]
    private TextMeshProUGUI text;

    public void Initialize()
    {

    }

    public void SetVisible(bool _isVisible)
    {
        text.enabled = _isVisible;
    }
}
