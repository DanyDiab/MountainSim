#ifndef TEXTURE_BLEND_INCLUDED
#define TEXTURE_BLEND_INCLUDED

#include "UnityCG.cginc"

// Shader Properties
uniform float _TilingFactor;
uniform float _Bounds[10];
uniform int _numBounds;
UNITY_DECLARE_TEX2DARRAY(_Textures);

// Common Structs
struct Interpolators {
    float4 position : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 worldPos : TEXCOORD1;
    float3 localPos : TEXCOORD2;
    float3 normal : TEXCOORD3;
};

struct VertexData {
    float4 position : POSITION;
    float2 uv : TEXCOORD0;
    float3 normal : NORMAL;
};

Interpolators VertexProgram(VertexData v) {
    Interpolators i;
    i.position = UnityObjectToClipPos(v.position);
    i.worldPos = mul(unity_ObjectToWorld, v.position).xyz;
    i.localPos = v.position.xyz;
    i.normal = UnityObjectToWorldNormal(v.normal);
    i.uv = v.uv;
    return i;
}

float4 blendTextures(float percent, int topIndex, int botIndex, float2 uv) {
    // Determine texture layers, including tiling
    float3 uvLayerTop = float3(uv * _TilingFactor, topIndex);
    float3 uvLayerLow = float3(uv * _TilingFactor, botIndex);

    // Grab the textures
    float4 texTop = UNITY_SAMPLE_TEX2DARRAY(_Textures, uvLayerTop);
    float4 texLow = UNITY_SAMPLE_TEX2DARRAY(_Textures, uvLayerLow);

    // Lerp between the 2 textures using the calculated percent
    return lerp(texLow, texTop, percent);
}
#endif 