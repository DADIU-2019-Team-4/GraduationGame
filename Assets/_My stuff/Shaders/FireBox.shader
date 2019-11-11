// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FireBox"
{
	Properties
	{
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Texture3("Texture 3", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
		};

		uniform sampler2D _Texture3;
		uniform sampler2D _TextureSample1;
		uniform sampler2D _TextureSample2;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color29 = IsGammaSpace() ? float4(1.498039,1.498039,1.498039,0) : float4(2.433049,2.433049,2.433049,0);
			float temp_output_14_0 = ( 1.0 - ( i.uv_texcoord.y - -1.41 ) );
			float mulTime23 = _Time.y * 0.8;
			float2 appendResult25 = (float2(0.0 , -1.0));
			float2 uv_TexCoord27 = i.uv_texcoord * float2( 1.5,1 );
			float2 panner26 = ( mulTime23 * appendResult25 + ( uv_TexCoord27 * 0.54 ));
			float4 tex2DNode17 = tex2D( _TextureSample1, panner26 );
			float mulTime55 = _Time.y * 0.5;
			float2 appendResult54 = (float2(0.0 , -0.9));
			float2 uv_TexCoord57 = i.uv_texcoord * float2( 1.5,1 );
			float2 panner56 = ( mulTime55 * appendResult54 + ( uv_TexCoord57 * 0.34 ));
			float4 temp_output_32_0 = saturate( ( ( temp_output_14_0 + tex2DNode17 ) + ( temp_output_14_0 + tex2DNode17 + tex2D( _TextureSample2, panner56 ) ) ) );
			o.Emission = ( color29 * saturate( tex2D( _Texture3, temp_output_32_0.rg, float2( 0,0 ), float2( 0,0 ) ) ) ).rgb;
			float3 ase_worldNormal = i.worldNormal;
			o.Alpha = ( ( temp_output_32_0 * 1.0 ) * ( 1.0 - ase_worldNormal.y ) ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17000
-46;556;1906;495;1269.013;231.6808;2.176443;True;True
Node;AmplifyShaderEditor.RangedFloatNode;20;-1216,1184;Float;False;Constant;_8;,8;1;0;Create;True;0;0;False;0;0.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1360,928;Float;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;False;0;0.54;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-1392,752;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.5,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;-1376,1056;Float;False;Constant;_Float3;Float 3;1;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1045.025,1881.742;Float;False;Constant;_Float5;Float 5;1;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;57;-1192.811,1416.116;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.5,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;50;-1160.811,1591.116;Float;False;Constant;_Float4;Float 4;1;0;Create;True;0;0;False;0;0.34;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1183.025,1710.742;Float;False;Constant;_Float6;Float 6;1;0;Create;True;0;0;False;0;-0.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;55;-840.025,1847.742;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-895.025,1566.742;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;54;-895.025,1742.742;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-830.7285,716.7858;Float;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;-1.41;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;-1088,1088;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1088,912;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;23;-992,1200;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-931.0463,363.8354;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;26;-832,1024;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;-512.3317,513.4055;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;56;-687.025,1694.742;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;48;-411.003,1156.19;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;None;f87d8c8d5cc498648a4c4638e2198a53;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;14;-270.3327,503.3477;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-462.1678,862.2379;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;49;37.73068,888.3528;Float;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-21.90872,587.0956;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;265.1323,659.0583;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;32;348.2533,373.2271;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;31;71.12527,163.6509;Float;True;Property;_Texture3;Texture 3;1;0;Create;True;0;0;False;0;92ed0dec3122fc742a61517ba54e65bd;c2f70a58c1100504ca5b09fc2c27dcec;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.WorldNormalVector;2;742.6791,557.259;Float;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;30;455.6038,85.59745;Float;True;Property;_TextureSample5;Texture Sample 5;9;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;60;613.5712,475.6344;Float;False;Constant;_Float7;Float 7;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;758.6666,276.2725;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;4;1000.136,388.7021;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;456.8658,-123.3872;Float;False;Constant;_Color0;Color 0;2;1;[HDR];Create;True;0;0;False;0;1.498039,1.498039,1.498039,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;80;791.7107,13.70412;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;1164.973,103.5882;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;980.8662,-141.5009;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1419.28,-105.0827;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;FireBox;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;55;0;51;0
WireConnection;53;0;57;0
WireConnection;53;1;50;0
WireConnection;54;1;52;0
WireConnection;25;1;22;0
WireConnection;24;0;27;0
WireConnection;24;1;21;0
WireConnection;23;0;20;0
WireConnection;26;0;24;0
WireConnection;26;2;25;0
WireConnection;26;1;23;0
WireConnection;8;0;7;2
WireConnection;8;1;13;0
WireConnection;56;0;53;0
WireConnection;56;2;54;0
WireConnection;56;1;55;0
WireConnection;48;1;56;0
WireConnection;14;0;8;0
WireConnection;17;1;26;0
WireConnection;49;0;14;0
WireConnection;49;1;17;0
WireConnection;49;2;48;0
WireConnection;19;0;14;0
WireConnection;19;1;17;0
WireConnection;58;0;19;0
WireConnection;58;1;49;0
WireConnection;32;0;58;0
WireConnection;30;0;31;0
WireConnection;30;1;32;0
WireConnection;59;0;32;0
WireConnection;59;1;60;0
WireConnection;4;0;2;2
WireConnection;80;0;30;0
WireConnection;69;0;59;0
WireConnection;69;1;4;0
WireConnection;68;0;29;0
WireConnection;68;1;80;0
WireConnection;0;2;68;0
WireConnection;0;9;69;0
ASEEND*/
//CHKSM=B0BF04850AFF1BCDA9BAF964CB452A9FB77549FF