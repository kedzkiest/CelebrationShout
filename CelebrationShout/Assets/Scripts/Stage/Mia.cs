using UnityEngine;
using System.Collections.Generic;

public class Mia : MonoBehaviour, IStageCharacter
{
    /// <summary>
    /// Santa hat gameobject for Merry Christmas event.
    /// </summary>
    [SerializeField]
    private GameObject hat;

    /// <summary>
    /// The component that have blendshapes, which is needed to control facial expression.
    /// </summary>
    [SerializeField]
    private SkinnedMeshRenderer face;

    /// <summary>
    /// The component that contains the number of total blendshapes, which is needed to limit iterations regarding blendshapes.
    /// </summary>
    [SerializeField]
    private Mesh faceMesh;

    public enum BlendShapeName
    {
        // left entry: the name of blendshapes that face have
        // right comment: the emotion where left blendshape is used

        mouthSmileLeft,     // Happy
        mouthSmileRight,    // Happy
        cheekSquintLeft,    // Happy
        cheekSquintRight,   // Happy

        mouthFrownLeft,     // Sad
        mouthFrownRight,    // Sad
        browDownLeft,       // Sad
        browDownRight,      // Sad
        browInnerUp,        // Sad
    }

    /// <summary>
    /// The table to define the value of each blendshape when happy emotion is applied to character.
    /// </summary>
    private readonly Dictionary<BlendShapeName, int> happyDict = new Dictionary<BlendShapeName, int>
    {
        {BlendShapeName.mouthSmileLeft, 100 },
        {BlendShapeName.mouthSmileRight, 100 },
        {BlendShapeName.cheekSquintLeft, 100 },
        {BlendShapeName.cheekSquintRight, 100 },
    };

    /// <summary>
    /// The table to define the value of each blendshape when sad emotion is applied to character.
    /// </summary>
    private readonly Dictionary<BlendShapeName, int> sadDict = new Dictionary<BlendShapeName, int>
    {
        {BlendShapeName.mouthFrownLeft, 100 },
        {BlendShapeName.mouthFrownRight, 100 },
        {BlendShapeName.browDownLeft, 100 },
        {BlendShapeName.browDownRight, 100 },
        {BlendShapeName.browInnerUp, 100 },
    };

    /// <summary>
    /// Reset all the blendshapes of face that character face has.
    /// </summary>
    public void FeelNeutral()
    {
        for(int i = 0; i < faceMesh.blendShapeCount; i++)
        {
            face.SetBlendShapeWeight(i, 0);
        }
    }

    /// <summary>
    /// Make the character's facial expression looks happy.
    /// </summary>
    public void FeelHappy()
    {
        SetEmotion(happyDict);
    }

    /// <summary>
    /// Make the character's facial expression looks sad.
    /// </summary>
    public void FeelSad()
    {
        SetEmotion(sadDict);
    }

    /// <summary>
    /// The method to set blendshapes according to the given emotion table.
    /// </summary>
    /// <param name="_dict"></param>
    private void SetEmotion(Dictionary<BlendShapeName, int> _dict)
    {
        foreach (var pair in _dict)
        {
            string blendshapeName = pair.Key.ToString();
            int blendshapeIndex = faceMesh.GetBlendShapeIndex(blendshapeName);

            face.SetBlendShapeWeight(blendshapeIndex, pair.Value);
        }
    }

    /// <summary>
    /// The method to make character wear a santa hat on christmas event.
    /// </summary>
    /// <param name="_doesWearHat"></param>
    public void WearHat(bool _doesWearHat)
    {
        hat.SetActive(_doesWearHat);
    }
}
