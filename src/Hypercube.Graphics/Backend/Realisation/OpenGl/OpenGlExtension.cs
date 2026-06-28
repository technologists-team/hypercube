using System.Runtime.CompilerServices;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;
using Silk.NET.OpenGL;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

[PublicAPI]
public static class OpenGlExtension
{
    extension(GL gl)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnbindTexture(TextureTarget target)
        {
            gl.BindTexture(TextureTarget.Texture2D, 0);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Viewport(Vector2i size)
        {
            gl.Viewport(0, 0, (uint)size.X, (uint)size.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ObjectLabel(ObjectIdentifier identifier, uint handle, string name)
        {
            gl.ObjectLabel(identifier, handle, (uint) name.Length, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasErrors()
        {
            return gl.GetError() != (int) ErrorCode.NoError;
        }

        public string HasErrors(string title)
        {
            var error = gl.GetError();
            var result = string.Empty;

            while (error != (int) ErrorCode.NoError)
            {
                result += $"{title}: {error}{Environment.NewLine}";
                error = gl.GetError();
            }

            return result;
        }
    }
}