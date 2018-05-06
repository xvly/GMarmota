//------------------------------------------------------------------------------
// This file is part of MistLand project in GStd.
// Copyright © 2016-2016 GStd Technology Co., Ltd.
// All Right Reserved.
//------------------------------------------------------------------------------

Shader "Game/Rock"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
		_DecalTex("Decal Texture", 2D) = "white" {}
		_RockScale("Rock Scale", Range(0, 1)) = 0.5
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
            #pragma multi_compile_instancing
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma skip_variants FOG_EXP FOG_EXP2 LIGHTMAP_ON DIRLIGHTMAP_SEPARATE DIRLIGHTMAP_COMBINED DYNAMICLIGHTMAP_ON VERTEXLIGHT_ON

            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                V2F_SHADOW_CASTER;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
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

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            #include "ShaderColor.cginc"
            #include "ShaderTexture.cginc"
            #include "ShaderLighting.cginc"
            #include "ShaderVerticalFog.cginc"
            #include "ShaderAttributes.cginc"

            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile _ ENABLE_VERTICAL_FOG
            #pragma skip_variants FOG_EXP FOG_EXP2 LIGHTMAP_ON DIRLIGHTMAP_SEPARATE DIRLIGHTMAP_COMBINED DYNAMICLIGHTMAP_ON VERTEXLIGHT_ON

            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
                half2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
				half4 uv : TEXCOORD0;
                V2F_VERTEX_ATTRIBUTES(TEXCOORD1, TEXCOORD2, TEXCOORD3, COLOR0, COLOR1, COLOR2)
                LIGHTING_COORDS(4, 5)
                UNITY_FOG_COORDS(6)
                UNITY_VERTEX_OUTPUT_STEREO
            };

			sampler2D _DecalTex;
			half4 _DecalTex_ST;
			fixed _RockScale;

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                // Calculate position.
                o.pos = UnityObjectToClipPos(v.vertex);

                // Calculate vertex attributes.
                VertexAttribute a;
                CALCULATE_VERTEX_ATTRIBUTES(a, v);

                // Calculate the uv.
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.uv, _DecalTex);

				// Transfer data to pixel shader.
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
                
				fixed4 mainColor = tex2D(_MainTex, i.uv.xy);
				col.albedo = mainColor.rgb;
				col.alpha = mainColor.a;

                // Calcualte pixel attributes.
                PixelAttribute a;
                CALCULATE_PIXEL_ATTRIBUTES(a, i);

                // Apply the lighting
                ApplyLighting(col, a);

                // Apply light attenuation
                fixed atten = LIGHT_ATTENUATION(i);

                // Calculate final color.
                fixed4 finalColor = getFinalColor(col, atten);
				fixed gray = 0.3 * finalColor.r + 0.59 * finalColor.g + 0.11 * finalColor.b;
				finalColor.rgb = fixed3(gray, gray, gray);
				finalColor.rgb = lerp(finalColor.rgb, tex2D(_DecalTex, i.uv.zw), _RockScale);

                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                SHADER_APPLY_VFOG(a, finalColor);
                return finalColor;
            }
            ENDCG
        }
    }
}
