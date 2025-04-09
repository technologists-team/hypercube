using Hypercube.Graphics.Rendering.Api.Handlers;
using Hypercube.Graphics.Rendering.Api.Settings;
using Hypercube.Graphics.Rendering.Batching;
using Hypercube.Graphics.Rendering.Shaders;
using Hypercube.Graphics.Windowing;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Rendering.Api.Realisations.Headless;

public sealed class HeadlessRenderingApi : IRenderingApi
{
    public event InitHandler? OnInit;
    public event DrawHandler? OnDraw;
    public event DebugInfoHandler? OnDebugInfo;

    public IShaderProgram? PrimitiveShaderProgram => null;
    public IShaderProgram? TexturingShaderProgram => null;

    public int BatchVerticesIndex => 0;
    public int BatchIndicesIndex => 0;

    public void Init(IContextInfo context, RenderingApiSettings settings)
    {
    }

    public void Load()
    {
    }

    public void Terminate()
    {
    }

    public void Render(IWindow window)
    {
    }

    public void EnsureBatch(PrimitiveTopology topology, uint shader, uint? texture)
    {
    }

    public void BreakCurrentBatch()
    {
    }

    public void PushVertex(Vertex vertex)
    {
    }

    public void PushIndex(uint start, uint offset)
    {
    }

    public void PushIndex(int start, int index)
    {
    }

    public IShaderProgram CreateShaderProgram(string source)
    {
        return new ShaderProgram();
    }
    
    private sealed class ShaderProgram : BaseShaderProgram
    {
        public ShaderProgram() : base(0)
        {
        }

        public override void SetUniform(string name, int value)
        {
        }

        public override void SetUniform(string name, float value)
        {
        }

        public override void SetUniform(string name, double value)
        {
        }

        public override void SetUniform(string name, Vector2 value)
        {
        }

        public override void SetUniform(string name, Vector2i value)
        {
        }

        public override void SetUniform(string name, Vector3 value)
        {
        }

        public override void SetUniform(string name, Vector3i value)
        {
        }

        public override void SetUniform(string name, Vector4 value)
        {
        }

        public override void SetUniform(string name, Matrix3x3 value, bool transpose = false)
        {
        }

        public override void SetUniform(string name, Matrix4x4 value, bool transpose = false)
        {
        }

        protected override void InternalUseProgram(uint handle)
        {
        }

        protected override void InternalLabel(string name)
        {
        }

        protected override void InternalDelete(uint handle)
        {
        }

        public override void SetUniform(string name, Color value)
        {
        }
    }
}