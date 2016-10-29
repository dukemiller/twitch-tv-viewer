# twitch-tv-viewer
A semi light-weight, retro-looking display to track if twitch streamers are online and start their stream via livestreamer. That is to say, **to watch streams from this program [livestreamer](http://docs.livestreamer.io/) is required**.

**Features**  
\- Track streamers to see if they're online with simple management options (add/delete)  
\- Double click their username to  start their stream in livestreamer interfaced through whatever your default media player is (usually VLC)  
\- Right click them to get options (currently, only open their twitch chat in your browser)  
  
Shortcuts: A (Add), D (Prompt delete selected), E (Edit), Enter (accept menu / start selected stream), Escape (close popup window), R (refresh window)  

---  

### Build & Run
**Requirements:**  [nuget.exe](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe) on PATH, Visual Studio 2015 and/or C# 6.0 Roslyn Compiler  
**Optional:** Devenv (Visual Studio 2015) on PATH  
```
git clone https://github.com/dukemiller/twitch-tv-viewer.git
cd twitch-tv-viewer
nuget install twitch-tv-viewer\packages.config -OutputDirectory packages
```
**Building with Devenv (CLI):** ```devenv twitch-tv-viewer.sln /Build```  
**Building with Visual Studio:**  Open (ctrl-shift-o) "twitch-tv-viewer.sln", Build Solution (ctrl-shift-b)

A "twitch-tv-viewer.exe" artifact will be created in the parent twitch-tv-viewer folder.

---