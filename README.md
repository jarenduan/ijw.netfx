# ijw.netfx
A few nuget packages providing extensions and helpers for .Net Core and .Net Frameworks. 

# Repro structure
there're 3 folders in the repro:

  1. `pkg`: output nuget packages.

  2. `src`: source codes.

  3. `test`: all test projects.

# Source codes
There's only one solution `ijw.netfx.sln`, where holds all the projects in sub-folders of `src` and `test`.

Most projects in `src` are cross-platform projects targeting NetStandard1.4, net35, net40 and net45. A few are only for net35, net40 or net45 above. `project.json`s in each project folder decribe the target.

# Packages
Projects in `src` folder are built into nuget pacakges. Use `src/packall.bat` to build all packages into `pkg` folder.

# Tests
Testing projects are also in `ijw.netfx.sln`.
