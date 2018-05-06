Shader "UI/CircleMask"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	//_Color ("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		_Mask("Mask Tex", 2D) = "white" {}
		_EdgeValue("Edge Thick", Range(0, 0.02)) = 0.01
		_EdgeColor("Edge Color", Color) = (0, 0, 0, 1)
		_Color("Color Tint", Color) = (1, 1, 1, 1)
		_Radius("Radius", Float) = 0.1
		_X("_X", Float) = 0.5
		_Y("_Y", Float) = 0.5

		_CircleColor("Circle Color", Color) = (0, 0, 0, 1)
		_CircleEdge("Circle Edge", Range(0, 0.02)) = 0.01
		_CircleRadius("Circle Radius", Float) = 0.1
		_CircleX("Circle X", Float) = 0.45
		_CircleY("Circle Y", Float) = 0.55

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Stencil
	{
		Ref[_Stencil]
		Comp[_StencilComp]
		Pass[_StencilOp]
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
	{
		Name "Default"
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0

#include "UnityCG.cginc"
#include "UnityUI.cginc"

#pragma multi_compile __ UNITY_UI_ALPHACLIP

		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord  : TEXCOORD0;
		float4 worldPosition : TEXCOORD1;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	//fixed4 _Color;
	fixed4 _TextureSampleAdd;
	float4 _ClipRect;

	sampler2D _Mask;
	float _EdgeValue;
	fixed4 _EdgeColor;
	fixed4 _Color;
	float _Radius;
	float _X;
	float _Y;

	fixed4 _CircleColor;
	float _CircleRadius;
	float _CircleEdge;
	float _CircleX;
	float _CircleY;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		UNITY_SETUP_INSTANCE_ID(IN);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
		OUT.worldPosition = IN.vertex;
		OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

		OUT.texcoord = IN.texcoord;

		//OUT.color = IN.color * _Color;
		return OUT;
	}

	sampler2D _MainTex;

	fixed4 frag(v2f IN) : SV_Target
	{
		fixed4 texColor = tex2D(_Mask, IN.texcoord);
	if (texColor.a - 0.5 < 0) discard;

	float circleResult = pow((IN.texcoord.x - _CircleX), 2) + pow((IN.texcoord.y - _CircleY), 2);
	if (circleResult > pow(_CircleRadius, 2) && circleResult < pow(_CircleRadius + _CircleEdge, 2))
	{
		_Color.xyz = _CircleColor.xyz;
		_Color.a = _CircleColor.a;
	}
	else
	{
		float result = pow((IN.texcoord.x - _X), 2) + pow((IN.texcoord.y - _Y), 2);
		if (result < pow(_Radius, 2)) discard;
		if (result < pow(_Radius + _EdgeValue, 2))
		{
			_Color.xyz = _EdgeColor.xyz;
			_Color.a = _EdgeColor.a;
		}
	}

	//half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

	_Color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

#ifdef UNITY_UI_ALPHACLIP
	clip(_Color.a - 0.001);
#endif

	//return color;
	return _Color;
	}
		ENDCG
	}
	}
}