# GoogleTo1Password

Utility to convert Google Chrome passwords to 1Password passwords.  
Records are filtered to exclude IP addresses and masked usernames.  
Records are modified to only include the domain portion of the URL.

## License

[![GitHub](https://img.shields.io/github/license/ptr727/googleto1password)](./LICENSE)  
Licensed under the [MIT License](./LICENSE)

## Project

![GitHub last commit](https://img.shields.io/github/last-commit/ptr727/googleto1password?logo=github)  
Code is on [GitHub](https://github.com/ptr727/GoogleTo1Password).  
CI is on [Azure DevOps](https://dev.azure.com/pieterv/GoogleTo1Password).

## Build Status

[![Build Status](https://dev.azure.com/pieterv/GoogleTo1Password/_apis/build/status/GoogleTo1Password-Master-CI?branchName=master)](https://dev.azure.com/pieterv/GoogleTo1Password/_build/latest?definitionId=32&branchName=master)
![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/ptr727/googleto1password?logo=github&sort=semver)

## Getting Started

### Installation

- Install the [.NET Core 3.1 Runtime](https://dotnet.microsoft.com/download) and [download](https://github.com/ptr727/GoogleTo1Password/releases/latest) pre-compiled binaries.
- Or compile from [code](https://github.com/ptr727/GoogleTo1Password.git) using [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/download) or the [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download).

### Import and Export

Follow the 1Password [Move your passwords from Chrome to 1Password](https://support.1password.com/import-chrome/) instructions.

### Usage

```console
C:\...\Debug\netcoreapp3.1>GoogleTo1Password.exe --help
GoogleTo1Password:
  Utility to convert Google Chrome passwords to 1Password passwords.

Usage:
  GoogleTo1Password [options]

Options:
  --google <google> (REQUIRED)              Path to Google passwords file.
  --onepassword <onepassword> (REQUIRED)    Path to 1Password passwords file.
  --version                                 Show version information
  -?, -h, --help                            Show help and usage information
```

Example:  
`GoogleTo1Password.exe --google "Google.csv" --onepassword "1Password.csv"`
