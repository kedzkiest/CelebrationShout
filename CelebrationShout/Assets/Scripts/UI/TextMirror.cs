using TMPro;
using UnityEngine;

public class TextMirror : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI realText;

    [SerializeField]
    private TextMeshProUGUI fakeText;

    // Update is called once per frame
    private void Update()
    {
        fakeText.text = realText.text;
    }
}
