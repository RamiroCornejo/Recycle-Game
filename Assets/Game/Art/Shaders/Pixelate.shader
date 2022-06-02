// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/PP/Pixelate"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_X("X", Float) = 0
		_Y("Y", Float) = 0
		_Float0("Float 0", Float) = 0
		_Color0("Color 0", Color) = (0,0,0,0)

	}

	SubShader
	{
		LOD 0

		
		
		ZTest Always
		Cull Off
		ZWrite On

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			

			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform float _X;
			uniform float _Y;
			uniform float4 _Color0;
			uniform float _Float0;


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float2 texCoord1 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float pixelWidth4 =  1.0f / _X;
				float pixelHeight4 = 1.0f / _Y;
				half2 pixelateduv4 = half2((int)(texCoord1.x / pixelWidth4) * pixelWidth4, (int)(texCoord1.y / pixelHeight4) * pixelHeight4);
				float4 tex2DNode2 = tex2D( _MainTex, pixelateduv4 );
				float grayscale24 = dot(tex2DNode2.rgb, float3(0.299,0.587,0.114));
				float4 temp_cast_2 = (grayscale24).xxxx;
				float div25=256.0/float((int)_Float0);
				float4 posterize25 = ( floor( temp_cast_2 * div25 ) / div25 );
				float4 lerpResult27 = lerp( tex2DNode2 , ( tex2DNode2 * _Color0 ) , (posterize25).r);
				

				finalColor = lerpResult27;

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18935
272;73;827;320;220.949;-16.93286;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1091.273,57.19719;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-1035.393,183.2334;Inherit;False;Property;_X;X;0;0;Create;True;0;0;0;False;0;False;0;160;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1033.596,249.5983;Inherit;False;Property;_Y;Y;1;0;Create;True;0;0;0;False;0;False;0;160;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCPixelate;4;-797.0231,107.1528;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;3;-346,-136;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-180,-125;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;86.19409,235.5536;Inherit;False;Property;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;0;46.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;24;82.19409,155.5536;Inherit;False;1;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;30;121.53,0.5191193;Inherit;False;Property;_Color0;Color 0;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.6132076,0.6132076,0.6132076,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosterizeNode;25;285.1941,150.5536;Inherit;False;1;2;1;COLOR;0,0,0,0;False;0;INT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;28;440.1941,136.5536;Inherit;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;404.53,-51.48088;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.3584906,0.3584906,0.3584906,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;27;658.1941,-112.4464;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;833.0926,-132.0602;Float;False;True;-1;2;ASEMaterialInspector;0;2;Custom/PP/Pixelate;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;True;True;0;False;-1;True;7;False;-1;False;True;0;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;4;0;1;0
WireConnection;4;1;5;0
WireConnection;4;2;6;0
WireConnection;2;0;3;0
WireConnection;2;1;4;0
WireConnection;24;0;2;0
WireConnection;25;1;24;0
WireConnection;25;0;26;0
WireConnection;28;0;25;0
WireConnection;29;0;2;0
WireConnection;29;1;30;0
WireConnection;27;0;2;0
WireConnection;27;1;29;0
WireConnection;27;2;28;0
WireConnection;0;0;27;0
ASEEND*/
//CHKSM=222250F77D4E33F2F21EC9670F13BBF362D1A681