// ScanShader 셰이더
Shader "CustomShader/ScanShader"
{
    Properties
    {
        _MainTex("Screen", 2D) = "black" {}
        [HDR]_Color("Color", Color) = (1, 1, 1, 1)
        _Position("Position", Vector) = (0, 0, 0, 0)
        _Power("Effect Power", Float) = 10
        _Tiling("Texture Tiling", Float) = 1
        _Speed("Effect Speed", Float) = 1
        _MaskRadius("Mask Radius", Float) = 5
        _MaskHardness("Mask Hardness", Range(0, 1)) = 1
        _MaskPower("Mask Power", Float) = 1
        _MultiplyBlend("Multiply Blend", Range(0, 1)) = 0
        _Progress("Progress", Range(0, 1)) = 0
        [HideInInspector] _TexCoord("", 2D) = "white"
    }      

    SubShader
    {
        LOD 0

        ZTest Always
        Cull Off
        ZWrite Off

        Tags
        { 
            "Queue" = "Transparent" 
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img_custom
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            #include "UnityShaderVariables.cginc"

            // 입력 버텍스 구조체 
            struct InputData
            {
                float4 vertex : POSITION;
                half2 texcoord : TEXCOORD0;
            };

            // 출력 버텍스 구조체 
            struct OutputData
            {
                float4 position : SV_POSITION;
                half2 uv : TEXCOORD0;
                half2 stereoUV : TEXCOORD2;
                #if UNITY_UV_STARTS_AT_TOP
                half4 uv2 : TEXCOORD1;
                half4 stereoUV2 : TEXCOORD3;
                #endif
                float4 screenPos : TEXCOORD4;
            };

            // 유니폼 변수 정의
            uniform sampler2D _MainTex;
            uniform half4 _MainTex_TexelSize;
            uniform half4 _MainTex_ST;
            uniform float  _Progress;
            uniform half4 _Color;
            uniform float _MultiplyBlend;
            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            uniform half4 _CameraDepthTexture_TexelSize;
            uniform half3 _Position;
            uniform float _Tiling;
            uniform float _Speed;
            uniform float _Power;
            uniform float _MaskRadius;
            uniform float _MaskHardness;
            uniform float _MaskPower;

            // 스테레오 이미지에 대한 처리
            float2 UnStereo(float2 UV)
            {
                #if UNITY_SINGLE_PASS_STEREO
                float4 scaleOffset = unity_StereoScaleOffset[unity_StereoEyeIndex];
                UV.xy = (UV.xy - scaleOffset.zw) / scaleOffset.xy;
                #endif
                return UV;
            }

            // 깊이 방향 뒤집기 함수
            float3 InvertDepthDir(float3 In)
            {
                float3 result = In;
                #if !defined(ASE_SRP_VERSION) || ASE_SRP_VERSION <= 70301
                result *= float3(1, 1, -1);
                #endif
                return result;
            }

            // 버텍스 셰이더 (주석 추가)
            OutputData vert_img_custom(InputData v)
            {
                OutputData o;
                float4 clipPos = UnityObjectToClipPos(v.vertex);
                float4 screenPos = ComputeScreenPos(clipPos);
                o.screenPos = screenPos;

                o.position = clipPos;
                o.uv = float4(v.texcoord.xy, 1, 1);

                #if UNITY_UV_STARTS_AT_TOP
                o.uv2 = float4(v.texcoord.xy, 1, 1);
                o.stereoUV2 = UnityStereoScreenSpaceUVAdjust(o.uv2, _MainTex_ST);

                if (_MainTex_TexelSize.y < 0.0) o.uv.y = 1.0 - o.uv.y;
                #endif

                o.stereoUV = UnityStereoScreenSpaceUVAdjust(o.uv, _MainTex_ST);
                return o;
            }

            // 프래그먼트 셰이더 (주석 추가)
            half4 frag(OutputData i) : SV_Target
            {
                half4 finalColor = half4(0, 0, 0, 0);
                if (_Progress > 0)
                {
                    #ifdef UNITY_UV_STARTS_AT_TOP
                    half2 uv = i.uv2;
                    half2 stereoUV = i.stereoUV2;
                    #else
                    half2 uv = i.uv;
                    half2 stereoUV = i.stereoUV;
                    #endif

                    // 화면 좌표에서 텍스처 좌표로 변환
                    float2 uv_MainTex = i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;

                    // 텍스처에서 화면의 컬러를 가져오고 색상 계산
                    half4 screenColor = tex2D(_MainTex, uv_MainTex);
                    half4 lerpResult = lerp(_Color, (_Color * screenColor), _MultiplyBlend);

                    // 스크린 좌표를 정규화하여 깊이 정보 가져오기
                    float4 screenPosNorm = i.screenPos / i.screenPos.w;
                    screenPosNorm.z = (UNITY_NEAR_CLIP_VALUE >= 0) ? screenPosNorm.z : screenPosNorm.z * 0.5 + 0.5;

                    // 스테레오 이미지 처리
                    float2 localUnStereo = UnStereo(screenPosNorm.xy);

                    // 깊이 정보 가져오기
                    float clampDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenPosNorm.xy);

                    // 깊이 정보 방향에 따른 스위치 설정
                    #ifdef UNITY_REVERSED_Z
                    float staticSwitch = (1.0 - clampDepth);
                    #else
                    float staticSwitch = clampDepth;
                    #endif

                    // 계산 결과를 백터로 -> 4차원으로 확장 -> 투영 행렬과 계산하여 3D좌표 얻기
                    float3 appendResult = float3(localUnStereo.x, localUnStereo.y, staticSwitch);
                    float4 appendResult2 = float4((appendResult * 2.0 + -1.0), 1.0);
                    float4 tempOutput = mul(unity_CameraInvProjection, appendResult2);
                    float3 tempOutput2 = (tempOutput).xyz / (tempOutput).w;

                    // 깊이 방향 뒤집기
                    float3 localInvertDepthDir = InvertDepthDir(tempOutput2);

                    // 4차원 벡터로 확장
                    float4 appendResult3 = float4(localInvertDepthDir, 1.0);

                    // 카메라 월드 좌표와 원점 위치 계산
                    float3 cameraWorldPosition = mul(unity_CameraToWorld, appendResult3).xyz;
                    float3 originPosition = _Position.xyz;

                    // 위치 차이 , 길이, 진행률*진행속도 구하기
                    float3 positionDifference = cameraWorldPosition - originPosition;
                    float SDF = length(positionDifference);
                    float ProgressSpeed = _Progress * _Speed;

                    // 보간 및 스무딩 계산
                    float lerpValue = lerp(0.0, ((_MaskRadius + 1.0) - 0.001), _MaskHardness);
                    float smoothstepValue = smoothstep((_MaskRadius + 1.0), lerpValue, SDF);

                    // 마스크 및 스캔 효과 계산
                    float SDFMask = pow(smoothstepValue, _MaskPower);
                    float scan = (pow(frac((SDF * _Tiling - ProgressSpeed)), _Power) * SDFMask * _Color.a);

                    // 최종 컬러 계산
                    finalColor = lerp(screenColor, lerpResult, scan);
                }
                return finalColor;
            }
            ENDCG
        }
    }
    CustomEditor "ASEMaterialInspector"
}
