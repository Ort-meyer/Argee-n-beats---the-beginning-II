Shader "Custom/SoundShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NormMap("NormalMap (RGB)", 2D) = "" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
        _GlowTex("Glow Tex", 2D) = "black" {}
        _GlowStrength("Glow Multiplier", Range(0, 1)) = 1.0
        _SoundMap("Sound Map", 2D) = "" {}
        _SoundMapPosition("Sound Map Position", Vector) = (0,0,0,0)
        _SoundMapWidth("Sound MapWidth", Float) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
        sampler2D _NormMap;
        sampler2D _GlowTex;
        sampler2D _SoundMap;

		struct Input {
			float2 uv_MainTex;
            float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
        half _GlowStrength;
		fixed4 _Color;
        fixed4 _SoundMapPosition;
        float _SoundMapWidth;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

        // Albedo, normal map, emission(glow?)
        void surf(Input IN, inout SurfaceOutputStandard o) {
            // Sample soundmap
            float2 localPos = float2(IN.worldPos.x - _SoundMapPosition.x, IN.worldPos.z - _SoundMapPosition.z);
            float2 uvCords = float2(((localPos.x / _SoundMapWidth) + 1.0f) / 2.0f, ((localPos.y / _SoundMapWidth) + 1.0f) /2.0f);
            fixed4 soundMapCol = tex2D(_SoundMap, uvCords);

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            float f = 0.5f; // desaturate by 20%
            float L = 0.3f*c.r + 0.6f*c.g + 0.1f*c.b;
            fixed4 cNew;
            cNew.r = c.r + f * (L - c.r);
            cNew.g = c.g + f * (L - c.g);
            cNew.b = c.b + f * (L - c.b);
            cNew *= 0.25f;

            o.Albedo = c.rgb*soundMapCol + cNew.rgb* (1.0f - soundMapCol);
            //o.Albedo = cNew.rgb;
            //o.Albedo = c;
            //o.Albedo = soundMapCol.rgb;
            //o.Albedo = float3(uvCords.x, uvCords.y, 0.0f);

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

            //Norm
            o.Normal = UnpackNormal(tex2D(_NormMap, IN.uv_MainTex));
            fixed4 g = tex2D(_GlowTex, IN.uv_MainTex);
            o.Emission = g.rgb * g.a*_GlowStrength;
        }
		ENDCG
	}
	FallBack "Diffuse"
}


/*
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "ShieldShader"
{
Properties
{
_MainTex("Color (RGB) Alpha (A)", 2D) = "white"
}
SubShader
{
Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
LOD 100
Cull Off
ZWrite Off
Blend SrcAlpha One

Pass
{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
// make fog work
#pragma multi_compile_fog

#include "UnityCG.cginc"

struct appdata
{
float4 vertex : POSITION;
float2 uv : TEXCOORD0;
};

struct v2f
{
float2 uv : TEXCOORD0;
UNITY_FOG_COORDS(1)
float4 vertex : SV_POSITION;
float4 wVertex : WORLDPOS;
};

sampler2D _MainTex;
float4 _MainTex_ST;
uniform float4 _Color;
uniform float _MaxDistance;
uniform float _Distances[20];
uniform float3 _HitPositions[20];

v2f vert(appdata v)
{
v2f o;
o.vertex = UnityObjectToClipPos(v.vertex);
o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//o.wVertex = mul(unity_ObjectToWorld, v.vertex).xyzw;
o.wVertex = v.vertex;
UNITY_TRANSFER_FOG(o,o.vertex);
return o;
}

fixed4 frag(v2f i) : SV_Target
{
float4 textureCol = tex2D(_MainTex, i.uv*50.0f);
fixed4 col = float4(0,0,0,0);

for (int hitIndex = 0; hitIndex < 20; hitIndex++)
{
float distance = length(i.wVertex.xyz - _HitPositions[hitIndex].xyz);

// sample the texture

float width = max(0, pow((_MaxDistance - _Distances[hitIndex]), 2));
if (distance < _Distances[hitIndex] && distance >(_Distances[hitIndex] - width) && _Distances[hitIndex] < _MaxDistance)
{
float alpha = min((distance - (_Distances[hitIndex] - width)) / 1.0f, (_Distances[hitIndex] - distance) / 1.0f);
if (alpha > col.w)
{
col = float4(float3(1, 1, 1)*_Color*textureCol, alpha);
}
}
}

// apply fog
UNITY_APPLY_FOG(i.fogCoord, col);
return col;
}
ENDCG
}
}
}

*/