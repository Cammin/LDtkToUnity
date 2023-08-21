root=$(dirname "$0")

search_dir=$root
for entry in "$search_dir"/*
do
  echo "$entry"
done

startPath="${root}/Samples"
destPath="${root}/LDtkUnity/Samples~"

echo "$root"
echo StartPath is: "$startPath"
echo DestPath is: "$destPath"

ln -s "$destPath" "$startPath"

read -p "Press any key to continue "