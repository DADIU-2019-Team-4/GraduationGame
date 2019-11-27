// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DissolveNoiseOpague"
{
	Properties
	{
		_StartPoint("StartPoint", Vector) = (1,1,0,0)
		_TimeSpread("TimeSpread", Float) = 0
		_maintexture("main texture", 2D) = "white" {}
		_noise("noise", 2D) = "white" {}
		[HDR]_burnramp("burn ramp", 2D) = "white" {}
		_BurntTexture("BurntTexture", 2D) = "white" {}
		_Float3("Float 3", Float) = 0
		_Float4("Float 4", Float) = 0.57
		_Float5("Float 5", Float) = 0.5
		_T("T", Float) = 1.82
		[HDR]_DissolveBoost("DissolveBoost", Color) = (4,3.32549,0.7843137,0)
		_Redglownoise("Red glow noise", 2D) = "white" {}
		_Redglownoiseb("Red glow noise b", 2D) = "white" {}
		_RedGlowramp("RedGlow ramp", 2D) = "white" {}
		_RedGlowAmount("Red Glow Amount", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _BurntTexture;
		uniform float4 _BurntTexture_ST;
		uniform float _T;
		uniform float _TimeSpread;
		uniform float3 _StartPoint;
		uniform sampler2D _noise;
		uniform float4 _noise_ST;
		uniform float _Float5;
		uniform sampler2D _maintexture;
		uniform float4 _maintexture_ST;
		uniform float _Float3;
		uniform sampler2D _Redglownoise;
		uniform float4 _Redglownoise_ST;
		uniform sampler2D _RedGlowramp;
		uniform sampler2D _Redglownoiseb;
		uniform float4 _Redglownoiseb_ST;
		uniform float _RedGlowAmount;
		uniform sampler2D _burnramp;
		uniform float _Float4;
		uniform float4 _DissolveBoost;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BurntTexture = i.uv_texcoord * _BurntTexture_ST.xy + _BurntTexture_ST.zw;
			float4 color57 = IsGammaSpace() ? float4(0.1226415,0.1226415,0.1226415,0) : float4(0.01390275,0.01390275,0.01390275,0);
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 objToWorld2 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
			float clampResult9 = clamp( ( ( _T - _TimeSpread ) - distance( _StartPoint , objToWorld2 ) ) , 0.0 , 1.0 );
			float2 uv_noise = i.uv_texcoord * _noise_ST.xy + _noise_ST.zw;
			float4 temp_output_14_0 = ( 1.0 - ( (-0.6 + (( 1.0 - clampResult9 ) - 0.0) * (1.0 - -0.6) / (1.0 - 0.0)) + tex2D( _noise, uv_noise ) ) );
			float4 temp_cast_0 = (_Float5).xxxx;
			float4 temp_output_24_0 = ( 1.0 - step( temp_output_14_0 , temp_cast_0 ) );
			float2 uv_maintexture = i.uv_texcoord * _maintexture_ST.xy + _maintexture_ST.zw;
			float4 temp_cast_1 = (_Float3).xxxx;
			o.Albedo = ( ( ( tex2D( _BurntTexture, uv_BurntTexture ) * color57 ) * temp_output_24_0 ) + ( tex2D( _maintexture, uv_maintexture ) * step( temp_output_14_0 , temp_cast_1 ) ) ).rgb;
			float2 uv_Redglownoise = i.uv_texcoord * _Redglownoise_ST.xy + _Redglownoise_ST.zw;
			float4 tex2DNode49 = tex2D( _Redglownoise, uv_Redglownoise );
			float4 color55 = IsGammaSpace() ? float4(1,0.2454576,0,0) : float4(1,0.04907652,0,0);
			float2 uv_Redglownoiseb = i.uv_texcoord * _Redglownoiseb_ST.xy + _Redglownoiseb_ST.zw;
			float4 clampResult16 = clamp( (float4( -4,0,0,0 ) + (temp_output_14_0 - float4( 0,0,0,0 )) * (float4( 4,0,0,0 ) - float4( -4,0,0,0 )) / (float4( 1,0,0,0 ) - float4( 0,0,0,0 ))) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			float2 appendResult20 = (float2(( 1.0 - clampResult16 ).rg));
			float4 temp_cast_5 = (_Float4).xxxx;
			o.Emission = ( ( ( tex2DNode49.r * color55 * tex2D( _RedGlowramp, ( tex2DNode49 * saturate( ( abs( _SinTime.x ) + 0.24 ) ) * tex2D( _Redglownoiseb, uv_Redglownoiseb ) ).rg ) * _RedGlowAmount ) * temp_output_24_0 ) + ( tex2D( _burnramp, appendResult20 ) * temp_output_24_0 * step( temp_output_14_0 , temp_cast_5 ) * _DissolveBoost ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17000
7;134;1899;587;3958.573;921.6664;1.3;True;True
Node;AmplifyShaderEditor.PosVertexDataNode;1;-3488.058,-412.6789;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;4;-3254.448,-595.6769;Float;False;Property;_StartPoint;StartPoint;0;0;Create;True;0;0;False;0;1,1,0;1,1,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TransformPositionNode;2;-3282.447,-423.5451;Float;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;3;-3236.056,-698.6918;Float;False;Property;_TimeSpread;TimeSpread;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-3224.206,-796.7292;Float;False;Property;_T;T;9;0;Create;True;0;0;False;0;1.82;2.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;6;-3031.612,-700.9481;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;7;-3021.487,-495.0653;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;-2858.64,-573.175;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;9;-2685.992,-560.5394;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;10;-2455.839,-486.1124;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;12;-2156.571,-417.8615;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-2356.496,-203.9178;Float;True;Property;_noise;noise;3;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-1918.099,-416.8603;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinTimeNode;37;-2088.847,-1137.759;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;-1916.413,-1028.605;Float;False;Constant;_Float6;Float 6;14;0;Create;True;0;0;False;0;0.24;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;44;-1889.057,-1171.579;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;14;-1797.76,-413.1426;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-1731.813,-1167.705;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;15;-1554.039,-321.5891;Float;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;3;COLOR;-4,0,0,0;False;4;COLOR;4,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;16;-1349.062,-167.5259;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;49;-1797.019,-1844.445;Float;True;Property;_Redglownoise;Red glow noise;11;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;47;-1953.072,-1557.632;Float;True;Property;_Redglownoiseb;Red glow noise b;12;0;Create;True;0;0;False;0;None;61c0b9c0523734e0e91bc6043c72a490;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;48;-1681.648,-1300.766;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;18;-1123.022,-185.1062;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1332.378,-359.681;Float;False;Property;_Float5;Float 5;8;0;Create;True;0;0;False;0;0.5;0.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-1509.68,-1493.393;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1128.866,-1174.938;Float;False;Property;_RedGlowAmount;Red Glow Amount;14;0;Create;True;0;0;False;0;0;0.178;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1090.834,-621.0489;Float;False;Property;_Float3;Float 3;6;0;Create;True;0;0;False;0;0;0.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;63;-1518.994,27.88396;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;57;-760.503,-1578.614;Float;False;Constant;_BurntColor;BurntColor;16;0;Create;True;0;0;False;0;0.1226415,0.1226415,0.1226415,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;58;-412.2272,-1710.329;Float;True;Property;_BurntTexture;BurntTexture;5;0;Create;True;0;0;False;0;None;7130c16fd8005b546b111d341310a9a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;51;-887.1651,-1634.713;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-870.35,257.7092;Float;False;Property;_Float4;Float 4;7;0;Create;True;0;0;False;0;0.57;0.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;19;-1082.328,-473.9733;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;20;-896.8983,-146.4071;Float;False;FLOAT2;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;55;-1241.387,-1631.085;Float;False;Constant;_Color2;Color 2;19;0;Create;True;0;0;False;0;1,0.2454576,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;54;-1265.117,-1408.899;Float;True;Property;_RedGlowramp;RedGlow ramp;13;0;Create;True;0;0;False;0;None;c2f70a58c1100504ca5b09fc2c27dcec;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;26;-847.7342,-1020.037;Float;True;Property;_maintexture;main texture;2;0;Create;True;0;0;False;0;53ddad541c3bb26408c8a07eb0b532f6;53ddad541c3bb26408c8a07eb0b532f6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;27;-900.4329,-814.6554;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;24;-869.2896,-494.6005;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;22;-524.2949,132.8994;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-836.8967,-1340.615;Float;True;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-60.07718,-1338.169;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;35;-235.2487,77.07078;Float;False;Property;_DissolveBoost;DissolveBoost;10;1;[HDR];Create;True;0;0;False;0;4,3.32549,0.7843137,0;4,3.32549,0.7843137,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;23;-639.6039,-145.1721;Float;True;Property;_burnramp;burn ramp;4;1;[HDR];Create;True;0;0;False;0;None;97a8e753cd5d5ac4c8e96283d73a10aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-106.3011,-535.7267;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;101.4788,-829.7966;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-20.94604,-287.3277;Float;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-366.0452,-1023.167;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;254.8415,-407.3656;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;347.3107,-687.1572;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;29;-3231.206,-867.7292;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;638.9773,-585.5476;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;DissolveNoiseOpague;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;6;0;5;0
WireConnection;6;1;3;0
WireConnection;7;0;4;0
WireConnection;7;1;2;0
WireConnection;8;0;6;0
WireConnection;8;1;7;0
WireConnection;9;0;8;0
WireConnection;10;0;9;0
WireConnection;12;0;10;0
WireConnection;13;0;12;0
WireConnection;13;1;11;0
WireConnection;44;0;37;1
WireConnection;14;0;13;0
WireConnection;45;0;44;0
WireConnection;45;1;42;0
WireConnection;15;0;14;0
WireConnection;16;0;15;0
WireConnection;48;0;45;0
WireConnection;18;0;16;0
WireConnection;50;0;49;0
WireConnection;50;1;48;0
WireConnection;50;2;47;0
WireConnection;63;0;14;0
WireConnection;51;0;49;1
WireConnection;19;0;14;0
WireConnection;19;1;17;0
WireConnection;20;0;18;0
WireConnection;54;1;50;0
WireConnection;27;0;14;0
WireConnection;27;1;25;0
WireConnection;24;0;19;0
WireConnection;22;0;63;0
WireConnection;22;1;21;0
WireConnection;52;0;51;0
WireConnection;52;1;55;0
WireConnection;52;2;54;0
WireConnection;52;3;53;0
WireConnection;59;0;58;0
WireConnection;59;1;57;0
WireConnection;23;1;20;0
WireConnection;31;0;26;0
WireConnection;31;1;27;0
WireConnection;32;0;59;0
WireConnection;32;1;24;0
WireConnection;28;0;23;0
WireConnection;28;1;24;0
WireConnection;28;2;22;0
WireConnection;28;3;35;0
WireConnection;61;0;52;0
WireConnection;61;1;24;0
WireConnection;62;0;61;0
WireConnection;62;1;28;0
WireConnection;30;0;32;0
WireConnection;30;1;31;0
WireConnection;0;0;30;0
WireConnection;0;2;62;0
ASEEND*/
//CHKSM=C4F1F8BCA649AE0F1EAD7CE10E37B783E30732F0