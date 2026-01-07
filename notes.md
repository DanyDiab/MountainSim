# Mountain Sim Ideas


## todo 

- Add Controls Menu
- Main Menu
- Toast message to display alerts
- loading symbol to show terrain is generating
- Camera Settings 
- Make Tex Load panel use a scroll view to be to display many more textures


## optimizations

### GPU generation
Move all CPU generation to the GPU to be done in parallel

### simplex noise
This is a faster algorithm that produces similar results to perlin noise

### lod/mesh decimation
This is a way to reduce the number of triangles and verticies in the mesh. You can have high detail when nearby, and low detail when far away.

## future ideas
these ideas will be implemented if I pick up the project again in the future.

### erosion simulation
Simulate erosion to make mountains even more real.

### chunk system
Allow for user to infinietly explore using a chunking system.

### feature generation
use noise to generate trees and maybe rocks or other features. Allow for density, height/gradient restrictions
for example if the feature is ver tall or steep, no trees should spawn

### biomes
use voronoi noise or other noise to generate biomes 

### weather
Use noise or other factors such as biome to drive weather. (Or maybe they can just be a parameter)

### experimenting different noise
I am curious to know what other noise algorithms could look like.
