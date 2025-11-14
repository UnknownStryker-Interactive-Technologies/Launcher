



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
    public string? EntryWorldPath { get; set; }

    public List<Tuple<string?, string?>> WorldPaths { get; set; } = new();
    public List<Tuple<string?, string?>> ModulePaths { get; set; } = new();
}

[Serializable]
public class ProjectInfo
{
    public string? ProjectType { get; set; }
    public string? ProjectName { get; set; }
    public string? ProjectPath { get; set; }
}

[Serializable]
public class ProjectConfig
{
    public GlobalResourceLookUpTable GlobalResourceLookUpTable { get; set; } = new();

    public string? CompressionMethod { get; set; }
    public string? DecompressionMethod { get; set; }

    public string? EncryptionMethod { get; set; }
    public string? DecryptionMethod { get; set; }

    public uint MaxEngineEntities { get; set; } = 1024;
    public uint MaxEngineComponentTypeCountHint { get; set; } = 1024;
    public uint MaxEngineSystemCountHint { get; set; } = 1024;

    public uint GCIterationsPerFrame { get; set; } = 30;
    public uint FramePerReachabilityAnalysis { get; set; } = 60;
    public uint FiberStackSize { get; set; } = 1048576; // 1 MiB
    public uint FibersPerThread { get; set; } = 3;

    public WindowConfig WindowConfig { get; set; } = new();
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

    public ProjectConfig ProjectConfig { get; set; } = new();
}


// .fmodule, .ao, .flevel, .fasset, .oplan
