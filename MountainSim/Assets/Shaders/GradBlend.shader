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
                if(currGrad > _Bounds[_numBounds - 1]){
                    return blendTextures(1,_numBounds - 1,_numBounds - 1, i.uv);
                }
                else if(currGrad < _Bounds[0]){
                    return blendTextures(1,0,0, i.uv);
                }
                for(int idx = 0; idx < _numBounds - 1; idx++){
                    float currBound = _Bounds[idx];
                    float nextBound = _Bounds[idx + 1];
                    if(currGrad >= currBound && currGrad < nextBound){
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
