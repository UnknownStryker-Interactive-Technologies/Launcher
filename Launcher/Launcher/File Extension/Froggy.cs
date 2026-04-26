namespace Launcher;




[Serializable]
public class WorldPath
{
    public string Name { get; set; } = String.Empty;
    public string Path { get; set; } = String.Empty;
}


[Serializable]
public class EngineInfo
{
    public string Version { get; set; } = String.Empty;
    public string InstallationPath { get; set; } = String.Empty;
}

[Serializable]
public class GlobalResourceLookUpTable
{
    public string EntryWorldPath { get; set; } = String.Empty;

    public List<WorldPath> WorldPaths { get; set; } = new();
}

[Serializable]
public class ProjectInfo
{
    public string ProjectType { get; set; } = String.Empty;
    public string ProjectName { get; set; } = String.Empty;
    public string ProjectPath { get; set; } = String.Empty;
}

[Serializable]
public class ProjectConfig
{
    public GlobalResourceLookUpTable GlobalResourceLookUpTable { get; set; } = new();

    public string CompressionMethod { get; set; } = String.Empty;
    public string DecompressionMethod { get; set; } = String.Empty;

    public string EncryptionMethod { get; set; } = String.Empty;
    public string DecryptionMethod { get; set; } = String.Empty;

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
    public string Title { get; set; } = String.Empty;
    public string[] IconPaths { get; set; } = { "Assets\\Icon\\runtime icon.png" };
    public string[] RandomPlayIntroVideoPaths { get; set; } = { "Assets\\Video\\runtime splash video 1.mp4", "Assets\\Video\\runtime splash video 2.mp4" };
    public string[] SequentialPlayIntroVideoPaths { get; set; } = Array.Empty<string>();
    public UInt16 SwapChainBufferCount { get; set; } = 3;
}




[Serializable]
public class ShaderDefines
{
    public string Identifier { get; set; } = String.Empty;
    public string Value { get; set; } = String.Empty;
}

[Serializable]
public class Shader
{
    public string Source { get; set; } = String.Empty;
    public string MainFunction { get; set; } = String.Empty;
    public string ShaderTarget { get; set; } = String.Empty;
    public List<ShaderDefines> Defines { get; set; } = new();
}




[Serializable]
public class Froggy // .froggy
{
    public EngineInfo EngineInfo { get; set; } = new();
    public ProjectInfo ProjectInfo { get; set; } = new();

    public ProjectConfig ProjectConfig { get; set; } = new();
    public List<Shader> Shaders { get; set; } = new();
}


// .fmodule, .ao, .flevel, .fasset, .oplan
