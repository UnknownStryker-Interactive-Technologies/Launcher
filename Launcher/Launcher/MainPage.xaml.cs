using System.Collections;
using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Win32;
/*
 The MIT License

Copyright (c) 2025 by UNKNOWN STRYKER

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
 */

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238


namespace Launcher;


/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainPage : Page
{
    private Uri _news = new("https://savory-moth-a00.notion.site/News-Updates-27a5fa4fb82e800eb173d5ba463171c0?source=copy_link");
    private Uri _docs = new("https://savory-moth-a00.notion.site/Frogman-Engine-1735fa4fb82e800e8fccc8df394eec5b");
    private List<EngineInfo> _engineVersions = new();
    private List<Froggy> _projects = new();


    public MainPage()
    {
        this.InitializeComponent();

        _docsWebView.Visibility = Visibility.Collapsed;
        _docsWebView.IsEnabled = false;

        RefreshEngineVersionList();
        RefreshProjectList();
        RegisterFroggyFileExtension(Path.Combine(AppContext.BaseDirectory, "icon.ico"));

        EnableEnginePage();
    }


    public void RegisterFroggyFileExtension(string iconPath)
    {
        try
        {
            const string progId = "FrogmanEngine.FroggyFileExtension";

#pragma warning disable CA1416 // Validate platform compatibility 
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\" + ".froggy", "", progId); // Associate extension with ProgID
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\" + progId, "", "A Frogman Engine project file"); // Set description
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\" + progId + @"\DefaultIcon", "", iconPath); // Set default icon
#pragma warning restore CA1416 // Validate platform compatibility
        }
        catch (Exception e)
        {
            Environment.FailFast(e.Message);
        }
    }


    // it may be has to be ran in parallel to handle greater quantity 
    private void RefreshProjectList()
    {
        _projects.Clear();

        string projectListPath = Path.Combine(AppContext.BaseDirectory, "froggies.flist");
        if (File.Exists(projectListPath) is false)
        {
            File.WriteAllText(projectListPath, "");
            return;
        }
        string projectList = File.ReadAllText(projectListPath);

        string?[] projectPaths = projectList.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        string newProjectList = "";
        for (uint i = 0; i < projectPaths.Length; ++i)
        {
            Debug.Assert(String.IsNullOrEmpty(projectPaths[i]) is false);
            if (File.Exists(projectPaths[i]) is false)
            {
                projectPaths[i] = null;
                continue;
            }

            string json = File.ReadAllText(projectPaths[i]!);
            Froggy? project = JsonSerializer.Deserialize<Froggy>(json);
            if (project is null)
            {
                projectPaths[i] = null;
                continue;
            }

            _projects.Add(project);
            newProjectList += projectPaths[i] + ";";
        }
        File.WriteAllText(projectListPath, newProjectList);
    }


    private void RefreshEngineVersionList()
    {
        _engineVersions.Clear();
        foreach (DictionaryEntry engine in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine))
        {
            string? engineInstallationPathEnvName = engine.Key.ToString();
            string? engineInstallationPathEnvValue = engine.Value?.ToString();
            if (String.IsNullOrEmpty(engineInstallationPathEnvName) || String.IsNullOrEmpty(engineInstallationPathEnvValue))
            {
                continue;
            }

            if (engineInstallationPathEnvName.StartsWith("FROGMAN_GDK_") &&
                engineInstallationPathEnvName.EndsWith("_PATH"))
            {
                EngineInfo engineVersion = new();
                int versionStringIndex = "FROGMAN_GDK_".Length;
                int versionStringEndIndex = engineInstallationPathEnvName.LastIndexOf("_PATH");
                int stringLength = versionStringEndIndex - versionStringIndex;
                engineVersion.Version = engineInstallationPathEnvName.Substring(versionStringIndex, stringLength);
                engineVersion.InstallationPath = engineInstallationPathEnvValue;

                _engineVersions.Add(engineVersion);
            }
        }
    }




    private void CollapsEnginePage()
    {
        _borderOnTheTop.Visibility = Visibility.Collapsed;
        _launchEngineInstaller.Visibility = Visibility.Collapsed;
        _border.Visibility = Visibility.Collapsed;
        _textEngineVersions.Visibility = Visibility.Collapsed;
        _textProjects.Visibility = Visibility.Collapsed;
        _buttonNewProject.Visibility = Visibility.Collapsed;
        _engineVersionGrid.Visibility = Visibility.Collapsed;
        _scrollableProjectList.Visibility = Visibility.Collapsed;
    }

    private void EnableEnginePage()
    {
        _borderOnTheTop.Visibility = Visibility.Visible;
        _launchEngineInstaller.Visibility = Visibility.Visible;
        _border.Visibility = Visibility.Visible;
        _textEngineVersions.Visibility = Visibility.Visible;
        _textProjects.Visibility = Visibility.Visible;
        _buttonNewProject.Visibility = Visibility.Visible;
        _engineVersionGrid.Visibility = Visibility.Visible;
        _scrollableProjectList.Visibility = Visibility.Visible;
    }


    private void OnClickNews(object sender, RoutedEventArgs e)
    {
        _docsWebView.IsEnabled = true;
        _docsWebView.Visibility = Visibility.Visible;

        _docsWebView.Source = _news;

        CollapsEnginePage();
    }

    private void OnClickEngine(object sender, RoutedEventArgs e)
    {
        _docsWebView.Visibility = Visibility.Collapsed;
        _docsWebView.IsEnabled = false;

        RefreshProjectList();
        EnableEnginePage();
    }

    private void OnClickGuide(object sender, RoutedEventArgs e)
    {
        _docsWebView.IsEnabled = true;
        _docsWebView.Visibility = Visibility.Visible;

        _docsWebView.Source = _docs;

        CollapsEnginePage();
    }




    private const string _engineInstallerLatestReleaseUrl = "https://api.github.com/repos/UnknownStryker-Interactive-Technology/Installer/releases/latest";
    string _installationPath = Path.Combine(AppContext.BaseDirectory, "Installer.Windows.11.Edition");
    private bool _isClicked = false;
    private void OnClickLaunchEngineInstaller(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_isClicked is true)
            {
                return;
            }
            _isClicked = true;
            RefreshEngineVersionList();

            if ( Directory.Exists(_installationPath) is true) // Always fetch the latest installer.
            {
                Directory.Delete(_installationPath, true);
            }

            using HttpClient client = new();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Frogman-Engine-Launcher", "1.0"));

            using HttpResponseMessage response = client.GetAsync(_engineInstallerLatestReleaseUrl).Result;
            if (response.IsSuccessStatusCode is false)
            {
                throw new Exception("Failed to fetch the latest release information.");
            }

            string json = response.Content.ReadAsStringAsync().Result;
            JsonNode? jsonNode = JsonNode.Parse(json);
            if (jsonNode is null)
            {
                throw new Exception("The assets property is missing in the JSON response.");
            }

            jsonNode = jsonNode["assets"];
            if (jsonNode is null)
            {
                throw new Exception("The assets property is missing in the JSON response.");
            }


            string? downloadURL;
            foreach (JsonNode? asset in jsonNode.AsArray())
            {
                if (asset is null)
                {
                    continue;
                }

                JsonNode? releaseJSON = asset["name"];
                if (releaseJSON is null)
                {
                    continue;
                }

                if (releaseJSON.ToString().Contains(".zip") is false)
                {
                    continue;
                }


                downloadURL = asset["browser_download_url"]?.ToString();
                if (string.IsNullOrEmpty(downloadURL))
                {
                    throw new Exception("The zipball_url property is missing or empty in the JSON response.");
                }


                using HttpResponseMessage downloadRequestResponse = client.GetAsync(downloadURL).Result;
                using Stream responseStream = downloadRequestResponse.Content.ReadAsStream();

                string downloadPath = _installationPath + ".zip";
                using FileStream fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None);

                responseStream.CopyTo(fileStream);

                fileStream.Close();
                responseStream.Close();

                ZipFile.ExtractToDirectory(downloadPath, _installationPath);
                File.Delete(downloadPath);
            }

            string _appName = "\0";
            foreach (string file in Directory.GetFiles(_installationPath))
            {
                if (file.EndsWith(".exe"))
                {
                    _appName = Path.GetFileName(file);
                    break;
                }
            }
            Debug.Assert(String.IsNullOrEmpty(_appName) is not true);

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(AppContext.BaseDirectory, Path.Combine(_installationPath, _appName)),
                UseShellExecute = true,
                Verb = "runas"
            };

            Process.Start(processStartInfo);
            _isClicked = false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"An error occurred while trying to launch the installer. Please try again later.\n\nError details: {ex.Message}");
            _isClicked = false;
        }
    }




#if !DEBUG
    private bool _isProjectGeneratorWindowOpened = false;
    private Window? _projectGeneratorWindow;
    private Frame _projectGeneratorWindowFrame = new();
#endif
    private void OnClickNewProject(object sender, RoutedEventArgs e)
    {
        if (_selectedEngineVersion is null)
        {
            return;
        }
#if DEBUG
        Frame.Navigate(typeof(ProjectGeneratorPage), _selectedEngineVersion);
#else
        if (_isProjectGeneratorWindowOpened is true)
        {
            return;
        }
        _isProjectGeneratorWindowOpened = true;

        _projectGeneratorWindow = new();
        _projectGeneratorWindow.Content = _projectGeneratorWindowFrame;
        _projectGeneratorWindow.Activate();
        _projectGeneratorWindowFrame.Navigate(typeof(ProjectGeneratorPage), _selectedEngineVersion);

        _projectGeneratorWindow.Closed += (s, e) => 
        {
            _isProjectGeneratorWindowOpened = false;
            _projectGeneratorWindow = null;
        };
#endif
    }


    private EngineInfo? _selectedEngineVersion = null;
    private void OnClickEngineVersion(object sender, ItemClickEventArgs e)
    {
        Debug.Assert(e.ClickedItem is EngineInfo);

        _selectedEngineVersion = (EngineInfo)e.ClickedItem;
    }




    private Froggy? _selectedProject = null;
    private void OnSelectProject(object sender, ItemClickEventArgs e)
    {
        Debug.Assert(e.ClickedItem is Froggy);
        _selectedProject = (Froggy)e.ClickedItem;
        Debug.Assert(_selectedProject is not null);

        RefreshProjectList();
        if (Directory.Exists(_selectedProject.ProjectInfo.ProjectPath) is false)
        {
            return;
        }
        Debug.Assert(_selectedProject.ProjectInfo.ProjectPath is not null);
        Process.Start("explorer.exe", _selectedProject.ProjectInfo.ProjectPath);
    }




    private void OnClickSignOut(object sender, RoutedEventArgs e)
    {
        Debug.Assert(Window.Current is not null);
        Window.Current.Close();
    }
}
