Shader "Custom/DistortedBubbleShader"
{
    Properties {
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _MainTex ("Particle Texture", 2D) = "white" {}
    _NoiseTex ("Noise Texture", 2D) = "white" {}
    _Speed ("Speed", Range(0, 30)) = 2
    _NoiseAmount ("NoiseAmount", Range(0, 0.1)) = 0.025
}

Category {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
    Blend SrcAlpha OneMinusSrcAlpha
    ColorMask RGB
    Cull Off Lighting Off ZWrite Off
    Fog {Mode Off}

    SubShader {
        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_particles
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _TintColor;

            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float randomNo : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float randomNo : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float4 _MainTex_ST;

            float _Speed;
            sampler2D _NoiseTex;
            float _NoiseAmount;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color * _TintColor;
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.randomNo = v.randomNo;
                return o;
            }

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

            fixed4 frag (v2f i) : SV_Target
            {
                float2 nUv = i.texcoord;
                nUv.y += _Time.x * _Speed + i.randomNo;
                float4 uvNoise = 2 * tex2D(_NoiseTex, nUv) -1;//0 - 1座標を-1 - 1に変換
                i.texcoord += uvNoise.xy * _NoiseAmount;

                fixed4 col = 2.0f * i.color * tex2D(_MainTex, i.texcoord);
                col.a = saturate(col.a); // alpha should not have double-brightness applied to it, but we can't fix that legacy behavior without breaking everyone's effects, so instead clamp the output to get sensible HDR behavior (case 967476)

                return col;
            }
            ENDCG
        }
    }
}
}
