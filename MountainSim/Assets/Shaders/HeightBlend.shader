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
                int topIdx = 0;
                int botIdx = 0;
                float percent = 0;
                int earlyExit = 0;

                if (yPos < _Bounds[0]) {
                    // Sample texture at index 0
                    topIdx = 0;
                    botIdx = 0;
                    percent = 1;
                    earlyExit = 1;
                }

                else if (yPos > _Bounds[_numBounds - 1]) {
                    // Sample texture at the last index
                    topIdx = _numBounds - 1;
                    botIdx = _numBounds - 1;
                    percent = 1;
                    earlyExit = 1;
                }
                for (int idx = 0; idx < _numBounds - 1 && !earlyExit; idx++){
                    float boundY = _Bounds[idx];
                    float nextBound = _Bounds[idx + 1];

                    if(yPos > boundY && yPos < nextBound){
                        topIdx = idx + 1;
                        botIdx = idx;
                        percent = saturate((yPos - boundY) / (nextBound - boundY));
                        break;
                    }
                }
                return blendTextures(percent, topIdx, botIdx, i.uv);
            }
            ENDCG
        }
    }
}