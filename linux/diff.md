# diff utility

manual: ```man diff``` or https://linux.die.net/man/1/diff

## My commonly used flags:
- ```-q``` only show which file differ
- ```-r``` recursively compare two directory
- ```-w``` ignore white space difference
- ```-Z``` ignore trailing space at line end, e.g.: ```some content    ``` and ```some content```
- ```-b``` ignore amount of space
- ```-B``` ignore amount of blank lines
- ```-i``` ignore casing (upper/lower-case)
- ```--color``` give color (green=added, red=removed), GNU diffutils 3.4 (2016-08-08)

## My common use cases:
- usually programming project like android studio files, or php web project, etc
- because it's programming project, usually I want to:
  - ignore whitespace so that line ending LF and CR-LF is not treated as different,
  - ignore differences of space-indented and tab-indented document,
  - only list file names that are different first, then later show differences per file,
- finding differences between two directory, show only list of files that are different:
```diff -wbBrq myproject1 myproject2```
- see diff per files
```diff -wbBr myproject1 myproject2```
