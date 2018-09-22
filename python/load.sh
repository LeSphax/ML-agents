#!/bin/bash
python learn.py Builds/$1/Basic$1.exe --load --slow --run-id=$2 $3 --worker-id=$(($RANDOM % 1000))