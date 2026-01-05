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
            #include "TextureBlend.cginc"
            #define MAX_BOUNDS 4
        
            float4 FragProgram(Interpolators i) : SV_TARGET{
                float currGrad =  1 - saturate(i.normal.y);
                
                float botIndex = 0;
                float topIndex = 0;
                float percent = 0;
                
                [unroll(MAX_BOUNDS)] for(int idx = 0; idx < MAX_BOUNDS; idx++){
                    if(idx > _numBounds - 1) break;
                    float currBound = _Bounds[idx];
                    float nextBound = _Bounds[min(idx + 1, 3)];

                    float inInterval = step(currBound, currGrad) * step(currGrad, nextBound);

                    botIndex += idx * inInterval;
                    topIndex += (idx + 1) * inInterval;
                    
                    float intervalRange = nextBound - currBound;

                    float p = (currGrad - currBound) / max(intervalRange, 0.00001);
                    percent += saturate(p) * inInterval;
                }

                float aboveLast = step(_Bounds[_numBounds - 1], currGrad);
                botIndex += (_numBounds - 1) * aboveLast;
                topIndex += (_numBounds - 1) * aboveLast;
                saturate(percent);
                return blendTextures(percent, (int)topIndex, (int)botIndex, i.uv);
            }
            ENDCG
                
        }
    }
}