Shader "Hidden/Onovich/OboroSampleContour" {
    SubShader {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Pass {
            ZWrite Off
            Cull Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define MAX_OBSTACLES 16
            #define MAX_CONTOURS 32
            #define DISTURBANCE_CURVE_SAMPLES 32

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _ScreenSize;
            float _TimeValue;
            float _LineThickness;
            float _LineFeather;
            float _DisturbanceIntensity;
            int _ObstacleCount;
            float4 _ObstacleData[MAX_OBSTACLES];
            int _ContourCount;
            float _ContourValues[MAX_CONTOURS];
            float4 _ContourColors[MAX_CONTOURS];
            float _DisturbanceCurve[DISTURBANCE_CURVE_SAMPLES];

            v2f vert(appdata input) {
                v2f output;
                output.vertex = float4(input.vertex.xy * 2.0 - 1.0, 0.0, 1.0);
                output.uv = input.uv;
                return output;
            }

            float SampleDisturbanceCurve(float normalized) {
                float scaled = saturate(normalized) * (DISTURBANCE_CURVE_SAMPLES - 1);
                int lowerIndex = (int)scaled;
                int upperIndex = min(lowerIndex + 1, DISTURBANCE_CURVE_SAMPLES - 1);
                float blend = scaled - lowerIndex;
                return lerp(_DisturbanceCurve[lowerIndex], _DisturbanceCurve[upperIndex], blend);
            }

            float EvaluateField(float2 pixelPosition) {
                float rawValue =
                    (sin(pixelPosition.x * 0.002 + _TimeValue * 0.4) +
                     cos(pixelPosition.y * 0.0025 - _TimeValue * 0.3) +
                     sin((pixelPosition.x + pixelPosition.y) * 0.0015 + _TimeValue * 0.2)) * 1.5;

                float normalizedDisturbance = saturate(rawValue / 9.0 + 0.5);
                float value = (SampleDisturbanceCurve(normalizedDisturbance) - 0.5) * 9.0 * _DisturbanceIntensity;

                [loop]
                for (int index = 0; index < MAX_OBSTACLES; index++) {
                    if (index >= _ObstacleCount) {
                        break;
                    }

                    float4 obstacle = _ObstacleData[index];
                    float2 delta = pixelPosition - obstacle.xy;
                    float distSq = dot(delta, delta) + obstacle.w;
                    value += obstacle.z / distSq;
                }

                return value;
            }

            fixed4 frag(v2f input) : SV_Target {
                float2 pixelPosition = input.uv * _ScreenSize.xy;
                float field = EvaluateField(pixelPosition);
                float fieldWidth = max(fwidth(field), 1e-4);

                float totalAlpha = 0.0;
                float3 weightedColor = 0.0;

                [loop]
                for (int index = 0; index < MAX_CONTOURS; index++) {
                    if (index >= _ContourCount) {
                        break;
                    }

                    float level = _ContourValues[index];
                    float distanceToLevelInPixels = abs(field - level) / fieldWidth;
                    float band = 1.0 - smoothstep(_LineThickness, _LineThickness + _LineFeather, distanceToLevelInPixels);
                    float4 contourColor = _ContourColors[index];
                    float alpha = contourColor.a * band;
                    weightedColor += contourColor.rgb * alpha;
                    totalAlpha += alpha;
                }

                totalAlpha = saturate(totalAlpha);
                float3 finalColor = totalAlpha > 0.0001 ? weightedColor / max(totalAlpha, 0.0001) : 0.0;
                return float4(finalColor, totalAlpha);
            }
            ENDCG
        }
    }
}