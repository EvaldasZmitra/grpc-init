using System.Globalization;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.Codecov;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.Octopus;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Build;

[GitHubActions(
    "ci",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.Push },
    ImportSecrets = new[] { "NugetServerKey", "CodecovKey" },
    FetchDepth = 0
)]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.BuildSolution);

    [Parameter("Nuget Server URL to push nuget to.")]
    private readonly string _nugetServerUrl;

    [Parameter("Nuget Server API Key")]
    [Secret]
    private readonly string _nugetServerKey;

    [Parameter("CodeCov Key")]
    [Secret]
    private readonly string _codecovKey;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration _configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution]
    private readonly Solution _solution;

    [GitVersion]
    readonly GitVersion _gitVersion;

    [GitRepository]
    readonly GitRepository _gitRepository;

    private AbsolutePath _nugetsDirectory => RootDirectory / "artifacts";
    private AbsolutePath _testsDirectory => _nugetsDirectory / "tests";
    private readonly string[] _projectsToCreateNugetsFrom =
    {
        "GrpcInit.Console"
    };

    Target Clean => _ => _
        .Executes(() => EnsureCleanDirectory(_nugetsDirectory));

    Target Test => _ => _
        .DependsOn(BuildSolution)
        .Triggers(CreateNugets)
        .Executes(() =>
        {
            foreach (var unitTestProject in _solution.AllProjects.Where(p => p.Name.EndsWith(".Tests", true, CultureInfo.InvariantCulture)))
            {
                var results = DotNetTest(
                    s => s
                        .SetNoBuild(true)
                        .SetDataCollector("XPlat Code Coverage")
                        .EnableCollectCoverage()
                        .SetResultsDirectory(_testsDirectory)
                        .SetConfiguration(_configuration)
                        .SetProjectFile(unitTestProject));
            }
        });

    Target UploadCoverage => _ => _
        .OnlyWhenStatic(() => !IsLocalBuild)
        .Executes(() =>
        {
            foreach (var file in _testsDirectory.GlobFiles("**/*"))
            {
                CodecovTasks.Codecov(
                    s => s
                        .SetFramework("net5.0")
                        .SetFiles(file)
                        .SetToken(_codecovKey)
                );
            }
        });

    Target BuildSolution => _ => _
        .DependsOn(Clean)
        .Triggers(Test)
        .Executes(() =>
        {
            DotNetBuild(x => x
                .SetProjectFile(_solution)
                .SetConfiguration(_configuration)
            );
        });

    Target CreateNugets => _ => _
        .Triggers(PushNugets)
        .OnlyWhenStatic(() => !IsLocalBuild && _gitRepository.IsOnMainOrMasterBranch())
        .DependsOn(Clean)
        .DependsOn(Test)
        .Executes(() =>
        {
            foreach (var projectName in _projectsToCreateNugetsFrom)
            {
                var pathToProject = $"src/{projectName}/{projectName}.csproj";
                DotNetPack(
                    s => s
                        .SetTitle("Grpc Init")
                        .AddAuthors("Evaldas Zmitra")
                        .SetDescription("Dotnet tool that gets nuget files via reflection endpoint.")
                        .SetRepositoryUrl("https://github.com/EvaldasZmitra/grpc-init")
                        .SetPackageId("grpc-init")
                        .SetNoBuild(true)
                        .SetRepositoryType("git")
                        .SetProject(pathToProject)
                        .SetConfiguration(_configuration)
                        .SetVersion(_gitVersion.NuGetVersion)
                        .SetOutputDirectory(_nugetsDirectory)
                );
            }
        });

    Target PushNugets => _ => _
        .OnlyWhenStatic(() => !IsLocalBuild && _gitRepository.IsOnMainOrMasterBranch())
        .Triggers(UploadCoverage)
        .Executes(() =>
        {
            foreach (var packagePath in _nugetsDirectory.GlobFiles("*.nupkg"))
            {
                DotNetNuGetPush(
                    s => s
                        .SetSource(_nugetServerUrl)
                        .SetSkipDuplicate(true)
                        .SetApiKey(_nugetServerKey)
                        .SetTargetPath(packagePath)
                );
            }
        });
}
