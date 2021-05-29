// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "finishlinepart2"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_doublelignetexture("doubleligne texture", 2D) = "white" {}
		_Textetexture("Texte texture", 2D) = "white" {}
		_doublelignecolor("doubleligne color", Color) = (1,1,1,0)
		_Textecolor("Texte color", Color) = (1,1,1,0)
		_doubleligneSpeed("doubleligne Speed", Vector) = (0.05,0,0,0)
		_TexteSpeed("Texte Speed", Vector) = (0,0,0,0)
		_doubleligneAlpha("doubleligne Alpha", Range( 0 , 1)) = 1
		_TexteAlpha("Texte Alpha", Range( 0 , 1)) = 0
		_doubleligneintensity("doubleligne intensity", Range( 0 , 1)) = 1
		_Texteintensity("Texte intensity", Range( 0 , 1)) = 1
		_Opacity1("Opacity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPosition;
		};

		uniform float _doubleligneintensity;
		uniform sampler2D _doublelignetexture;
		uniform float2 _doubleligneSpeed;
		uniform float _doubleligneAlpha;
		uniform float4 _doublelignecolor;
		uniform float _Texteintensity;
		uniform sampler2D _Textetexture;
		uniform float2 _TexteSpeed;
		uniform float _TexteAlpha;
		uniform float4 _Textecolor;
		uniform float _Opacity1;
		uniform float _Cutoff = 0.5;


		inline float Dither4x4Bayer( int x, int y )
		{
			const float dither[ 16 ] = {
				 1,  9,  3, 11,
				13,  5, 15,  7,
				 4, 12,  2, 10,
				16,  8, 14,  6 };
			int r = y * 4 + x;
			return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner3 = ( 1.0 * _Time.y * _doubleligneSpeed + i.uv_texcoord);
			float4 tex2DNode4 = tex2D( _doublelignetexture, panner3 );
			float4 lerpResult8 = lerp( tex2DNode4 , ( 1.0 - tex2DNode4 ) , _doubleligneAlpha);
			float2 panner15 = ( 1.0 * _Time.y * _TexteSpeed + i.uv_texcoord);
			float4 tex2DNode16 = tex2D( _Textetexture, panner15 );
			float4 lerpResult20 = lerp( tex2DNode16 , ( 1.0 - tex2DNode16 ) , _TexteAlpha);
			o.Emission = ( ( _doubleligneintensity * lerpResult8 * _doublelignecolor ) * ( _Texteintensity * lerpResult20 * _Textecolor ) ).rgb;
			o.Alpha = 1;
			float4 LignecentralMask11 = lerpResult8;
			float4 texteMask23 = lerpResult20;
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen28 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither28 = Dither4x4Bayer( fmod(clipScreen28.x, 4), fmod(clipScreen28.y, 4) );
			dither28 = step( dither28, _Opacity1 );
			clip( ( ( LignecentralMask11 + texteMask23 ) * dither28 ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
1059.2;365.6;1452;1035;1405.058;-280.493;1;True;False
Node;AmplifyShaderEditor.Vector2Node;1;-2325.746,-6.049368;Inherit;False;Property;_doubleligneSpeed;doubleligne Speed;5;0;Create;True;0;0;0;False;0;False;0.05,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-2357.746,-198.0488;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-2340.888,366.5524;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;13;-2308.888,558.5521;Inherit;False;Property;_TexteSpeed;Texte Speed;6;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;3;-2101.747,-134.0493;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;15;-2084.889,430.5519;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;16;-1780.889,414.552;Inherit;True;Property;_Textetexture;Texte texture;2;0;Create;True;0;0;0;False;0;False;-1;d01dfb8f94e78c847b88e7dcfea646e7;8a1ad9d9d5520c844ae26f1cc3406558;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-1797.747,-150.0492;Inherit;True;Property;_doublelignetexture;doubleligne texture;1;0;Create;True;0;0;0;False;0;False;-1;5bcf36b0d9cc8ac49a9a2f2091060bc6;8a1ad9d9d5520c844ae26f1cc3406558;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-1493.747,105.9506;Inherit;False;Property;_doubleligneAlpha;doubleligne Alpha;7;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;18;-1412.888,542.5521;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;6;-1429.746,-22.04936;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1476.889,670.552;Inherit;False;Property;_TexteAlpha;Texte Alpha;8;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;8;-1221.746,-134.0493;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;20;-1204.888,430.5519;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-996.8879,542.5521;Inherit;False;texteMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;-1013.746,-22.04936;Inherit;False;LignecentralMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-397.426,828.2753;Inherit;False;Property;_Opacity1;Opacity;11;0;Create;True;0;0;0;False;0;False;0;0.75;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;-981.7458,73.95064;Inherit;False;Property;_doublelignecolor;doubleligne color;3;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;26;-333.426,444.2753;Inherit;False;11;LignecentralMask;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1045.746,-214.0478;Inherit;False;Property;_doubleligneintensity;doubleligne intensity;9;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1028.888,350.5534;Inherit;False;Property;_Texteintensity;Texte intensity;10;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;-964.8879,638.5521;Inherit;False;Property;_Textecolor;Texte color;4;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;34;-326.0581,526.493;Inherit;False;23;texteMask;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-708.8877,398.552;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-725.7457,-166.0492;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DitheringNode;28;-93.42615,764.2754;Inherit;False;0;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-93.42615,508.2753;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-330.1102,94.31827;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;24;-1428.888,318.5534;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;82.57367,572.2753;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;12;-1445.746,-246.0478;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;247.1355,43.44049;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;finishlinepart2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;False;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;3;2;1;0
WireConnection;15;0;14;0
WireConnection;15;2;13;0
WireConnection;16;1;15;0
WireConnection;4;1;3;0
WireConnection;18;0;16;0
WireConnection;6;0;4;0
WireConnection;8;0;4;0
WireConnection;8;1;6;0
WireConnection;8;2;5;0
WireConnection;20;0;16;0
WireConnection;20;1;18;0
WireConnection;20;2;17;0
WireConnection;23;0;20;0
WireConnection;11;0;8;0
WireConnection;22;0;19;0
WireConnection;22;1;20;0
WireConnection;22;2;21;0
WireConnection;10;0;7;0
WireConnection;10;1;8;0
WireConnection;10;2;9;0
WireConnection;28;0;27;0
WireConnection;29;0;26;0
WireConnection;29;1;34;0
WireConnection;25;0;10;0
WireConnection;25;1;22;0
WireConnection;24;0;16;0
WireConnection;30;0;29;0
WireConnection;30;1;28;0
WireConnection;12;0;4;0
WireConnection;0;2;25;0
WireConnection;0;10;30;0
ASEEND*/
//CHKSM=CAF9C77E7CBEFA8C417AA7F6C6156150E0E25F5F