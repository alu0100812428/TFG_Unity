Shader "Custom/TextureShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _Color2 ("Color", Color) = (1,1,1,1)
        _SecondTex ("Second Texture", 2D) = "white" {} 
        _Glossiness2 ("Smoothness 2", Range(0,1)) = 0.5
        _Metallic2 ("Metallic 2", Range(0,1)) = 0.0

        _Color3 ("Color", Color) = (1,1,1,1)
        _ThirdTex ("Third Texture", 2D) = "white" {}
        _Glossiness3 ("Smoothness 3", Range(0,1)) = 0.5
        _Metallic3 ("Metallic 3", Range(0,1)) = 0.0

        _Color4 ("Color", Color) = (1,1,1,1)
        _SnowTex ("Snow Texture", 2D) = "white" {}
        _Glossiness4 ("Smoothness 4", Range(0,1)) = 0.5
        _Metallic4 ("Metallic 4", Range(0,1)) = 0.0
        _snow ("snow", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _SecondTex;
        sampler2D _ThirdTex;
        sampler2D _SnowTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_SecondTex;
            float2 uv_ThirdTex;
            float2 uv_SnowTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        half _Glossiness2;
        half _Metallic2;
        half _Glossiness3;
        half _Metallic3;
        half _Glossiness4;
        half _Metallic4;
        float _snow;
        fixed4 _Color;
        fixed4 _Color2;
        fixed4 _Color3;
        fixed4 _Color4;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            //o.localPos = v.vertex.xyz;
            fixed4 c;
            if(IN.worldPos.y < 10){
                c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
            }
            if((IN.worldPos.y >= 10)&&(IN.worldPos.y < 15)){
                float rango = (IN.worldPos.y - 10)/(15-10);
                c=lerp (tex2D (_MainTex, IN.uv_MainTex), tex2D (_SecondTex, IN.uv_SecondTex), rango) *lerp(_Color,_Color2,rango);
                o.Metallic = lerp(_Metallic, _Metallic2,rango);
                o.Smoothness =lerp(_Glossiness, _Glossiness2,rango);
            }
            if(((IN.worldPos.y >= 15))&&(IN.worldPos.y < 25)){
                c = tex2D (_SecondTex, IN.uv_SecondTex) * _Color2;
                o.Metallic = _Metallic2;
                o.Smoothness = _Glossiness2;
            }
            if((IN.worldPos.y >= 25)&&(IN.worldPos.y < 30)){
                float rango = (IN.worldPos.y - 25)/(30-25);
                c=lerp (tex2D (_SecondTex, IN.uv_SecondTex),tex2D (_ThirdTex, IN.uv_ThirdTex), rango) * lerp(_Color2,_Color3,rango);
                o.Metallic = lerp(_Metallic2, _Metallic3,rango);
                o.Smoothness =lerp(_Glossiness2, _Glossiness3,rango);
            }
            if(IN.worldPos.y >= 30){
                
                if((IN.worldPos.y >=70)&&(o.Normal.y > _snow)){
                    float rango = (o.Normal.y - _snow)/(1.0f-_snow);
                    if(IN.worldPos.y > 85){
                        c=lerp (tex2D (_ThirdTex, IN.uv_ThirdTex), tex2D (_SnowTex, IN.uv_SnowTex), rango) * lerp(_Color3,_Color4,rango);
                        o.Metallic = lerp(_Metallic3, _Metallic4,rango);
                        o.Smoothness =lerp(_Glossiness3, _Glossiness4,rango);
                    }
                    else{
                        float rango2 = (IN.worldPos.y - 70)/(85-70);
                        c=lerp (tex2D (_ThirdTex, IN.uv_ThirdTex), lerp (tex2D (_ThirdTex, IN.uv_ThirdTex), tex2D (_SnowTex, IN.uv_SnowTex), rango) * lerp(_Color3,_Color4,rango), rango2) * lerp(_Color3,_Color4,rango);
                        o.Metallic = lerp(_Metallic3, lerp(_Metallic3, _Metallic4,rango),rango2);
                        o.Smoothness =lerp(_Glossiness3, lerp(_Glossiness3, _Glossiness4,rango),rango2);
                    }
                    /*
                    c=lerp (tex2D (_ThirdTex, IN.uv_ThirdTex), tex2D (_SnowTex, IN.uv_SnowTex), rango) * lerp(_Color3,_Color4,rango);
                    o.Metallic = lerp(_Metallic3, _Metallic4,rango);
                    o.Smoothness =lerp(_Glossiness3, _Glossiness4,rango);
                    */
                    //c = tex2D (_SnowTex, IN.uv_SnowTex) * _Color4;
                }else{
                    c = tex2D (_ThirdTex, IN.uv_ThirdTex) * _Color3;
                    o.Metallic = _Metallic3;
                    o.Smoothness = _Glossiness3;
                }
                
                //c = tex2D (_ThirdTex, IN.uv_ThirdTex) * _Color3;
                //o.Metallic = _Metallic3;
                //o.Smoothness = _Glossiness3;
            }
            
            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            //fixed4 c = tex2D (_SecondTex, IN.uv_SecondTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
