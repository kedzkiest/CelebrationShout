using TMPro;
using UnityEngine;

public class BestScoreMirror : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI realBestScore;

    [SerializeField]
    private TextMeshProUGUI fakeBestScore;

    // Update is called once per frame
    private void Update()
    {
        fakeBestScore.text = realBestScore.text;
    }
}
