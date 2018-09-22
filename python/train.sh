#!/bin/bash
date=DATE=`date +%Y-%m-%d`
if [ "$3" == "c" ]; then
  python learn.py Builds/$1/Basic$1.exe --train --run-id=$2 --curriculum=curricula/catapult.json --worker-id=$(($RANDOM % 1000)) $4
elif [ "$3" == "cc" ]; then
   python learn.py Builds/$1/Basic$1.exe --train --run-id=$2 --curriculum=curricula/$4.json --worker-id=$(($RANDOM % 1000)) $5
else
  python learn.py Builds/$1/Basic$1.exe --train --run-id=$2 $3 --worker-id=$(($RANDOM % 1000)) $4
fi