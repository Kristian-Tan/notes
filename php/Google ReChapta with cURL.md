- Step 1: create reChapta site in https://www.google.com/recaptcha/admin
- Step 2: take note of "Site key" and "Secret key"
- Step 3: create html form to use reChapta,
    add this line inside head element
```html
    <script src='https://www.google.com/recaptcha/api.js'></script>
```
    add this line inside form element
```html
    <div class="g-recaptcha" data-sitekey="{copy paste your site key here}"></div>
```
    make sure your form action point "process.php"
- Step 4: server side integration
    make sure your server's php is set to enable curl library
    inside "process.php", use these to check reChapta result

```php
$google_rechapta_secret = {copy paste your "Secret Key" here};
$google_rechapta_response = $_POST["g-recaptcha-response"];
$google_rechapta_remoteip = $_SERVER["REMOTE_ADDR"];


$uri = 'https://www.google.com/recaptcha/api/siteverify';
$ch = curl_init($uri);
curl_setopt_array($ch, array(
    CURLOPT_HTTPHEADER  => array('Content-Type: application/x-www-form-urlencoded'),
    CURLOPT_RETURNTRANSFER => true,
    CURLOPT_VERBOSE => 1
));
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch, CURLOPT_PORT, 443);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, "secret=".$google_rechapta_secret . "&" . "response=".$google_rechapta_response . "&" . "remoteip=". $google_rechapta_remoteip );
$out = curl_exec($ch);
$response = $out;
curl_close($ch);


$arrJson = json_decode($out, true);
if($arrJson["success"] == true)
{
    echo "user has successfully passed rechapta test";
}
else
{
    echo "user failed to pass rechapta test";
}
```

Example of form.html file:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Try ReChapta</title>
    <script src='https://www.google.com/recaptcha/api.js'></script>
</head>
<body>
    <form action="process.php" method="POST">
        <div class="g-recaptcha" data-sitekey="{copy paste your "Site Key" here}"></div>
        <input type="text" name="input1">
        <input type="submit">
    </form>
</body>
</html>
```