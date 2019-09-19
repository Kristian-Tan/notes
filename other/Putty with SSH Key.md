# Generating SSH Key: see git tutorial (do it on local machine)
Result: file in ```C:\Users\USERNAME\.ssh``` called id_rsa and id_rsa.pub

# Set remote machine to recognize public key
- Login to remote machine (using ssh)
- Create directory if not exists ~/.ssh
- Copy id_rsa.pub to ```~/.ssh/authorized_keys```

# Generate putty key (optional, do on local machine)
- If this step is not done, then copying (pscp) and putty won't work (can only use git bash utility)
- Open puttygen.exe, click "conversions" - "import key" - select "C:\Users\USERNAME\.ssh\id_rsa"
- Click "save private key", save to "C:\Users\USERNAME\.ssh\id_rsa_putty.ppk"
- When using pscp: pscp -i C:\Users\USERNAME\.ssh\id_rsa_putty.ppk C:/path/to/file usernameremote@hostremote:/path/to/file
- When using putty: in left menu - connections - SSH - Auth - private key for authentication - Browse - select "C:\Users\USERNAME\.ssh\id_rsa_putty.ppk" - save profile

# Using pscp with shortcut
- Create file in pscp.exe directory, give it name "pscpp.bat", with content as follows:
```
@ECHO OFF
setlocal DISABLEDELAYEDEXPANSION
SET BIN_TARGET=%~dp0/pscp.exe
%BIN_TARGET% -i C:\Users\USERNAME\.ssh\id_rsa_putty.ppk %*
```

# Without putty key (on local machine, using git bash utility)
- Using ssh: ssh usernameremote@hostremote
- Using scp: scp ~/path/to/file usernameremote@hostremote:/path/to/file