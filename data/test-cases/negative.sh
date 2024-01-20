#!/usr/bin/env bash

cp ./data/test-cases/proper_negative_prep.bbtl ./data/input # because the negative.bbtl actually showcases a bug, we are using this as an alternative
./process_once.sh
cat ./data/test-cases/proper_negative_prep.bbtl ./data/test-cases/proper_negative_fail_step.bbtl > ./data/input/proper_negative_with_fail_step.bbtl

expected_prep=$(cat <<EOT
MyWallet 0.00
TheirWallet 1.50
EOT
)
actual_prep=$(cat ./data/output/proper_negative_prep.bbrf | tr -d '\r')
expected_fail_message_containing="InvalidOperationException.*balance 0 < 0\.01"
actual_fail_message=$(./process_once.sh)

[ -e "./data/output/proper_negative_with_fail_step.bbtr" ]
failStepOutputExists=$?

./clean_up.sh

fail_message_contains_expected=$(echo "$actual_fail_message" | grep -qE "$expected_fail_message_containing"; echo $?)
if [[ "$expected_prep" != "$actual_prep" || "$fail_message_contains_expected" -ne 0 || "$failStepOutputExists" -ne 1 ]]; then
    exit 1
fi
