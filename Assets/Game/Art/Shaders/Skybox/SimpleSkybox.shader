// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/SimpleSkybox"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Clouds("Clouds", 2D) = "white" {}
		_Color1("Color 1", Color) = (0,0,0,0)
		_Color2("Color 2", Color) = (0,0,0,0)
		_Color3("Color 3", Color) = (0,0,0,0)
		_Vector0("Vector 0", Vector) = (0,0,0,0)
		_Tilling("Tilling", Float) = 0

	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
	LOD 100
		Cull Off

		
		Pass
		{
			CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif

			#pragma target 3.0 
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform sampler2D _MainTex;
			uniform fixed4 _Color;
			uniform float4 _Color1;
			uniform sampler2D _Clouds;
			uniform float2 _Vector0;
			uniform float _Tilling;
			uniform float4 _Color2;
			uniform float4 _Color3;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.texcoord.xy = v.texcoord.xy;
				o.texcoord.zw = v.texcoord1.xy;
				
				// ase common template code
				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

				fixed4 myColorVar;
				// ase common template code
				float3 ase_worldPos = i.ase_texcoord1.xyz;
				float3 normalizeResult67 = normalize( ase_worldPos );
				float3 break68 = normalizeResult67;
				float2 appendResult73 = (float2(( atan2( break68.x , break68.z ) / 6.28318548202515 ) , ( asin( break68.y ) / ( UNITY_PI / 2.0 ) )));
				float2 panner89 = ( 1.0 * _Time.y * _Vector0 + ( appendResult73 * _Tilling ));
				float4 lerpResult84 = lerp( _Color2 , _Color3 , appendResult73.y);
				
				
				myColorVar = ( ( _Color1 * tex2D( _Clouds, panner89 ).a ) + lerpResult84 );
				return myColorVar;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18935
0;411;1100;408;215.4611;-543.5839;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;66;-650.5563,816.5499;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;67;-498.3951,821.6976;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PiNode;76;-246.2017,1029.129;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;68;-375.0279,824.9728;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ASinOpNode;74;-50.77905,902.4865;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;77;-56.23766,990.9179;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;72;-198.1649,833.7066;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;70;-248.3849,744.1835;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;71;37.65248,769.2935;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;75;62.76257,905.7616;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;73;172.1576,762.4861;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;92;323.5389,691.5839;Inherit;False;Property;_Tilling;Tilling;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;534.5389,698.5839;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;90;584.5389,757.5839;Inherit;False;Property;_Vector0;Vector 0;11;0;Create;True;0;0;0;False;0;False;0,0;0.01,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.BreakToComponentsNode;83;853.0525,1125.451;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.PannerNode;89;667.8016,619.5702;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;87;975.9274,956.1525;Inherit;False;Property;_Color3;Color 3;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.09570109,0.004716992,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;86;1223.927,1075.152;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;81;1101.185,399.9726;Inherit;False;Property;_Color1;Color 1;8;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.8494571,0.8916575,0.9528301,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;65;870.5009,576.3888;Inherit;True;Property;_Clouds;Clouds;7;0;Create;True;0;0;0;False;0;False;-1;None;ade1ea0dbf3da8d43a2966bda7f2ff29;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;85;958.9274,762.1525;Inherit;False;Property;_Color2;Color 2;9;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.6462264,0.717462,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;84;1276.637,773.1598;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;1348.223,452.74;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-318.5229,36.28214;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-515.8541,99.09882;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;1;-1299.105,-87.43675;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;254.8971,-294.8168;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;26;-105.4508,229.4486;Inherit;False;Property;_Middle;Middle;6;1;[Header];Create;True;1;Middle;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;39;-282.1488,-348.4726;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;37;434.6827,389.3214;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-70.36618,447.3268;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;13;-621.1946,-203.1741;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;154.6357,109.9104;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;536.8779,-45.99239;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-86.14718,-20.85532;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.78;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;38;-470.5332,324.3279;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;20;29.09351,45.06211;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;2;-1127.599,-88.52176;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;5;-995.0065,-87.17358;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.WireNode;40;-508.6585,5.868872;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1085.678,105.9071;Inherit;False;Property;_BotPosition;BotPosition;5;0;Create;True;0;0;0;False;0;False;0;0.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;-372.8113,-523.736;Inherit;False;Property;_Top;Top;0;1;[Header];Create;True;1;Top;0;0;False;0;False;0.5613208,0.9548879,1,0;0.4605286,0.6453437,0.745283,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMinOpNode;16;-794.7192,64.87525;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-858.2092,181.127;Inherit;False;Property;_BotFallOff;BotFallOff;4;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;24;-355.6641,514.3547;Inherit;False;Property;_Bot;Bot;3;1;[Header];Create;True;1;Bot;0;0;False;0;False;0.6792453,0.2726171,0,0;0.8396226,0.8396226,0.8396226,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-679.2092,67.12696;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.38;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-848.6479,-40.01312;Inherit;False;Property;_TopPosition;TopPosition;1;0;Create;True;0;0;0;False;0;False;0;0.52;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;41.47629,-262.7589;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1.12;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;1513.581,462.7536;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-894.6779,57.90713;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.84;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-731.7424,-195.9251;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-143.2196,-182.8134;Inherit;False;Property;_TopFallOff;TopFallOff;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;64;1686.488,454.792;Float;False;True;-1;2;ASEMaterialInspector;100;5;Custom/SimpleSkybox;6e114a916ca3e4b4bb51972669d463bf;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;67;0;66;0
WireConnection;68;0;67;0
WireConnection;74;0;68;1
WireConnection;77;0;76;0
WireConnection;70;0;68;0
WireConnection;70;1;68;2
WireConnection;71;0;70;0
WireConnection;71;1;72;0
WireConnection;75;0;74;0
WireConnection;75;1;77;0
WireConnection;73;0;71;0
WireConnection;73;1;75;0
WireConnection;91;0;73;0
WireConnection;91;1;92;0
WireConnection;83;0;73;0
WireConnection;89;0;91;0
WireConnection;89;2;90;0
WireConnection;86;0;83;1
WireConnection;65;1;89;0
WireConnection;84;0;85;0
WireConnection;84;1;87;0
WireConnection;84;2;86;0
WireConnection;88;0;81;0
WireConnection;88;1;65;4
WireConnection;19;0;40;0
WireConnection;19;1;17;0
WireConnection;17;0;33;0
WireConnection;21;0;22;0
WireConnection;21;1;31;0
WireConnection;39;0;13;0
WireConnection;37;0;23;0
WireConnection;23;0;38;0
WireConnection;23;1;24;0
WireConnection;13;0;28;0
WireConnection;25;0;20;0
WireConnection;25;1;26;0
WireConnection;27;0;21;0
WireConnection;27;1;25;0
WireConnection;27;2;37;0
WireConnection;38;0;17;0
WireConnection;20;1;19;0
WireConnection;2;0;1;0
WireConnection;5;0;2;0
WireConnection;40;0;13;0
WireConnection;16;0;35;0
WireConnection;33;0;16;0
WireConnection;33;1;34;0
WireConnection;31;0;39;0
WireConnection;31;1;32;0
WireConnection;82;0;88;0
WireConnection;82;1;84;0
WireConnection;35;0;5;1
WireConnection;35;1;36;0
WireConnection;28;0;5;1
WireConnection;28;1;29;0
WireConnection;64;0;82;0
ASEEND*/
//CHKSM=CB77ECBA04610CF1E3EB532C2A7475B7725E8CDD