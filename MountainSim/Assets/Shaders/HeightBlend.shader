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
                
                float botIndex = 0;
                float topIndex = 0;
                float percent = 0;

                for(int idx = 0; idx < _numBounds - 1; idx++){
                    float currBound = _Bounds[idx];
                    float nextBound = _Bounds[idx + 1];

                    float inInterval = step(currBound, yPos) * (1 - step(nextBound, yPos));

                    botIndex += idx * inInterval;
                    topIndex += (idx + 1) * inInterval;
                    
                    float intervalRange = nextBound - currBound;

                    float p = (yPos - currBound) / max(intervalRange, 0.00001);
                    percent += saturate(p) * inInterval;
                }

                float aboveLast = step(_Bounds[_numBounds - 1], yPos);
                botIndex += (_numBounds - 1) * aboveLast;
                topIndex += (_numBounds - 1) * aboveLast;
                
                return blendTextures(percent, (int)topIndex, (int)botIndex, i.uv);
            }
            ENDCG
        }
    }
}