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
    GNU AFFERO GENERAL PUBLIC LICENSE
    Version 3, 19 November 2007

    Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
    Everyone is permitted to copy and distribute verbatim copies
    of this license document, but changing it is not allowed.

    Copyright © 2026 by UNKNOWN STRYKER (Hojin Lee / Joey)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published
    by the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
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
        string projectListPath = System.IO.Path.Combine(AppContext.BaseDirectory, "froggies.flist");
        string contents = File.ReadAllText(projectListPath);
        if (contents.Contains(_existingProjectPath.Text))
        {
            _errorText.Text = "The project already exists in the project list.";
            return;
        }
        contents += _existingProjectPath.Text + ';';
        File.WriteAllText(projectListPath, contents);
    }


    private string mainDotCpp = "/*\r\nCopyright © from 2024 to present, UNKNOWN STRYKER (Hojin Lee / Joey). All Rights Reserved.\r\n\r\nLicensed under the Frogman Engine License (the \"License\");\r\nyou may not use this file except in compliance with the License.\r\nYou may obtain a copy of the License at\r\n\r\n\thttps://github.com/UnknownStryker-Interactive-Technology/Frogman-Engine-Apache-License/blob/release/LICENSE.md\r\n\r\nUnless required by applicable law or agreed to in writing, software\r\ndistributed under the License is distributed on an \"AS IS\" BASIS,\r\nWITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.\r\nSee the License for the specific language governing permissions and\r\nlimitations under the License.\r\n*/" +
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

        switch (_projectType)
        {
        case ProjectType.cli:
        case ProjectType.dll:
        case ProjectType.lib:
            _errorText.Text = "Sorry, this feature is under development.";
            return;
        }
        

        _newProjectName.IsReadOnly = true;

        _progressBar.IsEnabled = true;
        _progressBar.Visibility = Visibility.Visible;
        _warningText.Visibility = Visibility.Visible;


        string newProjectDir = System.IO.Path.Combine(_newProjectDirectory.Text, _newProjectName.Text);
        if (Directory.Exists(newProjectDir))
        {
            _errorText.Text = "The project name is already taken.";
            _newProjectName.IsReadOnly = false;

            _progressBar.IsEnabled = false;
            _progressBar.Visibility = Visibility.Collapsed;
            _warningText.Visibility = Visibility.Collapsed;
            return;
        }


        string pathToAssetsFolder = System.IO.Path.Combine(newProjectDir, "Assets");
        string pathToCMakeFolder = System.IO.Path.Combine(newProjectDir, "CMake");
        string pathToIncludeFolder = System.IO.Path.Combine(newProjectDir, "Include");
        string pathToSourceFolder = System.IO.Path.Combine(newProjectDir, "Source");

        Directory.CreateDirectory(newProjectDir);
        Directory.CreateDirectory(System.IO.Path.Combine(newProjectDir, "Binaries"));
        Directory.CreateDirectory(pathToAssetsFolder);
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
        string froggyFilePath = System.IO.Path.Combine(newProjectDir, froggyFileName);
        File.WriteAllText(froggyFilePath, jsonFlavoredFroggy);


        string projectListPath = System.IO.Path.Combine(AppContext.BaseDirectory, "froggies.flist");
        if (File.Exists(projectListPath) is not true)
        {
            File.WriteAllText(projectListPath, "");
        }
        string contents = File.ReadAllText(projectListPath);
        contents += froggyFilePath + ';';
        File.WriteAllText(projectListPath, contents);


        string launcherAssetFolderDir = System.IO.Path.Combine(AppContext.BaseDirectory, "Assets");

        // Generate the CMakeLists.txt and run the CMake to generate the project files. ${YOUR_PROJECT_NAME} ${TARGET_FE_GDK_PATH}
        string? cmakeListsTxt;
        string buildDotBatFile = File.ReadAllText( System.IO.Path.Combine(AppContext.BaseDirectory, System.IO.Path.Combine(launcherAssetFolderDir, "build.bat")) );
        File.WriteAllText(System.IO.Path.Combine(pathToCMakeFolder, "build.bat"), buildDotBatFile);

        string buildWithVs2022BatFile = File.ReadAllText(System.IO.Path.Combine(AppContext.BaseDirectory, System.IO.Path.Combine(launcherAssetFolderDir, "build-with-vs2022.bat")));
        File.WriteAllText(System.IO.Path.Combine(pathToCMakeFolder, "build-with-vs2022.bat"), buildWithVs2022BatFile);

        string buildWithVs2026BatFile = File.ReadAllText(System.IO.Path.Combine(AppContext.BaseDirectory, System.IO.Path.Combine(launcherAssetFolderDir, "build-with-vs2026.bat")));
        File.WriteAllText(System.IO.Path.Combine(pathToCMakeFolder, "build-with-vs2026.bat"), buildWithVs2026BatFile);

        File.WriteAllText(System.IO.Path.Combine(pathToCMakeFolder, ".gitignore"), "Solution_X64_AVX/\r\nSolution_X64_AVX512F/", System.Text.Encoding.UTF8);
        switch (_projectType)
        {
        case ProjectType.cli:
            cmakeListsTxt = File.ReadAllText( System.IO.Path.Combine(launcherAssetFolderDir, "CMakeListsTemplateForCLI.txt") );
            cmakeListsTxt = cmakeListsTxt.Replace("${YOUR_PROJECT_NAME}", froggy.ProjectInfo.ProjectName.Trim());
            cmakeListsTxt = cmakeListsTxt.Replace("${TARGET_FE_GDK_PATH}", froggy.EngineInfo.InstallationPath);
            cmakeListsTxt = cmakeListsTxt.Replace("\\", "/");

            File.WriteAllText(System.IO.Path.Combine(pathToCMakeFolder, "CMakeLists.txt"), cmakeListsTxt);
            File.WriteAllText(System.IO.Path.Combine(pathToCMakeFolder, "generated.cpp"), String.Empty, System.Text.Encoding.UTF8);
            File.WriteAllText(System.IO.Path.Combine(pathToIncludeFolder, "app.hpp"), Path.Combine(launcherAssetFolderDir, "cliapp.hpp"), System.Text.Encoding.UTF8);
            File.WriteAllText(System.IO.Path.Combine(pathToSourceFolder, "app.cpp"), Path.Combine(launcherAssetFolderDir, "cliapp.cpp"), System.Text.Encoding.UTF8);
            break;

        case ProjectType.game:
            cmakeListsTxt = File.ReadAllText( System.IO.Path.Combine(launcherAssetFolderDir, "CMakeListsTemplateForGame.txt") );
            cmakeListsTxt = cmakeListsTxt.Replace("${YOUR_PROJECT_NAME}", froggy.ProjectInfo.ProjectName.Trim());
            cmakeListsTxt = cmakeListsTxt.Replace("${TARGET_FE_GDK_PATH}", froggy.EngineInfo.InstallationPath);
            cmakeListsTxt = cmakeListsTxt.Replace("\\", "/");

            File.WriteAllText(System.IO.Path.Combine(pathToCMakeFolder, "CMakeLists.txt"), cmakeListsTxt);
            File.WriteAllText(System.IO.Path.Combine(pathToCMakeFolder, "generated.cpp"), String.Empty, System.Text.Encoding.UTF8);
            File.WriteAllText(System.IO.Path.Combine(pathToCMakeFolder, "main.cpp"), mainDotCpp, System.Text.Encoding.UTF8);

            string iconFolder = System.IO.Path.Combine(pathToAssetsFolder, "Icon");
            Directory.CreateDirectory(iconFolder);
            File.Copy(System.IO.Path.Combine(launcherAssetFolderDir, "runtime icon.png"), System.IO.Path.Combine(iconFolder, "runtime icon.png"));

            string videoFolder = System.IO.Path.Combine(pathToAssetsFolder, "Video");
            Directory.CreateDirectory(videoFolder);
            File.Copy(System.IO.Path.Combine(launcherAssetFolderDir, "runtime splash video 1.mp4"), System.IO.Path.Combine(videoFolder, "runtime splash video 1.mp4"));
            File.Copy(System.IO.Path.Combine(launcherAssetFolderDir, "runtime splash video 2.mp4"), System.IO.Path.Combine(videoFolder, "runtime splash video 2.mp4"));

            Directory.CreateDirectory(System.IO.Path.Combine(pathToAssetsFolder, "Shaders"));

            string splashFolder = System.IO.Path.Combine(pathToAssetsFolder, "Splash");
            Directory.CreateDirectory(splashFolder);
            File.Copy(System.IO.Path.Combine(launcherAssetFolderDir, "Shader Compile Splash.png"), System.IO.Path.Combine(splashFolder, "Shader Compile Splash.png"));
            File.Copy(System.IO.Path.Combine(launcherAssetFolderDir, "Shader Compile Splash 2.png"), System.IO.Path.Combine(splashFolder, "Shader Compile Splash 2.png"));
            File.Copy(System.IO.Path.Combine(launcherAssetFolderDir, "Shader Compile Splash 3.png"), System.IO.Path.Combine(splashFolder, "Shader Compile Splash 3.png"));
            break;

        case ProjectType.dll:
            cmakeListsTxt = File.ReadAllText( System.IO.Path.Combine(launcherAssetFolderDir, "CMakeListsTemplateForDLL.txt") );
            cmakeListsTxt = cmakeListsTxt.Replace("${YOUR_PROJECT_NAME}", froggy.ProjectInfo.ProjectName.Trim());
            cmakeListsTxt = cmakeListsTxt.Replace("${TARGET_FE_GDK_PATH}", froggy.EngineInfo.InstallationPath);
            cmakeListsTxt = cmakeListsTxt.Replace("\\", "/");

            File.WriteAllText(System.IO.Path.Combine(pathToCMakeFolder, "CMakeLists.txt"), cmakeListsTxt);
            File.WriteAllText(System.IO.Path.Combine(pathToIncludeFolder, "placeholder.hpp"), String.Empty, System.Text.Encoding.UTF8);
            File.WriteAllText(System.IO.Path.Combine(pathToSourceFolder, "placeholder.cpp"), String.Empty, System.Text.Encoding.UTF8);
            break;

        case ProjectType.lib:
            cmakeListsTxt = File.ReadAllText( System.IO.Path.Combine(launcherAssetFolderDir, "CMakeListsTemplateForLIB.txt") );
            cmakeListsTxt = cmakeListsTxt.Replace("${YOUR_PROJECT_NAME}", froggy.ProjectInfo.ProjectName.Trim());
            cmakeListsTxt = cmakeListsTxt.Replace("${TARGET_FE_GDK_PATH}", froggy.EngineInfo.InstallationPath);
            cmakeListsTxt = cmakeListsTxt.Replace("\\", "/");

            File.WriteAllText(System.IO.Path.Combine(pathToCMakeFolder, "CMakeLists.txt"), cmakeListsTxt);
            File.WriteAllText(System.IO.Path.Combine(pathToIncludeFolder, "placeholder.hpp"), String.Empty, System.Text.Encoding.UTF8);
            File.WriteAllText(System.IO.Path.Combine(pathToSourceFolder, "placeholder.cpp"), String.Empty, System.Text.Encoding.UTF8);
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
        Window.Current?.Close();
#endif
    }
}
