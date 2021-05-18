Shader "Test/GoggleTest"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        Power("Power", Int) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM
        #pragma surface surf SimpleLambert

        half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten)
        {
            half NdotL = dot(s.Normal, lightDir);
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
            c.a = s.Alpha;
            return c;
        }

        int Power = 0;

        struct Input 
        {
            float2 uv_MainTex;            
        };

        sampler2D _MainTex;

        void surf(Input IN, inout SurfaceOutput o)
        {            
            if (Power == 1)
            {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
                o.Albedo.r += 1;
            }
            else
            {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            }
        }
        ENDCG
    }
    Fallback "Diffuse"
}
