//------------------------------------------------------------------------------
// This file is part of MistLand project in GStd.
// Copyright © 2016-2016 GStd Technology Co., Ltd.
// All Right Reserved.
//------------------------------------------------------------------------------

Shader "Game/ParticleUI"
{
    Properties
    {
        // Rendering mode.
        _RenderingMode("Rendering Mode", Float) = 0.0
        _CullMode("Cull Mode", Float) = 0.0
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.1
		_CustomRenderQueue("Custom Render Queue", Float) = 0

        _SrcBlend("Alpha Source Blend", Float) = 0.0
        _DstBlend("Alpha Destination Blend", Float) = 0.0
        _ZWrite("Z Write", Float) = 0.0

        // Basic colors.
        _MainTex("Main Texture", 2D) = "white" {}
		_TintColor("Tine Color", Color) = (0.5,0.5,0.5,0.5)

        // Decal.
        _DecalTex("Decal Texture", 2D) = "white" {}

		// Disslove.
		_DissloveTex("Disslove Texture", 2D) = "white" {}
        _DissloveAmount("Disslove Amount", Range(0.0, 1.01)) = 0.1

		// UVNoise
		_UVNoise("UV Noise", 2D) = "black" {}
		_UVNoiseBias("UV Noise Bias", Range(-1, 1)) = 0.6
        _UVNoiseIntensity("UV Noise Bias", Range(0, 1)) = 0.5
        _UVNoiseSpeed("UV Noise Speed", Vector) = (0, 0, 0, 0)

        // Rim
        _RimColor("Rim Color (A)Opacity", Color) = (1,1,1,1)
        _RimIntensity("Rim Intensity", Range(0, 10)) = 1
        _RimFresnel("Rim Fresnel", Range(0, 5)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
			"PreviewType" = "Plane"
        }

		Stencil{
			Ref 1
			Comp equal
		}

        Pass
        {
            Name "Main"
            Tags
            {
                "LightMode" = "ForwardBase"
            }

				

            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]
            Cull [_CullMode]
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #pragma multi_compile_particles
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma fragmentoption ARB_precision_hint_fastest
			#pragma shader_feature _ALPHA_TEST _ALPHA_BLEND _ALPHA_PREMULTIPLY
			#pragma shader_feature _ _CHANNEL_R _CHANNEL_G _CHANNEL_B _CHANNEL_A
			#pragma shader_feature _ _DECAL_CHANNEL_R _DECAL_CHANNEL_G _DECAL_CHANNEL_B _DECAL_CHANNEL_A
            #pragma shader_feature ENABLE_TINT_COLOR
            #pragma shader_feature ENABLE_DECAL
            #pragma shader_feature ENABLE_DISSLOVE
			#pragma shader_feature ENABLE_UV_NOISE
            #pragma shader_feature ENABLE_RIM
            #pragma shader_feature ENABLE_FOG
            #pragma skip_variants FOG_EXP FOG_EXP2 DIRLIGHTMAP_SEPARATE DIRLIGHTMAP_COMBINED DYNAMICLIGHTMAP_ON LIGHTMAP_ON VERTEXLIGHT_ON

			

			// Main texture.
            sampler2D _MainTex;
            half4 _MainTex_ST;
            half4 _TintColor;
            fixed _Cutoff;

            // Decal texture.
            sampler2D _DecalTex;
            half4 _DecalTex_ST;

			// Disslove.
			sampler2D _DissloveTex;
			half4 _DissloveTex_ST;
            fixed _DissloveAmount;

            // Noise
			sampler2D _UVNoise;
			float4 _UVNoise_ST;
			half _UVNoiseBias;
            half _UVNoiseIntensity;
			half4 _UVNoiseSpeed;

            // Rim.
            fixed4 _RimColor;
            fixed _RimFresnel;
            fixed _RimIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
                half2 uv : TEXCOORD0;
                fixed4 color : COLOR0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
#if ENABLE_UV_NOISE
				half4 uv : TEXCOORD0;
#else
                half2 uv : TEXCOORD0;
#endif
#if defined(ENABLE_DECAL) && defined(ENABLE_DISSLOVE)
                half4 uv2 : TEXCOORD1;
#elif defined(ENABLE_DECAL) || defined(ENABLE_DISSLOVE)
				half2 uv2 : TEXCOORD1;
#endif
#if ENABLE_RIM
                half3 worldNormal : TEXCOORD2;
                half3 viewDir : TEXCOORD3;
#endif
                half4 color : COLOR0;
#if ENABLE_FOG
                UNITY_FOG_COORDS(4)
#endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                // Position, color and UV.
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
#if ENABLE_UV_NOISE
				o.uv.zw = TRANSFORM_TEX(v.uv, _UVNoise);
#endif

#if defined(ENABLE_DECAL) && defined(ENABLE_DISSLOVE)
                o.uv2.xy = TRANSFORM_TEX(v.uv, _DecalTex);
                o.uv2.zw = TRANSFORM_TEX(v.uv, _DissloveTex);
#elif defined(ENABLE_DECAL)
                o.uv2 = TRANSFORM_TEX(v.uv, _DecalTex);
#elif defined(ENABLE_DISSLOVE)
				o.uv2 = TRANSFORM_TEX(v.uv, _DissloveTex);
#endif

#if ENABLE_RIM
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
#endif

#if ENABLE_FOG
                UNITY_TRANSFER_FOG(o, o.pos);
#endif

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
#if ENABLE_UV_NOISE
				half2 uvNoise = i.uv.zw;
                uvNoise.xy += _Time.y * _UVNoiseSpeed.zw;
				uvNoise.xy = frac(uvNoise.xy);
				half2 noise = _UVNoiseBias + tex2D(_UVNoise, uvNoise).rg;
                noise *= _UVNoiseIntensity;

				half2 uvTex = i.uv.xy;
                uvTex.xy += _Time.y * _UVNoiseSpeed.xy;
				fixed4 col = tex2D(_MainTex, uvTex + noise);
#else
				fixed4 col = tex2D(_MainTex, i.uv.xy);
#endif

#if _CHANNEL_R
				col = fixed4(col.r, col.r, col.r, col.r);
#elif _CHANNEL_G
				col = fixed4(col.g, col.g, col.g, col.g);
#elif _CHANNEL_B
				col = fixed4(col.b, col.b, col.b, col.b);
#elif _CHANNEL_A
				col = fixed4(col.a, col.a, col.a, col.a);
#endif

#if ENABLE_TINT_COLOR
				col *= 2.0 * i.color * _TintColor;
#else
				col *= i.color;
#endif

#if defined(ENABLE_DECAL) && defined(ENABLE_DISSLOVE)
                fixed4 decal = tex2D(_DecalTex, i.uv2.xy);
#	if _DECAL_CHANNEL_R
				decal = fixed4(decal.r, decal.r, decal.r, decal.r);
#	elif _DECAL_CHANNEL_G
				decal = fixed4(decal.g, decal.g, decal.g, decal.g);
#	elif _DECAL_CHANNEL_B
				decal = fixed4(decal.b, decal.b, decal.b, decal.b);
#	elif _DECAL_CHANNEL_A
				decal = fixed4(decal.a, decal.a, decal.a, decal.a);
#	endif
                col *= decal;

                fixed4 disslove = tex2D(_DissloveTex, i.uv2.zw);
                clip(disslove.a - _DissloveAmount);
#elif defined(ENABLE_DECAL)
                fixed4 decal = tex2D(_DecalTex, i.uv2);
#	if _DECAL_CHANNEL_R
				decal = fixed4(decal.r, decal.r, decal.r, decal.r);
#	elif _DECAL_CHANNEL_G
				decal = fixed4(decal.g, decal.g, decal.g, decal.g);
#	elif _DECAL_CHANNEL_B
				decal = fixed4(decal.b, decal.b, decal.b, decal.b);
#	elif _DECAL_CHANNEL_A
				decal = fixed4(decal.a, decal.a, decal.a, decal.a);
#	endif
                col *= decal;
#elif defined(ENABLE_DISSLOVE)
                fixed4 disslove = tex2D(_DissloveTex, i.uv2);
                clip(disslove.a - _DissloveAmount);
#endif

#ifdef _ALPHA_PREMULTIPLY
                col.rgb *= col.a;
#endif

#if !defined(_ALPHA_TEST) && !defined(_ALPHA_BLEND) && !defined(_ALPHA_PREMULTIPLY)
                UNITY_OPAQUE_ALPHA(col.a);
#endif

#ifdef ENABLE_RIM
                fixed rimOpacity = pow(1 - saturate(dot(i.viewDir, i.worldNormal)), _RimFresnel);
                col.rgb = lerp(col.rgb, _RimColor.rgb * _RimIntensity, rimOpacity);
#endif

#if _ALPHA_TEST
                clip(col.a - _Cutoff);
#endif

#if ENABLE_FOG
                // fog towards black due to our blend mode
                UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0, 0, 0, 0));
#endif
                return col;
            }
            ENDCG
        }
    }

    CustomEditor "GameParticleShaderGUI"
}
