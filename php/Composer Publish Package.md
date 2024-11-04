# Composer Publish Package
ref: https://dev.to/joemoses33/create-a-composer-package-how-to-29kn

### Overview:
- install composer
- create `composer.json` to describe package
- customize autoload namespace
- pick a package name

### Steps:
- install composer (test it with `composer --version` if it has been installed successfully)
- create `composer.json` file to describe the package
    - use interactive generator: `composer init`
    - or create manually by creating and editing file `composer.json`:
- example below assume author name is `kristian-tan` and package name is `my-package`
```json
{
    "name": "kristian-tan/my-package",
    "autoload": {
        "psr-4": {
            "KristianTan\\MyPackage\\": "src/"
        },
        "files": [
            "src/helpers.php"
        ]
    },
    "authors": [
        {
            "name": "Kristian Tanuwijaya",
            "email": "tanuwijayakristian@gmail.com"
        }
    ],
    "require": {}
}
```
- section `autoload.psr-4` contains namespace of the package
    - if the package is intended to be used like this: `\KristianTan\MyPackage::staticMethod1();`
	    - file `composer.json` section `autoload.psr-4` should contains: `    "KristianTan\\": "src/", `
	    - file `src/MyPackage.php` should contains: `<?php namespace KristianTan; class MyPackage { public static function staticMethod1() {...} } `
	    - this pattern is a good fit for a small library/utility with only 1 class, e.g.: jwt parser
    - if the package is intended to be used like this: `\KristianTan\MyPackage\MyClass::staticMethod1();`
	    - file `composer.json` section `autoload.psr-4` should contains: `    "KristianTan\\MyPackage\\": "src/", `
	    - file `src/MyClass.php` should contains: `<?php namespace KristianTan\MyPackage; class MyClass { public static function staticMethod1() {...} } `
	    - this pattern is a good fit for a huge library/framework with many components, e.g.: laravel framework
- section `autoload.files` are always included when `vendor/autoload.php` is loaded ?? (confirm this!)
- when section `autoload` is changed, run `composer dump-autoload` to regenerate autoloader file `vendor/autoload.php`
- pick a package name:
    - package name in packagist.org should be named as such: `vendor-author-name/package-name`, this name will be used to install the package via composer, e.g.: `composer install vendor-author-name/package-name`
    - namespace and class name in PHP should be named as such: `VendorAuthorName\PackageName`, this name will be used to call the library inside PHP, e.g.: `\VendorAuthorName\PackageName::staticMethod1();`
- push the project to git hosting, e.g.: github/bitbucket/gitlab/gitea
    - file `composer.json` MUST be on root directory of the repo
    - source code should be on `src/` directory (optional)
    - a branch MUST be tagged with version number, e.g.: `git tag 'v1.0.0' ; git push origin 'v1.0.0'` (or use github release to tag the repository); this tag will be used for composer to see what version number of the package is used
    - a package without any tag cannot be installed normally (example to install package without tag: `composer require vendor-author-name/package-name:dev-master` assuming the package have a branch called `master`)
- create account on https://packagist.org then click 'submit', then enter git url (the git repository in github/bitbucket/gitlab/gitea should be open/public so packagist.org can clone the repo)

### Example of published packages:
- kristian-tan/http-client
