// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Runes"
{
	Properties
	{
		_RuneTex("Rune Tex", 2D) = "black" {}
		_TexIndex("Tex Index", Int) = 0
		_MaxPower("Max Power", Float) = 1
		_Power("Power", Range( 0 , 1)) = 1

	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
		
		Cull Back
		HLSLINCLUDE
		#pragma target 3.0
		ENDHLSL

		
		Pass
		{
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend One OneMinusSrcAlpha , One OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define _ALPHAPREMULTIPLY_ON 1
			#pragma multi_compile_instancing
			#define _ALPHATEST_ON 1
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

			#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
			#pragma multi_compile_instancing


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD0;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _RuneTex;
			UNITY_INSTANCING_BUFFER_START(Runes)
				UNITY_DEFINE_INSTANCED_PROP(int, _TexIndex)
			UNITY_INSTANCING_BUFFER_END(Runes)
			CBUFFER_START( UnityPerMaterial )
			float _Power;
			float _MaxPower;
			CBUFFER_END


			
			float4 SampleGradient( Gradient gradient, float time )
			{
				float3 color = gradient.colors[0].rgb;
				UNITY_UNROLL
				for (int c = 1; c < 8; c++)
				{
				float colorPos = saturate((time - gradient.colors[c-1].w) / (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, gradient.colorsLength-1);
				color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
				}
				#ifndef UNITY_COLORSPACE_GAMMA
				color = SRGBToLinear(color);
				#endif
				float alpha = gradient.alphas[0].x;
				UNITY_UNROLL
				for (int a = 1; a < 8; a++)
				{
				float alphaPos = saturate((time - gradient.alphas[a-1].y) / (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, gradient.alphasLength-1);
				alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
				}
				return float4(color, alpha);
			}
			

			VertexOutput vert ( VertexInput v  )
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

				Gradient gradient73 = NewGradient( 0, 4, 3, float4( 0.2235033, 0.2589117, 0.4786131, 0 ), float4( 0.3137255, 0.3529412, 1.058824, 0.237995 ), float4( 0.2352941, 0.9098039, 1.733333, 0.6469978 ), float4( 1.443601, 5.824182, 9.507854, 1 ), 0, 0, 0, 0, float2( 0, 0 ), float2( 1, 0.2500038 ), float2( 1, 1 ), 0, 0, 0, 0, 0 );
				int _TexIndex_Instance = UNITY_ACCESS_INSTANCED_PROP(Runes,_TexIndex);
				float2 appendResult55 = (float2((float)( _TexIndex_Instance % 8 ) , (float)floor( ( _TexIndex_Instance / 8 ) )));
				float2 temp_output_74_0 = ( ( float2( 1,1 ) / appendResult55 ) + float2( 0,0 ) );
				float4 tex2DNode6 = tex2D( _RuneTex, temp_output_74_0 );
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = ( SampleGradient( gradient73, ( _Power * _MaxPower ) ) * tex2DNode6 ).rgb;
				float Alpha = tex2DNode6.a;
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
			#define _ALPHAPREMULTIPLY_ON 1
			#pragma multi_compile_instancing
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70108

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment


			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#pragma multi_compile_instancing


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

			sampler2D _RuneTex;
			UNITY_INSTANCING_BUFFER_START(Runes)
				UNITY_DEFINE_INSTANCED_PROP(int, _TexIndex)
			UNITY_INSTANCING_BUFFER_END(Runes)
			CBUFFER_START( UnityPerMaterial )
			float _Power;
			float _MaxPower;
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

				int _TexIndex_Instance = UNITY_ACCESS_INSTANCED_PROP(Runes,_TexIndex);
				float2 appendResult55 = (float2((float)( _TexIndex_Instance % 8 ) , (float)floor( ( _TexIndex_Instance / 8 ) )));
				float2 temp_output_74_0 = ( ( float2( 1,1 ) / appendResult55 ) + float2( 0,0 ) );
				float4 tex2DNode6 = tex2D( _RuneTex, temp_output_74_0 );
				
				float Alpha = tex2DNode6.a;
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
			#define _ALPHAPREMULTIPLY_ON 1
			#pragma multi_compile_instancing
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70108

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag


			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#pragma multi_compile_instancing


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

			sampler2D _RuneTex;
			UNITY_INSTANCING_BUFFER_START(Runes)
				UNITY_DEFINE_INSTANCED_PROP(int, _TexIndex)
			UNITY_INSTANCING_BUFFER_END(Runes)
			CBUFFER_START( UnityPerMaterial )
			float _Power;
			float _MaxPower;
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

				int _TexIndex_Instance = UNITY_ACCESS_INSTANCED_PROP(Runes,_TexIndex);
				float2 appendResult55 = (float2((float)( _TexIndex_Instance % 8 ) , (float)floor( ( _TexIndex_Instance / 8 ) )));
				float2 temp_output_74_0 = ( ( float2( 1,1 ) / appendResult55 ) + float2( 0,0 ) );
				float4 tex2DNode6 = tex2D( _RuneTex, temp_output_74_0 );
				
				float Alpha = tex2DNode6.a;
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
355;73;1168;607;2661.189;684.6511;1.982846;True;False
Node;AmplifyShaderEditor.IntNode;42;-2048.26,-27.44369;Inherit;False;InstancedProperty;_TexIndex;Tex Index;1;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;54;-1823.356,38.32832;Inherit;False;2;0;INT;0;False;1;INT;8;False;1;INT;0
Node;AmplifyShaderEditor.FloorOpNode;52;-1666.018,37.86781;Inherit;False;1;0;INT;0;False;1;INT;0
Node;AmplifyShaderEditor.SimpleRemainderNode;53;-1770.356,-77.67166;Inherit;False;2;0;INT;0;False;1;INT;8;False;1;INT;0
Node;AmplifyShaderEditor.Vector2Node;18;-1507.729,-200.9816;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;55;-1498.399,-35.69535;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;76;-1239.734,-131.065;Inherit;False;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;-1027.199,58.82841;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;33;416.5609,-171.0002;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;66;-2198.474,257.136;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;312.637,-351.7424;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-760.6542,-94.83826;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-647.8867,-344.858;Inherit;False;Property;_MaxPower;Max Power;2;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-391.292,-428.4042;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;67;-1810.226,253.5439;Inherit;True;Property;_RuneFlow;RuneFlow;4;0;Create;True;0;0;False;0;-1;254544d71e7353e4e88352f42f9226eb;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientSampleNode;69;-192.4791,-487.5529;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;57;-765.8685,-434.1964;Inherit;False;Property;_Power;Power;3;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;73;-440.2455,-532.0097;Inherit;False;0;4;3;0.2235033,0.2589117,0.4786131,0;0.3137255,0.3529412,1.058824,0.237995;0.2352941,0.9098039,1.733333,0.6469978;1.443601,5.824182,9.507854,1;0,0;1,0.2500038;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.SamplerNode;6;-145.8136,-172.743;Inherit;True;Property;_RuneTex;Rune Tex;0;0;Create;True;0;0;False;0;6;890a2ea8c502bf245b88dab58bb37101;890a2ea8c502bf245b88dab58bb37101;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;68;-1985.145,281.6024;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;75;-1473.68,282.8872;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;23;239.6436,-96.89384;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;1;ShadowCaster;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;25;239.6436,-96.89384;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;3;Meta;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;True;2;False;-1;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;24;239.6436,-96.89384;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;2;DepthOnly;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;True;False;False;False;False;0;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;22;627.7771,-284.1234;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;13;Runes;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;0;Forward;7;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;0;True;3;1;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;10;Surface;1;  Blend;1;Two Sided;1;Cast Shadows;1;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;0;Built-in Fog;0;Meta Pass;0;Vertex Position,InvertActionOnDeselection;1;0;4;True;True;True;False;False;;0
WireConnection;54;0;42;0
WireConnection;52;0;54;0
WireConnection;53;0;42;0
WireConnection;55;0;53;0
WireConnection;55;1;52;0
WireConnection;76;0;18;0
WireConnection;76;1;55;0
WireConnection;74;0;76;0
WireConnection;59;0;69;0
WireConnection;59;1;6;0
WireConnection;38;1;74;0
WireConnection;60;0;57;0
WireConnection;60;1;58;0
WireConnection;67;1;68;0
WireConnection;69;0;73;0
WireConnection;69;1;60;0
WireConnection;6;1;74;0
WireConnection;68;0;66;1
WireConnection;68;1;66;2
WireConnection;75;0;67;1
WireConnection;75;1;67;2
WireConnection;22;2;59;0
WireConnection;22;3;6;4
WireConnection;22;4;33;0
ASEEND*/
//CHKSM=A35B444D2EF53D3E9F7546ADC8E254FFEEFEA449