using UnityEngine;

public class SoundPlayer
{
    // Make this class Singleton
    private static SoundPlayer instance;
    public static SoundPlayer Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new SoundPlayer();
            }

            return instance;
        }
    }

    private AudioSource audioSource;

    /// <summary>
    /// The path to sound database
    /// </summary>
    const string SOUNDTABLE_PATH = "Sound/SoundTable";

    /// <summary>
    /// The instance of sound database
    /// </summary>
    private SoundTable soundTable;

    public void Initialize()
    {
        GameObject speaker = new GameObject();
        audioSource = speaker.AddComponent<AudioSource>();

        soundTable = (SoundTable)Resources.Load(SOUNDTABLE_PATH);
    }

    public void Play(SoundTable.SoundName _soundName)
    {
        AudioClip clip = soundTable.GetSoundClip(_soundName);

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public float PlayOneShot(SoundTable.SoundName _soundName)
    {
        AudioClip clip = soundTable.GetSoundClip(_soundName);

        audioSource.PlayOneShot(clip);
        return clip.length;
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
