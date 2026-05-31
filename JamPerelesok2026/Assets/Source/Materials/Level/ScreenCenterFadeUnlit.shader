Shader "Custom/ScreenCenterFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)

        _FadeRadius ("Fade Radius", Range(0.01, 1.0)) = 0.5
        _FadePower ("Fade Power", Range(0.1, 8.0)) = 2.0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _Color;
            float _FadeRadius;
            float _FadePower;

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs posInputs =
                    GetVertexPositionInputs(IN.positionOS.xyz);

                OUT.positionCS = posInputs.positionCS;
                OUT.positionWS = posInputs.positionWS;
                OUT.uv = IN.uv;

                return OUT;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                // Получаем clip-space позицию из мировой
                float4 clipPos = TransformWorldToHClip(IN.positionWS);

                // NDC: -1..1
                float2 ndc = clipPos.xy / clipPos.w;

                // Viewport: 0..1
                float2 screenUV = ndc * 0.5 + 0.5;

                float2 center = float2(0.5, 0.5);

                float dist = distance(screenUV, center);

                float normalized = saturate(dist / _FadeRadius);

                float alpha = 1.0 - pow(normalized, _FadePower);

                float4 col =
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv)
                    * _Color;

                col.a *= saturate(alpha);

                return col;
            }

            ENDHLSL
        }
    }
}