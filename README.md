# describefmt
Format the nushell `describe` command in a sane way

## Usage
`<some_command> | describe | descfmt`
Or in nu config file:
```nu
def _describe [] {
	describe | descfmt
}
alias describe = _describe
```
## Building
```bash
nix develop
dotnet publish -o ./out/
```
without nix, ensure you have .NET SDK 7 and zlib-devel
