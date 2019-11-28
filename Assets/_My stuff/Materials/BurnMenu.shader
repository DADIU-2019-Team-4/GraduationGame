// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BurnMenu"
{
	Properties
	{
		_TextureSample4("Texture Sample 4", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_maintexture("main texture", 2D) = "white" {}
		[HDR]_TextureSample5("Texture Sample 5", 2D) = "white" {}
		_Float0("Float 0", Range( 0 , 1)) = 0
		_Float9("Float 9", Float) = 0.57
		_Float7("Float 7", Float) = 0.5
		[HDR]_Color4("Color 4", Color) = (4,3.32549,0.7843137,0)
		[HDR]_Color3("Color 3", Color) = (4,3.32549,0.7843137,0)
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Float2("Float 2", Float) = 1
		_Wallpapershadow("Wallpaper shadow", Color) = (0.1053452,0.09732112,0.254717,0)
		_burnDarken("burnDarken", Float) = 0
		_BurnDark("BurnDark", Color) = (0.2075472,0.07421782,0.008810962,0)
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

		uniform sampler2D _maintexture;
		uniform float4 _maintexture_ST;
		uniform float _Float0;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float4 _Wallpapershadow;
		uniform float _burnDarken;
		uniform sampler2D _TextureSample4;
		uniform float4 _TextureSample4_ST;
		uniform float4 _BurnDark;
		uniform sampler2D _TextureSample5;
		uniform float _Float9;
		uniform float4 _Color4;
		uniform float4 _Color3;
		uniform sampler2D _TextureSample3;
		uniform float _Float2;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float _Float7;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_maintexture = i.uv_texcoord * _maintexture_ST.xy + _maintexture_ST.zw;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float clampResult9 = clamp( (-4.0 + (( (-0.6 + (( 1.0 - _Float0 ) - 0.0) * (0.6 - -0.6) / (1.0 - 0.0)) + ( 1.0 - tex2D( _TextureSample1, uv_TextureSample1 ).r ) ) - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float temp_output_11_0 = ( 1.0 - clampResult9 );
			float smoothstepResult49 = smoothstep( 0.0 , 1.0 , temp_output_11_0);
			float2 uv_TextureSample4 = i.uv_texcoord * _TextureSample4_ST.xy + _TextureSample4_ST.zw;
			float clampResult69 = clamp( (-4.0 + (( (-0.6 + (( 1.0 - ( _Float0 + _burnDarken ) ) - 0.0) * (0.6 - -0.6) / (1.0 - 0.0)) + ( 1.0 - tex2D( _TextureSample4, uv_TextureSample4 ).r ) ) - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float4 temp_output_76_0 = saturate( ( ( ( 1.0 - clampResult69 ) * _BurnDark ) + smoothstepResult49 ) );
			float clampResult18 = clamp( (-4.0 + (temp_output_11_0 - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float2 appendResult26 = (float2(( 1.0 - clampResult18 ) , 0.0));
			float2 temp_cast_0 = (_SinTime.x).xx;
			float2 panner20 = ( _Float2 * temp_cast_0 + i.uv_texcoord);
			float4 tex2DNode22 = tex2D( _TextureSample3, panner20 );
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 lerpResult40 = lerp( _Color4 , _Color3 , ( tex2DNode22 * saturate( ( abs( _SinTime.x ) + 0.24 ) ) * tex2D( _TextureSample2, uv_TextureSample2 ) * tex2DNode22 ));
			o.Emission = ( ( tex2D( _maintexture, uv_maintexture ) * smoothstepResult49 * ( i.uv_texcoord.y + _Wallpapershadow ) * temp_output_76_0 * temp_output_76_0 ) + float4( 0,0,0,0 ) + ( tex2D( _TextureSample5, appendResult26 ) * smoothstepResult49 * step( temp_output_11_0 , _Float9 ) * lerpResult40 ) ).rgb;
			o.Alpha = ( 1.0 - step( temp_output_11_0 , _Float7 ) );
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
145;644;1768;775;206.7557;1307.969;2.299278;True;True
Node;AmplifyShaderEditor.RangedFloatNode;1;-1000.277,186.2222;Float;False;Property;_Float0;Float 0;4;0;Create;True;0;0;False;0;0;0.469;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;4;-696.4623,223.0857;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-1137.703,922.8862;Float;False;Property;_burnDarken;burnDarken;17;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-766.7743,466.8916;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;6;-567.6461,223.8696;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;48;-457.2165,468.4699;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-979.1594,856.9312;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-360.9003,206.3716;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;63;-906.9761,1112.703;Float;True;Property;_TextureSample4;Texture Sample 4;0;0;Create;True;0;0;False;0;None;e28dc97a9541e3642a48c0e3886688c5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;64;-836.6641,868.8967;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;8;-233.4725,229.8068;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-4;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;66;-597.4183,1114.281;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;65;-707.8479,869.6807;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;67;-501.1021,852.1826;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;9;-65.43789,98.96738;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;10;466.7347,1708.119;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;11;11.61567,-263.052;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;68;-373.6743,875.6178;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-4;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;639.1688,1817.273;Float;False;Constant;_Float1;Float 1;14;0;Create;True;0;0;False;0;0.24;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;17;435.2671,-157.5338;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-4;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;221.1818,1470.193;Float;False;Property;_Float2;Float 2;13;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;96.18176,1201.193;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;15;666.5251,1674.298;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;69;-77.51442,739.3657;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;14;85.18176,1334.193;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;72;1371.746,-905.8502;Float;False;Property;_BurnDark;BurnDark;18;0;Create;True;0;0;False;0;0.2075472,0.07421782,0.008810962,0;0.1226415,0.07289071,0.0480153,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;19;1151.407,1476.203;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;20;401.1818,1220.193;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;73;312.4983,619.3428;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;18;656.5416,-152.3442;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;602.5098,1288.245;Float;True;Property;_TextureSample2;Texture Sample 2;12;0;Create;True;0;0;False;0;None;61c0b9c0523734e0e91bc6043c72a490;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;23;1302.401,1280.277;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;24;928.772,-220.8346;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;1626.037,-879.0313;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;49;1452.88,-488.2918;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;758.563,1001.432;Float;True;Property;_TextureSample3;Texture Sample 3;11;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;956.4705,264.1366;Float;False;Property;_Float9;Float 9;7;0;Create;True;0;0;False;0;0.57;0.71;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;35;1209.56,480.0167;Float;False;Property;_Color4;Color 4;9;1;[HDR];Create;True;0;0;False;0;4,3.32549,0.7843137,0;4.268951,0.5700653,0.140956,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;26;1131.821,-224.4406;Float;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;34;1272.345,688.0558;Float;False;Property;_Color3;Color 3;10;1;[HDR];Create;True;0;0;False;0;4,3.32549,0.7843137,0;43.67059,47.93726,16.31373,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;28;663.5716,-999.8336;Float;False;Property;_Wallpapershadow;Wallpaper shadow;15;0;Create;True;0;0;False;0;0.1053452,0.09732112,0.254717,0;0.8679245,0.8679245,0.8679245,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;74;1786.98,-797.1522;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;1478.347,904.0676;Float;True;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;50;399.537,100.7211;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;647.2239,-1298.584;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;39;1302.526,139.3276;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;76;2007.703,-749.9731;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;43;1392.715,-165.6333;Float;True;Property;_TextureSample5;Texture Sample 5;3;1;[HDR];Create;True;0;0;False;0;None;64e7766099ad46747a07014e44d0aea1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;40;1891.284,89.13831;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;720.7795,-558.142;Float;False;Property;_Float7;Float 7;8;0;Create;True;0;0;False;0;0.5;0.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;51;808.7301,-1440.937;Float;True;Property;_maintexture;main texture;2;0;Create;True;0;0;False;0;53ddad541c3bb26408c8a07eb0b532f6;056f3bf9b199f8f4c99334b2fa7fad33;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;41;997.6321,-1075.689;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;2283.514,-318.7746;Float;True;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;31;1119.472,-559.4373;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;2166.579,-961.79;Float;True;5;5;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;1408.141,-1606.704;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;7130c16fd8005b546b111d341310a9a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;914.7151,-683.7178;Float;False;Property;_Float8;Float 8;6;0;Create;True;0;0;False;0;0;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;36;1188.576,-794.7872;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;2514.511,-857.9067;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;1780.04,-1344.448;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;42;2131.834,-578.0852;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;1083.255,-1573.229;Float;False;Property;_Color1;Color 1;14;0;Create;True;0;0;False;0;0.1226415,0.1226415,0.1226415,0;0.2358491,0.2358491,0.2358491,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;55;-128.1214,-1465.162;Float;False;Property;_Colorgradient;Color gradient ;16;0;Create;True;0;0;False;0;0.1053452,0.09732112,0.254717,0;0.6603774,0.6603774,0.6603774,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;53;205.9388,-1541.018;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;1233.968,-1278.103;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;54;-144.4691,-1763.912;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;33;380.2557,-654.3811;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2869.135,-956.3784;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;BurnMenu;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;1;0
WireConnection;6;0;4;0
WireConnection;48;0;5;1
WireConnection;70;0;1;0
WireConnection;70;1;75;0
WireConnection;7;0;6;0
WireConnection;7;1;48;0
WireConnection;64;0;70;0
WireConnection;8;0;7;0
WireConnection;66;0;63;1
WireConnection;65;0;64;0
WireConnection;67;0;65;0
WireConnection;67;1;66;0
WireConnection;9;0;8;0
WireConnection;11;0;9;0
WireConnection;68;0;67;0
WireConnection;17;0;11;0
WireConnection;15;0;10;1
WireConnection;69;0;68;0
WireConnection;19;0;15;0
WireConnection;19;1;12;0
WireConnection;20;0;16;0
WireConnection;20;2;14;1
WireConnection;20;1;13;0
WireConnection;73;0;69;0
WireConnection;18;0;17;0
WireConnection;23;0;19;0
WireConnection;24;0;18;0
WireConnection;71;0;73;0
WireConnection;71;1;72;0
WireConnection;49;0;11;0
WireConnection;22;1;20;0
WireConnection;26;0;24;0
WireConnection;74;0;71;0
WireConnection;74;1;49;0
WireConnection;32;0;22;0
WireConnection;32;1;23;0
WireConnection;32;2;21;0
WireConnection;32;3;22;0
WireConnection;50;0;11;0
WireConnection;39;0;50;0
WireConnection;39;1;30;0
WireConnection;76;0;74;0
WireConnection;43;1;26;0
WireConnection;40;0;35;0
WireConnection;40;1;34;0
WireConnection;40;2;32;0
WireConnection;41;0;27;2
WireConnection;41;1;28;0
WireConnection;46;0;43;0
WireConnection;46;1;49;0
WireConnection;46;2;39;0
WireConnection;46;3;40;0
WireConnection;31;0;11;0
WireConnection;31;1;25;0
WireConnection;44;0;51;0
WireConnection;44;1;49;0
WireConnection;44;2;41;0
WireConnection;44;3;76;0
WireConnection;44;4;76;0
WireConnection;36;0;33;0
WireConnection;36;1;29;0
WireConnection;47;0;44;0
WireConnection;47;2;46;0
WireConnection;37;0;3;0
WireConnection;37;1;2;0
WireConnection;42;0;31;0
WireConnection;53;0;54;2
WireConnection;53;1;55;0
WireConnection;52;1;53;0
WireConnection;33;0;11;0
WireConnection;0;2;47;0
WireConnection;0;9;42;0
ASEEND*/
//CHKSM=8FDB2FEB2FA681A5172B50156736D52387FF95FD