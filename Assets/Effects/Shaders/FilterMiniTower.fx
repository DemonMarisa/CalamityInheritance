sampler uImage0 : register(s0);
float3 uColor;
float uOpacity;

float4 FilterMiniTower(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 S = tex2D(uImage0, coords);

    float V = max(S.r, max(S.g, S.b)) + min(S.r, min(S.g, S.b)); // = 2*HSL-L

    float3 tint;
    if (V >= 1.0)
        tint = lerp(uColor, float3(1, 1, 1), V - 1.0); // 亮部：uColor → 白
    else
        tint = lerp(float3(0, 0, 0), uColor, V); // 暗部：黑 → uColor

    S.rgb = lerp(S.rgb, tint * S.a, uOpacity); // 乘法 + 插值合成
    return S;
}

technique Technique1
{
    pass Pass0
    {
        PixelShader = compile ps_3_0 FilterMiniTower();
    }
}