//------------------------------------------------------------------------------
// This file is part of MistLand project in GStd.
// Copyright © 2016-2016 GStd Technology Co., Ltd.
// All Right Reserved.
//------------------------------------------------------------------------------

Shader "Game/Grass"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Amount("Amount", Range(0, 1)) = 0.25
		_Speed("Speed", Range(0, 1)) = 0.5
		_XShake("X_Shake", Range(0, 1)) = 0.5
		_ZShake("Z_Shake", Range(0, 1)) = 0.5
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 150

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert noforwardadd

		sampler2D _MainTex;

		struct Input
		{
			half2 uv_MainTex;
		};

		half _Amount;
		half _Speed;
		half _XShake;
		half _ZShake;
		void vert(inout appdata_full v)
		{
			//v.vertex.x += sin(_Speed * _Time.y) * v.color.a * _Amount;
			v.vertex.x += sin(_Speed * _Time.y) * (_XShake - 0.5) * _Amount * v.color.a;
			v.vertex.z += sin(_Speed * _Time.y) * (_ZShake - 0.5) * _Amount * v.color.a;
			
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}

		Fallback "Mobile/VertexLit"
}
