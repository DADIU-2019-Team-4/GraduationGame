// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RotateTexture"
{
	Properties
	{
		_Maintexture("Main texture", 2D) = "white" {}
		_Normals("Normals", 2D) = "white" {}
		_Rotation("Rotation", Float) = 90
		_Metallicness("Metallicness", Float) = 0
		_Smoothness("Smoothness", Float) = 0
		_Texturetiling("Texture tiling", Vector) = (1,1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normals;
		uniform float2 _Texturetiling;
		uniform float _Rotation;
		uniform sampler2D _Maintexture;
		uniform float _Metallicness;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord6 = i.uv_texcoord * _Texturetiling;
			float cos2 = cos( radians( _Rotation ) );
			float sin2 = sin( radians( _Rotation ) );
			float2 rotator2 = mul( uv_TexCoord6 - float2( 0.5,0.5 ) , float2x2( cos2 , -sin2 , sin2 , cos2 )) + float2( 0.5,0.5 );
			o.Normal = tex2D( _Normals, rotator2 ).rgb;
			o.Albedo = tex2D( _Maintexture, rotator2 ).rgb;
			o.Metallic = _Metallicness;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17000
7;448;1899;553;1630.597;225.3512;1.037147;True;True
Node;AmplifyShaderEditor.RangedFloatNode;9;-1031.5,233;Float;False;Property;_Rotation;Rotation;2;0;Create;True;0;0;False;0;90;45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;13;-1269.715,-76.03557;Float;False;Property;_Texturetiling;Texture tiling;5;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RadiansOpNode;8;-868.5,216;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1092.5,-89;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;7;-1056.394,73.81723;Float;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RotatorNode;2;-755.5,-32;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-461.5,-207;Float;True;Property;_Maintexture;Main texture;0;0;Create;True;0;0;False;0;84508b93f15f2b64386ec07486afc7a3;84508b93f15f2b64386ec07486afc7a3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-471.5,-9;Float;True;Property;_Normals;Normals;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-194.5,171;Float;False;Property;_Smoothness;Smoothness;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-208.5,111;Float;False;Property;_Metallicness;Metallicness;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;RotateTexture;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;9;0
WireConnection;6;0;13;0
WireConnection;2;0;6;0
WireConnection;2;1;7;0
WireConnection;2;2;8;0
WireConnection;1;1;2;0
WireConnection;3;1;2;0
WireConnection;0;0;1;0
WireConnection;0;1;3;0
WireConnection;0;3;4;0
WireConnection;0;4;5;0
ASEEND*/
//CHKSM=63EA0D403A803BB65C9438ADC7C7466A15ED9BD4