# HeroWarzLauncher
Standalone launcher for HeroWarz

<br />

#### How-To
1\. Make sure you have the game already installed.

**If you don't have the game already installed, you can download and install it from the official website without installing a browser extension: http://koggames.cdn.reloadedtech.com/HeroWarz/WebApp/MCSetup_OneClick.exe*

2\. Download HeroWarzLauncher.exe or get the source and build the project yourself (links down below).

3\. Copy HeroWarzLauncher.exe to the game directory (Default: `C:\KOGGAMES\HeroWarz`)

4\. Launch HeroWarzLauncher.exe to run the game.

<br />

#### Building
Use Visual Studio 2015 (Community Edition is sufficient) to build **HeroWarzLauncher**.

<br />

#### Note about saving username/password
Saving username and/or password is optional.
<br />
If you choose to save username and/or password, the launcher will NOT save them as plaintext.
<br />
It will encrypt them based on your HWID (Hardware ID) using AES with [RFC 2898](https://tools.ietf.org/html/rfc2898).
<br />
With the processor ID (your CPU's ID) as the password and the hard drive serial number being used as the salt.
<br />
Meaning the only way to decrypt the encrypted text is to have access to your PC.
<br />
If someone snags your settings file (HeroWarzLauncher.ini) he wouldn't be able to do much with it without access to your PC or your PC's information.
