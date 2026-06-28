namespace Hypercube.Windowing.Types;

public enum ErrorCode : short
{
    NoError,
    NotInitialized,
    NoContext,
    NoWindowContext,
    InvalidEnum,
    InvalidValue,
    
    OutOfMemory,
    
    ApiUnavailable,
    VersionUnavailable,
    PlatformError,
    
    FormatUnavailable
}
