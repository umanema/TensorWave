Shader "Azure[Sky]/Vertex Sky"
{
	SubShader
	{
		Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" "IgnoreProjector"="True" }
	    Cull [_Azure_CullMode] // Render side
		Fog{Mode Off}          // Don't use fog
    	ZWrite Off             // Don't draw to bepth buffer

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		    #pragma target 3.0

			uniform int         _Azure_SunsetColorMode;
			uniform float       _Azure_Pi316, _Azure_Pi14, _Azure_SunIntensity, _Azure_SunDiskIntensity, _Azure_Pi, _Azure_Exposure, _Azure_NightIntensity, _Azure_LightSpeed, _Azure_SunSize, _Azure_MoonSize,
								_Azure_MoonBrightRange, _Azure_MoonEmission, _Azure_StarfieldIntensity, _Azure_MilkyWayIntensity, _Azure_Kr, _Azure_Km;
			uniform float3      _Azure_SunDirection, _Azure_MoonDirection, _Azure_Br, _Azure_Bm, _Azure_MieG, _Azure_StarfieldColorBalance;
			uniform float4      _Azure_RayleighColor, _Azure_MieColor, _Azure_MoonColor, _Azure_MoonBrightColor;
			uniform float4x4    _Azure_SunMatrix, _Azure_MoonMatrix, _Azure_StarMatrix, _Azure_NoiseMatrix, _Azure_RelativeSunMatrix, _Azure_UpMatrix;
			uniform sampler2D   _Azure_SunTexture, _Azure_MoonTexture;
			uniform samplerCUBE _Azure_StarfieldTexture, _Azure_StarNoiseTexture;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 Position : SV_POSITION;
				float3 WorldPos : TEXCOORD0;
				float3 SunPos   : TEXCOORD1;
				float3 MoonPos  : TEXCOORD2;
				float3 StarPos  : TEXCOORD3;
				float3 NoiseRot : TEXCOORD4;
				float4 OutColor : COLOR;
				float4 Fex      : COLOR1;
			};
			
			v2f vert (appdata v)
			{
				v2f Output;
				UNITY_INITIALIZE_OUTPUT(v2f, Output);

				Output.Position = UnityObjectToClipPos(v.vertex);
				Output.WorldPos = normalize(mul((float3x3)unity_WorldToObject, v.vertex.xyz));
				Output.WorldPos = normalize(mul((float3x3)_Azure_UpMatrix, Output.WorldPos));

				//Matrix.
			    //--------------------------------
				Output.SunPos  = mul((float3x3)_Azure_SunMatrix, v.vertex.xyz) * 2.0 * _Azure_SunSize;
				Output.StarPos  = mul((float3x3)_Azure_RelativeSunMatrix, Output.WorldPos);
				Output.StarPos  = mul((float3x3)_Azure_StarMatrix, Output.StarPos);
				Output.NoiseRot = mul((float3x3)_Azure_NoiseMatrix, v.vertex.xyz);
				Output.MoonPos  = mul((float3x3)_Azure_MoonMatrix, v.vertex.xyz) * 1.5 * _Azure_MoonSize;
				Output.MoonPos.x *= -1.0;

				//Initializations.
				//--------------------------------
				float3 inScatter = float3(0.0, 0.0, 0.0);
				float3 nightSky  = float3(0.0, 0.0, 0.0);
				float3 fex = float3(0.0, 0.0, 0.0);
				float  r = length(float3(0.0, _Azure_LightSpeed, 0.0));

				//Directions.
				//--------------------------------
				float3 viewDir     = normalize(Output.WorldPos);
				float  sunCosTheta = dot(viewDir, _Azure_SunDirection);
				float  sunRise     = saturate(dot(float3(0.0, 500.0, 0.0), _Azure_SunDirection) / r);

				if(_Azure_SunsetColorMode == 0)
				{
					//Optical Depth.
					//--------------------------------
					float zenith = acos(saturate(dot(float3(0.0, 1.0, 0.0), viewDir)));
					float z      = cos(zenith) + 0.15 * pow(93.885 - ((zenith * 180.0) / _Azure_Pi), -1.253);
					float SR     = _Azure_Kr / z;
					float SM     = _Azure_Km / z;

					//Total Extinction.
					//--------------------------------
					fex = exp(-(_Azure_Br*SR  + _Azure_Bm*SM));
					float  sunset = clamp(dot(float3(0.0, 1.0, 0.0), _Azure_SunDirection), 0.0, 0.5);
					float3 extinction = lerp(fex, (1.0 - fex), sunset);

					//Scattering.
					//--------------------------------
					//float  rayPhase = 1.0 + pow(sunCosTheta, 2.0);										 //Preetham rayleigh phase function.
					float  rayPhase = 2.0 + 0.5 * pow(sunCosTheta, 2.0);									 //Rayleigh phase function based on the Nielsen's paper.
					float  miePhase = _Azure_MieG.x / pow(_Azure_MieG.y - _Azure_MieG.z * sunCosTheta, 1.5); //The Henyey-Greenstein phase function.

					float3 BrTheta  = _Azure_Pi316 * _Azure_Br * rayPhase * _Azure_RayleighColor.rgb * extinction;
					float3 BmTheta  = _Azure_Pi14  * _Azure_Bm * miePhase * _Azure_MieColor.rgb * extinction * sunRise;
					float3 BrmTheta = (BrTheta + BmTheta) / (_Azure_Br + _Azure_Bm);

					inScatter  = BrmTheta * _Azure_SunIntensity * (1.0 - fex);
					inScatter *= sunRise;

					//Night Sky.
					//--------------------------------
					BrTheta  = _Azure_Pi316 * _Azure_Br * rayPhase * _Azure_RayleighColor.rgb;
					BrmTheta = (BrTheta) / (_Azure_Br + _Azure_Bm);
					nightSky = BrmTheta * _Azure_NightIntensity * (1.0 - fex);
				}
				else
					{
						//Optical Depth
						//--------------------------------
						float zenith = acos(length(viewDir.y));
						//float zenith = acos(saturate(dot(float3(0.0, 1.0, 0.0), viewDir)));
						float z      = cos(zenith) + 0.15 * pow(93.885 - ((zenith * 180.0) / _Azure_Pi), -1.253);
						float SR     = _Azure_Kr / z;
						float SM     = _Azure_Km / z;

						//Total Extinction.
						//--------------------------------
						fex = exp(-(_Azure_Br*SR  + _Azure_Bm*SM));

						//Scattering.
						//--------------------------------
						float  rayPhase = 2.0 + 0.5 * pow(sunCosTheta, 2.0);
						float  miePhase = _Azure_MieG.x / pow(_Azure_MieG.y - _Azure_MieG.z * sunCosTheta, 1.5);

						float3 BrTheta  = _Azure_Pi316 * _Azure_Br * rayPhase * _Azure_RayleighColor.rgb;
						float3 BmTheta  = _Azure_Pi14  * _Azure_Bm * miePhase * _Azure_MieColor.rgb * sunRise;
						float3 BrmTheta = (BrTheta + BmTheta) / (_Azure_Br + _Azure_Bm);

						inScatter  = BrmTheta * _Azure_SunIntensity * (1.0 - fex);
						inScatter *= sunRise;
						//inScatter *= pow(max(0.5, viewDir.y + 0.5), 2.0);

						//Night Sky.
						//--------------------------------
						//nightSky = (1.0 - fex) * _Azure_RayleighColor.rgb * _Azure_NightIntensity;
						BrmTheta = (BrTheta) / (_Azure_Br + _Azure_Bm);
						nightSky = BrmTheta * _Azure_NightIntensity * (1.0 - fex);
					}
				float horizonExtinction = saturate((viewDir.y) * 1000.0) * fex.b;

				//Moon Bright.
				//--------------------------------
				float  moonRise    = saturate(dot(float3(0.0, 500.0, 0.0), _Azure_MoonDirection) / r);
				float  bright      = 1.0 + dot(viewDir, -_Azure_MoonDirection);
				float3 moonBright  = 1.0 / (1.0  + bright * _Azure_MoonBrightRange) * _Azure_MoonBrightColor.rgb;
					   moonBright += 1.0 / (_Azure_MoonEmission  + bright * 200.0) * _Azure_MoonColor.rgb;
					   moonBright  = moonBright * moonRise;

				float4 OutputColor  = float4(inScatter + nightSky + moonBright, horizonExtinction);

				//Outputs.
				//--------------------------------
				Output.OutColor = OutputColor;
				Output.Fex = float4(fex, sunCosTheta);
				return Output;
			}


			bool iSphere(in float3 origin, in float3 direction, in float3 position, in float radius, out float3 normalDirection)
			{
				float3 rc = origin - position;
				float c = dot(rc, rc) - (radius * radius);
				float b = dot(direction, rc);
				float d = b * b - c;
				float t = -b - sqrt(abs(d));
				float st = step(0.0, min(t, d));
				normalDirection = normalize(-position + (origin + direction * t));

				if (st > 0.0) { return true; }
				return false;
			}


			float4 frag (v2f IN) : SV_Target
			{
				//Sun Disk.
				//--------------------------------
				float3 sunTex = tex2D( _Azure_SunTexture, IN.SunPos + 0.5).rgb * _Azure_SunDiskIntensity;
					   sunTex = pow(sunTex, 2.0);
					   sunTex *= IN.Fex.b * saturate(IN.Fex.a);

				//Moon Sphere.
				//--------------------------------
				float3 rayOrigin    = float3(0.0, 0.0, 0.0);//_WorldSpaceCameraPos;
				float3 rayDirection = normalize(IN.WorldPos);
				float3 moonPosition = _Azure_MoonDirection * 38400.0 * _Azure_MoonSize;
				float3 normalDirection = float3(0.0, 0.0, 0.0);
				float3 moonColor = float3(0.0, 0.0, 0.0);
				float4 moonTex = tex2D( _Azure_MoonTexture, IN.MoonPos.xy + 0.5);
				float moonMask = 1.0 - moonTex.a;
				if(iSphere(rayOrigin, rayDirection, moonPosition, 17370.0, normalDirection))
				{
					float moonSphere = max(dot(normalDirection, _Azure_SunDirection), 0.0) * moonTex.a * 2.0;
					moonColor = moonTex.rgb * moonSphere * _Azure_MoonColor.rgb * IN.OutColor.a;
				}

				//Starfield.
				//--------------------------------
				float scintillation = texCUBE(_Azure_StarNoiseTexture, IN.NoiseRot) * 2.0;
				float4 starTex   = texCUBE(_Azure_StarfieldTexture, IN.StarPos);
				float3 stars     = starTex.rgb * starTex.a * scintillation;
				float3 milkyWay  = pow(starTex.rgb, 1.5) * _Azure_MilkyWayIntensity;
				float3 starfield = (stars + milkyWay) * _Azure_StarfieldColorBalance * moonMask * IN.OutColor.a * _Azure_StarfieldIntensity;

				//Output.
				//--------------------------------
				float3 OutputColor  = IN.OutColor.rgb + sunTex + starfield + moonColor;

				//Tonemapping.
				OutputColor  = saturate(1.0 - exp(-_Azure_Exposure * OutputColor));

				//Color Correction.
				OutputColor = pow(OutputColor, 2.2);
			    #ifdef UNITY_COLORSPACE_GAMMA
			    OutputColor = pow(OutputColor, 0.4545);
				#else
				OutputColor = OutputColor;
    			#endif

    			return float4(OutputColor, 1.0);
			}
			ENDCG
		}
	}
}