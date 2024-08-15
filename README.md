<div align="center">
  <img alt="Hypercube" width="100%" height="360" src="header.svg"/>
</div>

[![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/en-us/download)
[![OpenGL Badge](https://img.shields.io/badge/OpenGL-5586A4?logo=opengl&logoColor=white&style=for-the-badge)](https://www.opengl.org/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

# About

Hypercube is a game engine/framework inspired by RobustToolbox, for creating cross-platform desktop games, with a predominant approach in ECS. 

> [!NOTE]
> Checkout the [Documentation](https://github.com/technologists-team/hypercube/wiki)

## Building
```
git clone https://github.com/Tornado-Technology/hypercube.git
git submodule update --init --recursive
dotnet restore
dotnet tool restore
dotnet run --configuration=Release --project=.\Hypercube.Example\
```

## License
All code for the repository is licensed under [MIT](https://github.com/Tornado-Technology/hypercube/blob/master/LICENSE).
