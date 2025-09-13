Shader "Custom/TextureBlend"{
    Properties{
        _Tex1 ("Texture 1", 2D) = "white"
        _Tex2 ("Texture 2", 2D) = "white"
        _TVal ("TVal", Float) = .5
    }

    SubShader{
        Pass{

            CGPROGRAM
            #pragma vertex VertexProgram
            #pragma fragment FragProgram

            #include "UnityCG.cginc"

            sampler2D _Tex1;
            sampler2D _Tex2;
            float _TVal;

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
                return lerp(tex2D(_Tex1, i.uv), tex2D(_Tex2, i.uv), _TVal);
            }

            ENDCG
        }
    }
}