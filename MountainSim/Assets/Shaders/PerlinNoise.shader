Shader "Custom/PerlinNoise" {
    Properties{
        // properties will go here
    }
    SubShader{
        Pass{

            CGPROGRAM
            #pragma vertex VertexPogram
            #pragma fragment FragProgram

            #include "UnityCG.cginc"

            struct Interpolators {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct VertexData {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            Interpolators VertexPogram(VertexData v){
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                i.uv = v.uv;
                i.color = v.color;
                return i;                
            }

            float4 FragProgram(Interpolators i) : SV_TARGET{
                return i.color;
            }
            ENDCG
        }
    }
}