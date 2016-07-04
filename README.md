# HeroWarzLauncher
Standalone launcher for HeroWarz

<br />

#### Building
Use Visual Studio 2015 (Community Edition is sufficient) to build **HeroWarzLauncher**.

<br />

#### Note about saving username/password:
Saving username and/or password is optional.
<br />
If you choose to save username and/or password, the launcher will NOT save them as plaintext.
<br />
It will encrypt them based on your HWID (Hardware ID) using AES with [RFC 2898](https://tools.ietf.org/html/rfc2898).
<br />
With the processor ID is being used as the password and the hard drive serial number being used as the salt.
<br />
Meaning the only way to decrypt the encrypted text is to have access to your PC.
<br />
If someone snags your settings file (HeroWarzLauncher.ini) he wouldn't be able to do much with it without access to your PC or your PC's information.
