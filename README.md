# NetCoreTemplate

This is a simple layout for creating a .NET Core library or application, mostly copied from the structure at dotnet/corefx.
Also includes some primitive build steps for making CoreCLR-based applications and putting a workable runtime folder.

**To Use:**

* Clone the repo
* Add some projects under src
* Build the project(s) or run build.cmd
* Projects with OutputType=Exe will produce a "Runtime" folder in the root of the repository, as well as a "Launch" command which can be used to launch the executable with CoreRun.
* 

**Exe Projects**
You will need to add appropriate NuGet references in project.json to create a working Runtime folder for the executable (as well as include all of the framework dependencies). A basic working runtime for x86 Windows needs:

* APISet forwarder assemblies (Microsoft.NETCore.Runtime.ApiSets-x86)
* Runtime assemblies (Microsoft.NETCore.Runtime.CoreCLR-x86)
* Runtime Host (Microsoft.NETCore.Runtime.CoreCLR.TestHost-x86)

The runtime folder creation logic will need much more work to support different platforms, architectures, etc. Right now it just takes whatever gets referenced from the csproj and project.json and puts it into the "*-Runtime" folder.
