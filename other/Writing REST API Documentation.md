# Writing REST API Documentation
- format: OpenAPI 3
- text editor: VisualStudio Code, with `42crunch.vscode-openapi` https://marketplace.visualstudio.com/items?itemName=42Crunch.vscode-openapi
  - can preview (as you edit) rendered (with swagger) in side-by-side preview (right in the text editor)
  - linting (red underline, when hovered will show error message)
  - autocomplete (intellisense)
  - browse json structure in tree-like browser
  - jump to reference
- displaying to API consumer: swagger-ui

### Installation of editor
- install vscode
- go to https://marketplace.visualstudio.com/items?itemName=42Crunch.vscode-openapi and follow installation instruction
  - usually, `ctrl+p` in vscode, then paste `ext install 42Crunch.vscode-openapi`, then enter
- create new OpenAPI document (see documentation above)
  - usually, `ctrl+shift+p`, type `openapi`, select `create new openapi v3.0 file`
- preview the rendered result (side-by-side): click preview icon

### Installation of UI
- use swagger-ui
  - download from https://github.com/swagger-api/swagger-ui/releases/latest
  - extract, then edit `index.html`, point it to your json file
  - serve it in webserver (ex: in `/var/www/html/` of apache, or `c:/xampp/htdocs` of xampp, or `php -S 0.0.0.0:8080` in directory, or other node.js server)
  - API consumer/user can try the API directly in browser if it's hosted in same server
