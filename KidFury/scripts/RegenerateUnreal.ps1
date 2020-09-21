$ASKFORDELETION = $true;

$scriptPath = Split-Path $script:MyInvocation.MyCommand.Path;

$parentDir = (get-item $scriptPath).parent.FullName

Write-Host "`nGathering directories/files in $parentDir\`n" -ForegroundColor yellow;

$paths = New-Object string[] 6;

$paths[0] = $parentDir + "\.vs\";
$paths[1] = $parentDir + "\Binaries\";
$paths[2] = $parentDir + "\DerivedDataCache\";
$paths[3] = $parentDir + "\Intermediate\";
$paths[4] = $parentDir + "\Saved\";
$paths[5] = $parentDir + "\GP2.sln";

$paths | ForEach-Object -Process { 
	Write-Host -NoNewLine "$_"; 
	if(Test-Path -Path $_) 
	{ 
		Write-Host " --- OK" -ForegroundColor green;
 	} 
	else 
	{ 
		Write-Host " --- Can't Find" -ForegroundColor red; 
	}
}

if($ASKFORDELETION)
{
	Write-Host -NoNewLine "`nDirectory-/filepaths gathered.";
	Write-Host " Press Enter to continue." -ForegroundColor darkgreen;
	$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
	Write-Host "";
}
else 
{
	Write-Host "`nDirectory-/filepaths gathered.";
}

$deletingAllowed = 0;

$iteration = 0;

DO
{
	$deletingAllowed = 1;
	$UE4Process = Get-Process UE4Editor -ErrorAction SilentlyContinue;
	$VSProcess = Get-Process devenv -ErrorAction SilentlyContinue;
	
	$UE4IsClosed = ($UE4Process -eq $null);
	$VSIsClosed = ($VSProcess -eq $null);
	
	$UE4CheckParsed = $(If ($UE4IsClosed) {"OK"} Else {"Failed"});
	$VSCheckParsed = $(If ($VSIsClosed) {"OK"} Else {"Failed"});
	
	$dots = "".PadLeft($iteration%3, '.');

	$UE4Color = $(if($UE4IsClosed) { "green" } else { "red" });
	$VSColor = $(if($VSIsClosed) { "green" } else { "red" });

	Write-Host "`rVS Closed $VSCheckParsed" -NoNewLine -ForegroundColor $VSColor;
	Write-Host "    UE4 Closed $UE4CheckParsed          " -NoNewLine -ForegroundColor $UE4Color;

	if(-not ($UE4IsClosed) -or -not ($VSIsClosed))
	{
		$deletingAllowed = 0;
	}
} While ($deletingAllowed -eq 0)

Write-Host "";

$paths | ForEach-Object -Process {if (Test-Path  $_ -PathType Any) { Remove-Item -path $_ -recurse -force}}

$o = new-object -com Shell.Application;
$folder = $o.NameSpace($parentDir)
$file = $folder.ParseName("GP2.uproject");

$file.Verbs() | ForEach-Object -Process { $item = $_.Name; if($item -eq  "Generate Visual Studio project files") {$_.DoIt();}}


#$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')