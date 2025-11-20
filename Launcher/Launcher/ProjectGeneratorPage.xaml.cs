using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using static Uno.WinRTFeatureConfiguration.Storage;
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


[Serializable]
public enum ProjectType
{
    cli,
    game,
    dll,
    lib,
    @null
}
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
/// 
public sealed partial class ProjectGeneratorPage : Page
{
    private ProjectType _projectType = ProjectType.@null;
    private EngineInfo? _selectedEngineInfo = null;




    public ProjectGeneratorPage()
    {
        this.InitializeComponent();
        _fileOpenPicker.FileTypeFilter.Add(".froggy");
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        Debug.Assert(e.Parameter is EngineInfo);
        _selectedEngineInfo = (EngineInfo)e.Parameter;

        Debug.Assert(_selectedEngineInfo is not null);
        _selectedEngineVersion.Text = _selectedEngineInfo.Version;
    }




    private void OnSelectEXE(object sender, RoutedEventArgs e)
    {
        _selectedEXE.IsChecked = true;
        _selectedGame.IsChecked = false;
        _selectedDLL.IsChecked = false;
        _selectedLIB.IsChecked = false;
        _projectType = ProjectType.cli;
        _selectedProjectType.Source = _exeImage.Source;
    }

    private void OnSelectGame(object sender, RoutedEventArgs e)
    {
        _selectedGame.IsChecked = true;
        _selectedEXE.IsChecked = false;
        _selectedDLL.IsChecked = false;
        _selectedLIB.IsChecked = false;
        _projectType = ProjectType.game;
        _selectedProjectType.Source = _gameImage.Source;
    }

    private void OnSelectDLL(object sender, RoutedEventArgs e)
    {
        _selectedDLL.IsChecked = true;
        _selectedEXE.IsChecked = false;
        _selectedGame.IsChecked = false;
        _selectedLIB.IsChecked = false;
        _projectType = ProjectType.dll;
        _selectedProjectType.Source = _dllImage.Source;
    }

    private void OnSelectLIB(object sender, RoutedEventArgs e)
    {
        _selectedLIB.IsChecked = true;
        _selectedEXE.IsChecked = false;
        _selectedGame.IsChecked = false;
        _selectedDLL.IsChecked = false;
        _projectType = ProjectType.lib;
        _selectedProjectType.Source = _libImage.Source;
    }




    FileOpenPicker _fileOpenPicker = new Windows.Storage.Pickers.FileOpenPicker();
    private async void OnClickSelectFroggy(object sender, RoutedEventArgs e)
    {
        var file = await _fileOpenPicker.PickSingleFileAsync();
        if (file == null)
        {
            return;
        }

        _existingProjectPath.Text = file.Path;
    }


    FolderPicker _folderPicker = new Windows.Storage.Pickers.FolderPicker();
    private async void OnClickSelectFolder(object sender, RoutedEventArgs e)
    {
        var folder = await _folderPicker.PickSingleFolderAsync();
        if (folder == null)
        {
            return;
        }

        _newProjectDirectory.Text = folder.Path;
    }

    private void OnClickAdd(object sender, RoutedEventArgs e)
    {
        string projectListPath = Path.Combine(AppContext.BaseDirectory, "froggies.flist");
        string contents = File.ReadAllText(projectListPath);
        if (contents.Contains(_existingProjectPath.Text))
        {
            _errorText.Text = "The project already exists in the project list.";
            return;
        }
        contents += _existingProjectPath.Text + ';';
        File.WriteAllText(projectListPath, contents);
    }




    private string appDotHpp = "/*\r\nCopyright © from 2024 to present, UNKNOWN STRYKER. All Rights Reserved.\r\n\r\nLicensed under the Frogman Engine Apache License (the \"License\");\r\nyou may not use this file except in compliance with the License.\r\nYou may obtain a copy of the License at\r\n\r\n\thttps://github.com/UnknownStryker-Interactive-Technology/Frogman-Engine-Apache-License/blob/release/LICENSE.md\r\n\r\nUnless required by applicable law or agreed to in writing, software\r\ndistributed under the License is distributed on an \"AS IS\" BASIS,\r\nWITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.\r\nSee the License for the specific language governing permissions and\r\nlimitations under the License.\r\n*/" +
        "\r\n#include <FE/framework.hxx>\r\n\r\n\r\n" +
        "class cli_application : public FE::framework::framework_base\r\n" +
        "{\r\n" +
        "public:" +
        "\r\n\tcli_application(FE::int32 argc_p, FE::ASCII** argv_p) noexcept = default;" +
        "\r\n\t~cli_application() noexcept override = default;\r\n" +
        "\r\n\tvirtual FE::int32 launch(FE::int32 argc_p, FE::ASCII** argv_p) override;" +
        "\r\n\tvirtual FE::int32 run() override;" +
        "\r\n\tvirtual FE::int32 shutdown() override;" +
        "\r\n};";
    private string appDotCpp = "/*\r\nCopyright © from 2024 to present, UNKNOWN STRYKER. All Rights Reserved.\r\n\r\nLicensed under the Frogman Engine Apache License (the \"License\");\r\nyou may not use this file except in compliance with the License.\r\nYou may obtain a copy of the License at\r\n\r\n\thttps://github.com/UnknownStryker-Interactive-Technology/Frogman-Engine-Apache-License/blob/release/LICENSE.md\r\n\r\nUnless required by applicable law or agreed to in writing, software\r\ndistributed under the License is distributed on an \"AS IS\" BASIS,\r\nWITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.\r\nSee the License for the specific language governing permissions and\r\nlimitations under the License.\r\n*/" +
        "\r\n#include <app.hpp>\r\n\r\n" +
        "\r\nFE::int32 cli_application::launch(FE::int32 argc_p, FE::ASCII** argv_p)" +
        "\r\n{" +
        "\r\nreturn 0;" +
        "\r\n};\r\n\r\n" +
        "FE::int32 cli_application::run()" +
        "\r\n{" +
        "\r\nreturn 0;" +
        "\r\n};\r\n\r\n" +
        "FE::int32 cli_application::shutdown()" +
         "\r\n{" +
        "\r\nreturn 0;" +
        "\r\n};\r\n\r\n" +
        "CUSTOM_ENGINE(cli_application);";


    private string mainDotCpp = "/*\r\nCopyright © from 2024 to present, UNKNOWN STRYKER. All Rights Reserved.\r\n\r\nLicensed under the Frogman Engine Apache License (the \"License\");\r\nyou may not use this file except in compliance with the License.\r\nYou may obtain a copy of the License at\r\n\r\n\thttps://github.com/UnknownStryker-Interactive-Technology/Frogman-Engine-Apache-License/blob/release/LICENSE.md\r\n\r\nUnless required by applicable law or agreed to in writing, software\r\ndistributed under the License is distributed on an \"AS IS\" BASIS,\r\nWITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.\r\nSee the License for the specific language governing permissions and\r\nlimitations under the License.\r\n*/" +
        "\r\n#include <FE/framework.hxx>\r\n#include <FE/engine.hpp>\r\nFROGMAN_ENGINE();";


    private void OnClickCreate(object sender, RoutedEventArgs e)
    {
        if (String.IsNullOrEmpty(_newProjectDirectory.Text))
        {
            _errorText.Text = "Please select a valid directory.";
            _newProjectName.IsReadOnly = false;
            return;
        }
        if (String.IsNullOrEmpty(_newProjectName.Text))
        {
            _errorText.Text = "Please enter a valid project name.";
            _newProjectName.IsReadOnly = false;
            return;
        }
        if (_projectType is ProjectType.@null)
        {
            _errorText.Text = "Please select a project type.";
            _newProjectName.IsReadOnly = false;
            return;
        }
        

        _newProjectName.IsReadOnly = true;

        _progressBar.IsEnabled = true;
        _progressBar.Visibility = Visibility.Visible;
        _warningText.Visibility = Visibility.Visible;


        string newProjectDir = Path.Combine(_newProjectDirectory.Text, _newProjectName.Text);
        if (Directory.Exists(newProjectDir))
        {
            _errorText.Text = "The project name is already taken.";
            _newProjectName.IsReadOnly = false;

            _progressBar.IsEnabled = false;
            _progressBar.Visibility = Visibility.Collapsed;
            _warningText.Visibility = Visibility.Collapsed;
            return;
        }


        string pathToCMakeFolder = Path.Combine(newProjectDir, "CMake");
        string pathToIncludeFolder = Path.Combine(newProjectDir, "Include");
        string pathToSourceFolder = Path.Combine(newProjectDir, "Source");

        Directory.CreateDirectory(newProjectDir);
        Directory.CreateDirectory(Path.Combine(newProjectDir, "Binaries"));
        Directory.CreateDirectory(pathToCMakeFolder);
        Directory.CreateDirectory(pathToIncludeFolder);
        Directory.CreateDirectory(pathToSourceFolder);

        Froggy froggy = new();
        Debug.Assert(_selectedEngineInfo is not null);
        froggy.EngineInfo = _selectedEngineInfo;
        froggy.ProjectInfo.ProjectType = _projectType.ToString();
        froggy.ProjectInfo.ProjectName = _newProjectName.Text;
        froggy.ProjectInfo.ProjectPath = newProjectDir;
        string jsonFlavoredFroggy = System.Text.Json.JsonSerializer.Serialize<Froggy>(froggy, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

        string froggyFileName = _newProjectName.Text + ".froggy";
        string froggyFilePath = Path.Combine(newProjectDir, froggyFileName);
        File.WriteAllText(froggyFilePath, jsonFlavoredFroggy);


        string projectListPath = Path.Combine(AppContext.BaseDirectory, "froggies.flist");
        if (File.Exists(projectListPath) is not true)
        {
            File.WriteAllText(projectListPath, "");
        }
        string contents = File.ReadAllText(projectListPath);
        contents += froggyFilePath + ';';
        File.WriteAllText(projectListPath, contents);


        // Generate the CMakeLists.txt and run the CMake to generate the project files. ${YOUR_PROJECT_NAME} ${TARGET_FE_GDK_PATH}
        string? cmakeListsTxt;
        string buildDotBatFile = File.ReadAllText( Path.Combine(AppContext.BaseDirectory, Path.Combine("Assets", "build.bat")) );
        File.WriteAllText(Path.Combine(pathToCMakeFolder, "build.bat"), buildDotBatFile);

        File.WriteAllText(Path.Combine(pathToCMakeFolder, ".gitignore"), String.Empty, System.Text.Encoding.UTF8);
        switch (_projectType)
        {
        case ProjectType.cli:
            cmakeListsTxt = File.ReadAllText( Path.Combine(AppContext.BaseDirectory, Path.Combine("Assets", "CMakeListsTemplateForCLI.txt")) );
            cmakeListsTxt = cmakeListsTxt.Replace("${YOUR_PROJECT_NAME}", froggy.ProjectInfo.ProjectName.Trim());
            cmakeListsTxt = cmakeListsTxt.Replace("${TARGET_FE_GDK_PATH}", froggy.EngineInfo.InstallationPath);
            cmakeListsTxt = cmakeListsTxt.Replace("\\", "/");

            File.WriteAllText(Path.Combine(pathToCMakeFolder, "CMakeLists.txt"), cmakeListsTxt);
            File.WriteAllText(Path.Combine(pathToCMakeFolder, "generated.cpp"), String.Empty, System.Text.Encoding.UTF8);
            File.WriteAllText(Path.Combine(pathToIncludeFolder, "app.hpp"), appDotHpp, System.Text.Encoding.UTF8);
            File.WriteAllText(Path.Combine(pathToSourceFolder, "app.cpp"), appDotCpp, System.Text.Encoding.UTF8);
            break;

        case ProjectType.game:
            cmakeListsTxt = File.ReadAllText( Path.Combine(AppContext.BaseDirectory, Path.Combine("Assets", "CMakeListsTemplateForGame.txt")) );
            cmakeListsTxt = cmakeListsTxt.Replace("${YOUR_PROJECT_NAME}", froggy.ProjectInfo.ProjectName.Trim());
            cmakeListsTxt = cmakeListsTxt.Replace("${TARGET_FE_GDK_PATH}", froggy.EngineInfo.InstallationPath);
            cmakeListsTxt = cmakeListsTxt.Replace("\\", "/");

            File.WriteAllText(Path.Combine(pathToCMakeFolder, "CMakeLists.txt"), cmakeListsTxt);
            File.WriteAllText(Path.Combine(pathToCMakeFolder, "generated.cpp"), String.Empty, System.Text.Encoding.UTF8);
            File.WriteAllText(Path.Combine(pathToCMakeFolder, "main.cpp"), mainDotCpp, System.Text.Encoding.UTF8);
            break;

        case ProjectType.dll:
            cmakeListsTxt = File.ReadAllText( Path.Combine(AppContext.BaseDirectory, Path.Combine("Assets", "CMakeListsTemplateForDLL.txt")) );
            cmakeListsTxt = cmakeListsTxt.Replace("${YOUR_PROJECT_NAME}", froggy.ProjectInfo.ProjectName.Trim());
            cmakeListsTxt = cmakeListsTxt.Replace("${TARGET_FE_GDK_PATH}", froggy.EngineInfo.InstallationPath);
            cmakeListsTxt = cmakeListsTxt.Replace("\\", "/");

            File.WriteAllText(Path.Combine(pathToCMakeFolder, "CMakeLists.txt"), cmakeListsTxt);
            File.WriteAllText(Path.Combine(pathToIncludeFolder, "placeholder.hpp"), String.Empty, System.Text.Encoding.UTF8);
            File.WriteAllText(Path.Combine(pathToSourceFolder, "placeholder.cpp"), String.Empty, System.Text.Encoding.UTF8);
            break;

        case ProjectType.lib:
            cmakeListsTxt = File.ReadAllText( Path.Combine(AppContext.BaseDirectory, Path.Combine("Assets", "CMakeListsTemplateForLIB.txt")) );
            cmakeListsTxt = cmakeListsTxt.Replace("${YOUR_PROJECT_NAME}", froggy.ProjectInfo.ProjectName.Trim());
            cmakeListsTxt = cmakeListsTxt.Replace("${TARGET_FE_GDK_PATH}", froggy.EngineInfo.InstallationPath);
            cmakeListsTxt = cmakeListsTxt.Replace("\\", "/");

            File.WriteAllText(Path.Combine(pathToCMakeFolder, "CMakeLists.txt"), cmakeListsTxt);
            File.WriteAllText(Path.Combine(pathToIncludeFolder, "placeholder.hpp"), String.Empty, System.Text.Encoding.UTF8);
            File.WriteAllText(Path.Combine(pathToSourceFolder, "placeholder.cpp"), String.Empty, System.Text.Encoding.UTF8);
            break;
        
        default:
            Debug.Assert(false, "Assertion failed and reached no-default: invalid project type.");
            break;
        }


        _newProjectName.IsReadOnly = false;

        _progressBar.IsEnabled = false;
        _progressBar.Visibility = Visibility.Collapsed;
        _warningText.Visibility = Visibility.Collapsed;
        Frame.Navigate(typeof(MainPage));
#if !DEBUG
        Window.Current.Close();
#endif
    }
}
