using System.Diagnostics;
using Hypercube.Graphics.Utilities.Extensions;
using JetBrains.Annotations;
using Silk.NET.OpenGL;

namespace Hypercube.Graphics.Rendering.Api.Realisations.OpenGl;

public sealed partial class OpenGlRenderingApi
{
    private ArrayObject GenArrayObject()
    {
        return new ArrayObject(Gl);
    }
    
    private ArrayObject GenArrayObject(string label)
    {
        return new ArrayObject(Gl, label);
    }

    private BufferObject GenBufferObject(BufferTargetARB target)
    {
        return new BufferObject(Gl, target);
    }
    
    private BufferObject GenBufferObject(BufferTargetARB target, string label)
    {
        return new BufferObject(Gl, target, label);
    }

    [DebuggerDisplay("VertexArray {_handle}")]
    private class ArrayObject : IDisposable
    {
        private const uint Null = 0;

        private readonly GL _gl;
        private readonly uint _handle;
        
        private bool _bound;

        public ArrayObject(GL gl)
        {
            _gl = gl;
            _handle = _gl.GenVertexArray();
        }

        public ArrayObject(GL gl, string label) : this(gl)
        {
            Label(label);
        }
        
        [PublicAPI]
        public void Bind()
        {
            if (_bound)
                return;

            _bound = true;
            _gl.BindVertexArray(_handle);
        }

        public void Unbind()
        {
            if (!_bound)
                return;

            _bound = false;
            _gl.BindVertexArray(Null);
        }

        [PublicAPI]
        public void Delete()
        {
            _gl.DeleteVertexArray(_handle);
        }

        [PublicAPI]
        public void Label(string name)
        {
            Bind();
            _gl.ObjectLabel(ObjectIdentifier.VertexArray, _handle, name);
        }

        public void Dispose()
        {
            Delete();
        }
    }

    [DebuggerDisplay("Buffer {_handle} ({BufferTargetName})")]
    public class BufferObject : IDisposable
    {
        private const uint Null = 0;

        private readonly GL _gl;
        private readonly uint _handle;
        private readonly BufferTargetARB _target;
        
        private bool _bound;

        private string BufferTargetName => Enum.GetName(_target) ?? _target.ToString();

        public BufferObject(GL gl, BufferTargetARB target)
        {
            _gl = gl;
            _handle = _gl.GenBuffer();
            _target = target;
        }

        public BufferObject(GL gl, BufferTargetARB target, string label) : this(gl, target)
        {
            Label(label);
        }
    
        public void Bind()
        {
            if (_bound)
                return;
    
            _bound = true;
            _gl.BindBuffer(_target, _handle);
        }
    
        public void Unbind()
        {
            if (!_bound)
                return;
    
            _bound = false;
            _gl.BindBuffer(_target, Null);
        }
    
        public void Delete()
        {
            _gl.DeleteBuffer(_handle);
        }
        
        public unsafe void SetData(int size, nint data, BufferUsageARB hint = BufferUsageARB.StaticDraw)
        {
            Bind();
            _gl.BufferData(_target, (nuint) size, (void*) data, hint);
        }
    
        public unsafe void SetData<T>(T[] data, BufferUsageARB hint = BufferUsageARB.StaticDraw)
            where T : unmanaged
        {
            Bind();
            fixed (T* dataPtr = data)
                _gl.BufferData(_target, (nuint) (data.Length * sizeof(T)), dataPtr, hint);
        }
        
        public void Label(string name)
        {
            Bind();
            _gl.ObjectLabel(ObjectIdentifier.Buffer, _handle, name);
        }
    
        public void Dispose()
        {
            Delete();
        }
    }
}