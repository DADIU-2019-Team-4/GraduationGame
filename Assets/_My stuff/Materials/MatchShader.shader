// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MatchShader"
{
	Properties
	{
		_maintexture("main texture", 2D) = "white" {}
		_DisolveGuide("Disolve Guide", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_DissolveAmount("Dissolve Amount", Range( 0 , 1)) = 0
		_Float1("Float 1", Range( 0 , 1)) = 0
		_BurntTexture("BurntTexture", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_BurntColor("BurntColor", Color) = (0.1226415,0.1226415,0.1226415,0)
		_RedGlowAmount("Red Glow Amount", Range( 0 , 1)) = 0
		_Colorgradient("Color gradient ", Color) = (0.1053452,0.09732112,0.254717,0)
		_TextureSample4("Texture Sample 4", 2D) = "white" {}
		[HDR]_Color2("Color 2", Color) = (1,0.2454576,0,0)
		[HDR]_Color0("Color 0", Color) = (1,0,0,0)
		_BurntWood("BurntWood", Color) = (0.2641509,0.07947904,0,0)
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
		uniform float4 _Colorgradient;
		uniform float _DissolveAmount;
		uniform sampler2D _DisolveGuide;
		uniform float4 _DisolveGuide_ST;
		uniform float _Float1;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float4 _BurntWood;
		uniform sampler2D _BurntTexture;
		uniform float4 _BurntTexture_ST;
		uniform float4 _BurntColor;
		uniform float4 _Color0;
		uniform float4 _Color2;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _RedGlowAmount;
		uniform sampler2D _TextureSample4;
		uniform float4 _TextureSample4_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_maintexture = i.uv_texcoord * _maintexture_ST.xy + _maintexture_ST.zw;
			float2 uv_TexCoord49 = i.uv_texcoord + float2( -0.58,-0.12 );
			float smoothstepResult101 = smoothstep( -0.14 , -0.03 , uv_TexCoord49.x);
			float4 temp_output_53_0 = ( ( 1.0 - smoothstepResult101 ) + _Colorgradient );
			float2 uv_DisolveGuide = i.uv_texcoord * _DisolveGuide_ST.xy + _DisolveGuide_ST.zw;
			float clampResult9 = clamp( (-4.0 + (( (-0.6 + (( 1.0 - _DissolveAmount ) - 0.0) * (0.6 - -0.6) / (1.0 - 0.0)) + tex2D( _DisolveGuide, uv_DisolveGuide ).r + ( 1.0 - i.uv_texcoord.y ) ) - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float temp_output_10_0 = ( 1.0 - clampResult9 );
			float smoothstepResult78 = smoothstep( 0.0 , 0.42 , ( 1.0 - temp_output_10_0 ));
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float clampResult90 = clamp( (-4.0 + (( (-0.6 + (( 1.0 - _Float1 ) - 0.0) * (0.6 - -0.6) / (1.0 - 0.0)) + tex2D( _TextureSample2, uv_TextureSample2 ).r + ( 1.0 - i.uv_texcoord.y ) ) - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float smoothstepResult79 = smoothstep( 0.46 , 0.94 , ( 1.0 - clampResult90 ));
			float2 uv_BurntTexture = i.uv_texcoord * _BurntTexture_ST.xy + _BurntTexture_ST.zw;
			float4 tex2DNode23 = tex2D( _BurntTexture, uv_BurntTexture );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 temp_output_25_0 = ( tex2D( _TextureSample0, uv_TextureSample0 ) * saturate( ( abs( _SinTime.y ) + 0.24 ) ) * tex2D( _TextureSample1, uv_TextureSample1 ) );
			float4 lerpResult75 = lerp( _Color0 , _Color2 , temp_output_25_0);
			o.Emission = ( ( ( float4( 0,0,0,0 ) + ( tex2D( _maintexture, uv_maintexture ) * ( uv_TexCoord49.x + temp_output_53_0 ) * ( uv_TexCoord49.y + ( uv_TexCoord49.y * _Colorgradient ) ) ) ) * smoothstepResult78 * ( ( smoothstepResult79 * _BurntWood * tex2DNode23 ) + ( 1.0 - smoothstepResult79 ) ) ) + ( temp_output_53_0 * ( ( tex2DNode23 * _BurntColor ) + ( lerpResult75 * temp_output_25_0 * _RedGlowAmount ) ) * temp_output_10_0 ) ).rgb;
			float2 uv_TextureSample4 = i.uv_texcoord * _TextureSample4_ST.xy + _TextureSample4_ST.zw;
			o.Alpha = tex2D( _TextureSample4, uv_TextureSample4 ).r;
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
7;508;1906;504;591.8365;785.1216;1.236278;True;True
Node;AmplifyShaderEditor.RangedFloatNode;85;-1323.473,1250.445;Float;False;Property;_Float1;Float 1;4;0;Create;True;0;0;False;0;0;0.727;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;92;-1040.804,941.4494;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;86;-985.2747,1251.758;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-3756.235,823.6134;Float;False;Property;_DissolveAmount;Dissolve Amount;3;0;Create;True;0;0;False;0;0;0.416;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;87;-1024.886,1450.229;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;None;43213caca3f4a584fb048dba0613a270;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;88;-795.6606,1242.975;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;38;-2709.514,-303.3873;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;72;-3473.566,514.6173;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;4;-3418.037,824.9257;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;94;-702.4235,1026.856;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;6;-3228.423,816.1434;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-3457.648,1023.397;Float;True;Property;_DisolveGuide;Disolve Guide;1;0;Create;True;0;0;False;0;None;43213caca3f4a584fb048dba0613a270;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;89;-588.9145,1225.477;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-340.7883,-800.4543;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.58,-0.12;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;33;-2509.724,-337.2074;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;73;-3135.186,600.0235;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2537.08,-194.2334;Float;False;Constant;_Float0;Float 0;14;0;Create;True;0;0;False;0;0.24;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;101;-65.8295,-591.27;Float;True;3;0;FLOAT;0;False;1;FLOAT;-0.14;False;2;FLOAT;-0.03;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;93;-251.8475,1267.862;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-4;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-3021.677,798.6454;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-2352.48,-333.3334;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;99;184.9361,-431.3624;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;90;9.139507,1223.774;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;27;-2302.315,-466.3944;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;36;-2417.686,-1010.073;Float;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;24;-2573.739,-723.2604;Float;True;Property;_TextureSample1;Texture Sample 1;8;0;Create;True;0;0;False;0;None;61c0b9c0523734e0e91bc6043c72a490;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;8;-2684.61,841.0297;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-4;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;51;-86.37251,-287.5781;Float;False;Property;_Colorgradient;Color gradient ;11;0;Create;True;0;0;False;0;0.1053452,0.09732112,0.254717,0;0.6313726,0.4194788,0.4039216,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;9;-2423.623,796.9415;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1955.595,-202.2839;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;340.7178,-693.9852;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;91;247.7115,1122.189;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;76;-1860.116,-387.5779;Float;False;Property;_Color0;Color 0;14;1;[HDR];Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;53;337.0926,-310.3902;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;32;-1675.762,-561.9766;Float;False;Property;_Color2;Color 2;13;1;[HDR];Create;True;0;0;False;0;1,0.2454576,0,0;20.35602,4.310686,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;75;-1337.789,-241.5664;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;59;774.953,-771.847;Float;True;Property;_maintexture;main texture;0;0;Create;True;0;0;False;0;53ddad541c3bb26408c8a07eb0b532f6;38da61131e89bc9428955119858b6578;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-1334.31,-753.2543;Float;False;Property;_BurntColor;BurntColor;9;0;Create;True;0;0;False;0;0.1226415,0.1226415,0.1226415,0;0.1792453,0.1445799,0.1445799,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;79;-431.6816,130.7389;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.46;False;2;FLOAT;0.94;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;100;682.509,-431.6846;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;10;-2185.051,695.3572;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;23;-957.8556,-758.1609;Float;True;Property;_BurntTexture;BurntTexture;5;0;Create;True;0;0;False;0;None;7130c16fd8005b546b111d341310a9a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;82;-266.0494,287.8205;Float;False;Property;_BurntWood;BurntWood;15;0;Create;True;0;0;False;0;0.2641509,0.07947904,0,0;0.6792453,0.1630189,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;63;-1145.845,30.86605;Float;False;Property;_RedGlowAmount;Red Glow Amount;10;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;106;444.7371,-928.3588;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;84;47.22317,305.7063;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-849.2675,-539.5071;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;951.8026,-437.7647;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-827.8328,-174.0518;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-9.351858,71.26848;Float;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;56;-1053.82,211.6159;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;78;-235.6836,-0.8251936;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.42;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;102;1220.618,-316.2699;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;83;213.5813,108.6035;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-519.6181,-231.7023;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;989.4295,243.2059;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;1006.054,-46.16841;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;104;150.4955,-782.6609;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1527.87,703.1814;Float;False;Property;_Float5;Float 5;6;0;Create;True;0;0;False;0;0.5;0.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;71;1367.245,601.3689;Float;True;Property;_TextureSample4;Texture Sample 4;12;0;Create;True;0;0;False;0;None;6875392c4074db84ebc77880ed0f85d9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;67;1343.547,116.9543;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;48;-1291.633,481.7118;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;107;142.0113,-917.7599;Float;False;3;0;FLOAT;0;False;1;FLOAT;0.18;False;2;FLOAT;0.45;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1725.462,101.8984;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MatchShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;86;0;85;0
WireConnection;88;0;86;0
WireConnection;4;0;3;0
WireConnection;94;0;92;2
WireConnection;6;0;4;0
WireConnection;89;0;88;0
WireConnection;89;1;87;1
WireConnection;89;2;94;0
WireConnection;33;0;38;2
WireConnection;73;0;72;2
WireConnection;101;0;49;1
WireConnection;93;0;89;0
WireConnection;7;0;6;0
WireConnection;7;1;5;1
WireConnection;7;2;73;0
WireConnection;28;0;33;0
WireConnection;28;1;30;0
WireConnection;99;0;101;0
WireConnection;90;0;93;0
WireConnection;27;0;28;0
WireConnection;8;0;7;0
WireConnection;9;0;8;0
WireConnection;25;0;36;0
WireConnection;25;1;27;0
WireConnection;25;2;24;0
WireConnection;105;0;49;2
WireConnection;105;1;51;0
WireConnection;91;0;90;0
WireConnection;53;0;99;0
WireConnection;53;1;51;0
WireConnection;75;0;76;0
WireConnection;75;1;32;0
WireConnection;75;2;25;0
WireConnection;79;0;91;0
WireConnection;100;0;49;1
WireConnection;100;1;53;0
WireConnection;10;0;9;0
WireConnection;106;0;49;2
WireConnection;106;1;105;0
WireConnection;84;0;79;0
WireConnection;54;0;23;0
WireConnection;54;1;22;0
WireConnection;96;0;59;0
WireConnection;96;1;100;0
WireConnection;96;2;106;0
WireConnection;77;0;75;0
WireConnection;77;1;25;0
WireConnection;77;2;63;0
WireConnection;81;0;79;0
WireConnection;81;1;82;0
WireConnection;81;2;23;0
WireConnection;56;0;10;0
WireConnection;78;0;56;0
WireConnection;102;1;96;0
WireConnection;83;0;81;0
WireConnection;83;1;84;0
WireConnection;70;0;54;0
WireConnection;70;1;77;0
WireConnection;68;0;53;0
WireConnection;68;1;70;0
WireConnection;68;2;10;0
WireConnection;60;0;102;0
WireConnection;60;1;78;0
WireConnection;60;2;83;0
WireConnection;104;0;49;2
WireConnection;67;0;60;0
WireConnection;67;1;68;0
WireConnection;48;0;10;0
WireConnection;48;1;20;0
WireConnection;107;0;49;2
WireConnection;0;2;67;0
WireConnection;0;9;71;0
ASEEND*/
//CHKSM=EE31D48772DE276E3118404B269D7BEFDBA1DFA2