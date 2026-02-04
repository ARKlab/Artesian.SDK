namespace Artesian.SDK.Common
{
    /// <summary>
    /// Defines how a key conflict is handled when adding a value.
    /// </summary>
    public enum KeyConflictPolicy
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Throw,
        Overwrite,
        Skip
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    }
}
