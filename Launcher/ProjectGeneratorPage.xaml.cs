using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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


        Debug.Assert(_selectedEngineInfo is not null);
        Debug.Assert(_selectedEngineInfo.InstallationPath is not null);
        Debug.Assert(_selectedEngineInfo.InstallationPath.Length > 0);
        string dllPath = Path.Combine(_selectedEngineInfo.InstallationPath, ScriptMain.GdkLauncherScriptDllPathFromGdkRoot);
        string pdbPath = Path.ChangeExtension(dllPath, ".pdb");

        byte[] dll = File.ReadAllBytes(dllPath);
        byte[] pdb = File.ReadAllBytes(pdbPath);
        Assembly assembly = Assembly.Load(dll, pdb);

        Type[] candidates = assembly.GetTypes()
            .Where(type => type.IsPublic && (type.IsAbstract is false) && type.IsSubclassOf(typeof(ScriptMain)))
            .ToArray();

        if (candidates.Length is not 1)
        {
            _errorText.Text = "Project Generation Failed! Launcher.ScriptMain's methods are not overriden or, more than one definitions of Launcher.ScriptMain's methods coexists.";
            _newProjectName.IsReadOnly = false;

            _progressBar.IsEnabled = false;
            _progressBar.Visibility = Visibility.Collapsed;
            _warningText.Visibility = Visibility.Collapsed;
            return;
        }


        ScriptMain script = (ScriptMain)Activator.CreateInstance(candidates[0])!;

        Debug.Assert(_selectedEngineInfo is not null);
        Froggy froggy = new Froggy
        {
            EngineInfo = _selectedEngineInfo,
            ProjectInfo = new ProjectInfo
            {
                ProjectType = _projectType.ToString(),
                ProjectName = _newProjectName.Text,
                ProjectPath = newProjectDir
            }
        };

        if (File.Exists(ScriptMain.ProjectListPath) is not true)
        {
            File.WriteAllText(ScriptMain.ProjectListPath, "");
        }
        string contents = File.ReadAllText(ScriptMain.ProjectListPath);
        string froggyFileName = froggy.ProjectInfo.ProjectName + ".froggy";
        string froggyFilePath = System.IO.Path.Combine(newProjectDir, froggyFileName);
        contents += froggyFilePath + ';';
        File.WriteAllText(ScriptMain.ProjectListPath, contents);


        switch (_projectType)
        {
        case ProjectType.cli:
            script.CreateCommandLineInterfaceAppProject(_selectedEngineInfo, froggy.ProjectInfo);
            break;

        case ProjectType.game:
            script.CreateGameProject(_selectedEngineInfo, froggy.ProjectInfo);
            break;

        case ProjectType.dll:
            script.CreateDynamicLibraryProject(_selectedEngineInfo, froggy.ProjectInfo);
            break;

        case ProjectType.lib:
            script.CreateStaticLibraryProject(_selectedEngineInfo,  froggy.ProjectInfo);
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
