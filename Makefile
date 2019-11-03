.PHONY: format build build-osx build-linux

os=osx

format:
	~/.dotnet/tools/fantomas ./safe-ab --recurse

build: build-osx build-linux

build-osx:
	dotnet publish -c Release -r osx-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
	cd ./SafeAB/bin/Release/netcoreapp3.0/osx-x64/publish && \
	mv SafeAB safe-ab && \
	mv safe-ab safeab && \
	zip ~/Desktop/safeab_osx.zip safeab

build-linux:
	dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
	cd ./SafeAB/bin/Release/netcoreapp3.0/linux-x64/publish && \
	mv SafeAB safe-ab && \
	mv safe-ab safeab && \
	zip ~/Desktop/safeab_linux.zip safeab
