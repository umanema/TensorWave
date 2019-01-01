Shader "Azure[Sky]/Fog Base"
{
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			uniform sampler2D_float _CameraDepthTexture;
			uniform float4x4  _FrustumCorners;
			uniform float4    _MainTex_TexelSize;

			// Scattering.
			uniform float3 _Azure_Br;
			uniform float3 _Azure_Bm;
			uniform float  _Azure_Kr;
			uniform float  _Azure_Km;
			uniform float  _Azure_Scattering;
			uniform float  _Azure_SunIntensity;
			uniform float  _Azure_NightIntensity;
			uniform float  _Azure_Exposure;
			uniform float4 _Azure_RayleighColor;
			uniform float4 _Azure_MieColor;
			uniform float3 _Azure_MieG;
			uniform float  _Azure_Pi316;
			uniform float  _Azure_Pi14;
			uniform float  _Azure_Pi;

			// Deep Space.
			uniform float4 _Azure_MoonBrightColor;
			uniform float  _Azure_MoonBrightRange;
			uniform float  _Azure_MoonEmission;
			uniform float4 _Azure_MoonColor;

			// Fog.
			//uniform float _Azure_FogDistance;
			//uniform float _Azure_FogBlend;
			uniform float _Azure_MieDistance;
			//uniform float _Azure_HeightFogBlend;
			//uniform float _Azure_HeightFogDistance;
			//uniform float _Azure_HeightFogStart;
			//uniform float _Azure_HeightFogEnd;
			//uniform float _Azure_HeightFogDensity;

			// Directions.
			uniform float3 _Azure_SunDirection;
			uniform float3 _Azure_MoonDirection;

			// Matrix.
			uniform float4x4 _Azure_UpMatrix;
			uniform float4x4 _Azure_SunMatrix;
			uniform float4x4 _Azure_MoonMatrix;
			uniform float4x4 _Azure_StarfieldMatrix;

			// Options.
			uniform float _Azure_LightSpeed;
			uniform float _Azure_MieDepth;
			uniform float _Azure_FogScale;
			uniform int   _Azure_SunsetColorMode;

			// Vertex shader inputs.
			struct appdata
			{
				float4 vertex   : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			// Vertex to Fragment.
			// Vertex shader outputs || Fragment shader inputs.
			struct v2f
			{
				float4 Position        : SV_POSITION;
				float2 uv 	           : TEXCOORD0;
				float4 interpolatedRay : TEXCOORD1;
				float2 uv_depth        : TEXCOORD2;
			};

			// Vertex shader.
			v2f vert(appdata v)
			{
				v2f Output;
				UNITY_INITIALIZE_OUTPUT(v2f, Output);

				v.vertex.z = 0.1;
				Output.Position = UnityObjectToClipPos(v.vertex);
				Output.uv = v.texcoord.xy;
				Output.uv_depth = v.texcoord.xy;
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					Output.uv.y = 1 - Output.uv.y;
				#endif

				//Based on Unity5.6 GlobalFog.
				//--------------------------------
				int index = v.texcoord.x + (2.0 * Output.uv.y);
				Output.interpolatedRay = _FrustumCorners[index];
				Output.interpolatedRay.xyz = mul((float3x3)_Azure_UpMatrix, Output.interpolatedRay.xyz);
				Output.interpolatedRay.w = index;

				return Output;
			}

			// Fragment shader || Pixel shader.
			float4 frag(v2f IN) : SV_Target
			{
				//Initializations.
				//--------------------------------
				float3 inScatter = float3(0.0, 0.0, 0.0);
				float3 nightSky = float3(0.0, 0.0, 0.0);
				float3 fex = float3(0.0, 0.0, 0.0);
				float  r = length(float3(0.0, _Azure_LightSpeed, 0.0));

				//Reconstruct world space position and direction towards this screen pixel.
				//--------------------------------
				float depth = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,UnityStereoTransformScreenSpaceTex(IN.uv_depth))));
				//if (depth == 1.0) return float4(screen, 1.0);
				float3 viewDir = normalize(IN.interpolatedRay.xyz);
				//float3 viewDir = normalize(mul((float3x3)_Azure_SkyUpDirectionMatrix, depth * IN.interpolatedRay.xyz));
				float  sunCosTheta = dot(viewDir, _Azure_SunDirection);
				float mieDepth = saturate(lerp(1.0, viewDir.y * 4, _Azure_MieDepth));

				if (_Azure_SunsetColorMode == 0)
				{
					//Optical Depth.
					//--------------------------------
					//float zenith = acos(saturate(dot(float3(0.0, 1.0, 0.0), viewDir)));
					float zenith = acos(saturate(dot(float3(-1.0, 1.0, -1.0), length(viewDir)))) * _Azure_FogScale;
					float z = cos(zenith) + 0.15 * pow(93.885 - ((zenith * 180.0) / _Azure_Pi), -1.253);
					float SR = _Azure_Kr / z;
					float SM = _Azure_Km / z;

					//Total Extinction.
					//--------------------------------
					fex = exp(-(_Azure_Br*SR + _Azure_Bm * SM));
					float  sunset = clamp(dot(float3(0.0, 1.0, 0.0), _Azure_SunDirection), 0.0, 0.5);
					float3 extinction = lerp(fex, (1.0 - fex), sunset);

					//Scattering.
					//--------------------------------
					//float  rayPhase = 1.0 + pow(sunCosTheta, 2.0);										 //Preetham rayleigh phase function.
					float  rayPhase = 2.0 + 0.5 * pow(sunCosTheta, 2.0);									 //Rayleigh phase function based on the Nielsen's paper.
					float  miePhase = _Azure_MieG.x / pow(_Azure_MieG.y - _Azure_MieG.z * sunCosTheta, 1.5); //The Henyey-Greenstein phase function.

					float sunRise = saturate(dot(float3(0.0, 500.0, 0.0), _Azure_SunDirection) / r);

					float3 BrTheta = _Azure_Pi316 * _Azure_Br * rayPhase * _Azure_RayleighColor.rgb * extinction;
					float3 BmTheta = _Azure_Pi14 * _Azure_Bm * miePhase * _Azure_MieColor.rgb * extinction * sunRise;
					BmTheta *= mieDepth;
					float3 BrmTheta = (BrTheta + BmTheta) / (_Azure_Br + _Azure_Bm);

					inScatter = BrmTheta * _Azure_SunIntensity * (1.0 - fex);
					inScatter *= sunRise;

					//Night Sky.
					//--------------------------------
					BrTheta = _Azure_Pi316 * _Azure_Br * rayPhase * _Azure_RayleighColor.rgb;
					BrmTheta = (BrTheta) / (_Azure_Br + _Azure_Bm);
					nightSky = BrmTheta * _Azure_NightIntensity * (1.0 - fex);
				}
				else
				{
					//Optical Depth
					//--------------------------------
					float zenith = acos(length(viewDir.y));
					//float zenith = acos(saturate(dot(float3(-1.0, 1.0, -1.0), depth))) * _Azure_FogScale;
					float z = cos(zenith) + 0.15 * pow(93.885 - ((zenith * 180.0) / _Azure_Pi), -1.253);
					float SR = _Azure_Kr / z;
					float SM = _Azure_Km / z;

					//Total Extinction.
					//--------------------------------
					fex = exp(-(_Azure_Br*SR + _Azure_Bm * SM));

					//Scattering.
					//--------------------------------
					float  rayPhase = 2.0 + 0.5 * pow(sunCosTheta, 2.0);
					float  miePhase = _Azure_MieG.x / pow(_Azure_MieG.y - _Azure_MieG.z * sunCosTheta, 1.5);

					float sunRise = saturate(dot(float3(0.0, 500.0, 0.0), _Azure_SunDirection) / r);

					float3 BrTheta = _Azure_Pi316 * _Azure_Br * rayPhase * _Azure_RayleighColor.rgb;
					float3 BmTheta = _Azure_Pi14 * _Azure_Bm * miePhase * _Azure_MieColor.rgb * sunRise;
					BmTheta *= mieDepth;
					float3 BrmTheta = (BrTheta + BmTheta) / (_Azure_Br + _Azure_Bm);

					inScatter = BrmTheta * _Azure_SunIntensity * (1.0 - fex);
					inScatter *= sunRise;
					//inScatter *= pow(max(0.5, depth-0.5), 2.0);

					//Night Sky.
					//--------------------------------
					BrmTheta = (BrTheta) / (_Azure_Br + _Azure_Bm);
					nightSky = BrmTheta * _Azure_NightIntensity * (1.0 - fex);
				}

				//Moon Bright.
				//--------------------------------
				float  moonRise = saturate(dot(float3(0.0, 500.0, 0.0), _Azure_MoonDirection) / r);
				float  bright = 1.0 + dot(viewDir, -_Azure_MoonDirection);
				float3 moonBright = 1.0 / (1.0 + bright * _Azure_MoonBrightRange) * _Azure_MoonBrightColor.rgb;
				moonBright += 1.0 / (_Azure_MoonEmission + bright * 200.0) * _Azure_MoonColor.rgb;
				moonBright = moonBright * moonRise * mieDepth;

				//Output.
				//--------------------------------
				float3 OutputColor = inScatter + nightSky + moonBright;

				//Tonemapping.
				OutputColor = saturate(1.0 - exp(-_Azure_Exposure * OutputColor));

				//Color Correction.
				OutputColor = pow(OutputColor, 2.2);
				#ifdef UNITY_COLORSPACE_GAMMA
				OutputColor = pow(OutputColor, 0.4545);
				#else
				OutputColor = OutputColor;
				#endif

				return float4(OutputColor.rgb, 1.0);
			}
			ENDCG
		}
	}
}