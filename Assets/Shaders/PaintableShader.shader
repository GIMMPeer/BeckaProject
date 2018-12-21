Shader "Custom/PaintableShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)

		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_SecondaryTex ("Secondary (RGB)", 2D) = "white" {}
		_SplatMap ("Splat (RGB)", 2D) = "white" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
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
		sampler2D _SecondaryTex;
		sampler2D _SplatMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_SecondaryTex;
			float2 uv_SplatMap;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 mainTexC = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 secondaryTexC = tex2D(_SecondaryTex, IN.uv_SecondaryTex);
			fixed4 splatTexC = tex2D(_SplatMap, IN.uv_MainTex);

			o.Albedo = (mainTexC * (splatTexC.r)) + (secondaryTexC * (1 - splatTexC.r)); //black is secondary texture white is primary
			//o.Albedo = mainTexC;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			//o.Alpha = mainTexC.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
