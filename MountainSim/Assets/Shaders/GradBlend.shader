Shader "Custom/GradBlend"{
    Properties{
        _TilingFactor("Tiling Factor", Float) = 3
    }

    SubShader{
        Pass{
            CGPROGRAM
            #pragma vertex VertexProgram
            #pragma fragment FragProgram

            #include "UnityCG.cginc"

            uniform float _TilingFactor;
            uniform float _Bounds[10];
            uniform int _numBounds;
            UNITY_DECLARE_TEX2DARRAY(_Textures);

            struct Interpolators {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 localPos : TEXCOORD2;
            };

            struct VertexData {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolators VertexProgram(VertexData v) {
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                i.worldPos = mul(unity_ObjectToWorld, v.position).xyz;
                i.localPos = v.position.xyz;
                i.uv = v.uv;
                return i;
            }

            float4 FragProgram(Interpolators i) : SV_TARGET{
                return float4(1,0,0,1);
            }
            
            ENDCG
        }
    }
}

// logic to calculate gradients
// grab each vertex. Move in each x and z directions to create dx and dz. Record the positions.
// create a vector from x to dx and x to dz
// add the 2 vectors to create the vector in the plane.
// calculate graident of the vector? 