Shader "GUI/3D Text Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting Off Cull Back ZWrite Off Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            Color [_Color]
            SetTexture [_MainTex] {
                combine primary, texture * primary
            }
        }
    }
}
