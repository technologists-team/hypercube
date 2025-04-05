using JetBrains.Annotations;

namespace Hypercube.Resources.Preloading;

/// <summary>
/// Represents the progress state of a resource preloading operation.
/// </summary>
/// <remarks>
/// This structure is immutable and thread-safe. It provides detailed information about
/// the preloading progress including completion percentage, current operation, and any errors.
/// </remarks>
[PublicAPI]
public readonly struct PreloadProgress
{
    /// <summary>
    /// Gets the number of resources that have been successfully loaded.
    /// </summary>
    public readonly int Loaded;
    
    /// <summary>
    /// Gets the total number of resources to be loaded.
    /// </summary>
    public readonly int Total;
    
    /// <summary>
    /// Gets the path of the resource currently being loaded.
    /// </summary>
    public readonly ResourcePath CurrentPath;
    
    /// <summary>
    /// Gets the exception that occurred during loading, if any.
    /// </summary>
    /// <remarks>
    /// Null if no error occurred, otherwise the exception instance.
    /// </remarks>
    public readonly Exception? Error;
    
    /// <summary>
    /// Gets the loading progress as a normalized value between 0 and 1.
    /// </summary>
    public float Progress => Total > 0 ? (float) Loaded / Total : 0f;
    
    /// <summary>
    /// Gets whether the preload operation has completed.
    /// </summary>
    public bool IsComplete => Loaded >= Total && Total > 0;

    /// <summary>
    /// Gets whether the preload operation has failed.
    /// </summary>
    public bool HasError => Error != null;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PreloadProgress"/> struct.
    /// </summary>
    /// <param name="loaded"> Number of resources already loaded.</param>
    /// <param name="total">Total number of resources to load.</param>
    /// <param name="currentPath"> Path of the resource currently being processed.</param>
    /// <param name="error">Exception that occurred, if any.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="loaded"/> is negative or greater than <paramref name="total"/>.
    /// </exception>
    public PreloadProgress(int loaded, int total, ResourcePath currentPath, Exception? error = null)
    {
        if (loaded < 0)
            throw new ArgumentOutOfRangeException(nameof(loaded), "Cannot be negative");
        
        if (total < 0)
            throw new ArgumentOutOfRangeException(nameof(total), "Cannot be negative");
        
        if (loaded > total)
            throw new ArgumentOutOfRangeException(nameof(loaded), "Cannot exceed total");
        
        Loaded = loaded;
        Total = total;
        CurrentPath = currentPath;
        Error = error;
    }
}