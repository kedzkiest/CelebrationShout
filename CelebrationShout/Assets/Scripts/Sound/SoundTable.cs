using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreateSoundTable")]
public class SoundTable : ScriptableObject
{
    public enum SoundName
    {
        HAPPY_BIRTHDAY,     // The voice says "Happy birthday!"
        HAPPY_NEW_YEAR,     // The voice says "Happy new year!"
        MERRY_CHRISTMAS,    // The voice says "Merry christmas!"
        DRUM_ROLL,          // The sound for announcement preparation
        DRUM_ROLL_FINISH,   // The sound for announcement
        CORRECT_SHOUT,      // The sound tells user made correct shout
        WRONG_SHOUT,        // The sound tells user made wrong shout
    }

    [SerializeField]
    private List<SoundName> soundNameList = new List<SoundName>();

    [SerializeField]
    private List<AudioClip> audioClipList = new List<AudioClip>();

    public AudioClip GetSoundClip(SoundName _soundName)
    {
        int index = soundNameList.IndexOf(_soundName);

        return audioClipList.ElementAt(index);
    }
}
