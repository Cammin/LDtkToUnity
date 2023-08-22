@echo off

SET root=%~dp0
SET startPath=%root%Samples
SET destPath=LDtkUnity\Samples~

echo %root%
echo StartPath is: %startPath%
echo DestPath is: %destPath%

mklink /d %startPath% %destPath%