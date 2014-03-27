Shader "Visible Through Wall/Diffuse" 
{
   Properties 
{
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) ", 2D) = "white" {}
    _MainTex2 ("Base (RGB) ThroughWall", 2D) = "white" {}
    
}
 
Category 
{
    SubShader 
    { 
        Tags { "Queue"="Overlay+1" }
 
        Pass
        {
            ZWrite Off
            ZTest Greater
            Lighting Off
            //Color [_Color]
            SetTexture [_MainTex2] {combine texture}
        }
 
        Pass 
        {
            ZTest Less          
            SetTexture [_MainTex] {combine texture}
        }
    }
}
 
FallBack "Diffuse", 1
}