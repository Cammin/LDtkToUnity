@echo off

SET root=%~dp0
SET startPath=Samples
SET destPath=LDtkUnity\Samples~

echo StartPath is: %startPath%
echo DestPath is: %destPath%

cd %root%

mklink /d %startPath% %destPath%


pause