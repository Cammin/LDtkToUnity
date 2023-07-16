@echo off

SET root=%~dp0
SET startPath=%root%Samples
SET destPath=%root%LDtkUnity\Samples~

echo %root%
echo StartPath is: %startPath%
echo DestPath is: %destPath%

mklink /d %startPath% %destPath%


pause