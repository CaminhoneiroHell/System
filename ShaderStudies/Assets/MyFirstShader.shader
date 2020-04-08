Shader "Holistic/HelloShader" {
	
	//Properties
	Properties {
	     _myColour ("Example Colour", Color) = (1,1,1,1)
		 _myNormal ("Example Normal", Color) = (1,1,1,1)
	     _myEmission ("Example Emission", Color) = (1,1,1,1)
	}
	
		//Processing...
		
	SubShader {
		
		CGPROGRAM //CG/HLSL (High level shader language)
			#pragma surface surf Lambert	//surface = Compile surface shader. surf = name of shader function. Lambert = type of lighting model

			//Type of input data that will be used (vertices, normals, UVs...)
			struct Input {
				float2 uvMainTex;
			};

			//Propertie accessors (What I want to be available outside)
			fixed4 _myColour;
			fixed4 _myEmission;
			fixed4 _myNormal;
			
			//My shader function
			void surf (Input IN, inout SurfaceOutput o){
			    o.Albedo = _myColour.rgb;
			    o.Emission = _myEmission.rgb;
				o.Normal = _myNormal.rgb;
			}
		
		ENDCG
	}
	
	FallBack "Diffuse"	//For inferior gpus
}