Shader "Unlit/Glow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		//_Circle ("Circle", Vector) = (0,0,0,0)
		//_STime ("Time", float) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

		ZWrite off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Circle;
			float4 _MainTex_TexelSize;
			float _STime;

			float4 _Circles[20];
			float _STimes[20];

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
			//col.a =  (1.0 - step(_Circle.z, distance(_Circle.xy * _MainTex_TexelSize.zw / _Circle.z, i.uv * _MainTex_TexelSize.zw / _Circle.z)))*(1.0 - pow(distance(_Circle.xy * _MainTex_TexelSize.zw / _Circle.z, i.uv * _MainTex_TexelSize.zw / _Circle.z), _Circle.w*_STime));
			
			float Alpha = 0;
			for (int j = 0; j < 20; j++)
			{
				Alpha += (1.0 - step(_Circles[j].z, distance(_Circles[j].xy * _MainTex_TexelSize.zw / _Circles[j].z, i.uv * _MainTex_TexelSize.zw / _Circles[j].z) * _MainTex_TexelSize.z)) * (1.0 - pow(distance(_Circles[j].xy * _MainTex_TexelSize.zw / _Circles[j].z, i.uv * _MainTex_TexelSize.zw / _Circles[j].z) * _MainTex_TexelSize.z / _Circles[j].z, _Circles[j].w * _STimes[j]));
			}
			//Alpha += (1.0 - step(_Circle.z, distance(_Circle.xy * _MainTex_TexelSize.zw / _Circle.z, i.uv * _MainTex_TexelSize.zw / _Circle.z) * _MainTex_TexelSize.z)) * (1.0 - pow(distance(_Circle.xy * _MainTex_TexelSize.zw / _Circle.z, i.uv * _MainTex_TexelSize.zw / _Circle.z) * _MainTex_TexelSize.z / _Circle.z, _Circle.w * _STime));
			//Alpha += (1.0 - step(_Circle.z, distance(_Circle.xy/2.0 * _MainTex_TexelSize.zw / _Circle.z, i.uv * _MainTex_TexelSize.zw / _Circle.z) * _MainTex_TexelSize.z)) * (1.0 - pow(distance(_Circle.xy/2.0 * _MainTex_TexelSize.zw / _Circle.z, i.uv * _MainTex_TexelSize.zw / _Circle.z) * _MainTex_TexelSize.z / _Circle.z, _Circle.w * _STime));
			col.a = Alpha;
			return col;
            }
            ENDCG
        }
    }
}
