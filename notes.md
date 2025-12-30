# Mountain Sim Ideas


## todo 

- fix shader multiple texture issue 
- finish presets menu
- fix texturing and json saving bug
- finish misc menu 

# future ideas 

This is a list of features I want to have in my mountain simulator

### feature generation
use noise to generate trees and maybe rocks or other features. Allow for density, height/gradient restrictions
for example if the feature is ver tall or steep, no trees should spawn

# optimizations
only implement if more performance is needed

### move all cpu generation to the gpu

### simplex noise
This is a faster algorithm that produces similar results to perlin noise

### lod/mesh decimation
This is a way to reduce the number of triangles and verticies in the mesh. You can have high detail when nearby, and low detail when far away.

# future ideas
these ideas will be implemented if I pick up the project again in the future.

### erosion simulation
Simulate erosion to make mountains even more real.

### chunk system
Allow for user to infinietly explore using a chunking system. 

### experimenting different noise
I am curious to know what other noise algorithms could look like.
