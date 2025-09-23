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
                return blendTextures(percent, topIndex, botIndex, i.uv);
            }
            ENDCG
        }
    }
}