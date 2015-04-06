// ARGUMENTS

var target          = Argument<string>("target", "All");
var configuration   = Argument<string>("configuration", "Release");

// GLOBAL VARIABLES

var solutions       = GetFiles("./**/*.sln");
var solutionDirs    = solutions.Select(solution => solution.GetDirectory());
var nugetToolPath   = "./packages/nuget.exe";
var outputDirectory = "./build/"+ configuration;

// TASK DEFINITIONS

Task("Clean")
    .Does(() =>
{
    // Clean solution directories.
    foreach(var solutionDir in solutionDirs)
    {
        Information("Cleaning {0}", solutionDir);
        CleanDirectories(solutionDir + "/**/bin/" + configuration);
        CleanDirectories(solutionDir + "/**/obj/" + configuration);
        CleanDirectories(solutionDir + "/**/build/" + configuration);
    }
});

Task("RestorePackages")
    .Does(() =>
{
    // Restore all NuGet packages.
    foreach(var solution in solutions)
    {
        Information("Restoring packages for {0}", solution);
        NuGetRestore(solution, new NuGetRestoreSettings { ToolPath = nugetToolPath });
    }
    // restore packages used as tools
    NuGetRestore("./tools/packages.config", new NuGetRestoreSettings { ToolPath = nugetToolPath });
});

Task("Compile")
    .IsDependentOn("Clean")
    .IsDependentOn("RestorePackages")
    .Does(() =>
{
    // Build all solutions.
    foreach(var solution in solutions)
    {
        Information("Building {0}", solution);
        MSBuild(solution, settings => 
          settings.SetConfiguration(configuration)
              .WithProperty("TreatWarningsAsErrors", "true")
              .UseToolVersion(MSBuildToolVersion.NET45)
              .SetNodeReuse(false));
    }
});

Task("PrepareOutputDirectory")
    .Does(() =>
{
    CreateDirectory(outputDirectory);
});

Task("PackageNuget")
    .IsDependentOn("Compile")
    .IsDependentOn("PrepareOutputDirectory")
    .Does(() =>
{
   StartProcess(nugetToolPath, new ProcessSettings() {
      Arguments = "pack " + "./src/nsemversion/nsemversion.csproj" + " -OutputDirectory " + outputDirectory
   });
});

Task("Test")
    .IsDependentOn("Compile")
    .Does(() =>
{
    var xunitPath = GetFiles("./packages/**/xunit.console.exe").FirstOrDefault();

    XUnit2("./test/**/bin/" + configuration + "/*.Test.dll", new XUnit2Settings() { ToolPath = xunitPath }); 
});

Task("TransformParser")
    .Description("Transform ragel parser")
    .Does(() =>
{
   StartProcess("./tools/ragel.exe", new ProcessSettings() {
      Arguments = "-A -G1 SemVersionParser.Ragel.rl",
      WorkingDirectory = "./src/NSemVersion"
   });
});

Task("All")
    .IsDependentOn("TransformParser")
    .IsDependentOn("Compile")
    .IsDependentOn("Test")
    .IsDependentOn("PackageNuget");
       
///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);