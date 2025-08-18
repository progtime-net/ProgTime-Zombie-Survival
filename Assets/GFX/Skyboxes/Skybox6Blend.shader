Shader "Skybox/6SidedBlendSmooth"
                {
                    Properties
                    {
                        _Front1 ("Front Day", 2D) = "white" {}
                        _Back1  ("Back Day", 2D)  = "white" {}
                        _Left1  ("Left Day", 2D)  = "white" {}
                        _Right1 ("Right Day", 2D) = "white" {}
                        _Up1    ("Up Day", 2D)    = "white" {}
                        _Down1  ("Down Day", 2D)  = "white" {}
                
                        _Front2 ("Front Night", 2D) = "black" {}
                        _Back2  ("Back Night", 2D)  = "black" {}
                        _Left2  ("Left Night", 2D)  = "black" {}
                        _Right2 ("Right Night", 2D) = "black" {}
                        _Up2    ("Up Night", 2D)    = "black" {}
                        _Down2  ("Down Night", 2D)  = "black" {}
                
                        _Blend ("Blend Day/Night", Range(0,1)) = 0
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
                
                            sampler2D _Front1; sampler2D _Back1; sampler2D _Left1; sampler2D _Right1; sampler2D _Up1; sampler2D _Down1;
                            sampler2D _Front2; sampler2D _Back2; sampler2D _Left2; sampler2D _Right2; sampler2D _Up2; sampler2D _Down2;
                
                            float4 _Right1_TexelSize, _Left1_TexelSize, _Up1_TexelSize, _Down1_TexelSize, _Front1_TexelSize, _Back1_TexelSize;
                            float4 _Right2_TexelSize, _Left2_TexelSize, _Up2_TexelSize, _Down2_TexelSize, _Front2_TexelSize, _Back2_TexelSize;
                
                            float _Blend;
                
                            struct appdata { float4 vertex : POSITION; };
                            struct v2f { float4 pos : SV_POSITION; float3 dir : TEXCOORD0; };
                
                            float2 SafeUV(float2 uv, float2 texel)
                            {
                                float2 e = texel;
                                uv = saturate(uv);
                                return uv * (1.0 - 2.0 * e) + e;
                            }
                
                            v2f vert(appdata v)
                            {
                                v2f o;
                                o.pos = UnityObjectToClipPos(v.vertex);
                                o.dir = normalize(v.vertex.xyz);
                                return o;
                            }
                
                            half4 frag(v2f i) : SV_Target
                            {
                                float3 dir = i.dir;
                                float3 a = abs(dir);
                                float contraction = 0.495;
                
                                // choose dominant axis only (no cross-face blending)
                                if (a.x >= a.y && a.x >= a.z)
                                {
                                    float2 uv = (dir.x > 0)
                                        ? float2(-dir.z/a.x*contraction + 0.5,  dir.y/a.x*contraction + 0.5)
                                        : float2( dir.z/a.x*contraction + 0.5,  dir.y/a.x*contraction + 0.5);
                
                                    if (dir.x > 0)
                                    {
                                        uv = SafeUV(uv, max(_Right1_TexelSize.xy, _Right2_TexelSize.xy));
                                        return lerp(tex2D(_Right1, uv), tex2D(_Right2, uv), _Blend);
                                    }
                                    else
                                    {
                                        uv = SafeUV(uv, max(_Left1_TexelSize.xy, _Left2_TexelSize.xy));
                                        return lerp(tex2D(_Left1, uv), tex2D(_Left2, uv), _Blend);
                                    }
                                }
                                else if (a.y >= a.z)
                                {
                                    float2 uv = (dir.y > 0)
                                        ? float2( dir.x/a.y*contraction + 0.5, -dir.z/a.y*contraction + 0.5)
                                        : float2( dir.x/a.y*contraction + 0.5,  dir.z/a.y*contraction + 0.5);
                
                                    if (dir.y > 0)
                                    {
                                        uv = SafeUV(uv, max(_Up1_TexelSize.xy, _Up2_TexelSize.xy));
                                        return lerp(tex2D(_Up1, uv), tex2D(_Up2, uv), _Blend);
                                    }
                                    else
                                    {
                                        uv = SafeUV(uv, max(_Down1_TexelSize.xy, _Down2_TexelSize.xy));
                                        return lerp(tex2D(_Down1, uv), tex2D(_Down2, uv), _Blend);
                                    }
                                }
                                else
                                {
                                    float2 uv = (dir.z > 0)
                                        ? float2( dir.x/a.z*contraction + 0.5,  dir.y/a.z*contraction + 0.5)
                                        : float2(-dir.x/a.z*contraction + 0.5,  dir.y/a.z*contraction + 0.5);
                
                                    if (dir.z > 0)
                                    {
                                        uv = SafeUV(uv, max(_Front1_TexelSize.xy, _Front2_TexelSize.xy));
                                        return lerp(tex2D(_Front1, uv), tex2D(_Front2, uv), _Blend);
                                    }
                                    else
                                    {
                                        uv = SafeUV(uv, max(_Back1_TexelSize.xy, _Back2_TexelSize.xy));
                                        return lerp(tex2D(_Back1, uv), tex2D(_Back2, uv), _Blend);
                                    }
                                }
                            }
                            ENDCG
                        }
                    }
                }