// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MC_shader"
{
	Properties
	{
		_DisolveGuide("Disolve Guide", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_BurnRamp("Burn Ramp", 2D) = "white" {}
		_DissolveAmount("Dissolve Amount", Range( 0 , 1)) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_FlameWave("Flame Wave", 2D) = "white" {}
		_FlameNoise("Flame Noise", 2D) = "white" {}
		_v("v", Range( -1 , 1)) = 0
		_Float3("Float 3", Range( -1 , 1)) = 0
		_Float4("Float 4", Range( -1 , 1)) = 0
		_U("U", Range( -1 , 1)) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_RedGlowAmount("Red Glow Amount", Range( 0 , 1)) = 0
		[HDR]_BodyFireColor("BodyFireColor", Color) = (1.502837,0.161228,0,0)
		[HDR]_Burnedgeboost("Burn edge boost", Color) = (4,2.596858,1.759162,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _TextureSample3;
		uniform sampler2D _TextureSample2;
		uniform float _Float4;
		uniform float _Float3;
		uniform float _RedGlowAmount;
		uniform float _DissolveAmount;
		uniform sampler2D _DisolveGuide;
		uniform float4 _DisolveGuide_ST;
		uniform sampler2D _FlameWave;
		uniform float _U;
		uniform float _v;
		uniform sampler2D _FlameNoise;
		uniform float4 _BodyFireColor;
		uniform sampler2D _BurnRamp;
		uniform float4 _Burnedgeboost;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.normal = BlendNormals( float3( 1,1,1 ) , float3( 1,1,1 ) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 color153 = IsGammaSpace() ? float4(0.1226415,0.1226415,0.1226415,0) : float4(0.01390275,0.01390275,0.01390275,0);
			o.Albedo = ( tex2D( _TextureSample0, uv_TextureSample0 ) * color153 ).rgb;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 tex2DNode112 = tex2D( _TextureSample1, uv_TextureSample1 );
			float4 color114 = IsGammaSpace() ? float4(1,0.2454576,0,0) : float4(1,0.04907652,0,0);
			float mulTime121 = _Time.y * 0.1;
			float4 appendResult120 = (float4(_Float4 , _Float3 , 0.0 , 0.0));
			float2 panner123 = ( mulTime121 * appendResult120.xy + i.uv_texcoord);
			float2 uv_DisolveGuide = i.uv_texcoord * _DisolveGuide_ST.xy + _DisolveGuide_ST.zw;
			float4 temp_output_6_0 = ( (-0.6 + (( 1.0 - _DissolveAmount ) - 0.0) * (0.6 - -0.6) / (1.0 - 0.0)) + tex2D( _DisolveGuide, uv_DisolveGuide ) );
			float4 temp_cast_3 = (0.5).xxxx;
			float4 temp_output_13_0 = step( temp_output_6_0 , temp_cast_3 );
			float4 appendResult72 = (float4(_U , _v , 0.0 , 0.0));
			float2 panner75 = ( _Time.y * appendResult72.xy + i.uv_texcoord);
			float4 tex2DNode85 = tex2D( _FlameNoise, panner75 );
			float4 FireSurface212 = ( ( tex2D( _FlameWave, panner75 ) * tex2DNode85 ) + tex2DNode85 );
			float4 temp_cast_5 = (0.5).xxxx;
			float4 temp_output_15_0 = ( 1.0 - temp_output_13_0 );
			float4 clampResult9 = clamp( (float4( -4,0,0,0 ) + (temp_output_6_0 - float4( 0,0,0,0 )) * (float4( 4,0,0,0 ) - float4( -4,0,0,0 )) / (float4( 1,0,0,0 ) - float4( 0,0,0,0 ))) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			float2 appendResult12 = (float2(( 1.0 - clampResult9 ).rg));
			float4 temp_cast_7 = (0.57).xxxx;
			o.Emission = ( ( ( tex2DNode112.r * color114 * tex2D( _TextureSample3, ( tex2DNode112 * saturate( ( abs( _SinTime.x ) + 0.2 ) ) * tex2D( _TextureSample2, panner123 ) ).rg ) * _RedGlowAmount ) * temp_output_13_0 ) + ( ( FireSurface212 * _BodyFireColor * temp_output_15_0 ) + ( tex2D( _BurnRamp, appendResult12 ) * temp_output_15_0 * step( temp_output_6_0 , temp_cast_7 ) * _Burnedgeboost ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17000
86;516;1624;503;1466.746;243.4573;1.948115;True;True
Node;AmplifyShaderEditor.CommentaryNode;1;-1842.172,598.5189;Float;False;908.2314;498.3652;Dissolve - Opacity Mask;5;6;5;4;3;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1808.84,700.7853;Float;False;Property;_DissolveAmount;Dissolve Amount;3;0;Create;True;0;0;False;0;0;0.52;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;3;-1531.815,692.3022;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-3232.7,249.0911;Float;False;Property;_U;U;10;0;Create;True;0;0;False;0;0;-0.1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-3274.625,325.0111;Float;False;Property;_v;v;7;0;Create;True;0;0;False;0;0;-0.2;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;73;-2985.678,127.8462;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;72;-2957.726,255.2241;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleTimeNode;117;-2773.596,346.3922;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;4;-1374.326,672.345;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-1432.358,887.3851;Float;True;Property;_DisolveGuide;Disolve Guide;0;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;119;-1802.593,-515.6145;Float;False;Property;_Float4;Float 4;9;0;Create;True;0;0;False;0;0;0.1669402;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-1147.186,656.645;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinTimeNode;139;-1204.361,-222.4145;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;124;-1467.695,-293.8915;Float;False;Constant;_Float5;Float 5;22;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-1844.518,-439.6946;Float;False;Property;_Float3;Float 3;8;0;Create;True;0;0;False;0;0;0.1913025;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;7;-977.5665,-14.38372;Float;False;814.5701;432.0292;Burn Effect - Emission;4;17;12;10;8;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;75;-2570.49,123.3222;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;85;-2333.485,115.7442;Float;True;Property;_FlameNoise;Flame Noise;6;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;122;-1555.571,-636.8595;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;8;-964.3685,233.2329;Float;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;3;COLOR;-4,0,0,0;False;4;COLOR;4,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;82;-2334.342,-106.6501;Float;True;Property;_FlameWave;Flame Wave;5;0;Create;True;0;0;False;0;None;9fbef4b79ca3b784ba023cb1331520d5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;121;-1341.01,-455.4911;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;144;-1031.927,-113.26;Float;False;Constant;_Float2;Float 2;14;0;Create;True;0;0;False;0;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;120;-1527.619,-509.4816;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.AbsOpNode;140;-1004.571,-256.2348;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;9;-923.5516,-25.62808;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;143;-847.3268,-252.36;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;123;-1248.119,-580.5953;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-1982.052,-116.0127;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;148;-1733.688,91.11067;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;10;-671.7026,12.34628;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-544.2012,618.2152;Float;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;115;-1068.586,-640.505;Float;True;Property;_TextureSample2;Texture Sample 2;12;0;Create;True;0;0;False;0;None;61c0b9c0523734e0e91bc6043c72a490;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;145;-797.1626,-385.4211;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;112;-912.5327,-929.1002;Float;True;Property;_TextureSample1;Texture Sample 1;11;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;13;-304.3761,488.3986;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;212;-1471.249,102.0054;Float;False;FireSurface;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-282.9419,966.5969;Float;False;Constant;_Float1;Float 1;6;0;Create;True;0;0;False;0;0.57;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;12;-488.5331,65.31564;Float;False;FLOAT2;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;-625.1939,-578.0479;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;17;-429.7819,201.9584;Float;True;Property;_BurnRamp;Burn Ramp;2;0;Create;True;0;0;False;0;None;64e7766099ad46747a07014e44d0aea1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;16;-70.18076,770.4976;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;15;22.12578,421.7216;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-242.5918,-263.1702;Float;False;Property;_RedGlowAmount;Red Glow Amount;14;0;Create;True;0;0;False;0;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;216;276.5439,743.3907;Float;False;Property;_Burnedgeboost;Burn edge boost;16;1;[HDR];Create;True;0;0;False;0;4,2.596858,1.759162,0;1.835294,1.07451,0.6196079,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;114;-356.9008,-715.7404;Float;False;Constant;_Color2;Color 2;19;0;Create;True;0;0;False;0;1,0.2454576,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;213;102.5522,49.42317;Float;False;212;FireSurface;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;128;-380.6313,-493.5544;Float;True;Property;_TextureSample3;Texture Sample 3;13;0;Create;True;0;0;False;0;None;c2f70a58c1100504ca5b09fc2c27dcec;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;109;100.7178,137.7493;Float;False;Property;_BodyFireColor;BodyFireColor;15;1;[HDR];Create;True;0;0;False;0;1.502837,0.161228,0,0;2.118547,0.2772968,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;214;-102.1314,-771.3564;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;402.2777,107.2248;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;428.6737,400.2337;Float;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;113;47.58916,-425.2705;Float;True;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;215;48.50505,-40.50905;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;499.6003,-198.9144;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;110;752.5463,186.9576;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;153;755.4092,-468.9651;Float;False;Constant;_Color3;Color 3;16;0;Create;True;0;0;False;0;0.1226415,0.1226415,0.1226415,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;23;759.1098,-622.2942;Float;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;None;7130c16fd8005b546b111d341310a9a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;111;1228.514,400.1401;Float;False;0;3;0;FLOAT3;1,1,1;False;1;FLOAT3;1,1,1;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;19;872.2645,-245.7423;Float;True;Property;_Normal;Normal;1;0;Create;True;0;0;False;0;None;7ddcba51d9fc0894d98b4ba77fbdfbd7;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;129;1107.425,95.79452;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;151;1142.261,-507.225;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1542.108,-5.500892;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MC_shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;72;0;69;0
WireConnection;72;1;68;0
WireConnection;4;0;3;0
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;75;0;73;0
WireConnection;75;2;72;0
WireConnection;75;1;117;0
WireConnection;85;1;75;0
WireConnection;8;0;6;0
WireConnection;82;1;75;0
WireConnection;121;0;124;0
WireConnection;120;0;119;0
WireConnection;120;1;118;0
WireConnection;140;0;139;1
WireConnection;9;0;8;0
WireConnection;143;0;140;0
WireConnection;143;1;144;0
WireConnection;123;0;122;0
WireConnection;123;2;120;0
WireConnection;123;1;121;0
WireConnection;91;0;82;0
WireConnection;91;1;85;0
WireConnection;148;0;91;0
WireConnection;148;1;85;0
WireConnection;10;0;9;0
WireConnection;115;1;123;0
WireConnection;145;0;143;0
WireConnection;13;0;6;0
WireConnection;13;1;11;0
WireConnection;212;0;148;0
WireConnection;12;0;10;0
WireConnection;125;0;112;0
WireConnection;125;1;145;0
WireConnection;125;2;115;0
WireConnection;17;1;12;0
WireConnection;16;0;6;0
WireConnection;16;1;14;0
WireConnection;15;0;13;0
WireConnection;128;1;125;0
WireConnection;214;0;112;1
WireConnection;22;0;213;0
WireConnection;22;1;109;0
WireConnection;22;2;15;0
WireConnection;21;0;17;0
WireConnection;21;1;15;0
WireConnection;21;2;16;0
WireConnection;21;3;216;0
WireConnection;113;0;214;0
WireConnection;113;1;114;0
WireConnection;113;2;128;0
WireConnection;113;3;150;0
WireConnection;215;0;13;0
WireConnection;24;0;113;0
WireConnection;24;1;215;0
WireConnection;110;0;22;0
WireConnection;110;1;21;0
WireConnection;129;0;24;0
WireConnection;129;1;110;0
WireConnection;151;0;23;0
WireConnection;151;1;153;0
WireConnection;0;0;151;0
WireConnection;0;1;19;0
WireConnection;0;2;129;0
WireConnection;0;12;111;0
ASEEND*/
//CHKSM=4F596660AA1A52BE58DD3C354F6843E89D7456B7