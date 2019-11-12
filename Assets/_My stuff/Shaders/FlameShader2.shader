// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/test"
{
	Properties
	{
		_Color2("Color 2", Color) = (1,0.6465492,0.2216981,0)
		_Color1("Color 1", Color) = (0.764151,0.3049861,0.0756942,0)
		_Color0("Color 0", Color) = (0,0.2846522,1,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_NoiseScale("NoiseScale", Float) = -0.05
		_FlameFlickerSpeed("FlameFlickerSpeed", Float) = 0.2
		_timeScale("timeScale", Float) = -1
		_WorldPosMultiplier("WorldPosMultiplier", Float) = 0.001
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		Blend One One , One OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustomLighting keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
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

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _Noise;
		uniform float _timeScale;
		uniform float _FlameFlickerSpeed;
		uniform float _WorldPosMultiplier;
		uniform float _NoiseScale;
		uniform float4 _Noise_ST;
		uniform float4 _Color2;
		uniform float4 _Color1;
		uniform float4 _Color0;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			//Calculate new billboard vertex position and normal;
			float3 upCamVec = float3( 0, 1, 0 );
			float3 forwardCamVec = -normalize ( UNITY_MATRIX_V._m20_m21_m22 );
			float3 rightCamVec = normalize( UNITY_MATRIX_V._m00_m01_m02 );
			float4x4 rotationCamMatrix = float4x4( rightCamVec, 0, upCamVec, 0, forwardCamVec, 0, 0, 0, 0, 1 );
			v.normal = normalize( mul( float4( v.normal , 0 ), rotationCamMatrix )).xyz;
			//This unfortunately must be made to take non-uniform scaling into account;
			//Transform to world coords, apply rotation and transform back to local;
			v.vertex = mul( v.vertex , unity_ObjectToWorld );
			v.vertex = mul( v.vertex , rotationCamMatrix );
			v.vertex = mul( v.vertex , unity_WorldToObject );
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			c.rgb = 0;
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
			float2 uv0_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float mulTime38 = _Time.y * _timeScale;
			float4 appendResult23 = (float4(-_FlameFlickerSpeed , -_FlameFlickerSpeed , 0.0 , 0.0));
			float3 ase_worldPos = i.worldPos;
			float4 appendResult44 = (float4(ase_worldPos.x , ase_worldPos.y , 0.0 , 0.0));
			float2 uv0_Noise = i.uv_texcoord * _Noise_ST.xy + _Noise_ST.zw;
			float2 panner20 = ( mulTime38 * appendResult23.xy + ( ( appendResult44 * _WorldPosMultiplier ) + float4( ( _NoiseScale * uv0_Noise ), 0.0 , 0.0 ) ).xy);
			float4 tex2DNode3 = tex2D( _TextureSample0, ( uv0_TextureSample0 + ( uv0_TextureSample0.y * ( (tex2D( _Noise, panner20 )).rg - float2( 0.5,0.5 ) ) * uv0_TextureSample0.y ) ) );
			o.Emission = ( 2.0 * ( ( tex2DNode3.r * _Color2 ) + ( tex2DNode3.g * _Color1 ) + ( tex2DNode3.b * _Color0 ) ) ).rgb;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17000
95;232;1513;582;4274.056;730.0274;3.717993;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;43;-1885.262,-409.1432;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;44;-1650.839,-370.0727;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1734.608,-93.66541;Float;False;Property;_NoiseScale;NoiseScale;6;0;Create;True;0;0;False;0;-0.05;-0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1660.263,-217.5954;Float;False;Property;_WorldPosMultiplier;WorldPosMultiplier;9;0;Create;True;0;0;False;0;0.001;0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1974.766,199.0989;Float;False;Property;_FlameFlickerSpeed;FlameFlickerSpeed;7;0;Create;True;0;0;False;0;0.2;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-1809.553,49.96489;Float;False;0;21;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-1718,415.1518;Float;False;Property;_timeScale;timeScale;8;0;Create;True;0;0;False;0;-1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;22;-1741.918,238.3517;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-1526.861,63.25041;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1435.263,-274.5953;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-1438.102,-96.06319;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleTimeNode;38;-1563.284,405.9516;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;23;-1591.722,211.2811;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;20;-1355,62.08879;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;21;-1205.138,166.3568;Float;True;Property;_Noise;Noise;5;0;Create;True;0;0;False;0;ba8de44547793e14cbb75f66c36d544b;ba8de44547793e14cbb75f66c36d544b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;36;-892.9981,353.6995;Float;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;-1073.718,-155.5374;Float;True;0;3;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;37;-647.5576,318.8474;Float;False;2;0;FLOAT2;-0.5,-0.5;False;1;FLOAT2;0.5,0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-675.5768,106.3247;Float;False;3;3;0;FLOAT;0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-570.8335,23.93;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;2;-74.27785,-195.4001;Float;False;Property;_Color1;Color 1;2;0;Create;True;0;0;False;0;0.764151,0.3049861,0.0756942,0;0.764151,0.3049861,0.0756942,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;13.33334,-478.8585;Float;False;Property;_Color2;Color 2;1;0;Create;True;0;0;False;0;1,0.6465492,0.2216981,0;1,0.6465492,0.2216981,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-391.1185,2.903976;Float;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;6d3f48118dadfdf4b861917cc85afe71;6d3f48118dadfdf4b861917cc85afe71;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-177.3985,286.713;Float;False;Property;_Color0;Color 0;3;0;Create;True;0;0;False;0;0,0.2846522,1,0;0,0.2846522,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;186.9524,-66.39952;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;113.5903,266.5477;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;273.2016,-372.6024;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;453.1346,26.88597;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;588.2261,-140.9422;Float;False;Constant;_FlameBrightness;FlameBrightness;6;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;698.5867,-14.25181;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;957.8095,-132.9171;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Hidden/test;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;3;1;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;True;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;44;0;43;1
WireConnection;44;1;43;2
WireConnection;22;0;24;0
WireConnection;40;0;39;0
WireConnection;40;1;19;0
WireConnection;46;0;44;0
WireConnection;46;1;45;0
WireConnection;47;0;46;0
WireConnection;47;1;40;0
WireConnection;38;0;41;0
WireConnection;23;0;22;0
WireConnection;23;1;22;0
WireConnection;20;0;47;0
WireConnection;20;2;23;0
WireConnection;20;1;38;0
WireConnection;21;1;20;0
WireConnection;36;0;21;0
WireConnection;37;0;36;0
WireConnection;42;0;30;2
WireConnection;42;1;37;0
WireConnection;42;2;30;2
WireConnection;31;0;30;0
WireConnection;31;1;42;0
WireConnection;3;1;31;0
WireConnection;6;0;3;2
WireConnection;6;1;2;0
WireConnection;16;0;3;3
WireConnection;16;1;1;0
WireConnection;7;0;3;1
WireConnection;7;1;4;0
WireConnection;8;0;7;0
WireConnection;8;1;6;0
WireConnection;8;2;16;0
WireConnection;14;0;25;0
WireConnection;14;1;8;0
WireConnection;0;2;14;0
ASEEND*/
//CHKSM=DDF8D880ADC08C4BCD78F42BFCB93B6DA51CF48D