Shader "Game/Wave"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" { }
		_WaveY("Wave y", Range(0, 10)) = 0.1
		_WindSpeed("Wind Speed", Range(50,200)) = 100
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 150

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert noforwardadd

		sampler2D _MainTex;
		half4 _Color;
		half _WaveY;
		half _WindSpeed;
			
		struct Input 
		{
			half2 uv_MainTex;
		};

		void vert(inout appdata_full v)
		{
			half angle = _Time * _WindSpeed;

			if (v.vertex.z < 5)
				v.vertex.y = v.vertex.y + sin(v.vertex.z + v.vertex.x + angle) * _WaveY * v.color.a;

			v.vertex.x = v.vertex.x + v.vertex.z * 0.4;
		}
			
		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "VertexLit"
}