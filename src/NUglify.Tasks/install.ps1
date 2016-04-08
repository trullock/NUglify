param($installPath, $toolsPath, $package, $project)
	Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' | Out-Null
	$buildProject = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) | Select-Object -First 1

	$existingImports = $buildProject.Xml.Imports | Where-Object { $_.Project -like "*\NUglify.tasks" }
	if ($existingImports) {
		$existingImports | ForEach-Object { $buildProject.Xml.RemoveChild($_) | Out-Null }
	}

	$projectFolder = New-Object System.Uri([System.IO.Path]::GetDirectoryName($project.FullName), [System.UriKind]::Absolute)
	$tasksPath = New-Object System.Uri([System.IO.Path]::Combine($toolsPath, "net40", "NUglify.tasks"), [System.UriKind]::Absolute)
    $relativeTasksPath = $tasksPath.MakeRelativeUri($tasksPath).ToString()

	$buildProject.Xml.AddImport(relativeTasksPath) | Out-Null
	$buildProject.Save()
	$project.Save()
