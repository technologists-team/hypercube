using Hypercube.Graphics.Rendering.Api.Settings;

namespace Hypercube.Graphics.Rendering.Api.Handlers;

public delegate void InitHandler(string info, RenderingApiSettings settings);
public delegate void DrawHandler();
public delegate void DebugInfoHandler(string info);