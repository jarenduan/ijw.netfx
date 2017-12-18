# ijw.netfx
A few nuget packages providing extensions and helpers for .Net Core and .Net Frameworks. 

# Repo structure
there're 3 folders in the repo:

  1. `src`: source codes.

  2. `pkg`: output nuget packages.  

  3. `test`: all test projects.

# src - Source codes
There's only one solution `ijw.netfx.sln`, where holds all the projects in sub-folders of `src` and `test`.

Most projects in `src` are cross-platform projects targeting NetStandard1.4, net35, net40 and net45. A few are only for net35, net40 or net45 above. `project.json`s in each project folder decribe the target.

# pkg - Packages
Projects in `src` folder are built into nuget pacakges. Use `src/packall.bat` to build all packages into `pkg` folder.

# test - Tests
Testing projects are also in `ijw.netfx.sln`.
