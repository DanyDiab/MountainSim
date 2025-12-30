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
                float4 currTex = (0,0,0,0); 
                if(currGrad > _Bounds[_numBounds - 1]){
                    currTex = blendTextures(0,_numBounds - 1,_numBounds - 1,i.uv);
                }
                else if(currGrad < _Bounds[0]){
                    currTex = blendTextures(0,0,0,i.uv);
                }
                for(int idx = 0; idx < _numBounds - 1; idx++){
                    float currBound = _Bounds[idx];
                    float nextBound = _Bounds[idx + 1];

                    float selectionMask = step(currBound, currGrad) && step(currGrad, nextBound);
                    
                    float percent = saturate((currGrad - currBound) / (nextBound - currBound));
                    currTex = (selectionMask == 0) ? currTex : blendTextures(percent,idx,idx+1,i.uv) * selectionMask;
                }
                return currTex;
            }
            ENDCG
                
        }
    }
}