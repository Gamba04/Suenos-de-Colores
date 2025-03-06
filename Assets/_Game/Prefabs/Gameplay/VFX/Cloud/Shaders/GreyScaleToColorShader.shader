Shader "Custom/GreyscaleToColorShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Gradient] _GradientRamp ("Gradient Ramp", 2D) = "white" {}

        [Space][Header(Extras)][Space]
        [Enum(One,1,SrcAlpha,13)] _BlendMode("Blend Mode", Float) = 1

        [Enum(UnityEngine.Rendering.CompareFunction)]_StencilComp("Stencil Comparison", Float) = 4
    }
    SubShader
    {
        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
        }

        Blend SrcAlpha [_BlendMode]
        ZTest [_StencilComp]
        Cull Off
        ZWrite Off

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
                half4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _GradientRamp;

            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);

                half4 output = tex2D(_GradientRamp, col.r);

                output.rgb *= i.color;

                output.a *= col.a * i.color.a;

                return output;
            }
            ENDCG
        }
    }
}
