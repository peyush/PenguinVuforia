//==============================================================================
//Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
//All Rights Reserved.
//==============================================================================

Shader "Vuforia/DynamicMeshSurface"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "gray" {}
        _Fill ("Fill", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent+1"
            "RenderType" = "Transparent"
        }

        ZWrite On
        ZTest LEqual
        Cull Back
        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha

    CGPROGRAM
        #pragma surface surf BlinnPhong alpha:fade

        struct Input
        {
			float3 worldPos;         
        };

        sampler2D _MainTex;

        void surf (Input IN, inout SurfaceOutput o)
        {
        	float2 worldUV = IN.worldPos.xz / 5;
            float4 color = tex2D(_MainTex, worldUV);
            o.Albedo = color.rgb;
            o.Alpha = color.a * 0.3;
        }
    ENDCG
    }

    Fallback "Diffuse"
}
