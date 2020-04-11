// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BOTWToonLit"
{
	Properties
	{
		_AlbedoMap("AlbedoMap", 2D) = "white" {}
		_Tint("Tint", Color) = (1,1,1,1)
		[Toggle]_UseNormalMap("UseNormalMap", Float) = 0
		[Normal]_NormalMap("NormalMap", 2D) = "bump" {}
		_SpecularSize("SpecularSize", Float) = 0.5
		_SpecularStrength("SpecularStrength", Float) = 1
		[Toggle]_UseSecularDabs("UseSecularDabs", Float) = 1
		_DabTexture("DabTexture", 2D) = "white" {}
		_DabsRotation("DabsRotation", Float) = 15
		_DabsSize("DabsSize", Range( 0.01 , 50)) = 5
		_MetalicnessMap("MetalicnessMap", 2D) = "white" {}
		[Toggle]_UseEmissionMap("UseEmissionMap", Float) = 0
		_EmissionMap("EmissionMap", 2D) = "black" {}
		_EmissionMultiplier("EmissionMultiplier", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }
		
		Cull Back
		HLSLINCLUDE
		#pragma target 3.0
		ENDHLSL

		
		Pass
		{
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend One Zero , One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 70108

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag


			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _NormalMap;
			sampler2D _AlbedoMap;
			sampler2D _DabTexture;
			sampler2D _MetalicnessMap;
			sampler2D _EmissionMap;
			CBUFFER_START( UnityPerMaterial )
			float _UseNormalMap;
			float4 _NormalMap_ST;
			float4 _Tint;
			float4 _AlbedoMap_ST;
			float _UseSecularDabs;
			float _SpecularStrength;
			float _SpecularSize;
			float _DabsSize;
			float _DabsRotation;
			float4 _MetalicnessMap_ST;
			float _UseEmissionMap;
			float _EmissionMultiplier;
			float4 _EmissionMap_ST;
			CBUFFER_END


			
			VertexOutput vert ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				o.ase_texcoord4.xyz = ase_worldTangent;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord5.xyz = ase_worldBitangent;
				VertexPositionInputs ase_vertexInput = GetVertexPositionInputs (v.vertex.xyz);
				#ifdef _MAIN_LIGHT_SHADOWS//ase_lightAtten_vert
				o.ase_texcoord6 = GetShadowCoord( ase_vertexInput );
				#endif//ase_lightAtten_vert
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				o.ase_normal = v.ase_normal;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				o.ase_texcoord2.w = 0;
				o.ase_texcoord3.zw = 0;
				o.ase_texcoord4.w = 0;
				o.ase_texcoord5.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				o.clipPos = TransformObjectToHClip( v.vertex.xyz );
				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( o.clipPos.z );
				#endif
				return o;
			}

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float3 normalizeResult118 = normalize( SafeNormalize(_MainLightPosition.xyz) );
				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float dotResult35 = dot( ( -1.0 * normalizeResult118 ) , ase_worldViewDir );
				float temp_output_40_0 = ( ( 1.0 - saturate( ( dotResult35 + 0.2 ) ) ) + 0.2 );
				float3 ase_worldNormal = IN.ase_texcoord2.xyz;
				float fresnelNdotV41 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode41 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV41, 5.0 ) );
				float2 uv_NormalMap = IN.ase_texcoord3.xy * _NormalMap_ST.xy + _NormalMap_ST.zw;
				float3 ase_worldTangent = IN.ase_texcoord4.xyz;
				float3 ase_worldBitangent = IN.ase_texcoord5.xyz;
				float3x3 ase_worldToTangent = float3x3(ase_worldTangent,ase_worldBitangent,ase_worldNormal);
				float3 objectToTangentDir20 = mul( ase_worldToTangent, mul( GetObjectToWorldMatrix(), float4( IN.ase_normal, 0 ) ).xyz);
				float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
				float3 tangentToWorldDir21 = mul( ase_tangentToWorldFast, BlendNormal( (( _UseNormalMap )?( UnpackNormalScale( tex2D( _NormalMap, uv_NormalMap ), 1.0f ) ):( float3(0,0,1) )) , objectToTangentDir20 ) );
				float dotResult48 = dot( normalizeResult118 , tangentToWorldDir21 );
				float ase_lightAtten = 0;
				Light ase_lightAtten_mainLight = GetMainLight( IN.ase_texcoord6 );
				ase_lightAtten = ase_lightAtten_mainLight.distanceAttenuation * ase_lightAtten_mainLight.shadowAttenuation;
				float temp_output_46_0 = step( 0.7 , ase_lightAtten );
				float smoothstepResult50 = smoothstep( 0.29 , 0.3 , ( dotResult48 * temp_output_46_0 ));
				float2 uv_AlbedoMap = IN.ase_texcoord3.xy * _AlbedoMap_ST.xy + _AlbedoMap_ST.zw;
				float3 normalizeResult51 = normalize( ( ase_worldViewDir + normalizeResult118 ) );
				float dotResult52 = dot( normalizeResult51 , tangentToWorldDir21 );
				float temp_output_53_0 = ( temp_output_46_0 * dotResult52 );
				float4 temp_cast_0 = (( _SpecularStrength * step( ( 1.0 - _SpecularSize ) , (0.0 + (temp_output_53_0 - 0.0) * (1.0 - 0.0) / (1.0 - 0.0)) ) )).xxxx;
				float4 temp_cast_1 = (0.01).xxxx;
				float4 temp_cast_2 = (0.02).xxxx;
				float2 temp_cast_3 = (( 50.0 / _DabsSize )).xx;
				float2 uv062 = IN.ase_texcoord3.xy * temp_cast_3 + float2( 0,0 );
				float cos59 = cos( radians( _DabsRotation ) );
				float sin59 = sin( radians( _DabsRotation ) );
				float2 rotator59 = mul( uv062 - float2( 0.5,0.5 ) , float2x2( cos59 , -sin59 , sin59 , cos59 )) + float2( 0.5,0.5 );
				float4 smoothstepResult69 = smoothstep( temp_cast_1 , temp_cast_2 , ( ( temp_output_53_0 + (-1.0 + (_SpecularSize - 0.0) * (-0.9 - -1.0) / (1.0 - 0.0)) ) * tex2D( _DabTexture, rotator59 ) ));
				float4 temp_cast_4 = (0.4).xxxx;
				float2 uv_MetalicnessMap = IN.ase_texcoord3.xy * _MetalicnessMap_ST.xy + _MetalicnessMap_ST.zw;
				float4 temp_cast_5 = (_EmissionMultiplier).xxxx;
				float2 uv_EmissionMap = IN.ase_texcoord3.xy * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
				float4 temp_output_97_0 = ( ( saturate( ( step( ( temp_output_40_0 + 0.2 ) , fresnelNode41 ) * smoothstepResult50 ) ) + ( 1.5 * ( ( ( _Tint * tex2D( _AlbedoMap, uv_AlbedoMap ) ) * _MainLightColor ) * ( ( step( temp_output_40_0 , fresnelNode41 ) + (0.2 + (smoothstepResult50 - 0.0) * (0.9 - 0.2) / (1.0 - 0.0)) ) + ( smoothstepResult50 * ( (( _UseSecularDabs )?( ( smoothstepResult69 * _SpecularStrength ) ):( temp_cast_0 )) * step( temp_cast_4 , tex2D( _MetalicnessMap, uv_MetalicnessMap ) ) ) ) ) ) ) ) * (( _UseEmissionMap )?( ( ( tex2D( _EmissionMap, uv_EmissionMap ) * _EmissionMultiplier ) + 1.0 ) ):( temp_cast_5 )) );
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = temp_output_97_0.rgb;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				return half4( Color, Alpha );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 70108

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment


			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START( UnityPerMaterial )
			float _UseNormalMap;
			float4 _NormalMap_ST;
			float4 _Tint;
			float4 _AlbedoMap_ST;
			float _UseSecularDabs;
			float _SpecularStrength;
			float _SpecularSize;
			float _DabsSize;
			float _DabsRotation;
			float4 _MetalicnessMap_ST;
			float _UseEmissionMap;
			float _EmissionMultiplier;
			float4 _EmissionMap_ST;
			CBUFFER_END


			
			float3 _LightDirection;

			VertexOutput ShadowPassVertex( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 normalWS = TransformObjectToWorldDir( v.ase_normal );

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				o.clipPos = clipPos;

				return o;
			}

			half4 ShadowPassFragment(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 70108

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag


			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START( UnityPerMaterial )
			float _UseNormalMap;
			float4 _NormalMap_ST;
			float4 _Tint;
			float4 _AlbedoMap_ST;
			float _UseSecularDabs;
			float _SpecularStrength;
			float _SpecularSize;
			float _DabsSize;
			float _DabsRotation;
			float4 _MetalicnessMap_ST;
			float _UseEmissionMap;
			float _EmissionMultiplier;
			float4 _EmissionMap_ST;
			CBUFFER_END


			
			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				o.clipPos = TransformObjectToHClip(v.vertex.xyz);
				return o;
			}

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

	
	}
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=17700
282;73;1079;655;-2605.66;754.7463;1.539801;True;False
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;30;-2713.754,-497.1744;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SmoothstepOpNode;69;496.7774,936.3013;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;88;727.2493,1789.791;Inherit;False;Constant;_Float4;Float 4;8;0;Create;True;0;0;False;0;0.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;52;-1276.822,701.9952;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;11;-2747.811,27.18548;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;15;-3335.878,211.593;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;89;-3291.528,-181.2589;Inherit;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;7;-3409.233,-21.79483;Inherit;True;Property;_NormalMap;NormalMap;3;1;[Normal];Create;True;0;0;False;0;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;55;-552.9552,790.6502;Inherit;False;Property;_SpecularSize;SpecularSize;4;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;26;-2340.206,-244.1248;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;118;-2461.332,-500.3812;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-842.0381,1050.514;Inherit;False;Property;_DabsSize;DabsSize;9;0;Create;True;0;0;False;0;5;0;0.01;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;474.7775,1095.301;Inherit;False;Property;_SpecularStrength;SpecularStrength;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-1782.611,700.3435;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;51;-1563.438,706.5964;Inherit;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightAttenuation;28;-1534.793,288.4825;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-526.2376,1121.014;Inherit;False;Property;_DabsRotation;DabsRotation;8;0;Create;True;0;0;False;0;15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;35;-1037.882,-1182.766;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;63;-733.7378,943.2142;Inherit;False;2;0;FLOAT;50;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;67;-513.6382,1199.013;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;75;-796.9377,1460.217;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;76;-98.36903,1436.848;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;56;-281.2225,776.3013;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;-0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-1024.716,688.4965;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;59;-293.0383,1059.614;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1258.222,-1224.512;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;58;-67.48976,972.5687;Inherit;True;Property;_DabTexture;DabTexture;7;0;Create;True;0;0;False;0;-1;c7e5d971600b65044ab050fd08857719;c7e5d971600b65044ab050fd08857719;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;38;-692.9747,-1175.416;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;272.7775,1000.301;Inherit;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1422.702,-1247.255;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-854.6487,-1175.028;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformDirectionNode;20;-3069.884,205.7269;Inherit;False;Object;Tangent;False;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;81;-275.0501,1373.291;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;272.7775,840.3013;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;71;272.7775,1080.301;Inherit;False;Constant;_Float3;Float 3;6;0;Create;True;0;0;False;0;0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;48;-1429.818,-177.2531;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-35.881,685.4335;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;62;-547.7382,966.2141;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;44;63.87054,-1351.819;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;46;-1292.847,264.269;Inherit;False;2;0;FLOAT;0.7;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;3512.405,-312.8746;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TransformDirectionNode;21;-2411.657,126.4689;Inherit;True;Tangent;World;False;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;733.1621,1411.784;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;74;968.9493,1206.979;Inherit;False;Property;_UseSecularDabs;UseSecularDabs;6;0;Create;True;0;0;False;0;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;50;-431.9084,-282.294;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.29;False;2;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-326.1775,-1167.603;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;9;-3059.227,-102.7009;Inherit;False;Property;_UseNormalMap;UseNormalMap;2;0;Create;True;0;0;False;0;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FresnelNode;41;-460.0986,-1485.944;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;109;1311.684,-906.4667;Inherit;False;Property;_Tint;Tint;1;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;91;674.5083,-289.1263;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.2;False;4;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;2359.803,-303.3625;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;110;1223.684,-716.4667;Inherit;True;Property;_AlbedoMap;AlbedoMap;0;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;1220.249,1553.662;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;92;1128.004,-368.4591;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;1610.684,-804.4667;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;1468.484,326.1116;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;85;962.2582,1797.189;Inherit;False;2;0;FLOAT;0.4;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;45;48.38541,-1005.732;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-945.1323,-222.1936;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;720.7777,1000.301;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;39;-525.1776,-1176.603;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;104;1665.029,-1317.654;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;2013.793,-313.9294;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;93;1570.738,-368.445;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;113;2268.089,304.7454;Inherit;False;Property;_EmissionMultiplier;EmissionMultiplier;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;1300.4,-1318.73;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;117;2179.089,58.74535;Inherit;True;Property;_EmissionMap;EmissionMap;12;0;Create;True;0;0;False;0;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;1805.119,-730.7018;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;116;2525.089,227.7454;Inherit;False;Constant;_Float6;Float 6;10;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;111;2382.997,-424.7117;Inherit;False;Constant;_Float5;Float 5;9;0;Create;True;0;0;False;0;1.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;43;258.6762,-1311.552;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;29;1602.351,-626.2597;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;114;2691.894,206.2357;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;96;2758.702,-342.9053;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;112;2866.612,283.6673;Inherit;False;Property;_UseEmissionMap;UseEmissionMap;11;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;103;3494.671,-399.1335;Inherit;False;unity_AmbientSky;0;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;3137.828,-331.3901;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;2525.089,109.9606;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;87;632.5352,1877.781;Inherit;True;Property;_MetalicnessMap;MetalicnessMap;10;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;131;4072.031,-568.897;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;1;ShadowCaster;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;132;4072.031,-568.897;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;2;DepthOnly;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;True;False;False;False;False;0;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;133;4072.031,-568.897;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;3;Meta;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;True;2;False;-1;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;130;3841.713,-337.8174;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;BOTWToonLit;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;0;Forward;7;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;10;Surface;0;  Blend;0;Two Sided;1;Cast Shadows;1;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;0;Built-in Fog;0;Meta Pass;0;Vertex Position,InvertActionOnDeselection;1;0;4;True;True;True;False;False;;0
WireConnection;69;0;57;0
WireConnection;69;1;70;0
WireConnection;69;2;71;0
WireConnection;52;0;51;0
WireConnection;52;1;21;0
WireConnection;11;0;9;0
WireConnection;11;1;20;0
WireConnection;118;0;30;0
WireConnection;47;0;26;0
WireConnection;47;1;118;0
WireConnection;51;0;47;0
WireConnection;35;0;32;0
WireConnection;35;1;26;0
WireConnection;63;1;65;0
WireConnection;67;0;60;0
WireConnection;75;0;53;0
WireConnection;76;0;81;0
WireConnection;76;1;75;0
WireConnection;56;0;55;0
WireConnection;53;0;46;0
WireConnection;53;1;52;0
WireConnection;59;0;62;0
WireConnection;59;2;67;0
WireConnection;32;0;33;0
WireConnection;32;1;118;0
WireConnection;58;1;59;0
WireConnection;38;0;36;0
WireConnection;36;0;35;0
WireConnection;20;0;15;0
WireConnection;81;0;55;0
WireConnection;57;0;54;0
WireConnection;57;1;58;0
WireConnection;48;0;118;0
WireConnection;48;1;21;0
WireConnection;54;0;53;0
WireConnection;54;1;56;0
WireConnection;62;0;63;0
WireConnection;44;0;40;0
WireConnection;46;1;28;0
WireConnection;98;0;103;0
WireConnection;98;1;97;0
WireConnection;21;0;11;0
WireConnection;82;0;73;0
WireConnection;82;1;76;0
WireConnection;74;0;82;0
WireConnection;74;1;72;0
WireConnection;50;0;49;0
WireConnection;40;0;39;0
WireConnection;9;0;89;0
WireConnection;9;1;7;0
WireConnection;91;0;50;0
WireConnection;95;0;111;0
WireConnection;95;1;94;0
WireConnection;84;0;74;0
WireConnection;84;1;85;0
WireConnection;92;0;45;0
WireConnection;92;1;91;0
WireConnection;107;0;109;0
WireConnection;107;1;110;0
WireConnection;99;0;50;0
WireConnection;99;1;84;0
WireConnection;85;0;88;0
WireConnection;85;1;87;0
WireConnection;45;0;40;0
WireConnection;45;1;41;0
WireConnection;49;0;48;0
WireConnection;49;1;46;0
WireConnection;72;0;69;0
WireConnection;72;1;73;0
WireConnection;39;0;38;0
WireConnection;104;0;105;0
WireConnection;94;0;106;0
WireConnection;94;1;93;0
WireConnection;93;0;92;0
WireConnection;93;1;99;0
WireConnection;105;0;43;0
WireConnection;105;1;50;0
WireConnection;106;0;107;0
WireConnection;106;1;29;0
WireConnection;43;0;44;0
WireConnection;43;1;41;0
WireConnection;114;0;115;0
WireConnection;114;1;116;0
WireConnection;96;0;104;0
WireConnection;96;1;95;0
WireConnection;112;0;113;0
WireConnection;112;1;114;0
WireConnection;97;0;96;0
WireConnection;97;1;112;0
WireConnection;115;0;117;0
WireConnection;115;1;113;0
WireConnection;130;2;97;0
ASEEND*/
//CHKSM=2D4B8E1E019B84BAF4621B8D6F15F81E29E203AB