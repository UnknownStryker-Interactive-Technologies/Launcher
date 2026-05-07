namespace Launcher;




[Serializable]
public class WorldPath
{
    public string? Name { get; set; } = null;
    public string? Path { get; set; } = null;
}


[Serializable]
public class EngineInfo
{
    public string? Version { get; set; } = null;
    public string? InstallationPath { get; set; } = null;
}

[Serializable]
public class GlobalResourceLookUpTable
{
    public string? EntryWorldPath { get; set; } = null;

    public List<WorldPath> WorldPaths { get; set; } = new();
}

[Serializable]
public class ProjectInfo
{
    public string? ProjectType { get; set; } = null;
    public string? ProjectName { get; set; } = null;
    public string? ProjectPath { get; set; } = null;
}

[Serializable]
public class ProjectConfig
{
    public GlobalResourceLookUpTable GlobalResourceLookUpTable { get; set; } = new();

    public string? CompressionMethod { get; set; } = null;
    public string? DecompressionMethod { get; set; } = null;

    public string? EncryptionMethod { get; set; } = null;
    public string? DecryptionMethod { get; set; } = null;
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
    public string[] ShaderCompileSplashImagePaths { get; set; } = { "Assets\\Splash\\shader compilation.png" };
    public uint ShaderCompileSplashImageDurationInSeconds { get; set; } = 0; // 0 is infinite
    public uint SwapChainBufferCount { get; set; } = 3;
}




//[Serializable]
//public class ShaderDefines
//{
//    public string Identifier { get; set; } = String.Empty;
//    public string Value { get; set; } = String.Empty;
//}

//[Serializable]
//public class Shader
//{
//    public string Source { get; set; } = String.Empty;
//    public string MainFunction { get; set; } = String.Empty;
//    public string ShaderTarget { get; set; } = String.Empty;
//    public List<ShaderDefines> Defines { get; set; } = new();
//}




[Serializable]
public class Froggy // .froggy
{
    public EngineInfo EngineInfo { get; set; } = new();
    public ProjectInfo ProjectInfo { get; set; } = new();

    public ProjectConfig ProjectConfig { get; set; } = new();
    // public List<Shader> Shaders { get; set; } = new();
}


// .fmodule, .ao, .flevel, .fasset, .oplan
