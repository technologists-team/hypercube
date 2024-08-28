using Hypercube.Mathematics.Vectors;
using ImGuiNET;
using JetBrains.Annotations;

namespace Hypercube.ImGui;

[PublicAPI]
public interface IImGui
{    
    public void Dummy(Vector2 size)
    {
        ImGuiNET.ImGui.Dummy(size);
    }

    public bool ArrowButton(string label, ImGuiDirection direction)
    {
        return ImGuiNET.ImGui.ArrowButton(label, (ImGuiDir) direction);
    }

    public bool CheckboxFlags(string label, ref int flags, int value)
    {
        return ImGuiNET.ImGui.CheckboxFlags(label, ref flags, value);
    }

    public void Text(string label)
    {
        ImGuiNET.ImGui.Text(label);
    }

    public bool Button(string label)
    {
        return ImGuiNET.ImGui.Button(label);
    }

    public void Begin(string name)
    {
        ImGuiNET.ImGui.Begin(name);
    }

    public bool Checkbox(string label, ref bool value)
    {
        return ImGuiNET.ImGui.Checkbox(label, ref value);
    }

    public void Bullet()
    {
        ImGuiNET.ImGui.Bullet();
    }

    public void Columns()
    {
        ImGuiNET.ImGui.Columns();
    }
    
    public void Columns(int count)
    {
        ImGuiNET.ImGui.Columns(count);
    }

    public void Columns(int count, string id)
    {
        ImGuiNET.ImGui.Columns(count, id);
    }
    
    public void Columns(int count, string id, bool border)
    {
        ImGuiNET.ImGui.Columns(count, id, border);
    }

    public void Combo(string label, ref int item, string separatedItems)
    {
        ImGuiNET.ImGui.Combo(label, ref item, separatedItems);
    }
   
    public void Image(nint texture, Vector2 size)
    {
        ImGuiNET.ImGui.Image(texture, size);
    }
    
    public void Image(nint texture, Vector2 size, Vector2 uv)
    {
        ImGuiNET.ImGui.Image(texture, size, uv);
    }
    
    public void Separator()
    {
        ImGuiNET.ImGui.Separator();
    }
    
    public void Spacing()
    {
        ImGuiNET.ImGui.Spacing();
    }
    
    public void BeginGroup()
    {
        ImGuiNET.ImGui.BeginGroup();
    }
    
    public bool BeginMainMenuBar()
    {
        return ImGuiNET.ImGui.BeginMainMenuBar();
    }
    
    public void EndMainMenuBar()
    {
        ImGuiNET.ImGui.EndMainMenuBar();
    }
    
    public bool BeginMenu(string label)
    {
        return ImGuiNET.ImGui.BeginMenu(label);
    }
    
    public void EndMenu()
    {
        ImGuiNET.ImGui.EndMenu();
    }
    
    public void End()
    {
        ImGuiNET.ImGui.End();
    }

    public void DockSpaceOverViewport()
    {
        ImGuiNET.ImGui.DockSpaceOverViewport();
    }

    public void ShowDemoWindow()
    {
       ImGuiNET.ImGui.ShowDemoWindow();
    }
}