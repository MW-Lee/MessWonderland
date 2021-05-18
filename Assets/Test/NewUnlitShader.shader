Shader "Unlit/NewUnlitShader"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f {
                // Get Pixel UV
                half3 worldNormal : TEXCOORD0;
                // Get Position
                float4 pos : SV_POSITION;
            };

            v2f vert(float4 vertex : POSITION, float3 normal : NORMAL)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                // Caculate World Normal
                o.worldNormal = UnityObjectToWorldNormal(normal);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Create to retun result
                fixed4 col = 0;

                // Caculate color with Normal
                // Normal : -1 ~ 1
                // Normal * 0.5 : -0.5 ~ 0.5
                // Normal * 0.5 + 0.5 : 0 ~ 1
                col.rgb = i.worldNormal * 0.5 + 0.5;

                // Return it
                return col;
            }

            ENDCG
        }
    }
}
