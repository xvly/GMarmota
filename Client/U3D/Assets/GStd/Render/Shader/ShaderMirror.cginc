//------------------------------------------------------------------------------
// This file is part of MistLand project in GStd.
// Copyright © 2016-2016 GStd Technology Co., Ltd.
// All Right Reserved.
//------------------------------------------------------------------------------

#ifndef SHADERMIRROR_INCLUDED
#define SHADERMIRROR_INCLUDED

#include "ShaderColor.cginc"

#ifdef ENABLE_MIRROR
#   ifndef REQUIRE_PS_VIEW_REFLECT
#   define REQUIRE_PS_VIEW_REFLECT
#   endif
#   ifndef REQUIRE_PS_NDOTV
#   define REQUIRE_PS_NDOTV
#   endif
#endif

// Mirror contant buffer.
CBUFFER_START(ShaderMirror)
    sampler2D _MirrorTex;
    half _MirrorSpace;
    half _MirrorOpacity;
CBUFFER_END

inline void applyMirror(
    inout half4 finalColor,
    half specularControl,
    half3 viewReflect)
{
    fixed4 mircol = tex2D(_MirrorTex, _MirrorSpace * viewReflect);
    finalColor += mircol * specularControl * _MirrorOpacity;
}

#ifdef ENABLE_MIRROR
#define ApplyMirror(finalColor, col, a) applyMirror(finalColor, col.specularControl, a.viewReflect)
#else
#define ApplyMirror(finalColor, col, a)
#endif

#endif
