#version 150

// downsample shader by hunterk
// place as the first pass to make most of the existing shaders play nicely with
// pixel-doubled inputs, like you get from higan/ares running the accuracy PPU.

// to add it, place these lines in the manifest.bml before any other passes:
// program
//   filter: nearest
//   fragment: downsample.fs
//   height: 50%
//   width: 50%

uniform sampler2D source[];
uniform vec4 sourceSize[];
uniform vec4 targetSize;

in Vertex {
   vec2 texCoord;
};

out vec4 fragColor;

void main() {
   vec2 one_px = texCoord.xy * sourceSize[0].zw;
   fragColor = texture(source[0], texCoord - vec2(float(mod(one_px, 2.00001))));
}
