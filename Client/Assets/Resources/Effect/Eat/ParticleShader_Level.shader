// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Custom/Level/Particle Shader" {
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Alpha ("Alpha", range(0, 1)) = 1
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct appdata_t members alphaBlended)
// #pragma exclude_renderers d3d11
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				float alphaBlended : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color.a = IN.color.a;
				OUT.color.r = IN.color.r * IN.color.a;
				OUT.color.g = IN.color.g * IN.color.a;
				OUT.color.b = IN.color.b * IN.color.a;
				OUT.color.a *= IN.alphaBlended.x;
				
				return OUT;
			}

			sampler2D _MainTex;
			float _Alpha;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 tex = tex2D(_MainTex, IN.texcoord);
				tex.a *= _Alpha;
				tex.rgb *= tex.a;
				return tex * IN.color;
			}
		ENDCG
		}
	}
}
