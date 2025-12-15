Shader "Doctrina/Glass" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Distort", 2D) = "black" {}
        _Cube ("Reflection Cubemap", Cube) = "_Skybox" {}
    }

    SubShader {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass {
            ZWrite On
            ColorMask 0
        }

        GrabPass {}

        CGPROGRAM
        #pragma surface surf No vertex:vert alpha
        #include "UnityCG.cginc"

        half4 LightingNo (SurfaceOutput s, half3 lightDir, half atten) {
            half4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }

        sampler2D _GrabTexture;
        samplerCUBE _Cube;
        sampler2D _MainTex;
        float4 _Color;

        struct Input {
            float2 uv_MainTex;
            float4 grabUV;
            float3 viewDir; // manually passed from vertex
        };

        void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            float4 hpos = UnityObjectToClipPos(v.vertex);
            o.grabUV = ComputeGrabScreenPos(hpos);
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
        }

        void surf (Input IN, inout SurfaceOutput o) {
            float4 c = tex2D(_MainTex, IN.uv_MainTex);
            c = (0.5 - c) * 0.02;

            // Recalculate world-space normal and reflection vector
            float3 worldNormal = WorldNormalVector(IN, o.Normal);
            float3 reflVec = reflect(-IN.viewDir, worldNormal);
            fixed4 reflcol = texCUBE(_Cube, reflVec);

            half orim = dot(normalize(IN.viewDir), o.Normal);
            half rim = pow(orim, 0.2);

            float3 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(IN.grabUV + float4(c.r, c.g, 0, 1 - rim)));

            o.Albedo = (col * _Color.rgb * 0.65 + reflcol.rgb * reflcol.rgb * 0.1) * rim + (pow(1 - orim, 5) / 3);
            o.Alpha = 1;
        }
        ENDCG
    }

    Fallback "Diffuse"
}