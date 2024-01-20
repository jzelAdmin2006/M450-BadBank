#!/usr/bin/env bash

log_file="app_log.txt"
dotnet run --project BadBank.Demo data/input data/output 1 > "$log_file" 2>&1 &
sleep 2
kill $! > /dev/null 2>&1
cat "$log_file"
rm $log_file
