<img align="right" src="logo.png"><p></p>

# VoltstroEngine
 
[![Build Status](https://dev.azure.com/Voltstro/VoltstroEngine/_apis/build/status/VoltstroEngine?branchName=master)](https://dev.azure.com/Voltstro/VoltstroEngine/_build/latest?definitionId=4&branchName=master)
[![Discord](https://img.shields.io/badge/Discord-Voltstro-7289da.svg)](https://discord.voltstro.dev) 
[![YouTube](https://img.shields.io/badge/Youtube-Voltstro-red.svg?logo=youtube)](https://www.youtube.com/Voltstro)

Voltstro Engine is C#, (currently only) 2D game engine, using (also currently only) OpenGL.

Please note that:

- This is made by a single hobbyist/student in their spare time
- This is still in VERY early development, you shouldn't use this in a production game

# Features

Here are some features the VoltstroEngine supports, obviously, things will change and new features will be added.

- Completely done in C#
- Uses .NET Core 3.1
- One copy of the engine can run multiple copies of the game (like how sourcemods work)
- Easy to make a game using the engine

# Building

## Prerequisites

You will need:

- [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- An IDE
    - Solution/Workspace files for [Visual Studio](https://visualstudio.microsoft.com/), [Rider](https://www.jetbrains.com/rider/) and [VS Code](https://code.visualstudio.com/) are provided, ready to go
    - Visual Studio 2019 the recommend IDE to use, if you have a platform compatible

## Compiling

Once you have your prerequisites sorted out, go to `src/` and open up `VoltstroEngine.sln` (or `VoltstroEngine.code-workspace` with VS code).

Once the solution/workspace is opened, build the ENTIRE solution, you can do this by right clicking on the solution, and clicking 'Build Solution', or in VS Code by running the `build` task provided.

The built engine will be placed into the `game/bin/(Configuration)/` directory.

## Were from now?

There are some guides (such as how to create a game) on the [wiki](https://github.com/Voltstro/VoltstroEngine/wiki).

# Authors

- [Voltstro](https://github.com/Voltstro) - *Initial work*

# License

This project is licensed under the Apache-2.0 License - see the [LICENSE](LICENSE) file for details.

# Special Thanks

[EternalClickbait](https://github.com/EternalClickbait) - For help on some maths stuff

[TheCherno](https://github.com/TheCherno) - For his amazing [Game Engine series](https://www.youtube.com/playlist?list=PLlrATfBNZ98dC-V-N3m0Go4deliWHPFwT) (you will notice some similarities between his engine, and mine)