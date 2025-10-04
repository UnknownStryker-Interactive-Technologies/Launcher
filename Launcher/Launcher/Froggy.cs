



namespace Launcher;




[Serializable]
public class EngineInfo
{
    public string? Version { get; set; }
    public string? InstallationPath { get; set; }
}

[Serializable]
public class GlobalResourceLookUpTable
{
    public List<Tuple<string?, string?>> WorldPaths { get; set; } = new();
    public List<Tuple<string?, string?>> LevelPaths { get; set; } = new();
    public List<Tuple<string?, string?>> AssetPaths { get; set; } = new();
}

[Serializable]
public class ProjectInfo
{
    public string? ProjectType { get; set; }
    public string? ProjectName { get; set; }
    public string? ProjectPath { get; set; }

    public string? EntryWorldPath { get; set; }
}




[Serializable]
public class Froggy // .froggy
{
    public EngineInfo EngineInfo { get; set; } = new();
    public ProjectInfo ProjectInfo { get; set; } = new();
    public GlobalResourceLookUpTable GlobalResourceLookUpTable { get; set; } = new();

    public List<Tuple<string?, string?>> ModulePaths { get; set; } = new();

    public string? CompressionMethod { get; set; }
    public string? EncryptionMethod { get; set; }
}




[Serializable]
public class FModule // .fmodule
{
    public string? ModuleName { get; set; }
    public string? ModulePath { get; set; }

    public List<string?> ReflectiveMethods { get; set; } = new();
}


[Serializable]
public class AO // .ao
{
    public string? AOName { get; set; }
    public string? AOPath { get; set; }

    public string? InfilPointPath { get; set; }
    public List<Tuple<string?, string?>> LevelPaths { get; set; } = new();
    public List<Tuple<string?, string?>> AssetPaths { get; set; } = new();
}


[Serializable]
public class FLevel // .flevel
{
    public string? LevelName { get; set; }
    public string? LevelPath { get; set; }

    public List<Tuple<string?, string?>> AssetPaths { get; set; } = new();
}


[Serializable]
public class FAsset // .fasset
{
    public string? AssetName { get; set; }
    public string? AssetPath { get; set; }

    public string? AssetType { get; set; }
    public List<Tuple<string?, string?>> AssetComponentPaths { get; set; } = new();
}
