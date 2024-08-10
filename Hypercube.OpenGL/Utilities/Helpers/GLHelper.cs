using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.OpenGL.Utilities.Helpers;

public static class GLHelper
{
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