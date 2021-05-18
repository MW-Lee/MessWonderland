Shader "Unlit/GoggleRedCulling"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    _RedPower("RedPower",Range(0,1)) = 0
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}

        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _RedPower;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
            if (col.r == 1 && col.g == 0 && col.b == 1) col.a = 0;
            else
            {
                col.a = 1;
              /*  col.g *= 1-_RedPower;
                col.b *= 1-_RedPower;*/
                col.r += 1;
            }
                return col;
            }
            ENDCG
        }
    }
}
