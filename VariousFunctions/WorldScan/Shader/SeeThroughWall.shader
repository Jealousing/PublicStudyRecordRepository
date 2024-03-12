Shader "CustomShader/SeeThroughWall"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_EdgeColor("Edge Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			ZTest Greater
			Blend One One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			
			#include "UnityCG.cginc"

			// 입력 버텍스 구조체 정의
            struct InputData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			// 출력 버텍스 구조체 정의
			struct OutputData
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 viewDir : TEXCOORD1;
			};

			// 텍스처 및 색상 프로퍼티 선언
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _EdgeColor;
			
			// 버텍스 셰이더
			OutputData  vert (InputData v)
			{
				OutputData o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);

				return o;
			}
			
			fixed4 frag (OutputData i) : SV_Target
			{
				// 법선과 뷰 방향 사이의 내적 계산 -> 가장자리 색상과 결합하여 최종 색상 생성
				float NdotV = 1 - dot(i.normal, i.viewDir) * 1.5;
				return _EdgeColor * NdotV;
			}
			ENDCG
		}

		Pass
		{
			ZTest Less 

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			
			#include "UnityCG.cginc"

			// 입력 버텍스 구조체 정의
            struct InputData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			// 출력 버텍스 구조체 정의
			struct OutputData
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			// 텍스처 프로퍼티 선언
			sampler2D _MainTex;
			float4 _MainTex_ST;

			// 버텍스 셰이더
			OutputData vert (InputData v)
			{
				OutputData o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			// 프래그먼트 셰이더
			fixed4 frag (OutputData i) : SV_Target
			{
				// 메인 텍스처 샘플링 및 색상 반환
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}