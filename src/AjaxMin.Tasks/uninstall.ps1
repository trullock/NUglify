param($installPath, $toolsPath, $package, $project)
	Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
	$buildProject = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) | Select-Object -First 1

	$existingImports = $buildProject.Xml.Imports | Where-Object { $_.Project -like "*\AjaxMin.tasks" }
	if ($existingImports) {
		$existingImports | ForEach-Object { $buildProject.Xml.RemoveChild($_) | Out-Null }
	}

	$buildProject.Save()
	$project.Save()
