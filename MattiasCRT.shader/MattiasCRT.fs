#version 150

/* 
   MattiasCRT by Mattias
   ported to quark format by hunterk
   based on https://www.shadertoy.com/view/Ms23DR
*/

uniform sampler2D source[];
uniform vec4 sourceSize[];
uniform vec4 targetSize;
uniform int phase;

//////// shadertoy compatibility macros /////////

#define iResolution targetSize
#define iTime float(phase)
#define fragCoord gl_FragCoord.xy

////// end shadertoy compatibility macros ///////

//////// user settings /////////

// wiggle makes the screen wiggle
//#define wiggle

// roll makes the scanlines roll
#define roll

// whether to have a mask effect or not. Disabling for now since Ares does
// post-fullscreen stretching, which breaks it unless windowed
//#define mask

////// end user settings ///////

in Vertex {
   vec2 texCoord;
};

out vec4 fragColor;

vec2 curve(vec2 uv)
{
	uv = (uv - 0.5) * 2.0;
	uv *= 1.1;	
	uv.x *= 1.0 + pow((abs(uv.y) / 5.0), 2.0);
	uv.y *= 1.0 + pow((abs(uv.x) / 4.0), 2.0);
	uv  = (uv / 2.0) + 0.5;
	uv =  uv *0.92 + 0.04;
	return uv;
}

void main() {
    vec2 q = fragCoord.xy / iResolution.xy;
    vec2 uv = q;
    uv = curve( uv );
    vec3 oricol = texture( source[0], vec2(q.x,q.y) ).xyz;
    vec3 col;
#ifdef wiggle
	float x =  sin(0.3*wiggle+uv.y*21.0)*sin(0.7*wiggle+uv.y*29.0)*sin(0.3+0.33*wiggle+uv.y*31.0)*0.0017;
#else
   float x = 0.;
#endif

    col.r = texture(source[0],vec2(x+uv.x+0.001,uv.y+0.001)).x+0.05;
    col.g = texture(source[0],vec2(x+uv.x+0.000,uv.y-0.002)).y+0.05;
    col.b = texture(source[0],vec2(x+uv.x-0.002,uv.y+0.000)).z+0.05;
    col.r += 0.08*texture(source[0],0.75*vec2(x+0.025, -0.027)+vec2(uv.x+0.001,uv.y+0.001)).x;
    col.g += 0.05*texture(source[0],0.75*vec2(x+-0.022, -0.02)+vec2(uv.x+0.000,uv.y-0.002)).y;
    col.b += 0.08*texture(source[0],0.75*vec2(x+-0.02, -0.018)+vec2(uv.x-0.002,uv.y+0.000)).z;

    col = clamp(col*0.6+0.4*col*col*1.0,0.0,1.0);

    float vig = (0.0 + 1.0*16.0*uv.x*uv.y*(1.0-uv.x)*(1.0-uv.y));
	col *= vec3(pow(vig,0.3));

    col *= vec3(0.95,1.05,0.95);
	col *= 2.8;

#ifdef roll
	float scans = clamp( 0.35+0.35*sin(3.5*iTime+uv.y*iResolution.y*1.5), 0.0, 1.0);
#else
   float scans = clamp( 0.35+0.35*sin(3.5+uv.y*iResolution.y*1.5), 0.0, 1.0);
#endif
	
	float s = pow(scans,1.7);
	col = col*vec3( 0.4+0.7*s) ;

    col *= 1.0+0.01*sin(110.0*iTime);
	if (uv.x < 0.0 || uv.x > 1.0)
		col *= 0.0;
	if (uv.y < 0.0 || uv.y > 1.0)
		col *= 0.0;

#ifdef mask	
	col*=1.0-0.65*vec3(clamp((mod(fragCoord.x, 2.0)-1.0)*2.0,0.0,1.0));
#else
   col*=0.7;
#endif
	
    float comp = smoothstep( 0.1, 0.9, sin(iTime) );
 
	// Remove the next line to stop cross-fade between original and postprocess
//	col = mix( col, oricol, comp );

    fragColor = vec4(col,1.0);
}
