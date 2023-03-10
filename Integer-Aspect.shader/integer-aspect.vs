#version 150

in vec4 position;
in vec2 texCoord;

out Vertex {
   vec2 texCoord;
};

uniform vec4 targetSize;
uniform vec4 sourceSize[];

void main() {
   gl_Position = position;
	// get the largest integer that will fit onscreen vertically
	float vert_scale = near(params.OutputSize.y / params.SourceSize.y);
	// get the closest integer to the desired vert scale multiplied by 4/3
	float horz_scale = round(vert_scale * 1.3333333) + 0.0001;
	// find the ratio of the current scale to the desired scale
	vec2 scale = (params.OutputSize.xy / params.SourceSize.xy) / vec2(horz_scale, vert_scale);
	vec2 middle = vec2(0.4999999, 0.4999999);
	// move the center of the screen to the origin
	vec2 diff = texCoord.xy - middle;
	// scale our image by the ratio and then move the center back to screen center
	texCoord = middle + (diff * scale);
}
