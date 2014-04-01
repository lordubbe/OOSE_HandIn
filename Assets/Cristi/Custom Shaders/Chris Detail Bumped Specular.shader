Shader "Chris Detail Bumped Specular" {
Properties {
_Color ("Main Color", Color) = (1,1,1,1)
_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
 
_Shininess ("Shininess", Range (0.03, 1)) = 0.078125

_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
_Detail ("Detail (RGB) Gloss (A)", 2D) = "gray" {}
_BumpMap ("Normalmap", 2D) = "bump" {}
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

 
struct Input {
float2 uv_MainTex;
float2 uv_Detail;
float2 uv_BumpMap;
float2 uv_BumpMapD;

};
 
void surf (Input IN, inout SurfaceOutput o) {
fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
fixed4 d = tex2D(_Detail, IN.uv_Detail);
c.rgb *= d.rgb*2;

o.Albedo = c.rgb; 
o.Gloss = (c.a/2)+(d.a/2);
o.Specular = _Shininess;
o.Alpha = c.a * _Color.a;            
o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap) *tex2D(_BumpMapD, IN.uv_BumpMapD));


}
ENDCG               
}
 
Fallback "Specular"
}