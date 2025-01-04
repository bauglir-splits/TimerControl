#!/bin/bash

if [[ $EUID -ne 0 ]]; then
	echo "Not running as root"
	exit
fi

sudo environment/bin/python timercontrol.py
