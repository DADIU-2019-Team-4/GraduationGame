// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "flame/surface"
{
	Properties
	{
		_F_bias("F_bias", Range( 0 , 1)) = 0.2
		[HDR]_FlameColor("Flame Color", Color) = (0.9811321,0.5481471,0,0)
		[HDR]_Flamecolor2("Flame color2", Color) = (0.9803922,0.2173763,0,0)
		_F_scale("F_scale", Range( 0 , 5)) = 1.5
		_F_power("F_power", Range( 0 , 5)) = 1
		_Y_mask("Y_mask", Range( 0 , 5)) = 0
		_Float0("Float 0", Range( 0 , 5)) = 0
		_FlameHeight("FlameHeight", Range( 0 , 1)) = 0
		_FlameWave("Flame Wave", 2D) = "white" {}
		_FlameNoise("Flame Noise", 2D) = "white" {}
		_v("v", Range( -1 , 1)) = 0
		_U("U", Range( -1 , 1)) = 0
		_Flameon("Flame on", Range( 0 , 1)) = 0
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
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform sampler2D _FlameWave;
		uniform float _U;
		uniform float _v;
		uniform sampler2D _FlameNoise;
		uniform float4 _FlameNoise_ST;
		uniform float _Y_mask;
		uniform float _Float0;
		uniform float _FlameHeight;
		uniform float4 _Flamecolor2;
		uniform float4 _FlameColor;
		uniform float _F_bias;
		uniform float _F_scale;
		uniform float _F_power;
		uniform float _Flameon;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 transform23 = mul(unity_WorldToObject,float4( float3(0,1,0) , 0.0 ));
			float4 appendResult36 = (float4(_U , _v , 0.0 , 0.0));
			float2 uv0_FlameNoise = v.texcoord.xy * _FlameNoise_ST.xy + _FlameNoise_ST.zw;
			float2 panner31 = ( 1.0 * _Time.y * appendResult36.xy + uv0_FlameNoise);
			float4 temp_output_37_0 = ( tex2Dlod( _FlameWave, float4( panner31, 0, 0.0) ) * tex2Dlod( _FlameNoise, float4( panner31, 0, 0.0) ) );
			float4 lerpResult30 = lerp( float4( 0,0,0,0 ) , transform23 , temp_output_37_0);
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float clampResult18 = clamp( ( distance( ase_worldNormal.y , _Y_mask ) - _Float0 ) , 0.0 , 1.0 );
			float temp_output_20_0 = ( 1.0 - clampResult18 );
			float4 lerpResult24 = lerp( float4( 0,0,0,0 ) , lerpResult30 , temp_output_20_0);
			v.vertex.xyz += ( lerpResult24 * _FlameHeight ).xyz;
			float3 ase_vertexNormal = v.normal.xyz;
			v.normal = ase_vertexNormal;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode1 = ( _F_bias + _F_scale * pow( 1.0 - fresnelNdotV1, _F_power ) );
			float4 lerpResult11 = lerp( _Flamecolor2 , _FlameColor , saturate( fresnelNode1 ));
			o.Emission = lerpResult11.rgb;
			float clampResult18 = clamp( ( distance( ase_worldNormal.y , _Y_mask ) - _Float0 ) , 0.0 , 1.0 );
			float temp_output_20_0 = ( 1.0 - clampResult18 );
			float4 appendResult36 = (float4(_U , _v , 0.0 , 0.0));
			float2 uv0_FlameNoise = i.uv_texcoord * _FlameNoise_ST.xy + _FlameNoise_ST.zw;
			float2 panner31 = ( 1.0 * _Time.y * appendResult36.xy + uv0_FlameNoise);
			float4 temp_output_37_0 = ( tex2D( _FlameWave, panner31 ) * tex2D( _FlameNoise, panner31 ) );
			float4 lerpResult39 = lerp( float4( 0,0,0,0 ) , ( fresnelNode1 * temp_output_20_0 * temp_output_37_0 ) , _Flameon);
			o.Alpha = lerpResult39.r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
0;120;1906;899;2194.309;772.9565;1.852108;True;True
Node;AmplifyShaderEditor.RangedFloatNode;34;-2103.588,873.2445;Float;False;Property;_v;v;10;0;Create;True;0;0;False;0;0;-0.2;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-2061.664,797.325;Float;False;Property;_U;U;11;0;Create;True;0;0;False;0;0;-0.1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;12;-2226.864,224.0191;Float;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;13;-2202.493,458.4007;Float;False;Property;_Y_mask;Y_mask;5;0;Create;True;0;0;False;0;0;2.47;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;36;-1786.689,803.458;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-1814.641,676.0801;Float;False;0;28;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;15;-1871.655,271.859;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;31;-1399.453,671.556;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1979.325,561.7627;Float;False;Property;_Float0;Float 0;6;0;Create;True;0;0;False;0;0;1.25;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;27;-1162.445,469.3656;Float;True;Property;_FlameWave;Flame Wave;8;0;Create;True;0;0;False;0;None;9fbef4b79ca3b784ba023cb1331520d5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-1162.448,663.9778;Float;True;Property;_FlameNoise;Flame Noise;9;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;16;-1613.122,485.5503;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;22;-931.4377,956.695;Float;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldToObjectTransfNode;23;-686.2151,847.6443;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;18;-1413.296,406.8085;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-811.8077,654.2961;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1368.89,-215.8214;Float;False;Property;_F_bias;F_bias;0;0;Create;True;0;0;False;0;0.2;0.6769869;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1395.741,-137.2372;Float;False;Property;_F_scale;F_scale;3;0;Create;True;0;0;False;0;1.5;0.51;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1299.351,-43.45162;Float;False;Property;_F_power;F_power;4;0;Create;True;0;0;False;0;1;0.44;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;1;-1031.334,-234.5344;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;20;-876.2236,229.8861;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;30;-411.247,728.5272;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;24;-179.1752,873.7971;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-236.3516,1084.555;Float;False;Property;_FlameHeight;FlameHeight;7;0;Create;True;0;0;False;0;0;0.432;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-199.2748,500.2772;Float;False;Property;_Flameon;Flame on;12;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-962.5671,-656.2366;Float;False;Property;_Flamecolor2;Flame color2;2;1;[HDR];Create;True;0;0;False;0;0.9803922,0.2173763,0,0;11.98431,0.9411765,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-378.3637,145.508;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;2;-1048.576,-467.8832;Float;False;Property;_FlameColor;Flame Color;1;1;[HDR];Create;True;0;0;False;0;0.9811321,0.5481471,0,0;5.992157,4.988235,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;41;-705.2136,-280.2957;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;21;322.3908,749.6616;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;102.0881,888.9014;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;39;112.8137,370.7004;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;11;-521.22,-371.5511;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;473.0574,-77.61099;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;flame/surface;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;36;0;33;0
WireConnection;36;1;34;0
WireConnection;15;0;12;2
WireConnection;15;1;13;0
WireConnection;31;0;32;0
WireConnection;31;2;36;0
WireConnection;27;1;31;0
WireConnection;28;1;31;0
WireConnection;16;0;15;0
WireConnection;16;1;17;0
WireConnection;23;0;22;0
WireConnection;18;0;16;0
WireConnection;37;0;27;0
WireConnection;37;1;28;0
WireConnection;1;1;3;0
WireConnection;1;2;6;0
WireConnection;1;3;7;0
WireConnection;20;0;18;0
WireConnection;30;1;23;0
WireConnection;30;2;37;0
WireConnection;24;1;30;0
WireConnection;24;2;20;0
WireConnection;19;0;1;0
WireConnection;19;1;20;0
WireConnection;19;2;37;0
WireConnection;41;0;1;0
WireConnection;26;0;24;0
WireConnection;26;1;25;0
WireConnection;39;1;19;0
WireConnection;39;2;40;0
WireConnection;11;0;10;0
WireConnection;11;1;2;0
WireConnection;11;2;41;0
WireConnection;0;2;11;0
WireConnection;0;9;39;0
WireConnection;0;11;26;0
WireConnection;0;12;21;0
ASEEND*/
//CHKSM=D67869E34410DDBB04E7163AF7CF49ACF7DFC2AE