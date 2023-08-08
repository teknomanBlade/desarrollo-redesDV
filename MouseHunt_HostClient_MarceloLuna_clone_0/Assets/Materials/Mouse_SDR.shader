// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Mouse_SDR"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_EarsMap("EarsMap", 2D) = "white" {}
		_PawsMap("PawsMap", 2D) = "white" {}
		_FirstPosition("FirstPosition", Float) = 0
		_SecondPosition("SecondPosition", Float) = 0
		_SecondPositionIntensity("SecondPositionIntensity", Float) = 0
		_ShadowIntensity("ShadowIntensity", Float) = 0
		_MinPaws("MinPaws", Float) = 0
		_MinEars("MinEars", Float) = 0
		_MaxPaws("MaxPaws", Float) = 0
		_MaxEars("MaxEars", Float) = 0
		_TimeScalePaws("TimeScalePaws", Float) = 0
		_TimeScaleEars("TimeScaleEars", Float) = 0
		_Period("Period", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _PawsMap;
		uniform float4 _PawsMap_ST;
		uniform float _MinPaws;
		uniform float _MaxPaws;
		uniform float _TimeScalePaws;
		uniform sampler2D _EarsMap;
		uniform float4 _EarsMap_ST;
		uniform float _MinEars;
		uniform float _MaxEars;
		uniform float _TimeScaleEars;
		uniform float _Period;
		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;
		uniform float _FirstPosition;
		uniform float _SecondPosition;
		uniform float _SecondPositionIntensity;
		uniform float _ShadowIntensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_PawsMap = v.texcoord * _PawsMap_ST.xy + _PawsMap_ST.zw;
			float mulTime41 = _Time.y * _TimeScalePaws;
			float smoothstepResult45 = smoothstep( _MinPaws , _MaxPaws , sin( mulTime41 ));
			float3 ase_vertexNormal = v.normal.xyz;
			float4 appendResult49 = (float4(0.0 , ase_vertexNormal.y , 0.0 , 0.0));
			float2 uv_EarsMap = v.texcoord * _EarsMap_ST.xy + _EarsMap_ST.zw;
			float mulTime64 = _Time.y * _TimeScaleEars;
			float smoothstepResult69 = smoothstep( _MinEars , _MaxEars , ( cos( mulTime64 ) * _Period ));
			float4 appendResult71 = (float4(ase_vertexNormal.x , 0.0 , ase_vertexNormal.z , 0.0));
			v.vertex.xyz += ( ( ( tex2Dlod( _PawsMap, float4( uv_PawsMap, 0, 0.0) ) * smoothstepResult45 ) * appendResult49 ) + ( ( tex2Dlod( _EarsMap, float4( uv_EarsMap, 0, 0.0) ) * smoothstepResult69 ) * appendResult71 ) ).rgb;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_worldNormal = i.worldNormal;
			float dotResult3 = dot( ase_worldlightDir , ase_worldNormal );
			float temp_output_5_0 = ( ( dotResult3 + 1.0 ) * 0.5 );
			float temp_output_16_0 = ( saturate( ( 1.0 - step( temp_output_5_0 , _FirstPosition ) ) ) + saturate( ( ( 1.0 - step( temp_output_5_0 , _SecondPosition ) ) - _SecondPositionIntensity ) ) );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			c.rgb = saturate( ( tex2D( _MainTexture, uv_MainTexture ) * ( ( temp_output_16_0 + ( ( 1.0 - temp_output_16_0 ) * _ShadowIntensity ) ) * ase_lightColor * ( 1.0 - step( ase_lightAtten , 0.0 ) ) ) ) ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
312;497;1137;553;1743.803;-1590.579;3.921327;True;False
Node;AmplifyShaderEditor.WorldNormalVector;1;-2644.117,71.8132;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-2644.117,-104.1866;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;3;-2356.117,-24.18661;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;4;-2148.117,7.813385;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1956.117,-40.18661;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1755.424,265.3937;Inherit;False;Property;_SecondPosition;SecondPosition;4;0;Create;True;0;0;False;0;0;0.61;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;7;-1566.779,212.8759;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1620.877,-41.6337;Inherit;False;Property;_FirstPosition;FirstPosition;3;0;Create;True;0;0;False;0;0;0.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;11;-1328.864,-19.2373;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;10;-1392.706,160.9218;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1242.141,377.0654;Inherit;False;Property;_SecondPositionIntensity;SecondPositionIntensity;5;0;Create;True;0;0;False;0;0;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;12;-1083.222,-31.13791;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-932.1171,247.8132;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;14;-724.1171,39.81329;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;155.0184,2515.785;Inherit;False;Property;_TimeScaleEars;TimeScaleEars;12;0;Create;True;0;0;False;0;0;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;15;-724.1171,295.8132;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-322.3689,1590.457;Inherit;False;Property;_TimeScalePaws;TimeScalePaws;11;0;Create;True;0;0;False;0;0;35.295;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-532.1169,119.8132;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;64;324.5018,2294.426;Inherit;False;1;0;FLOAT;8.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;560.5761,2403.171;Inherit;False;Property;_Period;Period;13;0;Create;True;0;0;False;0;0;1.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;41;-149.3929,1369.098;Inherit;False;1;0;FLOAT;8.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;18;-228.1169,295.8132;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;17;-116.2417,788.5941;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-260.117,551.8132;Inherit;False;Property;_ShadowIntensity;ShadowIntensity;6;0;Create;True;0;0;False;0;0;0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;73;560.9215,2109.21;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;42;136.0513,1329.299;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;529.9902,2736.906;Inherit;False;Property;_MaxEars;MaxEars;10;0;Create;True;0;0;False;0;0;11.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;797.374,2112.248;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;21;252.2217,726.7329;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;48.28428,1611.527;Inherit;False;Property;_MinPaws;MinPaws;7;0;Create;True;0;0;False;0;0;0.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;52.60289,1811.578;Inherit;False;Property;_MaxPaws;MaxPaws;9;0;Create;True;0;0;False;0;0;4.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;531.7422,2536.855;Inherit;False;Property;_MinEars;MinEars;8;0;Create;True;0;0;False;0;0;0.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;27.883,391.8133;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;48;746.3342,1433.822;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;45;688.6461,1237.806;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;29;989.7136,1765.604;Inherit;True;Property;_EarsMap;EarsMap;1;0;Create;True;0;0;False;0;-1;e4cda15359d957e469e41c918965345b;1952abee6a1368b468efdad386871cae;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;23;370.1358,548.7529;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;22;76.10901,662.0483;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.NormalVertexDataNode;68;1220.84,2334.515;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;24;219.8831,151.8132;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;30;507.7404,900.5802;Inherit;True;Property;_PawsMap;PawsMap;2;0;Create;True;0;0;False;0;-1;e4cda15359d957e469e41c918965345b;66f9a587fce0caf48bd06dfb72426a71;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;69;1172.84,2126.515;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;372.2613,-551.4083;Inherit;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;False;0;-1;e4cda15359d957e469e41c918965345b;e4cda15359d957e469e41c918965345b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;539.7612,112.5735;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;71;1492.84,2302.515;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;49;1007.348,1408.886;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;902.099,945.6656;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;1380.84,1838.515;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;1238.59,1069.867;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;1275.31,98.34364;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;1716.84,1966.515;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;1630.144,887.6128;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;28;1496.836,144.377;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1708.512,-33.50023;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Mouse_SDR;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;3;1;1;0
WireConnection;4;0;3;0
WireConnection;5;0;4;0
WireConnection;7;0;5;0
WireConnection;7;1;6;0
WireConnection;11;0;5;0
WireConnection;11;1;8;0
WireConnection;10;0;7;0
WireConnection;12;0;11;0
WireConnection;13;0;10;0
WireConnection;13;1;9;0
WireConnection;14;0;12;0
WireConnection;15;0;13;0
WireConnection;16;0;14;0
WireConnection;16;1;15;0
WireConnection;64;0;63;0
WireConnection;41;0;50;0
WireConnection;18;0;16;0
WireConnection;73;0;64;0
WireConnection;42;0;41;0
WireConnection;75;0;73;0
WireConnection;75;1;76;0
WireConnection;21;0;17;0
WireConnection;20;0;18;0
WireConnection;20;1;19;0
WireConnection;45;0;42;0
WireConnection;45;1;53;0
WireConnection;45;2;55;0
WireConnection;23;0;21;0
WireConnection;24;0;16;0
WireConnection;24;1;20;0
WireConnection;69;0;75;0
WireConnection;69;1;67;0
WireConnection;69;2;65;0
WireConnection;26;0;24;0
WireConnection;26;1;22;0
WireConnection;26;2;23;0
WireConnection;71;0;68;1
WireConnection;71;2;68;3
WireConnection;49;1;48;2
WireConnection;46;0;30;0
WireConnection;46;1;45;0
WireConnection;70;0;29;0
WireConnection;70;1;69;0
WireConnection;47;0;46;0
WireConnection;47;1;49;0
WireConnection;27;0;25;0
WireConnection;27;1;26;0
WireConnection;72;0;70;0
WireConnection;72;1;71;0
WireConnection;74;0;47;0
WireConnection;74;1;72;0
WireConnection;28;0;27;0
WireConnection;0;13;28;0
WireConnection;0;11;74;0
ASEEND*/
//CHKSM=00897664883245781345D866BF14024AA0164910