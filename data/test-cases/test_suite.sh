#!/usr/bin/env bash

cd ../../
./data/test-cases/positive.sh && \
./data/test-cases/negative.sh && \

(echo "tests passed") || (echo "tests failed"; exit 1)
