// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MossShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_Alpha("Alpha", 2D) = "white" {}
		_AlphaPower("Alpha Power", Range( 0.001 , 3)) = 1.691979
		_AlphaPowerMultiplier("AlphaPowerMultiplier", Range( 0.001 , 5)) = 1.691979
		_1Diffuse("1Diffuse", 2D) = "white" {}
		_2Diffuse("2Diffuse", 2D) = "white" {}
		_1Normal("1Normal", 2D) = "bump" {}
		_2Normal("2Normal", 2D) = "bump" {}
		_NormalGlobalIntensity("NormalGlobalIntensity", Range( 0 , 1)) = 0
		_1Rough("1Rough", 2D) = "white" {}
		_2Rough("2Rough", 2D) = "white" {}
		_Roughtness("Roughtness", Range( 0 , 1)) = 0
		_1AO("1AO", 2D) = "white" {}
		_2AO("2AO", 2D) = "white" {}
		_1NormalExtra("1NormalExtra", 2D) = "bump" {}
		_1NormalExtraIntensity("1NormalExtraIntensity", Range( 0.001 , 5)) = 1.691979
		_2NormalExtra("2NormalExtra", 2D) = "bump" {}
		_2NormalExtraIntensity("2NormalExtraIntensity", Range( 0.001 , 5)) = 1.691979
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma only_renderers d3d11 glcore gles gles3 d3d11_9x 
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _1Normal;
		uniform float4 _1Normal_ST;
		uniform sampler2D _1NormalExtra;
		uniform float4 _1NormalExtra_ST;
		uniform float _1NormalExtraIntensity;
		uniform sampler2D _2Normal;
		uniform float4 _2Normal_ST;
		uniform sampler2D _2NormalExtra;
		uniform float4 _2NormalExtra_ST;
		uniform float _2NormalExtraIntensity;
		uniform sampler2D _Alpha;
		uniform float4 _Alpha_ST;
		uniform float _AlphaPower;
		uniform float _AlphaPowerMultiplier;
		uniform float _NormalGlobalIntensity;
		uniform sampler2D _1Diffuse;
		uniform float4 _1Diffuse_ST;
		uniform sampler2D _2Diffuse;
		uniform float4 _2Diffuse_ST;
		uniform sampler2D _1Rough;
		uniform float4 _1Rough_ST;
		uniform sampler2D _2Rough;
		uniform float4 _2Rough_ST;
		uniform float _Roughtness;
		uniform sampler2D _1AO;
		uniform float4 _1AO_ST;
		uniform sampler2D _2AO;
		uniform float4 _2AO_ST;
		uniform float _MaskClipValue = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_1Normal = i.uv_texcoord * _1Normal_ST.xy + _1Normal_ST.zw;
			float2 uv_1NormalExtra = i.uv_texcoord * _1NormalExtra_ST.xy + _1NormalExtra_ST.zw;
			float2 uv_2Normal = i.uv_texcoord * _2Normal_ST.xy + _2Normal_ST.zw;
			float2 uv_2NormalExtra = i.uv_texcoord * _2NormalExtra_ST.xy + _2NormalExtra_ST.zw;
			float2 uv_Alpha = i.uv_texcoord * _Alpha_ST.xy + _Alpha_ST.zw;
			float4 temp_cast_0 = (( _AlphaPower * _AlphaPowerMultiplier )).xxxx;
			float4 temp_output_74_0 = pow( tex2D( _Alpha, uv_Alpha ) , temp_cast_0 );
			float3 lerpResult82 = lerp( ( UnpackNormal( tex2D( _1Normal, uv_1Normal ) ) + ( UnpackNormal( tex2D( _1NormalExtra, uv_1NormalExtra ) ) * _1NormalExtraIntensity ) ) , ( UnpackNormal( tex2D( _2Normal, uv_2Normal ) ) + ( UnpackNormal( tex2D( _2NormalExtra, uv_2NormalExtra ) ) * _2NormalExtraIntensity ) ) , temp_output_74_0.x);
			float4 lerpResult107 = lerp( float4(0,0,1,0) , float4( lerpResult82 , 0.0 ) , _NormalGlobalIntensity);
			o.Normal = lerpResult107.rgb;
			float2 uv_1Diffuse = i.uv_texcoord * _1Diffuse_ST.xy + _1Diffuse_ST.zw;
			float4 tex2DNode1 = tex2D( _1Diffuse, uv_1Diffuse );
			float2 uv_2Diffuse = i.uv_texcoord * _2Diffuse_ST.xy + _2Diffuse_ST.zw;
			float4 lerpResult5 = lerp( ( tex2DNode1 * float4(1,1,1,1) ) , ( tex2D( _2Diffuse, uv_2Diffuse ) * float4(1,1,1,1) ) , temp_output_74_0.x);
			o.Albedo = lerpResult5.xyz;
			float2 uv_1Rough = i.uv_texcoord * _1Rough_ST.xy + _1Rough_ST.zw;
			float4 tex2DNode58 = tex2D( _1Rough, uv_1Rough );
			o.Metallic = tex2DNode58.x;
			float2 uv_2Rough = i.uv_texcoord * _2Rough_ST.xy + _2Rough_ST.zw;
			float lerpResult39 = lerp( tex2DNode58.a , tex2D( _2Rough, uv_2Rough ).a , temp_output_74_0.x);
			o.Smoothness = ( lerpResult39 * _Roughtness );
			float2 uv_1AO = i.uv_texcoord * _1AO_ST.xy + _1AO_ST.zw;
			float2 uv_2AO = i.uv_texcoord * _2AO_ST.xy + _2AO_ST.zw;
			float4 lerpResult34 = lerp( tex2D( _1AO, uv_1AO ) , tex2D( _2AO, uv_2AO ) , temp_output_74_0.x);
			o.Occlusion = lerpResult34.x;
			o.Alpha = 1;
			clip( tex2DNode1.a - _MaskClipValue );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
289;92;1715;919;2247.22;703.3309;1.716493;True;True
Node;AmplifyShaderEditor.SamplerNode;89;-2082.056,404.2909;Float;True;Property;_2NormalExtra;2NormalExtra;16;0;Assets/RenanRemaster/Materiais/Grass_01/Bitmap2Material_3_Normal.tga;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;90;-2076.308,-148.4707;Float;True;Property;_1NormalExtra;1NormalExtra;14;0;Assets/RenanRemaster/Materiais/Cliff_01/Bitmap2Material_3_Normal.tga;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;114;-2078.766,94.73918;Float;False;Property;_1NormalExtraIntensity;1NormalExtraIntensity;15;0;1.691979;0.001;5;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;75;-1289.713,18.33764;Float;False;Property;_AlphaPower;Alpha Power;2;0;1.691979;0.001;3;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;116;-1292.805,99.37545;Float;False;Property;_AlphaPowerMultiplier;AlphaPowerMultiplier;3;0;1.691979;0.001;5;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;112;-2064.774,623.1454;Float;False;Property;_2NormalExtraIntensity;2NormalExtraIntensity;17;0;1.691979;0.001;5;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;15;-2105.777,188.6273;Float;True;Property;_2Normal;2Normal;7;0;Assets/RenanRemaster/Materiais/Grass_01/Bitmap2Material_3_Normal.tga;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;14;-2076.405,-372.7889;Float;True;Property;_1Normal;1Normal;6;0;Assets/RenanRemaster/Materiais/Cliff_01/Bitmap2Material_3_Normal.tga;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;113;-1748.239,-15.95049;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;-957.4032,-7.224527;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;8;-1291.339,-182.014;Float;True;Property;_Alpha;Alpha;1;0;Assets/RenanRemaster/Materiais/Mud_01/Sem Título-1.tif;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;-1700.425,363.9254;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;3;-1574.656,-373.4412;Float;True;Property;_2Diffuse;2Diffuse;5;0;Assets/RenanRemaster/Materiais/Grass_01/Bitmap2Material_3_Base_Color2.tif;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;69;-1540.892,-173.8235;Float;False;Constant;_Color2;Color 2;11;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;57;-1523.56,1089.933;Float;True;Property;_2Rough;2Rough;10;0;Assets/RenanRemaster/Materiais/Grass_01/Sem Título-1.tif;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;58;-1531.604,872.7589;Float;True;Property;_1Rough;1Rough;9;0;Assets/RenanRemaster/Materiais/Cliff_01/Sem Título-1.tif;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;87;-1454.626,82.2851;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.PowerNode;74;-833.7984,-118.7312;Float;False;2;0;FLOAT4;0.0;False;1;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;1;-1571.82,-782.8744;Float;True;Property;_1Diffuse;1Diffuse;4;0;Assets/RenanRemaster/Materiais/Cliff_01/Bitmap2Material_3_Base_Color.tga;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;88;-1445.192,235.8055;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ColorNode;60;-1531.351,-582.3751;Float;False;Constant;_Color0;Color 0;11;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;36;-1556.194,462.2228;Float;True;Property;_1AO;1AO;12;0;Assets/RenanRemaster/Materiais/Cliff_01/Bitmap2Material_3_Ambient_Occlusion.tga;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;100;-764.4512,772.6699;Float;False;Property;_Roughtness;Roughtness;11;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;35;-1542.404,676.9525;Float;True;Property;_2AO;2AO;13;0;Assets/RenanRemaster/Materiais/Grass_01/Bitmap2Material_3_Ambient_Occlusion.tga;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;39;-944.8418,689.6008;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-840.8257,-365.7565;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;102;-525.0267,83.40812;Float;False;Property;_NormalGlobalIntensity;NormalGlobalIntensity;8;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;105;-464.4068,-153.1207;Float;False;Constant;_Color3;Color 3;18;0;0,0,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-837.2534,-493.0517;Float;False;2;2;0;FLOAT4;0.0;False;1;COLOR;0.0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.LerpOp;82;-680.6121,35.99886;Float;False;3;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.LerpOp;34;-956.8595,510.4291;Float;False;3;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-683.8508,576.3705;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;107;-227.4983,-15.38751;Float;False;3;0;COLOR;0.0;False;1;FLOAT3;0.0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;5;-483.1542,-336.0097;Float;False;3;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;42.68978,1.310221;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;MossShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;TransparentCutout;Geometry;All;False;True;True;True;True;False;True;False;False;False;False;False;False;True;True;True;True;False;0;255;255;0;0;0;0;False;0;10;0;10;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;113;0;90;0
WireConnection;113;1;114;0
WireConnection;115;0;75;0
WireConnection;115;1;116;0
WireConnection;111;0;89;0
WireConnection;111;1;112;0
WireConnection;87;0;14;0
WireConnection;87;1;113;0
WireConnection;74;0;8;0
WireConnection;74;1;115;0
WireConnection;88;0;15;0
WireConnection;88;1;111;0
WireConnection;39;0;58;4
WireConnection;39;1;57;4
WireConnection;39;2;74;0
WireConnection;70;0;3;0
WireConnection;70;1;69;0
WireConnection;59;0;1;0
WireConnection;59;1;60;0
WireConnection;82;0;87;0
WireConnection;82;1;88;0
WireConnection;82;2;74;0
WireConnection;34;0;36;0
WireConnection;34;1;35;0
WireConnection;34;2;74;0
WireConnection;99;0;39;0
WireConnection;99;1;100;0
WireConnection;107;0;105;0
WireConnection;107;1;82;0
WireConnection;107;2;102;0
WireConnection;5;0;59;0
WireConnection;5;1;70;0
WireConnection;5;2;74;0
WireConnection;0;0;5;0
WireConnection;0;1;107;0
WireConnection;0;3;58;0
WireConnection;0;4;99;0
WireConnection;0;5;34;0
WireConnection;0;10;1;4
ASEEND*/
//CHKSM=3717E123DA151FC45AA44C78B287ED24268656DF