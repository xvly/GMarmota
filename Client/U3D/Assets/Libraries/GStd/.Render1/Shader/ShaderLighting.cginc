//------------------------------------------------------------------------------
// This file is part of MistLand project in GStd.
// Copyright © 2016-2016 GStd Technology Co., Ltd.
// All Right Reserved.
//------------------------------------------------------------------------------

#ifndef SHADERLIGHTING_INCLUDED
#define SHADERLIGHTING_INCLUDED

#include "ShaderColor.cginc"

// Require the necessary resource.
#ifndef REQUIRE_PS_AMBIENCE
#define REQUIRE_PS_AMBIENCE
#endif

// Require the necessary resource.
#ifndef REQUIRE_PS_NDOTL
#define REQUIRE_PS_NDOTL
#endif

#ifdef ENABLE_SEPCULAR
#   ifndef REQUIRE_PS_NDOTV
#   define REQUIRE_PS_NDOTV
#   endif
#endif

// Rim contant buffer.
CBUFFER_START(ShaderLighting)
    fixed _SpecularPower;
    fixed _SpecularIntensity;
    fixed3 _SpecularColor;
CBUFFER_END

inline void applyLighting(
    inout ShaderColor col,
    half nDotL,
#ifdef ENABLE_SEPCULAR
    half nDotV,
#endif
    half3 ambience)
{
    // Ambience
    col.ambience = ambience;

    // Diffuse
#ifndef LIGHTMAP_ON
    col.diffuse = _LightColor0.rgb * nDotL;
#else
    col.diffuse = half3(1.0, 1.0, 1.0);
#endif

	// Specular
#ifdef ENABLE_SEPCULAR
    col.specular = pow(nDotV, _SpecularPower) * _SpecularIntensity * _SpecularColor;
#endif
}

#ifdef ENABLE_SEPCULAR
#define ApplyLighting(col, a) applyLighting(col, a.nDotL, a.nDotV, a.ambience)
#else
#define ApplyLighting(col, a) applyLighting(col, a.nDotL, a.ambience)
#endif

#endif
