#!/bin/bash
if [ "$2" == "c" ]; then
  python learn.py Builds/$1/Basic$1.exe --train --run-id=$1 --curriculum=curricula/main.json $3 --worker-id=$(($RANDOM % 1000))
else
  python learn.py Builds/$1/Basic$1.exe --train --run-id=$1 $2 $3 --worker-id=$(($RANDOM % 1000))
fi