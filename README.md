# NetCoreTemplate

This is a simple layout for creating a .NET Core library or application, mostly copied from the structure at dotnet/corefx.
Also includes some very primitive build steps for making CoreCLR-based applications and putting a workable runtime folder.

**To Use:**

* Clone the repo
* Add some projects under src
  *  See the existing sample project for a good starting point
* Build the project(s) or run build.cmd
* Projects with OutputType=Exe will produce a "Runtime" folder in the root of the repository, as well as a "Launch" command which can be used to launch the executable with CoreRun.