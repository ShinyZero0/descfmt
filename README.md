# descfmt

Format the nushell `describe` command in a sane way

# Usage

`<some_command> | describe | descfmt`
Or in nu config file:

```nu
def _describe [] {
    describe | descfmt
}
export alias describe = _describe
```

# Installation

```bash
nix profile install .
```

without nix, ensure you have .NET SDK 7 and zlib-devel or build without AOT and add ~50ms to startup time

and run:

```bash
dotnet publish -o out/ -c Release
```

and grab the executable from `out` dir
