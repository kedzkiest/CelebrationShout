public interface IStageCharacter
{
    public void FeelNeutral();
    public void FeelHappy();
    public void FeelSad();

    /// <summary>
    /// The method for make stage character wear a santa hat for Merry Christmas event.
    /// </summary>
    /// <param name="_doesWearHat"></param>
    public void WearHat(bool _doesWearHat);
}
