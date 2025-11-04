



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
public class WindowConfig
{
    public string? Title { get; set; }
    public string? IconPath { get; set; }
    public Int32 MonitorIndex { get; set; } = 0;
    public bool ShouldEnableVSync { get; set; } = false;
    public bool IsAtopEverything { get; set; } = false;
    public bool ShouldScaleContentToMonitorDPI { get; set; } = true;
    public bool HasBorder { get; set; } = true;
    public UInt16 SwapChainBufferCount { get; set; } = 3;
    public bool IsVirtualReality { get; set; } = false;
    public bool ShouldEnableHDR { get; set; } = false;
     
    public Int32 Width { get; set; } = 0;
    public Int32 Height { get; set; } = 0;
    public bool IsResizable { get; set; } = true;
    public bool IsMaximized { get; set; } = true;
    public bool IsFullScreen { get; set; } = false;
}




[Serializable]
public class Froggy // .froggy
{
    public EngineInfo EngineInfo { get; set; } = new();
    public ProjectInfo ProjectInfo { get; set; } = new();
    public GlobalResourceLookUpTable GlobalResourceLookUpTable { get; set; } = new();
    public ProjectConfig ProjectConfig { get; set; } = new();
    public WindowConfig WindowConfig { get; set; } = new();
}


// .fmodule, .ao, .flevel, .fasset, .oplan
