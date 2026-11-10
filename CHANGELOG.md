# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- `Zone.UNKNOWN` for GME public offers whose zone is not recognized.

### Changed

- **Breaking:** renamed `Market.GMP_ASTA` and `Market.GMP_Cont` to `Market.MGP_ASTA` and `Market.MGP_Cont` to match the GME market names. The ENUMs GMP_ASTA and GMP_Cont where typos, those markets don't exist.

### Security

- Require MessagePack 3.1.12 to pick up the fix for GHSA-qhrr-8q5h-9q3h.

### Fixed

- `UpdateDerivedConfigurationAsync` no longer blocks the calling thread while reading the market data registry.
