// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "shader/booster"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_cercleTexture("cercle Texture", 2D) = "white" {}
		_cercleinverseTexture("cercleinverse Texture", 2D) = "white" {}
		_CentreTexture("Centre Texture", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 0
		_cerclecolor("cercle color", Color) = (0.5080471,0,1,0)
		_cercleinversecolor("cercleinverse color", Color) = (1,0.7726722,0,0)
		_Centrecolor("Centre color", Color) = (1,0.07530873,0,0)
		_CentreSpeed("Centre Speed", Vector) = (0,-0.5,0,0)
		_cercleinverseSpeed("cercleinverse Speed", Vector) = (0,-0.2,0,0)
		_cercleSpeed("cercle Speed", Vector) = (0,-0.2,0,0)
		_clercleAlpha("clercle Alpha", Range( 0 , 1)) = 0
		_cercleinverseAlpha("cercleinverse Alpha", Range( 0 , 1)) = 1
		_CentreAlpha("Centre Alpha", Range( 0 , 1)) = 0
		_cercleIntensity("cercle Intensity", Range( 0 , 1)) = 1
		_cercleinverseIntensity("cercleinverse Intensity", Range( 0 , 1)) = 1
		_CentreIntensity("Centre Intensity", Range( 0 , 1)) = 1
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

		uniform float _cercleIntensity;
		uniform sampler2D _cercleTexture;
		uniform float2 _cercleSpeed;
		uniform float _clercleAlpha;
		uniform float4 _cerclecolor;
		uniform float _cercleinverseIntensity;
		uniform sampler2D _cercleinverseTexture;
		uniform float2 _cercleinverseSpeed;
		uniform float _cercleinverseAlpha;
		uniform float4 _cercleinversecolor;
		uniform float _CentreIntensity;
		uniform sampler2D _CentreTexture;
		uniform float2 _CentreSpeed;
		uniform float _CentreAlpha;
		uniform float4 _Centrecolor;
		uniform float _Opacity;
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
			float2 panner150 = ( 1.0 * _Time.y * _cercleSpeed + i.uv_texcoord);
			float4 tex2DNode151 = tex2D( _cercleTexture, panner150 );
			float4 lerpResult155 = lerp( tex2DNode151 , ( 1.0 - tex2DNode151 ) , _clercleAlpha);
			float2 panner177 = ( 1.0 * _Time.y * _cercleinverseSpeed + i.uv_texcoord);
			float4 tex2DNode178 = tex2D( _cercleinverseTexture, panner177 );
			float4 lerpResult181 = lerp( tex2DNode178 , ( 1.0 - tex2DNode178 ) , _cercleinverseAlpha);
			float2 panner163 = ( 1.0 * _Time.y * _CentreSpeed + i.uv_texcoord);
			float4 tex2DNode164 = tex2D( _CentreTexture, panner163 );
			float4 lerpResult167 = lerp( tex2DNode164 , ( 1.0 - tex2DNode164 ) , _CentreAlpha);
			o.Emission = ( ( ( _cercleIntensity * lerpResult155 * _cerclecolor ) + ( _cercleinverseIntensity * lerpResult181 * _cercleinversecolor ) ) + ( _CentreIntensity * lerpResult167 * _Centrecolor ) ).rgb;
			o.Alpha = 1;
			float4 utilemasck158 = lerpResult155;
			float4 centremasck168 = lerpResult167;
			float4 inversemasck182 = lerpResult181;
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen106 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither106 = Dither4x4Bayer( fmod(clipScreen106.x, 4), fmod(clipScreen106.y, 4) );
			dither106 = step( dither106, _Opacity );
			clip( ( ( utilemasck158 + centremasck168 + inversemasck182 ) * dither106 ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
1920;0;1920;1019;2013.143;720.4434;1.3;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;149;-2476.782,-1419.697;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;148;-2494.782,-1230.697;Inherit;False;Property;_cercleSpeed;cercle Speed;31;0;Create;True;0;0;0;False;0;False;0,-0.2;0,-0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;175;-2466.98,-893.1755;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;176;-2484.98,-703.1755;Inherit;False;Property;_cercleinverseSpeed;cercleinverse Speed;30;0;Create;True;0;0;0;False;0;False;0,-0.2;0,-0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;161;-2499.188,-222.0795;Inherit;False;Property;_CentreSpeed;Centre Speed;29;0;Create;True;0;0;0;False;0;False;0,-0.5;0,-0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;162;-2481.188,-411.0796;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;163;-2225.187,-347.0796;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;150;-2220.781,-1355.697;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;177;-2210.979,-829.1755;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;164;-1921.186,-363.0796;Inherit;True;Property;_CentreTexture;Centre Texture;5;0;Create;True;0;0;0;False;0;False;-1;09e0e9a212aca974ba1a93b9daead5a1;09e0e9a212aca974ba1a93b9daead5a1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;151;-1916.781,-1371.697;Inherit;True;Property;_cercleTexture;cercle Texture;3;0;Create;True;0;0;0;False;0;False;-1;775ccb06ba3056d4fb13ebca5f7a72a4;775ccb06ba3056d4fb13ebca5f7a72a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;178;-1906.979,-845.1755;Inherit;True;Property;_cercleinverseTexture;cercleinverse Texture;4;0;Create;True;0;0;0;False;0;False;-1;775ccb06ba3056d4fb13ebca5f7a72a4;775ccb06ba3056d4fb13ebca5f7a72a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;165;-1617.186,-107.0796;Inherit;False;Property;_CentreAlpha;Centre Alpha;48;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;166;-1553.186,-235.0795;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;180;-1538.979,-717.1755;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;179;-1602.979,-589.1755;Inherit;False;Property;_cercleinverseAlpha;cercleinverse Alpha;47;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;152;-1612.781,-1115.697;Inherit;False;Property;_clercleAlpha;clercle Alpha;46;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;153;-1548.781,-1243.697;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;167;-1344.187,-361.0796;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;181;-1329.98,-843.1755;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;155;-1339.781,-1369.697;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;168;-1137.187,-235.0795;Inherit;False;centremasck;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;182;-1122.98,-717.1755;Inherit;False;inversemasck;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;183;-1154.98,-909.1755;Inherit;False;Property;_cercleinverseIntensity;cercleinverse Intensity;54;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;184;-1090.98,-621.1755;Inherit;False;Property;_cercleinversecolor;cercleinverse color;22;0;Create;True;0;0;0;False;0;False;1,0.7726722,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;156;-1164.781,-1435.697;Inherit;False;Property;_cercleIntensity;cercle Intensity;53;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;158;-1132.781,-1243.697;Inherit;False;utilemasck;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;154;-1100.781,-1147.697;Inherit;False;Property;_cerclecolor;cercle color;21;0;Create;True;0;0;0;False;0;False;0.5080471,0,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;174;-677.45,60.52214;Inherit;False;168;centremasck;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;187;-685.8429,156.4068;Inherit;False;182;inversemasck;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;160;-669.5841,-43.34502;Inherit;False;158;utilemasck;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;185;-834.98,-861.1755;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-700.467,318.0002;Inherit;False;Property;_Opacity;Opacity;17;0;Create;True;0;0;0;False;0;False;0;0.75;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;157;-844.7814,-1387.697;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;169;-1105.187,-139.0794;Inherit;False;Property;_Centrecolor;Centre color;23;0;Create;True;0;0;0;False;0;False;1,0.07530873,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;170;-1169.187,-427.0796;Inherit;False;Property;_CentreIntensity;Centre Intensity;55;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;67;2360.691,-465.1809;Inherit;False;2460.671;1812.266;exterieur;37;14;15;1;16;2;3;17;52;4;18;6;19;20;50;5;21;7;8;11;22;12;23;9;13;24;68;69;70;71;72;73;74;75;76;77;78;79;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;107;-396.467,-1.99975;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;92;2361.841,-2370.195;Inherit;False;2473.551;1723.442;interieur;37;27;48;28;29;30;31;32;33;36;45;34;26;37;49;38;39;40;41;53;43;42;51;44;46;47;80;81;82;83;84;85;86;87;88;89;90;91;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;-849.1868,-379.0796;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DitheringNode;106;-396.467,254.0002;Inherit;False;0;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;189;-584.4434,-1072.093;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;32;3557.841,-2208.194;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;2421.841,-2272.195;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;104;1356.521,-722.9958;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;72;3317.224,1112.551;Inherit;False;Property;_hexexterieurcontourAlpha;hexexterieurcontour Alpha;41;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;101;1820.52,-402.9957;Inherit;False;Property;_Grandeflechecolor;Grandefleche color;25;0;Create;True;0;0;0;False;0;False;0,0.8,0.01356068,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;23;3385.962,208.8191;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;99;1756.52,-690.9958;Inherit;False;Property;_GrandeflecheIntensity;Grandefleche Intensity;59;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;46;3365.842,-1696.194;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;139;-1656.974,-2688.981;Inherit;False;Property;_cloudAlpha;cloud Alpha;49;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;15;2452.262,444.9193;Inherit;False;Property;_hexexterieurSpeed;hexexterieur Speed;37;0;Create;True;0;0;0;False;0;False;0,-0.05;0,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;118;-1132.06,-2151.611;Inherit;False;Property;_diagonal1color;diagonal1 color;24;0;Create;True;0;0;0;False;0;False;0,0.8,0.01356068,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;116;-1371.06,-2373.611;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;3733.841,-2288.195;Inherit;False;Property;_flecheinterieurintensity;flecheinterieur intensity;52;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;-875.6713,-1846.334;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;130;-1370.671,-1828.334;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;128;-1131.671,-1606.334;Inherit;False;Property;_diagonal2color;diagonal2  color;19;0;Create;True;0;0;0;False;0;False;0.8,0.1743997,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;120;-876.0602,-2391.611;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;121;-1596.059,-2471.611;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;19;3609.962,320.8192;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;83;3037.788,-1077.522;Inherit;True;Property;_HexinterieurcontourTexture;Hexinterieurcontour Texture;12;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;117;-1164.06,-2247.61;Inherit;False;utilemasck;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;147;-304.6065,-2224.33;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;134;-608.0477,-2174.715;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;110;-2508.06,-2423.611;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;135;-2520.975,-2992.981;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;28;2677.842,-2208.194;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-1643.67,-1574.334;Inherit;False;Property;_diagonal2Alpha;diagonal2  Alpha;45;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;73;3377.377,997.1712;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;34;3765.841,-2096.195;Inherit;False;LignecentralMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;129;-1195.671,-1894.334;Inherit;False;Property;_diagonal2Intensity;diagonal2  Intensity;58;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;173;-406.3802,-552.5913;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;4913.377,-648.8964;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;69;2449.377,821.1711;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;70;2721.377,869.1711;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;49;2411.841,-1456.194;Inherit;False;Property;_HexinterieurSpeed;Hexinterieur Speed;35;0;Create;True;0;0;0;False;0;False;0,-0.05;0,-0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.OneMinusNode;140;-1592.974,-2816.981;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;141;-1144.975,-2720.981;Inherit;False;Property;_cloudcolor;cloud color;20;0;Create;True;0;0;0;False;0;False;0.5080471,0,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;133;-1163.671,-1702.334;Inherit;False;utilemasck;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;142;-1383.975,-2942.981;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;143;-1208.975,-3008.981;Inherit;False;Property;_cloudIntensity;cloud Intensity;56;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;-888.975,-2960.981;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;4069.841,-1600.194;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;18;3385.962,448.8192;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;1;2423.962,-178.1808;Inherit;False;Property;_flecheexterieurSpeed;flecheexterieur Speed;33;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DesaturateOpNode;78;3377.377,757.171;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;82;2733.789,-1077.522;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;112;-2252.059,-2359.611;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;136;-2538.975,-2803.981;Inherit;False;Property;_cloudSpeed;cloud Speed;32;0;Create;True;0;0;0;False;0;False;-0.1,-0.2;0,-0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;3797.841,-1472.194;Inherit;False;HexMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;96;1004.52,-626.9957;Inherit;True;Property;_GrandeflecheTexture;Grandefleche Texture;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;123;-2525.671,-1689.334;Inherit;False;Property;_diagonal2Speed;diagonal2  Speed;27;0;Create;True;0;0;0;False;0;False;0.1,0.2;0,-0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;17;3033.962,320.8192;Inherit;True;Property;_hexexterieurtexture;hexexterieur texture;11;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;3817.962,432.8192;Inherit;False;HexMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;122;-2507.671,-1878.334;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;3765.841,-1664.194;Inherit;False;Property;_HexinterieurIntensity;Hexinterieur Intensity;64;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;13;3353.962,-415.1808;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DesaturateOpNode;26;3333.842,-2320.195;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;4357.842,-1600.194;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;132;-1595.67,-1926.334;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector2Node;111;-2526.06,-2234.61;Inherit;False;Property;_diagonal1Speed;diagonal1 Speed;28;0;Create;True;0;0;0;False;0;False;-0.1,-0.2;0,-0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;40;3013.842,-1584.194;Inherit;True;Property;_Hexinterieurtexture;Hexinterieur texture;13;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;74;3601.377,869.1711;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;80;2435.788,-949.5225;Inherit;False;Property;_HexinterieurcontourSpeed;Hexinterieurcontour Speed;36;0;Create;True;0;0;0;False;0;False;0,-0.05;0,-0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.OneMinusNode;31;3349.842,-2096.195;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;2076.52,-642.9957;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;71;3025.377,869.1711;Inherit;True;Property;_hexexterieurcontourtexture;hexexterieurcontour texture;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;39;2709.842,-1584.194;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;87;3613.789,-1077.522;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;109;4943.005,-318.592;Inherit;False;103;utilemasck;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;86;3789.788,-1157.522;Inherit;False;Property;_Hexinterieurcontourintensity;Hexinterieurcontour intensity;63;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;3785.962,240.8192;Inherit;False;Property;_hexexterieurintensity;hexexterieur intensity;62;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;51;3822.563,-1362.423;Inherit;False;Property;_Hexinterieurcolor;Hexinterieur color;15;0;Create;True;0;0;0;False;0;False;0,0.8,0.01356068,0;0.06532454,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;90;3389.789,-1189.522;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;7;3578.962,-317.1808;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;159;-1564.781,-1467.697;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;6;3369.962,-191.1808;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;88;3846.51,-855.7518;Inherit;False;Property;_Color0;Color 0;14;0;Create;True;0;0;0;False;0;False;0,0.8,0.01356068,0;1,0.01286284,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;171;-1569.186,-459.0796;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;16;2729.962,320.8192;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;75;3777.377,789.1711;Inherit;False;Property;_hexexterieurcontourintensity;hexexterieurcontour intensity;61;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;42;3589.842,-1584.194;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;126;-1579.67,-1702.334;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;4053.841,-2240.195;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;2441.962,-367.1808;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;4377.963,304.8192;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;95;700.5203,-610.9957;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;103;1788.52,-498.9957;Inherit;False;utilemasck;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;93;444.5193,-674.9958;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;50;3825.837,564.7596;Inherit;False;Property;_hexexterieurcolor;hexexterieur color;16;0;Create;True;0;0;0;False;0;False;0,0.8,0.01356068,0;1,0.909804,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;4081.377,853.1711;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;94;426.5193,-485.9957;Inherit;False;Property;_GrandeflecheSpeed;Grandefleche Speed;26;0;Create;True;0;0;0;False;0;False;0,0;0,-0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;119;-1196.06,-2439.611;Inherit;False;Property;_diagonal1Intensity;diagonal1 Intensity;57;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;4089.963,304.8192;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;76;3817.252,1113.112;Inherit;False;Property;_hexexterieurcontourcolor;hexexterieurcontour color;18;0;Create;True;0;0;0;False;0;False;0,0.8,0.01356068,0;1,0.2691505,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;186;-1554.979,-941.1755;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;114;-1580.059,-2247.61;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;3305.962,-63.18086;Inherit;False;Property;_flecheexterieurAlpha;flecheexterieur Alpha;44;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;137;-2264.974,-2928.981;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;4585.963,-335.1808;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;146;-1608.974,-3040.981;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;41;3365.842,-1456.194;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;4073.962,-335.1808;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;124;-2251.67,-1814.334;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;125;-1947.67,-1830.334;Inherit;True;Property;_diagonal2Texture;diagonal2 Texture;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;113;-1948.059,-2375.611;Inherit;True;Property;_diagonal1Texture;diagonal1 Texture;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;115;-1644.059,-2119.611;Inherit;False;Property;_diagonal1Alpha;diagonal1 Alpha;50;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;84;3389.789,-949.5225;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;85;3351.752,-826.3077;Inherit;False;Property;_HexinterieurcontourAlpha;Hexinterieurcontour Alpha;43;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;79;3809.377,981.1712;Inherit;False;HexMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;138;-1960.974,-2944.981;Inherit;True;Property;_cloudTexture;cloud Texture;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;3;2697.962,-303.1808;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;38;2437.841,-1632.194;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;29;2981.842,-2224.195;Inherit;True;Property;_flecheinterieurtexture;flecheinterieur texture;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;53;3327.804,-1332.979;Inherit;False;Property;_hexinterieurAlpha;hexinterieur Alpha;42;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;1308.521,-370.9956;Inherit;False;Property;_GrandeflecheAlpha;Grandefleche Alpha;51;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;4599.993,-2020.112;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;81;2461.789,-1125.522;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;3001.962,-319.1808;Inherit;True;Property;_flecheexterieurtexture;flecheexterieur texture;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;91;3821.788,-965.5224;Inherit;False;HexMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;145;-1176.975,-2816.981;Inherit;False;utilemasck;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;68;2443.677,993.2712;Inherit;False;Property;_hexexterieurcontourSpeed;hexexterieurcontour Speed;38;0;Create;True;0;0;0;False;0;False;0,-0.05;0,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;2457.962,272.8192;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;4093.789,-1093.522;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;3753.962,-383.1808;Inherit;False;Property;_flecheexterieurintensity;flecheexterieur intensity;60;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;97;1372.521,-498.9957;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;100;1581.52,-624.9957;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;48;2453.841,-2080.194;Inherit;False;Property;_flecheinterieurSpeed;flecheinterieur Speed;34;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-220.467,62.00023;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;52;3325.809,564.1992;Inherit;False;Property;_hexexterieurAlpha;hexexterieur Alpha;40;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;3285.842,-1968.194;Inherit;False;Property;_flecheinterieurAlpha;flecheinterieur Alpha;39;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;3785.962,-191.1808;Inherit;False;LignecentralMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-45.86392,-700.6274;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;shader/booster;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;163;0;162;0
WireConnection;163;2;161;0
WireConnection;150;0;149;0
WireConnection;150;2;148;0
WireConnection;177;0;175;0
WireConnection;177;2;176;0
WireConnection;164;1;163;0
WireConnection;151;1;150;0
WireConnection;178;1;177;0
WireConnection;166;0;164;0
WireConnection;180;0;178;0
WireConnection;153;0;151;0
WireConnection;167;0;164;0
WireConnection;167;1;166;0
WireConnection;167;2;165;0
WireConnection;181;0;178;0
WireConnection;181;1;180;0
WireConnection;181;2;179;0
WireConnection;155;0;151;0
WireConnection;155;1;153;0
WireConnection;155;2;152;0
WireConnection;168;0;167;0
WireConnection;182;0;181;0
WireConnection;158;0;155;0
WireConnection;185;0;183;0
WireConnection;185;1;181;0
WireConnection;185;2;184;0
WireConnection;157;0;156;0
WireConnection;157;1;155;0
WireConnection;157;2;154;0
WireConnection;107;0;160;0
WireConnection;107;1;174;0
WireConnection;107;2;187;0
WireConnection;172;0;170;0
WireConnection;172;1;167;0
WireConnection;172;2;169;0
WireConnection;106;0;105;0
WireConnection;189;0;157;0
WireConnection;189;1;185;0
WireConnection;32;0;29;0
WireConnection;32;1;31;0
WireConnection;32;2;30;0
WireConnection;104;0;96;0
WireConnection;23;0;17;0
WireConnection;46;0;40;0
WireConnection;116;0;113;0
WireConnection;116;1;114;0
WireConnection;116;2;115;0
WireConnection;131;0;129;0
WireConnection;131;1;130;0
WireConnection;131;2;128;0
WireConnection;130;0;125;0
WireConnection;130;1;126;0
WireConnection;130;2;127;0
WireConnection;120;0;119;0
WireConnection;120;1;116;0
WireConnection;120;2;118;0
WireConnection;121;0;113;0
WireConnection;19;0;17;0
WireConnection;19;1;18;0
WireConnection;19;2;52;0
WireConnection;83;1;82;0
WireConnection;117;0;116;0
WireConnection;147;0;144;0
WireConnection;147;1;134;0
WireConnection;134;0;120;0
WireConnection;134;1;131;0
WireConnection;28;0;27;0
WireConnection;28;2;48;0
WireConnection;73;0;71;0
WireConnection;34;0;32;0
WireConnection;173;0;189;0
WireConnection;173;1;172;0
WireConnection;54;0;37;0
WireConnection;54;1;12;0
WireConnection;54;2;102;0
WireConnection;70;0;69;0
WireConnection;70;2;68;0
WireConnection;140;0;138;0
WireConnection;133;0;130;0
WireConnection;142;0;138;0
WireConnection;142;1;140;0
WireConnection;142;2;139;0
WireConnection;144;0;143;0
WireConnection;144;1;142;0
WireConnection;144;2;141;0
WireConnection;44;0;43;0
WireConnection;44;1;42;0
WireConnection;44;2;51;0
WireConnection;18;0;17;0
WireConnection;78;0;71;0
WireConnection;82;0;81;0
WireConnection;82;2;80;0
WireConnection;112;0;110;0
WireConnection;112;2;111;0
WireConnection;47;0;42;0
WireConnection;96;1;95;0
WireConnection;17;1;16;0
WireConnection;24;0;19;0
WireConnection;13;0;4;0
WireConnection;26;0;29;0
WireConnection;45;0;44;0
WireConnection;45;1;89;0
WireConnection;132;0;125;0
WireConnection;40;1;39;0
WireConnection;74;0;71;0
WireConnection;74;1;73;0
WireConnection;74;2;72;0
WireConnection;31;0;29;0
WireConnection;102;0;99;0
WireConnection;102;1;100;0
WireConnection;102;2;101;0
WireConnection;71;1;70;0
WireConnection;39;0;38;0
WireConnection;39;2;49;0
WireConnection;87;0;83;0
WireConnection;87;1;84;0
WireConnection;87;2;85;0
WireConnection;90;0;83;0
WireConnection;7;0;4;0
WireConnection;7;1;6;0
WireConnection;7;2;5;0
WireConnection;159;0;151;0
WireConnection;6;0;4;0
WireConnection;171;0;164;0
WireConnection;16;0;14;0
WireConnection;16;2;15;0
WireConnection;42;0;40;0
WireConnection;42;1;41;0
WireConnection;42;2;53;0
WireConnection;126;0;125;0
WireConnection;36;0;33;0
WireConnection;36;1;32;0
WireConnection;22;0;21;0
WireConnection;22;1;77;0
WireConnection;95;0;93;0
WireConnection;95;2;94;0
WireConnection;103;0;100;0
WireConnection;77;0;75;0
WireConnection;77;1;74;0
WireConnection;77;2;76;0
WireConnection;21;0;20;0
WireConnection;21;1;19;0
WireConnection;21;2;50;0
WireConnection;186;0;178;0
WireConnection;114;0;113;0
WireConnection;137;0;135;0
WireConnection;137;2;136;0
WireConnection;12;0;11;0
WireConnection;12;1;22;0
WireConnection;146;0;138;0
WireConnection;41;0;40;0
WireConnection;11;0;8;0
WireConnection;11;1;7;0
WireConnection;124;0;122;0
WireConnection;124;2;123;0
WireConnection;125;1;124;0
WireConnection;113;1;112;0
WireConnection;84;0;83;0
WireConnection;79;0;74;0
WireConnection;138;1;137;0
WireConnection;3;0;2;0
WireConnection;3;2;1;0
WireConnection;29;1;28;0
WireConnection;37;0;36;0
WireConnection;37;1;45;0
WireConnection;4;1;3;0
WireConnection;91;0;87;0
WireConnection;145;0;142;0
WireConnection;89;0;86;0
WireConnection;89;1;87;0
WireConnection;89;2;88;0
WireConnection;97;0;96;0
WireConnection;100;0;96;0
WireConnection;100;1;97;0
WireConnection;100;2;98;0
WireConnection;108;0;107;0
WireConnection;108;1;106;0
WireConnection;9;0;7;0
WireConnection;0;2;173;0
WireConnection;0;10;108;0
ASEEND*/
//CHKSM=7E8E300563DD01E8F62F121D79F779495C260B33