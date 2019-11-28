// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Life"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Charge("Charge", Range( -0.2 , 2)) = 0.35
		[HDR]_Color1("Color 1", Color) = (0.9150943,0,0,0)
		_Normalcolor("Normal color", Color) = (1,0.6298988,0,0)
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		[HDR]_Charged2("Charged 2", Color) = (1,0.02020948,0,0)
		[HDR]_Color0("Color 0", Color) = (1,0.8792536,0,0)
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_TextureSample4("Texture Sample 4", 2D) = "white" {}
		[Toggle(_FULLHP_ON)] _FullHP("Full HP", Float) = 0
		[HDR]_Fulllifecolor("Full life color", Color) = (1,0.6746032,0,0)
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
		#pragma shader_feature _FULLHP_ON
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform float _Charge;
		uniform float4 _Color0;
		uniform sampler2D _TextureSample4;
		uniform float4 _TextureSample4_ST;
		uniform float4 _Normalcolor;
		uniform float4 _Charged2;
		uniform float4 _Color1;
		uniform float4 _Fulllifecolor;
		uniform sampler2D _TextureSample1;
		uniform sampler2D _TextureSample2;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color62 = IsGammaSpace() ? float4(1,0.9189128,0.1273585,0) : float4(1,0.825358,0.01480528,0);
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float4 tex2DNode53 = tex2D( _TextureSample3, uv_TextureSample3 );
			float2 temp_cast_0 = (_Charge).xx;
			float2 uv_TexCoord4 = i.uv_texcoord + temp_cast_0;
			float temp_output_5_0 = ( 1.0 - uv_TexCoord4.y );
			float smoothstepResult6 = smoothstep( -0.13 , 0.1 , temp_output_5_0);
			float smoothstepResult33 = smoothstep( 0.18 , 0.31 , ( ( 1.0 - smoothstepResult6 ) * smoothstepResult6 ));
			float2 uv_TextureSample4 = i.uv_texcoord * _TextureSample4_ST.xy + _TextureSample4_ST.zw;
			#ifdef _FULLHP_ON
				float4 staticSwitch66 = _Fulllifecolor;
			#else
				float4 staticSwitch66 = _Color1;
			#endif
			float mulTime2 = _Time.y * 0.2;
			float2 appendResult25 = (float2(0.0 , -1.0));
			float2 uv_TexCoord22 = i.uv_texcoord * float2( 1.5,1 );
			float2 panner27 = ( mulTime2 * appendResult25 + ( uv_TexCoord22 * 0.95 ));
			float4 lerpResult10 = lerp( _Charged2 , staticSwitch66 , tex2D( _TextureSample1, tex2D( _TextureSample2, panner27 ).rg ).g);
			float smoothstepResult35 = smoothstep( -0.09 , 0.01 , temp_output_5_0);
			float4 lerpResult16 = lerp( ( tex2D( _TextureSample4, uv_TextureSample4 ) * _Normalcolor ) , lerpResult10 , smoothstepResult35);
			o.Emission = ( ( color62 * tex2DNode53 ) + ( ( smoothstepResult33 * _Color0 ) + lerpResult16 ) ).rgb;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Alpha = ( tex2DNode53 + tex2D( _TextureSample0, uv_TextureSample0 ) ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

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
100;350;1195;497;611.127;-154.5253;1.427638;True;True
Node;AmplifyShaderEditor.RangedFloatNode;3;-1186.736,-179.8574;Float;False;Property;_Charge;Charge;2;0;Create;True;0;0;False;0;0.35;-0.2;-0.2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1232.652,792.5986;Float;False;Constant;_Float8;Float 8;1;0;Create;True;0;0;False;0;0.95;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1248.652,920.5986;Float;False;Constant;_Float10;Float 10;1;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-1258.927,613.9879;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.5,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;1;-1088.652,1048.599;Float;False;Constant;_Float9;Float 9;1;0;Create;True;0;0;False;0;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-775.0699,-200.873;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-960.6525,776.5987;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;-960.6525,952.5986;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;5;-276.1429,-215.0427;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;2;-858.9266,1061.988;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;6;2.874983,-149.4191;Float;True;3;0;FLOAT;0;False;1;FLOAT;-0.13;False;2;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;27;-698.9266,885.9876;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;29;211.6466,-277.9847;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;69;14.42055,387.047;Float;False;Property;_Fulllifecolor;Full life color;11;1;[HDR];Create;True;0;0;False;0;1,0.6746032,0,0;2,0.7127352,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;21;-251.6013,595.8879;Float;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;False;0;ec2f296ae6cdc774cbfee494d76ab906;ec2f296ae6cdc774cbfee494d76ab906;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;8;-35.85398,197.9354;Float;False;Property;_Color1;Color 1;3;1;[HDR];Create;True;0;0;False;0;0.9150943,0,0,0;0.9150943,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;64;673.5742,-109.2305;Float;True;Property;_TextureSample4;Texture Sample 4;9;0;Create;True;0;0;False;0;None;7130c16fd8005b546b111d341310a9a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;66;439.5847,312.1327;Float;True;Property;_FullHP;Full HP;10;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;453.3902,-263.5667;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;736.449,-274.4492;Float;False;Property;_Normalcolor;Normal color;4;0;Create;True;0;0;False;0;1,0.6298988,0,0;0.4433962,0.3409131,0.3409131,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;20;332.3112,605.6082;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;e28dc97a9541e3642a48c0e3886688c5;e28dc97a9541e3642a48c0e3886688c5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;367.0938,119.2599;Float;False;Property;_Charged2;Charged 2;6;1;[HDR];Create;True;0;0;False;0;1,0.02020948,0,0;1,0.4895674,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;35;713.0378,25.81984;Float;True;3;0;FLOAT;0;False;1;FLOAT;-0.09;False;2;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;1079.831,-311.8472;Float;False;Property;_Color0;Color 0;7;1;[HDR];Create;True;0;0;False;0;1,0.8792536,0,0;1,0.8792536,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;1072.635,-92.75544;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;33;861.5742,-473.7199;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.18;False;2;FLOAT;0.31;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;10;771.3206,246.8665;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;53;1523.847,-562.1301;Float;True;Property;_TextureSample3;Texture Sample 3;8;0;Create;True;0;0;False;0;None;f8dadbd3cc214d446a24ca8241dfcb92;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;62;1579.208,-746.5314;Float;False;Constant;_Color2;Color 2;10;0;Create;True;0;0;False;0;1,0.9189128,0.1273585,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;1290.558,-392.8023;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;16;1356.461,-34.57573;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;1900.506,-733.8334;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;1599.658,-185.8191;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;11;1739.711,310.1488;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;175c4a42dbb81f449ab90aa86e6d6154;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;61;2262.193,-420.5542;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;2334.557,-23.26275;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2660.229,-289.3517;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Life;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;9.18;0,0,0,0;VertexScale;False;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;1;3;0
WireConnection;26;0;22;0
WireConnection;26;1;24;0
WireConnection;25;1;23;0
WireConnection;5;0;4;2
WireConnection;2;0;1;0
WireConnection;6;0;5;0
WireConnection;27;0;26;0
WireConnection;27;2;25;0
WireConnection;27;1;2;0
WireConnection;29;0;6;0
WireConnection;21;1;27;0
WireConnection;66;1;8;0
WireConnection;66;0;69;0
WireConnection;28;0;29;0
WireConnection;28;1;6;0
WireConnection;20;1;21;0
WireConnection;35;0;5;0
WireConnection;65;0;64;0
WireConnection;65;1;9;0
WireConnection;33;0;28;0
WireConnection;10;0;7;0
WireConnection;10;1;66;0
WireConnection;10;2;20;2
WireConnection;31;0;33;0
WireConnection;31;1;32;0
WireConnection;16;0;65;0
WireConnection;16;1;10;0
WireConnection;16;2;35;0
WireConnection;63;0;62;0
WireConnection;63;1;53;0
WireConnection;30;0;31;0
WireConnection;30;1;16;0
WireConnection;61;0;63;0
WireConnection;61;1;30;0
WireConnection;58;0;53;0
WireConnection;58;1;11;0
WireConnection;0;2;61;0
WireConnection;0;9;58;0
ASEEND*/
//CHKSM=0B80E5F577551EF02754420C00E104BEFFABAFD3