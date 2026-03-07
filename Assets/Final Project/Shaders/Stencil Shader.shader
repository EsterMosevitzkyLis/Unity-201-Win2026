Shader "Custom/StencilMask"
{
    Properties
    {
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1" "RenderPipeline" = "UniversalPipeline"}

        Pass 
        {
            // Only write stencil, no color
            ColorMask 0
            ZWrite Off

            // Pick ONE:
            //Cull Back   // writes stencil only for front faces
            Cull Front // writes stencil only for back faces

            Stencil
            {
                Ref [_StencilID]
                Comp Always
                Pass Replace
            }
        }
    }
}