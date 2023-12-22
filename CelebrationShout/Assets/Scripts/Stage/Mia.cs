using UnityEngine;
using System.Collections.Generic;

public class Mia : MonoBehaviour, IStageCharacter
{
    [SerializeField]
    private GameObject hat;

    [SerializeField]
    private SkinnedMeshRenderer face;

    [SerializeField]
    private Mesh faceMesh;

    public enum BlendShapeName
    {
        mouthSmileLeft,
        mouthSmileRight,
        cheekSquintLeft,
        cheekSquintRight,
        mouthFrownLeft,
        mouthFrownRight,
        browDownLeft,
        browDownRight,
        browInnerUp,
    }

    private readonly Dictionary<BlendShapeName, int> happyDict = new Dictionary<BlendShapeName, int>
    {
        {BlendShapeName.mouthSmileLeft, 100 },
        {BlendShapeName.mouthSmileRight, 100 },
        {BlendShapeName.cheekSquintLeft, 100 },
        {BlendShapeName.cheekSquintRight, 100 },
    };

    private readonly Dictionary<BlendShapeName, int> sadDict = new Dictionary<BlendShapeName, int>
    {
        {BlendShapeName.mouthFrownLeft, 100 },
        {BlendShapeName.mouthFrownRight, 100 },
        {BlendShapeName.browDownLeft, 100 },
        {BlendShapeName.browDownRight, 100 },
        {BlendShapeName.browInnerUp, 100 },
    };

    public void FeelNeutral()
    {
        for(int i = 0; i < faceMesh.blendShapeCount; i++)
        {
            face.SetBlendShapeWeight(i, 0);
        }
    }

    public void FeelHappy()
    {
        SetEmotion(happyDict);
    }

    public void FeelSad()
    {
        SetEmotion(sadDict);
    }

    private void SetEmotion(Dictionary<BlendShapeName, int> _dict)
    {
        foreach (var pair in _dict)
        {
            string blendshapeName = pair.Key.ToString();
            int blendshapeIndex = faceMesh.GetBlendShapeIndex(blendshapeName);

            face.SetBlendShapeWeight(blendshapeIndex, pair.Value);
        }
    }

    public void WearHat(bool _doesWearHat)
    {
        hat.SetActive(_doesWearHat);
    }
}
