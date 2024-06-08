Shader "Unlit/Notification"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 coords = i.uv;
                coords.x *= 16;

                float2 pointOnLineSegment = float2(clamp(coords.x, 1, 15), 0.5);
                
                float4 sdf = distance(coords, pointOnLineSegment);
                clip(-(sdf * 2 - 1));
                float3 col = float3(255, 0, 0);
                float flash = abs(cos(_Time.y * 2.5f) * 0.7);
                bool border = sdf.x > 0.4;
                if (!border)
                {
                    col = 0.8;
                } else col += flash;
                return float4(col, 1);
            }
            ENDCG
        }
    }
}
