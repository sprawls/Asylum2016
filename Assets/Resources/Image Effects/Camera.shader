Shader "Image Effects/Camera" {
 
Properties {
	_MainTex ("Base (RGB)", RECT) = "white" {}
}
 
SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
 
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"
 
// frag shaders data
uniform sampler2D _MainTex;
uniform float4 lum;
uniform float time;
uniform float noiseFactor;
 
// frag shader
float4 frag (v2f_img i) : COLOR
{	
 
	float4 pix = tex2D(_MainTex, i.uv);
 
	// return pix pixel ;)	
	return pix;
}
ENDCG
 
	}
}
 
Fallback off
 
}