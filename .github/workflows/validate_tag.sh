#!/usr/bin/env bash

set -euo pipefail

TAG="$1"
TYPE="$2"

case "$TYPE" in
  ga)
	PATTERN='^v[0-9]+\.[0-9]+\.[0-9]+$'
	EXPECTED='vX.Y.Z'
	;;

  beta)
	PATTERN='^v[0-9]+\.[0-9]+\.[0-9]+-beta\.[0-9]+$'
	EXPECTED='vX.Y.Z-beta.N'
	;;

  preview)
	PATTERN='^v[0-9]+\.[0-9]+\.[0-9]+-PR[0-9]+\.[0-9]+$'
	EXPECTED='vX.Y.Z-PR{PR_NUMBER}.{ITERATION}'
	;;

  *)
	echo "::error::Unknown tag type: $TYPE"
	exit 1
	;;
esac

if [[ ! "$TAG" =~ $PATTERN ]]; then
  echo "::error::Invalid $TYPE release tag: $TAG"
  echo "Expected format: $EXPECTED"
  exit 1
fi

echo "Valid $TYPE release tag: $TAG"