## Screeps dingus dotnet

My dotnet Screeps repo

This project is using [ScreepsDotNet](https://github.com/thomasfn/ScreepsDotNet)

> Requires .NET 8
> ```
> winget install Microsoft.DotNet.SDK.8
> ```

---

### Building

Run `build.ps1`

```
.\build.ps1 Debug
-- or --
.\build.ps1 Release
```

> Note:
> 
> `build.ps1` copies files to a hard coded directory.

Essentially runs this:

```
dotnet publish -c Debug
-- or --
dotnet publish -c Release
```

> See: [ScreepsDotNet - Build](https://github.com/thomasfn/ScreepsDotNet?tab=readme-ov-file#building)

---
