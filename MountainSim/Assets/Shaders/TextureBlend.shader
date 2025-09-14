Shader "Custom/TextureBlend"{
    Properties{
        _TilingFactor("Tiling Factor", Float) = 10
        _TVal("T Val", Float) = .1
    }

    SubShader{
        Pass{

            CGPROGRAM
            #pragma vertex VertexProgram
            #pragma fragment FragProgram

            #include "UnityCG.cginc"

            uniform float _TilingFactor;
            uniform float _Heights[10];
            uniform int _numBounds;
            uniform float _TVal;
            UNITY_DECLARE_TEX2DARRAY(_Textures);

            struct Interpolators {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct VertexData {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolators VertexProgram(VertexData v) {
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                i.uv = v.uv;
                return i;
            }

            float4 FragProgram(Interpolators i) : SV_TARGET{

                float3 uvLayer1 = float3(i.uv * _TilingFactor, 0);
                float3 uvLayer2 = float3(i.uv * _TilingFactor, 1);


                float4 tex1 = UNITY_SAMPLE_TEX2DARRAY_LOD(_Textures, uvLayer1, 0);
                float4 tex2 = UNITY_SAMPLE_TEX2DARRAY_LOD(_Textures, uvLayer2, 0);

                return lerp(tex1,tex2,_TVal);
            }

            ENDCG
        }
    }
}