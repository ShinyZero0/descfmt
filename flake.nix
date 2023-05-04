# STILL WIP. DevShell 100% works, but i'm not sure in the package. It builds but i want to make sure it handles the icu-libs and i need to patch the csproj to false the invariantglobalization cuz whynot. 
{
  description = "A very basic flake";

  inputs = { nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable"; };
  outputs = { self, nixpkgs }:
    let
      system = "x86_64-linux";
      pkgs = nixpkgs.legacyPackages.${system};
    in {
      devShells.x86_64-linux.default = pkgs.mkShell {
        name = "descfmt";
        buildInputs = with pkgs; [ dotnet-sdk_7 clang zlib omnisharp-roslyn ];
      };
      packages.${system}.default = pkgs.stdenv.mkDerivation {
        pname = "descfmt";
				version = "1.1";
        # unpackPhase = ":";
				src = pkgs.fetchFromGitHub {
					owner = "ShinyZero0";
					repo = "descfmt";
					rev = "1.1";
					sha256 = "sha256-VLOZn3CiMaohO8zRtcCE/Q+iEc2UVRY8TVUeSX+L03k=";
				};
				nativeBuildInputs = with pkgs; [ zlib ];
				buildInputs = with pkgs; [ icu ];
        buildPhase = ''
					HOME=$PWD/home
					PATH=${pkgs.dotnet-sdk_7}/bin:$PATH
					DOTNET_ROOT=${pkgs.dotnet-sdk_7}

					mkdir -p $HOME
          dotnet publish descfmt.csproj -o ./out/
        '';

        installPhase = ''
          mkdir -p $out/bin
          cp ./out/descfmt $out/bin/
        '';
      };
    };
}
