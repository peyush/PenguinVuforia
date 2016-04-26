//==============================================================================
//Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
//All Rights Reserved.
//==============================================================================

Shader "Vuforia/SimpleIce"
{
    Properties
    {
        _Color ("Tint (RGB)", Color) = (1,1,1,1)
        _SurfaceTex ("Texture (RGB)", 2D) = "white" {}
        _RampTex ("Facing Ratio Ramp (RGB)", 2D) = "white" {}
        _Reflections ("Reflection SphereMap", 2D) = "White" {}
        _ColorMap        ("Noise Map (Greyscale)", 2D) = "white" {}
        _DistanceRatio   ("Distance Ratio", Vector)    = (0.5, 0.5, 64, 96)
        _MaxAlpha        ("Max Alpha", Range (0, 1)) = 1
		_CutOff ("Cutoff Value", Range(0,1)) = 0.5
    }
    SubShader
    {
        ZWrite Off
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM 
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_fog_exp2
            #include "UnityCG.cginc" 

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 normal : TEXCOORD2;
            };
            
            uniform float _Offset;
            uniform float4 _Color;
            uniform sampler2D _RampTex : register(s0);
            uniform sampler2D _SurfaceTex : register(s1);
            sampler2D _ColorMap;
            sampler2D _Reflections;
            float4 _DistanceRatio;
            float _DeltaTime;
            float _MaxAlpha;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                
                float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                v.texcoord.x = v.texcoord.x;
                v.texcoord.y = v.texcoord.y;
                
                o.uv = TRANSFORM_UV (1);
                o.uv2 = float2( abs (dot (viewDir, v.normal)), 0.5);
                o.normal = v.normal;
                return o;
            }
            

            
   
                
            half4 frag (v2f f) : COLOR 
            {
                f.normal = normalize (f.normal);
                
                // Get the texture with spherical mapping
                float4 reflectedColor = tex2D(_Reflections, f.uv2);
                
                half4 ramp = tex2D (_RampTex, f.uv2) * _Color.a;
               
                half4 thisTex = tex2D (_SurfaceTex, f.uv) * ramp * _Color;
                
                float4 noise = tex2D(_ColorMap, f.uv);
                float2 relative = float2((0.5 - abs(f.uv.x - 0.5)) * _DistanceRatio.x, (0.5 - abs(f.uv.y - 0.5)) * _DistanceRatio.y);
                float dist = relative.x * relative.x + relative.y * relative.y;
                float alpha = min(_MaxAlpha, _DeltaTime - dist * _DistanceRatio.z - dist * _DistanceRatio.w * noise.r);

                half4 result = half4 (thisTex.r, thisTex.g, thisTex.b, ramp.r * 0.75f * alpha);
                result.rgb += reflectedColor.rgb;
                return result;
            }
            
            ENDCG
	
        }

    }
    Fallback "Transparent/VertexLit"
}
