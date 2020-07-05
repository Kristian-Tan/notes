## source: https://dzone.com/articles/executable-package-pip-install
steps:
- register on pypi
- install python package ```setuptools```, ```wheel```, ```twine```, ```tqdm```
- make directory with your package in it
- create executable inside the package
- create ```setup.py``` file, content:
```
import setuptools
with open("README.md", "r") as fh:
    long_description = fh.read()
setuptools.setup(
     name='packagenamehere',
     version='0.1',
     scripts=['packagenamehere'] ,
     author="My Name",
     author_email="myname@gmail.com",
     description="A short description",
     long_description=long_description,
   long_description_content_type="text/markdown",
     url="https://github.com/myusername/packagenamehere",
     packages=setuptools.find_packages(),
     classifiers=[
         "Programming Language :: Python :: 3",
         "License :: OSI Approved :: MIT License",
         "Operating System :: OS Independent",
     ],
 )
```
- note: file in scripts are copied to ```PATH``` in linux (to make it easier to execute your module), it's better to leave it empty
- add ```LICENSE``` file, add ```README.md``` file
- compile with command: ```python3 setup.py bdist_wheel```
- make file ```C:/Users/UserName/.pypirc``` (widows) or ```~/.pypirc (linux)```, content:
```
[distutils]
index-servers=pypi
[pypi]
repository = https://upload.pypi.org/legacy/
username = myusername
```
- upload with command: ```python3 -m twine upload dist/*```
- install on other machine: ```python3 -m pip install packagenamehere```

NOTE:
- when creating new version/new release, edit ```setup.py``` and change upload command (version number) to upload only the new release (to prevent 'already exist' error)
- when making a module (kind of library of classes or methods that can be called by other scripts, ex: like tkinter, PonyORM), use ```__init__.py``` to import your classes (that file will be executed when the module is imported)
- when making an executable (ex: like PumpkinLB), use ```__main__.py``` to make the package executable (put main program there), it can then be called with ```python -m packagenamehere```

FASTER UPLOAD WITH API:
- go to https://pypi.org/manage/account/token/
- create token
- edit ```.pypirc``` file (see above)
```
[distutils]
index-servers=pypi
[pypi]
repository = https://upload.pypi.org/legacy/
username = __token__
password = pypi-Ag*********************************************************************************************************************************************************************
```
