root=$(dirname "$0")
startPath="${root}/Samples"
destPath="${root}/LDtkUnity/Samples~"



echo "$root"
echo StartPath is: "$startPath"
echo DestPath is: "$destPath"

ln -s "$destPath" "$startPath"


echo "Listing everything!"
echo "----------"
ls -R | grep ":$" | sed -e 's/:$//' -e 's/[^-][^\/]*\//--/g' -e 's/^/   /' -e 's/-/|/'



#read -p "Press any key to continue "