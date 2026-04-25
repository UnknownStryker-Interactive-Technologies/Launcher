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

    public uint MaxEngineArchetypeCountHint { get; set; } = 1024;
    public uint MaxEngineComponentTypeCountHint { get; set; } = 1024;

    public uint GCIterationsPerFrame { get; set; } = 30;
    public uint FramesPerReachabilityAnalysis { get; set; } = 60;
    public uint FiberStackSize { get; set; } = 1048576; // 1 MiB
    public uint FibersPerThread { get; set; } = 3;

    public WindowConfig WindowConfig { get; set; } = new();
}

[Serializable]
public class WindowConfig
{
    public string? Title { get; set; }
    public string[] IconPaths { get; set; } = { "Assets\\Icon\\runtime icon.png" };
    public string[] RandomPlayIntroVideoPaths { get; set; } = { "Assets\\Video\\runtime splash video 1.mp4", "Assets\\Video\\runtime splash video 2.mp4" };
    public string[] SequentialPlayIntroVideoPaths { get; set; } = Array.Empty<string>();
    public UInt16 SwapChainBufferCount { get; set; } = 3;
}
// -enable-fullscreen -enable-vsync



[Serializable]
public class Froggy // .froggy
{
    public EngineInfo EngineInfo { get; set; } = new();
    public ProjectInfo ProjectInfo { get; set; } = new();

    public ProjectConfig ProjectConfig { get; set; } = new();
}


// .fmodule, .ao, .flevel, .fasset, .oplan
