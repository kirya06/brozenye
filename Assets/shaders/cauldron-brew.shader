// Ideally you wouldn't need half these includes for an unlit shader
// But it's stupiod

FEATURES
{
    #include "common/features.hlsl"
}

MODES
{
    Forward();
    Depth();
}

COMMON
{
	#include "common/shared.hlsl"
}

struct VertexInput
{
	#include "common/vertexinput.hlsl"
};

struct PixelInput
{
	#include "common/pixelinput.hlsl"
};

VS
{
	#include "common/vertex.hlsl"
	#include "procedural.hlsl"

	float g_flNoiseScale;

	PixelInput MainVs( VertexInput i )
	{
		PixelInput o = ProcessVertex( i );

		float2 noise = VoronoiNoise(o.vPositionWs.xy + (g_flTime * g_flNoiseScale), g_flTime, 1);
		o.vPositionWs.z += noise * 2;

		o.vPositionPs = Position3WsToPs( o.vPositionWs.xyz );
		return FinalizeVertex( o );
	}
}

PS
{
	#define CUSTOM_MATERIAL_INPUTS
    #include "common/pixel.hlsl"
	
	float3 g_vColor < UiType( Color );>;

	float4 MainPs( PixelInput i ) : SV_Target0
	{
		return float4( g_vColor, 1 );
	}
}
