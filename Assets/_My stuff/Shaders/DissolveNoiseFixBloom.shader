// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DissolveNoiseFixBloom"
{
    Properties
    {
        _StartPoint("StartPoint", Vector) = (0,0,0,0)
        _TimeSpread("TimeSpread", Float) = 0
        _maintexture("main texture", 2D) = "white" {}
        _noise("noise", 2D) = "white" {}
        [HDR]_burnramp("burn ramp", 2D) = "white" {}
        _Float3("Float 3", Float) = 0
        _Float4("Float 4", Float) = 0.57
        _Float5("Float 5", Float) = 0.5
        _T("T", Float) = 0
        [HDR]_Color3("Color 3", Color) = (0.8588235,0.2470588,0.08627451,0)
        [HDR]_Color4("Color 4", Color) = (5.458824,5.992157,0.6901961,0)
        _TextureSample1("Texture Sample 1", 2D) = "white" {}
        _Float7("Float 7", Float) = 1
        [HideInInspector] _texcoord( "", 2D ) = "white" {}
        [HideInInspector] __dirty( "", Int ) = 1
    }
    SubShader
    {
        Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        AlphaToMask On
        CGINCLUDE
        #include "UnityShaderVariables.cginc"
        #include "UnityPBSLighting.cginc"
        #include "Lighting.cginc"
        #pragma target 3.0
        struct Input
        {
            float2 uv_texcoord;
            float3 worldPos;
        };
        uniform sampler2D _maintexture;
        uniform float4 _maintexture_ST;
        uniform float4 _Color3;
        uniform float4 _Color4;
        uniform sampler2D _TextureSample1;
        uniform float _Float7;
        uniform sampler2D _burnramp;
        uniform float _T;
        uniform float _TimeSpread;
        uniform float3 _StartPoint;
        uniform sampler2D _noise;
        uniform float4 _noise_ST;
        uniform float _Float5;
        uniform float _Float4;
        uniform float _Float3;
        void surf( Input i , inout SurfaceOutputStandard o )
        {
            float2 uv_maintexture = i.uv_texcoord * _maintexture_ST.xy + _maintexture_ST.zw;
            o.Albedo = tex2D( _maintexture, uv_maintexture ).rgb;
            float2 temp_cast_1 = (_SinTime.x).xx;
            float2 panner83 = ( _Float7 * temp_cast_1 + i.uv_texcoord);
            float4 tex2DNode86 = tex2D( _TextureSample1, panner83 );
            float4 lerpResult90 = lerp( _Color3 , _Color4 , ( tex2DNode86 * saturate( ( abs( _SinTime.x ) + 0.24 ) ) * tex2DNode86 ));
            float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
            float3 objToWorld5 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 1 ) ).xyz;
            float clampResult9 = clamp( ( ( _T - _TimeSpread ) - distance( _StartPoint , objToWorld5 ) ) , 0.0 , 1.0 );
            float2 uv_noise = i.uv_texcoord * _noise_ST.xy + _noise_ST.zw;
            float4 temp_output_74_0 = ( 1.0 - ( (-0.6 + (( 1.0 - clampResult9 ) - 0.0) * (1.0 - -0.6) / (1.0 - 0.0)) + tex2D( _noise, uv_noise ) ) );
            float4 clampResult68 = clamp( (float4( -4,0,0,0 ) + (temp_output_74_0 - float4( 0,0,0,0 )) * (float4( 4,0,0,0 ) - float4( -4,0,0,0 )) / (float4( 1,0,0,0 ) - float4( 0,0,0,0 ))) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
            float2 appendResult59 = (float2(( 1.0 - clampResult68 ).rg));
            float4 temp_cast_3 = (_Float5).xxxx;
            float4 temp_cast_4 = (_Float4).xxxx;
            o.Emission = ( ( lerpResult90 * tex2D( _burnramp, appendResult59 ) ) * ( 1.0 - step( temp_output_74_0 , temp_cast_3 ) ) * step( temp_output_74_0 , temp_cast_4 ) ).rgb;
            float4 temp_cast_6 = (_Float3).xxxx;
            o.Alpha = step( temp_output_74_0 , temp_cast_6 ).r;
        }
        ENDCG
        CGPROGRAM
        #pragma surface surf Standard keepalpha fullforwardshadows 
        ENDCG
        Pass
        {
            Name "ShadowCaster"
            Tags{ "LightMode" = "ShadowCaster" }
            ZWrite On
            AlphaToMask Off
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
                surfIN.worldPos = worldPos;
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
102;337;1571;465;1801.997;1972.885;1.875967;True;True
Node;AmplifyShaderEditor.PosVertexDataNode;1;-2162.042,157.3297;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TransformPositionNode;5;-1956.431,146.4635;Float;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;4;-1910.04,-128.6832;Float;False;Property;_TimeSpread;TimeSpread;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;2;-1928.432,-25.66827;Float;False;Property;_StartPoint;StartPoint;0;0;Create;True;0;0;False;0;0,0,0;-0.66,0.76,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;75;-1898.19,-226.7206;Float;False;Property;_T;T;9;0;Create;True;0;0;False;0;0;1.95;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;6;-1705.596,-130.9395;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;7;-1695.471,74.94334;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;-1532.624,-3.166453;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;9;-1359.976,9.469223;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;10;-1129.823,83.89619;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;71;-830.5554,152.1471;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;56;-1030.48,366.0908;Float;True;Property;_noise;noise;4;0;Create;True;0;0;False;0;None;e28dc97a9541e3642a48c0e3886688c5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-592.0834,153.1483;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;77;-1696.686,-1853.324;Float;False;2077.146;1493.975;Comment;13;90;89;88;87;86;85;84;83;82;81;80;79;78;Edge glow variation;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;74;-435.4436,159.4589;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinTimeNode;78;-1677.252,-876.5374;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;67;-274.6955,-60.13757;Float;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;3;COLOR;-4,0,0,0;False;4;COLOR;4,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;82;-1673.686,-1160.43;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;81;-1440.152,-892.4255;Float;False;Property;_Float7;Float 7;13;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;80;-1072.108,-701.8824;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-1092.699,-474.3505;Float;False;Constant;_Float2;Float 2;14;0;Create;True;0;0;False;0;0.24;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;84;-782.6436,-765.8525;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;83;-1330.686,-1071.43;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;68;-59.34733,-100.5433;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;86;-973.3055,-1290.19;Float;True;Property;_TextureSample1;Texture Sample 1;12;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;69;205.5871,-66.26514;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;85;-599.4675,-926.3453;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;88;-378.6866,-1803.324;Float;False;Property;_Color3;Color 3;10;1;[HDR];Create;True;0;0;False;0;0.8588235,0.2470588,0.08627451,0;0.8588235,0.2470588,0.08627451,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;66;195.8854,238.8497;Float;False;Property;_Float5;Float 5;8;0;Create;True;0;0;False;0;0.5;0.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;89;-421.2996,-1633.229;Float;False;Property;_Color4;Color 4;11;1;[HDR];Create;True;0;0;False;0;5.458824,5.992157,0.6901961,0;5.458824,5.992157,0.6901961,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-253.5214,-1387.554;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;59;421.3389,8.734825;Float;False;FLOAT2;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;65;558.2374,-178.3709;Float;True;Property;_burnramp;burn ramp;5;1;[HDR];Create;True;0;0;False;0;None;97a8e753cd5d5ac4c8e96283d73a10aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;90;115.4599,-1630.761;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;62;471.8649,171.23;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;61;375.2854,635.842;Float;False;Property;_Float4;Float 4;7;0;Create;True;0;0;False;0;0.57;7.31;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;63;728.9824,179.1248;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;73;858.6928,645.1079;Float;False;Property;_Float3;Float 3;6;0;Create;True;0;0;False;0;0;0.62;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;916.5502,-265.6093;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;70;664.1461,508.0101;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;76;-1905.19,-297.7206;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;1061.949,214.3433;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;72;1077.329,483.3729;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;11;1140.849,-272.8962;Float;True;Property;_maintexture;main texture;3;0;Create;True;0;0;False;0;53ddad541c3bb26408c8a07eb0b532f6;7e7a571f42d4af34d83f41f2c7c4025f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1490.801,157.6185;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;DissolveNoiseFix;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;2;-1;-1;-1;0;True;0;0;False;-1;-1;0;True;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;1;0
WireConnection;6;0;75;0
WireConnection;6;1;4;0
WireConnection;7;0;2;0
WireConnection;7;1;5;0
WireConnection;8;0;6;0
WireConnection;8;1;7;0
WireConnection;9;0;8;0
WireConnection;10;0;9;0
WireConnection;71;0;10;0
WireConnection;58;0;71;0
WireConnection;58;1;56;0
WireConnection;74;0;58;0
WireConnection;67;0;74;0
WireConnection;80;0;78;1
WireConnection;84;0;80;0
WireConnection;84;1;79;0
WireConnection;83;0;82;0
WireConnection;83;2;78;1
WireConnection;83;1;81;0
WireConnection;68;0;67;0
WireConnection;86;1;83;0
WireConnection;69;0;68;0
WireConnection;85;0;84;0
WireConnection;87;0;86;0
WireConnection;87;1;85;0
WireConnection;87;2;86;0
WireConnection;59;0;69;0
WireConnection;65;1;59;0
WireConnection;90;0;88;0
WireConnection;90;1;89;0
WireConnection;90;2;87;0
WireConnection;62;0;74;0
WireConnection;62;1;66;0
WireConnection;63;0;62;0
WireConnection;91;0;90;0
WireConnection;91;1;65;0
WireConnection;70;0;74;0
WireConnection;70;1;61;0
WireConnection;64;0;91;0
WireConnection;64;1;63;0
WireConnection;64;2;70;0
WireConnection;72;0;74;0
WireConnection;72;1;73;0
WireConnection;0;0;11;0
WireConnection;0;2;64;0
WireConnection;0;9;72;0
ASEEND*/
//CHKSM=5B22EFDABD36EBC9B32285B1BB7A96068F2A6047