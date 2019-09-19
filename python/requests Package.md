# install python "requests" library / module
```[TERMINAL] pip install requests```

## example code:
```python
from requests import *
r = get("http://localhost/www/latihan/http-printresponse.php",
    headers={"header1":"val1", "header2":"val2"},
    data='{"status":"success"}',
    timeout=0.001, # for specifying timeout, can be set to None to wait forever
    verify='/path/to/cacert.pem' # for SSL certificate verification, can be set to False
)

# for sending json encoded string in post body, use data='{"status":"success"}'
# for sending form data / urlencoded, use data={'key1': 'value1', 'key2': 'value2'}

print(r.text)
# outputs response body (data type = string)

print(r.headers)
# outputs response header (data type = dictionary / associative array)

print(r.headers['Content-Length']) # case sensitive, non RFC compliant
print(r.headers.get('Content-Length') # case insensitive, RFC compliant (recommended)
# outputs a certain response header (data type = string)

print(r.status_code)
# outputs http response status code (ex: 200 means OK)
```