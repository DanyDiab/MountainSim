Shader "Custom/TextureBlend"{
    Properties{
        _TilingFactor("Tiling Factor", Float) = 100
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
                float yPos = i.localPos.y;
                int topIndex = 0;
                int botIndex = 0;
                float percent = 0;
                if(yPos > _Bounds[_numBounds - 1]){
                    // if above
                    return float4(0,0,1,1);
                }
                if(yPos < _Bounds[0]){
                    // if below
                    return float4(1,0,0,1);
                }

                for (int idx = 0; idx < _numBounds - 1; idx++){
                    float boundY = _Bounds[idx];
                    float nextBound = _Bounds[idx + 1];
                    // found the correct bound
                    if(yPos >= boundY && yPos < nextBound){
                        topIndex = idx + 1;
                        botIndex = idx;
                        // calculates the percent or T value for lerping between the 2 textures. This is the percent from the bottom of this bound to the next bound
                        // saturate bounds between 0 and 1
                        percent = saturate((yPos - boundY) / (nextBound - boundY));
                        break;
                    }
                }
                // deteremine tedxture layer, including tiling of the texture
                float3 uvLayerTop = float3(i.uv * _TilingFactor, topIndex);
                float3 uvLayerLow = float3(i.uv * _TilingFactor, botIndex);
                // grab the textures
                float4 texTop = UNITY_SAMPLE_TEX2DARRAY_LOD(_Textures, uvLayerTop, 0);
                float4 texLow = UNITY_SAMPLE_TEX2DARRAY_LOD(_Textures, uvLayerLow, 0);
                // lerp beyween the 2 textures using the calculated percent
                return lerp(texLow,texTop, percent);
            }
            ENDCG
        }
    }
}