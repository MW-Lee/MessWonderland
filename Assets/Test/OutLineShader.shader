Shader "Unlit/OutLine"
{
	Properties
	{
		_MainColor("LineColor", Color) = (1,1,1,1)
		_Thick("Thick", range(0,1)) = 0.2
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		cull back

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			half4 _MainColor;
			float4 _MainTex_ST;
			float _Thick;

			v2f vert(appdata v)
			{
				v2f o;

				// 월드 행렬을 구함
				float3x3 m_mvp3x3 = float3x3(
					unity_ObjectToWorld._m00, unity_ObjectToWorld._m01, unity_ObjectToWorld._m02,
					unity_ObjectToWorld._m10, unity_ObjectToWorld._m11, unity_ObjectToWorld._m12,
					unity_ObjectToWorld._m20, unity_ObjectToWorld._m21, unity_ObjectToWorld._m22);

				// 월드 행렬의 전치행렬을 구함
				float3x3 m_transpose_mvp3x3 = transpose(m_mvp3x3);

				// 월드 행렬 * 전치행렬 해서 스케일의 제곱행렬을 구함
				float3x3 m_scale_mvp3x3 = m_mvp3x3 * m_transpose_mvp3x3;

				// 스케일행렬 구하기
				float4 v_mvp_scale = float4(sqrt(m_scale_mvp3x3._m00), sqrt(m_scale_mvp3x3._m11), sqrt(m_scale_mvp3x3._m22),0);

				// 각 정점당 두께만큼 확장후 스케일로 나눠줌
				o.vertex = v.vertex + normalize(v.vertex) *  _Thick * (1 / v_mvp_scale);

				// 정점 * (월드 * 뷰 * 투영) 행렬을 곱하면서 위에서 나누었던 스케일 값이 소거됨
				o.vertex = UnityObjectToClipPos(o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				return _MainColor;
			}
			ENDCG
		}
	}
}
