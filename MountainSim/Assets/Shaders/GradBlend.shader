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
        
            float4 FragProgram(Interpolators i) : SV_TARGET{
                float currGrad = 1 - saturate(i.normal.y);
                int topIndex = 0;
                int botIndex = 0;
                float percent = 0;
                int earlyExit = 0;

                if(currGrad > _Bounds[_numBounds - 1]){
                    topIndex = _numBounds - 1;
                    botIndex = _numBounds - 1;
                    percent = 1;
                    earlyExit = 1;
                }
                else if(currGrad < _Bounds[0]){
                    topIndex = 0;
                    botIndex = 0;
                    percent = 1;
                    earlyExit = 1;
                }
                for(int idx = 0; idx < _numBounds - 1 && !earlyExit; idx++){
                    float currBound = _Bounds[idx];
                    float nextBound = _Bounds[idx + 1];
                    if(currGrad > currBound && currGrad < nextBound){
                        topIndex = idx + 1;
                        botIndex = idx;
                        percent = saturate((currGrad - currBound) / (nextBound - currBound));
                        break;
                    }
                }
                
                return blendTextures(percent, topIndex,botIndex, i.uv);
            }
            
            ENDCG
        }
    }
}
