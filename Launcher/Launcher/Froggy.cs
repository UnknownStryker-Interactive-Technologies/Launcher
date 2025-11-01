



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
    public List<Tuple<string?, string?>> ModulePaths { get; set; } = new();
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
public class ProjectConfig
{
    public string? CompressionMethod { get; set; }
    public string? DecompressionMethod { get; set; }

    public string? EncryptionMethod { get; set; }
    public string? DecryptionMethod { get; set; }

    public uint MaxEntities { get; set; } = 10240;
    public uint MaxComponentTypeCountHint { get; set; } = 1024;
    public uint MaxSystemCountHint { get; set; } = 1024;

    public uint GCIterationsPerFrame { get; set; } = 30;
    public uint FiberStackSize { get; set; } = 1048576; // 1 MiB
    public uint FibersPerThread { get; set; } = 3;
}




[Serializable]
public class Froggy // .froggy
{
    public EngineInfo EngineInfo { get; set; } = new();
    public ProjectInfo ProjectInfo { get; set; } = new();
    public GlobalResourceLookUpTable GlobalResourceLookUpTable { get; set; } = new();
    public ProjectConfig ProjectConfig { get; set; } = new();
}


// .fmodule, .ao, .flevel, .fasset, .oplan
