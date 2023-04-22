{
  description = "A very basic flake";

  inputs = { nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable"; };
  outputs = { self, nixpkgs, }:
    let
      system = "x86_64-linux";
      pkgs = nixpkgs.legacyPackages.${system};
    in {
      devShells.x86_64-linux.default = pkgs.mkShell {
        name = "descfmt";
        buildInputs = [ pkgs.dotnet-sdk_7 pkgs.clang pkgs.zlib ];
      };
      packages.system.default = nixpkgs.stdenv.mkDerivation {
        name = "descfmt-1.0";
        unpackPhase = ":";

        buildPhase = ''
          dotnet publish -o ./out/
        '';

        installPhase = ''
          mkdir -p $out/bin
          cp ./out/descfmt $out/bin/
        '';
      };
    };
}
