// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Azure[Sky]/BuiltIn/Unlit/Transparent"
{
	Properties
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			// Azure Fog: Including the file that contains the methods and functions needed to apply the fog.
			#include "AzureFogCore.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(1)

				// Azure Fog: Adding a TEXCOORD to store the projection and a TEXCOORD to store the global position.
				float4 projPos  : TEXCOORD2;
				float3 worldPos : TEXCOORD3;

				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				// Azure Fog: Calculating the projection and the global position and storing in the respective TEXCOORD.
				o.projPos = ComputeScreenPos(o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				UNITY_APPLY_FOG(i.fogCoord, col);

				// Azure Fog: Applying the fog in the output color of the fragment shader.
				col = BlendAzureFog(col, i.projPos, i.worldPos);
				/*Note: In this particular case the BlendAzureFog method is being used instead of the ApplyAzureFog, this particular method was used to
				remove some artifacts. In most cases it is recommended to use the standard method as explained in the "Read Me" file.
				*/

				return col;
			}
			ENDCG
		}
	}
}