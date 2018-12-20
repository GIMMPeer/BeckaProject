Shader "Custom/StencilEffectOld" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("Tint Color", Color) = (0, 1, 0, 1)
	}
		SubShader{
			Tags { "RenderType" = "Opaque"}

			Stencil {
				Ref 1
				Comp NotEqual
				Pass Keep
			}

			CGPROGRAM
			#pragma surface surf Lambert

			sampler2D _MainTex;
			fixed4 _Color;

			struct Input {
				float2 uv_MainTex;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				half4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = c.rgb * _Color;
				o.Alpha = c.a;
			}
			ENDCG
	}
		FallBack "Diffuse"
}