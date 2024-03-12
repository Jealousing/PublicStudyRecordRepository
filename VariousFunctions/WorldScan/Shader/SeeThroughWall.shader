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

			// �Է� ���ؽ� ����ü ����
            struct InputData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			// ��� ���ؽ� ����ü ����
			struct OutputData
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 viewDir : TEXCOORD1;
			};

			// �ؽ�ó �� ���� ������Ƽ ����
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _EdgeColor;
			
			// ���ؽ� ���̴�
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
				// ������ �� ���� ������ ���� ��� -> �����ڸ� ����� �����Ͽ� ���� ���� ����
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

			// �Է� ���ؽ� ����ü ����
            struct InputData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			// ��� ���ؽ� ����ü ����
			struct OutputData
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			// �ؽ�ó ������Ƽ ����
			sampler2D _MainTex;
			float4 _MainTex_ST;

			// ���ؽ� ���̴�
			OutputData vert (InputData v)
			{
				OutputData o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			// �����׸�Ʈ ���̴�
			fixed4 frag (OutputData i) : SV_Target
			{
				// ���� �ؽ�ó ���ø� �� ���� ��ȯ
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}