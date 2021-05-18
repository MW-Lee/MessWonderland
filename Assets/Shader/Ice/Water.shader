Shader "Custom/Water"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("NormalMap", 2D) = "bump" {}
		_CubeMap("CubeMap", Cube) = "white" {}
		_Brightness("Brightness", range(0,1)) = 1
		_Strength("Strength", range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert noambient

        sampler2D _MainTex;
		sampler2D _BumpMap;
		samplerCUBE _CubeMap;

		float _Brightness;
		float _Strength;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldRefl;
			INTERNAL_DATA
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			float3 re = texCUBE(_CubeMap, WorldReflectionVector(IN, o.Normal));
            o.Albedo = c.rgb * _Brightness;
			o.Emission = re.rgb * _Strength;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
