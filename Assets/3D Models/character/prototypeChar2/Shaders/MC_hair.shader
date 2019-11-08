// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MC_hair"
{
	Properties
	{
		_NormalMap("NormalMap", 2D) = "bump" {}
		_MainTexture("MainTexture", 2D) = "white" {}
		_FireTexA("FireTex A", 2D) = "white" {}
		_FireTexB("FireTex B", 2D) = "white" {}
		_FireSpeedB("FireSpeed B", Range( -1 , 1)) = 0
		_V("V", Range( -1 , 1)) = 0
		_U("U", Range( -1 , 1)) = 0
		_FireSpeedA("FireSpeed A", Range( -1 , 1)) = 0
		_RedGlowTextureB("RedGlowTexture B", 2D) = "white" {}
		_RedGlowTextureA("RedGlowTexture A", 2D) = "white" {}
		_TextureSample20("Texture Sample 20", 2D) = "white" {}
		_RedGlowAmount("RedGlow Amount", Range( 0 , 1)) = 0
		[HDR]_FireColorB("FireColor B", Color) = (0,0,0,0)
		[HDR]_FireColorA("FireColor A", Color) = (1.502837,0.161228,0,0)
		_GlowAmount("GlowAmount", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;
		uniform sampler2D _RedGlowTextureB;
		uniform float4 _RedGlowTextureB_ST;
		uniform sampler2D _TextureSample20;
		uniform sampler2D _RedGlowTextureA;
		uniform float _U;
		uniform float _V;
		uniform float _RedGlowAmount;
		uniform float4 _FireColorB;
		uniform float4 _FireColorA;
		uniform sampler2D _FireTexB;
		uniform float _FireSpeedA;
		uniform float _FireSpeedB;
		uniform sampler2D _FireTexA;
		uniform float _GlowAmount;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.normal = BlendNormals( float3( 1,1,1 ) , float3( 1,1,1 ) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			o.Normal = UnpackNormal( tex2D( _NormalMap, uv_NormalMap ) );
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float4 color154 = IsGammaSpace() ? float4(0.1226415,0.1226415,0.1226415,0) : float4(0.01390275,0.01390275,0.01390275,0);
			o.Albedo = ( tex2D( _MainTexture, uv_MainTexture ) * color154 ).rgb;
			float2 uv_RedGlowTextureB = i.uv_texcoord * _RedGlowTextureB_ST.xy + _RedGlowTextureB_ST.zw;
			float4 tex2DNode133 = tex2D( _RedGlowTextureB, uv_RedGlowTextureB );
			float4 color144 = IsGammaSpace() ? float4(1,0.2454576,0,0) : float4(1,0.04907652,0,0);
			float mulTime121 = _Time.y * 0.1;
			float4 appendResult125 = (float4(_U , _V , 0.0 , 0.0));
			float2 panner129 = ( mulTime121 * appendResult125.xy + i.uv_texcoord);
			float4 appendResult103 = (float4(_FireSpeedA , _FireSpeedB , 0.0 , 0.0));
			float2 panner107 = ( _Time.y * appendResult103.xy + i.uv_texcoord);
			float4 tex2DNode110 = tex2D( _FireTexB, panner107 );
			float4 myVarName130 = ( tex2DNode110 + ( tex2D( _FireTexA, panner107 ) * tex2DNode110 ) );
			float4 lerpResult164 = lerp( _FireColorB , _FireColorA , myVarName130);
			float4 lerpResult162 = lerp( ( tex2DNode133.r * color144 * tex2D( _TextureSample20, ( tex2DNode133 * saturate( ( abs( _SinTime.x ) + 0.2 ) ) * tex2D( _RedGlowTextureA, panner129 ) ).rg ) * _RedGlowAmount ) , lerpResult164 , _GlowAmount);
			o.Emission = lerpResult162.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17000
7;313;1906;699;-1518.282;-668.6249;2.403046;True;True
Node;AmplifyShaderEditor.RangedFloatNode;100;1189.57,2310.389;Float;False;Property;_FireSpeedB;FireSpeed B;4;0;Create;True;0;0;False;0;0;-0.2;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;1231.495,2234.469;Float;False;Property;_FireSpeedA;FireSpeed A;7;0;Create;True;0;0;False;0;0;-0.1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;101;1478.517,2113.225;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;103;1506.469,2240.602;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleTimeNode;104;1690.599,2331.771;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;107;1893.705,2108.7;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;112;1886.842,1211.635;Float;False;Constant;_Float19;Float 19;22;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;116;1551.944,989.9119;Float;False;Property;_U;U;6;0;Create;True;0;0;False;0;0;0.1910588;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;1510.019,1065.832;Float;False;Property;_V;V;5;0;Create;True;0;0;False;0;0;0.3560084;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;113;2150.176,1283.112;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;118;1798.966,868.6669;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;110;2130.71,2101.123;Float;True;Property;_FireTexB;FireTex B;3;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;111;2129.853,1878.728;Float;True;Property;_FireTexA;FireTex A;2;0;Create;True;0;0;False;0;None;9fbef4b79ca3b784ba023cb1331520d5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;121;2013.527,1050.035;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;125;1826.918,996.0448;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.AbsOpNode;119;2349.966,1249.292;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;122;2323.61,1392.266;Float;False;Constant;_Float23;Float 23;14;0;Create;True;0;0;False;0;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;2481.35,2091.44;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;126;2507.21,1253.166;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;129;2106.418,924.9311;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;124;2765.612,1995.955;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;131;2557.375,1120.105;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;133;2442.004,576.4261;Float;True;Property;_RedGlowTextureB;RedGlowTexture B;8;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;136;2285.951,865.0214;Float;True;Property;_RedGlowTextureA;RedGlowTexture A;9;0;Create;True;0;0;False;0;None;61c0b9c0523734e0e91bc6043c72a490;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;130;3228.437,1992.569;Float;False;myVarName;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;2729.343,927.4785;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;134;3636.228,1533.524;Float;False;Property;_FireColorA;FireColor A;13;1;[HDR];Create;True;0;0;False;0;1.502837,0.161228,0,0;2.118547,0.3492284,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;139;3813.632,1356.808;Float;False;Property;_FireColorB;FireColor B;12;1;[HDR];Create;True;0;0;False;0;0,0,0,0;2.588278,1.964923,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;147;2973.906,1011.972;Float;True;Property;_TextureSample20;Texture Sample 20;10;0;Create;True;0;0;False;0;None;c2f70a58c1100504ca5b09fc2c27dcec;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;149;3252.406,734.17;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;148;3032.945,1225.356;Float;False;Property;_RedGlowAmount;RedGlow Amount;11;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;144;2997.636,789.7859;Float;False;Constant;_Color8;Color 8;19;0;Create;True;0;0;False;0;1,0.2454576,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;135;3712.294,1850.752;Float;False;130;myVarName;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;164;4137.387,1522.021;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;155;4211.556,816.0399;Float;True;Property;_MainTexture;MainTexture;1;0;Create;True;0;0;False;0;None;7130c16fd8005b546b111d341310a9a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;161;4251.949,1789.625;Float;False;Property;_GlowAmount;GlowAmount;14;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;150;3402.126,1080.256;Float;True;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;154;4109.946,1036.561;Float;False;Constant;_MainTextureTint;MainTextureTint;16;0;Create;True;0;0;False;0;0.1226415,0.1226415,0.1226415,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;158;4428.802,1231.784;Float;True;Property;_NormalMap;NormalMap;0;0;Create;True;0;0;False;0;None;7ddcba51d9fc0894d98b4ba77fbdfbd7;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;162;4565.631,1467.179;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;156;4496.798,998.3014;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendNormalsNode;159;4695.868,1794.946;Float;False;0;3;0;FLOAT3;1,1,1;False;1;FLOAT3;1,1,1;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;5060.01,1342.008;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MC_hair;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;103;0;98;0
WireConnection;103;1;100;0
WireConnection;107;0;101;0
WireConnection;107;2;103;0
WireConnection;107;1;104;0
WireConnection;110;1;107;0
WireConnection;111;1;107;0
WireConnection;121;0;112;0
WireConnection;125;0;116;0
WireConnection;125;1;114;0
WireConnection;119;0;113;1
WireConnection;115;0;111;0
WireConnection;115;1;110;0
WireConnection;126;0;119;0
WireConnection;126;1;122;0
WireConnection;129;0;118;0
WireConnection;129;2;125;0
WireConnection;129;1;121;0
WireConnection;124;0;110;0
WireConnection;124;1;115;0
WireConnection;131;0;126;0
WireConnection;136;1;129;0
WireConnection;130;0;124;0
WireConnection;142;0;133;0
WireConnection;142;1;131;0
WireConnection;142;2;136;0
WireConnection;147;1;142;0
WireConnection;149;0;133;1
WireConnection;164;0;139;0
WireConnection;164;1;134;0
WireConnection;164;2;135;0
WireConnection;150;0;149;0
WireConnection;150;1;144;0
WireConnection;150;2;147;0
WireConnection;150;3;148;0
WireConnection;162;0;150;0
WireConnection;162;1;164;0
WireConnection;162;2;161;0
WireConnection;156;0;155;0
WireConnection;156;1;154;0
WireConnection;0;0;156;0
WireConnection;0;1;158;0
WireConnection;0;2;162;0
WireConnection;0;12;159;0
ASEEND*/
//CHKSM=A92B71B9BE76CC5D8DA83D480254888025A3E4B9