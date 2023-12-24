/* TextMirror.cs
 * 
 * The class just for copying the content of one text to another.
 * 
 * Poor implementation because of short time.
 * Better solution should exists (using an event for example).
 */

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
