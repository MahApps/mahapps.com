[![Stand With Ukraine](https://raw.githubusercontent.com/vshymanskyy/StandWithUkraine/main/banner2-direct.svg)](https://vshymanskyy.github.io/StandWithUkraine)

# mahapps.com

![Statiq](https://github.com/MahApps/mahapps.com/workflows/Statiq/badge.svg)

This is the repository for the new web site of MahApps.Metro v2.0.

You can reach the old site for MahApps.Metro v1.6.5 [here](https://mahapps.github.io/).

## build

The site is generated with [Statiq Docs](https://www.statiq.dev/docs).
There is no CLI tool to install - the generator *is* the
`MahApps.Docs` console project in this repository.

Prerequisite: the [.NET 8 SDK](https://dotnet.microsoft.com/download).

```ps
git submodule update --init --recursive

.\build.ps1                              # preview with live reload
.\build.ps1 --target=Build               # generate the site into ./output
.\build.ps1 --target=Build --api=true    # ... including the API reference
```

With the preview running, open <http://localhost:5080/mahapps.com>.

The `mahapps.com` part of that URL is not a typo: the site is deployed to a
subdirectory, so every generated link is prefixed with the `LinkRoot` setting.
The `Preview` target reads `LinkRoot` from `statiq.json` and serves the site
under a matching virtual directory - otherwise every stylesheet, image and page
would 404 locally. If you drive the generator directly instead of going through
Cake, you have to pass it yourself:

```
dotnet run -- preview --virtual-dir mahapps.com
```

The API reference is skipped by default because it is what makes the build
slow: roughly 2900 extra pages, three minutes instead of twenty seconds. CI
always builds it.

There are two submodules. `mahapps` is the MahApps.Metro source that the API
documentation is generated from - if the API section comes out empty, it was
not initialised. `theme` is the [Docable](https://github.com/statiqdev/Docable)
theme; without it the build has no layout at all.
