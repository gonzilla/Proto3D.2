// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader/mur"
{
	Properties
	{
		_lignestexture("lignes texture", 2D) = "white" {}
		_Hextexture("Hex texture", 2D) = "white" {}
		_cloudtexture("cloud texture", 2D) = "white" {}
		_Transparancelignes("Transparance lignes", Range( 0 , 1)) = 0.5027746
		_lignecolor("ligne color", Color) = (1,0.05316135,0,0)
		_hexcolor("hex color", Color) = (0,0.8,0.01356068,0)
		_Timer("Timer", Range( 0 , 5)) = 5
		_lignesSpeed("lignes Speed", Vector) = (0.05,0,0,0)
		_HexSpeed("Hex Speed", Vector) = (0.05,0,0,0)
		_cloudSpeed("cloud Speed", Vector) = (0.05,0,0,0)
		_HexAlpha("Hex Alpha", Range( 0 , 1)) = 0
		_cloudAlpha("cloud Alpha", Range( 0 , 1)) = 0
		_cloudcolor("cloud color", Color) = (0.8018868,0,0,0)
		_hexintensity("hex intensity", Range( 0 , 1)) = 1
		_cloudintensity("cloud intensity", Range( 0 , 1)) = 1
		_Bias("Bias", Float) = 0
		_Scale("Scale", Float) = 0
		_Power("Power", Range( 0 , 1)) = 0
		_Frenselintensity("Frensel intensity", Range( 0 , 1)) = 0
		_invertFresnselintensity("invert Fresnsel intensity", Range( 0 , 1)) = 0
		_Opacity("Opacity", Range( 0 , 1)) = 0
		_Frenselcolor("Frensel color", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Background+0" "IsEmissive" = "true"  }
		Cull Back
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
			float3 worldNormal;
			float4 screenPosition;
		};

		uniform float4 _lignecolor;
		uniform sampler2D _lignestexture;
		uniform float2 _lignesSpeed;
		uniform float _Transparancelignes;
		uniform float _hexintensity;
		uniform sampler2D _Hextexture;
		uniform float2 _HexSpeed;
		uniform float _HexAlpha;
		uniform float4 _hexcolor;
		uniform float _cloudintensity;
		uniform sampler2D _cloudtexture;
		uniform float2 _cloudSpeed;
		uniform float _cloudAlpha;
		uniform float4 _cloudcolor;
		uniform float _Frenselintensity;
		uniform float _Bias;
		uniform float _Scale;
		uniform float _Power;
		uniform float _invertFresnselintensity;
		uniform float4 _Frenselcolor;
		uniform float _Timer;
		uniform float _Opacity;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


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
			float2 panner96 = ( 1.0 * _Time.y * _lignesSpeed + i.uv_texcoord);
			float4 temp_output_2_0 = saturate( tex2D( _lignestexture, panner96 ) );
			float4 lerpResult3 = lerp( temp_output_2_0 , ( 1.0 - temp_output_2_0 ) , _Transparancelignes);
			float4 Varlignes23 = ( _lignecolor * lerpResult3 );
			float2 panner30 = ( 1.0 * _Time.y * _HexSpeed + i.uv_texcoord);
			float4 tex2DNode33 = tex2D( _Hextexture, panner30 );
			float4 lerpResult36 = lerp( tex2DNode33 , ( 1.0 - tex2DNode33 ) , _HexAlpha);
			float4 varhex43 = ( _hexintensity * lerpResult36 * _hexcolor );
			float2 panner47 = ( 1.0 * _Time.y * _cloudSpeed + i.uv_texcoord);
			float4 tex2DNode48 = tex2D( _cloudtexture, panner47 );
			float4 lerpResult51 = lerp( tex2DNode48 , ( 1.0 - tex2DNode48 ) , _cloudAlpha);
			float4 varcloud57 = ( _cloudintensity * lerpResult51 * _cloudcolor );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV60 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode60 = ( _Bias + _Scale * pow( 1.0 - fresnelNdotV60, _Power ) );
			float clampResult72 = clamp( ( ( _Frenselintensity * fresnelNode60 ) + ( _invertFresnselintensity * ( 1.0 - fresnelNode60 ) ) ) , 0.0 , 1.0 );
			float frenselmask73 = clampResult72;
			float4 varfrensel79 = ( frenselmask73 * _Frenselcolor );
			o.Emission = ( Varlignes23 + varhex43 + varcloud57 + varfrensel79 ).rgb;
			float4 CloudMask56 = lerpResult51;
			float4 HexMask38 = lerpResult36;
			float temp_output_15_0 = ( _Time.y * _Timer );
			float2 temp_cast_1 = (temp_output_15_0).xx;
			float2 uv_TexCoord12 = i.uv_texcoord + temp_cast_1;
			float simplePerlin2D13 = snoise( uv_TexCoord12*50.0 );
			simplePerlin2D13 = simplePerlin2D13*0.5 + 0.5;
			float2 temp_cast_2 = (-temp_output_15_0).xx;
			float2 uv_TexCoord11 = i.uv_texcoord + temp_cast_2;
			float simplePerlin2D14 = snoise( uv_TexCoord11*50.0 );
			simplePerlin2D14 = simplePerlin2D14*0.5 + 0.5;
			float4 varnoise24 = ( saturate( ( simplePerlin2D13 * simplePerlin2D14 ) ) * Varlignes23 );
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen92 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither92 = Dither4x4Bayer( fmod(clipScreen92.x, 4), fmod(clipScreen92.y, 4) );
			dither92 = step( dither92, _Opacity );
			o.Alpha = ( ( CloudMask56 + HexMask38 + varnoise24 ) * dither92 ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

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
				float4 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
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
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack2.xyzw = customInputData.screenPosition;
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
				surfIN.screenPosition = IN.customPack2.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
Version=18800
1536;0;794;1017;5010.022;2348.65;7.028839;True;False
Node;AmplifyShaderEditor.CommentaryNode;99;-2808.048,-650.7346;Inherit;False;3444.967;3081.081;Shader mur;5;27;74;28;44;58;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;27;-2472.592,121.0546;Inherit;False;1854.263;504.3375;lignes mask;2;95;94;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;94;-2396.562,187.8703;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;95;-2367.126,369.1341;Inherit;False;Property;_lignesSpeed;lignes Speed;8;0;Create;True;0;0;0;False;0;False;0.05,0;0,0.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;74;-2758.048,1888.781;Inherit;False;2167.102;521.5347;Frensel mask;16;77;76;75;73;72;70;66;65;67;69;68;64;62;63;60;79;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;28;-2597.636,-600.7346;Inherit;False;1993.872;694.7603;Noise;3;16;9;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;96;-2108.085,287.578;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-1801.117,361.129;Inherit;True;Property;_lignestexture;lignes texture;1;0;Create;True;0;0;0;False;0;False;-1;None;e98fa89f6a92e48479349419488d6a16;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-2547.636,-241.1206;Inherit;False;Property;_Timer;Timer;7;0;Create;True;0;0;0;True;0;False;5;1.77;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;9;-2447.49,-349.8354;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-2708.048,2182.267;Inherit;False;Property;_Power;Power;18;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-2572.048,2026.268;Inherit;False;Property;_Bias;Bias;16;0;Create;True;0;0;0;False;0;False;0;-0.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-2572.048,2106.267;Inherit;False;Property;_Scale;Scale;17;0;Create;True;0;0;0;False;0;False;0;0.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;2;-1492.018,365.4637;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-2274.938,-296.666;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;60;-2367.187,2038.65;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-2004.732,1938.781;Inherit;False;Property;_Frenselintensity;Frensel intensity;19;0;Create;True;0;0;0;False;0;False;0;0.8;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;44;-2661.506,683.6926;Inherit;False;2082.734;572.7748;hex mask;3;30;29;32;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;58;-2669.837,1287.821;Inherit;False;2086.1;572.7744;cloud mask;3;45;47;46;;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;4;-1340.112,441.4597;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-2026.641,2124.269;Inherit;False;Property;_invertFresnselintensity;invert Fresnsel intensity;20;0;Create;True;0;0;0;False;0;False;0;0.51;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1429.934,523.7733;Inherit;False;Property;_Transparancelignes;Transparance lignes;4;0;Create;True;0;0;0;False;0;False;0.5027746;0.71;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;10;-2097.045,-210.8919;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;68;-2010.575,2235.27;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-1233.552,185.4359;Inherit;False;Property;_lignecolor;ligne color;5;0;Create;True;0;0;0;False;0;False;1,0.05316135,0,0;1,0.05316135,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-2611.506,785.5125;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;46;-2586.563,1578.335;Inherit;False;Property;_cloudSpeed;cloud Speed;10;0;Create;True;0;0;0;False;0;False;0.05,0;-0.1,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.LerpOp;3;-1171.287,366.4595;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;32;-2578.232,974.2064;Inherit;False;Property;_HexSpeed;Hex Speed;9;0;Create;True;0;0;0;False;0;False;0.05,0;0.05,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-1724.31,2011.808;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-1905.663,-257.5785;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-2619.837,1389.641;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1906.691,-423.1858;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-1731.612,2211.901;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;30;-2347.112,837.5019;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;13;-1651.593,-550.7346;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;50;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-972.015,247.1517;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-1554.888,2116.966;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;14;-1651.593,-164.9747;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;50;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;47;-2355.443,1441.631;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;33;-2045.348,835.0374;Inherit;True;Property;_Hextexture;Hex texture;2;0;Create;True;0;0;0;False;0;False;-1;None;782b01cd74123774298dc4a8bd83ab6b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;72;-1389.847,2116.965;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-826.7285,242.0223;Inherit;False;Varlignes;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;48;-1997.679,1379.166;Inherit;True;Property;_cloudtexture;cloud texture;3;0;Create;True;0;0;0;False;0;False;-1;None;136141a31a9e7b94880b3f9132847986;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1375.925,-350.154;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;73;-1198.517,2111.125;Inherit;False;frenselmask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-1735.823,1083.946;Inherit;False;Property;_HexAlpha;Hex Alpha;11;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;20;-1185.631,-350.1541;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;35;-1683.183,963.4831;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1744.154,1688.075;Inherit;False;Property;_cloudAlpha;cloud Alpha;12;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;49;-1691.514,1567.612;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;-1205.671,-233.7438;Inherit;False;23;Varlignes;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;36;-1466.552,838.9711;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;51;-1474.883,1443.1;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1287.166,765.0938;Inherit;False;Property;_hexintensity;hex intensity;14;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1289.443,1374.267;Inherit;False;Property;_cloudintensity;cloud intensity;15;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;52;-1242.055,1648.596;Inherit;False;Property;_cloudcolor;cloud color;13;0;Create;True;0;0;0;False;0;False;0.8018868,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;41;-1233.724,1044.467;Inherit;False;Property;_hexcolor;hex color;6;0;Create;True;0;0;0;False;0;False;0,0.8,0.01356068,0;1,0.05490196,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;75;-1171.894,1944.201;Inherit;False;73;frenselmask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;77;-1208.156,2218.346;Inherit;False;Property;_Frenselcolor;Frensel color;22;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1003.255,-307.6442;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-920.9575,2084.899;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;38;-1257.007,956.3977;Inherit;False;HexMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-988.9821,1422.854;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;-827.7624,-312.3135;Inherit;False;varnoise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-980.6512,818.7257;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-1265.338,1560.526;Inherit;False;CloudMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;-218.2661,280.6508;Inherit;False;38;HexMask;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-285.8299,582.8486;Inherit;False;Property;_Opacity;Opacity;21;0;Create;True;0;0;0;False;0;False;0;0.73;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;79;-774.2385,2079.557;Inherit;False;varfrensel;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;90;-215.4839,392.8937;Inherit;False;24;varnoise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-807.7372,1416.95;Inherit;False;varcloud;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;-802.7711,814.8209;Inherit;False;varhex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;85;-222.2661,187.6508;Inherit;False;56;CloudMask;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;-233.8516,-343.9713;Inherit;False;23;Varlignes;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.DitheringNode;92;31.51694,506.2013;Inherit;False;0;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;-234.8516,-255.9713;Inherit;False;43;varhex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;82;-234.8516,-167.9713;Inherit;False;57;varcloud;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;-236.3378,-74.72766;Inherit;False;79;varfrensel;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;28.06396,262.951;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;19.14844,-250.9713;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;207.6713,319.2892;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;34;-1689.256,733.6926;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DesaturateOpNode;54;-1697.587,1337.821;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;381.9188,-102.8146;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Shader/mur;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;5;True;True;0;True;Transparent;;Background;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;96;0;94;0
WireConnection;96;2;95;0
WireConnection;1;1;96;0
WireConnection;2;0;1;0
WireConnection;15;0;9;0
WireConnection;15;1;16;0
WireConnection;60;1;62;0
WireConnection;60;2;63;0
WireConnection;60;3;64;0
WireConnection;4;0;2;0
WireConnection;10;0;15;0
WireConnection;68;0;60;0
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;3;2;5;0
WireConnection;66;0;65;0
WireConnection;66;1;60;0
WireConnection;11;1;10;0
WireConnection;12;1;15;0
WireConnection;69;0;67;0
WireConnection;69;1;68;0
WireConnection;30;0;29;0
WireConnection;30;2;32;0
WireConnection;13;0;12;0
WireConnection;26;0;6;0
WireConnection;26;1;3;0
WireConnection;70;0;66;0
WireConnection;70;1;69;0
WireConnection;14;0;11;0
WireConnection;47;0;45;0
WireConnection;47;2;46;0
WireConnection;33;1;30;0
WireConnection;72;0;70;0
WireConnection;23;0;26;0
WireConnection;48;1;47;0
WireConnection;19;0;13;0
WireConnection;19;1;14;0
WireConnection;73;0;72;0
WireConnection;20;0;19;0
WireConnection;35;0;33;0
WireConnection;49;0;48;0
WireConnection;36;0;33;0
WireConnection;36;1;35;0
WireConnection;36;2;37;0
WireConnection;51;0;48;0
WireConnection;51;1;49;0
WireConnection;51;2;50;0
WireConnection;21;0;20;0
WireConnection;21;1;22;0
WireConnection;76;0;75;0
WireConnection;76;1;77;0
WireConnection;38;0;36;0
WireConnection;55;0;53;0
WireConnection;55;1;51;0
WireConnection;55;2;52;0
WireConnection;24;0;21;0
WireConnection;39;0;42;0
WireConnection;39;1;36;0
WireConnection;39;2;41;0
WireConnection;56;0;51;0
WireConnection;79;0;76;0
WireConnection;57;0;55;0
WireConnection;43;0;39;0
WireConnection;92;0;93;0
WireConnection;89;0;85;0
WireConnection;89;1;86;0
WireConnection;89;2;90;0
WireConnection;80;0;83;0
WireConnection;80;1;81;0
WireConnection;80;2;82;0
WireConnection;80;3;84;0
WireConnection;91;0;89;0
WireConnection;91;1;92;0
WireConnection;34;0;33;0
WireConnection;54;0;48;0
WireConnection;0;2;80;0
WireConnection;0;9;91;0
ASEEND*/
//CHKSM=217467300EB024526F9107F7FF436F3DD7987393