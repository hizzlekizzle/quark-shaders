#version 150

/*
   Shader Modified: Pokefan531
   Color Mangler
   Author: hunterk
   License: Public domain
*/
// Shader that replicates the LCD dynamics from a GameBoy Advance

// Darken Screen - sane values from -0.25 to 1.0
#define darken_screen 1.0

#define target_gamma 2.2
#define display_gamma 2.2
#define sat 1.0
#define lum 0.94
#define contrast 1.0
#define blr 0.0
#define blg 0.0
#define blb 0.0
#define r 0.82
#define g 0.665
#define b 0.73
#define rg 0.125
#define rb 0.195
#define gr 0.24
#define gb 0.075
#define br -0.06
#define bg 0.21
#define overscan_percent_x 0.0
#define overscan_percent_y 0.0

uniform sampler2D source[];
uniform vec4 sourceSize[];
uniform vec4 targetSize;

in Vertex {
   vec2 vTexCoord;
};

out vec4 FragColor;

void main() {
   vec4 screen = pow(texture(source[0], vTexCoord), vec4(target_gamma + darken_screen)).rgba;
   vec4 avglum = vec4(0.5);
   screen = mix(screen, avglum, (1.0 - contrast));
   
 //				r   g    b   black
mat4 color = mat4(r,  rg,  rb, 0.0,  //red channel
			   gr,  g,   gb, 0.0,  //green channel
			   br,  bg,  b,  0.0,  //blue channel
			  blr, blg, blb,    0.0); //alpha channel; these numbers do nothing for our purposes.
			  
mat4 adjust = mat4((1.0 - sat) * 0.3086 + sat, (1.0 - sat) * 0.3086, (1.0 - sat) * 0.3086, 1.0,
(1.0 - sat) * 0.6094, (1.0 - sat) * 0.6094 + sat, (1.0 - sat) * 0.6094, 1.0,
(1.0 - sat) * 0.0820, (1.0 - sat) * 0.0820, (1.0 - sat) * 0.0820 + sat, 1.0,
0.0, 0.0, 0.0, 1.0);
	color *= adjust;
	screen = clamp(screen * lum, 0.0, 1.0);
	screen = color * screen;
	FragColor = pow(screen, vec4(1.0 / (display_gamma)));
}
