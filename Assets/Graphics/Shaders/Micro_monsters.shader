Shader "Mobile/Micro_Monsters" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.7
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 150

CGPROGRAM
#pragma surface surf Lambert noforwardadd


sampler2D _MainTex;

struct Input {
    float2 uv_MainTex;
};
    float _Cutoff;

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = c.rgb;
    o.Alpha = c.a;
     clip(c.a - _Cutoff);
}
ENDCG
}

Fallback "Mobile/VertexLit"
}