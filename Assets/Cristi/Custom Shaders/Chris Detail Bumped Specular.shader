Shader "Chris Detail Bumped Specular" {
Properties {
_Color ("Main Color", Color) = (1,1,1,1)
_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
//_BlendColor ("Blend Color", Range (0.03,1)) =  0.5
//_IntBumpTex ("Bump Intensity Base", Range (0.03,1)) =  0.5
//_IntBumpD ("Bump Intensity Detail", Range (0.03,1)) =  0.5
_BaseGlow ("Base Glow", Range (0.03,3)) = 1

_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
_BumpMap ("Normalmap", 2D) = "bump" {}
_Detail ("Detail (RGB) Gloss (A)", 2D) = "gray" {}
_BumpMapD ("Normalmap Detail",2D) = "bump"{}
 

}
 
SubShader {
Tags { "RenderType"="Opaque" }
LOD 400
CGPROGRAM
#pragma surface surf BlinnPhong
 
sampler2D _MainTex;
sampler2D _Detail;
sampler2D _BumpMap; 
sampler2D _BumpMapD;

fixed4 _Color;
float _Shininess;
float _BlendColor;
float _IntBumpTex;
float _IntBumpD;
float _BaseGlow;



 
struct Input {
float2 uv_MainTex;
float2 uv_Detail;
float2 uv_BumpMap;
float2 uv_BumpMapD;

};
 
void surf (Input IN, inout SurfaceOutput o) {
fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
fixed4 d = tex2D(_Detail, IN.uv_Detail);
float3 e = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
float3 f = UnpackNormal(tex2D(_BumpMapD, IN.uv_BumpMapD));

//float3 g = (_IntBumpTex,_IntBumpTex,0);




c.rgb *= d.rgb*2;
o.Albedo = c.rgb; 
//d.a=c.a*_GlowBlend;
o.Gloss = d.a+c.a*_BaseGlow;    
o.Specular = _Shininess;
o.Alpha = c.a * _Color.a;    

e.y += f.y;
e.x += f.x;
o.Normal = normalize(e);

//norm1 = UnpackNormal(tex2D(_DetailNormal, IN.uv_DetailNormal));
//float3 norm1Scaled = (norm1 - float3(0.5, 0.5, 1)) * _LerpAmt;
//o.Normal = normalize(float3(0.5, 0.5, 1) + norm1Scaled);


}
ENDCG               
}
 
Fallback "Specular"
}