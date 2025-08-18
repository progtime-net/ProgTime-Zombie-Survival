Shader "Skybox/BlendCubemap"
{
    Properties
    {
        _Cubemap1 ("Skybox 1 (Day)", Cube) = "" {}
        _Cubemap2 ("Skybox 2 (Night)", Cube) = "" {}
        _Blend ("Blend Factor", Range(0,1)) = 0
        _Exposure ("Exposure", Range(0,8)) = 1
        _Rotation ("Rotation", Range(0,360)) = 0
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            samplerCUBE _Cubemap1;
            samplerCUBE _Cubemap2;
            float _Blend;
            float _Exposure;
            float _Rotation;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Rotate around Y axis
                float3 texcoord = v.vertex.xyz;
                float theta = radians(_Rotation);
                float s = sin(theta);
                float c = cos(theta);
                float3x3 rotY = float3x3(c,0,-s, 0,1,0, s,0,c);

                o.texcoord = mul(rotY, texcoord);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col1 = texCUBE(_Cubemap1, i.texcoord);
                half4 col2 = texCUBE(_Cubemap2, i.texcoord);
                half4 col = lerp(col1, col2, _Blend);
                return col * _Exposure;
            }
            ENDCG
        }
    }
}
