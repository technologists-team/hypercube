using Hypercube.Audio.Loading;
using Hypercube.Audio.Loading.TypeLoaders;
using JetBrains.Annotations;

namespace Hypercube.Audio;

/// <summary>
/// Displays all sound file types available on the engine side,
/// they are only available, but not necessarily supported.
/// Supported formats can be found in <see cref="IAudioLoader"/> implementation.
/// </summary>
/// <remarks>
/// Taken from <a href="https://en.wikipedia.org/wiki/Audio_file_format">Wikipedia</a>.
/// </remarks>
/// <seealso cref="IAudioLoader"/>
/// <seealso cref="IAudioTypeLoader"/>
[PublicAPI]
public enum AudioType
{
    _3gp,
    Aa,
    Aac,
    Aax,
    Act,
    Aiff,
    Alac,
    Amr,
    Ape,
    Au,
    Awb,
    Dss,
    Dvf,
    Flac,
    Gsm,
    Iklax,
    Ivs,
    M4A,
    M4B,
    M4P,
    Mmf,
    Movpkg,
    Mp3,
    Mpc,
    Msv,
    Nmf,
    Ogg,
    Oga,
    Mogg,
    Opus,
    Ra,
    Rm,
    Raw,
    Rf64,
    Sln,
    Tta,
    Voc,
    Vox,
    Wav,
    Wma,
    Wv,
    Webm,
    _8svx,
    Cda,
}