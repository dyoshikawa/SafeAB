.PHONY: format migrate create-migration build deploy

os=osx

format:
	~/.dotnet/tools/fantomas ./safe-ab --recurse

build:
	dotnet publish -c Release -r osx-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
	dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
