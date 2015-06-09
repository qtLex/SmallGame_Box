Shader "Custom/OutlineToonShader" {
    Properties {
    
        _MainTex ("Base (RGB)", 2D) = "white" {} // Текстура
        _Bump ("Bump", 2D) = "bump" {} // Бамп
        
        _Ramp ("Ramp Texture", 2D) = "gray" {}
        _Outline ("Outline", Range(0, 0.30)) = 0.08 // Ширина обводки
        _OutlineColor("Outline color", Color) = (0.0, 0.0, 0.0, 1)
        
        _FinalBrightness("Final brightness", range(1,15)) = 1 // Яркость филального цвета(после всех расчетов)
        
        _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0) // Цвет выделения
      	_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0 // Степень выделения
        _RimStrength("Rim strength", range(0, 1)) = 0 // Напряженность выделения
        
        _GlossSize("Gloss size", range(-1, 1)) = 0.99
        _AlphaTransparency("Alpha", range(0.0, 1)) = 0.0
        
    }
    SubShader {
        Tags {"Queue"="Transparent-1" "RenderType"="Transparent" }
        LOD 200
 
        Pass {
 
            Cull Front
            Lighting Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha 
            Tags { "LightMode"="ForwardBase" }
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
            
            float4 _OutlineColor;
            
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 tangent : TANGENT;
            }; 
 
            struct v2f
            {
                float4 pos : POSITION;
            };
 
            float _Outline;
            float _AlphaTransparency;
 
            v2f vert (a2v v)
            {
                v2f o;
                float4 pos = mul( UNITY_MATRIX_MV, v.vertex); 
                float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal);  
                normal.z = -0.4;
                pos = pos + float4(normalize(normal),0) * _Outline;
                o.pos = mul(UNITY_MATRIX_P, pos);
 
                return o;
            }
 
            float4 frag (v2f IN) : COLOR
            {	
            	
                return float4(_OutlineColor.r, _OutlineColor.g, _OutlineColor.b, 1.0 - _AlphaTransparency);
            }
 
            ENDCG
 
        }
               
        Pass {
 
            Cull Back 
            Lighting On
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha 
            Tags { "LightMode"="ForwardBase" }
 
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members lightDirection)
#pragma exclude_renderers d3d11 xbox360
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
            uniform float4 _LightColor0;
 
            sampler2D _MainTex;
            sampler2D _Bump;
            sampler2D _Ramp;
 
            float4 _MainTex_ST;
            float4 _Bump_ST;
 
            float _Tooniness;
            
            float _FinalBrightness;
            
            float4 _RimColor;
      		float _RimPower;
        	float _RimStrength; 
        	half _GlossSize;  
        	float _AlphaTransparency;    
 
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 tangent : TANGENT;
 
            }; 
 
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 lightDirection;
                float3 viewDir;
                float3 wNormal;
                float3 facingOutFactor;
 
            };
 
            v2f vert (inout a2v v)
            {
                v2f o;
                //Create a rotation matrix for tangent space
                TANGENT_SPACE_ROTATION; 
                //Store the light's direction in tangent space
                o.lightDirection = mul(rotation, ObjSpaceLightDir(v.vertex));
                
                // Store view direction in tangent space
				o.viewDir  = mul (rotation, ObjSpaceViewDir(v.vertex));
                
                //Transform the vertex to projection space
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); 
                //Get the UV coordinates
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);  
                o.uv2 = TRANSFORM_TEX (v.texcoord, _Bump);
                
                // как мне тут нормаль получить? пздц я не шарю.                                
                o.wNormal = mul(rotation, v.normal);
                
                return o;
            }
 
            float4 frag(v2f i) : COLOR  
            { 
                //Get the color of the pixel from the texture
                float4 c = tex2D (_MainTex, i.uv); 
                 
                //Merge the colours
                //c.rgb = (floor(c.rgb*_ColorMerge) / _ColorMerge);
 
                //Get the normal from the bump map
                float3 n =  UnpackNormal(tex2D (_Bump, i.uv2)); 
 
                //Based on the ambient light
                float3 lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz;
 
                //Work out this distance of the light
                float lengthSq = dot(i.lightDirection, i.lightDirection);
                //Fix the attenuation based on the distance
                float atten = 1.0 / (1.0 + lengthSq);
                
                //Angle to the light
                float diff = saturate (dot (n, normalize(i.lightDirection)));  
                
                //Perform our toon light mapping 
                diff = tex2D(_Ramp, float2(diff, 0.3));
                //Update the colour
                lightColor = _LightColor0.rgb * (diff * atten);
                
               	half Rim = pow (1.0 - saturate(dot(normalize(i.viewDir), n)), _RimPower) * _RimStrength;
               	
               	half Gloss = saturate(dot(normalize(i.lightDirection), i.wNormal)) > _GlossSize ? 30.0 : 0.0;
               	
                c.rgb = c.rgb * lightColor + Gloss; // toon color
                
                c.rgb *= _FinalBrightness; // brightness adjustments
                
                c.rgb += Rim * _RimColor; // rim effect;
                
                c.a = 1.0 - _AlphaTransparency;
                  
                return c; 
 
            } 
 
            ENDCG
        }
        
    }
    FallBack "Diffuse"
}