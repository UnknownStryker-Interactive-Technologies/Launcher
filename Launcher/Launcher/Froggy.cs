

namespace Launcher;


[Serializable]
public class EngineInfo
{
    public string? Version { get; set; }
    public string? InstallationPath { get; set; }
}

[Serializable]
public struct Froggy
{
    public EngineInfo? EngineInfo { get; set; }
    public string? ProjectType { get; set; }
    public string? ProjectName { get; set; }
    public string? ProjectPath { get; set; }
}
