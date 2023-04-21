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
