root=$(dirname "$0")
startPath="${root}/Samples"
destPath="${root}/LDtkUnity/Samples~"

echo "$root"
echo StartPath is: "$startPath"
echo DestPath is: "$destPath"

ln -s "$destPath" "$startPath"

read -p "Press any key to continue "