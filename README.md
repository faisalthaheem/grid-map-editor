# Grid Map Editor
For robotics and simple Intelligent Transportation System projects, a utility to classify an image into a grid and mark obstacles.

I was unable to find a simple tool that would let me convert any image into a grid and then mark obstacles to let me run different motion planning / path finding algorithms on it, therefore I decided to write this tool.

It has limited functionality but saves all the data into JSON format to be consumed by any other applications.

As shown in the following short video - the red spots indicate an obstacle, while the blue spots indicate presence of a landmark, such as a bluetooth or wifi access point. This sensor data can be collected using the [Wifi Access Point Mapper](https://github.com/faisalthaheem/wifi-access-point-mapper) project.

![demo](https://cdn.rawgit.com/faisalthaheem/grid-map-editor/0ec3f0db/docs/grid-map-editor-completed-project.gif)

## Compiling and Running

Use Community edition of Microsoft Visual Studio 2017 to compile and run this tool.