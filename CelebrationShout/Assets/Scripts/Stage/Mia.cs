using UnityEngine;

public class Mia : MonoBehaviour, IStageCharacter
{
    [SerializeField]
    private GameObject hat;

    [SerializeField]
    private SkinnedMeshRenderer face;

    public void FeelNeutral()
    {

    }

    public void FeelHappy()
    {

    }

    public void FeelSad()
    {

    }

    public void WearHat(bool _doesWearHat)
    {
        hat.SetActive(_doesWearHat);
    }
}
