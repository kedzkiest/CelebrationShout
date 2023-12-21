using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    /// <summary>
    /// The background image of title.
    /// </summary>
    [SerializeField]
    private Image background;

    public void SetVisible(bool _isVisible)
    {
        background.enabled = _isVisible;
    }
}
