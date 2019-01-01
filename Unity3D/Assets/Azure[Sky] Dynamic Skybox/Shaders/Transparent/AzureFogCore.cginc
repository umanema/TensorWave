uniform float _Azure_FogDistance;
uniform float _Azure_FogBlend;
uniform float _Azure_HeightFogBlend;
uniform float _Azure_HeightFogDistance;
uniform float _Azure_HeightFogStart;
uniform float _Azure_HeightFogEnd;
uniform float _Azure_HeightFogDensity;
uniform sampler2D _Azure_ScatteringTexture;
uniform float4x4 _Azure_UpMatrix;

// The standard method to apply Azure fog scattering.
float4 ApplyAzureFog (float4 fragOutput, float4 projPos, float3 worldPos)
{
	float4 projP = normalize(mul(_Azure_UpMatrix, projPos));
	float3 uv = float3(projPos.xyz / projPos.w);
	float3 fogScatteringColor = tex2D(_Azure_ScatteringTexture, uv.xy).rgb;

	// Calcule Standard Fog.
	float fog = smoothstep(-_Azure_FogBlend, 1.25, projPos.z / _Azure_FogDistance);
	float heightFogDistance = smoothstep(-_Azure_HeightFogBlend, 1.25, projPos.z / _Azure_HeightFogDistance);

	// Calcule Height Fog.
	float3 worldSpaceDirection = mul((float3x3)_Azure_UpMatrix, worldPos.xyz);
	float heightFog = saturate((worldSpaceDirection.y - _Azure_HeightFogStart) / (_Azure_HeightFogEnd + _Azure_HeightFogStart));
	heightFog = 1.0 - heightFog;
	heightFog *= heightFog;
	heightFog *= heightFogDistance;
	float fogFactor = saturate(fog + heightFog * _Azure_HeightFogDensity);

	// Apply Fog.
	fogScatteringColor = lerp(fragOutput.rgb, fogScatteringColor, fogFactor * lerp(fragOutput.a, 1.0, 1.0 - fogFactor));
	return float4(fogScatteringColor, fragOutput.a);
}

// Apply Azure fog scattering to alpha blend particles. (Fix edge artfacts)
float4 BlendAzureFog(float4 fragOutput, float4 projPos, float3 worldPos)
{
	float2 uv = float2(projPos.xy / projPos.w);
	float3 fogScatteringColor = tex2D(_Azure_ScatteringTexture, uv).rgb;

	// Calcule Standard Fog.
	float fog = smoothstep(-_Azure_FogBlend, 1.25, projPos.z / _Azure_FogDistance);
	float heightFogDistance = smoothstep(-_Azure_HeightFogBlend, 1.25, projPos.z / _Azure_HeightFogDistance);

	// Calcule Height Fog.
	float3 worldSpaceDirection = mul((float3x3)_Azure_UpMatrix, worldPos.xyz);
	float heightFog = saturate((worldSpaceDirection.y - _Azure_HeightFogStart) / (_Azure_HeightFogEnd + _Azure_HeightFogStart));
	heightFog = 1.0 - heightFog;
	heightFog *= heightFog;
	heightFog *= heightFogDistance;
	float fogFactor = saturate(fog + heightFog * _Azure_HeightFogDensity);

	// Apply Fog.
	fogScatteringColor = lerp(fragOutput.rgb, fogScatteringColor, fogFactor * lerp(fragOutput.a, 1.0, 2.0 - fogFactor));
	return float4(fogScatteringColor, fragOutput.a);
}

// Apply Azure fog scattering to multiply blend particles.
float4 MultiplyAzureFog(float4 fragOutput, float4 projPos, float3 worldPos)
{
	float2 uv = float2(projPos.xy / projPos.w);
	float3 fogScatteringColor = tex2D(_Azure_ScatteringTexture, uv).rgb;

	// Calcule Standard Fog.
	float fog = smoothstep(-_Azure_FogBlend, 1.25, projPos.z / _Azure_FogDistance);
	float heightFogDistance = smoothstep(-_Azure_HeightFogBlend, 1.25, projPos.z / _Azure_HeightFogDistance);

	// Calcule Height Fog.
	float3 worldSpaceDirection = mul((float3x3)_Azure_UpMatrix, worldPos.xyz);
	float heightFog = saturate((worldSpaceDirection.y - _Azure_HeightFogStart) / (_Azure_HeightFogEnd + _Azure_HeightFogStart));
	heightFog = 1.0 - heightFog;
	heightFog *= heightFog;
	heightFog *= heightFogDistance;
	float fogFactor = saturate(fog + heightFog * _Azure_HeightFogDensity);

	// Apply Fog.
	return float4(fogScatteringColor, lerp(fragOutput.a, 0.0, saturate(1.5 - fogFactor)));
}

// Apply Azure fog scattering to add blend particles.
float4 AddAzureFog(float4 fragOutput, float4 projPos, float3 worldPos)
{
	float2 uv = float2(projPos.xy / projPos.w);
	float3 fogScatteringColor = tex2D(_Azure_ScatteringTexture, uv).rgb;

	// Calcule Standard Fog.
	float fog = smoothstep(-_Azure_FogBlend, 1.25, projPos.z / _Azure_FogDistance);
	float heightFogDistance = smoothstep(-_Azure_HeightFogBlend, 1.25, projPos.z / _Azure_HeightFogDistance);

	// Calcule Height Fog.
	float3 worldSpaceDirection = mul((float3x3)_Azure_UpMatrix, worldPos.xyz);
	float heightFog = saturate((worldSpaceDirection.y - _Azure_HeightFogStart) / (_Azure_HeightFogEnd + _Azure_HeightFogStart));
	heightFog = 1.0 - heightFog;
	heightFog *= heightFog;
	heightFog *= heightFogDistance;
	float fogFactor = saturate(fog + heightFog * _Azure_HeightFogDensity);

	// Apply Fog.
	return float4(fogScatteringColor, lerp(fragOutput.a, 0.0, 1.0 - fogFactor));
}