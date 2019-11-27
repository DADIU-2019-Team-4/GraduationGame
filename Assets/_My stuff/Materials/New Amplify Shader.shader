// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PaperDissolve"
{
	Properties
	{
		_maintexture("main texture", 2D) = "white" {}
		_DisolveGuide("Disolve Guide", 2D) = "white" {}
		[HDR]_burnramp("burn ramp", 2D) = "white" {}
		_DissolveAmount("Dissolve Amount", Range( 0 , 1)) = 0
		_BurntTexture("BurntTexture", 2D) = "white" {}
		_Float3("Float 3", Float) = 0
		_BurnGlow("BurnGlow", Range( 0 , 1)) = 0.57
		_Float5("Float 5", Float) = 0.5
		[HDR]_DissolveBoost("DissolveBoost", Color) = (4,3.32549,0.7843137,0)
		[HDR]_Color0("Color 0", Color) = (4,3.32549,0.7843137,0)
		_Redglownoise("Red glow noise", 2D) = "white" {}
		_GlowSpeed("GlowSpeed", Float) = 1
		_BurntColor("BurntColor", Color) = (0.1226415,0.1226415,0.1226415,0)
		_Colorgradient("Color gradient ", Color) = (0.1053452,0.09732112,0.254717,0)
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

		uniform sampler2D _BurntTexture;
		uniform float4 _BurntTexture_ST;
		uniform float4 _BurntColor;
		uniform float _DissolveAmount;
		uniform sampler2D _DisolveGuide;
		uniform float4 _DisolveGuide_ST;
		uniform float _Float5;
		uniform sampler2D _maintexture;
		uniform float4 _maintexture_ST;
		uniform float _Float3;
		uniform float4 _Colorgradient;
		uniform sampler2D _burnramp;
		uniform float _BurnGlow;
		uniform float4 _DissolveBoost;
		uniform float4 _Color0;
		uniform sampler2D _Redglownoise;
		uniform float _GlowSpeed;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BurntTexture = i.uv_texcoord * _BurntTexture_ST.xy + _BurntTexture_ST.zw;
			float4 temp_output_40_0 = ( tex2D( _BurntTexture, uv_BurntTexture ) * _BurntColor );
			float2 uv_DisolveGuide = i.uv_texcoord * _DisolveGuide_ST.xy + _DisolveGuide_ST.zw;
			float4 tex2DNode10 = tex2D( _DisolveGuide, uv_DisolveGuide );
			float clampResult14 = clamp( (-4.0 + (( (-0.6 + (( 1.0 - _DissolveAmount ) - 0.0) * (0.6 - -0.6) / (1.0 - 0.0)) + tex2DNode10.r ) - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float temp_output_15_0 = ( 1.0 - clampResult14 );
			float temp_output_37_0 = ( 1.0 - step( temp_output_15_0 , _Float5 ) );
			float2 uv_maintexture = i.uv_texcoord * _maintexture_ST.xy + _maintexture_ST.zw;
			float4 temp_output_7_0 = ( i.uv_texcoord.y + _Colorgradient );
			float clampResult20 = clamp( (-4.0 + (temp_output_15_0 - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float2 appendResult33 = (float2(( 1.0 - clampResult20 ) , 0.0));
			float2 temp_cast_0 = (_SinTime.x).xx;
			float2 panner19 = ( _GlowSpeed * temp_cast_0 + i.uv_texcoord);
			float4 tex2DNode24 = tex2D( _Redglownoise, panner19 );
			float4 lerpResult42 = lerp( _DissolveBoost , _Color0 , ( tex2DNode24 * saturate( ( abs( _SinTime.x ) + 0.24 ) ) * tex2DNode24 ));
			o.Emission = ( ( temp_output_40_0 * temp_output_37_0 ) + ( tex2D( _maintexture, uv_maintexture ) * step( temp_output_15_0 , _Float3 ) * temp_output_7_0 ) + ( tex2D( _burnramp, appendResult33 ) * temp_output_37_0 * step( temp_output_15_0 , _BurnGlow ) * lerpResult42 ) ).rgb;
			float smoothstepResult70 = smoothstep( 0.85 , 0.8 , temp_output_15_0);
			o.Alpha = smoothstepResult70;
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
7;209;1906;810;2023.292;779.9362;1.819747;True;True
Node;AmplifyShaderEditor.RangedFloatNode;8;-909.5849,-399.1815;Float;False;Property;_DissolveAmount;Dissolve Amount;3;0;Create;True;0;0;False;0;0;0.605;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;9;-571.387,-397.8693;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-610.9984,-199.3976;Float;True;Property;_DisolveGuide;Disolve Guide;1;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;11;-381.7729,-406.6515;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-175.0271,-424.1495;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;13;47.12283,-324.3068;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-4;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;63;332.2455,327.4206;Float;False;2077.146;1493.975;Comment;16;61;28;1;18;16;3;4;2;19;5;24;22;6;34;42;35;Edge glow variation;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;14;74.11102,-442.4685;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;15;232.9316,-517.4686;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;1;763.7983,1597.241;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;936.232,1706.395;Float;False;Constant;_Float6;Float 6;14;0;Create;True;0;0;False;0;0.24;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;393.2455,1090.315;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;518.2455,1359.315;Float;False;Property;_GlowSpeed;GlowSpeed;13;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;17;595.869,-384.4031;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-4;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;16;382.2455,1223.315;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;4;963.5881,1563.421;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;5;1448.47,1365.326;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;19;698.2453,1109.315;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;20;923.7803,-564.9604;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;23;1149.821,-582.5409;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;6;1429.464,1254.4;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;740.4642,-884.1157;Float;False;Property;_Float5;Float 5;7;0;Create;True;0;0;False;0;0.5;0.48;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;1055.626,890.5554;Float;True;Property;_Redglownoise;Red glow noise;10;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;31;1860.615,-2107.764;Float;True;Property;_BurntTexture;BurntTexture;4;0;Create;True;0;0;False;0;None;7130c16fd8005b546b111d341310a9a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;1512.34,-1976.049;Float;False;Property;_BurntColor;BurntColor;15;0;Create;True;0;0;False;0;0.1226415,0.1226415,0.1226415,0;0.6981132,0.6981132,0.6981132,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;28;1607.632,547.5164;Float;False;Property;_Color0;Color 0;9;1;[HDR];Create;True;0;0;False;0;4,3.32549,0.7843137,0;43.67059,47.93726,16.31373,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;33;1375.944,-543.8418;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;29;1190.515,-871.408;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;2613.974,-1633.622;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;1032.785,-1099.878;Float;False;Property;_Float3;Float 3;5;0;Create;True;0;0;False;0;0;0.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;35;1650.245,377.4206;Float;False;Property;_DissolveBoost;DissolveBoost;8;1;[HDR];Create;True;0;0;False;0;4,3.32549,0.7843137,0;2.037736,0.915867,0.3556426,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;1775.41,793.1913;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;25;2808.366,-1509.212;Float;False;Property;_Colorgradient;Color gradient ;18;0;Create;True;0;0;False;0;0.1053452,0.09732112,0.254717,0;0.3448052,0.3420256,0.3962264,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;32;1402.492,-140.7256;Float;False;Property;_BurnGlow;BurnGlow;6;0;Create;True;0;0;False;0;0.57;0.56;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;2212.765,-1735.604;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;36;1712.703,-1460.882;Float;True;Property;_maintexture;main texture;0;0;Create;True;0;0;False;0;53ddad541c3bb26408c8a07eb0b532f6;3091e3661b4c5864fa2672a383092147;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;39;1560.809,-1108.463;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;41;1633.239,-542.6068;Float;True;Property;_burnramp;burn ramp;2;1;[HDR];Create;True;0;0;False;0;None;97a8e753cd5d5ac4c8e96283d73a10aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;42;2144.391,549.9832;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;3211.507,-1338.223;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;38;1748.548,-264.5345;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;37;1403.553,-892.0352;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;2183.601,-1163.441;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;2251.896,-684.7624;Float;True;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;2395.477,-945.4242;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;48;544.3352,-1689.189;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;309.5702,-1417.028;Float;False;Constant;_Float0;Float 0;14;0;Create;True;0;0;False;0;0.24;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;55;272.9113,-1946.055;Float;True;Property;_TextureSample1;Texture Sample 1;14;0;Create;True;0;0;False;0;None;61c0b9c0523734e0e91bc6043c72a490;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;67;1119.021,-1478.139;Float;True;Step Antialiasing;-1;;1;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-287.7039,-42.6772;Float;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;90;-850.9712,424.1236;Float;True;Property;_TextureSample3;Texture Sample 3;20;0;Create;True;0;0;False;0;None;ee7c95f1ad634a745afef482a4afcb28;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;1389.086,-1729.038;Float;True;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;65;1057.025,-1255.964;Float;False;Constant;_Float1;Float 1;19;0;Create;True;0;0;False;0;0.98;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;1097.117,-1563.361;Float;False;Property;_RedGlowAmount;Red Glow Amount;17;0;Create;True;0;0;False;0;0;0.104795;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;58;1338.818,-2023.136;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;716.3032,-1881.816;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;70;410.8195,-1041.681;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.85;False;2;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;494.1702,-1556.128;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;91;-600.938,86.75919;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;52;960.8662,-1797.322;Float;True;Property;_RedGlowramp;RedGlow ramp;16;0;Create;True;0;0;False;0;None;c2f70a58c1100504ca5b09fc2c27dcec;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;53;2162.836,-1500.998;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;22;899.5729,1177.368;Float;True;Property;_Redglownoiseb;Red glow noise b;11;0;Create;True;0;0;False;0;None;61c0b9c0523734e0e91bc6043c72a490;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;49;137.1362,-1526.182;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;50;984.5963,-2019.508;Float;False;Constant;_Color2;Color 2;19;0;Create;True;0;0;False;0;1,0.2454576,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;57;336.9263,-1560.002;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;2557.119,-1227.948;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;71;-861.8999,75.62207;Float;True;Property;_TextureSample2;Texture Sample 2;19;0;Create;True;0;0;False;0;None;758f9723b9e34bc4aa5de4b104ec9257;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;51;428.9642,-2232.868;Float;True;Property;_TextureSample0;Texture Sample 0;12;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;54;3136.616,-938.7691;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3727.87,-1282.678;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;PaperDissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;61;1352.724,1559.782;Float;False;100;100;Comment;0;;1,1,1,1;0;0
WireConnection;9;0;8;0
WireConnection;11;0;9;0
WireConnection;12;0;11;0
WireConnection;12;1;10;1
WireConnection;13;0;12;0
WireConnection;14;0;13;0
WireConnection;15;0;14;0
WireConnection;17;0;15;0
WireConnection;4;0;1;1
WireConnection;5;0;4;0
WireConnection;5;1;2;0
WireConnection;19;0;18;0
WireConnection;19;2;16;1
WireConnection;19;1;3;0
WireConnection;20;0;17;0
WireConnection;23;0;20;0
WireConnection;6;0;5;0
WireConnection;24;1;19;0
WireConnection;33;0;23;0
WireConnection;29;0;15;0
WireConnection;29;1;21;0
WireConnection;34;0;24;0
WireConnection;34;1;6;0
WireConnection;34;2;24;0
WireConnection;40;0;31;0
WireConnection;40;1;30;0
WireConnection;39;0;15;0
WireConnection;39;1;27;0
WireConnection;41;1;33;0
WireConnection;42;0;35;0
WireConnection;42;1;28;0
WireConnection;42;2;34;0
WireConnection;7;0;26;2
WireConnection;7;1;25;0
WireConnection;38;0;15;0
WireConnection;38;1;32;0
WireConnection;37;0;29;0
WireConnection;66;0;40;0
WireConnection;66;1;37;0
WireConnection;45;0;41;0
WireConnection;45;1;37;0
WireConnection;45;2;38;0
WireConnection;45;3;42;0
WireConnection;43;0;36;0
WireConnection;43;1;39;0
WireConnection;43;2;7;0
WireConnection;48;0;60;0
WireConnection;73;0;10;1
WireConnection;73;1;10;1
WireConnection;73;2;91;0
WireConnection;73;3;90;1
WireConnection;56;0;58;0
WireConnection;56;1;50;0
WireConnection;56;2;52;0
WireConnection;56;3;47;0
WireConnection;58;0;51;1
WireConnection;46;0;51;0
WireConnection;46;1;48;0
WireConnection;46;2;55;0
WireConnection;70;0;15;0
WireConnection;60;0;57;0
WireConnection;60;1;59;0
WireConnection;91;0;71;1
WireConnection;52;1;46;0
WireConnection;53;0;56;0
WireConnection;53;1;40;0
WireConnection;57;0;49;2
WireConnection;44;0;40;0
WireConnection;44;1;37;0
WireConnection;44;2;7;0
WireConnection;54;0;66;0
WireConnection;54;1;43;0
WireConnection;54;2;45;0
WireConnection;0;2;54;0
WireConnection;0;9;70;0
ASEEND*/
//CHKSM=59FF7F30B5108B4CADCD78CEC91FD68AE0D65228