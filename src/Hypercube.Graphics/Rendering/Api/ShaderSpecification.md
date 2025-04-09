# Hypercube Engine Shader Specification (.shd format)

## Overview
The Hypercube Engine uses a unified shader file format (.shd) that combines all shader types in a single file with tag-based separation. This specification describes version 1.0 of the format.

## File Structure

1. **Global Section** (copied to all shaders)
    - Appears before any shader type tags
    - Contain defines, structs, and functions shared across all shaders

2. **Shader Sections**
    - `[vertex]` - Vertex shader
    - `[fragment]` - Fragment shader
    - `[geometry]` - Geometry shader
    - `[compute]` - Compute shader
    - `[tessellation]` - Tessellation shader

## Example Implementation
```glsl
// Hypercube Shader Example
// Version 1.0

// Global section - copied to all shaders
#version 460
#define MAX_LIGHTS 8
#define PI 3.14159265359

struct LightData {
    vec3 position;
    vec3 color;
    float intensity;
};

float calculateFalloff(float distance, float radius) {
    float ratio = distance / radius;
    return clamp(1.0 - ratio * ratio, 0.0, 1.0);
}

[vertex]
in vec3 aPosition;
in vec3 aNormal;

uniform mat4 uMVP;

out vec3 vWorldPos;
out vec3 vNormal;

void main() {
    vWorldPos = aPosition;
    vNormal = aNormal;
    gl_Position = uMVP * vec4(aPosition, 1.0);
}

[fragment]
uniform LightData uLights[MAX_LIGHTS];
uniform vec3 uAmbient;

in vec3 vWorldPos;
in vec3 vNormal;

out vec4 fragColor;

void main() {
    vec3 normal = normalize(vNormal);
    vec3 lighting = uAmbient;
    
    for (int i = 0; i < MAX_LIGHTS; i++) {
        if (uLights[i].intensity <= 0.0) continue;
        
        vec3 lightDir = uLights[i].position - vWorldPos;
        float distance = length(lightDir);
        lightDir = normalize(lightDir);
        
        float diffuse = max(dot(normal, lightDir), 0.0);
        float attenuation = calculateFalloff(distance, 5.0);
        
        lighting += uLights[i].color * diffuse * attenuation * uLights[i].intensity;
    }
    
    fragColor = vec4(lighting, 1.0);
}
```

This specification may be extended through official Hypercube Engine updates.