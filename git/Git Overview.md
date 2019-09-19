# BROAD STEPS
1. create git account (in github / gitlab)
2. install git client
3. set SSH key
4. initiate a local git project
5. configure local git project to point to remote repository (in github or gitlab)
6. push and commit (sync from local to remote repository)
7. pull (sync from remote to local repository)

# INSTALL GIT CLIENT

# SET SSH KEY
may be helpful:
https://help.github.com/articles/generating-a-new-ssh-key-and-adding-it-to-the-ssh-agent/
https://gist.github.com/bsara/5c4d90db3016814a3d2fe38d314f9c23

- create the following directories: "C:/Users/uname/.ssh/", "C:/Users/uname/.ssh/config/", "C:/Users/uname/.bash_profile/", "C:/Users/uname/bashrc/"
- open git bash
- generate a SSH private-public key pair
```
    $ ssh-keygen -t rsa -b 4096 -C "any label here"
```
- when prompted where to save key, press enter, when prompted to add passphrase, press enter (let it be saved to default location with no passphrase)
- run ssh-agent
```
    $ eval $(ssh-agent -s)
```
- add your SSH private key to ssh-agent
```
    $ ssh-add ~/.ssh/id_rsa
```
- add your SSH public key to your github / gitlab account (via web browser)
```
    $ cat ~/.ssh/id_rsa.pub
    login -> click profile picture (top right) -> settings -> SSH key -> paste content of C:/Users/uname/.ssh/id_rsa.pub to the field on web -> click save
```

# INITIATE A LOCAL GIT PROJECT
- open git bash
- change directory to target directory that you want to create repository in
- initiate / start a new repository
```
    $ git init
```

# CONFIGURE REMOTE REPOSITORY
- to initiate blank project on remote repository (gitlab): login to gitlab -> new project -> input project name -> ok -> select SSH rather than HTTPS -> copy SSH
- open git bash
- to set remote repository for the first time
```
    $ git remote add origin git@gitlab.com:<username>/<projectname>.git
```
- to change remote repository
```
    $ git remote set-url origin git@gitlab.com:<username>/<projectname>.git
```


# COMMIT AND PUSH
- push is sending changes in local repository to remote repository (usually for backup / mobility / accessibility purpose)
- open git bash
- change directory to a directory that is a repository
- add all newly created files inside repository directory into repository
```
    $ git add .
```
- commit the changes
```
    $ git commit -am 'commit message here'
```
- push the latest commit to remote repository
```
    $ git push origin master
```


# BRANCH AND CHECKOUT
- branches are used so that you won't mess up your currently working 'master' branch when making changes (it's a common story that a working program stop working or gets buggy after the developer added a new feature or try to fix another bug)
- to prevent this without git would means that the developer copy the whole project folder somewhere first (make a backup), then do 'experiments' or add feature after making the backup (so that when something went wrong, the developer can just delete the current project folder and just restore their previously working back up)
- in git, the working backup is the 'master' branch, while the experimented project is the 'experiment' branch (the name can be anything)
- may be helpful:
https://www.atlassian.com/git/tutorials/using-branches
https://www.atlassian.com/git/tutorials/using-branches/git-checkout
https://www.atlassian.com/git/tutorials/git-merge

- open git bash
- change directory to a directory that is a repository
- list all branches
```
    $ git branch --list
```
- create a new branch
```
    $ git branch newbranchnamehere
```
- move to another branch
```
    $ git checkout newbranchnamehere
```
    note: do not forget to do "$ git add .", and "$ git commit" before moving to other branch, or you might lose the changes made to that branch


# MERGE AND REBASE
needed when you change the project and want to put it back / merge it into master (example: a new branch 'experiment' is success and new feature successfully implemented, therefore you want to merge it back to master)

## MERGE
- merge two branch
```
    $ git checkout branch_lama_yang_akan_ditumpuki
    $ git merge branch_baru_yang_akan_menumpuki
```
- merge conflict solving:
    jika ada conflict: otomatis diberitahu file mana yang menyebabkan conflict saat menjalankan git merge
    buka file tersebut, lokasi yang menyebabkan konflik diberi tanda oleh git secara otomatis, save file penyebab konflik setelah diedit
    ulangi sampai semua file penyebab konflik habis
```
    $ git commit -am 'merged commit name here'
```
- BAD PRACTICE: jika sudah tidak pakai master lagi (masternya sudah ketinggalan jauh, akhirnya pakai branch tertentu sebagai master), lalu mau meng-set branch lain sebagai master (di-replace)
```
    $ git checkout masterbaru
    $ git merge -s ours master
    $ git checkout master
    $ git merge masterbaru
```

## REBASE
```
    $ git checkout branch_baru_yang_akan_menumpuki
    $ git rebase branch_lama_yang_akan_ditumpuki
```
- rebase conflict solving:
    jika ada conflict: otomatis diberitahu file mana yang menyebabkan conflict saat menjalankan git rebase
    jika ingin melihat detil konflik jalankan:
```
    $ git am --show-current-patch
```
    buka file tersebut, lokasi yang menyebabkan konflik diberi tanda oleh git secara otomatis, save file penyebab konflik setelah diedit
```
    $ git add nama_file_penyebab_conflict_here
```
    ulangi sampai semua file penyebab konflik habis
```
    $ git rebase --continue
```

# SHOW TREE LOG
- show log in tree format:
```
    $ git log --graph --pretty=oneline --abbrev-commit
    $ git log --graph --all --format='%C(cyan dim) %p %Cred %h %C(white dim) %s %Cgreen(%cr)%C(cyan dim) <%an>%C(bold yellow)%d%Creset' --author-date-order
```
- ordered by commit date order (bottommost=oldest,topmost=newest):
```
    add this flag: --author-date-order
```


# RESET / REVERT TO LAST COMMIT
- reset (warning: discaring) all changes and go back to LAST commit
```
    $ git reset --hard
```
- reset (warning: discaring) all changes and go back to PREVIOUS commit
```
    $ git reset --hard HEAD~1
```

## example 1:
- in repository (repository only have one branch, that's 'master') we have file src.txt,
- we have 10 commits, in which initially the file src.txt is empty
- before every commit, one more letter is added into src.txt, starting from A (so after those 10 commit src.txt contains "ABCDEFGHIJ")
- we accidentally added LMNOP to src.txt (we missed letter K),
- but we have not committed and would like to revert back to the last commit (revert back to "ABCDEFGHIJ")
- command: git reset --hard

## example 2:
after reverting, we remember that we are ordered to skip letter H, but we added H in 3 commits before
command: ```git reset --hard HEAD~3```
or
command: ```git reset --hard HEAD~1```
(run 3x)


# PULL
- pull is the reverse of push (it downloads the project from remote repository and merged them to local repository)
- pull is actually a shortcut of 2 process: fetch and merge
```
    $ git pull origin master
```


# CLONE (START / CREATE / INITIATE A PROJECT FROM AN EXISTING REMOTE REPOSITORY)
- open git bash
- change directory to the parent of target directory that you want to create repository in
- clone remote project
```
    $ git clone https://gitlab.com/<username>/<projectname>.git
```



# NOTE: HTTPS VS SSH REPOSITORY
- HTTPS = allow anonymous remote read only access, no credentials / logins needed, good for distributing source code
- SSH = allow authenticated read-write access, need credentials / logins to be able to read or write, good for collaborations