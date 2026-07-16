namespace Hypercube.Windowing.Core;

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
