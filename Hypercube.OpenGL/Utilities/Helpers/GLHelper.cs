using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.OpenGL.Utilities.Helpers;

[PublicAPI]
public static class GLHelper
{
    public const int DefaultTexture = 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetCap(EnableCap cap, bool value)
    {
        if (value)
        {
            GL.Enable(cap);
            return;
        }
        
        GL.Disable(cap);
    }

#region Unbind

    public static void UnbindTexture(TextureTarget target)
    {
        GL.BindTexture(target, DefaultTexture);
    }
    
#endregion

    public static void LabelObject(ObjectLabelIdentifier objLabelIdent, int glObject, string name)
    {
        GL.ObjectLabel(objLabelIdent, glObject, name.Length, name);
    }
    
    public static string CheckErrors(string title)
    {
        var error = GL.GetError();
        var result = string.Empty;
        
        while (error != ErrorCode.NoError)
        {
            result += $"{title}: {error}\r\n";
            error = GL.GetError();
        }

        return result;
    }
}