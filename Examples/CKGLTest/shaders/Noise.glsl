#vertex

layout(location = 0) in vec3 position;
layout(location = 1) in vec4 colour;
layout(location = 2) in vec2 uv;
layout(location = 3) in float textured;

uniform mat4 MVP;

out vec4 vColour;
out vec2 vUV;
out float vTextured;

void main()
{
	gl_Position = vec4(position.xyz, 1.0) * MVP;
	vColour = colour;
	vUV = uv;
	vTextured = textured;
}


#fragment

// pseudo-random
float random(vec2 v)
{
    return fract(sin(dot(v.xy, vec2(12.9898, 78.233))) * 43758.5453);
}

// Simple hash
float Hash(vec2 v)
{
	return fract(cos(dot(v, vec2(91.52, -74.27))) * 939.24);
}

//2D signed hash
vec2 Hash2(vec2 v)
{
	return 1. - 2. * fract(cos(v.x * vec2(91.52, -74.27) + v.y * vec2(-39.07, 09.78)) * 939.24);
}

//2D value noise.
float Value(vec2 P)
{
	vec2 F = floor(P);
	vec2 S = P-F;
    //Bi-cubic interpolation for mixing the cells.
	vec4 M = (S*S*(3.-S-S)).xyxy;
    M = M*vec4(-1,-1,1,1)+vec4(1,1,0,0);

    //Mix between cells.
	return (Hash(F+vec2(0,0))*M.x+Hash(F+vec2(1,0))*M.z)*M.y+
		   (Hash(F+vec2(0,1))*M.x+Hash(F+vec2(1,1))*M.z)*M.w;
}
//2D Perlin gradient noise.
float Perlin(vec2 P)
{
	vec2 F = floor(P);
	vec2 S = P-F;
    //Bi-quintic interpolation for mixing the cells.
	vec4 M = (S*S*S*(6.*S*S-15.*S+10.)).xyxy;
    M = M*vec4(-1,-1,1,1)+vec4(1,1,0,0);

    //Add up the gradients.
	return (dot(Hash2(F+vec2(0,0)),S-vec2(0,0))*M.x+dot(Hash2(F+vec2(1,0)),S-vec2(1,0))*M.z)*M.y+
		   (dot(Hash2(F+vec2(0,1)),S-vec2(0,1))*M.x+dot(Hash2(F+vec2(1,1)),S-vec2(1,1))*M.z)*M.w+.5;
}
//2D Worley noise.
float Worley(vec2 P)
{
    float D = 1.;
	vec2 F = floor(P+.5);

    //Find the the nearest point the neigboring cells.
    D = min(length(.5*Hash2(F+vec2( 1, 1))+F-P+vec2( 1, 1)),D);
    D = min(length(.5*Hash2(F+vec2( 0, 1))+F-P+vec2( 0, 1)),D);
    D = min(length(.5*Hash2(F+vec2(-1, 1))+F-P+vec2(-1, 1)),D);
    D = min(length(.5*Hash2(F+vec2( 1, 0))+F-P+vec2( 1, 0)),D);
    D = min(length(.5*Hash2(F+vec2( 0, 0))+F-P+vec2( 0, 0)),D);
    D = min(length(.5*Hash2(F+vec2(-1, 0))+F-P+vec2(-1, 0)),D);
    D = min(length(.5*Hash2(F+vec2( 1,-1))+F-P+vec2( 1,-1)),D);
    D = min(length(.5*Hash2(F+vec2( 0,-1))+F-P+vec2( 0,-1)),D);
    D = min(length(.5*Hash2(F+vec2(-1,-1))+F-P+vec2(-1,-1)),D);
    return D;
}
//	Simplex 3D Noise 
//	by Ian McEwan, Ashima Arts
//
vec4 permute(vec4 x){return mod(((x*34.0)+1.0)*x, 289.0);}
vec4 taylorInvSqrt(vec4 r){return 1.79284291400159 - 0.85373472095314 * r;}

float Simplex(vec3 v)
{
	const vec2 C = vec2(1.0/6.0, 1.0/3.0);
	const vec4 D = vec4(0.0, 0.5, 1.0, 2.0);

	// First corner
	vec3 i  = floor(v + dot(v, C.yyy) );
	vec3 x0 =   v - i + dot(i, C.xxx) ;

	// Other corners
	vec3 g = step(x0.yzx, x0.xyz);
	vec3 l = 1.0 - g;
	vec3 i1 = min( g.xyz, l.zxy );
	vec3 i2 = max( g.xyz, l.zxy );

	//  x0 = x0 - 0. + 0.0 * C 
	vec3 x1 = x0 - i1 + 1.0 * C.xxx;
	vec3 x2 = x0 - i2 + 2.0 * C.xxx;
	vec3 x3 = x0 - 1. + 3.0 * C.xxx;

	// Permutations
	i = mod(i, 289.0 ); 
	vec4 p = permute( permute( permute( 
			  i.z + vec4(0.0, i1.z, i2.z, 1.0 ))
			+ i.y + vec4(0.0, i1.y, i2.y, 1.0 )) 
			+ i.x + vec4(0.0, i1.x, i2.x, 1.0 ));

	// Gradients
	// ( N*N points uniformly over a square, mapped onto an octahedron.)
	float n_ = 1.0/7.0; // N=7
	vec3  ns = n_ * D.wyz - D.xzx;

	vec4 j = p - 49.0 * floor(p * ns.z *ns.z);  //  mod(p,N*N)

	vec4 x_ = floor(j * ns.z);
	vec4 y_ = floor(j - 7.0 * x_ );    // mod(j,N)

	vec4 x = x_ *ns.x + ns.yyyy;
	vec4 y = y_ *ns.x + ns.yyyy;
	vec4 h = 1.0 - abs(x) - abs(y);

	vec4 b0 = vec4( x.xy, y.xy );
	vec4 b1 = vec4( x.zw, y.zw );

	vec4 s0 = floor(b0)*2.0 + 1.0;
	vec4 s1 = floor(b1)*2.0 + 1.0;
	vec4 sh = -step(h, vec4(0.0));

	vec4 a0 = b0.xzyw + s0.xzyw*sh.xxyy ;
	vec4 a1 = b1.xzyw + s1.xzyw*sh.zzww ;

	vec3 p0 = vec3(a0.xy,h.x);
	vec3 p1 = vec3(a0.zw,h.y);
	vec3 p2 = vec3(a1.xy,h.z);
	vec3 p3 = vec3(a1.zw,h.w);

	//Normalise gradients
	vec4 norm = taylorInvSqrt(vec4(dot(p0,p0), dot(p1,p1), dot(p2, p2), dot(p3,p3)));
	p0 *= norm.x;
	p1 *= norm.y;
	p2 *= norm.z;
	p3 *= norm.w;

	// Mix final noise value
	vec4 m = max(0.6 - vec4(dot(x0,x0), dot(x1,x1), dot(x2,x2), dot(x3,x3)), 0.0);
	m = m * m;
	return 42.0 * dot( m*m, vec4( dot(p0,x0), dot(p1,x1), 
								  dot(p2,x2), dot(p3,x3) ) );
}

layout(location = 0) out vec4 colour;

uniform sampler2D Texture;
uniform float Time;

in vec4 vColour;
in vec2 vUV;
in float vTextured;

void main()
{
	//float x = gl_FragCoord.x;
	//float y = gl_FragCoord.y;
	float x = vUV.x * 300.;
	float y = vUV.y * 300.;
	float scale = 0.1;
	//float r = Simplex(vec3(x + Time * 0.1, y + Time * 0.1) * scale);
	//float g = Simplex(vec3(x - Time * 0.1, y + Time * 0.1) * scale);
	//float b = Simplex(vec3(x, y - Time * 0.13) * scale);
	//p = random(vec2(x, y));
	//float p = (r + g + b) / 3.0;
	float n = Simplex(vec3(x, y, Time * 4.0) * scale);
	n = n * 0.5 + 0.5;
	colour = vec4(n, n, n, 1.0);
}