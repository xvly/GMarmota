//------------------------------------------------------------------------------
// This file is part of MistLand project in GStd.
// Copyright © 2016-2016 GStd Technology Co., Ltd.
// All Right Reserved.
//------------------------------------------------------------------------------

Shader "Game/HighEnd"
{
    Properties
    {
        // Rendering mode.
        _RenderingMode("Rendering Mode", Float) = 0.0
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.1
        _SrcBlend("Alpha Source Blend", Float) = 0.0
        _DstBlend("Alpha Destination Blend", Float) = 0.0
        _ZWrite("Z Write", Float) = 1.0

        // Basic colors.
        _MainTex("Main Texture", 2D) = "white" {}
        _MainColor("Main Color", Color) = (1,1,1,1)
        [HDR]_EmissionColor("Emission Color", Color) = (1,1,1,1)

        // Specular
        _SpecularPower("Specular Power", Range(0, 20)) = 1
        _SpecularIntensity("Specular Intensity", Range(0, 5)) = 1
        _SpecularColor("Specular Color", Color) = (1,1,1,1)

        // Reflection.
        _ReflectionOpacity("Reflection Opacity", Range(0, 1)) = 1
        _ReflectionIntensity("Reflection Intensity", Range(0, 3)) = 1
        _ReflectionFresnel("Reflection Fresnel", Range(0, 5)) = 1
        _ReflectionMetallic("Reflection Metallic", Range(0, 1)) = 0
        _ReflectionCubemap("Reflection Cubemap", Cube) = "defaulttexture" {}

        // Mirror
        _MirrorTex("Mirror Texture", 2D) = "white" {}
        _MirrorSpace("Mirror Space", Range(0, 10)) = 1
        _MirrorOpacity("Mirror Opacity", Range(0, 1)) = 1
            
        // Mask Control
        _MaskControlTex("Mask Control Texture", 2D) = "white" {}

        // Rim
        _RimColor("Rim Color (A)Opacity", Color) = (1,1,1,1)
        _RimIntensity("Rim Intensity", Range(0, 10)) = 1
        _RimFresnel("Rim Fresnel", Range(0, 5)) = 1

        // Rim Light
        _RimLightColor("Rim Light Color (A)Opacity", Color) = (1,1,1,1)
        _RimLightIntensity("Rim Light Intensity", Range(0, 10)) = 1
        _RimLightFresnel("Rim Light Fresnel", Range(0, 5)) = 1

        // The occulde color
        _OccludeColor("Occlusion Color", Color) = (0,0,1,1)
        _OccludePower("Occlusion Power", Range(0.1, 10)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            CGPROGRAM
            #pragma vertex vert   
            #pragma fragment frag

            #include "UnityCG.cginc"

            #pragma multi_compile_shadowcaster
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma shader_feature _ _ALPHA_TEST _ALPHA_BLEND _ALPHA_PREMULTIPLY
            #pragma shader_feature ENABLE_MAIN_COLOR
            #pragma skip_variants FOG_EXP FOG_EXP2 LIGHTMAP_ON DIRLIGHTMAP_SEPARATE DIRLIGHTMAP_COMBINED DYNAMICLIGHTMAP_ON VERTEXLIGHT_ON

            #if defined(_ALPHA_TEST) || defined(_ALPHA_BLEND) || defined(_ALPHA_PREMULTIPLY)
            #   define REQUIRE_SHADOW_ALPHA
            #endif

            fixed _Cutoff;

            sampler2D _MainTex;
            half4 _MainTex_ST;
            fixed4 _MainColor;

            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
#ifdef REQUIRE_SHADOW_ALPHA
                half2 uv : TEXCOORD0;
#endif
            };

            struct v2f
            {
                V2F_SHADOW_CASTER;
#ifdef REQUIRE_SHADOW_ALPHA
                half2 uv : TEXCOORD1;
#endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v)
            {
                v2f o;

                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)

#ifdef REQUIRE_SHADOW_ALPHA
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
#endif

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
#ifdef REQUIRE_SHADOW_ALPHA
                fixed4 col = tex2D(_MainTex, i.uv);
#   ifdef ENABLE_MAIN_COLOR
                col *= _MainColor;
#   endif

#   ifdef _ALPHA_PREMULTIPLY
                col.rgb *= col.a;
#   endif

#   if !defined(_ALPHA_TEST) && !defined(_ALPHA_BLEND) && !defined(_ALPHA_PREMULTIPLY)
                UNITY_OPAQUE_ALPHA(col.a);
#   endif

#   if defined(_ALPHA_TEST)
                clip(col.a - _Cutoff);
#   endif
#endif
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
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

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            #include "ShaderColor.cginc"
            #include "ShaderTexture.cginc"
            #include "ShaderLighting.cginc"
            #include "ShaderReflection.cginc"
            #include "ShaderMirror.cginc"
            #include "ShaderRim.cginc"
            #include "ShaderAttributes.cginc"

            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma shader_feature _ _ALPHA_TEST _ALPHA_BLEND _ALPHA_PREMULTIPLY
            #pragma shader_feature ENABLE_MAIN_COLOR
            #pragma shader_feature _ ENABLE_EMISSION
            #pragma shader_feature _ ENABLE_EMISSION_ALPHA_CONTROL
            #pragma shader_feature ENABLE_SEPCULAR
            #pragma shader_feature ENABLE_REFLECTION
            #pragma shader_feature ENABLE_REFLECTION_CUBEMAP
            #pragma shader_feature ALPHA_IS_METALLIC
            #pragma shader_feature ENABLE_RIM
            #pragma shader_feature ENABLE_RIM_LIGHT
            #pragma shader_feature ENABLE_MASK_CONTROL
            #pragma skip_variants FOG_EXP FOG_EXP2 LIGHTMAP_ON DIRLIGHTMAP_SEPARATE DIRLIGHTMAP_COMBINED DYNAMICLIGHTMAP_ON VERTEXLIGHT_ON

            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
                half2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                SHADER_TEXCOORDS(TEXCOORD0)
                V2F_VERTEX_ATTRIBUTES(TEXCOORD1, TEXCOORD2, TEXCOORD3, COLOR0, COLOR1, COLOR2)
                LIGHTING_COORDS(4, 5)
                UNITY_FOG_COORDS(6)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v)
            {
                v2f o;

                // Calculate position.
                o.pos = UnityObjectToClipPos(v.vertex);

                // Calculate vertex attributes.
                VertexAttribute a;
                CALCULATE_VERTEX_ATTRIBUTES(a, v);

                // Transfer data to pixel shader.
                TRANSFER_UV(o, v);
                TRANSFER_VERTEX_ATTRIBUTES(o, a);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                UNITY_TRANSFER_FOG(o, o.pos);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Calculate the texture.
                ShaderColor col;
                SHADER_COLOR_INITIALIZE(col);
                applyTexture(i.uv, col);

                // Calcualte pixel attributes.
                PixelAttribute a;
                CALCULATE_PIXEL_ATTRIBUTES(a, i);

                // Apply the lighting
                ApplyLighting(col, a);

                // Apply light attenuation
                fixed atten = LIGHT_ATTENUATION(i);

                // Calculate final color.
                fixed4 finalColor = getFinalColor(col, atten);
                ApplyReflection(finalColor, col, a);
                ApplyMirror(finalColor, col, a);
                ApplyRim(finalColor, a);
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                return finalColor;
            }
            ENDCG
        }
        
        Pass
        {
            Name "Add"
            Tags
            {
                "LightMode" = "ForwardAdd"
            }

            Blend One One
			ZWrite Off
			Offset -1, -1

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            #include "ShaderColor.cginc"
            #include "ShaderTexture.cginc"
            #include "ShaderLighting.cginc"
            #include "ShaderAttributes.cginc"

            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma shader_feature _ _ALPHA_TEST _ALPHA_BLEND _ALPHA_PREMULTIPLY
            #pragma shader_feature ENABLE_MAIN_COLOR
            #pragma shader_feature _ ENABLE_EMISSION
            #pragma shader_feature _ ENABLE_EMISSION_ALPHA_CONTROL
            #pragma shader_feature ENABLE_SEPCULAR
            #pragma shader_feature ENABLE_REFLECTION
            #pragma shader_feature ENABLE_REFLECTION_CUBEMAP
            #pragma shader_feature ALPHA_IS_METALLIC
            #pragma shader_feature ENABLE_MASK_CONTROL
            #pragma skip_variants FOG_EXP FOG_EXP2 LIGHTMAP_ON DIRLIGHTMAP_SEPARATE DIRLIGHTMAP_COMBINED DYNAMICLIGHTMAP_ON VERTEXLIGHT_ON

            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
                half2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                SHADER_TEXCOORDS(TEXCOORD0)
                V2F_VERTEX_ATTRIBUTES(TEXCOORD1, TEXCOORD2, TEXCOORD3, COLOR0, COLOR1, COLOR2)
                LIGHTING_COORDS(4, 5)
                UNITY_FOG_COORDS(6)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v)
            {
                v2f o;

                // Calculate position.
                o.pos = UnityObjectToClipPos(v.vertex);

                // Calculate vertex attributes.
                VertexAttribute a;
                CALCULATE_VERTEX_ATTRIBUTES(a, v);

                // Transfer data to pixel shader.
                TRANSFER_UV(o, v);
                TRANSFER_VERTEX_ATTRIBUTES(o, a);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                UNITY_TRANSFER_FOG(o, o.pos);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Calculate the texture.
                ShaderColor col;
                SHADER_COLOR_INITIALIZE(col);
                applyTexture(i.uv, col);

                // Calcualte pixel attributes.
                PixelAttribute a;
                CALCULATE_PIXEL_ATTRIBUTES(a, i);

                // Apply the lighting
                ApplyLighting(col, a);

                // Apply light attenuation
                fixed atten = LIGHT_ATTENUATION(i);

                // Calculate final color.
                fixed4 finalColor = getFinalColor(col, atten);
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                return finalColor;
            }
            ENDCG
        }
    }

    CustomEditor "GameStandardShaderGUI"
}
