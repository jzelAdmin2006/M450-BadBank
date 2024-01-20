#!/usr/bin/env bash

cp ./data/test-cases/positive.bbtl ./data/input
./process_once.sh

expected="MyWallet 0.25"
actual=$(cat ./data/output/positive.bbrf)

./clean_up.sh

if [[ "$actual" != "$expected" ]]; then
    exit 1
fi
