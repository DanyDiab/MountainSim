Shader "Custom/HeightBlend"{
    Properties{
        _TilingFactor("Tiling Factor", Float) = 3
    }
    SubShader{
        Pass{
            CGPROGRAM
            #pragma vertex VertexProgram
            #pragma fragment FragProgram

            #include "UnityCG.cginc"
            #include "TextureBlend.cginc"

            float4 FragProgram(Interpolators i) : SV_TARGET{
                float yPos = i.localPos.y;

                if (yPos <= _Bounds[0]) {
                    // Sample texture at index 0
                    float3 uvLayer = float3(i.uv * _TilingFactor, 0);
                    return UNITY_SAMPLE_TEX2DARRAY_LOD(_Textures, uvLayer, 0);
                }

                if (yPos >= _Bounds[_numBounds - 1]) {
                    // Sample texture at the last index
                    float3 uvLayer = float3(i.uv * _TilingFactor, _numBounds - 1);
                    return UNITY_SAMPLE_TEX2DARRAY_LOD(_Textures, uvLayer, 0);
                }

                for (int idx = 0; idx < _numBounds - 1; idx++){
                    float boundY = _Bounds[idx];
                    float nextBound = _Bounds[idx + 1];

                    if(yPos >= boundY && yPos < nextBound){
                        int topIndex = idx + 1;
                        int botIndex = idx;
                        float percent = saturate((yPos - boundY) / (nextBound - boundY));
                        
                        // We found our slice, so blend and return
                        return blendTextures(percent, topIndex, botIndex, i.uv);
                    }
                }

                // Failsafe (should never be hit, but good practice)
                float3 uvLayer = float3(i.uv * _TilingFactor, 0);
                return UNITY_SAMPLE_TEX2DARRAY_LOD(_Textures, uvLayer, 0);
            }
            ENDCG
        }
    }
}