// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CircleShader"
{
	Properties
	{
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HDR]_Color1("Color 1", Color) = (0,0,0,0)
		_OutlineThckness("OutlineThckness", Range( 0.58 , 2)) = 0.51
		_LinePulseSpeed("LinePulseSpeed", Float) = 0.15
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform float _OutlineThckness;
		uniform sampler2D _TextureSample0;
		uniform float _LinePulseSpeed;
		uniform float4 _Color1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Emission = _Color0.rgb;
			float2 uv_TexCoord1 = i.uv_texcoord + float2( -0.5,-0.5 );
			float temp_output_2_0 = length( uv_TexCoord1 );
			float temp_output_3_0 = ( 1.0 - temp_output_2_0 );
			float smoothstepResult5 = smoothstep( 0.5 , _OutlineThckness , temp_output_3_0);
			float smoothstepResult15 = smoothstep( 0.01 , 0.47 , smoothstepResult5);
			float mulTime20 = _Time.y * _LinePulseSpeed;
			float2 temp_cast_1 = (( temp_output_2_0 + mulTime20 )).xx;
			float smoothstepResult14 = smoothstep( 0.0 , 1.19 , ( 1.0 - smoothstepResult5 ));
			float smoothstepResult7 = smoothstep( 0.07 , 0.46 , ( smoothstepResult14 * temp_output_3_0 ));
			o.Alpha = ( ( temp_output_2_0 * smoothstepResult15 * tex2D( _TextureSample0, temp_cast_1 ) ) + ( smoothstepResult5 * smoothstepResult7 * _Color1 ) ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
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
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17000
269;371;1429;574;2235.61;256.4587;1.811412;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-2437.995,118.2426;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LengthOpNode;2;-2057.672,195.4471;Float;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1790.644,276.1826;Float;False;Property;_OutlineThckness;OutlineThckness;3;0;Create;True;0;0;False;0;0.51;0.58;0.58;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;3;-1637.614,493.2417;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;5;-1429.64,243.454;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;0.63;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;12;-1145.409,328.2826;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1252.077,631.9019;Float;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;0;0.2349052;0;1.176766;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1602.802,-66.01933;Float;False;Property;_LinePulseSpeed;LinePulseSpeed;4;0;Create;True;0;0;False;0;0.15;-0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;14;-944.2501,413.828;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.86;False;2;FLOAT;1.19;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;20;-1307.044,-53.13842;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-1041.295,-57.61172;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-707.535,584.2072;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;15;-903.6098,153.2628;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.01;False;2;FLOAT;0.47;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-823.7665,-288.7027;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;e28dc97a9541e3642a48c0e3886688c5;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;7;-448.1734,567.2688;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.07;False;2;FLOAT;0.46;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-388.9352,915.9156;Float;False;Property;_Color1;Color 1;2;1;[HDR];Create;True;0;0;False;0;0,0,0,0;2.996078,1.678431,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-174.8511,348.7011;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-596.4188,18.95399;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;73.03444,128.9031;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;6;-244.628,-285.3256;Float;False;Property;_Color0;Color 0;0;1;[HDR];Create;True;0;0;False;0;0,0,0,0;2,0.5123312,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;317.0555,-62.22584;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;CircleShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;3;0;2;0
WireConnection;5;0;3;0
WireConnection;5;2;18;0
WireConnection;12;0;5;0
WireConnection;14;0;12;0
WireConnection;14;1;17;0
WireConnection;20;0;19;0
WireConnection;21;0;2;0
WireConnection;21;1;20;0
WireConnection;11;0;14;0
WireConnection;11;1;3;0
WireConnection;15;0;5;0
WireConnection;22;1;21;0
WireConnection;7;0;11;0
WireConnection;8;0;5;0
WireConnection;8;1;7;0
WireConnection;8;2;16;0
WireConnection;4;0;2;0
WireConnection;4;1;15;0
WireConnection;4;2;22;0
WireConnection;9;0;4;0
WireConnection;9;1;8;0
WireConnection;0;2;6;0
WireConnection;0;9;9;0
ASEEND*/
//CHKSM=794AC9A93BC1832869CAE389FC2A8A3E0217F05C