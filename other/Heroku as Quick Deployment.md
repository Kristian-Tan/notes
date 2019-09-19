Quick start (php)
- Make account at heroku.com
- Install heroku CLI program
- Create new app / project via web (like making new gitlab/github project)
- Prepare a local git repository
- Run command in repository directory: heroku git:remote -a slug-here
    Replace slug-here with the project slug created at previous step
- Run git push heroku master
    Replace master with any branch name

Debug (view console logcat):
- command "heroku logs --tail -a slug-here"

Show running process:
- command "heroku ps -a slug-here"

Run bash console or any app:
- command "heroku run bash -a slug-here"

Quick start (python)
- Create text file "requirements.txt", fill it with name of required python modules (ex: django, flask, waitress/gunicorn, etc), separated by line break
- Create text file "Procfile", fill it with command to run HTTP webserver (ex: ```web: waitress-serve --port=$PORT modulename:functionname```)
- Add git remote location and push