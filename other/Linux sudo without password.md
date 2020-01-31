# use sudo without password (your account must be a sudoers)

sudo visudo

## change line where it has your username to
YOUR_USERNAME_HERE ALL=(ALL) NOPASSWD: ALL

## to test out the change to the sudoers file (without inadvertently locking yourself out),
- either have a separate shell/login and test out sudo commands while the editor is still open; or,
- while editing (via sudo visudo), save (but don't exit) the sudoers file (:w in vim), type ctrl-z which suspends the editor, experiment with sudo ls, then return to the editor with fg if everything works as expected.

## NOTE: that line must be on the last line (otherwise it will be overridden by other lines, groups ect)

## atau untuk paling aman pakai saja sudo -i (sama seperti sudo su, tetapi tidak menjadi root, tetapi jadi curent user)
