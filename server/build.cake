#tool nuget:?package=NuGet.CommandLine&version=6.3.1

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

var buildOutputFolder = "./build";

var solutionName = "./DevsWeb.sln";

Setup(context=>{
    Information($"{configuration}");
    EnsureDirectoryExists(buildOutputFolder);
});

Task("Clean")
    .Does(()=>{
        CleanDirectories("./src/**/obj/*");
        CleanDirectories("./src/**/bin/*");
    });

Task("Restore-Packages")
    .Does(()=>{
        NuGetRestore(solutionName);
    });

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore-Packages")
    .Does(()=>{
        MSBuild(solutionName, cfg=>{
            cfg.SetConfiguration(configuration)
            .WithProperty("SitecoreWebUrl","") //Overwrite the SitecoreWebUrl, so the TDS projects won't be sync
            .SetMSBuildPlatform(MSBuildPlatform.Automatic)
            .SetPlatformTarget(PlatformTarget.MSIL)
            .WithRestore();
        });
    });

Task("Collect")
    .Does(()=>{
        CleanDirectory(buildOutputFolder);

        //Copy config files
        var appConfigs = GetDirectories("./src/**/App_Config");
        foreach (var item in appConfigs)
        {
            CopyDirectory(item, $"{buildOutputFolder}/App_Config");
        }

        //Copy Dlls
        EnsureDirectoryExists($"{buildOutputFolder}/bin");
        var binFolders = GetDirectories("./src/**/Bin");
        foreach(var item in binFolders){
            var withConfiguration = $"{item}/{configuration}";
            var files = DirectoryExists(withConfiguration)? GetFiles($"{withConfiguration}/DevsWeb*.*"): GetFiles($"{item}/DevsWeb*.*");
            foreach(var file in files){
                Information(file);
                CopyFile(file, $"{buildOutputFolder}/bin/{file.GetFilename()}");
            }
        }

        //Copy additional files
        EnsureDirectoryExists($"{buildOutputFolder}/sitecore/shell");
        var sitecoreFolders = GetDirectories("./src/**/sitecore/shell");
        foreach (var item in sitecoreFolders)
        {
            CopyDirectory(item, $"{buildOutputFolder}/sitecore/shell");
        }
    });

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("Collect")
    .Does(()=>{
        Information("DONE");
    });

RunTarget(target);