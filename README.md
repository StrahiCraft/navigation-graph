<h1>
 Navigation graph tool for Godot 4.6 C#
</h1>
<p>
 This is a tool made for the <a href="https://github.com/godotengine/godot/tree/master">Godot game engine</a> with C#
 for making simple navigation graphs for ai agents to use. The tool is designed to be used in 3D scenes, but could be changed pretty easily to 
 work with 2D ones. The tool also provides pathfinding between two given nodes.
</p>
<p>
 Pathfinding algorithms that are implemented:<br>
<ul>
  <li><a href="https://en.wikipedia.org/wiki/Depth-first_search">DFS</a> (Depth first search</li>
  <li><a href="https://en.wikipedia.org/wiki/Breadth-first_search">BFS</a> (Bredth first search)</li>
  <li><a href="https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm">Dijkstra</a></li>
  <li><a href="https://en.wikipedia.org/wiki/A*_search_algorithm">A*</a></li>
</ul>
</p>
<p>And since the pathfinding logic is in a seperate class, more can be added if needed.</p>
<h2>
 Using the tool
</h2>
<p>
 The tool consists of the navigation graph scene that has to be in the scene for the path nodes to be added.
</p>
<img src="repo_assets/tool_showcase.png" width=400px alt="Tool showcase">
<p>
 To start using the tool, after adding all the scripts, scenes, materials and textures from this project to yours, drag into the scene from "scenes/objects/pathfinding" the 
 navigation_graph.tscn scene.
</p>
<img src="repo_assets/adding_the_graph.gif" width=400px alt="Adding the graph">
<p>After the navigation graph has been added, it should teleport to the center of the scene.</p>
<p>
 With the graph in the scene, we can now start adding path nodes, they are found in the same folder as the navigation graph. Add nodes by draging them into the scene or copy pasting 
 already existing ones in the scene. The nodes automatically connect by raycasting to each other when spawning in.
</p>
<img src="repo_assets/adding_path_nodes.gif" width=400px alt="Adding path nodes">
<h2>
 Testing out pathfinding
</h2>
<p>
 Since there is no implementation for agents in this project, there are tools to help test out the pathfinding algorithms.
</p>
<img src="repo_assets/editor_tools.png" width=400px alt="Editor tools">
<p>
 These tools are found when selecting the navigation graph. The refresh tool simply refreshes the graph, regenerating connections and recalculating paths if needed.
 Path calculation type is where we choose the pathfinding algorithm to use for our testing purposes.
</p>
<img src="repo_assets/path_calculation_types.png" width=400px alt="Path calculation types">
<p>
 The calculate path button calculates a path between the starting and goal node that we give to the editor tool.
</p>
<img src="repo_assets/path_calculation.gif" width=400px alt="Path calculation showcase">
<p>
 <i>*Note - BFS, Dijkstra and A** usually have the same path, but the diffirence is the speed of calculation</i>
</p>
