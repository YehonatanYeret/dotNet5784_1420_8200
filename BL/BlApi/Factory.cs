namespace BlApi;

/// <summary>
///  Factory class for the BL layer
/// </summary>
public static class Factory
{
    public static IBl Get() => new BlImplementation.Bl();
}
