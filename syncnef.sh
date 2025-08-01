#! /bin/bash

# Installation in git bash for windows:
# remove the file ending .sh from this file (otherwise the command in the shell would be syncnef.sh ...)
# paste this file in <git intallation path>/usr/bin/

#Syntax:
# ./syncnef PathToJpgFolder PathToNefFolder
# works only for .jpg, .nef and upper case variants of it
# doesn't work for whitespace occurences in Filenames


#Arg 1 in src arg2 in dest füllen
src="$1"
dest="$2"

# look for all .jpg and .nef files
# transform to lower case
# replace '.jpg' or '.nef' in filename with empty Sting
# save in array (äußere runde Klammern stehen für Array)
inhaltSrcOhneJpg=($(ls "$src" | grep -i .jpg | tr "[:upper:]" "[:lower:]" | sed "s/.jpg//g"))
inhaltDestOhneJpg=($(ls "$dest" | grep -i .nef| tr "[:upper:]" "[:lower:]" | sed "s/.nef//g"))

#Verzeichnis für gelöschte Nefs erstellen
mkdir "$dest/deleted"

#src und dest vergleichen und unterschiede herausfinden
for i in "${inhaltDestOhneJpg[@]}"; do
	shouldDelete=0 #0 = true, 1 = false
	for j in "${inhaltSrcOhneJpg[@]}"; do
		#wenn in src gleicher filename dann nicht löschen
		if [ "$i" == "$j" ]; then
			shouldDelete=1
		fi
	done

	#nef löschen, wenn $shouldDelete=0
	if [ $shouldDelete -eq 0 ]; then
		mv "$dest/$i.nef" "$dest/deleted/"
		echo "deleting $i.nef"
	fi
done

exit 0
