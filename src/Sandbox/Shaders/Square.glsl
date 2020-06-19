#type vert
#version 330 core

layout(location = 0) in vec3 a_Position;

uniform mat4 u_ViewProjection;
uniform mat4 u_Transform;

out vec3 v_Position;

void main()
{
	v_Position = a_Position;
	gl_Position = u_ViewProjection * u_Transform * vec4(a_Position, 1.0);
}

#type frag
#version 330 core

layout(location = 0) out vec4 color;
in vec3 v_Position;

void main()
{
	color = vec4(0.2, 0.3, 0.8, 1.0);
}